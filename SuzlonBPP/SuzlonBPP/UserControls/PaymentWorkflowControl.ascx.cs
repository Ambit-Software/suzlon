using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuzlonBPP.UserControls
{
    public partial class PaymentWorkflowControl : System.Web.UI.UserControl
    {
        public PaymentWorkflowModel paymentWorkflowModel;
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (paymentWorkflowModel != null)
            {
                string drpname = "aggregator,controller,exceptional-approver,auditor,FASSC-DT,FASSC-CB";
                string result = commonFunctions.RestServiceCall(string.Format(Constants.BANKWORKFLOW_GETDROPDOWNVALUE, drpname, paymentWorkflowModel.subVerticalMaster.SubVerticalId), string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);

                lblName.Text = paymentWorkflowModel.subVerticalMaster.Name;
                DrpPriVerCont.DataSource = ddValues.Aggregator;
                DrpPriGrpCont.DataSource = ddValues.Controller;
                DrpPriTreasury.DataSource = ddValues.ExceptionalApprover;
                DrpPriMgmtAss.DataSource = ddValues.Auditor;
                DrpPriFASSC.DataSource = ddValues.FASSCDT;
                DrpPriCB.DataSource = ddValues.FASSCCB;

                DrpSecVerCont.DataSource = ddValues.Aggregator;               
                DrpSecGrpCont.DataSource = ddValues.Controller;
                DrpSecTreasury.DataSource = ddValues.ExceptionalApprover;
                DrpSecMgmtAss.DataSource = ddValues.Auditor;
                DrpSecFASSC.DataSource = ddValues.FASSCDT;
                DrpSecCB.DataSource = ddValues.FASSCCB;                

                DrpPriVerCont.DataBind();
                DrpPriGrpCont.DataBind();
                DrpPriTreasury.DataBind();
                DrpPriMgmtAss.DataBind();
                DrpPriFASSC.DataBind();
                DrpPriCB.DataBind();

                DrpSecVerCont.DataBind();
                DrpSecGrpCont.DataBind();
                DrpSecTreasury.DataBind();
                DrpSecMgmtAss.DataBind();
                DrpSecFASSC.DataBind();
                DrpSecCB.DataBind();

                DrpPriVerCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriVerCont.SelectedIndex = 0;
                DrpPriGrpCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriGrpCont.SelectedIndex = 0;
                DrpPriTreasury.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriTreasury.SelectedIndex = 0;
                DrpPriMgmtAss.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriMgmtAss.SelectedIndex = 0;
                DrpPriFASSC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriFASSC.SelectedIndex = 0;
                DrpPriCB.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriCB.SelectedIndex = 0;

                DrpSecVerCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecVerCont.SelectedIndex = 0;
                DrpSecGrpCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecGrpCont.SelectedIndex = 0;
                DrpSecTreasury.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecTreasury.SelectedIndex = 0;
                DrpSecMgmtAss.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecMgmtAss.SelectedIndex = 0;
                DrpSecFASSC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecFASSC.SelectedIndex = 0;
                DrpSecCB.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecCB.SelectedIndex = 0;

                if (paymentWorkflowModel.paymentWorkflow != null)
                {
                    hidWorkflowId.Value = Convert.ToString(paymentWorkflowModel.paymentWorkflow.PaymentWorkFlowId);

                    //Set Db Values to dropdowns
                    DrpPriVerCont.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.PriAggUserId);
                    DrpPriGrpCont.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.PriContUserId);
                    DrpPriTreasury.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.PriExpAppUserId);
                    DrpPriMgmtAss.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.PriAuditorUserId);
                    DrpPriFASSC.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.PriFASSCDTUserId);
                    DrpPriCB.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.PriFASSCCBUserId);
                    DrpSecVerCont.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.SecAggUserId);
                    DrpSecGrpCont.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.SecContUserId);
                    DrpSecTreasury.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.SecExpAppUserId);
                    DrpSecMgmtAss.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.SecAuditorUserId);
                    DrpSecFASSC.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.SecFASSCDTUserId);
                    DrpSecCB.SelectedValue = Convert.ToString(paymentWorkflowModel.paymentWorkflow.SecFASSCCBUserId);

                    //Set Selected Dates
                    DpFromVerCont.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecAggFromDt;
                    DpToVerCont.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecAggToDt;
                    DpFromGrpCont.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecContFromDt;
                    DpToGrpCont.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecContToDt;
                    DpFromTreasury.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecExpAppFromDt;
                    DpToTreasury.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecExpAppToDt;
                    DpFromMgmtAss.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecAuditorFromDt;
                    DpToMgmtAss.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecAuditorToDt;
                    DpFromFASCC.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecFASSCDTFromDt;
                    DpToFASCC.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecFASSCDTToDt;
                    DpFromCB.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecFASSCCBFromDt;
                    DpToCB.SelectedDate = paymentWorkflowModel.paymentWorkflow.SecFASSCCBToDt;
                    hidCreatedDate.Value = paymentWorkflowModel.paymentWorkflow.CreatedOn.Value.ToString("dd-MM-yyyy");
                }
                else
                {
                    DrpPriVerCont.SelectedIndex = 0;
                    DrpPriGrpCont.SelectedIndex = 0;
                    DrpPriTreasury.SelectedIndex = 0;
                    DrpPriMgmtAss.SelectedIndex = 0;
                    DrpPriFASSC.SelectedIndex = 0;
                    DrpPriCB.SelectedIndex = 0;
                    DrpSecVerCont.SelectedIndex = 0;
                    DrpSecGrpCont.SelectedIndex = 0;
                    DrpSecTreasury.SelectedIndex = 0;
                    DrpSecMgmtAss.SelectedIndex = 0;
                    DrpSecFASSC.SelectedIndex = 0;
                    DrpSecCB.SelectedIndex = 0;
                }

                if (paymentWorkflowModel.subVerticalMaster != null)
                {
                    hidSubVerticalId.Value = Convert.ToString(paymentWorkflowModel.subVerticalMaster.SubVerticalId);
                    hidVerticalId.Value = Convert.ToString(paymentWorkflowModel.subVerticalMaster.VerticalId);
                }
                else
                {
                    hidSubVerticalId.Value = string.Empty;
                    hidVerticalId.Value = string.Empty;
                }
            }
        }
    }
}