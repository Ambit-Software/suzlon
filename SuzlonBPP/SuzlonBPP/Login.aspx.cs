using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Web;
using Cryptography;
using System.DirectoryServices;
using System.Configuration;
using System.Web.Configuration;
using System.Collections;

namespace SuzlonBPP
{
    public partial class Login : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        #region"Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                    if (Session["Token"] != null && Session["UserId"] != null)
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
                        {
                            string[] queryString = Request.QueryString.ToString().Split('?');
                            string PageDetail = Crypto.Instance.Decrypt(HttpUtility.UrlDecode(queryString[0])).Split('=')[0];
                            if (PageDetail == "BankDeatilId")
                                Response.Redirect("AddVendorBankDetail.aspx?" + Request.QueryString.ToString(), false);
                            if (PageDetail == "TreasuryId")
                                Response.Redirect("VerticalControllerDetail.aspx?" + Request.QueryString.ToString(), false);
                        }
                        else
                            Response.Redirect("BankDetailList.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsADProfile = ddlbProfiles.Items.FindByValue("ActiveDirectory").Selected;

                if (IsADProfile)
                    ValidateLDAPUser();
                else
                    AuthenticateUser();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        #endregion

        /// <summary>
        /// Authenticate user and set sessions.
        /// </summary>
        private void AuthenticateUser()
        {
            try
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

                        if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
                        {
                            string[] queryString = Request.QueryString.ToString().Split('?');
                            string PageDetail = Crypto.Instance.Decrypt(HttpUtility.UrlDecode(queryString[0])).Split('=')[0];
                            if(PageDetail== "BankDeatilId")
                            Response.Redirect("AddVendorBankDetail.aspx?" + Request.QueryString.ToString(), false);
                            if (PageDetail == "TreasuryId")
                                Response.Redirect("VerticalControllerDetail.aspx?" + Request.QueryString.ToString(), false);
                        }
                        else
                            Response.Redirect("WelcomeScreen.aspx", false);
                    }
                    else
                        lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
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
                //dominName = WebConfigurationManager.AppSettings["DirectoryDomain"];
                adPath = WebConfigurationManager.AppSettings["DirectoryPath"];
                //if (!string.IsNullOrEmpty(dominName) && !string.IsNullOrEmpty(adPath))
                if (!string.IsNullOrEmpty(adPath))
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
                            Response.Redirect("BankDetailList.aspx", false);
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
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        public bool AuthenticateUser(string domain, string username, string password, string LdapPath, out string Errmsg)
        {
            Errmsg = "";
            //--------------------------------------
            //string domainAndUsername = domain + @"\" + username; Manish
            //DirectoryEntry entry = new DirectoryEntry(LdapPath, domainAndUsername, password); Manish
            DirectoryEntry entry = new DirectoryEntry(LdapPath);
            entry.AuthenticationType = AuthenticationTypes.Secure;
            entry.Username = username;
            entry.Password = password;
            ///------------------------

            try
            {
                // Bind to the native AdsObject to force authentication.
                //object obj = entry.NativeObject;
                // DirectorySearcher search = new DirectorySearcher(entry);
                // search.Filter = "(SAMAccountName=" + username + ")";
                // search.PropertiesToLoad.Add("cn");

                //SearchResult result = search.FindOne();

                string strSearch = "SAMAccountName=" + username;
                DirectorySearcher dsSystem = new DirectorySearcher(entry, strSearch);
                SearchResult result = dsSystem.FindOne();

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
                CommonFunctions.WriteErrorLog(ex);
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
            HttpContext.Current.Session["LoginUserName"] = tokenDetails.Name;
            HttpContext.Current.Session["Photo"] = string.IsNullOrEmpty(tokenDetails.Photo) ? "Content/images/placeholder-icon.png" : tokenDetails.Photo;
            HttpContext.Current.Session["ProfileName"] = tokenDetails.ProfileName;
            HttpContext.Current.Session["ProfileId"] = tokenDetails.ProfileId;

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
            string authTypeCookie = string.Empty;
            if (Request.Cookies["SuzlonBPPuserid"] != null)
                txtUserName.Text = Crypto.Instance.Decrypt(Request.Cookies["SuzlonBPPuserid"].Value);

            if (Request.Cookies["SuzlonBPPpwd"] != null)
            {
                passwordCookie = Crypto.Instance.Decrypt(Request.Cookies["SuzlonBPPpwd"].Value);
                txtPassword.Attributes.Add("value", passwordCookie);
            }
            if (Request.Cookies["SuzlonBPPAuthType"] != null)
            {
                authTypeCookie = Crypto.Instance.Decrypt(Request.Cookies["SuzlonBPPAuthType"].Value);
                if (authTypeCookie == "Database")
                    ddlbProfiles.Items.FindByValue("Database").Selected = true;
            }

            if (Request.Cookies["SuzlonBPPuserid"] != null &&  passwordCookie != string.Empty)
                chkRemember.Checked = true;
        }
        private void RememberCredentials()
        {
            if (chkRemember.Checked)
            {
                Response.Cookies.Clear();
                Response.Cookies["SuzlonBPPuserid"].Value = Crypto.Instance.Encrypt(txtUserName.Text.Trim());
                Response.Cookies["SuzlonBPPpwd"].Value = Crypto.Instance.Encrypt(txtPassword.Text);
                bool IsADProfile = ddlbProfiles.Items.FindByValue("ActiveDirectory").Selected;
                if (IsADProfile)
                    Response.Cookies["SuzlonBPPAuthType"].Value = Crypto.Instance.Encrypt("ActiveDirectory");
                else
                    Response.Cookies["SuzlonBPPAuthType"].Value = Crypto.Instance.Encrypt("Database");
                Response.Cookies["SuzlonBPPuserid"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["SuzlonBPPpwd"].Expires = DateTime.Now.AddDays(15);                
            }
            else
            {
                Response.Cookies["SuzlonBPPuserid"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["SuzlonBPPpwd"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["SuzlonBPPAuthType"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["SuzlonBPPuserid"].Value = string.Empty;
                Response.Cookies["SuzlonBPPpwd"].Value = string.Empty;
                Response.Cookies["SuzlonBPPAuthType"].Value = string.Empty;
            }
        }
       
    }
}