using System;
using System.Web.UI;
using SuzlonBPP.Models;
using Newtonsoft.Json;
using Cryptography;
using System.Collections;

namespace SuzlonBPP
{
    public partial class ApplicationConfiguration : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.APPLICATION_CONFIGURATION, menuList)) Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {
                GetSettings();
            }
        }

        private void GetSettings()
        {
            string result = commonFunctions.RestServiceCall(Constants.GET_APPLICATION_SETTINGS, string.Empty);
            Models.ApplicationConfiguration settings = JsonConvert.DeserializeObject<Models.ApplicationConfiguration>(result);
            if (settings != null)
            {
                txtDays.Text = Convert.ToString(settings.BudgetLimit);
                DrpAddendum.SelectedValue = Convert.ToString(settings.Addendum == true ? "Enabled" : "Disabled");
                txtPaymentMethod.Text = settings.PaymentMethod;
                RadDailyAmount.Text = settings.DailyPaymentLimit.ToString();
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int noDays = 0;
                    noDays = Convert.ToInt32(txtDays.Text);
                    if (noDays < 1 || noDays > 365)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show("Budget Utilisation Maximum Limit should be 1 to 365");
                        return;
                    }
                    Models.ApplicationConfiguration settings = new Models.ApplicationConfiguration()
                    {
                        BudgetLimit = Convert.ToInt32(txtDays.Text),
                        Addendum = (Convert.ToString(DrpAddendum.SelectedValue) == "Enabled" ? true : false),
                        PaymentMethod = txtPaymentMethod.Text,
                        DailyPaymentLimit=Convert.ToDecimal(RadDailyAmount.Text)
                    };
                    string jsonInputParameter = JsonConvert.SerializeObject(settings);
                    string result = commonFunctions.RestServiceCall(Constants.SAVE_APPLICATION_SETTINGS, Crypto.Instance.Encrypt(jsonInputParameter));
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    if (result == Constants.REST_CALL_FAILURE)
                        radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    else if (result.ToLower() == "true")
                        radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                    else
                        radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                }
            }
        }
    }
}