using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuzlonBPP
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    Initialize();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void Initialize()
        {
            try
            {
                if (Request.QueryString.Count > 0)
                {
                    string queryString = Cryptography.Crypto.Instance.Decrypt(HttpUtility.UrlDecode(Request.QueryString.ToString()));
                    string[] queryStringValues = queryString.Split('&');
                    string userName = string.Empty;
                    string emailId = string.Empty;
                    string password = string.Empty;

                    if (queryStringValues.Count() > 0)
                    {
                        hidUserName.Value = (queryString.Split('&'))[0].Split('=')[1];
                        emailId = (queryString.Split('&'))[1].Split('=')[1];
                        password = (queryString.Split('&'))[2];
                        password = password.Substring(password.IndexOf('=') + 1);
                        hidOldPassword.Value = password;

                        CommonFunctions commonFunctions = new CommonFunctions();
                        Constants.RESTURL = ConfigurationManager.AppSettings["WebsiteUrl"];
                        //Constants.RESTURL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/";
                        string isExists = commonFunctions.RestServiceCall(Constants.VALIDATE_CREDETIALS +
                                            "?emailId=" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt(emailId)) +
                                            "&password=" + HttpUtility.UrlEncode(password), string.Empty);

                        divContainer.Visible = isExists == "true";
                        divErrorMessage.Visible = isExists != "true";
                    }

                    divOldPassword.Visible = false;
                    txtOldPassword.Text = password;
                }
                else
                {
                    divOldPassword.Visible = divContainer.Visible = true;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ResetUserPassword();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void ResetUserPassword()
        {
            try
            {
                CommonFunctions commonFunctions = new CommonFunctions();
                ChangePasswordModel changePassword = new ChangePasswordModel()
                {
                    UserId = Convert.ToInt32(hidUserName.Value),
                    ConfirmPassword = txtConfirmPassword.Text,
                    OldPassword = Crypto.Instance.Decrypt(hidOldPassword.Value),
                    Password = txtNewPassword.Text
                };

                string jsonInputParameter = JsonConvert.SerializeObject(changePassword);
                Constants.RESTURL = ConfigurationManager.AppSettings["WebsiteUrl"];
                // Constants.RESTURL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/";
                string userDetails = commonFunctions.RestServiceCall(Constants.CHANGE_PASSWORD, Crypto.Instance.Encrypt(jsonInputParameter));
                lblUpdateSuccess.Text = Constants.PASSWORD_UPATED;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}