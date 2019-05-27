using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuzlonBPP.MasterPages
{
    public partial class SuzlonBPP : System.Web.UI.MasterPage
    {
        public string applicationPath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                applicationPath = ConfigurationManager.AppSettings["WebsiteUrl"].ToString();
                if (!IsPostBack)
                {
                    if (Session["Token"] == null || Session["UserId"] == null)                    
                        Response.Redirect("~/Login.aspx", false);
                    
                        SetMenuSecurity();
                    lblUserName.Text = "Welcome " + Session["LoginUserName"].ToString();
                    string pageName = this.Request.Url.Segments.Last();
                    if (!pageName.ToLower().Contains("workflowconfig.aspx"))
                        Session[Constants.DROPDOWN_VALUES_FOR_USERCONTROL] = null;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void lnlLogout_Click(object sender, EventArgs e)
        {
            try
            {
                Logout();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void Logout()
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx", false);
        }

        private void SetMenuSecurity()
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);

            // Menu Bank Details Updation Start
            if (PageSecurity.IsAccessGranted(PageSecurity.BANKDETAILLIST, menuList) || PageSecurity.IsAccessGranted(PageSecurity.VENDORBANKDETAILS, menuList))
                menuBankDetailUpdation.Visible = true;
            else
                menuBankDetailUpdation.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.BANKDETAILLIST, menuList))
                menuBankDetailList.Visible = true;
            else
                menuBankDetailList.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.VENDORBANKDETAILS, menuList))
                menuVendorBankDetails.Visible = true;
            else
                menuVendorBankDetails.Visible = false;
            // Menu Bank Details Updation End

            // Menu Budget Allocation Start

            if (PageSecurity.IsAccessGranted(PageSecurity.VERTICALCONTROLLER, menuList) || PageSecurity.IsAccessGranted(PageSecurity.TREASURY, menuList))
                menuBudgetAllocation.Visible = true;
            else
                menuBudgetAllocation.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.VERTICALCONTROLLER, menuList))
                menuVerticalController.Visible = true;
            else
                menuVerticalController.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.TREASURY, menuList))
                menuTreasury.Visible = true;
            else
                menuTreasury.Visible = false;

            // Menu Budget Allocation End


            // Menu Vendor Payments Start
            if (PageSecurity.IsAccessGranted(PageSecurity.AGAINSTBILLINITIATOR_INITIATOR, menuList) || PageSecurity.IsAccessGranted(PageSecurity.ADVANCE_INITIATOR, menuList)
                || PageSecurity.IsAccessGranted(PageSecurity.AGAINSTBILLINITIATOR_AGGREGATOR, menuList) || PageSecurity.IsAccessGranted(PageSecurity.ADVANCE_AGGREGATOR, menuList)
                ||PageSecurity.IsAccessGranted(PageSecurity.F_AND_A_APPROVER, menuList))
                menuVendorPayment.Visible = true;
            else
                menuVendorPayment.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.AGAINSTBILLINITIATOR_INITIATOR, menuList))
                menuAgainstBillInitiator_Initiator.Visible = true;
            else
                menuAgainstBillInitiator_Initiator.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.ADVANCE_INITIATOR, menuList) || PageSecurity.IsAccessGranted(PageSecurity.ADVANCE_AGGREGATOR, menuList))
                menuAgainstBillInitiator_Aggregator.Visible = true;
            else
                menuAgainstBillInitiator_Aggregator.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.F_AND_A_APPROVER, menuList))
                menuFnAApprover.Visible = true;
            else
                menuFnAApprover.Visible = false;
            // Menu Vendor Payments End

            // Menu Reports Start
            if (PageSecurity.IsAccessGranted(PageSecurity.REPORTS, menuList))
            {
                menuReports.Visible = true;
                if (Session["ProfileId"] != null)
                {
                    int profileId = Convert.ToInt32(Session["ProfileId"]);
                    if (profileId == 7 || profileId == 1)
                    {
                        liPaymentAgainstReport.Visible = liTreasuryReport.Visible = liPaymentTreasuryReport.Visible = liBankDetailsReport.Visible = true;
                    }
                    else if (profileId == 4)
                    {
                        liPaymentAgainstReport.Visible = liTreasuryReport.Visible = liPaymentTreasuryReport.Visible = true;
                    }
                }
            }
            else
                menuReports.Visible = false;
            // Menu Reports End

            // Menu Admin Configuration Start

            if (PageSecurity.IsAccessGranted(PageSecurity.USER_MANAGEMENT, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.PROFILE_MASTER, menuList)||
                PageSecurity.IsAccessGranted(PageSecurity.COMPANY_MASTER, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.VERTICALCONTROLLER, menuList)
                || PageSecurity.IsAccessGranted(PageSecurity.SUBVERTICAL_MASTER, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.NATURE_OF_REQUEST, menuList)||
                PageSecurity.IsAccessGranted(PageSecurity.VENDOR_MASTER, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.VENDOR_BANK_MASTER, menuList)||
                PageSecurity.IsAccessGranted(PageSecurity.IFSC_CODE_MASTER, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.WORKFLOW_CONFIGURATION, menuList)||
                PageSecurity.IsAccessGranted(PageSecurity.APPLICATION_CONFIGURATION, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.VENDOR_BANK_MASTER, menuList)
                )
                menuAdminConfiguration.Visible = true;
            else
                menuAdminConfiguration.Visible = false;

            // Menu User Management Start
            if (PageSecurity.IsAccessGranted(PageSecurity.USER_MANAGEMENT, menuList))
                menuUserManagement.Visible = true;
            else
                menuUserManagement.Visible = false;
            // Menu User Management End


            // Menu Master Management Start
            if (PageSecurity.IsAccessGranted(PageSecurity.PROFILE_MASTER, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.COMPANY_MASTER, menuList)||
                PageSecurity.IsAccessGranted(PageSecurity.VERTICALCONTROLLER, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.SUBVERTICAL_MASTER, menuList)||
                PageSecurity.IsAccessGranted(PageSecurity.NATURE_OF_REQUEST, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.VENDOR_MASTER, menuList)||
                PageSecurity.IsAccessGranted(PageSecurity.VENDOR_BANK_MASTER, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.IFSC_CODE_MASTER, menuList)
                || PageSecurity.IsAccessGranted(PageSecurity.VENDOR_BANK_MASTER, menuList))
                menuMasterManagement.Visible = true;
            else
                menuMasterManagement.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.PROFILE_MASTER, menuList))
                menuProfileMaster.Visible = true;
            else
                menuProfileMaster.Visible = false;

            //if (PageSecurity.IsAccessGranted(PageSecurity.COMPANY_MASTER, menuList))
            //    menuComapnyMaster.Visible = true;
            //else
            //    menuComapnyMaster.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.VERTICALCONTROLLER, menuList))
                menuVerticalMaster.Visible = true;
            else
                menuVerticalMaster.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.SUBVERTICAL_MASTER, menuList))
                menuSub_VerticalMaster.Visible = true;
            else
                menuSub_VerticalMaster.Visible = false;
            if (PageSecurity.IsAccessGranted(PageSecurity.NATURE_OF_REQUEST, menuList))
                menuNatureofRequest.Visible = true;
            else
                menuNatureofRequest.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.VENDOR_MASTER, menuList))
                menuVendorMaster.Visible = true;
            else
                menuVendorMaster.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.VENDOR_BANK_MASTER, menuList))
                menuVendorBankMaster.Visible = true;
            else
                menuVendorBankMaster.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.IFSC_CODE_MASTER, menuList))
                menuIFSCCodeMaster.Visible = true;
            else
                menuIFSCCodeMaster.Visible = false;

            // Menu Master Management End

            // Menu Configuration Start
            if (PageSecurity.IsAccessGranted(PageSecurity.WORKFLOW_CONFIGURATION, menuList)|| PageSecurity.IsAccessGranted(PageSecurity.APPLICATION_CONFIGURATION, menuList))
                menuConfiguraton.Visible = true;
            else
                menuConfiguraton.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.WORKFLOW_CONFIGURATION, menuList))
                menuWorkFlowConfiguration.Visible = true;
            else
                menuWorkFlowConfiguration.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.APPLICATION_CONFIGURATION, menuList))
                menuApplicationConfiguration.Visible = true;
            else
                menuApplicationConfiguration.Visible = false;

            // Menu Configuration End



            // Menu Admin Configuration End
        }
    }
}