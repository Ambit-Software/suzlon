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

namespace SolarPMS.Admin
{
    public partial class TestUpload : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnImport1_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
        {
            //  string FilePath1 = "~/Upload/Survey/" + e.File.GetName();
            string FilePath = "~/Upload/Survey/";
            int ImportCount;
            String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");

            string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
            BtnImport1.TargetFolder = FilePath;
            e.File.SaveAs(Path.Combine(Server.MapPath(BtnImport1.TargetFolder), newfilename));
            string FilePath1 = "~/Upload/Survey/" + newfilename;

            DataTable ExcelDT = commonFunctions.GetFileData(Server.MapPath(FilePath1), "");


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

                //GetSurveyDetails();
                //gridSurvey.DataBind();

                //string Errmsg = "<B>" + UploadCount + "</B> " + Constants.UPLOAD_RECORD_MESSAGE + " ";
                //Errmsg = Errmsg + "</br><B> " + Convert.ToInt32(validateDT.Rows.Count) + "</B> " + Constants.NOTUPLOAD_RECORD_MESSAGE + "";
                //Errmsg = Errmsg + "</br>" + NotUploadRec.Substring(0, NotUploadRec.Length - 1);
                //RadNotification1.Title = "Import SurveyMaster";
                //RadNotification1.Show(Errmsg);

            }
            catch (Exception ex)
            {
                throw ex;
                //RadNotification1.Title = "Import SurveyMaster";
                //RadNotification1.Show(ex.Message.ToString());
            }


        }
    }
}