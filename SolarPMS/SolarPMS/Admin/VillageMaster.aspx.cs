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
    public partial class VillageMaster : System.Web.UI.Page
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
                gridVillages.DataSource = GetVillages();
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Insert new record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridVillages_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveVillageDetails(e, Constants.CONST_NEW_MODE);
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridVillages_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveVillageDetails(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Hide edit column while exporting data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridVillages_ExcelMLWorkBookCreated(object sender, Telerik.Web.UI.GridExcelBuilder.GridExcelMLWorkBookCreatedEventArgs e)
        {
            try
            {
                gridVillages.Columns[2].Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Hide error message on cancel click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridVillages_CancelCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                lblErrorMessage.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Show either add or edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridVillages_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.CommandName))
                {
                    case "InitInsert":
                        gridVillages.MasterTableView.ClearEditItems();
                        break;
                    case "Edit":
                        e.Item.OwnerTableView.IsItemInserted = false;
                        break;
                    case "Filter":
                        gridVillages.MasterTableView.ClearEditItems();
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnImport_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
        {
            string fileExtension = e.File.GetExtension().ToString().Trim().ToLower();
            if (fileExtension == ".xls" || (fileExtension == ".xlsx"))
            {
                lblErrorMessage.Visible = false;
                lblErrorMessage.Text = "";

                try
                {
                    string FilePath = "~/Upload/Village/";
                    int ImportCount;
                    btnImport.TargetFolder = FilePath;

                    string timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                    string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
                    btnImport.TargetFolder = FilePath;
                    e.File.SaveAs(Path.Combine(Server.MapPath(btnImport.TargetFolder), newfilename));
                    string FilePath1 = "~/Upload/Village/" + newfilename;
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
                        sqlCopy.DestinationTableName = "[dbo].[VillageMasterStaging]";
                        sqlCopy.WriteToServer(ExcelDT);

                        commonFunctions.UploadData(Convert.ToString(userId), currentTime + ":000", "VillageMasterStaging");

                        StringBuilder StrQuery = new StringBuilder("");
                        StrQuery.Append("select VillageName,Description from[dbo].VillageMasterStaging where createdby = '" + userId + "' ");
                        StrQuery.Append("and createdon = '" + Convert.ToString(currentTime + ":000") + "'and isValidated is null and isMerged is null");

                        DataTable validateDT = commonFunctions.GetDataTable(StrQuery.ToString());

                        string NotUploadRec = string.Empty;
                        foreach (DataRow dr in validateDT.Rows)
                        {
                            NotUploadRec = NotUploadRec + Convert.ToString(dr["VillageName"]) + ", ";
                        }

                        int UploadCount = ImportCount - (Convert.ToInt32(validateDT.Rows.Count));

                        gridVillages.DataSource = GetVillages();
                        gridVillages.DataBind();

                        string Errmsg = Constants.TOTAL_RECORD_MESSAGE + "<B>" + ImportCount + "</B> " + "</br> ";
                        Errmsg = Errmsg + Constants.UPLOAD_RECORD_MESSAGE + "<B>" + UploadCount + "</B> " + "</br> ";
                        Errmsg = Errmsg + Constants.NOTUPLOAD_RECORD_MESSAGE + "<B> " + Convert.ToInt32(validateDT.Rows.Count) + "</B> " + "";
                        if (NotUploadRec.Length > 1)
                        {
                            Errmsg = Errmsg + "</br>Not Uploaded Village Names :- " + NotUploadRec.Substring(0, NotUploadRec.Length - 1);
                        }
                        RadNotification1.Title = "Import Village Master";
                        RadNotification1.Show(Errmsg);

                    }
                    catch (Exception ex)
                    {
                        Utility.WriteErrorLog(ex.Message, ex.StackTrace);
                        RadNotification1.Title = "Import Village Master";
                        RadNotification1.Show(ex.Message.Replace("'", "\""));
                    }

                    }
                    else
                    {
                        RadNotification1.Title = "Import Village Master";
                        RadNotification1.Show(Constants.BLANK_EXCELSHEET_MSG);
                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteErrorLog(ex.Message, ex.StackTrace);
                    RadNotification1.Title = "Import Village Master";
                    RadNotification1.Show(Constants.EXCELSHEETERRORMSG);
                }
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = Constants.FILEExTENSION;
            }
        }

        #endregion

        #region "Private Methods"
        /// <summary>
        /// Get village list.
        /// </summary>
        /// <returns></returns>
        private List<Models.VillageMaster> GetVillages()
        {
            string villageListResult1 = commonFunctions.RestServiceCall(Constants.VILLAGE_GET, string.Empty);
            List<Models.VillageMaster> lstVillage = JsonConvert.DeserializeObject<List<Models.VillageMaster>>(villageListResult1);
            return lstVillage;
        }

        /// <summary>
        /// Add/Update location details.
        /// </summary>
        /// <param name="e"></param>
        private void SaveVillageDetails(GridCommandEventArgs e, string editMode)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            string txtVillageName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("VillageName")).TextBoxControl.Text;
            int locationId = 0;

            if (editMode == Constants.CONST_EDIT_MODE)
                locationId = Convert.ToInt32(editableItem.GetDataKeyValue("VillageId"));

            // check record exist with same name.
            string isExist = commonFunctions.RestServiceCall(Constants.VILLAGE_EXISTS + "?name=" + txtVillageName + "&&id=" + locationId, string.Empty);

            if (isExist == "true")
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Village" + Constants.CONST_ALREADY_EXISTS;
                e.Canceled = true;
                return;
            }
            else
            {
                lblErrorMessage.Visible = false;
                //bool status = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                bool status = (editableItem.FindControl("chkStatus") as CheckBox).Checked;
                Models.VillageMaster village = new Models.VillageMaster()
                {
                    VillageId = locationId,
                    VillageName = txtVillageName,
                    Status = status
                };

                string parameter = JsonConvert.SerializeObject(village);
                // Save location API call here.
                commonFunctions.RestServiceCall((editMode == Constants.CONST_EDIT_MODE ? Constants.VILLAGE_EDIT : Constants.VILLAGE_ADD), Crypto.Instance.Encrypt(parameter));
            }
        }

        #endregion

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //gridVillages.ExportSettings.ExportOnlyData = true;
                //gridVillages.ExportSettings.IgnorePaging = true;
                //gridVillages.ExportSettings.OpenInNewWindow = true;
                //gridVillages.ExportSettings.FileName = "VillageMaster";
                //gridVillages.MasterTableView.ExportToExcel();

                gridVillages.MasterTableView.AllowFilteringByColumn = false;
                gridVillages.MasterTableView.AllowSorting = false;
                gridVillages.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridVillages.ExportSettings.FileName = "VillageMaster";
                gridVillages.Rebind();
                gridVillages.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
            }
        }


        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            // GridCommandItem commandItem = (GridCommandItem)gridSurvey.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            //commandItem.FireCommandEvent("InitInsert", null);
            gridVillages.MasterTableView.ClearEditItems();
            gridVillages.MasterTableView.IsItemInserted = true;
            gridVillages.MasterTableView.Rebind();
        }

        protected void gridVillages_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (e.Item is GridDataInsertItem)
                    {
                        CheckBox chkStatus = (CheckBox)dataItem.FindControl("chkStatus");
                        chkStatus.Attributes.Add("onclick", "oncheckedChaned(this);");
                        chkStatus.Checked = true;
                    }

                    if (e.Item.IsInEditMode)
                    {
                        CheckBox chkStatus = (CheckBox)dataItem.FindControl("chkStatus");
                        chkStatus.Attributes.Add("onclick", "oncheckedChaned(this);");
                    }
                    else
                    {
                        Label lblStatus = (Label)dataItem.FindControl("lblStatus");
                        string text = lblStatus.Text;
                        lblStatus.Text = text == "True" ? "Enabled" : "Disabled";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}