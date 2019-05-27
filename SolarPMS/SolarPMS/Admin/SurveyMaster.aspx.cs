using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Linq;
using System.Collections;

namespace SolarPMS.Admin
{
    public partial class SurveyMaster : Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {
                GetSurveyDetails();
                gridSurvey.DataBind();
            }
        }

        protected void SurveyGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {                
                GetSurveyDetails();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void GetSurveyDetails()
        {
            string SurveyResultResult = commonFunctions.RestServiceCall(Constants.SURVAY_MASTER_GET, string.Empty);
            if (!string.IsNullOrEmpty(SurveyResultResult))
            {
                List<SurveyMasterModel> lstSurvay = JsonConvert.DeserializeObject<List<SurveyMasterModel>>(SurveyResultResult);
                gridSurvey.DataSource = lstSurvay;
            }
            else
            {
                gridSurvey.DataSource = new DataTable();
            }
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
        public void bindProjDropDown(RadDropDownList drpProj, string siteId)
        {
            string result = commonFunctions.RestServiceCall(string.Format(Constants.TABLE_GET_PROJECTBYSITE, Convert.ToString(siteId)), string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                drpProj.DataTextField = "Name";
                drpProj.DataValueField = "Id";
                drpProj.DataSource = ddValues.project;
                drpProj.DataBind();
            }
        }
        private void SaveSurveyDetails(GridCommandEventArgs e, string editMode)
        {
            try
            {
                 int SurveyId = 0;
                GridEditableItem editableItem = (GridEditableItem)e.Item;

                if (editableItem != null)
                {

                    RadDropDownList drpSite = (RadDropDownList)editableItem.FindControl("drpSite");
                    RadDropDownList drpVillage = (RadDropDownList)editableItem.FindControl("drpVillage");
                   //  RadNumericTextBox txtPropsedTotal = (RadNumericTextBox)editableItem.FindControl("txtPropsedTotal");
                    RadNumericTextBox txtArea = (RadNumericTextBox)editableItem.FindControl("txtArea");

                    RadTextBox txtSurveyNo = (RadTextBox)editableItem.FindControl("txtSurveyNo");                  
                    RadNumericTextBox txtNoOfDivision = (RadNumericTextBox)editableItem.FindControl("txtNoOfDivision");
                    RadDropDownList drpProject = (RadDropDownList)editableItem.FindControl("drpProject");


                    string Area = Convert.ToString(txtArea.Text);
                    string SurvyNo = Convert.ToString(txtSurveyNo.Text);
                    string NoOfDiv = Convert.ToString(txtNoOfDivision.Text);
                    bool status = true; //(editableItem["chkStatus"].Controls[0] as CheckBox).Checked;



                    if (editMode == Constants.CONST_EDIT_MODE)
                        SurveyId = Convert.ToInt32(editableItem.GetDataKeyValue("surveyId"));

                    Models.SurveyMaster survey = new Models.SurveyMaster()
                    {
                        SurveyId = SurveyId,
                        Site = Convert.ToString(drpSite.SelectedValue.Trim()),
                        VillageId = (Convert.ToInt32(drpVillage.SelectedValue.Trim())),
                        SurveyNo = SurvyNo,
                        //PropsedTotal = Convert.ToInt32(PropsedTotal),
                        Area=Convert.ToDecimal(Area),
                        NoOfDivision = Convert.ToInt32(NoOfDiv),  
                        ProjectId=Convert.ToString(drpProject.SelectedValue.Trim()),                 
                        Status= status
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(survey);
                    string result1 = string.Empty;

                    result1 = commonFunctions.RestServiceCall(string.Format(Constants.SURVAY_MASTER_EXISTS, Convert.ToString(txtSurveyNo.Text), Convert.ToInt32(drpVillage.SelectedValue.Trim()), SurveyId, Convert.ToString(drpSite.SelectedValue.Trim()),Convert.ToString(drpProject.SelectedValue.Trim())), string.Empty);
                    bool isExist = Convert.ToBoolean(result1);

                    if (isExist)
                    {                        
                        RadNotification1.Title = "Alert";
                        RadNotification1.Show(Constants.SURVAYNO_EXIST_MSG);
                        e.Canceled = true;
                        return;
                    }                  

                    string serviceResult = string.Empty;
                    if (editMode == Constants.CONST_EDIT_MODE)
                        serviceResult = commonFunctions.RestServiceCall(Constants.SURVAY_MASTER_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        serviceResult = commonFunctions.RestServiceCall(Constants.SURVAY_MASTER_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                                     
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

        protected void gridSurvey_InsertCommand(object sender, GridCommandEventArgs e)
        {
            SaveSurveyDetails(e, Constants.CONST_NEW_MODE);
        }

        protected void gridSurvey_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            SaveSurveyDetails(e, Constants.CONST_EDIT_MODE);
        }

        protected void gridSurvey_ItemDataBound(object sender, GridItemEventArgs e)
        {

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                try
                {
                    string SiteId = string.Empty;
                    string VillageId = string.Empty;
                    string projId = string.Empty;
                    GridDataItem item = e.Item as GridDataItem;

                    GridEditableItem editItem = (GridEditableItem)e.Item;
                    RadDropDownList drpSite = (RadDropDownList)editItem.FindControl("drpSite");
                    RadDropDownList drpVillage = (RadDropDownList)editItem.FindControl("drpVillage");

                    Label lblVillage = item["Village"].FindControl("lblVillage") as Label;
                    VillageId = lblVillage.Text;
                    Label lblSite = item["Site"].FindControl("lblSite") as Label;
                    SiteId = Convert.ToString(lblSite.Text.Trim());
                    BindDropDown(drpSite, drpVillage);
                    drpSite.SelectedValue = SiteId;
                    drpVillage.SelectedValue = VillageId;

                    RadDropDownList drpProj = (RadDropDownList)editItem.FindControl("drpProject");
                    Label lblProject = item["Project"].FindControl("lblProject") as Label;
                    projId = lblProject.Text;

                    if (!string.IsNullOrEmpty(SiteId.Trim()))
                    {
                        bindProjDropDown(drpProj, SiteId.Trim());
                        drpProj.SelectedValue = projId.Trim();
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                }
            }
        }

        private void BindDropDown(RadDropDownList drpSite,RadDropDownList drpVillage)
        {
            string drpname = "site,village";
            string result1 = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname +"", string.Empty);
            DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result1);

            drpSite.DataTextField = "Name";
            drpSite.DataValueField = "Id";
            drpSite.DataSource = ddValues.site;
            drpSite.DataBind();        

            drpVillage.DataTextField = "Name";
            drpVillage.DataValueField = "Id";
            drpVillage.DataSource = ddValues.village;
            drpVillage.DataBind();
        }

        private List<Models.Menu> binddrpSite()
        {
            string result1 = commonFunctions.RestServiceCall(Constants.PROFILE_GET_MENULIST, string.Empty);
            List<Models.Menu> lstmnu = JsonConvert.DeserializeObject<List<Models.Menu>>(result1);
            return lstmnu;
        }

        protected void gridSurvey_CancelCommand(object sender, GridCommandEventArgs e)
        {
            lblErrorMessage.Visible = false;
        }

        protected void BtnImport1_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
        {
            if (e.File.GetExtension().ToString().Trim().ToLower() == ".xls" || (e.File.GetExtension().ToString().Trim().ToLower() == ".xlsx"))
            {       

                try
                {
                    string FilePath = "~/Upload/Survey/";
                    int ImportCount;
                    String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                    string fileext = e.File.GetExtension();
                    string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
                    BtnImport1.TargetFolder = FilePath;
                    e.File.SaveAs(Path.Combine(Server.MapPath(BtnImport1.TargetFolder), newfilename));
                    string FilePath1 = "~/Upload/Survey/" + newfilename;

                    DataTable ExcelDT = commonFunctions.GetFileData(Server.MapPath(FilePath1), "");

                    if (ExcelDT.Rows.Count > 0)
                    { 
                     ExcelDT = ExcelDT.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();

                    ImportCount = Convert.ToInt32(ExcelDT.Rows.Count);

                    ExcelDT.Columns.Add("Id", typeof(Int32)).SetOrdinal(0);
                    ExcelDT.Columns.Add("VillageId", typeof(int)).SetOrdinal(3);
                    ExcelDT.Columns.Add("CreatedBy", typeof(int)).SetOrdinal(7);
                    ExcelDT.Columns.Add("CreatedOn", typeof(DateTime)).SetOrdinal(8);
                    ExcelDT.Columns.Add("ModifiedBy", typeof(int)).SetOrdinal(9);
                    ExcelDT.Columns.Add("ModifiedOn", typeof(DateTime)).SetOrdinal(10);
                    ExcelDT.Columns.Add("Status", typeof(Boolean)).SetOrdinal(11);
                    ExcelDT.Columns.Add("IsValidated", typeof(Boolean)).SetOrdinal(12);
                    ExcelDT.Columns.Add("IsMerged", typeof(Boolean)).SetOrdinal(13);

                    if (string.Compare(fileext, ".xls", true) == 0)
                    {
                        ExcelDT.Columns["SurveyNo"].SetOrdinal(1);
                        ExcelDT.Columns["No Of Division"].SetOrdinal(6);
                        ExcelDT.Columns["Project"].SetOrdinal(14);

                    }
                    else
                    {
                        ExcelDT.Columns["F4"].SetOrdinal(1);
                        ExcelDT.Columns["F6"].SetOrdinal(6);
                        ExcelDT.Columns["F2"].SetOrdinal(14);

                    }
                    string currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); // HH 24 hrs format, hh 12 hours format, yyyy/MM/dd HH:mm:ss.fff for milisecond
                    foreach (DataRow dr in ExcelDT.Rows)
                    {
                        dr["Status"] = 1;
                        dr["CreatedBy"] = Convert.ToString(Session["UserId"]);
                        dr["CreatedOn"] = currentTime;
                        dr["ModifiedBy"] = Convert.ToString(Session["UserId"]);
                        dr["ModifiedOn"] = currentTime;
                    }
                    try
                    {
                        SqlBulkCopy sqlCopy = new SqlBulkCopy(commonFunctions.GetDBConnectionString());
                        sqlCopy.DestinationTableName = "[dbo].[SurveyMasterStaging]";
                        sqlCopy.WriteToServer(ExcelDT);

                        commonFunctions.UploadData(Convert.ToString(Session["UserId"]), currentTime + ":000", "SurveyMasterStaging");

                        StringBuilder StrQuery = new StringBuilder("");
                        StrQuery.Append("select SurveyNo,Site,VillageName from  SurveyMasterStaging where createdby = '" + Convert.ToString(Session["UserId"]) + "' ");
                        StrQuery.Append("and createdon = '" + Convert.ToString(currentTime + ":000") + "'and isValidated is null and isMerged is null");

                        DataTable validateDT = commonFunctions.GetDataTable(StrQuery.ToString());

                        string NotUploadRec = string.Empty;
                        foreach (DataRow dr in validateDT.Rows)
                        {
                            NotUploadRec = NotUploadRec + Convert.ToString(dr["SurveyNo"]) + ", ";
                        }

                        int UploadCount = ImportCount - (Convert.ToInt32(validateDT.Rows.Count));

                        GetSurveyDetails();
                        gridSurvey.DataBind();


                        string Errmsg = Constants.TOTAL_RECORD_MESSAGE + "<B>" + ImportCount + "</B> " + "</br> ";
                        Errmsg = Errmsg + Constants.UPLOAD_RECORD_MESSAGE + "<B>" + UploadCount + "</B> " + "</br> ";
                        Errmsg = Errmsg + Constants.NOTUPLOAD_RECORD_MESSAGE + "<B> " + Convert.ToInt32(validateDT.Rows.Count) + "</B> " + "";
                        if (NotUploadRec.Length > 1)
                        {
                            Errmsg = Errmsg + "</br>Not Uploaded Survey Nos:- " + NotUploadRec.Substring(0, NotUploadRec.Length - 1);
                        }
                        RadNotification1.Title = "Import Survey Master";
                        RadNotification1.Show(Errmsg);

                    }
                    catch (Exception ex)
                    {
                        CommonFunctions.WriteErrorLog(ex);
                        RadNotification1.Title = "Import Survey Master";
                        RadNotification1.Show(ex.Message.Replace("'", "\""));
                    }
                  }
                    else
                    {
                        RadNotification1.Title = "Import Survey Master";
                        RadNotification1.Show(Constants.BLANK_EXCELSHEET_MSG);
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                    RadNotification1.Title = "Import Survey Master";
                    RadNotification1.Show(Constants.EXCELSHEETERRORMSG);
                }


            }
            else
            {
               
                RadNotification1.Title = "Import Survey Master";
                RadNotification1.Show(Constants.FILEExTENSION);

            }

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //gridSurvey.ExportSettings.ExportOnlyData = true;
                //gridSurvey.ExportSettings.IgnorePaging = true;
                //gridSurvey.ExportSettings.OpenInNewWindow = true;
                //gridSurvey.ExportSettings.FileName = "SurveyMaster";
                //gridSurvey.MasterTableView.ExportToExcel();

                gridSurvey.MasterTableView.AllowFilteringByColumn = false;
                gridSurvey.MasterTableView.AllowSorting = false;
                gridSurvey.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridSurvey.ExportSettings.FileName= "SurveyMaster";
                gridSurvey.Rebind();
                gridSurvey.MasterTableView.ExportToExcel();


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
            gridSurvey.MasterTableView.ClearEditItems();
            gridSurvey.MasterTableView.IsItemInserted = true;
            gridSurvey.MasterTableView.Rebind();
        }


        public class SurveyMasterModel
        {
            public int SurveyId { get; set; }
            public string SurveyNo { get; set; }
            public string Site { get; set; }
            public int VillageId { get; set; }
            //public int PropsedTotal { get; set; }
            public decimal Area { get; set; }
            public int NoOfDivision { get; set; }
            public int CreatedBy { get; set; }
            public System.DateTime CreatedOn { get; set; }
            public int ModifiedBy { get; set; }
            public System.DateTime ModifiedOn { get; set; }
            public bool Status { get; set; }
            public string VillageName { get; set; }

            public string SAPProjectId { get; set; }
            public string ProjectDescription { get; set; }         

        }

        protected void gridSurvey_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (Convert.ToString(e.CommandName))
            {
                case "InitInsert":
                    gridSurvey.MasterTableView.ClearEditItems();
                    break;
                case "Edit":
                    e.Item.OwnerTableView.IsItemInserted = false;
                    break;
                case "Filter":
                    gridSurvey.MasterTableView.ClearEditItems();
                    break;
            }

        }
    }
}
