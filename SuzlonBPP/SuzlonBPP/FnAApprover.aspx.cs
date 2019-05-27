using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SuzlonBPP.Controllers;
using System.Xml.Linq;

namespace SuzlonBPP
{
    
    public partial class FnAApprover : System.Web.UI.Page
    {
        double TotalBalance = 0.00;
        CommonFunctions commonFunctions = new CommonFunctions();
        ApproverModel objApproverModel = new ApproverModel();
        PaymentWorkflowController paymentWorkflowController = new PaymentWorkflowController();
        SuzlonBPPEntities objSuzEntity = new SuzlonBPPEntities();

        public string XMLPayment { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                    if (menuList == null) Response.Redirect("~/Login.aspx", false);
                    if (!PageSecurity.IsAccessGranted(PageSecurity.F_AND_A_APPROVER, menuList)) Response.Redirect("~/webNoAccess.aspx");
                   

                    //lnkBudget.Text = Convert.ToString(getVerticalbudget(cmbVertical.SelectedValue));
                    GetProfiles();
                    GetStatus();
                    BindVertical();
                    Session["ValidateHouseBank"] = "false";
                    Session["HouseBank"] = string.Empty;
                   
                    string ProfileName = Convert.ToString(HttpContext.Current.Session["ProfileName"]);
                    if (ProfileName.ToUpper() == "ADMIN")
                    {
                        RadTab rootTab = RadTabStrip1.FindTabByText("Paid Request");
                        rootTab.Visible = true;
                        RadPageView3.Visible = true;
                        gvReverse.DataSource = null;
                        gvReverse.DataBind();
                        BindReverseReason();

                        RadTab rootTabReversed = RadTabStrip1.FindTabByText("Reversed Request");
                        rootTabReversed.Visible = true;
                        RadPageView4.Visible = true;

                        gvReversedRequest.DataSource = null;
                        gvReversedRequest.DataBind();

                        //lblINRAmtText.Visible = false;
                        //lblINRAmt.Visible = false;
                        //lblProposedAmtText.Visible = false;
                        //lblProposedAmt.Visible = false;
                        //RadTab rootTabPending = RadTabStrip1.FindTabByText("Pending for Approval");
                        //rootTabPending.Visible = false;
                        //RadPageView1.Visible = false;
                        //RadTab rootTabApprove = RadTabStrip1.FindTabByText("Approved Request");
                        //rootTabApprove.Visible = false;
                        //RadPageView2.Visible = false;
                    }
                    else
                    {
                        RadTab rootTab = RadTabStrip1.FindTabByText("Paid Request");
                        rootTab.Visible = false;
                        RadPageView3.Visible = false;


                        RadTab rootTabReversed = RadTabStrip1.FindTabByText("Reversed Request");
                        rootTabReversed.Visible = false;
                        RadPageView4.Visible = false;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void fillReveresData()
        {
            string ProfileName = Convert.ToString(HttpContext.Current.Session["ProfileName"]);
            if (ProfileName.ToUpper() == "ADMIN")
            {
                if (!string.IsNullOrEmpty(Convert.ToString(cmbVertical.SelectedValue)) && (rbtnBill.Checked||rbtnAdvance.Checked))
                {
                    List<sp_GetReversePaymentDetail_Result> lstDetail = (List<sp_GetReversePaymentDetail_Result>)paymentWorkflowController.fillReversePaymentDetail(Convert.ToInt32(cmbVertical.SelectedValue), rbtnBill.Checked ? "Against" : "Advance");
                    gvReverse.DataSource = lstDetail;
                    
                }
                else
                {
                    gvReverse.DataSource = null;
                   
                }

            }
            else
            {
                gvReverse.DataSource = null;
                
            }

        }
        private void fillReversedDoneData()
        {
            string ProfileName = Convert.ToString(HttpContext.Current.Session["ProfileName"]);
            if (ProfileName.ToUpper() == "ADMIN")
            {
                if (!string.IsNullOrEmpty(Convert.ToString(cmbVertical.SelectedValue)) && (rbtnBill.Checked || rbtnAdvance.Checked))
                {
                    List<Usp_GetReversedPaymentDone_Result> lstDetail = (List<Usp_GetReversedPaymentDone_Result>)paymentWorkflowController.GetReversedDetail(Convert.ToInt32(cmbVertical.SelectedValue), rbtnBill.Checked ? "Against" : "Advance");
                    gvReversedRequest.DataSource = lstDetail;

                }
                else
                {
                    gvReversedRequest.DataSource = null;

                }

            }
            else
            {
                gvReverse.DataSource = null;

            }

        }

        private void BindReverseReason()
        {           
            List <Models.ListItem> lstReason  = (List<Models.ListItem>)paymentWorkflowController.fillReverseReason();
            Session["ReverseReason"] = lstReason;
        }

        private void GetProfiles()
        {
            try
            {
                string drpname = "profile";
                string commonDrpValues = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                if (commonDrpValues == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(commonDrpValues);
                    ViewState["Profiles"] = ddValues.Profile;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void GetStatus()
        {
            try
            {
                List<SuzlonBPP.Models.ListItem> lstStatus = paymentWorkflowController.GetStatusList();
                ViewState["Status"] = lstStatus;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindVertical()
        {
            try
            {
                string vendorCode = string.Empty;
                string drpname = "vertical";
                string commonDrpValues = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                if (commonDrpValues == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(commonDrpValues);
                    string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETUSERDTL + Convert.ToString(HttpContext.Current.Session["UserId"]) + "", string.Empty);

                    if (result != Constants.REST_CALL_FAILURE)
                    {
                        UserModel UserInfo = JsonConvert.DeserializeObject<UserModel>(result);
                        if (UserInfo != null)
                        {
                            string[] strArr = UserInfo.UserDetail.Vertical.Split(',');
                            if (strArr.Length > 0)
                            {
                                ddValues.Vertical = (from c in ddValues.Vertical
                                                     where strArr.Contains(c.Id)
                                                     select c).ToList();

                                if (ddValues.Vertical.Count > 0)
                                {
                                    cmbVertical.DataTextField = "Name";
                                    cmbVertical.DataValueField = "Id";
                                    cmbVertical.DataSource = ddValues.Vertical;
                                    cmbVertical.DataBind();

                                    if (ddValues.Vertical.Count > 1)
                                    {
                                        cmbVertical.Items.Insert(0, new RadComboBoxItem("Select Vertical", String.Empty));
                                    }
                                    cmbVertical.SelectedIndex = 0;
                                    lnkVerticalBudget1.Text = Convert.ToString(paymentWorkflowController.getVerticalBalance(Convert.ToInt32(cmbVertical.SelectedValue)));
                    
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void cmbVertical_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmbVertical.SelectedValue != "")
                {
                    BindGrid();
                    // lnkVerticalBudget1.Text = Convert.ToString(paymentWorkflowController.getVerticalBalance(Convert.ToInt32(cmbVertical.SelectedValue)));
                    string VerticalBudget = Convert.ToString(paymentWorkflowController.getVerticalBalance(Convert.ToInt32(cmbVertical.SelectedValue)));
                    string formattedNumber = string.Format("{0:##,#0.00}", decimal.Parse(VerticalBudget), System.Globalization.CultureInfo.InvariantCulture);
                    lnkVerticalBudget1.Text = formattedNumber;
                    Session["ValidateHouseBank"] = "false";
                    Session["HouseBank"] = string.Empty;

                    lblINRAmt.Text = "0";
                    lblProposedAmt.Text ="0";

                    lnkVerticalBudget1.Text = Convert.ToString(paymentWorkflowController.getVerticalBalance(Convert.ToInt32(cmbVertical.SelectedValue)));

                    string ProfileName = Convert.ToString(HttpContext.Current.Session["ProfileName"]);
                    if (ProfileName.ToUpper() == "ADMIN")
                    {
                        fillReveresData();
                        gvReverse.DataBind();
                        fillReversedDoneData();
                        gvReversedRequest.DataBind();
                    }
                    

                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindGrid()
        {
            try
            {
                string strCompanyData = string.Empty;
                List<GetCompanyDetailsForFnA_Result> lstGetCompanyData = null;

                if (rbtnAdvance.Checked == true)
                {
                    if (cmbVertical.SelectedValue == "")
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.SELECTVERTICAL);
                        return;
                    }

                    if (RadTabStrip1.SelectedIndex == 0)  //For pending requests
                    {
                        lstGetCompanyData = objSuzEntity.GetCompanyDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToInt32(Session["UserId"]), "2", "1").ToList();
                        gvPendingApproval.DataSource = lstGetCompanyData;
                        gvPendingApproval.DataBind();
                    }

                    if (RadTabStrip1.SelectedIndex == 1)  //For Approved
                    {
                        lstGetCompanyData = objSuzEntity.GetCompanyDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToInt32(Session["UserId"]), "2", "2").ToList();
                        gvApproved.DataSource = lstGetCompanyData;
                        gvApproved.DataBind();
                    }
                }
                if (rbtnBill.Checked == true)
                {
                    if (cmbVertical.SelectedValue == "")
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.SELECTVERTICAL);
                        return;
                    }

                    if (RadTabStrip1.SelectedIndex == 0)  //For pending requests
                    {
                        lstGetCompanyData = objSuzEntity.GetCompanyDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToInt32(Session["UserId"]), "1", "1").ToList();
                        gvPendingApproval.DataSource = lstGetCompanyData;
                        gvPendingApproval.DataBind();
                    }

                    if (RadTabStrip1.SelectedIndex == 1)  //For Approved
                    {
                        lstGetCompanyData = objSuzEntity.GetCompanyDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToInt32(Session["UserId"]), "1", "2").ToList();
                        gvApproved.DataSource = lstGetCompanyData;
                        gvApproved.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindDataSource()
        {
            try
            {
                string strCompanyData = string.Empty;
                List<GetCompanyDetailsForFnA_Result> lstGetCompanyData = null;

                if (rbtnAdvance.Checked == true)
                {
                    if (cmbVertical.SelectedValue == "")
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.SELECTVERTICAL);
                        return;
                    }

                    if (RadTabStrip1.SelectedIndex == 0)  //For pending requests
                    {
                        lstGetCompanyData = objSuzEntity.GetCompanyDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToInt32(Session["UserId"]), "2", "1").ToList();
                        gvPendingApproval.DataSource = lstGetCompanyData;
                    }

                    if (RadTabStrip1.SelectedIndex == 1)  //For Approved
                    {
                        lstGetCompanyData = objSuzEntity.GetCompanyDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToInt32(Session["UserId"]), "2", "2").ToList();
                        gvApproved.DataSource = lstGetCompanyData;
                    }
                }
                if (rbtnBill.Checked == true)
                {
                    if (cmbVertical.SelectedValue == "")
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.SELECTVERTICAL);
                        return;
                    }

                    if (RadTabStrip1.SelectedIndex == 0)  //For pending requests
                    {
                        lstGetCompanyData = objSuzEntity.GetCompanyDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToInt32(Session["UserId"]), "1", "1").ToList();
                        gvPendingApproval.DataSource = lstGetCompanyData;
                    }

                    if (RadTabStrip1.SelectedIndex == 1)  //For Approved
                    {
                        lstGetCompanyData = objSuzEntity.GetCompanyDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToInt32(Session["UserId"]), "1", "2").ToList();
                        gvApproved.DataSource = lstGetCompanyData;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvPendingApproval_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                RadGrid grdVendor = null;

                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    grdVendor = (e.Item as GridDataItem).ChildItem.FindControl("gvPendingApproval1") as RadGrid;
                }


                if (e.CommandName == RadGrid.ExpandCollapseCommandName)
                {
                    string strCompanyNm = Convert.ToString((e.Item as GridDataItem).GetDataKeyValue("Company"));

                    if (strCompanyNm != "")
                    {
                        string strCompanyCode = string.Empty;

                        string[] strSplit = strCompanyNm.Split('-');

                        if (strSplit.Length > 1)
                        {
                            strCompanyCode = strSplit[1];
                            Session["CompanyCode"] = strCompanyCode;
                            Session["CompanyName"] = Convert.ToString(strSplit[0]);
                        }

                        List<GetVendorDetailsForFnA_Result> lstGetVendorData = null;

                        if (rbtnAdvance.Checked == true)
                            lstGetVendorData = objSuzEntity.GetVendorDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), strCompanyCode, Convert.ToInt32(Session["UserId"]), "2", "1").ToList();

                        if (rbtnBill.Checked == true)
                            lstGetVendorData = objSuzEntity.GetVendorDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), strCompanyCode, Convert.ToInt32(Session["UserId"]), "1", "1").ToList();

                        if (lstGetVendorData != null)
                        {
                           
                                grdVendor.DataSource = lstGetVendorData;
                                grdVendor.DataBind();
                            
                           
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvPendingApproval1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                RadGrid grdVendorItems = null;

                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    grdVendorItems = (e.Item as GridDataItem).ChildItem.FindControl("gvPendingApproval2") as RadGrid;

                    GridDataItem grdItem = (GridDataItem)e.Item;
                    string strVendorNm = grdItem["Vendor"].Text;

                    if (strVendorNm != "")
                    {
                        string[] strSplit = strVendorNm.Split('-');

                        string strVendorCode = string.Empty;

                        if (strSplit.Length > 1)
                        {
                            strVendorCode = strSplit[1];
                            Session["VendorCode"] = strVendorCode;
                            Session["VendorName"] = Convert.ToString(strSplit[0]);
                        }

                        List<GetApproverEndItems_Result> lstGetVendorItems = null;

                        if (rbtnAdvance.Checked == true)
                            lstGetVendorItems = objSuzEntity.GetApproverEndItems(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToString(Session["CompanyCode"]), strVendorCode, Convert.ToInt32(HttpContext.Current.Session["UserId"]), "2", "1").ToList();

                        if (rbtnBill.Checked == true)
                            lstGetVendorItems = objSuzEntity.GetApproverEndItems(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToString(Session["CompanyCode"]), strVendorCode, Convert.ToInt32(HttpContext.Current.Session["UserId"]), "1", "1").ToList();

                        if (lstGetVendorItems != null)
                        {
                            grdVendorItems.DataSource = lstGetVendorItems;
                            grdVendorItems.DataBind();
                        }
                    }
                    //22 Aug 16 For Hide Show col in Advance & Against Bill 

                    if (rbtnBill.Checked == true)
                    {
                        grdVendorItems.MasterTableView.GetColumnSafe("DocumentNumber").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("DPRNumber").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("PurchasingDocument").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("ExpectedClearingDate").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("SpecialGL").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("WithholdingTaxCode").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("UnsettledOpenAdvance").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("JustificationforAdvPayment").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("TaxRate").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("TaxAmount").Visible = false;
                    }
                    if (rbtnAdvance.Checked == true)
                    {
                        grdVendorItems.MasterTableView.GetColumnSafe("DocumentNumber").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("NetDueDate").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("Vertical").Visible = false;

                        grdVendorItems.MasterTableView.GetColumnSafe("DPRNumber").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("PurchasingDocument").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("ExpectedClearingDate").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("SpecialGL").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("WithholdingTaxCode").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("UnsettledOpenAdvance").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("JustificationforAdvPayment").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("TaxRate").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("TaxAmount").Visible = true;

                        grdVendorItems.MasterTableView.GetColumnSafe("Amount").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("AmountProposed").Visible = false;
          
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnRequestSave_Click(object sender, EventArgs e)
        {
            try
            {

                if(Convert.ToString(Session["ValidateHouseBank"]) == "true")
                { 
                decimal DebitAmt = 0;
                decimal CreditAmt = 0;

                LinkButton lnk = (LinkButton)sender;
                Session["sender"] = lnk;

                GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl("gvPendingApproval2")).Items;

                decimal dApprovedAmount = 0;

                var result = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, Convert.ToString(Session["VendorCode"]), Convert.ToString(Session["CompanyCode"])), string.Empty);
                if (Convert.ToBoolean(result))
                {
                    //radMessage.Title = "Alert";
                    //radMessage.Show(Constants.ISVENDORBANKDETAILUPDATE_INPROCESSMSG);
                    //return;
                    string msg = "For Vendor " + Convert.ToString(Session["VendorCode"]) + " - " + Convert.ToString(Session["VendorName"]) + " and Company " + Convert.ToString(Session["CompanyCode"]) + " - " + Convert.ToString(Session["CompanyName"]) + " bank details updation is in Progress you cannot submit the record for payment.";
                    radMessage.Title = "Alert";
                    radMessage.Show(msg.ToString());
                    return;
                }

                bool bRequiredFlag = false;

                foreach (GridDataItem item in collection)
                {
                    if (item.Selected)
                    {
                        string strAmount = item["Amount"].Text;

                        if (string.IsNullOrEmpty(strAmount.Trim()))
                            strAmount = "0";

                        decimal dAmount = Convert.ToDecimal(strAmount);

                        string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                        if (String.IsNullOrEmpty(ApprovedAmount.Trim()))
                        {
                            ApprovedAmount = "0";
                        }

                        decimal dApprAmt = Convert.ToDecimal(ApprovedAmount);

                        RadLabel lblAmt = (RadLabel)item.FindControl("lblApprovedAmount");
                        if (dApprAmt > dAmount)
                        {
                            lblAmt.Visible = true;
                            bRequiredFlag = true;
                            continue;
                        }
                        else
                        {
                            lblAmt.Visible = false;
                        }

                        string strTransStatusId = Convert.ToString(item["TransactionStatusId"].Text);
                        string strRemark = ((RadTextBox)item.FindControl("tbRemark")).Text;
                        string strStatus = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedItem.Text;

                        RadLabel lbl = (RadLabel)item.FindControl("lblRemark");
                        lbl.Visible = false;

                        if (strStatus != "Approved")
                        {
                            if (string.IsNullOrEmpty(strRemark))
                            {
                                lbl.Visible = true;
                                bRequiredFlag = true;
                                return;
                            }
                            else
                            {
                                lbl.Visible = false;
                            }
                        }

                        if (strStatus.Trim() == "Approved" && strTransStatusId.Trim() != "11")
                        {
                            dApprovedAmount = dApprovedAmount + Convert.ToDecimal(ApprovedAmount);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(item["DCFLag"].Text)))
                        {
                                //if (Convert.ToString(item["DCFLag"].Text) == "S" && (!string.IsNullOrEmpty(Convert.ToString(ApprovedAmount))))
                                //    DebitAmt = DebitAmt + Convert.ToDecimal(ApprovedAmount);
                                //else if (Convert.ToString(item["DCFLag"].Text) == "H" && (!string.IsNullOrEmpty(Convert.ToString(ApprovedAmount))))
                                //    CreditAmt = CreditAmt + Convert.ToDecimal(ApprovedAmount);

                                if (Convert.ToString(item["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(ApprovedAmount))))
                                    DebitAmt = DebitAmt + Convert.ToDecimal(ApprovedAmount);
                                else if (Convert.ToString(item["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(ApprovedAmount))))
                                    CreditAmt = CreditAmt + Convert.ToDecimal(ApprovedAmount);



                            }


                    }
                }
                if (DebitAmt >= CreditAmt)
                {
                    radMessage.Title = "Alert";
                    radMessage.Show("Its a debit balance.Hence could not proceed");
                    return;
                }



                if (bRequiredFlag == true)
                    return;

                string strVendorId = Convert.ToString(Session["VendorCode"]);
                string strCompanyId = Convert.ToString(Session["CompanyCode"]);

                bool bValid = false;

                    //bValid = checkTodayPaidTotalByCB(dApprovedAmount);
                    bValid = (CreditAmt - DebitAmt <= 20000000 ? true : false);
                    if (bValid == false)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.DAILYPAIDLIMITMSG_CB);
                    return;
                }

                bValid = checkTodayPaidTotal(strVendorId, strCompanyId, dApprovedAmount);
                if (bValid == false)
                {
                    RadAjaxManager1.ResponseScripts.Add("saveConfirmation('save')");
                }
                else
                {
                    if (rbtnBill.Checked)
                    {
                        ViewState["Sender"] = "GRD";
                        SaveAgainstData(lnk, "Save", true);
                    }
                    else
                    {
                        ViewState["Sender"] = "GRD";
                        SaveAdvnaceData(lnk, "Save", true);
                    }
                }

                BindGrid();
            }
            else
            {
                    radMessage.Title = "Alert";
                    radMessage.Show("Please Validate & Apply the House Bank Code");
                    return;
             }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private bool checkTodayPaidTotalByCB(decimal dVendorItmSum)
        {
            try
            {

                System.Data.Entity.Core.Objects.ObjectParameter TodayApprovedAmount = new System.Data.Entity.Core.Objects.ObjectParameter("TodayApprovedAmount", typeof(Decimal));
                objSuzEntity.GetTodaysPaymentAmount(TodayApprovedAmount);

                decimal dBudget = Convert.ToDecimal(TodayApprovedAmount.Value);

                if ((dBudget + dVendorItmSum) <= 20000000)               
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void SaveAgainstData(LinkButton lnk, string strMode, bool bValid)
        {
            decimal? dAmount = 0;
            Boolean IsCallToAllocation = false;
            if (String.Compare(Convert.ToString(ViewState["Sender"]), "ACT") == 0)
            {
                lnk = (LinkButton)Session["btSender"];
                bValid = (bool)ViewState["bValid"];
            }
            else
            {
                Session["btSender"] = lnk;
                ViewState["bValid"] = bValid;
            }

            List<PaymentDetailAgainstBillTxn> lstTXns = GetTransactionsAgaintList(lnk, strMode, bValid);
            List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];
            int ApprovedStatusId = Convert.ToInt32(lststatus.Where(c => c.Name.ToLower() == "approved").FirstOrDefault().Id);
            dAmount = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Sum(c => c.AppovedAmount);
            ViewState["ApprovedCount"] = Convert.ToString(lstTXns.Where(c => c.StatusId == ApprovedStatusId).Count());

            if (String.Compare(strMode, "Submit") == 0)
            {
                if (String.Compare(Convert.ToString(ViewState["Sender"]), "ACT") == 0)
                {
                  // TMP  paymentWorkflowController.SavePaymentAgainstDetailTxn_Adv(lstTXns); // SavePaymentTransactions(lstTXns);
                }
            }

            List<PaymentDetailAgainstBill> lstPaymentDetailAgainst = new List<PaymentDetailAgainstBill>();
            foreach (PaymentDetailAgainstBillTxn obj in lstTXns)
            {
                PaymentDetailAgainstBill objPaymentDetailAgainst = new PaymentDetailAgainstBill();
                objPaymentDetailAgainst.SAPAgainstBillPaymentId = obj.SAPAgainstBillPaymentId;
                objPaymentDetailAgainst.StatusId = obj.StatusId;
                objPaymentDetailAgainst.ApprovedAmount = obj.AppovedAmount;
                objPaymentDetailAgainst.PaymentWorkflowStatusId = obj.PaymentWorkflowStatusId;
                objPaymentDetailAgainst.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                objPaymentDetailAgainst.ModifiedOn = DateTime.Now;
                objPaymentDetailAgainst.Comment = obj.Comment;
                
                if (String.Compare(strMode, "Submit") == 0)
                {
                    //objPaymentDetailAgainst.PaymentWorkflowStatusId = 17;   
                    objPaymentDetailAgainst.ApprovedLineGroupID = paymentWorkflowController.getNewApprovedGroupId();

                    //if (bValid == false)
                    //{
                    //    if (objPaymentDetailAgainst.TransactionStatusId != 11)
                    //        objPaymentDetailAgainst.PaymentWorkflowStatusId = 13;     //Sent to approver for need correction
                    //}
                    objPaymentDetailAgainst.SaveMode = false;
                }
                objPaymentDetailAgainst.SaveMode = true;
                lstPaymentDetailAgainst.Add(objPaymentDetailAgainst);
                Session["SubmitPayment"] = lstPaymentDetailAgainst;
                XMLPayment = Convert.ToString(new XElement("Root", from i in lstPaymentDetailAgainst
                                                                   select new XElement("Payment",
                                                                   new XElement("SAPPaymentId", i.SAPAgainstBillPaymentId),
                                                                   new XElement("SAPAmount", i.ApprovedAmount))));
                ViewState["SelectedSAPIDs"] = Convert.ToString(XMLPayment);
            }
            List<PaymentAllocationDetail> objAllocation = null;
            if (String.Compare(strMode, "Submit") == 0 && Convert.ToString(ViewState["Sender"]) != "ACT")
            {
                string strProfileName = Convert.ToString(Session["ProfileName"]);

                if (strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                {
                    IsCallToAllocation = true;
                }
                else
                {
                    String isValidRecords = paymentWorkflowController.ValidPaymentAllocation((rbtnAdvance.Checked ? "AD" : "AG"), Convert.ToString(XMLPayment));
                    if (isValidRecords == "T")
                    {
                        IsCallToAllocation = false;
                    }
                    else
                    {
                        IsCallToAllocation = true;
                    }
                }

                if (IsCallToAllocation == true && Convert.ToInt32(ViewState["ApprovedCount"]) > 0)
                {
                    ViewState["TotalUserAmount"] = Convert.ToString(dAmount);
                    Session["AllocationTable"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openAllocationPopup('" + Convert.ToString(dAmount) + "')", true);
                    grdBudgetAllocation.DataSource = paymentWorkflowController.GetBudgetAllocationDetails((rbtnAdvance.Checked ? "AD" : "AG"), Convert.ToString(ViewState["SelectedSAPIDs"])); ;
                    grdBudgetAllocation.DataBind();
                }
                else
                {
                    objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);                    
                    string result = paymentWorkflowController.UpdatePaymentDetail_Agnt(lstPaymentDetailAgainst, objAllocation, strMode, Convert.ToString(Session["HouseBank"]));
                    if (result == Constants.DETAIL_SAVE_SUCCESS)
                    {
                        paymentWorkflowController.SavePaymentAgainstDetailTxn_Adv(lstTXns);
                        BindGrid();
                        ViewState["ApprovedCount"] = "0";
                        RefreshCurrentAllocation();
                    }

                    radMessage.Show(result);
                    return;
                }

                objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
            }
            if (String.Compare(strMode, "Submit") == 0)
            {
                objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
                if (String.Compare(Convert.ToString(ViewState["Sender"]), "ACT") == 0 || (IsCallToAllocation != true))
                {                    
                    
                    string result = paymentWorkflowController.UpdatePaymentDetail_Agnt(lstPaymentDetailAgainst, objAllocation,strMode, Convert.ToString(Session["HouseBank"])); // call from allocation popup only if called
                    if (result == Constants.DETAIL_SAVE_SUCCESS)
                    {
                        paymentWorkflowController.SavePaymentAgainstDetailTxn_Adv(lstTXns);
                        BindGrid();
                        ViewState["ApprovedCount"] = "0";
                        RefreshCurrentAllocation();
                    }
                    radMessage.Show(result);
                }

            }
            else
            {
                objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
                
                string result= paymentWorkflowController.UpdatePaymentDetail_Agnt(lstPaymentDetailAgainst, objAllocation,strMode, Convert.ToString(Session["HouseBank"])); // call from allocation popup only if called
                if (result == Constants.DETAIL_SAVE_SUCCESS)
                {
                    paymentWorkflowController.SavePaymentAgainstDetailTxn_Adv(lstTXns);
                    BindGrid();
                    RefreshCurrentAllocation();
                }
                radMessage.Show(result);
            }

        }

        protected void RefreshCurrentAllocation()
        {
            string VerticalBudget = Convert.ToString(paymentWorkflowController.getVerticalBalance(Convert.ToInt32(cmbVertical.SelectedValue)));
            string formattedNumber = string.Format("{0:##,#0.00}", decimal.Parse(VerticalBudget), System.Globalization.CultureInfo.InvariantCulture);
            lnkVerticalBudget1.Text = formattedNumber;
        }


        protected void SaveAdvnaceData(LinkButton lnk, string strMode, bool bValid)
        {
            try
            {
                decimal? dAmount = 0;
                Boolean IsCallToAllocation = false;
                if (String.Compare(Convert.ToString(ViewState["Sender"]), "ACT") == 0)
                {
                    lnk = (LinkButton)Session["btSender"];
                    bValid = (bool)ViewState["bValid"];
                }
                else
                {
                    Session["btSender"] = lnk;
                    ViewState["bValid"] = bValid;
                }

                List<PaymentDetailAdvanceTxn> lstTXns = GetTransactionsList(lnk, strMode, bValid);
                List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];
                int ApprovedStatusId = Convert.ToInt32(lststatus.Where(c => c.Name.ToLower() == "approved").FirstOrDefault().Id);
                dAmount = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Sum(c => c.AppovedAmount);
                ViewState["ApprovedCount"] = Convert.ToString(lstTXns.Where(c => c.StatusId == ApprovedStatusId).Count());
                if (String.Compare(strMode, "Submit") == 0)
                {
                    if (String.Compare(Convert.ToString(ViewState["Sender"]), "ACT") == 0)
                    {
                       // TMP paymentWorkflowController.SavePaymentDetailTxn_Adv(lstTXns);
                    }
                }

                List<PaymentDetailAdvance> lstPaymentDetailAdvance = new List<PaymentDetailAdvance>();
                foreach (PaymentDetailAdvanceTxn obj in lstTXns)
                {
                    PaymentDetailAdvance objPaymentDetailAdvance = new PaymentDetailAdvance();
                    objPaymentDetailAdvance.SAPAdvancePaymentId = obj.SAPAdvancePaymentId;
                    objPaymentDetailAdvance.StatusId = obj.StatusId;
                    objPaymentDetailAdvance.PaymentWorkFlowStatusId = obj.PaymentWorkflowStatusId;
                    objPaymentDetailAdvance.ApprovedAmount = obj.AppovedAmount;
                    objPaymentDetailAdvance.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                    objPaymentDetailAdvance.ModifiedOn = DateTime.Now;
                    objPaymentDetailAdvance.Comment = obj.Comment;

                    if (String.Compare(strMode, "Submit") == 0)
                    {
                        //objPaymentDetailAdvance.PaymentWorkFlowStatusId = 17;    
                        objPaymentDetailAdvance.ApprovedLineGroupID = paymentWorkflowController.getNewApprovedGroupId();

                        //if (bValid == false)
                        //{
                        //    if (objPaymentDetailAdvance.TransactionStatusId != 11)
                        //        objPaymentDetailAdvance.PaymentWorkFlowStatusId = 13;     //Sent to approver for need correction
                        //}
                        objPaymentDetailAdvance.SaveMode = false;
                    }
                    objPaymentDetailAdvance.SaveMode = true;
                    lstPaymentDetailAdvance.Add(objPaymentDetailAdvance);

                    Session["SubmitPayment"] = lstPaymentDetailAdvance;
                    XMLPayment = Convert.ToString(new XElement("Root", from i in lstPaymentDetailAdvance
                                                                       select new XElement("Payment",
                                                                       new XElement("SAPPaymentId", i.SAPAdvancePaymentId),
                                                                       new XElement("SAPAmount", i.ApprovedAmount))));
                    ViewState["SelectedSAPIDs"] = Convert.ToString(XMLPayment);
                }

                List<PaymentAllocationDetail> objAllocation = null;
                if (String.Compare(strMode, "Submit") == 0 && Convert.ToString(ViewState["Sender"]) != "ACT")
                {
                    string strProfileName = Convert.ToString(Session["ProfileName"]);

                    if (strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                    {
                        IsCallToAllocation = true;
                    }
                    else
                    {
                        String isValidRecords = paymentWorkflowController.ValidPaymentAllocation((rbtnAdvance.Checked ? "AD" : "AG"), Convert.ToString(XMLPayment));
                        if (isValidRecords == "T")
                        {
                            IsCallToAllocation = false;
                        }
                        else
                        {
                            IsCallToAllocation = true;
                        }
                    }

                    if (IsCallToAllocation == true && Convert.ToInt32(ViewState["ApprovedCount"]) > 0)
                    {
                        ViewState["TotalUserAmount"] = Convert.ToString(dAmount);
                        Session["AllocationTable"] = null;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openAllocationPopup('" + Convert.ToString(dAmount) + "')", true);
                        grdBudgetAllocation.DataSource = paymentWorkflowController.GetBudgetAllocationDetails((rbtnAdvance.Checked ? "AD" : "AG"), Convert.ToString(ViewState["SelectedSAPIDs"])); ;
                        grdBudgetAllocation.DataBind();
                    }
                    else
                    {
                        objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);                        
                        string result = paymentWorkflowController.UpdatePaymentDetail_Adv(lstPaymentDetailAdvance, objAllocation,strMode, Convert.ToString(Session["HouseBank"]));
                        if (result == Constants.DETAIL_SAVE_SUCCESS)
                        {
                            paymentWorkflowController.SavePaymentDetailTxn_Adv(lstTXns);
                            BindGrid();
                            ViewState["ApprovedCount"] = "0";
                            RefreshCurrentAllocation();                        
                        }

                        radMessage.Show(result);
                        return;
                    }
                    objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
                }

                if (String.Compare(strMode, "Submit") == 0)
                {
                    if (String.Compare(Convert.ToString(ViewState["Sender"]), "ACT") == 0 || (IsCallToAllocation != true))
                    {
                        objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
                        
                        string result = paymentWorkflowController.UpdatePaymentDetail_Adv(lstPaymentDetailAdvance, objAllocation,strMode, Convert.ToString(Session["HouseBank"]));
                        if (result == Constants.DETAIL_SAVE_SUCCESS)
                        {
                            paymentWorkflowController.SavePaymentDetailTxn_Adv(lstTXns);

                            BindGrid();
                            ViewState["ApprovedCount"] = "0";
                            RefreshCurrentAllocation();
                        }
                        radMessage.Show(result);
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnRequestSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Convert.ToString(Session["ValidateHouseBank"]) == "true")
                //{

                    LinkButton lnk = (LinkButton)sender;
                    Session["sender"] = lnk;

                    decimal DebitAmt = 0;
                    decimal CreditAmt = 0;

                    GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl("gvPendingApproval2")).Items;

                    decimal dApprovedAmount = 0;
                    var result = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, Convert.ToString(Session["VendorCode"]), Convert.ToString(Session["CompanyCode"])), string.Empty);
                    if (Convert.ToBoolean(result))
                    {
                        //radMessage.Title = "Alert";
                        //radMessage.Show(Constants.ISVENDORBANKDETAILUPDATE_INPROCESSMSG);
                        //return;
                        string msg = "For Vendor " + Convert.ToString(Session["VendorName"]) + " - " + Convert.ToString(Session["VendorCode"]) + " and Company " + Convert.ToString(Session["CompanyName"]) + " - " + Convert.ToString(Session["CompanyCode"]) + " bank details updation is in Progress you cannot submit the record for payment.";
                        radMessage.Title = "Alert";
                        radMessage.Show(msg.ToString());
                        return;
                    }

                    bool bRequiredFlag = false;

                    foreach (GridDataItem item in collection)
                    {
                        if (item.Selected)
                        {
                            string strAmount = item["Amount"].Text;

                            if (string.IsNullOrEmpty(strAmount.Trim()))
                                strAmount = "0";

                            decimal dAmount = Convert.ToDecimal(strAmount);

                            string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                            if (String.IsNullOrEmpty(ApprovedAmount.Trim()))
                            {
                                ApprovedAmount = "0";
                            }

                            decimal dApprAmt = Convert.ToDecimal(ApprovedAmount);

                            RadLabel lblAmt = (RadLabel)item.FindControl("lblApprovedAmount");
                            if (dApprAmt > dAmount)
                            {
                                // lblAmt.Visible = true;
                                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                                radMessage.Show(Constants.APPROVEDAMOUNTVALIDATION);
                                bRequiredFlag = true;
                                return;
                                //continue;
                            }
                            else
                            {
                                lblAmt.Visible = false;
                            }

                            string strTransStatusId = Convert.ToString(item["TransactionStatusId"].Text);
                            string strRemark = ((RadTextBox)item.FindControl("tbRemark")).Text;
                            string strStatus = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedItem.Text;
                            string houseBankText = Convert.ToString(((RadTextBox)item.FindControl("tbHouseBank")).Text);
                            if (strStatus != "Approved")
                            {
                                RadLabel lbl = (RadLabel)item.FindControl("lblRemark");
                                lbl.Visible = false;

                                if (string.IsNullOrEmpty(strRemark))
                                {
                                    lbl.Visible = true;
                                    bRequiredFlag = true;
                                    continue;
                                }
                                else
                                {
                                    lbl.Visible = false;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(houseBankText))
                                {
                                radMessage.Title = "Alert";
                                radMessage.Show("Please Validate & Apply the House Bank Code");
                                return;
                               }
                           }

                            if (strStatus.Trim() == "Approved" && strTransStatusId.Trim() != "11")
                            {
                                dApprovedAmount = dApprovedAmount + Convert.ToDecimal(ApprovedAmount);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(item["DCFLag"].Text)))
                            {
                                //if (Convert.ToString(item["DCFLag"].Text) == "S" && (!string.IsNullOrEmpty(Convert.ToString(ApprovedAmount))))
                                //    DebitAmt = DebitAmt + Convert.ToDecimal(ApprovedAmount);
                                //else if (Convert.ToString(item["DCFLag"].Text) == "H" && (!string.IsNullOrEmpty(Convert.ToString(ApprovedAmount))))
                                //    CreditAmt = CreditAmt + Convert.ToDecimal(ApprovedAmount);

                                if (Convert.ToString(item["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(ApprovedAmount))))
                                    DebitAmt = DebitAmt + Convert.ToDecimal(ApprovedAmount);
                                else if (Convert.ToString(item["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(ApprovedAmount))))
                                    CreditAmt = CreditAmt + Convert.ToDecimal(ApprovedAmount);
                            }


                            if (String.IsNullOrEmpty(Convert.ToString(ApprovedAmount)))
                            {
                                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                                radMessage.Show(Constants.SELECTAMOUNTREMARK);
                                return;
                            }

                            if (Convert.ToDecimal(ApprovedAmount) < 1)
                            {
                                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                                radMessage.Show(Constants.SELECTAMOUNTREMARK);
                                return;
                            }

                        }
                    }

                    if (bRequiredFlag == true)
                        return;
                    if (DebitAmt >= CreditAmt)
                    {
                        radMessage.Title = "Alert";
                        radMessage.Show("Its a debit balance.Hence could not proceed");
                        return;
                    }



                    string strVendorId = Convert.ToString(Session["VendorCode"]);
                    string strCompanyId = Convert.ToString(Session["CompanyCode"]);


                    bool bValid = false;
               // bValid = checkTodayPaidTotalByCB(dApprovedAmount); 2 Crore Validaion sp
                bValid = (CreditAmt - DebitAmt <= 20000000 ? true : false);

                    if (bValid == false)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.DAILYPAIDLIMITMSG_CB);
                        return;
                    }


                bValid = checkTodayPaidTotal(strVendorId, strCompanyId, dApprovedAmount);
                    if (bValid == false)
                    {
                        RadAjaxManager1.ResponseScripts.Add("saveConfirmation('submit')");
                    }
                    else
                    {

                        if (rbtnBill.Checked)
                        {
                            ViewState["Sender"] = "GRD";
                            SaveAgainstData(lnk, "Submit", true);
                        }
                        else
                        {
                            ViewState["Sender"] = "GRD";
                            SaveAdvnaceData(lnk, "Submit", true);
                        }
                    }
                //}               // BindGrid(); 
                //else
                //{
                //    radMessage.Title = "Alert";
                //    radMessage.Show("Please Validate & Apply the House Bank Code");
                //    return;
                //}
                RefreshCurrentAllocation();
                }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private List<PaymentDetailAgainstBillTxn> GetTransactionsAgaintList(LinkButton lnk, string strMode, bool bValid)
        {
            try
            {
                GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl("gvPendingApproval2")).Items;

                List<PaymentDetailAgainstBillTxn> lstTXns = new List<PaymentDetailAgainstBillTxn>();

                foreach (GridDataItem item in collection)
                {
                    if (item.Selected)
                    {
                        string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                        if (String.IsNullOrEmpty(ApprovedAmount.Trim()))
                        {
                            ApprovedAmount = "0";
                        }

                        string strRemark = ((RadTextBox)item.FindControl("tbRemark")).Text;
                        string strStatus = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedItem.Text;
                        string Stage = String.Empty;
                        string ProfileId = String.Empty;
                        string StatusId = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue;

                        string HouseBank = ((RadTextBox)item.FindControl("tbHouseBank")).Text;

                        if (((RadDropDownList)item.FindControl("cmbApproval")).SelectedItem.Text.ToLower().Contains("correction"))
                        {
                            ProfileId = ((RadDropDownList)item.FindControl("cmbStage")).SelectedValue;
                        }
                        else
                        {
                            ProfileId = Convert.ToString(Session["ProfileId"]);
                        }

                        if (strStatus.ToLower().Contains("correction"))
                        {
                            Stage = ((RadDropDownList)item.FindControl("cmbStage")).SelectedValue;
                        }

                        string SAPPaymentId = item.GetDataKeyValue("Id").ToString();

                        PaymentDetailAgainstBillTxn obj = new PaymentDetailAgainstBillTxn();
                        SAPAgainstBillPayment objSAP = null;//new SAPAgainstBillPayment();
                        using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                        {

                            int sapid = Convert.ToInt32(SAPPaymentId);
                             objSAP = suzlonBPPEntities.SAPAgainstBillPayments.AsNoTracking().FirstOrDefault(w => w.SAPAgainstBillPaymentId== sapid);
                        }
                        objSAP.HouseBank =Convert.ToString(HouseBank);
                        if(objSAP!=null)
                        obj.SAPAgainstBillPayment = objSAP;

                        obj.AppovedAmount = Convert.ToDecimal(ApprovedAmount);
                        obj.SAPAgainstBillPaymentId = Convert.ToInt32(SAPPaymentId);
                        obj.Comment = strRemark;
                        obj.CreatedBy = Convert.ToInt32(Session["UserId"]);
                        obj.CreatedOn = DateTime.Now;
                        obj.StatusId = Convert.ToInt32(((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue);
                        obj.ProfileId = Convert.ToInt32(ProfileId);
                        obj.PaymentWorkflowStatusId = 24;

                        var objAgainstPay = objSuzEntity.PaymentDetailAgainstBills
                                .Where(b => b.SAPAgainstBillPaymentId == obj.SAPAgainstBillPaymentId)
                                .FirstOrDefault();

                        if (String.Compare(strMode, "Submit") == 0)
                        {
                            if (obj.StatusId == 1)
                                obj.PaymentWorkflowStatusId = 17;

                            if (obj.StatusId == 2)
                                obj.PaymentWorkflowStatusId = 18;

                            if (obj.StatusId == 3)
                            {
                                var obPayWFStatus = objSuzEntity.PaymentWorkflowStatus
                               .Where(b => b.StatusId == obj.StatusId && b.ProfileId == obj.ProfileId)
                               .FirstOrDefault();

                                if (obPayWFStatus != null)
                                    obj.PaymentWorkflowStatusId = obPayWFStatus.PaymentWorkflowStatusId;
                            }

                            if (bValid == false)
                            {
                                if (objAgainstPay.TransactionStatusId != 11)
                                    obj.PaymentWorkflowStatusId = 13;     //Sent to approver for need correction
                            }
                        }

                        lstTXns.Add(obj);
                    }
                }
                return lstTXns;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return null;
            }
        }

        private List<PaymentDetailAdvanceTxn> GetTransactionsList(LinkButton lnk, string strMode, bool bValid)
        {
            try
            {
                GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl("gvPendingApproval2")).Items;

                List<PaymentDetailAdvanceTxn> lstTXns = new List<PaymentDetailAdvanceTxn>();

                foreach (GridDataItem item in collection)
                {
                    if (item.Selected)
                    {
                        string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                        if (String.IsNullOrEmpty(ApprovedAmount.Trim()))
                        {
                            ApprovedAmount = "0";
                        }

                        string strRemark = ((RadTextBox)item.FindControl("tbRemark")).Text;
                        string strStatus = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedItem.Text;
                        string Stage = String.Empty;
                        string ProfileId = String.Empty;
                        string StatusId = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue;

                        string HouseBank = ((RadTextBox)item.FindControl("tbHouseBank")).Text;

                        if (((RadDropDownList)item.FindControl("cmbApproval")).SelectedItem.Text.ToLower().Contains("correction"))
                        {
                            ProfileId = ((RadDropDownList)item.FindControl("cmbStage")).SelectedValue;
                        }
                        else
                        {
                            ProfileId = Convert.ToString(Session["ProfileId"]);
                        }

                        if (strStatus.ToLower().Contains("correction"))
                        {
                            Stage = ((RadDropDownList)item.FindControl("cmbStage")).SelectedValue;
                        }

                        string SAPPaymentId = item.GetDataKeyValue("Id").ToString();

                        PaymentDetailAdvanceTxn obj = new PaymentDetailAdvanceTxn();

                        SAPAdvancePayment objSAP = null;//new SAPAgainstBillPayment();
                        using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                        {

                            int sapid = Convert.ToInt32(SAPPaymentId);
                            objSAP = suzlonBPPEntities.SAPAdvancePayments.AsNoTracking().FirstOrDefault(w => w.SAPAdvancePaymentId == sapid);
                        }
                        objSAP.HouseBank = Convert.ToString(HouseBank);
                        if (objSAP != null)
                            obj.SAPAdvancePayment = objSAP;


                        obj.AppovedAmount = Convert.ToDecimal(ApprovedAmount);
                        obj.SAPAdvancePaymentId = Convert.ToInt32(SAPPaymentId);
                        obj.Comment = strRemark;
                        obj.CreatedBy = Convert.ToInt32(Session["UserId"]);
                        obj.CreatedOn = DateTime.Now;
                        obj.StatusId = Convert.ToInt32(((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue);
                        obj.ProfileId = Convert.ToInt32(ProfileId);
                        obj.PaymentWorkflowStatusId = 24;

                        var objAdvance = objSuzEntity.PaymentDetailAdvances
                                .Where(b => b.SAPAdvancePaymentId == obj.SAPAdvancePaymentId)
                                .FirstOrDefault();

                        if (String.Compare(strMode, "Submit") == 0)
                        {
                            if (obj.StatusId == 1)
                                obj.PaymentWorkflowStatusId = 17;

                            if (obj.StatusId == 2)
                                obj.PaymentWorkflowStatusId = 18;

                            if (obj.StatusId == 3)
                            {
                                var obPayWFStatus = objSuzEntity.PaymentWorkflowStatus
                               .Where(b => b.StatusId == obj.StatusId && b.ProfileId == obj.ProfileId)
                               .FirstOrDefault();

                                if (obPayWFStatus != null)
                                    obj.PaymentWorkflowStatusId = obPayWFStatus.PaymentWorkflowStatusId;
                            }

                            if (bValid == false)
                            {
                                if (objAdvance.TransactionStatusId != 11)
                                    obj.PaymentWorkflowStatusId = 13;     //Sent to approver for need correction
                            }
                        }

                        lstTXns.Add(obj);
                    }
                }
                return lstTXns;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return null;
            }
        }

        private decimal getVerticalbudget(string iVertical)
        {
            try
            {
                var budget = objSuzEntity.GetVerticalBudgetForFnA(Convert.ToInt32(iVertical)).FirstOrDefault();

                decimal dBudget = 0;

                if (budget != null)
                {
                    dBudget = Convert.ToDecimal(budget);
                }
                return dBudget;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private bool checkTodayPaidTotal(string strVendorId, string strCompanyId, decimal dVendorItmSum)
        {
            try
            {
                System.Data.Entity.Core.Objects.ObjectParameter TodayApprovedAmount = new System.Data.Entity.Core.Objects.ObjectParameter("TodayApprovedAmount", typeof(Decimal));
                decimal dBudget = 0;
                objSuzEntity.GetTodayApprovedAmountFnA(strVendorId, strCompanyId, "", TodayApprovedAmount);

                dBudget = Convert.ToDecimal(TodayApprovedAmount.Value);

                decimal dDailyLimit = 0;

                dDailyLimit = paymentWorkflowController.GetDailyPaymentLimit();

                if ((dBudget + dVendorItmSum) <= dDailyLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void gvPendingApproval2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    RadDropDownList cmbApproval = (RadDropDownList)e.Item.FindControl("cmbApproval");
                    RadNumericTextBox txtAprovedAmount = (RadNumericTextBox)e.Item.FindControl("tbApprovedAmount");
                    if (txtAprovedAmount != null)
                        txtAprovedAmount.Enabled = false;


                    if (ViewState["Status"] != null)
                    {
                        List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];

                        cmbApproval.DataSource = lststatus;
                        cmbApproval.DataTextField = "Name";
                        cmbApproval.DataValueField = "Id";
                        cmbApproval.DataBind();
                    }

                    RadDropDownList cmbStage = (RadDropDownList)e.Item.FindControl("cmbStage");

                    string stage = (string)DataBinder.Eval(e.Item.DataItem, "ProfileIds").ToString();
                    string[] strArr = stage.Split(',');

                    if (ViewState["Profiles"] != null)
                    {
                        List<Models.ListItem> profiles = (List<Models.ListItem>)ViewState["Profiles"];
                        profiles = (from c in profiles
                                    where strArr.Contains(c.Id)
                                    select c).ToList();

                        cmbStage.DataSource = profiles;
                        cmbStage.DataTextField = "Name";
                        cmbStage.DataValueField = "Id";
                        cmbStage.DataBind();
                    }

                    if (cmbApproval.SelectedItem.Text.ToLower().Contains("correction"))
                    {
                        cmbStage.Enabled = true;
                    }
                    else
                    {
                        cmbStage.Enabled = false;
                    }

                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewHistory");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);

                    GridDataItem item = e.Item as GridDataItem;
                    if (Convert.ToString(item["DCFLag"].Text) == "S")
                    {
                        item["DCFLag"].Text = "Debit";
                    }
                    if (Convert.ToString(item["DCFLag"].Text) == "H")
                    {
                        item["DCFLag"].Text = "Credit";
                    }

                }

                if (e.Item is GridHeaderItem && rbtnAdvance.Checked == true)
                {
                    GridHeaderItem header = (GridHeaderItem)e.Item;
                    header["ApprovedAmount"].Text = "Net Amount";
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rbtnBill_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
            Session["ValidateHouseBank"] = "false";
            lblINRAmt.Text = "0";
            lblProposedAmt.Text = "0";
            fillReveresData();
            gvReverse.DataBind();
            fillReversedDoneData();
            gvReversedRequest.DataBind();

        }

        protected void rbtnAdvance_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
            Session["ValidateHouseBank"] = "false";
            lblINRAmt.Text = "0";
            lblProposedAmt.Text = "0";
            fillReveresData();
            gvReverse.DataBind();
            fillReversedDoneData();
            gvReversedRequest.DataBind();
        }

        protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
        {
            BindGrid();
            fillReveresData();
            gvReverse.DataBind();
            fillReversedDoneData();
            gvReversedRequest.DataBind();
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            try
            {
                string values = e.Argument;
                string[] parameters = values.Split('#');

                if (e.Argument.Contains("Comment"))
                {
                    bindComment(Convert.ToInt32(parameters[1]));
                    grdComment.DataBind();
                }

                if (parameters[0].Contains("ProceedToSaveYes"))
                {
                    if (parameters[1] == "save")
                        SaveDetails("Save");
                    else if (parameters[1] == "submit")
                        SaveDetails("Submit");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void bindComment(int paymentId)
        {
            try
            {
                ViewState["PaymentId"] = paymentId;
                List<PaymentComments> lstComments = paymentWorkflowController.GetPaymentWorkFlowComment(paymentId, rbtnBill.Checked ? "AgainstBill" : "Advance");
                grdComment.DataSource = lstComments;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdComment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["PaymentId"])))
                    bindComment(Convert.ToInt32(ViewState["PaymentId"]));
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvApproved_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "gvApproved2")
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewHistory");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvApproved_ItemCommand(object sender, GridCommandEventArgs e)
        {
            RadGrid grdVendor = null;

            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                grdVendor = (e.Item as GridDataItem).ChildItem.FindControl("gvApproved1") as RadGrid;
            }

            try
            {
                if (e.CommandName == RadGrid.ExpandCollapseCommandName)
                {
                    string strCompanyNm = Convert.ToString((e.Item as GridDataItem).GetDataKeyValue("Company"));

                    if (strCompanyNm != "")
                    {
                        string strCompanyCode = string.Empty;

                        string[] strSplit = strCompanyNm.Split('-');

                        if (strSplit.Length > 1)
                        {
                            strCompanyCode = strSplit[1];
                            Session["CompanyCodeApproved"] = strCompanyCode;
                        }

                        List<GetVendorDetailsForFnA_Result> lstGetVendorData = null;

                        if (rbtnAdvance.Checked == true)
                            lstGetVendorData = objSuzEntity.GetVendorDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), strCompanyCode, Convert.ToInt32(Session["UserId"]), "2", "2").ToList();

                        if (rbtnBill.Checked == true)
                            lstGetVendorData = objSuzEntity.GetVendorDetailsForFnA(Convert.ToInt32(cmbVertical.SelectedValue), strCompanyCode, Convert.ToInt32(Session["UserId"]), "1", "2").ToList();

                        if (lstGetVendorData != null)
                        {
                            grdVendor.DataSource = lstGetVendorData;
                            grdVendor.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvApproved2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewHistory");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);
                    RadButton btnRefresh = (RadButton)e.Item.FindControl("btnRefresh");
                    string ProfileName = Convert.ToString(HttpContext.Current.Session["ProfileName"]);

                    //try
                    //{
                    //    if (rbtnAdvance.Checked && (ProfileName == "Admin"))
                    //    {
                    //        GridTableView detailTable = (GridTableView)e.Item.OwnerTableView;
                    //        detailTable.GetColumn("Reverse").Display = true;
                    //    }
                    //    else
                    //    {
                    //        GridTableView detailTable = (GridTableView)e.Item.OwnerTableView;
                    //        detailTable.GetColumn("Reverse").Display = false;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    CommonFunctions.WriteErrorLog(ex);
                    //}
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvApproved1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                RadGrid grdVendorItems = null;

                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    grdVendorItems = (e.Item as GridDataItem).ChildItem.FindControl("gvApproved2") as RadGrid;

                    GridDataItem grdItem = (GridDataItem)e.Item;
                    string strVendorNm = grdItem["Vendor"].Text;

                    if (strVendorNm != "")
                    {
                        string[] strSplit = strVendorNm.Split('-');

                        string strVendorCode = string.Empty;

                        if (strSplit.Length > 1)
                        {
                            strVendorCode = strSplit[1];
                        }

                        List<GetApproverEndItems_Result> lstGetVendorItems = null;

                        if (rbtnAdvance.Checked == true)
                            lstGetVendorItems = objSuzEntity.GetApproverEndItems(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToString(Session["CompanyCodeApproved"]), strVendorCode, Convert.ToInt32(HttpContext.Current.Session["UserId"]), "2", "2").ToList();

                        if (rbtnBill.Checked == true)
                            lstGetVendorItems = objSuzEntity.GetApproverEndItems(Convert.ToInt32(cmbVertical.SelectedValue), Convert.ToString(Session["CompanyCodeApproved"]), strVendorCode, Convert.ToInt32(HttpContext.Current.Session["UserId"]), "1", "2").ToList();

                        if (lstGetVendorItems != null)
                        {
                            grdVendorItems.DataSource = lstGetVendorItems;
                            grdVendorItems.DataBind();
                        }
                    }
                    //22 Aug 16 For Hide Show col in Advance & Against Bill 

                    if (rbtnBill.Checked == true)
                    {
                        grdVendorItems.MasterTableView.GetColumnSafe("DocumentNumber").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("DPRNumber").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("PurchasingDocument").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("ExpectedClearingDate").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("SpecialGL").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("WithholdingTaxCode").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("UnsettledOpenAdvance").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("JustificationforAdvPayment").Visible = false;
                    }
                    if (rbtnAdvance.Checked == true)
                    {
                        grdVendorItems.MasterTableView.GetColumnSafe("DocumentNumber").Visible = false;
                        grdVendorItems.MasterTableView.GetColumnSafe("DPRNumber").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("PurchasingDocument").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("ExpectedClearingDate").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("SpecialGL").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("WithholdingTaxCode").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("UnsettledOpenAdvance").Visible = true;
                        grdVendorItems.MasterTableView.GetColumnSafe("JustificationforAdvPayment").Visible = true;

                    }


                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvPendingApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindDataSource();
        }

        protected void gvApproved_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindDataSource();
        }

        protected void gvPendingApproval2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Attachment")
                {
                    String Id = e.CommandArgument.ToString();
                    //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                    RadAjaxManager1.ResponseScripts.Add("openRadWin('FNAApprover','update','" + Convert.ToString(Id) + "','true','true','true','false','PaymentInitiator');");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvApproved2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                String Id = e.CommandArgument.ToString();
                if (e.CommandName == "Attachment")
                {                   
                    //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                    RadAjaxManager1.ResponseScripts.Add("openRadWin('FNAApprover','update','" + Convert.ToString(Id) + "','false','false','true','false','PaymentInitiator');");
                }                
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }


        private void SaveDetails(string strSaveSubmit)
        {
            LinkButton lnk = (LinkButton)Session["sender"];

            if (rbtnBill.Checked)
            {
                ViewState["Sender"] = "GRD";
                SaveAgainstData(lnk, strSaveSubmit, false);
            }
            else
            {
                ViewState["Sender"] = "GRD";
                SaveAdvnaceData(lnk, strSaveSubmit, false);
            }

            BindGrid();
        }

        protected void cmbApproval_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            RadDropDownList dstatus = (sender as RadDropDownList);
            //  string StatusId = dstatus.SelectedValue;
            string statusText = dstatus.SelectedItem.Text.Trim();
            GridDataItem item = (GridDataItem)dstatus.Parent.NamingContainer;
            RadDropDownList cmbStage = ((RadDropDownList)item.FindControl("cmbStage"));
            RadNumericTextBox tbApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount"));

            if (statusText.ToLower().Contains("correction"))  //Need correction
            {
                cmbStage.Enabled = true;
            }
            else
            {
                cmbStage.Enabled = false;
            }

            if (statusText.ToLower().Contains("correction") || statusText.ToLower().Contains("reject"))  //Need correction
            {
                tbApprovedAmount.Enabled = false;
            }
            else
            {
                tbApprovedAmount.Enabled = true;
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string billType = string.Empty;
                string tabType = string.Empty;

                if (rbtnBill.Checked)
                    billType = "1";
                else if (rbtnAdvance.Checked)
                    billType = "2";

                if (RadTabStrip1.SelectedIndex == 0)
                    tabType = "1";
                else if (RadTabStrip1.SelectedIndex == 1)
                    tabType = "2";

                List<GetItemsForExportToExcel_Result> lstGetCompanyData = CommonFunctions.GetItemsForExportToExcel(Convert.ToInt32(cmbVertical.SelectedValue),
                                                                Convert.ToInt32(Session["UserId"]), billType, tabType);
                DataTable dt = CommonFunctions.ConvertListToDataTable(lstGetCompanyData);

                RemoveColumnFromDatatable(billType, tabType, dt);
                if (billType == "1")
                    dt.Columns["DPR_Number"].ColumnName = "Document Number";

                CommonFunctions.ExporttoExcel(dt, Response, "Report.xls");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private static void RemoveColumnFromDatatable(string billType, string tabType, DataTable dt)
        {
            //dt.Columns.Remove("Vertical");
            dt.Columns.Remove("OpenAdvance");
            if (tabType == "1")
            {
                dt.Columns.Remove("Status");
                if (billType == "1")
                {
                    dt.Columns.Remove("Purchasing_Document");
                    dt.Columns.Remove("Special_GL");
                    dt.Columns.Remove("Withholding_Tax_Code");
                    dt.Columns.Remove("Un_settled_Open_Advance__INR_");
                    dt.Columns.Remove("Justification_for_Adv_Payment");
                    dt.Columns.Remove("Assigned_By");
                    dt.Columns.Remove("Expected_Clearing_Date");
                }
                else
                {
                    //dt.Columns.Remove("DPR_Number");
                    dt.Columns.Remove("Vertical");
                    dt.Columns.Remove("Net_Due_Date");
                }
            }
            else if (tabType == "2")
            {
                dt.Columns.Remove("Assigned_By");
                dt.Columns.Remove("Status");
                if (billType == "1")
                {
                    dt.Columns.Remove("Purchasing_Document");
                    dt.Columns.Remove("Special_GL");
                    dt.Columns.Remove("Withholding_Tax_Code");
                    dt.Columns.Remove("Un_settled_Open_Advance__INR_");
                    dt.Columns.Remove("Justification_for_Adv_Payment");
                    dt.Columns.Remove("Expected_Clearing_Date");
                }
            }
                
        }

        protected DataTable CreateDTSchema(DataTable dt)
        {
            if (dt == null) dt = new DataTable();
            DataRow dr = dt.NewRow();

            dt.Columns.Add(new DataColumn("AllocationNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("SubVertical", typeof(string)));
            dt.Columns.Add(new DataColumn("ValidationDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("AssignedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));

            dr["AllocationNumber"] = "A1";
            dr["SubVertical"] = "SV1";
            dr["ValidationDate"] = DateTime.Now;
            dr["TotalAmount"] = "100";
            dr["AssignedAmount"] = "100.50";
            dr["BalanceAmount"] = "565";

            dt.Rows.Add(dr);
            return dt;
        }

        protected void grdAttachments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                {
                    DataTable dt = new DataTable();
                    grdBudgetAllocation.DataSource = paymentWorkflowController.GetBudgetAllocationDetails((rbtnAdvance.Checked ? "AD" : "AG"), Convert.ToString(ViewState["SelectedSAPIDs"]));
                }
            }
            catch (Exception)
            {
                throw;
            }
            //if (ViewState["SelectedSAPIDs"] != null)

        }

        protected void grdBudgetAllocation_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadNumericTextBox numCurrentAllocation = e.Item.FindControl("numCurrentAllocation") as RadNumericTextBox;
                //RadNumericTextBox numCurrentAllocation = e.Item.FindControl("numCurrentAllocation") as RadNumericTextBox;
                // Decimal CurrentBalanceAmount = (decimal)(((SuzlonBPP.Models.GetBudgetAllocationDetails_Result)e.Item.DataItem).BalanceAmount);
                String IsExpired = (string)(((SuzlonBPP.Models.GetBudgetAllocationDetails_Result)e.Item.DataItem).IsExpired);
                if (IsExpired == "T")
                {
                    numCurrentAllocation.Enabled = false;
                }
                else
                {
                    numCurrentAllocation.Enabled = true;
                }
                numCurrentAllocation.Text = string.Empty;
                //numCurrentAllocation.ClientEvents.OnValueChanged = "javascript:SearchReqsult(" +
                //                     DataBinder.Eval(ev.Row.DataItem, "Id") + ");";
                // numCurrentAllocation.ClientEvents.OnValueChanged = "javascript:alert('111');";
                // numCurrentAllocation.Attributes.Add("onchange", "ValidateAllocatedRow('"+ numCurrentAllocation.ClientID + "','"+ CurrentBalanceAmount + "');");

                GridDataItem dataItem = e.Item as GridDataItem;
                if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "BalanceAmount").ToString()))
                {
                    //double Amount = double.Parse(dataItem["TemplateBalanceAmount"].Text);
                    double Amount = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "BalanceAmount").ToString());
                    TotalBalance += Amount;
                }
            }
            if (e.Item is GridFooterItem)
            {
                GridFooterItem footerItem = e.Item as GridFooterItem;
                footerItem["ToBeUtilised"].Text = "Total Amount:";
                footerItem["TemplateBalanceAmount"].Text = string.Format("{0:##,#0.00}", double.Parse(TotalBalance.ToString()), System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SabVerticalAllocation"] = null;
                Session["AllocationTable"] = null;
                DataTable SabVerticalAllocation = getSabVerticalAllocationSchema();
                // Create allocation Master
                if (rbtnAdvance.Checked)
                {
                    List<PaymentDetailAdvance> PaymentRecords = ((List<PaymentDetailAdvance>)Session["SubmitPayment"]).OrderBy(a => a.SubVerticalId).ToList();

                    foreach (PaymentDetailAdvance pay in PaymentRecords) // Parent Grid
                    {
                        DataRow dr = SabVerticalAllocation.NewRow();
                        dr["SAPPaymentID"] = pay.SAPAdvancePaymentId;
                        dr["CurrentBalance"] = pay.ApprovedAmount;
                        dr["SubVerticalId"] = pay.SubVerticalId;

                        SabVerticalAllocation.Rows.Add(dr);
                    }

                    Session["SabVerticalAllocation"] = SabVerticalAllocation;

                    foreach (GridDataItem item in grdBudgetAllocation.Items) // ChildGrid
                    {

                        // decimal BalanceAmount = Convert.ToDecimal(String.IsNullOrEmpty(Convert.ToString(item.GetDataKeyValue("BalanceAmount"))) ? "0" : Convert.ToString(item.GetDataKeyValue("BalanceAmount")));

                        //get Similer SubVertical Payment Transaction first Priority
                        foreach (PaymentDetailAdvance pay in PaymentRecords) // Parent Grid
                        {
                            //Deduct from Allocation Master
                            DeductFromMaster(item, pay.SAPAdvancePaymentId, "AD");
                            break;
                        }
                    }
                }
                else
                {
                    List<PaymentDetailAgainstBill> PaymentRecords = PaymentRecords = ((List<PaymentDetailAgainstBill>)Session["SubmitPayment"]).OrderBy(a => a.SubVerticalId).ToList();
                    foreach (PaymentDetailAgainstBill pay in PaymentRecords) // Parent Grid
                    {
                        DataRow dr = SabVerticalAllocation.NewRow();
                        dr["SAPPaymentID"] = pay.SAPAgainstBillPaymentId;
                        dr["CurrentBalance"] = pay.ApprovedAmount;
                        dr["SubVerticalId"] = pay.SubVerticalId;
                        SabVerticalAllocation.Rows.Add(dr);
                    }
                    Session["SabVerticalAllocation"] = SabVerticalAllocation;

                    foreach (GridDataItem item in grdBudgetAllocation.Items) // ChildGrid
                    {
                        foreach (PaymentDetailAgainstBill pay in PaymentRecords) // Parent Grid
                        {
                            //Deduct from Allocation Master
                            DeductFromMaster(item, pay.SAPAgainstBillPaymentId, "AG");
                            break;
                        }
                    }
                }



                ViewState["Sender"] = "ACT";
                if (rbtnAdvance.Checked)
                {
                    SaveAdvnaceData((LinkButton)Session["Sender"], "Submit", true);
                }
                else
                {
                    SaveAgainstData((LinkButton)Session["Sender"], "Submit", true);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void DeductFromMaster(GridDataItem item, int SAPPaymentId, string paymentMode)
        {
            Decimal BalanceAmount = 0;
            Decimal TotalAssginedAmount = 0;
            DataTable UpdatedValue = new DataTable();
            Decimal Amount = 0;

            try
            {
                PaymentAllocationDetail objAllocationDetails = new PaymentAllocationDetail();
                RadNumericTextBox CurrentUserAllocation = (RadNumericTextBox)item.FindControl("numCurrentAllocation");
                int subVertical = Convert.ToInt32(item.GetDataKeyValue("SubVerticalId"));
                int TreasuryDetailId = Convert.ToInt32(item.GetDataKeyValue("TreasuryDetailId"));
                DataTable AllocationTable = (DataTable)Session["SabVerticalAllocation"];

                //Amount = Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(CurrentUserAllocation.Text)) ? "0" : CurrentUserAllocation.Text);
                //BalanceAmount = Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(CurrentUserAllocation.Text)) ? "0" : CurrentUserAllocation.Text);

                Amount = Convert.ToDecimal(String.IsNullOrEmpty(Convert.ToString(CurrentUserAllocation.Text)) ? "0" : CurrentUserAllocation.Text);
                BalanceAmount = Convert.ToDecimal(String.IsNullOrEmpty(Convert.ToString(CurrentUserAllocation.Text)) ? "0" : CurrentUserAllocation.Text);


                DataRow[] dr = AllocationTable.Select("SubVerticalId = '" + subVertical + "' AND CurrentBalance > 0");
                if (dr.Count() > 0)
                {
                    for (int i = 0; i < dr.Count(); i++)
                    {
                        Decimal CurrentBalance = Convert.ToDecimal(dr[i]["CurrentBalance"]);
                        if (BalanceAmount < CurrentBalance)
                        {
                            BalanceAmount = CurrentBalance - BalanceAmount;
                        }
                        else
                        {
                            BalanceAmount = BalanceAmount - CurrentBalance;
                        }
                        dr[i]["CurrentBalance"] = BalanceAmount;
                        TotalAssginedAmount = TotalAssginedAmount + (CurrentBalance - BalanceAmount);

                        objAllocationDetails.AllocatedAmount = CurrentBalance - BalanceAmount;
                        objAllocationDetails.SAPPaymentID = Convert.ToInt32(dr[i]["SAPPaymentID"]);
                        objAllocationDetails.SAPPaymentTYPE = paymentMode;
                        objAllocationDetails.TreasuryDetailId = TreasuryDetailId;
                        UpdateAllocation(objAllocationDetails);
                    }
                }
                if (TotalAssginedAmount < Amount)
                {

                    for (int i = 0; i < AllocationTable.Rows.Count; i++)
                    {
                        Decimal CurrentBalance = Convert.ToDecimal(AllocationTable.Rows[i]["CurrentBalance"]);

                        if (BalanceAmount < CurrentBalance)
                        {
                            BalanceAmount = CurrentBalance - BalanceAmount;
                            AllocationTable.Rows[i]["CurrentBalance"] = BalanceAmount;
                            objAllocationDetails.AllocatedAmount = CurrentBalance - BalanceAmount;
                            TotalAssginedAmount = TotalAssginedAmount + (CurrentBalance - BalanceAmount);
                        }
                        else
                        {
                            BalanceAmount = BalanceAmount - CurrentBalance;
                            AllocationTable.Rows[i]["CurrentBalance"] = 0;
                            objAllocationDetails.AllocatedAmount = CurrentBalance;
                            TotalAssginedAmount = TotalAssginedAmount + CurrentBalance;
                        }

                        objAllocationDetails.SAPPaymentID = Convert.ToInt32(AllocationTable.Rows[i]["SAPPaymentID"]);
                        objAllocationDetails.SAPPaymentTYPE = paymentMode;
                        objAllocationDetails.TreasuryDetailId = TreasuryDetailId;
                        if (objAllocationDetails.AllocatedAmount > 0) UpdateAllocation(objAllocationDetails);
                        if (TotalAssginedAmount >= Amount) break;
                    }
                }

                Session["SAPPaymentID"] = AllocationTable;
                Session["DBSubmit"] = UpdatedValue;

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void UpdateAllocation(PaymentAllocationDetail objAllocationDetail)
        {
            List<PaymentAllocationDetail> objPaymentAllocations = new List<PaymentAllocationDetail>();
            PaymentAllocationDetail objPayments = new PaymentAllocationDetail();

            if (Session["AllocationTable"] != null)
                objPaymentAllocations = (List<PaymentAllocationDetail>)Session["AllocationTable"];

            objPayments.SAPPaymentID = objAllocationDetail.SAPPaymentID;
            objPayments.SAPPaymentTYPE = objAllocationDetail.SAPPaymentTYPE;
            objPayments.TreasuryDetailId = objAllocationDetail.TreasuryDetailId; // needs to TrasuryDetailID
            objPayments.AllocatedAmount = objAllocationDetail.AllocatedAmount;
            objPayments.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objPayments.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objPayments.CreatedOn = DateTime.Now;
            objPayments.ModifiedOn = DateTime.Now;
            objPaymentAllocations.Add(objPayments);
            Session["AllocationTable"] = objPaymentAllocations;
        }

        protected DataTable getSabVerticalAllocationSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SAPPaymentID", typeof(string)));
            dt.Columns.Add(new DataColumn("CurrentBalance", typeof(decimal)));
            dt.Columns.Add(new DataColumn("SubVerticalId", typeof(string)));

            return dt;
        }

        protected void lnkVerticalBudget_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openVerticalBudgerPopup();", true);
                grdVerticalBudget.DataSource = paymentWorkflowController.getVerticalBalanceDetails(Convert.ToInt32(cmbVertical.SelectedValue));
                grdVerticalBudget.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void grdVerticalBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(cmbVertical.SelectedValue)))
            {
                grdVerticalBudget.DataSource = paymentWorkflowController.getVerticalBalanceDetails(Convert.ToInt32(cmbVertical.SelectedValue));
            }
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            //if (ValidateHouseField())
            //{                
            //    PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            //    string result = paymentWorkflowModel.SAP_HouseBankKeyData(Convert.ToString(Session["CompanyCode"]), txtHouseCode.Text, true);                
            //    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
            //    if (result == Constants.SUCCESS)
            //        radMessage.Show(Constants.HOUSE_KEY_VALIDATED);
            //    else
            //        radMessage.Show(result);
            //}

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();

            Button btn = sender as Button;
            GridNestedViewItem CurrentItem = btn.NamingContainer as GridNestedViewItem;
            string HouseBank = Convert.ToString((CurrentItem.FindControl("txtHouseCode") as RadTextBox).Text);
            string result = string.Empty;
            if (!string.IsNullOrEmpty(HouseBank))
            {
                result = paymentWorkflowModel.SAP_HouseBankKeyData(Convert.ToString(Session["CompanyCode"]), HouseBank, true);
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                if (result == Constants.SUCCESS)
                {
                    Session["ValidateHouseBank"] = "true";
                    Session["HouseBank"] = HouseBank;
                    radMessage.Show(Constants.HOUSE_KEY_VALIDATED);
                }
                else
                {
                    Session["ValidateHouseBank"] = "false";
                    Session["HouseBank"] = string.Empty;
                    radMessage.Show(result);
                }
            }
            else
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ENTER_HOUSE_BANK);
                return;
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            //if (ValidateHouseField())
            //{
            //    PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            //    string result = paymentWorkflowModel.SAP_HouseBankKeyData(Convert.ToString(Session["CompanyCode"]), txtHouseCode.Text, true);
            //    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
            //    if (result == Constants.SUCCESS)
            //        radMessage.Show(Constants.HOUSE_KEY_APPLIED);
            //    else
            //        radMessage.Show(result);
            //}
            Button btn = sender as Button;
            GridNestedViewItem CurrentItem = btn.NamingContainer as GridNestedViewItem;
            string HouseBank = Convert.ToString((CurrentItem.FindControl("txtHouseCode") as RadTextBox).Text);
            if (!string.IsNullOrEmpty(HouseBank))
            {
                PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
                string result = paymentWorkflowModel.SAP_HouseBankKeyData(Convert.ToString(Session["CompanyCode"]), HouseBank, true);
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;

                if (result == Constants.SUCCESS)
                {
                    GridDataItemCollection collection = ((RadGrid)btn.NamingContainer.FindControl("gvPendingApproval2")).Items;
                    foreach (GridDataItem item in collection)
                    {
                        if (item.Selected)
                        {
                            RadTextBox txtHouseBank = (RadTextBox)item.FindControl("tbHouseBank");
                            txtHouseBank.Text = HouseBank;
                            Session["HouseBank"] = HouseBank;
                        }
                    }
                 radMessage.Show(Constants.HOUSE_KEY_APPLIED);
                }
                else
                    radMessage.Show(result);
            }
            else
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ENTER_HOUSE_BANK);
                return;
            }
        }



        protected void gvReverse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string ProfileName = Convert.ToString(HttpContext.Current.Session["ProfileName"]);
            if (ProfileName.ToUpper() == "ADMIN")
            {
                fillReveresData();
            }
           
        }

        protected void gvReverse_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Reverse")
            {
                String DocumentClearingNo = e.CommandArgument.ToString();

                RadComboBox cmbReverseReason = (RadComboBox)e.Item.FindControl("cmbReverseReason");
                RadDatePicker dtpRevDate = (RadDatePicker)e.Item.FindControl("dtpRevDate");
                DateTime revDate = Convert.ToDateTime(dtpRevDate.SelectedDate);

                if (dtpRevDate.SelectedDate == null)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show("Please Select Reverse Date");
                }
                if (cmbReverseReason.SelectedValue == "")
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show("Please select reverse reason");
                }

               
                if (cmbReverseReason.SelectedValue != "" && dtpRevDate.SelectedDate!=null )
                {
                    GridDataItem grdItem = (GridDataItem)e.Item;
                    string result = string.Empty;
                    string FiscalYear = Convert.ToString(grdItem["FiscalYear"].Text);
                    string CompCode = Convert.ToString(grdItem["CompanyCode"].Text);
                    string ReverseReason =Convert.ToString(cmbReverseReason.SelectedValue)=="1"?"01":"02";
                    DateTime PostingDate = Convert.ToDateTime(grdItem["PostingDate"].Text);

                    if (rbtnBill.Checked)
                    {
                        result= paymentWorkflowController.PushDataToSAPReversePaymentAgainst(DocumentClearingNo, FiscalYear, CompCode, ReverseReason, PostingDate);
                    }
                    if (rbtnAdvance.Checked)
                    {
                        result=paymentWorkflowController.PushDataToSAPReversePaymentAdvance(DocumentClearingNo, FiscalYear, CompCode, ReverseReason, PostingDate);
                    }

                    if (result == Constants.SAP_SUCCESS)
                    {
                        try
                        {
                            if (DocumentClearingNo != null)
                            {
                                paymentWorkflowController.UpdateReverseCaseDetail(DocumentClearingNo, Convert.ToInt32(cmbReverseReason.SelectedValue.ToString()), rbtnBill.Checked ? "Against" : "Advance", revDate);
                                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                                radMessage.Show("Data reversal Sucessfully Done");
                            }
                            gvReverse.Rebind();
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                        }
                    }
                    else
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(result);
                    }
                }

            }
        }

        protected void gvReverse_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadComboBox cmbReverseReason = (RadComboBox)e.Item.FindControl("cmbReverseReason");
                RadDatePicker dtpRevDate = (RadDatePicker)e.Item.FindControl("dtpRevDate");
                if (cmbReverseReason != null)
                {
                    cmbReverseReason.DataSource = (List<Models.ListItem>)Session["ReverseReason"];
                    cmbReverseReason.DataTextField = "Name";
                    cmbReverseReason.DataValueField = "Id";
                    cmbReverseReason.DataBind();
                }
                if (dtpRevDate != null)
                {
                    dtpRevDate.SelectedDate = DateTime.Now;
                }

            }
        }

        protected void gvReversedRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string ProfileName = Convert.ToString(HttpContext.Current.Session["ProfileName"]);
            if (ProfileName.ToUpper() == "ADMIN")
            {
                fillReversedDoneData();
            }   
        }

        //private bool ValidateHouseField()
        //{
        //    bool isValid = true;
        //    if (string.IsNullOrEmpty(txtHouseCode.Text))
        //    {
        //        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
        //        radMessage.Show(Constants.ENTER_HOUSE_BANK);
        //        isValid = false;
        //    }
        //    return isValid;
        //}




    }
}
