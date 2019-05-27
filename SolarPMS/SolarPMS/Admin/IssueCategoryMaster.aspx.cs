using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
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
    public partial class IssueCategoryMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindCategoryGrid();
                gridIssueCategory.DataBind();
            }
        }

        private void bindCategoryGrid()
        {
            string getAllCategoryResult = commonFunctions.RestServiceCall(Constants.ISSUECAT_MASTER_GET, string.Empty);            
            if (!string.IsNullOrEmpty(getAllCategoryResult))
            {
                List<Models.IssueCategory> lstIssueCategory = JsonConvert.DeserializeObject<List<Models.IssueCategory>>(getAllCategoryResult);
                gridIssueCategory.DataSource = lstIssueCategory;                
            }
            else
            {
                gridIssueCategory.DataSource = new DataTable();
            }

        }

        protected void gridIssueCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bindCategoryGrid();
        }

        protected void gridIssueCategory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            switch (Convert.ToString(e.CommandName))
            {
                case "InitInsert":
                    gridIssueCategory.MasterTableView.ClearEditItems();
                    break;
                case "Edit":
                    e.Item.OwnerTableView.IsItemInserted = false;
                    break;
                case "Filter":
                    gridIssueCategory.MasterTableView.ClearEditItems();
                    break;
            }
        }

        protected void gridIssueCategory_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            SaveCategory(e, Constants.CONST_EDIT_MODE);
        }

        protected void gridIssueCategory_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            SaveCategory(e, Constants.CONST_NEW_MODE);
        }

        protected void gridIssueCategory_CancelCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            lblErrorMessage.Visible = false;
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //gridIssueCategory.ExportSettings.ExportOnlyData = true;
                //gridIssueCategory.ExportSettings.IgnorePaging = true;
                //gridIssueCategory.ExportSettings.OpenInNewWindow = true;
                //gridIssueCategory.ExportSettings.FileName = "IssueCategoryMaster";
                //gridIssueCategory.MasterTableView.ExportToExcel();

                gridIssueCategory.MasterTableView.AllowFilteringByColumn = false;
                gridIssueCategory.MasterTableView.AllowSorting = false;
                gridIssueCategory.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridIssueCategory.ExportSettings.FileName = "IssueCategoryMaster";
                gridIssueCategory.Rebind();
                gridIssueCategory.MasterTableView.ExportToExcel();


            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }


        protected void btnAddNew_Click(object sender, EventArgs e)
        {           
            gridIssueCategory.MasterTableView.ClearEditItems();
            gridIssueCategory.MasterTableView.IsItemInserted = true;
            gridIssueCategory.MasterTableView.Rebind();
        }
        protected void BtnImport1_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
        {
            if (e.File.GetExtension().ToString().Trim().ToLower() == ".xls" || (e.File.GetExtension().ToString().Trim().ToLower() == ".xlsx"))
            {               
                try
                {
                    string FilePath = "~/Upload/IssueCategory/";
                    int ImportCount;
                    BtnImport1.TargetFolder = FilePath;
                    String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                    string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
                    string FilePath1 = "~/Upload/IssueCategory/" + newfilename;
                    e.File.SaveAs(Path.Combine(Server.MapPath(BtnImport1.TargetFolder), newfilename));
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
                            sqlCopy.DestinationTableName = "IssueCategoryStaging";
                            sqlCopy.WriteToServer(ExcelDT);

                            commonFunctions.UploadData(Convert.ToString(Session["UserId"]), currentTime + ":000", "IssueCategoryStaging");

                            bindCategoryGrid();
                            gridIssueCategory.DataBind();

                            StringBuilder StrQuery = new StringBuilder("");
                            StrQuery.Append("select CategoryName, Description from [dbo].[IssueCategoryStaging] where createdby = '" + Convert.ToString(Session["UserId"]) + "' ");
                            StrQuery.Append("and createdon = '" + Convert.ToString(currentTime + ":000") + "'and isValidated is null and isMerged is null");

                            DataTable validateDT = commonFunctions.GetDataTable(StrQuery.ToString());

                            string NotUploadRec = string.Empty;
                            foreach (DataRow dr in validateDT.Rows)
                            {
                                NotUploadRec = NotUploadRec + Convert.ToString(dr["CategoryName"]) + ", ";
                            }

                            int UploadCount = ImportCount - (Convert.ToInt32(validateDT.Rows.Count));

                            string Errmsg = Constants.TOTAL_RECORD_MESSAGE + "<B>" + ImportCount + "</B> " + "</br> ";
                            Errmsg = Errmsg + Constants.UPLOAD_RECORD_MESSAGE + "<B>" + UploadCount + "</B> " + "</br> ";
                            Errmsg = Errmsg + Constants.NOTUPLOAD_RECORD_MESSAGE + "<B> " + Convert.ToInt32(validateDT.Rows.Count) + "</B> " + "";
                            if (NotUploadRec.Length > 1)
                            {
                                Errmsg = Errmsg + "</br>Not Uploaded Categories:- " + NotUploadRec.Substring(0, NotUploadRec.Length - 1);
                            }
                            radMesaage.Title = "Import Issue Category Master";
                            radMesaage.Show(Errmsg);


                        }

                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                            radMesaage.Title = "Import IssueCategory Master";
                            radMesaage.Show(ex.Message.Replace("'", "\""));
                        }
                    }
                    else
                    {
                        radMesaage.Title = "Import IssueCategory Master";
                        radMesaage.Show(Constants.BLANK_EXCELSHEET_MSG);
                    }

                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                    radMesaage.Title = "Import IssueCategory Master";
                    radMesaage.Show(Constants.EXCELSHEETERRORMSG);
                }
            }
            else
            {   
                radMesaage.Title = "Import IssueCategory Master";
                radMesaage.Show(Constants.FILEExTENSION);
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "hideProgress()", true);
            LoadingPanel1.Visible = false;
            //ScriptManager.RegisterStartupScript(this, GetType(), "Hide Progress", "hideProgress();", true);
        }
        private void SaveCategory(GridCommandEventArgs e, string editMode)
        {
            try
            {
                
                int IssueID = 0;

                GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    string Category = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("categoryName")).TextBoxControl.Text;
                    // bool IssueStatus = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                    bool IssueStatus = (editableItem.FindControl("chkStatus")as CheckBox).Checked;

                    TextBox txtDesc = editableItem.FindControl("txtDiscription") as TextBox;                    
                    string IssueDescription = txtDesc.Text;
                  
                   

                    if (editMode == Constants.CONST_EDIT_MODE)
                        IssueID = Convert.ToInt32(editableItem.GetDataKeyValue("issueCategoryId"));
                   

                    Models.IssueCategory IssueCat = new IssueCategory() {
                        IssueCategoryId= IssueID,
                        CategoryName= Convert.ToString(Category),
                        Description=Convert.ToString(IssueDescription),
                        Status= IssueStatus
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(IssueCat);
                    string isExistResult = string.Empty;

                    isExistResult = commonFunctions.RestServiceCall(string.Format(Constants.ISSUECAT_MASTER_EXISTS, Category,IssueID), string.Empty);
                    bool isExist = Convert.ToBoolean(isExistResult);
                    if (isExist)
                    {
                     
                        radMesaage.Title="Alert";
                        radMesaage.Show(Constants.ISSUECAT_ALREADY_EXISIT);
                        e.Canceled = true;
                        return;
                    }
                    

                    string serviceResult = string.Empty;

                    if (editMode == Constants.CONST_EDIT_MODE)
                        serviceResult = commonFunctions.RestServiceCall(Constants.ISSUECAT_MASTER_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        serviceResult = commonFunctions.RestServiceCall(Constants.ISSUECAT_MASTER_ADD, Crypto.Instance.Encrypt(jsonInputParameter));

                    if (string.Compare(serviceResult,Constants.REST_CALL_FAILURE,true)==0)
                    {
                        radMesaage.Title = "Alert";
                        radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);

                    }
                    else
                    { 
                        radMesaage.Title = "Sucesss";
                        radMesaage.Show(Constants.RECORD_SAVE_SUCESSFULLY);
                    }
                                    
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }



        protected void gridIssueCategory_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                if(e.Item is GridDataInsertItem)
                {
                    CheckBox chkStatus = (CheckBox)dataItem.FindControl("chkStatus");
                    chkStatus.Attributes.Add("onclick", "oncheckedChaned(this);");
                    chkStatus.Checked = true;
                }
                if (e.Item.IsInEditMode)  {
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
    }
}