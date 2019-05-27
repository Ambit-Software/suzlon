using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using SuzlonBPP.UserControls;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace SuzlonBPP
{
    public partial class BankWorkFlow : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        public List<Models.VerticalMaster> lstVerticalMaster;
        static List<Models.ListItem> lstVerticalController = new List<Models.ListItem>();
        static List<Models.ListItem> lstGroupController = new List<Models.ListItem>();
        static List<Models.ListItem> lstTreasury = new List<Models.ListItem>();
        static List<Models.ListItem> lstManagementAssurance = new List<Models.ListItem>();
        static List<Models.ListItem> lstFASSC = new List<Models.ListItem>();
        static List<Models.ListItem> lstCB = new List<Models.ListItem>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropdownData();
            }
        }

        private void BindDropdownData()
        {
            try
            {
                string drpname = "workflow,subvertical-vertical,verticalcontroller,groupcontroller,treasury,managementassurance,fassc,cb";
                string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty, this);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                ComboSubVertical.DataSource = ddValues.SubVerticalWithVertical;
                ComboSubVertical.DataBind();
                DrpWorkflow.DataSource = ddValues.WorkFlow;
                DrpWorkflow.DataBind();
                DrpWorkflow.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Workflow", string.Empty));
                DrpWorkflow.SelectedIndex = 0;
                lstVerticalController = ddValues.VerticalController;
                lstGroupController = ddValues.GroupController;
                lstTreasury = ddValues.Treasury;
                lstManagementAssurance = ddValues.ManagementAssurance;
                lstFASSC = ddValues.FASSC;
                lstCB = ddValues.CB;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        [WebMethod()]
        public static string SaveBankWorkFlow(string data, int workflowType)
        {

            if (workflowType == (int)(Models.WorkFlow.BankWorkflow))
            {
                CommonFunctions commonFunctions = new CommonFunctions();
                string result = commonFunctions.RestServiceCall(Constants.SAVE_BANK_WORKFLOW, Crypto.Instance.Encrypt(data));
            }
            else
            {
                CommonFunctions commonFunctions = new CommonFunctions();
                string result = commonFunctions.RestServiceCall(Constants.SAVE_TREASURY_WORKFLOW, Crypto.Instance.Encrypt(data));
            }
            return data;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (Convert.ToInt32(hidWorkflowType.Value) == (int)(Models.WorkFlow.BankWorkflow))
            {
                var bankWorkflowModel = e.Item.DataItem as BankWorkflowModel;
                var control = (BankWorkFlowControl)base.Page.LoadControl("UserControls/BankWorkflowControl.ascx");
                Label lblName = control.FindControl("lblName") as Label;
                Label btnSave = control.FindControl("lblSave") as Label;
                lblName.Attributes.Add("onclick", "javascript:expandDiv(this)");
                btnSave.Attributes.Add("onclick", "javascript:SaveBankDetails(this)");
                control.lstVerticalController = lstVerticalController;
                control.lstGroupController = lstGroupController;
                control.lstTreasury = lstTreasury;
                control.lstManagementAssurance = lstManagementAssurance;
                control.lstFASSC = lstFASSC;
                control.lstCB = lstCB;
                control.bankWorkflowModel = bankWorkflowModel;
                if (bankWorkflowModel.subVerticalMaster != null)
                {
                    HiddenField hidSubVerticalId = control.FindControl("hidSubVerticalId") as HiddenField;
                    HiddenField hidVerticalId = control.FindControl("hidVerticalId") as HiddenField;
                    hidSubVerticalId.Value = Convert.ToString(bankWorkflowModel.subVerticalMaster.SubVerticalId);
                    hidVerticalId.Value = Convert.ToString(bankWorkflowModel.subVerticalMaster.VerticalId);
                }
                Repeater1.Controls.Add(control);
            }
            else if(Convert.ToInt32(hidWorkflowType.Value) == (int)(Models.WorkFlow.Treasury))
            {
                var treasuryWorkflowModel = e.Item.DataItem as TreasuryWorkflowModel;
                var control = (TreasuryWorkflowControl)base.Page.LoadControl("UserControls/TreasuryWorkflowControl.ascx");
                Label lblName = control.FindControl("lblName") as Label;
                Label btnSave = control.FindControl("lblSave") as Label;
                lblName.Attributes.Add("onclick", "javascript:expandDiv(this)");
                btnSave.Attributes.Add("onclick", "javascript:SaveBankDetails(this)");
                control.lstVerticalController = lstVerticalController;
                control.lstTreasury = lstTreasury;
                control.lstCB = lstCB;
                control.treasuryWorkflowModel = treasuryWorkflowModel;
                if (treasuryWorkflowModel.subVerticalMaster != null)
                {
                    HiddenField hidSubVerticalId = control.FindControl("hidSubVerticalId") as HiddenField;
                    HiddenField hidVerticalId = control.FindControl("hidVerticalId") as HiddenField;
                    hidSubVerticalId.Value = Convert.ToString(treasuryWorkflowModel.subVerticalMaster.SubVerticalId);
                    hidVerticalId.Value = Convert.ToString(treasuryWorkflowModel.subVerticalMaster.VerticalId);
                }
                Repeater1.Controls.Add(control);
            }
        }

        private string GetSubVerticalIds()
        {
            string subVerticalIds = string.Empty;
            var collection = ComboSubVertical.CheckedItems;
            if (collection != null && collection.Count > 0)
            {
                for (int count = 0; count < collection.Count; count++)
                {
                    var splitArray = collection[count].Value.Split(new string[] { Constants.SEPERATOR }, StringSplitOptions.None);
                    if (string.IsNullOrEmpty(subVerticalIds))
                        subVerticalIds = splitArray[0];
                    else
                        subVerticalIds = subVerticalIds + "," + splitArray[0];
                }
            }
            return subVerticalIds;
        }
        private void BindWorkFlow()
        {
            string subVerticalIds = GetSubVerticalIds();
            hidWorkflowType.Value = DrpWorkflow.SelectedValue;
            Repeater1.DataSource = null;
            Repeater1.DataBind();
            if (subVerticalIds != string.Empty)
            {
                if (Convert.ToInt32(hidWorkflowType.Value)==(int)(Models.WorkFlow.BankWorkflow))
                {
                    string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_WORKFLOW, subVerticalIds), string.Empty, this);
                    List<BankWorkflowModel> lstBankWorkflow = JsonConvert.DeserializeObject<List<BankWorkflowModel>>(result);
                    Repeater1.DataSource = lstBankWorkflow;
                }
                else
                {
                    string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_TREASURY_WORKFLOW, subVerticalIds), string.Empty, this);
                    List<TreasuryWorkflowModel> lstTreasuryWorkflow = JsonConvert.DeserializeObject<List<TreasuryWorkflowModel>>(result);
                    Repeater1.DataSource = lstTreasuryWorkflow;
                }
            }
            Repeater1.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindWorkFlow();
        }
    }
}