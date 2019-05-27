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
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ResetPassword();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// If user has entered correct password reset password and send email.
        /// </summary>
        private void ResetPassword()
        {
            try
            {
                CommonFunctions commonFunctions = new CommonFunctions();
                Constants.RESTURL = ConfigurationManager.AppSettings["WebsiteUrl"];
                string userDetails = commonFunctions.RestServiceCall(Constants.FORGOT_PASSWORD +
                                 "?emailId=" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt(txtEmail.Text.Trim())) + "", string.Empty);
                //password = password.Replace('"', ' ').Trim();
                User user = JsonConvert.DeserializeObject<Models.User>(userDetails);

                string emailId = txtEmail.Text.Trim();
                //string password = commonFunctions.RestServiceCall(Constants.FORGOT_PASSWORD + "?emailId=" + emailId, string.Empty);

                if (user != null)
                {
                    try
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteUrl"] + "ResetPassword.aspx?" +
                                    HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("userid=" + user.UserId + "&emailId=" + user.EmailId + "&password=" + user.Password));
                        string mailBody = "Dear User,<br> we received a request for password change for user " + emailId + ".<br><br>" +
                                           "<a href='" + url + "'>Go to this page</a> to set your new password.<br><br><br> Thank You.";
                        string mailSubject = "Forgot password";
                        CommonFunctions.SendEmail(emailId, string.Empty, mailBody, mailSubject, null, "");
                        divSuccessMessage.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        CommonFunctions.WriteErrorLog(ex);
                        lblMessage.Text = Constants.CONST_MESSAGE_SENDING_FAIL;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblMessage.Text = "Email id is incorrect.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }

                txtEmail.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}