using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS.Admin
{
    public partial class LocationMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);

            if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");
        }
        /// <summary>
        /// Set datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LocationGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                gridLocation.DataSource = GetLocations();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Insert new record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridLocation_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveLocation(e, Constants.CONST_NEW_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridLocation_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveLocation(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Hide error message on cancel click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridLocation_CancelCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                lblErrorMessage.Visible = false;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Show either edit or add mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridLocation_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.CommandName))
                {
                    case "InitInsert":
                        gridLocation.MasterTableView.ClearEditItems();
                        break;
                    case "Edit":
                        e.Item.OwnerTableView.IsItemInserted = false;
                        break;
                    case "Filter":
                        gridLocation.MasterTableView.ClearEditItems();
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        #endregion

        #region "Private Methods"
        /// <summary>
        /// Get locations
        /// </summary>
        /// <returns></returns>
        private List<Models.LocationMaster> GetLocations()
        {
            string locationListResult = commonFunctions.RestServiceCall(Constants.LOCATION_GET, string.Empty);
            List<Models.LocationMaster> lstLocation = JsonConvert.DeserializeObject<List<Models.LocationMaster>>(locationListResult);
            return lstLocation;
        }

        /// <summary>
        /// Add/Update location details.
        /// </summary>
        /// <param name="e"></param>
        private void SaveLocation(GridCommandEventArgs e, string editMode)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            TextBox txtLocationName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("LocationName")).TextBoxControl;
            int locationId = 0;

            if (editMode == Constants.CONST_EDIT_MODE)
                locationId = Convert.ToInt32(editableItem.GetDataKeyValue("LocationId"));

            // check record exist with same name.
            string isExist = commonFunctions.RestServiceCall(Constants.LOCATION_EXISTS + "?name=" + txtLocationName.Text + "&&locationId=" + locationId, string.Empty);

            if (isExist == "true")
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Location" + Constants.CONST_ALREADY_EXISTS;
                e.Canceled = true;
                return;
            }
            else
            {
                lblErrorMessage.Visible = false;
                string locationDescription = (editableItem.FindControl("txtDiscription") as TextBox).Text;
                // bool status = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                bool status = (editableItem.FindControl("chkStatus") as CheckBox).Checked;

                Models.LocationMaster location = new Models.LocationMaster()
                {
                    LocationId = locationId,
                    LocationName = txtLocationName.Text,
                    Description = locationDescription,
                    Status = status
                };

                string parameter = JsonConvert.SerializeObject(location);
                // Save location API call here.
                commonFunctions.RestServiceCall(editMode == Constants.CONST_EDIT_MODE ? Constants.LOCATION_EDIT : Constants.LOCATION_ADD, Crypto.Instance.Encrypt(parameter));
            }
        }
        #endregion
        protected void btnImport_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            string fileExtension = e.File.GetExtension().Trim().ToLower();

            if (fileExtension == ".xls" || (fileExtension == ".xlsx"))
            {
                lblErrorMessage.Visible = false;
                lblErrorMessage.Text = "";
                try
                {
                    string FilePath = "~/Upload/Location/";
                    int ImportCount;

                    btnImport.TargetFolder = FilePath;

                    string timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");

                    string newfilename = e.File.GetNameWithoutExtension() + timeStamp + fileExtension;
                    btnImport.TargetFolder = FilePath;
                    e.File.SaveAs(Path.Combine(Server.MapPath(btnImport.TargetFolder), newfilename));
                    string FilePath1 = "~/Upload/Location/" + newfilename;

                    DataTable ExcelDT = commonFunctions.GetFileData(Server.MapPath(FilePath1), "");
                    if (ExcelDT != null && ExcelDT.Rows.Count > 0)
                    {

                     ExcelDT = ExcelDT.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                    ImportCount = Convert.ToInt32(ExcelDT.Rows.Count);

                    ExcelDT.Columns.Add("Id", typeof(Int32)).SetOrdinal(0);
                    ExcelDT.Columns.Add("Status", typeof(Boolean)).SetOrdinal(3);
                    ExcelDT.Columns.Add("CreatedBy", typeof(int)).SetOrdinal(4);
                    ExcelDT.Columns.Add("CreatedOn", typeof(DateTime)).SetOrdinal(5);
                    ExcelDT.Columns.Add("ModifiedBy", typeof(int)).SetOrdinal(6);
                    ExcelDT.Columns.Add("ModifiedOn", typeof(DateTime)).SetOrdinal(7);
                    ExcelDT.Columns.Add("IsVald", typeof(DateTime)).SetOrdinal(8);
                    string currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); // HH 24 hrs format, hh 12 hours format, yyyy/MM/dd HH:mm:ss.fff for milisecond

                    int userId = Convert.ToInt32(Session["UserId"]);
                    foreach (DataRow dr in ExcelDT.Rows)
                    {
                        dr["Status"] = 1;
                        dr["CreatedBy"] = userId;
                        dr["CreatedOn"] = currentTime;
                        dr["ModifiedBy"] = userId;
                        dr["ModifiedOn"] = currentTime;
                    }
                    try
                    {
                        SqlBulkCopy sqlCopy = new SqlBulkCopy(commonFunctions.GetDBConnectionString());
                        sqlCopy.DestinationTableName = "[dbo].[LocatioMasterStaging]";
                        sqlCopy.WriteToServer(ExcelDT);

                        commonFunctions.UploadData(Convert.ToString(userId), currentTime + ":000", "LocatioMasterStaging");

                        StringBuilder StrQuery = new StringBuilder("");
                        StrQuery.Append("select LocationName,Description from[dbo].LocatioMasterStaging where createdby = '" + userId + "' ");
                        StrQuery.Append("and createdon = '" + Convert.ToString(currentTime + ":000") + "'and isValidated is null and isMerged is null");

                        DataTable validateDT = commonFunctions.GetDataTable(StrQuery.ToString());

                        string NotUploadRec = string.Empty;
                        foreach (DataRow dr in validateDT.Rows)
                        {
                            NotUploadRec = NotUploadRec + Convert.ToString(dr["LocationName"]) + ", ";
                        }

                        int UploadCount = ImportCount - (Convert.ToInt32(validateDT.Rows.Count));

                        gridLocation.DataSource = GetLocations();
                        gridLocation.DataBind();


                        string Errmsg = Constants.TOTAL_RECORD_MESSAGE + "<B>" + ImportCount + "</B> " + "</br> ";
                        Errmsg = Errmsg + Constants.UPLOAD_RECORD_MESSAGE + "<B>" + UploadCount + "</B> " + "</br> ";
                        Errmsg = Errmsg + Constants.NOTUPLOAD_RECORD_MESSAGE + "<B> " + Convert.ToInt32(validateDT.Rows.Count) + "</B> " + "";
                        if (NotUploadRec.Length > 1)
                        {
                            Errmsg = Errmsg + "</br>Not Uploaded Location Names:- " + NotUploadRec.Substring(0, NotUploadRec.Length - 1);
                        }
                        RadNotification1.Title = "Import Location Master";
                        RadNotification1.Show(Errmsg);


                    }
                    catch (Exception ex)
                    {
                        CommonFunctions.WriteErrorLog(ex);
                        RadNotification1.Title = "Import Location Master";
                        RadNotification1.Show(ex.Message.Replace("'", "\""));
                    }
                    }
                    else
                    {
                        RadNotification1.Title = "Import Location Master";
                        RadNotification1.Show(Constants.BLANK_EXCELSHEET_MSG);
                    }

                }

                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                    RadNotification1.Title = "Import Location Master";
                    RadNotification1.Show(Constants.EXCELSHEETERRORMSG);
                }
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = Constants.FILEExTENSION;
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //gridLocation.ExportSettings.ExportOnlyData = true;
                //gridLocation.ExportSettings.IgnorePaging = true;
                //gridLocation.ExportSettings.OpenInNewWindow = true;
                //gridLocation.ExportSettings.FileName = "LocationMaster";
                //gridLocation.MasterTableView.ExportToExcel();
                gridLocation.MasterTableView.AllowFilteringByColumn = false;
                gridLocation.MasterTableView.AllowSorting = false;
                gridLocation.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridLocation.ExportSettings.FileName = "LocationMaster";
                gridLocation.Rebind();
                gridLocation.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            // GridCommandItem commandItem = (GridCommandItem)gridSurvey.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            //commandItem.FireCommandEvent("InitInsert", null);
            gridLocation.MasterTableView.ClearEditItems();
            gridLocation.MasterTableView.IsItemInserted = true;
            gridLocation.MasterTableView.Rebind();
        }

        protected void gridLocation_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                if (e.Item is GridDataInsertItem)
                {
                    CheckBox chkStatus = (CheckBox)dataItem.FindControl("chkStatus");                   
                    chkStatus.Checked = true;
                }
                if (e.Item.IsInEditMode)
                {
                    //CheckBox chkStatus = (CheckBox)dataItem.FindControl("chkStatus");
                    //chkStatus.Attributes.Add("onclick", "oncheckedChaned(this);");
                }
                else
                {
                    Label lblStatus = (Label)dataItem.FindControl("lblStatus");
                    string text = lblStatus.Text;
                    lblStatus.Text = text == "True" ? "Enabled" : "Disabled";
                }

            }
        }
    }
}