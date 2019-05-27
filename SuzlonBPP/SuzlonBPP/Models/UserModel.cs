using Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Web;

namespace SuzlonBPP.Models
{
    public class UserModel
    {
        public User UserDetail { get; set; }
        public string ProfileName { get; set; }

        public string CompanyName { get; set; }
        public string Vertical { get; set; }
        public string SubVertical { get; set; }
        public List<GetVendorNameBasedOnCode_Result> VendorDetails { get; set; }

        /// <summary>
        /// This method used to authenticate user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Authenticate(string userName, string password)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.Users.Include("ProfileMaster").FirstOrDefault(u => u.UserName == userName && u.Status && (string.IsNullOrEmpty(password) ? true : u.Password == password));
            }
        }

        /// <summary>
        /// This method used to get specific user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserModel GetUserInfo(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                UserModel userModel = new UserModel();
                User user = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    user.Password = Crypto.Instance.Decrypt(user.Password);
                    if (!string.IsNullOrEmpty(user.VendorCode))
                        userModel.VendorDetails = suzlonBPPEntities.GetVendorNameBasedOnCode(user.VendorCode).ToList();
                }
                userModel.UserDetail = user;
                return userModel;
            }
        }

        /// <summary>
        /// This method used to get all user details
        /// </summary>
        /// <returns></returns>
        public List<GetAllUserDetail_Result> GetAllUserInfo()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetAllUserDetail().ToList();
            }
        }

        /// <summary>
        /// This method used to add user details
        /// </summary>
        /// <param name="locationMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User AddUser(User user, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                user.Password = Crypto.Instance.Encrypt(user.Password);
                user.Status = true;
                user.CreatedBy = userId;
                user.CreatedOn = DateTime.Now;
                user.ModifiedBy = userId;
                user.ModifiedOn = DateTime.Now;
                suzlonBPPEntities.Users.Add(user);
                suzlonBPPEntities.SaveChanges();
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                suzlonBPPEntities.Configuration.LazyLoadingEnabled = false;
                SendMailToUserRegistration(user);
                return user;
            }
        }

        private void SendMailToUserRegistration(User user)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("Dear " + user.Name + ",<br><br>Welcome!<br><br>You are newest user of our Bank Payment Portal.<br><br>The Authentication details are as below.");
            mailBody.AppendFormat("<br><br>URL: " + ConfigurationManager.AppSettings["WebUrlForMail"]);
            mailBody.AppendFormat("<br>User Name: " + user.UserName);
            if (user.ProfileId == (int)UserProfileEnum.Vendor)
                mailBody.AppendFormat("<br>Password: " + Crypto.Instance.Decrypt(user.Password));
            else
                mailBody.AppendFormat("<br>Password: Your Active Directory Password");

            mailBody.AppendFormat("<br><br>Click <a href='{0}'>here</a> to download Mobile Application.", ConfigurationManager.AppSettings["MobileAppUrlForMail"]);
            mailBody.AppendFormat("<br><br> Thanks & Regards,<br> Support Team<br>"+ ConfigurationManager.AppSettings["ApplicationName"]);

            string mailSubject = ConfigurationManager.AppSettings["ApplicationName"] + " - New user creation";

            CommonFunctions.SendEmail(user.EmailId, string.Empty, mailBody.ToString(), mailSubject, null, "");
        }

        /// <summary>
        /// This method used to update user details.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateUserDetail(User userDetail, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                User user = suzlonBPPEntities.Users.AsNoTracking().FirstOrDefault(u => u.UserId == userDetail.UserId);
                if (user != null)
                {
                    userDetail.Password = Crypto.Instance.Encrypt(userDetail.Password);
                    userDetail.CreatedBy = user.CreatedBy;
                    userDetail.CreatedOn = user.CreatedOn;
                    userDetail.IsForgot = user.IsForgot;
                    userDetail.ModifiedBy = userId;
                    userDetail.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(userDetail).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// This method used to check existence of fields against db table.
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        public Dictionary<string, bool> CheckExistance(User userDetail)
        {
            Dictionary<string, bool> fieldStatus = new Dictionary<string, bool>();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                if (!string.IsNullOrEmpty(userDetail.EmployeeId))
                    fieldStatus.Add("employeeId", suzlonBPPEntities.Users.AsNoTracking().Where(u => u.EmployeeId == userDetail.EmployeeId && u.UserId != userDetail.UserId).Count() > 0);
                if (!string.IsNullOrEmpty(userDetail.EmailId))
                    fieldStatus.Add("emailId", suzlonBPPEntities.Users.AsNoTracking().Where(u => u.EmailId == userDetail.EmailId && u.UserId != userDetail.UserId).Count() > 0);
                if (!string.IsNullOrEmpty(userDetail.UserName))
                    fieldStatus.Add("userName", suzlonBPPEntities.Users.AsNoTracking().Where(u => u.UserName == userDetail.UserName && u.UserId != userDetail.UserId).Count() > 0);
            }
            return fieldStatus;
        }

        public User ForgotPassword(string emailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var user = suzlonBPPEntities.Users.FirstOrDefault(l => l.EmailId == emailId);
                var password = string.Empty;
                if (user != null)
                {
                    user.Password = Crypto.Instance.Encrypt(Guid.NewGuid().ToString()); //CommonFunctions.GenerateRandomPassword().ToString();
                    user.IsForgot = true;
                    user.ModifiedBy = user.UserId;
                    user.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.SaveChanges();
                    return user;
                }
                else
                    return null;
            }
        }

        public bool ValidateCredentials(string emailId, string password)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.Users.FirstOrDefault(l => l.EmailId == emailId && l.Password == password) != null;
            }
        }

        public string ADAuthentication(string emailId, string password)
        {
            string dominName = string.Empty;
            string adPath = ConfigurationManager.AppSettings["DirectoryPath"];
            string userName = emailId.Trim().ToUpper();
            DirectoryEntry entry = new DirectoryEntry(adPath);
            entry.AuthenticationType = AuthenticationTypes.Secure;
            entry.Username = userName;
            entry.Password = password;

            try
            {
                string strSearch = "SAMAccountName=" + userName;
                DirectorySearcher dsSystem = new DirectorySearcher(entry, strSearch);
                SearchResult result = dsSystem.FindOne();

                if (null == result)
                {
                    return string.Empty;
                }
                // Update the new path to the user in the directory
                adPath = result.Path;
                string _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                return ex.Message;
                CommonFunctions.WriteErrorLog(ex);
            }
            return Constants.SUCCESS;
        }

        public string ChangePassword(ChangePasswordModel changePasswordModel)
        {
            string message = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                changePasswordModel.OldPassword = Crypto.Instance.Encrypt(changePasswordModel.OldPassword);
                changePasswordModel.Password = Crypto.Instance.Encrypt(changePasswordModel.Password);
                var user = suzlonBPPEntities.Users.FirstOrDefault(l => l.UserId == changePasswordModel.UserId && l.Password == changePasswordModel.OldPassword);
                if (user != null)
                {
                    if (changePasswordModel.Password != user.Password)
                    {
                        user.Password = changePasswordModel.Password;
                        user.IsForgot = false;
                        user.ModifiedBy = user.UserId;
                        user.ModifiedOn = DateTime.Now;
                        suzlonBPPEntities.SaveChanges();
                        message = Constants.PASSWORD_UPATED;
                    }
                    else
                        message = Constants.OLD_PASSWORD_SHOULD_NOT_SAME;
                }
                else
                    message = Constants.OLD_PASSWORD_NOT_MATCH;

            }
            return message;
        }

        public bool IsUserAssignedToWorkflow(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lstBankWorkflow = suzlonBPPEntities.BankWorkflows.Where(w =>
                w.PriVerContUserId == userId
                || w.PriGrpContUserId == userId
                || w.PriTreasuryUserId == userId
                || w.PriMgmtAssUserId == userId
                || w.PriFASSCUserId == userId
                || w.PriCBUserId == userId
                || w.SecVerContUserId == userId
                || w.SecGrpContUserId == userId
                || w.SecTreasuryUserId == userId
                || w.SecMgmtAssUserId == userId
                || w.SecFASSCUserId == userId
                || w.SecCBUserId == userId).ToList();

                if (lstBankWorkflow == null || lstBankWorkflow.Count == 0)
                    return false;
                else
                    return true;
            }
        }

        public Hashtable GetUserMenus()
        {
            int profileID = Convert.ToInt32(HttpContext.Current.Session["ProfileId"]);
            Hashtable menuAccess = new Hashtable();

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var menuIDs = (from ma in suzlonBPPEntities.MenuAccesses
                               join me in suzlonBPPEntities.Menus on ma.MenuId equals me.MenuId
                               where ma.ProfileId == profileID
                               select new
                               {
                                   Access = true,
                                   menuName = me.MenuName
                               }).ToList();

                if (menuIDs.Count > 0)
                {
                    menuIDs.ForEach(ma => { menuAccess.Add(ma.menuName, ma.Access); });
                }
            }
            return menuAccess;
        }

        public bool SendEmail(User user)
        {
            bool result = true;
            try
            {
                string url = ConfigurationManager.AppSettings["WebsiteUrl"] + "/ResetPassword.aspx?" +
                                    HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("userid=" + user.UserId + "&emailId=" + user.EmailId + "&password=" + user.Password));
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append("Dear User,<br><br>We received a request to reset the password associated with this email address.<br><br>");
                mailBody.AppendFormat("Click the below link to reset your password: <br><a href='{0}'>{1}</a> <br><br>", url, url);
                mailBody.AppendFormat("If you have any comments or questions, please reach us {0}", ConfigurationManager.AppSettings["SupportEmail"]);
                mailBody.AppendFormat("<br><br><br> Thank You,<br> ", ConfigurationManager.AppSettings["ApplicationName"]);

                string mailSubject = ConfigurationManager.AppSettings["ApplicationName"] + " - Forgot password";

                CommonFunctions.SendEmail(user.EmailId, string.Empty, mailBody.ToString(), mailSubject, null, "");

            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
    }
    public class TokenResponseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("userid")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("profilename")]
        public string ProfileName { get; set; }

        [JsonProperty("profileid")]
        public int ProfileId { get; set; }
    }

    public class ChangePasswordModel
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public static class PageSecurity
    {
        //public const string BANKDETAILSUPDATION = "Bank Details Updation";
        //public const string BUDGETALLOCATION = "Budget Allocation";
        //public const string VENDORPAYMENTS = "Vendor Payments";
        //public const string REPORTS = "Reports";
        //public const string ADMINCONFIGURATION = "Admin Configurations";


        public const string BANKDETAILLIST = "Bank Detail List";
        public const string VENDORBANKDETAILS = "Vendor Bank Details(Validator)";

        public const string VERTICALCONTROLLER = "Vertical Controller";
        public const string TREASURY = "Treasury";

        public const string AGAINSTBILLINITIATOR_INITIATOR= "Against Bill Initiator(Initiator)";
        public const string ADVANCE_INITIATOR = "Advance(Initiator)";
        public const string AGAINSTBILLINITIATOR_AGGREGATOR = "Against Bill Initiator(Aggregator)";
        public const string ADVANCE_AGGREGATOR = "Advance(Aggregator)";
        public const string F_AND_A_APPROVER= "FnAApprover";


        public const string REPORTS = "Reports";

        public const string USER_MANAGEMENT = "User Management";

        public const string PROFILE_MASTER = "Profile Master";
        public const string COMPANY_MASTER = "Comapny Master";
        public const string VERTICAL_MASTER = "Vertical Master";
        public const string SUBVERTICAL_MASTER = "Sub-Vertical Master";
        public const string NATURE_OF_REQUEST = "Nature of Request";
        public const string VENDOR_MASTER = "Vendor Master";
        public const string VENDOR_BANK_MASTER = "Vendor Bank Master";
        public const string IFSC_CODE_MASTER = "IFSC Code Master";

        public const string WORKFLOW_CONFIGURATION = "Workflow Configuration";
        public const string APPLICATION_CONFIGURATION = "Application Configuration";

        public static bool IsAccessGranted(string attribute, Hashtable menuList)
        {
            if (menuList.ContainsKey(attribute) && menuList[attribute].ToString() == "True")
                return true;
            else
                return false;
        }
    }
}
