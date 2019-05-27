using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.ImageEditor;
namespace SolarPMS.Admin
{

    public partial class EscalationMatrics : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
       
        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
                lblMessage.Text = "";

                ViewState["EscalationId"] = "0";
                GetEscaltionMatrix();
            }
        }

        private void GetEscaltionMatrix()
        {
            string result1 = commonFunctions.RestServiceCall(Constants.ESCALATION_MATRIX_GET, string.Empty);
            SolarPMS.Models.EscalationMatrix EscalationInfo = JsonConvert.DeserializeObject<SolarPMS.Models.EscalationMatrix>(result1);
            if (EscalationInfo != null)
            {
                ViewState["EscalationId"] = Convert.ToInt32(EscalationInfo.EscalationMatrixId);
                txtActLonThanPlan.Text = Convert.ToString(EscalationInfo.ActivityTakingLongerThanPlanned);
                txtIssueResolution.Text = Convert.ToString(EscalationInfo.IssueNotResolved);
                txtIssueClosed.Text = Convert.ToString(EscalationInfo.IssueNotClosed);
                txtQuaRejResolution.Text = Convert.ToString(EscalationInfo.QualityIssueNotResolved);
                txtQulRejClosed.Text = Convert.ToString(EscalationInfo.QualityRejectionNotClosed);
            }

        }
        
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EscalationMatrix EscalationInfo = new EscalationMatrix()
                {
                    EscalationMatrixId = Convert.ToInt32(ViewState["EscalationId"]),
                    ActivityTakingLongerThanPlanned = Convert.ToInt32(txtActLonThanPlan.Text),
                    IssueNotResolved = Convert.ToInt32(txtIssueResolution.Text),
                    IssueNotClosed= Convert.ToInt32(txtIssueClosed.Text),
                    QualityIssueNotResolved= Convert.ToInt32(txtQuaRejResolution.Text),
                    QualityRejectionNotClosed= Convert.ToInt32(txtQulRejClosed.Text),

                };

                string jsonInputParameter = JsonConvert.SerializeObject(EscalationInfo);
                string serviceResult = string.Empty;

                if (Convert.ToInt32(ViewState["EscalationId"]) == 0)
                    serviceResult = commonFunctions.RestServiceCall(Constants.ESCALATION_MATRIX_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                else
                    serviceResult = commonFunctions.RestServiceCall(Constants.ESCALATION_MATRIX_UPDATE, Crypto.Instance.Encrypt(jsonInputParameter));

                if (string.Compare(serviceResult, Constants.REST_CALL_FAILURE, true) == 0)
                {
                    radMesaage.Title = "Alert";
                    radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);

                }
                else
                { 
                radMesaage.Title = "Sucesss";
                radMesaage.Show(Constants.RECORD_SAVE_SUCESSFULLY);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }


        }
        
    }
}