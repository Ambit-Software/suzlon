using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolarPMS
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ResetPassword();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// If user has entered correct password reset password and send email.
        /// </summary>
        private void ResetPassword()
        {
            CommonFunctions commonFunctions = new CommonFunctions();
            Constants.RESTURL = ConfigurationManager.AppSettings["WebsiteUrl"];
            string userDetails = commonFunctions.RestServiceCall(Constants.FORGOT_PASSWORD +
                             "?emailId=" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt(txtEmail.Text.Trim())) + "", string.Empty);

            User user = JsonConvert.DeserializeObject<Models.User>(userDetails);
            string emailId = txtEmail.Text.Trim();

            if (user != null)
            {
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

                    CommonFunctions.SendEmail(emailId, string.Empty, mailBody.ToString(), mailSubject, null, "");
                    divSuccessMessage.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = Constants.CONST_MESSAGE_SENDING_FAIL;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    CommonFunctions.WriteErrorLog(ex);
                }
            }
            else
            {
                lblMessage.Text = "Email id is incorrect.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }

            txtEmail.Text = string.Empty;
        }
    }


}