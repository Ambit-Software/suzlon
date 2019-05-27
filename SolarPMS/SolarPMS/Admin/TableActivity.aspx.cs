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
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.ExportInfrastructure;

namespace SolarPMS.Admin
{
    public partial class TableActivity : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        public const string NotificationTitle = "Import Table Activity";

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rdbAddBlock_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Admin/TableActivityBlock.aspx");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rdbAddInvertor_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Admin/TableActivityInvertor.aspx");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rdbAddSCB_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Admin/TableActivitySCB.aspx");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rdbAddTable_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Admin/TableActivityTable.aspx");

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rdbDownLoadBlock_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableActivityData("Block.xls", "Block");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rdbDownloadInvertor_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableActivityData("Inverter.xls", "Inverter");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rdbDownloadSCB_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableActivityData("SCB.xls", "SCB");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rdbDownloadTable_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableActivityData("Table.xls", "Table");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnDownloadTableTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                GetTemplateData("TableTemplate", "Table");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnDownloadSCBTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                GetTemplateData("SCBTemplate", "SCB");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnDownloadInverterTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                GetTemplateData("InvertorTemplate", "Inverter");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnDownloadBlockTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                GetTemplateData("BlockTemplate", "Block");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnImportBlock_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                string fileExtension = e.File.GetExtension().Trim().ToLower();
                string FilePath = btnImportBlock.TargetFolder;
                UploadData(e, fileExtension, FilePath, "Block"); ;

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                RadNotification1.Title = NotificationTitle;
                if (ex.Message == "External table is not in the expected format.")
                    RadNotification1.Show(Constants.INVALID_TEMPLATE_FILE);
                else
                    RadNotification1.Show(Constants.EXCELSHEETERRORMSG);
            }
        }

        protected void btnImportInvertor_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                string fileExtension = e.File.GetExtension().Trim().ToLower();
                string FilePath = btnImportInvertor.TargetFolder;
                UploadData(e, fileExtension, FilePath, "Inverter");

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                RadNotification1.Title = NotificationTitle;
                RadNotification1.Show(Constants.EXCELSHEETERRORMSG);
            }
        }

        protected void btnImportSCB_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                string fileExtension = e.File.GetExtension().Trim().ToLower();
                string FilePath = btnImportSCB.TargetFolder;
                UploadData(e, fileExtension, FilePath, "SCB");

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                RadNotification1.Title = NotificationTitle;
                RadNotification1.Show(Constants.EXCELSHEETERRORMSG);
            }
        }

        protected void btnImportTable_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                string fileExtension = e.File.GetExtension().Trim().ToLower();

                string FilePath = btnImportTable.TargetFolder;
                UploadData(e, fileExtension, FilePath, "Table");

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                RadNotification1.Title = NotificationTitle;
                RadNotification1.Show(Constants.EXCELSHEETERRORMSG);
            }
        }

        #endregion

        #region "Private Methods"
        private void UploadData(FileUploadedEventArgs e, string fileExtension, string FilePath, string FilterType)
        {
            if (fileExtension == ".xls" || (fileExtension == ".xlsx"))
            {
                int ImportCount;
                string timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                string newfilename = e.File.GetNameWithoutExtension() + timeStamp + fileExtension;
                e.File.SaveAs(Path.Combine(Server.MapPath(FilePath), newfilename));

                DataTable ExcelDT = commonFunctions.GetFileData(Server.MapPath(FilePath + newfilename), "");
                if (ExcelDT != null && ExcelDT.Rows.Count > 0)
                {
                    ExcelDT = ExcelDT.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull
                    || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                    ImportCount = Convert.ToInt32(ExcelDT.Rows.Count);

                    ExcelDT.Columns.RemoveAt(2);
                    ExcelDT.Columns.RemoveAt(3);
                    ExcelDT.Columns.RemoveAt(4);
                    ExcelDT.Columns.RemoveAt(5);
                    ExcelDT.Columns.RemoveAt(6);

                    ExcelDT.Columns.Add("TableActivityId", typeof(Int32)).SetOrdinal(0);
                    ExcelDT.Columns.Add("ActivityElementof", typeof(string)).SetOrdinal(7);
                    ExcelDT.Columns.Add("CreatedBy", typeof(int));
                    ExcelDT.Columns.Add("CreatedOn", typeof(DateTime));
                    ExcelDT.Columns.Add("ModifiedBy", typeof(int));
                    ExcelDT.Columns.Add("ModifiedOn", typeof(DateTime));
                    ExcelDT.Columns.Add("IsValidated", typeof(bool));
                    ExcelDT.Columns.Add("IsMerged", typeof(bool));

                    string currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); // HH 24 hrs format, hh 12 hours format, yyyy/MM/dd HH:mm:ss.fff for milisecond

                    int userId = Convert.ToInt32(Session["UserId"]);

                    DataTable dtClonedExcelData = UpdateExcelDataRow(ExcelDT, currentTime, userId);

                    try
                    {
                        DataTable validateDT;
                        string NotUploadRec;
                        int UploadCount;

                        SaveStagingTableData(dtClonedExcelData);
                        commonFunctions.UploadData(Convert.ToString(userId), currentTime + ":000", "TableActivityStaging");
                        ValidateSavedRecords(ImportCount, currentTime, userId, out validateDT, out NotUploadRec, out UploadCount, FilterType);
                        ShowMessage(ImportCount, validateDT, NotUploadRec, UploadCount);
                    }
                    catch (Exception ex)
                    {
                        CommonFunctions.WriteErrorLog(ex);
                        RadNotification1.Title = NotificationTitle;
                        RadNotification1.Show("Invalid data. Please verify excel sheet.");
                    }
                }
                else
                {
                    RadNotification1.Title = NotificationTitle;
                    RadNotification1.Show(Constants.BLANK_EXCELSHEET_MSG);
                }
            }
            else
            {
                RadNotification1.Title = NotificationTitle;
                RadNotification1.Show(Constants.INVALID_FILE_TYPE);
            }
        }

        private static DataTable UpdateExcelDataRow(DataTable ExcelDT, string currentTime, int userId)
        {
            // To change data type of activity column 
            DataTable dtCloned = ExcelDT.Clone();
            dtCloned.Columns[5].DataType = typeof(string);
            dtCloned.Columns[6].DataType = typeof(string);
            foreach (DataRow row in ExcelDT.Rows)
                dtCloned.ImportRow(row);

            foreach (DataRow dr in dtCloned.Rows)
            {
                string activity = !string.IsNullOrEmpty(Convert.ToString(dr[5])) ? dr[5].ToString() : "0";
                string subActivity = !string.IsNullOrEmpty(Convert.ToString(dr[6])) ? dr[6].ToString() : "";
                int activityCharacterLength = activity.Length;
                int subActivityCharacterLength = subActivity.Length;

                //if (activityCharacterLength == 1)
                //    dr[5] = "000" + activity;// in excel 00 is removed from activity id
                //else if (activityCharacterLength == 2)
                //    dr[5] = "00" + activity;
                //else if (activityCharacterLength == 3)
                //    dr[5] = "0" + activity;

                //if (subActivityCharacterLength == 1)
                //    dr[6] = "000" + subActivity;
                //else if (subActivityCharacterLength == 2)
                //    dr[6] = "00" + subActivity;
                //else if (subActivityCharacterLength == 3)
                //    dr[6] = "0" + subActivity;
                //else dr[6] = subActivity;

                dr["ActivityElementof"] = DBNull.Value;
                dr["TableActivityId"] = 0;
                dr["IsValidated"] = DBNull.Value;
                dr["IsMerged"] = DBNull.Value;
                dr["CreatedBy"] = userId;
                dr["CreatedOn"] = currentTime;
                dr["ModifiedBy"] = userId;
                dr["ModifiedOn"] = currentTime;
            }

            return dtCloned;
        }

        private void ShowMessage(int ImportCount, DataTable validateDT, string NotUploadRec, int UploadCount)
        {
            string Errmsg = Constants.TOTAL_RECORD_MESSAGE + "<B>" + ImportCount + "</B> " + "</br> ";
            Errmsg = Errmsg + Constants.UPLOAD_RECORD_MESSAGE + "<B>" + UploadCount + "</B> " + "</br> ";
            Errmsg = Errmsg + Constants.NOTUPLOAD_RECORD_MESSAGE + "<B> " + Convert.ToInt32(validateDT.Rows.Count) + "</B> " + "";
            if (NotUploadRec.Length > 1)
            {
                Errmsg = Errmsg + "</br>Not Uploaded Activitie(s):- " + NotUploadRec.Substring(0, NotUploadRec.Length - 1);
            }

            RadNotification1.Title = NotificationTitle;
            RadNotification1.Show(Errmsg);
        }

        private void ValidateSavedRecords(int ImportCount, string currentTime, int userId, out DataTable validateDT, out string NotUploadRec, out int UploadCount, string FilterType)
        {
            StringBuilder StrQuery = new StringBuilder("");
            StrQuery.Append("SELECT ActivityDescription from TableActivityStaging TS inner join SAPMaster S on S.SAPSite = Ts.Site ");
            StrQuery.Append("and S.SAPProjectId = TS.ProjectId and S.WBSAreaId = Ts.AreaId and S.SAPNetwork = Ts.NetworkId ");
            StrQuery.Append("and S.SAPActivity = TS.ActivityId where TS.createdby = '" + userId + "' AND Flag ='" + FilterType + "'");
            StrQuery.Append("and TS.Createdon = '" + Convert.ToString(currentTime + ":000") + "'and isValidated is null and isMerged is null");

            validateDT = commonFunctions.GetDataTable(StrQuery.ToString());
            NotUploadRec = string.Empty;
            foreach (DataRow dr in validateDT.Rows)
            {
                NotUploadRec = NotUploadRec + Convert.ToString(dr["ActivityDescription"]) + ", ";
            }

            UploadCount = ImportCount - (Convert.ToInt32(validateDT.Rows.Count));
        }

        private void SaveStagingTableData(DataTable ExcelDT)
        {
            SqlBulkCopy sqlCopy = new SqlBulkCopy(commonFunctions.GetDBConnectionString());
            sqlCopy.DestinationTableName = "[dbo].[TableActivityStaging]";
            sqlCopy.WriteToServer(ExcelDT);
        }

        private void ExportToExcel(string strFileName, GridView gv)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
            Response.ContentType = "application/excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        private void GetTemplateData(string FileName, string FilterType)
        {
            SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@FiterType", FilterType) };
            DataTable validateDT = commonFunctions.ExecuteProcedureAndGetDataTable("usp_GetTableActivityDataForExportTemplate", sqlParameter);
            DataTable dtCloned = validateDT.Clone();
            dtCloned.Columns["NetworkId"].DataType = System.Type.GetType("System.String");
            dtCloned.Columns["ActivityId"].DataType = System.Type.GetType("System.String");
            dtCloned.Columns["SubActivityId"].DataType = System.Type.GetType("System.String");
            foreach (DataRow dr in validateDT.Rows)
            {
                dtCloned.Rows.Add(dr.ItemArray);
            }
            radGrid.DataSource = validateDT;
            radGrid.DataBind();
            radGrid.ExportSettings.FileName = FileName;
            radGrid.ExportToExcel();
            //GridView gvreport = new GridView();
            //gvreport.DataSource = dtCloned;
            //gvreport.DataBind();
            //ExportToExcel(FileName, gvreport);
        }
        private void GetTableActivityData(string FileName, string FilterType)
        {
            StringBuilder StrQuery = new StringBuilder("");
            StrQuery.Append(" SELECT DISTINCT ");
            StrQuery.Append(" SM.SAPSite Site, ");
            StrQuery.Append(" SM.ProjectDescription Project, ");
            StrQuery.Append(" SM.WBSArea Area, ");
            StrQuery.Append(" SM.NetworkDescription Network, ");
            StrQuery.Append(" SM.ActivityDescription Activity, ");
            StrQuery.Append(" SM.SAPSubActivityDescription SubActivity,");
            StrQuery.Append(" TA.Number,");
            StrQuery.Append(" TA.Quantity");
            StrQuery.Append(" FROM TableActivity TA");
            StrQuery.Append(" INNER JOIN SAPMaster SM");
            StrQuery.Append(" ON SM.SAPSite = TA.Site");
            StrQuery.Append(" AND SM.SAPProjectId = TA.ProjectId");
            StrQuery.Append(" AND SM.WBSAreaId = TA.AreaId");
            StrQuery.Append(" AND SM.SAPNetwork = TA.NetworkId");
            StrQuery.Append(" AND SM.SAPActivity = TA.ActivityId");
            StrQuery.Append(" AND (ISNULL(SM.SAPSubActivity, '') = (CASE ");
            StrQuery.Append(" WHEN TA.SubActivityId <> '' THEN TA.SubActivityId");
            StrQuery.Append(" ELSE ''");
            StrQuery.Append(" END))");
            StrQuery.AppendFormat(" WHERE Flag = '{0}' AND SM.IsDeleted IS NULL", FilterType);
            DataTable validateDT = commonFunctions.GetDataTable(StrQuery.ToString());

            GridView gvreport = new GridView();
            gvreport.DataSource = validateDT;
            gvreport.DataBind();

            ExportToExcel(FileName, gvreport);
        }

        #endregion

        protected void radGrid_InfrastructureExporting(object sender, GridInfrastructureExportingEventArgs e)
        {
            try
            {
                foreach (Cell cell in e.ExportStructure.Tables[0].Columns[1].Cells)
                {
                    cell.Format = "@";
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}