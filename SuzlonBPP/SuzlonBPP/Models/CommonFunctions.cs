using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Data.Entity;
using System.Reflection;

namespace SuzlonBPP.Models
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

                PostParam paramObject = new PostParam();

                if (!string.IsNullOrEmpty(parameter))
                {
                    paramObject.Data = parameter;
                }
                StringContent str = new StringContent(JsonConvert.SerializeObject(paramObject));
                var response = ((string.IsNullOrEmpty(parameter)) ? client.GetAsync(url).GetAwaiter().GetResult() : client.PostAsJsonAsync(new Uri(Constants.RESTURL + url), paramObject).GetAwaiter().GetResult());
                string responseJson = Constants.REST_CALL_FAILURE;
                if (response.IsSuccessStatusCode)
                    responseJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return responseJson;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                return Constants.REST_CALL_FAILURE;
            }
        }

        public delegate void CallbackHandler(string str);

        public async void RestServiceCallSync(string url, string parameter, CallbackHandler callback)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(Constants.RESTURL);
                // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                if (HttpContext.Current.Session["Token"] != null)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Current.Session["Token"] as string);
                }
                else
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));



                StringContent str = new StringContent(parameter);

                var response = await ((string.IsNullOrEmpty(parameter)) ? client.GetAsync(url) : client.PostAsync(new Uri(Constants.RESTURL + url), str));
                var responseJson = response.Content.ReadAsStringAsync().Result;
                try
                {
                    callback(responseJson);
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex);

                }


            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
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
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                    names.Split(',').ToList().ForEach(name =>
                    {
                        switch (name)
                        {
                            case "profile":
                                {
                                    ddValues.Profile = new List<ListItem>();
                                    suzlonBPPEntities.ProfileMasters.AsNoTracking().Where(p => p.Status).OrderBy(s => s.ProfileName).ToList().ForEach(l =>
                                    {
                                        ddValues.Profile.Add(new ListItem() { Id = l.ProfileId.ToString(), Name = l.ProfileName });
                                    });

                                    break;
                                }
                            case "users":
                                {
                                    ddValues.Users = new List<ListItem>();
                                    suzlonBPPEntities.Users.AsNoTracking().Where(p => p.Status).OrderBy(s => s.Name).ToList().ForEach(l =>
                                    {
                                        ddValues.Users.Add(new ListItem() { Id = l.UserId.ToString(), Name = l.UserName });
                                    });

                                    break;
                                }
                            case "vertical":
                                {
                                    ddValues.Vertical = new List<ListItem>();
                                    suzlonBPPEntities.VerticalMasters.AsNoTracking().Where(p => p.Status).OrderBy(s => s.Name).ToList().ForEach(l =>
                                    {
                                        ddValues.Vertical.Add(new ListItem() { Id = l.VerticalId.ToString(), Name = l.Name });
                                    });

                                    break;
                                }
                            case "subvertical":
                                {
                                    ddValues.SubVertical = new List<ListItem>();
                                    suzlonBPPEntities.SubVerticalMasters.AsNoTracking().Where(p => p.Status).OrderBy(s => s.Name).ToList().ForEach(l =>
                                      {
                                          ddValues.SubVertical.Add(new ListItem() { Id = l.SubVerticalId.ToString(), Name = l.Name });
                                      });

                                    break;
                                }
                            case "company":
                                {
                                    ddValues.Company = new List<ListItem>();
                                    (from c in suzlonBPPEntities.VendorMasters
                                     select new ListItem()
                                     {
                                         Id = c.CompanyCode,
                                         Name = c.CompanyName                                         
                                     }).Distinct().OrderBy(c => c.Name).ToList().ForEach(item =>
                                      {
                                          if (!string.IsNullOrEmpty(item.Id))
                                              ddValues.Company.Add(item);
                                      });

                                    break;
                                }
                            case "vendorcode":
                                {
                                    ddValues.VendorCode = new List<ListItem>();
                                    (from v in suzlonBPPEntities.VendorMasters select new ListItem() { Id = v.VendorCode, Name = v.VendorName }).Distinct().OrderBy(v => v.Name).
                                    ToList().ForEach(item =>
                                    {
                                        ddValues.VendorCode.Add(item);
                                    });

                                    break;
                                }
                            case "workflow":
                                {
                                    ddValues.WorkFlow = new List<ListItem>();
                                    suzlonBPPEntities.WorkflowConfigurations.AsNoTracking().OrderBy(s => s.WorkflowName).ToList().ForEach(c =>
                                    {
                                        ddValues.WorkFlow.Add(new ListItem() { Id = Convert.ToString(c.WorkFlowTypeId), Name = c.WorkflowName });
                                    });

                                    break;
                                }
                            case "verticalcontroller":
                                {
                                    ddValues.VerticalController = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Vertical Controller"
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                              {
                                                  ddValues.VerticalController.Add(item);
                                              });
                                    break;
                                }
                            case "groupcontroller":
                                {
                                    ddValues.GroupController = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Group Controller"
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.GroupController.Add(item);
                                     });
                                    break;
                                }
                            case "treasury":
                                {
                                    ddValues.Treasury = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Treasury"
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.Treasury.Add(item);
                                     });
                                    break;
                                }
                            case "managementassurance":
                                {
                                    ddValues.ManagementAssurance = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Management Assurance"
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.ManagementAssurance.Add(item);
                                     });
                                    break;
                                }
                            case "fassc":
                                {
                                    ddValues.FASSC = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "F&A SSC"
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.FASSC.Add(item);
                                     });
                                    break;
                                }
                            case "cb":
                                {
                                    ddValues.CB = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "C&B"
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.CB.Add(item);
                                     });
                                    break;
                                }
                            case "payment-proposer":
                                {
                                    ddValues.PaymentProposer = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Payment Proposer"
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.PaymentProposer.Add(item);
                                     });
                                    break;
                                }
                            case "subvertical-vertical":
                                {
                                    ddValues.SubVerticalWithVertical = new List<ListItem>();
                                    (from subVertical in suzlonBPPEntities.SubVerticalMasters
                                     join vertical in suzlonBPPEntities.VerticalMasters
                                     on subVertical.VerticalId equals vertical.VerticalId
                                     where subVertical.Status == true
                                     select new ListItem()
                                     {
                                         Id = subVertical.SubVerticalId + Constants.SEPERATOR + vertical.Name,
                                         Name = subVertical.Name
                                     }).OrderBy(s => s.Name).OrderBy(v => v.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.SubVerticalWithVertical.Add(item);
                                     });
                                    break;
                                }
                            case "workflow-status":
                                {
                                    ddValues.WorkFlowStatus = new List<ListItem>();
                                    ddValues.WorkFlowStatus.Add(new ListItem() { Id = "1", Name = "Approved" });
                                    ddValues.WorkFlowStatus.Add(new ListItem() { Id = "2", Name = "Rejected" });
                                    ddValues.WorkFlowStatus.Add(new ListItem() { Id = "3", Name = "Need Correction" });

                                    //ddValues.WorkFlowStatus = new List<ListItem>();
                                    //(from c in suzlonBPPEntities.StatusMasters
                                    // where c.ShowInDropDown == false
                                    // select new ListItem()
                                    // {
                                    //     Id = Convert.ToString(c.StatusId),
                                    //     Name = c.Status
                                    // }).ToList().ForEach(item =>
                                    // {
                                    //     ddValues.WorkFlowStatus.Add(item);
                                    // });

                                    break;
                                }
                            case "account-type":
                                {
                                    ddValues.AccountType = new List<ListItem>();

                                    suzlonBPPEntities.AccountTypes.AsNoTracking().OrderBy(a => a.Type).ToList().ForEach(c =>
                                    {
                                        ddValues.AccountType.Add(new ListItem() { Id = Convert.ToString(c.Type), Name = Convert.ToString(c.Description) });
                                    });
                                    break;
                                }
                            case "nature-of-request":
                                {
                                    ddValues.NatureOfRequest = new List<ListItem>();
                                    (from r in suzlonBPPEntities.NatureRequestMasters
                                     select new ListItem()
                                     {
                                         Id = r.RequestId.ToString(),
                                         Name = r.Name
                                     }).Distinct().OrderBy(c => c.Name).ToList().ForEach(item =>
                                     {
                                         if (!string.IsNullOrEmpty(item.Id))
                                             ddValues.NatureOfRequest.Add(item);
                                     });

                                    break;
                                }

                            case "nature-of-request-automatic":
                                {
                                    ddValues.NatureOfRequest = new List<ListItem>();
                                    (from r in suzlonBPPEntities.NatureRequestMasters.Where(r => r.Type == "NEFT")
                                     select new ListItem()
                                     {
                                         Id = r.RequestId.ToString(),
                                         Name = r.Name
                                     }).Distinct().OrderBy(c => c.Name).ToList().ForEach(item =>
                                     {
                                         if (!string.IsNullOrEmpty(item.Id))
                                             ddValues.NatureOfRequest.Add(item);
                                     });

                                    break;
                                }
                            case "nature-of-request-manual":
                                {
                                    ddValues.NatureOfRequest = new List<ListItem>();
                                    (from r in suzlonBPPEntities.NatureRequestMasters.Where(r => r.Type == "Others")
                                     select new ListItem()
                                     {
                                         Id = r.RequestId.ToString(),
                                         Name = r.Name
                                     }).Distinct().OrderBy(c => c.Name).ToList().ForEach(item =>
                                     {
                                         if (!string.IsNullOrEmpty(item.Id))
                                             ddValues.NatureOfRequest.Add(item);
                                     });

                                    break;
                                }
                           

                        }
                    });
                }
            }
            return ddValues;
        }

        public DropdownValues PopulateDropdownsByUser(string names,int id)

        {
            DropdownValues ddValues = new DropdownValues();
            if (!string.IsNullOrEmpty(names))
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                    names.Split(',').ToList().ForEach(name =>
                    {
                        switch (name)
                        {
                           
                            case "nature-of-request":
                                {
                                    ddValues.NatureOfRequest = new List<ListItem>();
                                    (from r in suzlonBPPEntities.NatureRequestMasters
                                     select new ListItem()
                                     {
                                         Id = r.RequestId.ToString(),
                                         Name = r.Name
                                     }).Distinct().OrderBy(c => c.Name).ToList().ForEach(item =>
                                     {
                                         if (!string.IsNullOrEmpty(item.Id))
                                             ddValues.NatureOfRequest.Add(item);
                                     });

                                    break;
                                }

                            case "nature-of-request-automatic":
                                {
                                    ddValues.NatureOfRequest = new List<ListItem>();
                                    (from r in suzlonBPPEntities.NatureRequestMasters.Where(r => r.Type == "NEFT")
                                     select new ListItem()
                                     {
                                         Id = r.RequestId.ToString(),
                                         Name = r.Name
                                     }).Distinct().OrderBy(c => c.Name).ToList().ForEach(item =>
                                     {
                                         if (!string.IsNullOrEmpty(item.Id))
                                             ddValues.NatureOfRequest.Add(item);
                                     });

                                    break;
                                }
                            case "nature-of-request-manual":
                                {
                                    ddValues.NatureOfRequest = new List<ListItem>();
                                    (from r in suzlonBPPEntities.NatureRequestMasters.Where(r => r.Type == "Others")
                                     select new ListItem()
                                     {
                                         Id = r.RequestId.ToString(),
                                         Name = r.Name
                                     }).Distinct().OrderBy(c => c.Name).ToList().ForEach(item =>
                                     {
                                         if (!string.IsNullOrEmpty(item.Id))
                                             ddValues.NatureOfRequest.Add(item);
                                     });

                                    break;
                                }
                            case "usercompany":
                                {

                                    ddValues.Company = new List<ListItem>();
                                    String CompanyIds = suzlonBPPEntities.Users.Where(p => p.UserId.Equals(id)).FirstOrDefault().Company;
                                    //String[] arrCompanyIds = CompanyIds.Split(',');

                                    var arrCompanyIds = new List<string>(CompanyIds.Split(','));//CompanyIds.Split(',').Select(int.Parse).ToList(); ;//

                                    //suzlonBPPEntities.VendorMasters.Where(p => arrCompanyIds.Contains(p.CompanyCode)).OrderBy(s => s.CompanyName).Distinct().ToList().ForEach(l =>
                                    //{
                                    //    ddValues.Company.Add(new ListItem() { Id = l.CompanyCode.ToString(), Name = l.CompanyName });
                                    //});
                                    (from V in suzlonBPPEntities.VendorMasters                                     
                                     where CompanyIds.Contains(V.CompanyCode)
                                     select new ListItem()
                                     {
                                         Id = V.CompanyCode.ToString(),
                                         Name = V.CompanyName
                                     }).OrderBy(s => s.Name).Distinct().ToList().ForEach(item =>
                                     {
                                         if(item!=null)
                                         ddValues.Company.Add(item);
                                     });





                                    break;
                                }
                            case "usersubvertical":
                                {

                                    ddValues.SubVertical = new List<ListItem>();
                                    String SubverticalIds = suzlonBPPEntities.Users.Where(p => p.UserId.Equals(id)).FirstOrDefault().SubVertical;
                                    //String[] arrCompanyIds = CompanyIds.Split(',');

                                    var arrSubVerticalIds = SubverticalIds.Split(',').Select(int.Parse).ToList();

                                    suzlonBPPEntities.SubVerticalMasters.Where(p => arrSubVerticalIds.Contains(p.SubVerticalId)).OrderBy(s => s.Name).ToList().ForEach(l =>
                                    {
                                        ddValues.SubVertical.Add(new ListItem() { Id = l.SubVerticalId.ToString(), Name = l.Name });
                                    });
                                    break;
                                }

                        }
                    });
                }
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
        public static bool SendEmail(string toEmail, string fromEmail, string emailBody, string emailSubject, List<string> strBCCList = null, String AttachmentPath = "", string ccEmail = null)
        {
            bool result = true;
            string strFrompwd = "";

            try
            {
                string fromAlias = System.Configuration.ConfigurationManager.AppSettings["FromAlias"];
                strFrompwd = System.Configuration.ConfigurationManager.AppSettings["AdminMailPassword"];
                string strUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUSERNAME"];
                if (string.IsNullOrEmpty(fromEmail))
                    fromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"];

                SmtpClient smtp_mail = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPHOST"]);
                smtp_mail.Credentials = new System.Net.NetworkCredential(strUsername, strFrompwd);

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(fromEmail, fromAlias);
                mail.IsBodyHtml = true;
                mail.Subject = emailSubject;
                mail.Body = emailBody;

                //3.Prepare the Destination email Addresses list
                foreach (var curr_address in toEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(curr_address);
                }

                if (!string.IsNullOrEmpty(ccEmail))
                {
                    foreach (var curr_address in ccEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.CC.Add(curr_address);
                    }
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

                smtp_mail.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPORT"]);
                smtp_mail.EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableSsl"]);

                smtp_mail.Send(mail);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex, 0);
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

        public static void WriteErrorLog(Exception ex)
        {
            WriteErrorLog(ex, Convert.ToInt32(HttpContext.Current.Session["UserId"]));
        }

        /// <summary>
        /// Write exception details to database.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="errorDetails"></param>
        public static void WriteErrorLog(Exception ex, int userId)
        {
            try
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    var trace = new StackTrace(ex);
                    var frame = trace.GetFrame(0);
                    var method = frame.GetMethod();
                    suzlonBPPEntities.ErrorLogs.Add(new ErrorLog()
                    {
                        FunctionName = method.Name,
                        Page = method.DeclaringType.FullName,
                        Message = ex.Message + (ex.InnerException != null ? "," + ex.InnerException.ToString() : string.Empty),
                        StackTrace = ex.StackTrace,
                        UserId = userId,
                        CreatedOn = DateTime.Now
                    });
                    suzlonBPPEntities.SaveChanges();
                }
            }
            catch (Exception exception)
            {

            }
        }

        public static void WriteErrorLogMobile(string Message, string StackTrace, string PageName, string FunctionName, int userId)
        {
            try
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {

                    suzlonBPPEntities.ErrorLogs.Add(new ErrorLog()
                    {
                        FunctionName = FunctionName,
                        Page = PageName,
                        Message = Message,
                        StackTrace = StackTrace,
                        UserId = userId,
                        CreatedOn = DateTime.Now
                    });
                    suzlonBPPEntities.SaveChanges();
                }
            }
            catch (Exception exception)
            {

            }
        }

        public static List<Models.ListItem> GetNatureTypes()
        {
            List<Models.ListItem> types = new List<ListItem>();
            //types.Add(new ListItem() { Id = "Automatic", Name = "Automatic" });
            //types.Add(new ListItem() { Id = "Manual", Name = "Manual" });
            types.Add(new ListItem() { Id = "NEFT", Name = "NEFT" });
            types.Add(new ListItem() { Id = "Others", Name = "Others" });
            return types;
        }


        public DropdownValues PopulateBankWorkflowDropdowns(string names, string subVertical)

        {
            DropdownValues ddValues = new DropdownValues();
            if (!string.IsNullOrEmpty(names))
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                    names.Split(',').ToList().ForEach(name =>
                    {
                        switch (name)
                        {

                            case "verticalcontroller":
                                {
                                    ddValues.VerticalController = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Vertical Controller" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.VerticalController.Add(item);
                                     });
                                    break;
                                }
                            case "groupcontroller":
                                {
                                    ddValues.GroupController = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Group Controller" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.GroupController.Add(item);
                                     });
                                    break;
                                }
                            case "treasury":
                                {
                                    ddValues.Treasury = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Treasury" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.Treasury.Add(item);
                                     });
                                    break;
                                }
                            case "managementassurance":
                                {
                                    ddValues.ManagementAssurance = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Management Assurance" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.ManagementAssurance.Add(item);
                                     });
                                    break;
                                }
                            case "fassc":
                                {
                                    ddValues.FASSC = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "F&A SSC" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.FASSC.Add(item);
                                     });
                                    break;
                                }
                            case "cb":
                                {
                                    ddValues.CB = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "C&B" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.CB.Add(item);
                                     });
                                    break;
                                }
                            case "payment-proposer":
                                {
                                    ddValues.PaymentProposer = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Payment Proposer" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.PaymentProposer.Add(item);
                                     });
                                    break;
                                }


                            case "aggregator":
                                {
                                    ddValues.Aggregator = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Vertical Aggregator" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.Aggregator.Add(item);
                                     });
                                    break;
                                }
                            case "controller":
                                {
                                    ddValues.Controller = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Vertical Controller" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.Controller.Add(item);
                                     });
                                    break;
                                }
                            case "exceptional-approver":
                                {
                                    ddValues.ExceptionalApprover = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Exceptional Approver" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.ExceptionalApprover.Add(item);
                                     });
                                    break;
                                }
                            case "auditor":
                                {
                                    ddValues.Auditor = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "Auditor" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.Auditor.Add(item);
                                     });
                                    break;
                                }
                            case "FASSC-DT":
                                {
                                    ddValues.FASSCDT = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "F&A SSC-DT" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.FASSCDT.Add(item);
                                     });
                                    break;
                                }
                            case "FASSC-CB":
                                {
                                    ddValues.FASSCCB = new List<ListItem>();
                                    (from user in suzlonBPPEntities.Users
                                     join profile in suzlonBPPEntities.ProfileMasters
                                     on user.ProfileId equals profile.ProfileId
                                     where profile.ProfileName == "C&B" && ("," + user.SubVertical + ",").Contains("," + subVertical + ",")
                                     && user.Status == true
                                     select new ListItem()
                                     {
                                         Id = user.UserId.ToString(),
                                         Name = user.Name
                                     }).OrderBy(s => s.Name).ToList().ForEach(item =>
                                     {
                                         ddValues.FASSCCB.Add(item);
                                     });
                                    break;
                                }
                        }
                    });
                }
            }
            return ddValues;
        }

        public dynamic getFileUploads(int EntityID, string EntityName)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                //return suzlonBPPEntities.FileUploads.Where(s => s.EntityId == EntityID).Where(s => s.IsDeleted == false).Where(s => s.EntityName == EntityName)
                //                                               .OrderBy(n => n.ModifiedOn).ToList();
                var uploadfiles = (from upl in suzlonBPPEntities.FileUploads
                              join u in suzlonBPPEntities.Users on upl.CreatedBy equals u.UserId
                              where upl.EntityId == EntityID && upl.IsDeleted == false && upl.EntityName == EntityName
                              orderby upl.ModifiedBy descending
                              select new
                              {
                                  upl.FileUploadId,
                                  upl.EntityId,
                                  upl.EntityName,
                                  upl.FileName,
                                  upl.DisplayName,
                                  upl.CreatedBy,
                                  upl.CreatedOn,
                                  upl.ModifiedBy,
                                  upl.ModifiedOn,
                                  upl.IsDeleted,
                                  upl.DocumentType,
                                  u.Name 
                              }).ToList();

                return uploadfiles;
            }

        }

        public string AddFileUpload(FileUpload fileUpload)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.FileUploads.Add(fileUpload);
                int count = suzlonBPPEntities.SaveChanges();
                fileUpload.FileName = fileUpload.FileName.Replace(fileUpload.CreatedBy + "!", fileUpload.FileUploadId + "!");
                count = suzlonBPPEntities.SaveChanges();
                return Convert.ToString(fileUpload.FileUploadId);
            }
        }

        public bool DeleteUploadedFile(int Id, int userid)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                FileUpload FileObj = suzlonBPPEntities.FileUploads.FirstOrDefault(s => s.FileUploadId == Id);
                if (FileObj != null)
                {
                    FileObj.IsDeleted = true;
                    FileObj.ModifiedBy = userid;
                    suzlonBPPEntities.Entry(FileObj).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public static List<GetItemsForExportToExcel_Result> GetItemsForExportToExcel(int VerticalId, int UserId, string BillType, string TabName)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.sp_GetItemsForExportToExcel(VerticalId, UserId, BillType, TabName).ToList();
            }
        }

        public static DataTable ConvertListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static void ExporttoExcel(DataTable table, HttpResponse Response, string FileName)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            if (table.Rows.Count > 0)
            {
                GridView GridView_Result = new GridView();
                GridView_Result.DataSource = table;
                GridView_Result.DataBind();
                GridView_Result.RenderControl(htw);
            }
            Response.Write(sw.ToString());
            Response.End();
        }
    }

    [Serializable]
    public class ListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


    public class DropdownValues
    {
        public List<Models.ListItem> Profile { get; set; }
        public List<Models.ListItem> Users { get; set; }
        public List<Models.ListItem> Company { get; set; }
        public List<Models.ListItem> Vertical { get; set; }
        public List<Models.ListItem> SubVertical { get; set; }
        public List<Models.ListItem> VendorCode { get; set; }
        public List<Models.ListItem> VendorName { get; set; }
        public List<Models.ListItem> WorkFlow { get; set; }
        public List<Models.ListItem> VerticalController { get; set; }
        public List<Models.ListItem> GroupController { get; set; }
        public List<Models.ListItem> Treasury { get; set; }
        public List<Models.ListItem> ManagementAssurance { get; set; }
        public List<Models.ListItem> FASSC { get; set; }
        public List<Models.ListItem> CB { get; set; }
        public List<Models.ListItem> Aggregator { get; set; }
        public List<Models.ListItem> Controller { get; set; }
        public List<Models.ListItem> ExceptionalApprover { get; set; }
        public List<Models.ListItem> Auditor { get; set; }
        public List<Models.ListItem> FASSCDT { get; set; }
        public List<Models.ListItem> FASSCCB { get; set; }
        public List<Models.ListItem> PaymentProposer { get; set; }
        public List<Models.ListItem> SubVerticalWithVertical { get; set; }
        public List<Models.ListItem> WorkFlowStatus { get; set; }
        public List<Models.ListItem> AccountType { get; set; }
        public List<Models.ListItem> NatureOfRequest { get; set; }
    }

    public class PostParam
    {
        public string Data { get; set; }
    }
    public class VendorSearch
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string City { get; set; }
        public string Region { get; set; }

    }

}
