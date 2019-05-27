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
    public partial class TreasuryWorkflowControl : System.Web.UI.UserControl
    {
        public TreasuryWorkflowModel treasuryWorkflowModel;
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (treasuryWorkflowModel != null)
            {

                string drpname = "verticalcontroller,treasury,cb";
                string result = commonFunctions.RestServiceCall(string.Format(Constants.BANKWORKFLOW_GETDROPDOWNVALUE, drpname, treasuryWorkflowModel.subVerticalMaster.SubVerticalId), string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);

                lblName.Text = treasuryWorkflowModel.subVerticalMaster.Name;
                DrpPriVerCont.DataSource = ddValues.VerticalController;
                DrpPriTreasury.DataSource = ddValues.Treasury;
                DrpPriCB.DataSource = ddValues.CB;
                DrpSecVerCont.DataSource = ddValues.VerticalController;
               
                DrpSecTreasury.DataSource = ddValues.Treasury;
                DrpSecCB.DataSource = ddValues.CB;

                DrpPriVerCont.DataBind();
                DrpSecTreasury.DataBind();
                DrpSecVerCont.DataBind();
                DrpSecCB.DataBind();
                DrpPriTreasury.DataBind();
                DrpPriCB.DataBind();
                DrpSecCB.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecCB.SelectedIndex = 0;
                DrpSecTreasury.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecTreasury.SelectedIndex = 0;
                DrpSecVerCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpSecVerCont.SelectedIndex = 0;
                DrpPriCB.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriCB.SelectedIndex = 0;
                DrpPriTreasury.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriTreasury.SelectedIndex = 0;
                DrpPriVerCont.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select User", String.Empty));
                DrpPriVerCont.SelectedIndex = 0;

                if (treasuryWorkflowModel.treasuryWorkflow != null)
                {
                    hidWorkflowId.Value = Convert.ToString(treasuryWorkflowModel.treasuryWorkflow.TreasuryWorkFlowId);

                    //Set Db Values to dropdowns
                    DrpPriVerCont.SelectedValue = Convert.ToString(treasuryWorkflowModel.treasuryWorkflow.PriVerContUserId);
                    DrpPriTreasury.SelectedValue = Convert.ToString(treasuryWorkflowModel.treasuryWorkflow.PriTreasuryUserId);
                    DrpPriCB.SelectedValue = Convert.ToString(treasuryWorkflowModel.treasuryWorkflow.PriCBUserId);
                    DrpSecVerCont.SelectedValue = Convert.ToString(treasuryWorkflowModel.treasuryWorkflow.SecVerContUserId);
                    DrpSecTreasury.SelectedValue = Convert.ToString(treasuryWorkflowModel.treasuryWorkflow.SecTreasuryUserId);
                    DrpSecCB.SelectedValue = Convert.ToString(treasuryWorkflowModel.treasuryWorkflow.SecCBUserId);

                    //Set Selected Dates
                    DpFromVerCont.SelectedDate = treasuryWorkflowModel.treasuryWorkflow.SecVerContFromDt;
                    DpToVerCont.SelectedDate = treasuryWorkflowModel.treasuryWorkflow.SecVerContToDt;
                    DpFromTreasury.SelectedDate = treasuryWorkflowModel.treasuryWorkflow.SecTreasuryFromDt;
                    DpToTreasury.SelectedDate = treasuryWorkflowModel.treasuryWorkflow.SecTreasuryToDt;
                    DpFromCB.SelectedDate = treasuryWorkflowModel.treasuryWorkflow.SecCBFromDt;
                    DpToCB.SelectedDate = treasuryWorkflowModel.treasuryWorkflow.SecCBToDt;
                    hidCreatedDate.Value = treasuryWorkflowModel.treasuryWorkflow.CreatedOn.ToString("dd-MM-yyyy");
                }
                else
                {
                    DrpPriVerCont.SelectedIndex = 0;
                    DrpPriTreasury.SelectedIndex = 0;
                    DrpPriCB.SelectedIndex = 0;
                    DrpSecVerCont.SelectedIndex = 0;
                    DrpSecTreasury.SelectedIndex = 0;
                    DrpSecCB.SelectedIndex = 0;
                }

                if (treasuryWorkflowModel.subVerticalMaster != null)
                {
                    hidSubVerticalId.Value = Convert.ToString(treasuryWorkflowModel.subVerticalMaster.SubVerticalId);
                    hidVerticalId.Value = Convert.ToString(treasuryWorkflowModel.subVerticalMaster.VerticalId);
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