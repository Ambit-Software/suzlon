using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS.Admin
{
    public partial class TableMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);

            if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");
            if (!IsPostBack)
            {
                GetTableDetails();
                grdTable.DataBind();
            }
        }
        private void GetTableDetails()
        {
            string TableResultResult = commonFunctions.RestServiceCall(Constants.TABLE_MASTER_GET, string.Empty);
            List<Models.TableMaster> lstTable = JsonConvert.DeserializeObject<List<Models.TableMaster>>(TableResultResult);
            grdTable.DataSource = lstTable;

        }

        protected void grdTable_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            SaveTableDetails(e, Constants.CONST_NEW_MODE);
        }

        protected void grdTable_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetTableDetails();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdTable_CancelCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            //lblErrorMessage.Visible = false;
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {            
            grdTable.MasterTableView.ClearEditItems();
            grdTable.MasterTableView.IsItemInserted = true;
            grdTable.MasterTableView.Rebind();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //grdTable.ExportSettings.ExportOnlyData = true;
                //grdTable.ExportSettings.IgnorePaging = true;
                //grdTable.ExportSettings.OpenInNewWindow = true;
                //grdTable.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                //grdTable.ExportSettings.FileName = "TableMaster";
                //grdTable.MasterTableView.ExportToExcel();

                grdTable.MasterTableView.AllowFilteringByColumn = false;
                grdTable.MasterTableView.AllowSorting = false;
                grdTable.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                grdTable.ExportSettings.FileName = "TableMaster";
                grdTable.Rebind();
                grdTable.MasterTableView.ExportToExcel();

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private void SaveTableDetails(GridCommandEventArgs e, string editMode)
        {
            try
            {
                int TableId = 0;
                GridEditableItem editableItem = (GridEditableItem)e.Item;

                if (editableItem != null)
                {

                    RadDropDownList drpSite = (RadDropDownList)editableItem.FindControl("drpSite");
                    RadDropDownList drpProject = (RadDropDownList)editableItem.FindControl("drpProject");             
                    RadTextBox txtBlock = (RadTextBox)editableItem.FindControl("txtBlock");
                    RadTextBox txtInvertor = (RadTextBox)editableItem.FindControl("txtInvertor");
                    RadTextBox txtSCB = (RadTextBox)editableItem.FindControl("txtSCB");
                    RadTextBox txtTable = (RadTextBox)editableItem.FindControl("txtTable");


                    int StrBlock = Convert.ToInt32(txtBlock.Text);
                    int StrInvertor = Convert.ToInt32(txtInvertor.Text);
                    int StrSCB = Convert.ToInt32(txtSCB.Text);
                    int StrTable = Convert.ToInt32(txtTable.Text);

                    if (editMode == Constants.CONST_EDIT_MODE)
                        TableId = Convert.ToInt32(editableItem.GetDataKeyValue("tableId"));

                    Models.TableMaster Table = new Models.TableMaster()
                    {
                        TableId = TableId,
                        Site = (Convert.ToString(drpSite.SelectedValue.Trim())),
                        ProjectId = (Convert.ToString(drpProject.SelectedValue.Trim())),
                        Block = StrBlock,
                        Invertor = StrInvertor,
                        SCB = StrSCB,
                        Table = StrTable,
                        ProjectDescription= (Convert.ToString(drpProject.SelectedText.Trim()))
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(Table);
                    string result1 = string.Empty;

                    result1 = commonFunctions.RestServiceCall(Constants.TABLE_MASTER_EXISTS, Crypto.Instance.Encrypt(jsonInputParameter));
                    bool isExist = Convert.ToBoolean(result1);

                    if (isExist)
                    {
                        //lblErrorMessage.Visible = true;
                       // lblErrorMessage.Text = Constants.TABLE_ALREADY_EXISIT;

                        RadNotification1.Title = "Alert";
                        RadNotification1.Show(Constants.TABLE_ALREADY_EXISIT);
                        e.Canceled = true;
                        return;
                    }
                    //else
                    //{
                    //    lblErrorMessage.Visible = false;
                    //}

                    string serviceResult = string.Empty;
                    if (editMode == Constants.CONST_EDIT_MODE)
                        serviceResult = commonFunctions.RestServiceCall(Constants.TABLE_MASTER_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        serviceResult = commonFunctions.RestServiceCall(Constants.TABLE_MASTER_ADD, Crypto.Instance.Encrypt(jsonInputParameter));

                    if (string.Compare(serviceResult, Constants.REST_CALL_FAILURE, true) == 0)
                    {
                        RadNotification1.Title = "Alert";
                        RadNotification1.Show(Constants.ERROR_OCCURED_WHILE_SAVING);

                    }
                    else
                    {
                        RadNotification1.Title = "Sucesss";
                        RadNotification1.Show(Constants.RECORD_SAVE_SUCESSFULLY);
                    }

                }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        protected void grdTable_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            SaveTableDetails(e, Constants.CONST_EDIT_MODE);
        }

        protected void grdTable_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    try
                    {
                        string siteId = string.Empty;
                        string projId = string.Empty;
                        GridDataItem item = e.Item as GridDataItem;
                        Label lblSite = item["Site"].FindControl("lblSite") as Label;
                        siteId = lblSite.Text;

                        Label lblProject = item["Project"].FindControl("lblProject") as Label;
                        projId = lblProject.Text;

                        GridEditableItem editItem = (GridEditableItem)e.Item;
                        RadDropDownList drpSite = (RadDropDownList)editItem.FindControl("drpSite");
                        RadDropDownList drpProj = (RadDropDownList)editItem.FindControl("drpProject");
                        BindDropDown(drpSite, drpProj);
                        drpSite.SelectedValue = siteId.Trim();

                        if (!string.IsNullOrEmpty(siteId.Trim()))
                        { 
                         bindProjDropDown(drpProj, siteId.Trim());
                           drpProj.SelectedValue = projId.Trim();
                           
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonFunctions.WriteErrorLog(ex);
                    }
                }
            }
        }

        private void BindDropDown(RadDropDownList drpSite, RadDropDownList drpProject)
        {
            string drpname = "site,project";
            string result1 = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
            DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result1);

            drpSite.DataTextField = "Name";
            drpSite.DataValueField = "Id";
            drpSite.DataSource = ddValues.site;
            drpSite.DataBind();          
        }

        protected void drpSite_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            RadDropDownList drpsite = (RadDropDownList)sender;
            GridDataItem dataItem = (GridDataItem)drpsite.NamingContainer;
            string strText = Convert.ToString(drpsite.SelectedValue);

            GridEditableItem editedItem = (sender as RadDropDownList).NamingContainer as GridEditableItem;
            if (editedItem != null && !string.IsNullOrEmpty(strText))
            {
                RadDropDownList drpProj = (RadDropDownList)editedItem.FindControl("drpProject");
                bindProjDropDown(drpProj, strText);
                drpProj.SelectedIndex = -1;
            }

        }


        public void bindProjDropDown(RadDropDownList drpProj,string siteId)
        {
            string result1 = commonFunctions.RestServiceCall(string.Format(Constants.TABLE_GET_PROJECTBYSITE, Convert.ToString(siteId)), string.Empty);
            DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result1);
            drpProj.DataTextField = "Name";
            drpProj.DataValueField = "Id";
            drpProj.DataSource = ddValues.project;
            drpProj.DataBind();           
        }
        protected void BtnImport1_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
        {
            if (e.File.GetExtension().ToString().Trim().ToLower() == ".xls" || (e.File.GetExtension().ToString().Trim().ToLower() == ".xlsx"))
            {
                //lblErrorMessage.Visible = false;
                //lblErrorMessage.Text = "";
                try
                {
                    string FilePath = "~/Upload/Table/";
                    int ImportCount;
                    BtnImport1.TargetFolder = FilePath;
                    String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                    string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
                    e.File.SaveAs(Path.Combine(Server.MapPath(BtnImport1.TargetFolder), newfilename));
                    string FilePath1 = "~/Upload/Table/" + newfilename;
                    DataTable ExcelDT = commonFunctions.GetFileData(Server.MapPath(FilePath1), "");

                    if (ExcelDT.Rows.Count > 0)
                    {

                        ExcelDT = ExcelDT.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();

                        ImportCount = Convert.ToInt32(ExcelDT.Rows.Count);
                        ExcelDT.Columns.Add("Id", typeof(Int32)).SetOrdinal(0);
                        ExcelDT.Columns.Add("ProjectDescription", typeof(string)).SetOrdinal(3);
                        ExcelDT.Columns.Add("Status", typeof(Boolean)).SetOrdinal(8);
                        ExcelDT.Columns.Add("CreatedBy", typeof(int)).SetOrdinal(9);
                        ExcelDT.Columns.Add("CreatedOn", typeof(DateTime)).SetOrdinal(10);
                        ExcelDT.Columns.Add("ModifiedBy", typeof(int)).SetOrdinal(11);
                        ExcelDT.Columns.Add("ModifiedOn", typeof(DateTime)).SetOrdinal(12);
                        ExcelDT.Columns.Add("IsValidated", typeof(Boolean)).SetOrdinal(13);
                        ExcelDT.Columns.Add("IsMerged", typeof(Boolean)).SetOrdinal(14);

                        string currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); // HH 24 hrs format, hh 12 hours format, yyyy/MM/dd HH:mm:ss.fff for milisecond
                        foreach (DataRow dr in ExcelDT.Rows)
                        {
                            dr["Status"] = 1;
                            dr["CreatedBy"] = Convert.ToString(Session["UserId"]);
                            dr["CreatedOn"] = currentTime;
                            dr["ModifiedBy"] = Convert.ToString(Session["UserId"]);
                            dr["ModifiedOn"] = currentTime;
                            dr["ProjectDescription"] = "Test";
                        }
                        try
                        {
                            SqlBulkCopy sqlCopy = new SqlBulkCopy(commonFunctions.GetDBConnectionString());
                            sqlCopy.DestinationTableName = "[dbo].[TableMasterStaging]";
                            sqlCopy.WriteToServer(ExcelDT);

                            commonFunctions.UploadData(Convert.ToString(Session["UserId"]), currentTime + ":000", "TableMasterStaging");

                            StringBuilder StrQuery = new StringBuilder("");
                            StrQuery.Append("select Site,ProjectId,block,Invertor,SCB,[Table] from  TableMasterStaging where createdby = '" + Convert.ToString(Session["UserId"]) + "' ");
                            StrQuery.Append("and createdon = '" + Convert.ToString(currentTime + ":000") + "'and isValidated is null and isMerged is null");

                            DataTable validateDT = commonFunctions.GetDataTable(StrQuery.ToString());

                            string NotUploadRec = string.Empty;
                            foreach (DataRow dr in validateDT.Rows)
                            {
                                NotUploadRec = NotUploadRec + Convert.ToString(dr["site"]) + "-" + Convert.ToString(dr["ProjectId"]) + "-" + Convert.ToString(dr["block"]) + "-" + Convert.ToString(dr["Invertor"]) + "-" + Convert.ToString(dr["SCB"]) + "-" + Convert.ToString(dr["Table"]) + ", ";
                            }

                            int UploadCount = ImportCount - (Convert.ToInt32(validateDT.Rows.Count));

                            GetTableDetails();
                            grdTable.DataBind();


                            string Errmsg = Constants.TOTAL_RECORD_MESSAGE + "<B>" + ImportCount + "</B> " + "</br> ";
                            Errmsg = Errmsg + Constants.UPLOAD_RECORD_MESSAGE + "<B>" + UploadCount + "</B> " + "</br> ";
                            Errmsg = Errmsg + Constants.NOTUPLOAD_RECORD_MESSAGE + "<B> " + Convert.ToInt32(validateDT.Rows.Count) + "</B> " + "";
                            if (NotUploadRec.Length > 1)
                            {
                                Errmsg = Errmsg + "</br>Not Uploaded Records:- " + NotUploadRec.Substring(0, NotUploadRec.Length - 1);
                            }
                            RadNotification1.Title = "Import Table Master";
                            RadNotification1.Show(Errmsg);
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                            RadNotification1.Title = "Import Table Master";
                            RadNotification1.Show(ex.Message.Replace("'", "\""));
                        }
                    }
                    else
                    {
                        RadNotification1.Title = "Import Table Master";
                        RadNotification1.Show(Constants.BLANK_EXCELSHEET_MSG);
                    }
                }

                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                    RadNotification1.Title = "Import Table Master";
                    RadNotification1.Show(Constants.EXCELSHEETERRORMSG);
                }
            }
            else
            {
                //lblErrorMessage.Visible = true;
                //lblErrorMessage.Text = Constants.FILEExTENSION;
                RadNotification1.Title = "Import Table Master";
                RadNotification1.Show(Constants.FILEExTENSION);
            }
        }

        protected void grdTable_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (Convert.ToString(e.CommandName))
            {
                case "InitInsert":
                    grdTable.MasterTableView.ClearEditItems();
                    break;
                case "Edit":
                    e.Item.OwnerTableView.IsItemInserted = false;
                    break;
                case "Filter":
                    grdTable.MasterTableView.ClearEditItems();
                    break;
            }
        }
    }
}