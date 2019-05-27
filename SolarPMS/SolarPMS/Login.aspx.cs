using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Web;
using System.Web.Security;
using Cryptography;
using System.DirectoryServices;
using System.Configuration;
using System.Web.Configuration;
using System.Collections;
//using System.DirectoryServices;
//using System.DirectoryServices.AccountManagement;

namespace SolarPMS
{
    public partial class Login : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        #region"Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (Session["Token"] != null && Session["UserId"] != null)
                    Response.Redirect("WelcomeScreen.aspx");
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            bool IsADProfile = ddlbProfiles.Items.FindByValue("ActiveDirectory").Selected;

            if (IsADProfile)
                ValidateLDAPUser();
            else
                AuthenticateUser();
        }
        #endregion

        /// <summary>
        /// Authenticate user and set sessions.
        /// </summary>
        private void AuthenticateUser()
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text;
            if (userName != string.Empty && password != string.Empty)
            {
                string result = commonFunctions.GetToken(Constants.TOKEN_URL, "grant_type=password&username="
                                + HttpUtility.UrlEncode(Crypto.Instance.Encrypt(userName)) + "&password=" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt(password)));

                if (result != string.Empty)
                {
                    UpdateSessionVariable(result);
                    lblErrorMessage.Visible = false;
                    RememberCredentials();
                    RedircetToPage();
                }
                else
                    lblErrorMessage.Visible = true;
            }
        }

        ///// <summary>
        ///// This method is used to validate AD user.
        ///// </summary>
        ///// <param name="domain"></param>
        ///// <param name="username"></param>
        ///// <param name="password"></param>
        ///// <param name="LdapPath"></param>
        ///// <returns></returns>
        private void ValidateLDAPUser()
        {
            string dominName = string.Empty;
            string adPath = string.Empty;
            string userName = txtUserName.Text.Trim().ToUpper();
            string password = txtPassword.Text;
            string strError = string.Empty;
            try
            {
                dominName = WebConfigurationManager.AppSettings["DirectoryDomain"];
                adPath = WebConfigurationManager.AppSettings["DirectoryPath"];

                if (!string.IsNullOrEmpty(dominName) && !string.IsNullOrEmpty(adPath))
                {
                    if (true == AuthenticateUser(dominName, userName, password, adPath, out strError))
                    {
                        string result = commonFunctions.GetToken(Constants.TOKEN_URL, "grant_type=password&username="
                                + HttpUtility.UrlEncode(Crypto.Instance.Encrypt(userName)) + "&password=");

                        if (result != string.Empty)
                        {
                            UpdateSessionVariable(result);
                            lblErrorMessage.Visible = false;
                            RememberCredentials();
                            RedircetToPage();
                        }
                        else
                            lblErrorMessage.Visible = true;
                    }
                    dominName = string.Empty;
                    adPath = string.Empty;
                }

                if (!string.IsNullOrEmpty(strError))
                    lblErrorMessage.Visible = true;
            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
            }
        }

        private void RedircetToPage()
        {
            if (Request.QueryString["ReturnURL"] != null)
                Response.Redirect(Request.QueryString["ReturnURL"]);
            else
                Response.Redirect("WelcomeScreen.aspx");
        }

        public bool AuthenticateUser(string domain, string username, string password, string LdapPath, out string Errmsg)
        {
            Errmsg = "";
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(LdapPath, domainAndUsername, password);
            try
            {
                // Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory
                LdapPath = result.Path;
                string _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                Errmsg = ex.Message;
                Utility.WriteErrorLog("Error authenticating user." + ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }


        ///// <summary>
        ///// update or set required sessions details. 
        ///// </summary>
        ///// <param name="result"></param>
        private void UpdateSessionVariable(string result)
        {
            TokenResponseModel tokenDetails = JsonConvert.DeserializeObject<TokenResponseModel>(result);
            HttpContext.Current.Session["Token"] = tokenDetails.AccessToken;
            HttpContext.Current.Session["UserId"] = tokenDetails.UserId;
            HttpContext.Current.Session["ProfileName"] = tokenDetails.ProfileName;
            HttpContext.Current.Session[Constants.CONST_SESSION_DEDOCUMENT_ACEESS] = tokenDetails.DocumentAccess;
            string userDetails = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETUSERDTL + tokenDetails.UserId, string.Empty);
            User user = JsonConvert.DeserializeObject<User>(userDetails);

            if (user.UserName != null)
                HttpContext.Current.Session["UserName"] = user.UserName;
            if (user.Photo != null)
                HttpContext.Current.Session["Photo"] = user.Photo;
            if (user.EmployeeId != null)
                HttpContext.Current.Session["EmployeeId"] = user.EmployeeId;
            if (user.ProfileId != 0)
                HttpContext.Current.Session["ProfileId"] = user.ProfileId;
            if (user.Name != null)
                HttpContext.Current.Session["LoginUserName"] = user.Name;
            if (user.LocationId != 0)
                HttpContext.Current.Session["LoginUserLocationId"] = user.LocationId;
            if (!string.IsNullOrEmpty(user.Photo))
                HttpContext.Current.Session["PhotpPath"] = user.Photo;

            

            UserModel userModelObj = new UserModel();
            Hashtable menuList = userModelObj.GetUserMenus();
            HttpContext.Current.Session["MenuSecurity"] = menuList;

        }

        /// <summary>
        /// Initialize constants or variables.
        /// </summary>
        private void Initialize()
        {
            //Constants.RESTURL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/";
            Constants.RESTURL = ConfigurationManager.AppSettings["WebsiteUrl"];
            string passwordCookie = string.Empty;
            if (Request.Cookies["suzlon_solpms_userid"] != null)
                txtUserName.Text = Crypto.Instance.Decrypt(Request.Cookies["suzlon_solpms_userid"].Value);

            if (Request.Cookies["suzlon_solpms_pwd"] != null)
            {
                passwordCookie = Crypto.Instance.Decrypt(Request.Cookies["suzlon_solpms_pwd"].Value);
                txtPassword.Attributes.Add("value", passwordCookie);
            }

            if(Request.Cookies["suzlon_solpms_authentication"] != null)
            {
                ddlbProfiles.Value= Crypto.Instance.Decrypt(Request.Cookies["suzlon_solpms_authentication"].Value);
            }
            if (Request.Cookies["suzlon_solpms_userid"] != null && passwordCookie != string.Empty)
                chkRemember.Checked = true;
        }

        /// <summary>
        /// Remember user name and password.
        /// </summary>
        private void RememberCredentials()
        {
            if (chkRemember.Checked)
            {
                Response.Cookies.Clear();
                Response.Cookies["suzlon_solpms_userid"].Value = Crypto.Instance.Encrypt(txtUserName.Text.Trim());
                Response.Cookies["suzlon_solpms_pwd"].Value = Crypto.Instance.Encrypt(txtPassword.Text);
                Response.Cookies["suzlon_solpms_authentication"].Value = Crypto.Instance.Encrypt(ddlbProfiles.Value);
                Response.Cookies["suzlon_solpms_userid"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["suzlon_solpms_pwd"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["suzlon_solpms_authentication"].Expires = DateTime.Now.AddDays(15);
            }
            else
            {
                Response.Cookies["suzlon_solpms_userid"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["suzlon_solpms_pwd"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["suzlon_solpms_authentication"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["suzlon_solpms_userid"].Value = string.Empty;
                Response.Cookies["suzlon_solpms_pwd"].Value = string.Empty;
                Response.Cookies["suzlon_solpms_authentication"].Value = string.Empty;
            }
        }

    }
}
