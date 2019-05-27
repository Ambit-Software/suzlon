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
    public partial class BankWorkFlowControl : System.Web.UI.UserControl
    {
        public BankWorkflowModel bankWorkflowModel;
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bankWorkflowModel != null)
            {

                string drpname = "verticalcontroller,groupcontroller,treasury,managementassurance,fassc,cb,payment-proposer";
                string result = commonFunctions.RestServiceCall(string.Format(Constants.BANKWORKFLOW_GETDROPDOWNVALUE, drpname, bankWorkflowModel.subVerticalMaster.SubVerticalId), string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);               

                lblName.Text = bankWorkflowModel.subVerticalMaster.Name;
                DrpPriVerCont.DataSource = ddValues.VerticalController;
                DrpPriGrpCont.DataSource = ddValues.GroupController;
                DrpPriTreasury.DataSource = ddValues.Treasury ;
                DrpPriMgmtAss.DataSource = ddValues.ManagementAssurance;
                DrpPriFASSC.DataSource = ddValues.FASSC; 
                DrpPriCB.DataSource = ddValues.CB; 
                DrpSecVerCont.DataSource = ddValues.VerticalController; 
                DrpSecVerCont.DataBind();
                DrpSecGrpCont.DataSource = ddValues.GroupController;
                DrpSecTreasury.DataSource = ddValues.Treasury;
                DrpSecMgmtAss.DataSource = ddValues.ManagementAssurance;
                DrpSecFASSC.DataSource = ddValues.FASSC;
                DrpSecCB.DataSource = ddValues.CB;

                DrpPriVerCont.DataBind();
                DrpSecGrpCont.DataBind();
                DrpSecTreasury.DataBind();
                DrpSecMgmtAss.DataBind();
                DrpSecFASSC.DataBind();
                DrpSecCB.DataBind();
                DrpPriGrpCont.DataBind();
                DrpPriTreasury.DataBind();
                DrpPriMgmtAss.DataBind();
                DrpPriFASSC.DataBind();
                DrpPriCB.DataBind();
                DrpPriGrpCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriGrpCont.SelectedIndex = 0;
                DrpSecCB.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecCB.SelectedIndex = 0;
                DrpSecFASSC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecFASSC.SelectedIndex = 0;
                DrpSecMgmtAss.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecMgmtAss.SelectedIndex = 0;
                DrpSecTreasury.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecTreasury.SelectedIndex = 0;
                DrpSecGrpCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecGrpCont.SelectedIndex = 0;
                DrpSecVerCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecVerCont.SelectedIndex = 0;
                DrpPriCB.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriCB.SelectedIndex = 0;
                DrpPriFASSC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriFASSC.SelectedIndex = 0;
                DrpPriMgmtAss.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriMgmtAss.SelectedIndex = 0;
                DrpPriTreasury.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriTreasury.SelectedIndex = 0;
                DrpPriVerCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriVerCont.SelectedIndex = 0;

                if (bankWorkflowModel.bankWorkFlow != null)
                {
                    hidWorkflowId.Value = Convert.ToString(bankWorkflowModel.bankWorkFlow.BankWorkFlowId);

                    //Set Db Values to dropdowns
                    DrpPriVerCont.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.PriVerContUserId);
                    DrpPriGrpCont.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.PriGrpContUserId);
                    DrpPriTreasury.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.PriTreasuryUserId);
                    DrpPriMgmtAss.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.PriMgmtAssUserId);
                    DrpPriFASSC.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.PriFASSCUserId);
                    DrpPriCB.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.PriCBUserId);
                    DrpSecVerCont.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.SecVerContUserId);
                    DrpSecGrpCont.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.SecGrpContUserId);
                    DrpSecTreasury.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.SecTreasuryUserId);
                    DrpSecMgmtAss.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.SecMgmtAssUserId);
                    DrpSecFASSC.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.SecFASSCUserId);
                    DrpSecCB.SelectedValue = Convert.ToString(bankWorkflowModel.bankWorkFlow.SecCBUserId);

                    //Set Selected Dates
                    DpFromVerCont.SelectedDate = bankWorkflowModel.bankWorkFlow.SecVerContFromDt;
                    DpToVerCont.SelectedDate = bankWorkflowModel.bankWorkFlow.SecVerContToDt;                    
                    DpFromGrpCont.SelectedDate = bankWorkflowModel.bankWorkFlow.SecGrpContFromDt;
                    DpToGrpCont.SelectedDate = bankWorkflowModel.bankWorkFlow.SecGrpContToDt;
                    DpFromTreasury.SelectedDate = bankWorkflowModel.bankWorkFlow.SecTreasuryFromDt;
                    DpToTreasury.SelectedDate = bankWorkflowModel.bankWorkFlow.SecTreasuryToDt;
                    DpFromMgmtAss.SelectedDate = bankWorkflowModel.bankWorkFlow.SecMgmtAssFromDt;
                    DpToMgmtAss.SelectedDate = bankWorkflowModel.bankWorkFlow.SecMgmtAssToDt;
                    DpFromFASCC.SelectedDate = bankWorkflowModel.bankWorkFlow.SecFASSCFromDt;
                    DpToFASCC.SelectedDate = bankWorkflowModel.bankWorkFlow.SecFASSCToDt;
                    DpFromCB.SelectedDate = bankWorkflowModel.bankWorkFlow.SecCBFromDt;
                    DpToCB.SelectedDate = bankWorkflowModel.bankWorkFlow.SecCBToDt;
                    hidCreatedDate.Value = bankWorkflowModel.bankWorkFlow.CreatedOn.ToString("dd-MM-yyyy");
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

                if (bankWorkflowModel.subVerticalMaster != null)
                {
                    hidSubVerticalId.Value = Convert.ToString(bankWorkflowModel.subVerticalMaster.SubVerticalId);
                    hidVerticalId.Value = Convert.ToString(bankWorkflowModel.subVerticalMaster.VerticalId);
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