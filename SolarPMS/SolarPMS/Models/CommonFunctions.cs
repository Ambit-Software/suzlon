using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;
using SolarPMS.Models;

namespace SolarPMS.Models
{
    public class CommonFunctions
    {
        public string RestServiceCall(string url, string parameter)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(Constants.RESTURL);

                if (HttpContext.Current.Session["Token"] != null)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Current.Session["Token"] as string);
                }
                else
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                if (HttpContext.Current.Session["UserId"] != null)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("UserId", HttpContext.Current.Session["UserId"] as string);
                }
                PostParam paramObject = new PostParam();

                if (!string.IsNullOrEmpty(parameter))
                {
                    paramObject.Data = parameter;
                }
                StringContent str = new StringContent(JsonConvert.SerializeObject(paramObject));
                var response = ((string.IsNullOrEmpty(parameter)) ? client.GetAsync(url).GetAwaiter().GetResult() : client.PostAsJsonAsync(new Uri(Constants.RESTURL + url), paramObject).GetAwaiter().GetResult());
                //string responseJson = string.Empty;
                string responseJson = Constants.REST_CALL_FAILURE;
                if (response.IsSuccessStatusCode)
                    responseJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return responseJson;
            }
            catch (Exception ex)
            {
                //return string.Empty;
                CommonFunctions.WriteErrorLog(ex);
                return Constants.REST_CALL_FAILURE;
            }
        }

        public string GetToken(string url, string parameter)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(Constants.RESTURL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                StringContent str = new StringContent(parameter);
                var response = ((string.IsNullOrEmpty(parameter)) ? client.GetAsync(url).GetAwaiter().GetResult() : client.PostAsync(new Uri(Constants.RESTURL + url), str).GetAwaiter().GetResult());
                string responseJson = string.Empty;
                if (response.IsSuccessStatusCode)
                    responseJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return responseJson;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public DropdownValues PopulateDropdowns(string names)

        {
            DropdownValues ddValues = new DropdownValues();
            if (!string.IsNullOrEmpty(names))
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                    names.Split(',').ToList().ForEach(name =>
                    {
                        switch (name)
                        {
                            case "location":
                                {
                                    ddValues.location = new List<ListItem>();
                                    solarPMSEntities.LocationMasters.AsNoTracking().Where(p => p.Status).OrderBy(p => p.LocationName).ToList().ForEach(l =>
                                      {
                                          ddValues.location.Add(new ListItem() { Id = l.LocationId.ToString(), Name = l.LocationName });
                                      });

                                    break;
                                }
                            case "profile":
                                {
                                    ddValues.profile = new List<ListItem>();
                                    solarPMSEntities.ProfileMasters.AsNoTracking().Where(p => p.Status).OrderBy(p => p.ProfileName).ToList().ForEach(l =>
                                      {
                                          ddValues.profile.Add(new ListItem() { Id = l.ProfileId.ToString(), Name = l.ProfileName });
                                      });

                                    break;
                                }
                            case "users":
                                {
                                    ddValues.users = new List<ListItem>();
                                    solarPMSEntities.Users.AsNoTracking().Where(p => p.Status).OrderBy(p => p.UserName).ToList().ForEach(l =>
                                      {
                                          ddValues.users.Add(new ListItem() { Id = l.UserId.ToString(), Name = l.UserName });
                                      });

                                    break;
                                }
                            case "issue_names":
                                {
                                    ddValues.issue_names = new List<ListItem>();
                                    solarPMSEntities.IssueManagements.AsNoTracking().Where(p => p.Status).ToList().ForEach(l =>
                                    {
                                        ddValues.issue_names.Add(new ListItem() { Id = l.IssueId.ToString(), Name = l.Description });
                                    });
                                    break;
                                }
                            case "site":
                                {
                                    ddValues.site = new List<ListItem>();
                                    solarPMSEntities.SAPMasters.AsEnumerable().Where(s => s.IsDeleted == null || s.IsDeleted == false).Select(s => new { siteid = s.SAPSite.Trim() }).Distinct().ToList().ForEach(l =>
                                      {
                                          ddValues.site.Add(new ListItem() { Id = l.siteid.Trim(), Name = l.siteid.Trim() });
                                      });
                                    break;
                                }

                            case "project":
                                {
                                    ddValues.project = new List<ListItem>();
                                    solarPMSEntities.SAPMasters.AsNoTracking().Where(s => s.IsDeleted == null || s.IsDeleted == false).Distinct().ToList().ForEach(l =>
                                    {
                                        ddValues.project.Add(new ListItem() { Id = l.SAPProjectId.Trim(), Name = l.ProjectDescription.Trim() });
                                    });
                                    break;
                                }
                            case "village":
                                {
                                    ddValues.village = new List<ListItem>();
                                    solarPMSEntities.VillageMasters.AsNoTracking().Where(p => p.Status).ToList().ForEach(l =>
                                    {
                                        ddValues.village.Add(new ListItem() { Id = l.VillageId.ToString(), Name = l.VillageName });
                                    });
                                    break;
                                }
                            case "category":
                                {
                                    ddValues.category = new List<ListItem>();
                                    solarPMSEntities.IssueCategories.AsNoTracking().Where(p => p.Status).ToList().ForEach(l =>
                                    {
                                        ddValues.category.Add(new ListItem() { Id = l.IssueCategoryId.ToString(), Name = l.CategoryName });
                                    });
                                    break;
                                }
                        }
                    });
                }
            }
            return ddValues;
        }

        public static List<UserAssignedTaskDataForDropdown> GetUserTaskDropdownDataForReport(int UserId, string Site, string Project, string Area, string FilterType, bool FromRport = true, bool DEActivity = false)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var list = solarPMSEntities.usp_GetUserAssignedTaskDataForReports(UserId, FilterType, Site, Project, Area, DEActivity, FromRport).ToList();
                if (FromRport)
                    if ((list.Count > 1 || list.Count < 1) && FilterType != "network")
                        list.Insert(0, new UserAssignedTaskDataForDropdown() { Id = "All", Value = "All" });
                return list;
            }
        }

        public DropdownValues GetProjectsBySite(string value)
        {
            DropdownValues ddValues = new DropdownValues();
            if (!string.IsNullOrEmpty(value))
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    solarPMSEntities.Configuration.ProxyCreationEnabled = false;

                    ddValues.project = new List<ListItem>();
                    (from p in solarPMSEntities.SAPMasters where p.SAPSite == value select new { p.SAPProjectId, p.ProjectDescription }).Distinct().ToList().
                    ForEach(item =>
                    {
                        ddValues.project.Add(new ListItem() { Id = item.SAPProjectId.Trim(), Name = item.ProjectDescription.Trim() });
                    });

                }
            }
            return ddValues;
        }
        public DropdownValues GetAreas(string siteId, string projectId)
        {
            DropdownValues ddValues = new DropdownValues();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                ddValues.area = new List<ListItem>();

                (from saparea in solarPMSEntities.SAPMasters
                 join masterarea in solarPMSEntities.WBSAreas on saparea.WBSArea equals masterarea.WBSArea1
                 where saparea.SAPSite == siteId && saparea.SAPProjectId == projectId
                 select
                 new { saparea.WBSArea, masterarea.WBSAreaId }
                ).Distinct().ToList().ForEach(l =>
                {
                    ddValues.area.Add(new ListItem() { Id = l.WBSAreaId.ToString(), Name = l.WBSArea });
                });
            }
            return ddValues;
        }

        /// <summary>
        /// Without 'F'
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DropdownValues GetAllocatedArea(int userId, string siteId, string projectId)
        {
            DropdownValues ddValues = new DropdownValues();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                ddValues.area = new List<ListItem>();

                //(from taskAllocation in solarPMSEntities.TaskAllocations
                // join area in solarPMSEntities.Areas
                // on  taskAllocation.TaskAllocationId equals area.TaskAllocationId
                // join wArea in solarPMSEntities.WBSAreas
                // on area.WBSAreaId equals wArea.WBSAreaId
                // join sapMaster in solarPMSEntities.SAPMasters
                // on wArea.WBSArea1 equals sapMaster.WBSArea
                // where taskAllocation.SAPSite == siteId && taskAllocation.SAPProjectId == projectId
                // && taskAllocation.UserId == userId
                // && sapMaster.SAPSite == siteId
                // && sapMaster.SAPProjectId == projectId
                // && sapMaster.MobileFunction.ToLower() != "f"
                // orderby wArea.WBSArea1 ascending
                // select
                // new { wArea.WBSArea1, wArea.WBSAreaId }
                //).Distinct().ToList().ForEach(l =>
                //{
                //    ddValues.area.Add(new ListItem() { Id = l.WBSAreaId.ToString(), Name = l.WBSArea1.Trim() });
                //});
                solarPMSEntities.usp_GetUserAssignedTaskDataNonDE(userId, "area", siteId, projectId, string.Empty).ToList().ForEach(l =>
                {
                    ddValues.area.Add(new ListItem() { Id = l.Id.Trim(), Name = l.Value.Trim() });
                });
            }
            return ddValues;
        }


        public DropdownValues GetNetworks(string projId, string area, string site="" )
        {
            DropdownValues ddValues = new DropdownValues();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                ddValues.network = new List<ListItem>();

                solarPMSEntities.SAPMasters.AsEnumerable().Where(s => s.IsDeleted == null || s.IsDeleted == false).Select(s => new { Projid = s.SAPProjectId, Site = s.SAPSite, area = s.WBSAreaId, networkid = s.SAPNetwork, description = s.NetworkDescription }).Distinct().Where(f =>
                          f.Projid.Trim() == projId.Trim()
                          && f.Site.Trim() == site
                          && f.area == Convert.ToInt32(area)).ToList().ForEach(l =>
                          {
                              ddValues.network.Add(new ListItem() { Id = l.networkid, Name = l.description });
                          });
            }
            return ddValues;
        }

        public DropdownValues GetActivity(string projId, string area, string network)
        {
            DropdownValues ddValues = new DropdownValues();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                ddValues.activity = new List<ListItem>();

                solarPMSEntities.SAPMasters.Where(s => s.IsDeleted == null || s.IsDeleted == false).AsEnumerable().Select(s => new { Projid = s.SAPProjectId, area = s.WBSArea, networkid = s.SAPNetwork, activityid = s.SAPActivity, activitydesc = s.ActivityDescription, parentActid = s.ActivityElementof }).Distinct().Where(f =>
                                 f.Projid.Trim() == projId.Trim() && f.area.Trim() == area.Trim() && f.networkid.Trim() == network.Trim() && f.parentActid == null).ToList().ForEach(l =>
                                 {
                                     ddValues.activity.Add(new ListItem() { Id = l.activityid, Name = l.activitydesc });
                                 });
            }
            return ddValues;
        }

        public DropdownValues GetSubActivity(string projId, string area, string network, string activity)
        {
            DropdownValues ddValues = new DropdownValues();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                ddValues.subactivity = new List<ListItem>();

                //solarPMSEntities.SAPMasters.AsEnumerable().Select(s => new { Projid = s.SAPProjectId, area = s.WBSArea, networkid = s.SAPNetwork, activityid = s.SAPActivity, activitydesc = s.ActivityDescription, parentActid = s.ActivityElementof }).Distinct().Where(f =>
                // f.Projid.Trim() == projId.Trim() && f.area.Trim() == area.Trim() && f.networkid.Trim() == network.Trim()).ToList().ForEach(l =>
                //{
                //    ddValues.subactivity.Add(new ListItem() { Id = l.activityid, Name = l.activitydesc });
                //});

                solarPMSEntities.SAPMasters.AsEnumerable().Select(s => new { Projid = s.SAPProjectId, area = s.WBSArea, networkid = s.SAPNetwork, activityid = s.SAPActivity, activitydesc = s.ActivityDescription, SAPSubActivity = s.SAPSubActivity, SAPSubActivityDescription = s.SAPSubActivityDescription }).Distinct().Where(f =>
                      f.Projid.Trim() == projId.Trim() && f.area.Trim() == area.Trim() && f.networkid.Trim() == network.Trim() && f.activityid == activity).ToList().ForEach(l =>
                        {
                            if (l.SAPSubActivity != null)
                                ddValues.subactivity.Add(new ListItem() { Id = l.SAPSubActivity, Name = l.SAPSubActivityDescription });
                        });


            }

            return ddValues;
        }


        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="fromEmail"></param>
        /// <param name="emailBody"></param>
        /// <param name="emailSubject"></param>
        /// <param name="strBCCList"></param>
        /// <param name="AttachmentPath"></param>
        /// <returns></returns>
        public static bool SendEmail(string toEmail, string fromEmail, string emailBody, string emailSubject, List<string> strBCCList = null, String AttachmentPath = "")
        {
            bool result = true;
            string strFrompwd = "";

            try
            {
                string fromAlias = System.Configuration.ConfigurationManager.AppSettings["FromAlias"];
                strFrompwd = System.Configuration.ConfigurationManager.AppSettings["AdminMailPassword"];

                if (string.IsNullOrEmpty(fromEmail))
                    fromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"];

                SmtpClient smtp_mail = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPHOST"]);
                smtp_mail.Credentials = new System.Net.NetworkCredential(fromEmail, strFrompwd);

                //if (string.IsNullOrEmpty(fromEmail))
                //    fromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"];
                //var from = new MailAddress(fromEmail, fromAlias);


                //var to = new MailAddress(toEmail);
                //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(fromEmail, toEmail, emailSubject, emailBody);
                // MailMessage mail = new MailMessage(from.ToString(), TO_addressList.ToString()) { Subject = emailSubject, Body = emailBody };


                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(fromEmail, fromAlias);
                mail.IsBodyHtml = true;
                mail.Subject = emailSubject;
                mail.Body = emailBody;
                //MailAddressCollection TO_addressList = new MailAddressCollection();

                //3.Prepare the Destination email Addresses list
                foreach (var curr_address in toEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    // MailAddress mytoAddress = new MailAddress(curr_address);
                    //TO_addressList.Add(mytoAddress);
                    mail.To.Add(curr_address);
                }


                if (strBCCList != null)
                {
                    foreach (var item in strBCCList)
                    {
                        mail.Bcc.Add(item);
                    }
                }

                if (AttachmentPath != "")
                {
                    Attachment attach = new Attachment(AttachmentPath);
                    mail.Attachments.Add(attach);
                }

                //smtp_mail.UseDefaultCredentials = false;
                //smtp_mail.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPHOST"];
                //smtp_mail.Credentials = new System.Net.NetworkCredential(fromEmail, strFrompwd);
                //smtp_mail.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp_mail.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPORT"]);
                smtp_mail.EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableSsl"]);
                smtp_mail.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// Generate random password.
        /// </summary>
        /// <returns></returns>
        public static int GenerateRandomPassword()
        {
            var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            return new Random(seed).Next();
        }

        public DataTable GetFileData(string strFilePath, string strCommandText)
        {
            DataTable ReadDT = new DataTable();

            string extension = "";
            string connstring = null;
            try
            {
                extension = Path.GetExtension(strFilePath);
                if (extension.ToLower() == ".xlsx")
                {
                    connstring = GetXLSXConnectionString();
                }
                else if (extension.ToLower() == ".xls")
                {
                    connstring = GetXLSConnectionString();
                }
                using (OleDbConnection xlsConnection = new OleDbConnection(String.Format(connstring, strFilePath)))
                {
                    using (OleDbCommand xlsCommand = new OleDbCommand())
                    {
                        OleDbDataAdapter adp = new OleDbDataAdapter(xlsCommand);
                        xlsCommand.Connection = xlsConnection;
                        xlsConnection.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = xlsConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string SheetName = dtExcelSchema.Rows.Count > 0 ? dtExcelSchema.Rows[0]["TABLE_NAME"].ToString() : string.Empty;
                        if (string.IsNullOrEmpty(SheetName))
                            xlsCommand.CommandText = "select * from [Sheet1$]";
                        else
                        xlsCommand.CommandText = "select * from [" + SheetName + "]";

                        adp.Fill(ReadDT);
                    }
                    xlsConnection.Close();
                    if (extension.ToLower() == ".xlsx")
                        ReadDT.Rows.Remove(ReadDT.Rows[0]);
                    return ReadDT;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
                throw;
            }

        }

        public static String GetXLSConnectionString()
        {
            string _xlsConnectionString = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(_xlsConnectionString))
                {
                    _xlsConnectionString = ConfigurationManager.ConnectionStrings["ExcelConnStringForXls"].ConnectionString;

                }
            }
            catch (Exception excObj)
            {
                throw excObj;
            }


            return _xlsConnectionString;
        }

        public static String GetXLSXConnectionString()
        {
            string _xlsxConnectionString = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(_xlsxConnectionString))
                {
                    _xlsxConnectionString = ConfigurationManager.ConnectionStrings["ExcelConnStringForXlsx"].ConnectionString;

                }
            }
            catch (Exception excObj)
            {

                throw excObj;
            }

            return _xlsxConnectionString;
        }

        public String GetDBConnectionString()
        {
            string _dbConnectionString = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(_dbConnectionString))
                {
                    _dbConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                }
            }
            catch (Exception excObj)
            {
                throw excObj;
            }
            return _dbConnectionString;
        }

        public SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            return con;
        }
        public DataTable GetDataTable(string strSQL)
        {
            try
            {
                SqlConnection con = GetConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = strSQL;
                    cmd.CommandType = CommandType.Text;
                    da.SelectCommand = cmd;
                    da.SelectCommand.Connection = con;
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {

            }
        }

        public DataTable ExecuteProcedureAndGetDataTable(string strStoreprocedure, SqlParameter[] SqlParameter)
        {
            try
            {
                SqlConnection con = GetConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = strStoreprocedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(SqlParameter);
                    da.SelectCommand = cmd;
                    da.SelectCommand.Connection = con;
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {

            }
        }

        public void UploadData(string UserId, string TimeStamp, string TableName)
        {
            try
            {
                SqlConnection con = GetConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "ValidateLocationMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", Convert.ToString(UserId));
                cmd.Parameters.AddWithValue("@TimeStamp", Convert.ToString(TimeStamp));
                cmd.Parameters.AddWithValue("@TableName", Convert.ToString(TableName));

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);

            }


        }

        /// <summary>
        /// Write exception details to database.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="errorDetails"></param>
        //public static void WriteErrorLog(Exception ex, int userId)
        //{


        //}
        public static void WriteErrorLog(Exception ex)
        {
            WriteErrorLog(ex, Convert.ToInt32(HttpContext.Current.Session["UserId"]));
        }

        public static void WriteErrorLog(Exception ex, int userId)
        {
            try
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    var trace = new StackTrace(ex);
                    var frame = trace.GetFrame(0);
                    var method = frame.GetMethod();
                    solarPMSEntities.ErroLogs.Add(new ErroLog()
                    {
                        FunctionName = method.Name,
                        Page = method.DeclaringType.FullName,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        UserId = userId,
                        CreatedOn = DateTime.Now
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
            catch (Exception exception)
            {

            }
        }


        //public static void WriteErrorLogMobile(Errors errors, string functionName, string page)
        //{
        //    WriteErrorLogMobile(errors, Convert.ToInt32(HttpContext.Current.Session["UserId"]));
        //}

        public static void WriteErrorLogMobile(string Message, string StackTrace, string PageName, string FunctionName, int userId)
        {
            try
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {

                    solarPMSEntities.ErroLogs.Add(new ErroLog()
                    {
                        FunctionName = FunctionName,
                        Page = PageName,
                        Message = Message,
                        StackTrace = StackTrace,
                        UserId = userId,
                        CreatedOn = DateTime.Now
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
            catch (Exception exception)
            {

            }
        }

        public static int[] GetNumberRange(int FromRange, int ToRange)
        {
            return Enumerable.Range(FromRange, (ToRange - FromRange) + 1).ToArray();
        }
    }

    public class Errors
    {
        public System.Exception ex { get; set; }
        public string memberName { get; set; }
        public string fileName { get; set; }
    }

    public class ListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class SapMasterValues
    {
        public string site { get; set; }
        public string project { get; set; }
        public string areaid { get; set; }
        public string network { get; set; }
        public string activity { get; set; }
        public string subactivity { get; set; }
        public string area { get; set; }
    }
    public class DropdownValues
    {
        public List<Models.ListItem> location { get; set; }
        public List<Models.ListItem> profile { get; set; }
        public List<Models.ListItem> users { get; set; }
        public List<Models.ListItem> issue_names { get; set; }
        public List<Models.ListItem> site { get; set; }
        public List<Models.ListItem> village { get; set; }
        public List<Models.ListItem> category { get; set; }
        public List<Models.ListItem> project { get; set; }
        public List<Models.ListItem> area { get; set; }
        public List<Models.ListItem> network { get; set; }
        public List<Models.ListItem> activity { get; set; }
        public List<Models.ListItem> subactivity { get; set; }
    }

    public class PostParam
    {
        public string Data { get; set; }
    }
}
