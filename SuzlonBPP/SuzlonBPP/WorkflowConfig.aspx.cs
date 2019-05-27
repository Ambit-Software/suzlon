using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using SuzlonBPP.UserControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuzlonBPP
{
    public partial class WorkflowConfig : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        public List<Models.VerticalMaster> lstVerticalMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.WORKFLOW_CONFIGURATION, menuList)) Response.Redirect("~/webNoAccess.aspx");

                if (!IsPostBack)
                {
                    BindDropdownData();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindDropdownData()
        {
            try
            {
                string drpname = "workflow,subvertical-vertical,verticalcontroller,groupcontroller,treasury,managementassurance,fassc,cb,payment-proposer";
                string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                if (result != Constants.REST_CALL_FAILURE)
                    Session[Constants.DROPDOWN_VALUES_FOR_USERCONTROL] = result;
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                ComboSubVertical.DataSource = ddValues.SubVerticalWithVertical;
                ComboSubVertical.DataBind();
                DrpWorkflow.DataSource = ddValues.WorkFlow;
                DrpWorkflow.DataBind();
                DrpWorkflow.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Workflow", string.Empty));
                DrpWorkflow.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        [WebMethod(EnableSession = true)]
        public static string SaveBankWorkFlow(string data, int workflowType)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["Token"] != null)
                {
                    CommonFunctions commonFunctions = new CommonFunctions();
                    string result = string.Empty;
                    if (workflowType == (int)(Models.WorkFlow.BankWorkflow))
                        result = commonFunctions.RestServiceCall(Constants.SAVE_BANK_WORKFLOW, Crypto.Instance.Encrypt(data));
                    else if (workflowType == (int)(Models.WorkFlow.Treasury))
                        result = commonFunctions.RestServiceCall(Constants.SAVE_TREASURY_WORKFLOW, Crypto.Instance.Encrypt(data));
                    else if (workflowType == (int)(Models.WorkFlow.Payment))
                        result = commonFunctions.RestServiceCall(Constants.SAVE_PAYMENT_WORKFLOW, Crypto.Instance.Encrypt(data));
                }
                else
                    data = "Refresh Page";

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
            return data;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                DropdownValues ddValues = null;
                if (Session[Constants.DROPDOWN_VALUES_FOR_USERCONTROL] != null)
                    ddValues = JsonConvert.DeserializeObject<DropdownValues>(Convert.ToString(Session[Constants.DROPDOWN_VALUES_FOR_USERCONTROL]));               
                if (Convert.ToInt32(hidWorkflowType.Value) == (int)(Models.WorkFlow.BankWorkflow))
                {
                    var bankWorkflowModel = e.Item.DataItem as BankWorkflowModel;
                    var control = (BankWorkFlowControl)base.Page.LoadControl("UserControls/BankWorkflowControl.ascx");
                    Label lblName = control.FindControl("lblName") as Label;
                    Label btnSave = control.FindControl("lblSave") as Label;
                    Panel headerPanel = control.FindControl("headerPanel") as Panel;
                    headerPanel.Attributes.Add("onclick", "javascript:expandDiv(this)");
                    btnSave.Attributes.Add("onclick", "javascript:SaveBankDetails(this)");
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
                else if (Convert.ToInt32(hidWorkflowType.Value) == (int)(Models.WorkFlow.Treasury))
                {
                    var treasuryWorkflowModel = e.Item.DataItem as TreasuryWorkflowModel;
                    var control = (TreasuryWorkflowControl)base.Page.LoadControl("UserControls/TreasuryWorkflowControl.ascx");
                    Label lblName = control.FindControl("lblName") as Label;
                    Label btnSave = control.FindControl("lblSave") as Label;
                    Panel headerPanel = control.FindControl("headerPanel") as Panel;
                    headerPanel.Attributes.Add("onclick", "javascript:expandDiv(this)");
                    btnSave.Attributes.Add("onclick", "javascript:SaveBankDetails(this)");
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
                else if (Convert.ToInt32(hidWorkflowType.Value) == (int)(Models.WorkFlow.Payment))
                {
                    var paymentWorkflowModel = e.Item.DataItem as PaymentWorkflowModel;
                    var control = (PaymentWorkflowControl)base.Page.LoadControl("UserControls/PaymentWorkflowControl.ascx");
                    Label lblName = control.FindControl("lblName") as Label;
                    Label btnSave = control.FindControl("lblSave") as Label;
                    Panel headerPanel = control.FindControl("headerPanel") as Panel;
                    headerPanel.Attributes.Add("onclick", "javascript:expandDiv(this)");
                    btnSave.Attributes.Add("onclick", "javascript:SaveBankDetails(this)");
                    control.paymentWorkflowModel = paymentWorkflowModel;
                    if (paymentWorkflowModel.subVerticalMaster != null)
                    {
                        HiddenField hidSubVerticalId = control.FindControl("hidSubVerticalId") as HiddenField;
                        HiddenField hidVerticalId = control.FindControl("hidVerticalId") as HiddenField;
                        hidSubVerticalId.Value = Convert.ToString(paymentWorkflowModel.subVerticalMaster.SubVerticalId);
                        hidVerticalId.Value = Convert.ToString(paymentWorkflowModel.subVerticalMaster.VerticalId);
                    }
                    Repeater1.Controls.Add(control);
                }
            }
            catch (Exception ex)
            {   
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private string GetSubVerticalIds()
        {
            string subVerticalIds = string.Empty;
            try
            {
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
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
            return subVerticalIds;
        }

        private void BindWorkFlow()
        {
            try
            {
                string subVerticalIds = GetSubVerticalIds();
                hidWorkflowType.Value = DrpWorkflow.SelectedValue;
                Repeater1.DataSource = null;
                Repeater1.DataBind();
                if (subVerticalIds != string.Empty)
                {
                    if (Convert.ToInt32(hidWorkflowType.Value) == (int)(Models.WorkFlow.BankWorkflow))
                    {
                        string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_WORKFLOW, subVerticalIds), string.Empty);
                        List<BankWorkflowModel> lstBankWorkflow = JsonConvert.DeserializeObject<List<BankWorkflowModel>>(result);
                        Repeater1.DataSource = lstBankWorkflow;
                    }
                    else if (Convert.ToInt32(hidWorkflowType.Value) == (int)(Models.WorkFlow.Treasury))
                    {
                        string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_TREASURY_WORKFLOW, subVerticalIds), string.Empty);
                        List<TreasuryWorkflowModel> lstTreasuryWorkflow = JsonConvert.DeserializeObject<List<TreasuryWorkflowModel>>(result);
                        Repeater1.DataSource = lstTreasuryWorkflow;
                    }
                    else if (Convert.ToInt32(hidWorkflowType.Value) == (int)(Models.WorkFlow.Payment))
                    {
                        string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_PAYMENT_WORKFLOW, subVerticalIds), string.Empty);
                        List<PaymentWorkflowModel> lstPaymentWorkflow = JsonConvert.DeserializeObject<List<PaymentWorkflowModel>>(result);
                        Repeater1.DataSource = lstPaymentWorkflow;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
            Repeater1.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindWorkFlow();
        }
        protected void btnAuditTrail_Click(object sender, EventArgs e)
        {
            try
            {
                string subverticals = GetSubVerticalIds();
                BindAuditTrail(Convert.ToInt32(DrpWorkflow.SelectedValue), subverticals);
                grdWorkFlowAuditTrial.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowAuditTrail();", true);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        public void BindAuditTrail(int WorkFlowId, string SubVerticalId)
        {
            try
            {
                string result = string.Empty;

                result = commonFunctions.RestServiceCall(string.Format(Constants.BANKWORKFLOW_AUDITTRAIL, WorkFlowId, SubVerticalId), string.Empty);

                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    List<GetWorkFlowkAuditTrailDetail_Result> lstBankDetailAuditTrial = JsonConvert.DeserializeObject<List<GetWorkFlowkAuditTrailDetail_Result>>(result);
                    if (lstBankDetailAuditTrial.Count > 0)
                    {
                        grdWorkFlowAuditTrial.DataSource = lstBankDetailAuditTrial;
                    }
                    else
                    {
                        grdWorkFlowAuditTrial.DataSource = new DataTable();
                    }
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        protected void grdWorkFlowAuditTrial_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (DrpWorkflow.SelectedValue != "")
                {
                    string subverticals = GetSubVerticalIds();
                    BindAuditTrail(Convert.ToInt32(DrpWorkflow.SelectedValue), subverticals);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}