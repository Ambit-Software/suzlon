using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuzlonBPP
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeUserPassword();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion

        #region "Private Methods"
        private void ChangeUserPassword()
        {
            CommonFunctions commonFunctions = new CommonFunctions();
            ChangePasswordModel changePassword = new ChangePasswordModel()
            {
                UserId = Convert.ToInt32(Session["UserId"]),
                ConfirmPassword = txtConfirmPassword.Text,
                OldPassword = txtOldPassword.Text,
                Password = txtNewPassword.Text
            };

            string jsonInputParameter = JsonConvert.SerializeObject(changePassword);
            Constants.RESTURL = ConfigurationManager.AppSettings["WebsiteUrl"];
            //Constants.RESTURL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/";
            string userDetails = commonFunctions.RestServiceCall(Constants.CHANGE_PASSWORD, Crypto.Instance.Encrypt(jsonInputParameter));
            lblMessage.Text = JsonConvert.DeserializeObject<string>(userDetails);
            divErrorMessage.Visible = true;
            if (lblMessage.Text == Constants.PASSWORD_UPATED)
            {
                lblMessage.ForeColor = Color.Blue;
                lblMessage.Font.Bold = true;
            }
            else
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Font.Bold = false;
            }
        }
        #endregion
    }
}