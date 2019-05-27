using Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace SolarPMS.Models
{
    public class UserModel
    {
        public User UserDetail { get; set; }
        public string ProfileName { get; set; }
        public string LocationName { get; set; }
        public string ManagerName { get; set; }        
        /// <summary>
        /// This method used to authenticate user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Authenticate(string userName, string password)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                //solarPMSEntities.Configuration.ProxyCreationEnabled = false;

                User user = solarPMSEntities.Users.Include("ProfileMaster").FirstOrDefault(u => u.UserName == userName && u.Status && (string.IsNullOrEmpty(password) ? true : u.Password == password));
                if (user != null)
                {
                    user.LastLoginDate = DateTime.Now;
                    solarPMSEntities.SaveChanges();
                }
                return user;
            }
        }

        /// <summary>
        /// This method used to get specific user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserInfo(int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                User user = solarPMSEntities.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                    user.Password = Crypto.Instance.Decrypt(user.Password);
                return user;
            }
        }
        
        public static bool GetUserStatus(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                User user = solarPMSEntities.Users.FirstOrDefault(u => u.UserId == UserId);
                if (user != null)
                    return user.Status;
                return false;
            }
        }

        /// <summary>
        /// This method used to get all user details
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUserInfo()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var userDetails = (from user in solarPMSEntities.Users
                                   join manager in solarPMSEntities.Users
                                   on user.ReportingManagerId equals manager.UserId
                                   join profile in solarPMSEntities.ProfileMasters
                                   on user.ProfileId equals profile.ProfileId
                                   //join location in solarPMSEntities.LocationMasters
                                   //on user.LocationId equals location.LocationId
                                   select new UserModel
                                   {
                                       UserDetail = user,
                                       ProfileName = profile.ProfileName,
                                       ManagerName = manager.Name
                                       //LocationName = location.LocationName
                                   }).AsNoTracking().ToList();

                return userDetails;
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
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                user.Password = Crypto.Instance.Encrypt(user.Password);
                user.Status = true;
                user.CreatedBy = userId;
                user.CreatedOn = DateTime.Now;
                user.ModifiedBy = userId;
                user.ModifiedOn = DateTime.Now;
                solarPMSEntities.Users.Add(user);
                solarPMSEntities.SaveChanges();
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                solarPMSEntities.Configuration.LazyLoadingEnabled = false;
                return user;
            }
        }

        /// <summary>
        /// This method used to update user details.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateUserDetail(User userDetail, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                User user = solarPMSEntities.Users.AsNoTracking().FirstOrDefault(u => u.UserId == userDetail.UserId);
                if (user != null)
                {
                    userDetail.Password = Crypto.Instance.Encrypt(userDetail.Password);
                    userDetail.CreatedBy = user.CreatedBy;
                    userDetail.CreatedOn = user.CreatedOn;
                    //userDetail.Status = user.Status;
                    userDetail.IsForgot = user.IsForgot;
                    userDetail.ModifiedBy = userId;
                    userDetail.ModifiedOn = DateTime.Now;
                    solarPMSEntities.Entry(userDetail).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool isUserDisable(int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter IsDisable = new System.Data.Entity.Core.Objects.ObjectParameter("ISDISABLE", typeof(Boolean));
                //return suzlonBPPEntities.BankDetailHistories.FirstOrDefault(B => B.BankDetailId == BankDetailids && B.IFSCCode.Trim().ToLower() == IFSCCode.Trim().ToLower() && B.AccountNumber.Trim().ToLower() == AccNo.Trim().ToLower() && B.IsRecordPushToSAP == true) != null;
                solarPMSEntities.usp_UserDisableValidation(userId, IsDisable);
                return Convert.ToBoolean(IsDisable.Value);
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
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                if (!string.IsNullOrEmpty(userDetail.EmployeeId))
                    fieldStatus.Add("employeeId", solarPMSEntities.Users.AsNoTracking().Where(u => u.EmployeeId == userDetail.EmployeeId && u.UserId != userDetail.UserId).Count() > 0);
                if (!string.IsNullOrEmpty(userDetail.EmailId))
                    fieldStatus.Add("emailId", solarPMSEntities.Users.AsNoTracking().Where(u => u.EmailId == userDetail.EmailId && u.UserId != userDetail.UserId).Count() > 0);
                if (!string.IsNullOrEmpty(userDetail.UserName))
                    fieldStatus.Add("userName", solarPMSEntities.Users.AsNoTracking().Where(u => u.UserName == userDetail.UserName && u.UserId != userDetail.UserId).Count() > 0);
            }
            return fieldStatus;
        }

        public User ForgotPassword(string emailId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var user = solarPMSEntities.Users.FirstOrDefault(l => l.EmailId == emailId);
                var password = string.Empty;
                if (user != null)
                {
                    user.Password = Crypto.Instance.Encrypt(Guid.NewGuid().ToString()); //CommonFunctions.GenerateRandomPassword().ToString();
                    user.IsForgot = true;
                    user.ModifiedBy = user.UserId;
                    user.ModifiedOn = DateTime.Now;
                    solarPMSEntities.SaveChanges();
                    return user;
                }
                else
                    return null;
            }
        }

        public bool ValidateCredentials(string emailId, string password)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.Users.FirstOrDefault(l => l.EmailId == emailId && l.Password == password) != null;
            }
        }

        public string ChangePassword(ChangePasswordModel changePasswordModel)
        {
            string message = string.Empty;
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                changePasswordModel.OldPassword = Crypto.Instance.Encrypt(changePasswordModel.OldPassword);
                changePasswordModel.Password = Crypto.Instance.Encrypt(changePasswordModel.Password);
                var user = solarPMSEntities.Users.FirstOrDefault(l => l.UserId == changePasswordModel.UserId && l.Password == changePasswordModel.OldPassword);
                if (user != null)
                {
                    if (changePasswordModel.Password != user.Password)
                    {
                        user.Password = changePasswordModel.Password;
                        user.IsForgot = false;
                        user.ModifiedBy = user.UserId;
                        user.ModifiedOn = DateTime.Now;
                        solarPMSEntities.SaveChanges();
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

        public Hashtable GetUserMenus()
        {
            int profileID = Convert.ToInt32(HttpContext.Current.Session["ProfileId"]);
            Hashtable menuAccess = new Hashtable();

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var menuIDs = (from ma in solarPMSEntities.MenuAccesses
                               join me in solarPMSEntities.Menus on ma.MenuId equals me.MenuId
                               where ma.ProfileId == profileID
                               select new
                               {
                                   Access = true,
                                   menuName = me.MenuName
                               }).ToList();

                if (menuIDs.Count > 0)
                {

                    menuIDs.ForEach(ma =>
                    {
                        if (!menuAccess.ContainsKey(ma.menuName))
                            menuAccess.Add(ma.menuName, ma.Access);
                    });
                }
            }
            return menuAccess;
        }

        public static string GetDEDocumentViewAccessProfiles()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<ProfileMaster> profiles = solarPMSEntities.ProfileMasters.Where(s => s.DEDocumentUploadAccess == 1).ToList();
                if (profiles != null && profiles.Count > 0)
                {
                    List<string> lstProfileName = profiles.Select(p => p.ProfileName).ToList();
                    return string.Join(",", lstProfileName);
                }
            }
            return string.Empty;
        }
        public bool SendEmail(User user)
        {
            bool result = true;
            try
            {
                string url = ConfigurationManager.AppSettings["MailUrl"] + "/ResetPassword.aspx?" +
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

        [JsonProperty("profilename")]
        public string ProfileName { get; set; }

        [JsonProperty("DocumentAccess")]
        public int DocumentAccess { get; set; }
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
        public const string DASHBOARD = "Dashboards";
        public const string TODOTASK = "To do List";
        public const string ISSUEMANAGEMENT = "Issue Management";
        public const string USERMANAGEMENT = "User Management";
        public const string MANPOWER = "Contractor Management";

        public static bool IsAccessGranted(string attribute, Hashtable menuList)
        {
            if (menuList != null && menuList.ContainsKey(attribute) && menuList[attribute].ToString() == "True")
                return true;
            else
                return false;
        }
    }
}
