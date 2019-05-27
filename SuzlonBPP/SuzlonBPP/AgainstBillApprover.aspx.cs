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
    public partial class AgainstBillApprover : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        ApproverModel objApproverModel = new ApproverModel();
        PaymentWorkflowController paymentWorkflowController = new PaymentWorkflowController();
        double TotalBalance=0.00;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
             //   Login l = new Login();
             //   l.AuthenticateUser();

                // dummy data binding : Start
                //DataTable dt = new DataTable();
                //dt.Columns.Add(new DataColumn("Vendor", typeof(string)));
                //dt.Columns.Add(new DataColumn("TotalAmount", typeof(string)));
                //dt.Columns.Add(new DataColumn("ProposedAmount", typeof(string)));
                //dt.Columns.Add(new DataColumn("ApprovedAmount", typeof(string)));
                //dt.Columns.Add(new DataColumn("Todays_Paid_Total", typeof(string)));
                //dt.Columns.Add(new DataColumn("i", typeof(int)));

                //for (int i = 1; i < 3; i++)
                //{

                //    DataRow dr = dt.NewRow();
                //    dr["i"] = i;
                //    dr["Vendor"] = "Vendor1";
                //    dr["TotalAmount"] = "10,000";
                //    dr["ProposedAmount"] = "10,00000";
                //    dr["ApprovedAmount"] = "10,00000";
                //    dr["Todays_Paid_Total"] = "10,000000";
                //    dt.Rows.Add(dr);
                //}

                //gvPendingApproval.DataSource = dt;
                //gvPendingApproval.DataBind();
                //gvApproved.DataSource = dt;
                //gvApproved.DataBind();
                //gvCorrection.DataSource = dt;
                //gvCorrection.DataBind();

                // dummy data binding : End

                GetProfiles();
                GetStatus();
                BindVertical();
                GetPaymentWorkflowStatus();
                GetDailyPaymentLimit();
                BindMainGrid();
                ViewState["PaymentAllicationAmount"] = 0;
            }
        }

        private void GetPaymentWorkflowStatus()
        {
            List<PaymentWorkflowStatu> lstPaymentWorkflowStatu = paymentWorkflowController.GetPaymentWorkFlowStatus();
            Session["PaymentWorkflowStatus"] = lstPaymentWorkflowStatu;
        }

        private void GetDailyPaymentLimit()
        {
            decimal dDailyPaymtLimit = paymentWorkflowController.GetDailyPaymentLimit();
            Session["DailyPaymentLimit"] = dDailyPaymtLimit;
        }

        private void GetProfiles()
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


        private void GetStatus()
        {
            //string drpname = "workflow-status";
            //string commonDrpValues = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
            //if (commonDrpValues == Constants.REST_CALL_FAILURE)
            //{
            //    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
            //    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            //}
            //else
            //{
            //    DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(commonDrpValues);
            //    ViewState["Status"] = ddValues.WorkFlowStatus;
            //}

            List<SuzlonBPP.Models.ListItem> lstStatus = paymentWorkflowController.GetStatusList();
            ViewState["Status"] = lstStatus;
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

                                cmbVertical.DataTextField = "Name";
                                cmbVertical.DataValueField = "Id";
                                cmbVertical.DataSource = ddValues.Vertical;
                                cmbVertical.DataBind();
                                if(cmbVertical.Items.Count>1)
                                cmbVertical.Items.Insert(0, new RadComboBoxItem("Select Vertical", String.Empty));
                                cmbVertical.SelectedIndex = 0;
                                lnkVerticalBudget.Text = Convert.ToString(paymentWorkflowController.getVerticalBalance(Convert.ToInt32(cmbVertical.SelectedValue)));
                            }
                            
                        }
                    }
                }  

                //string commonDrpValues = commonFunctions.RestServiceCall(String.Format(Constants.GETVERTICALBYUSER, HttpContext.Current.Session["UserId"]), String.Empty);
                //if (commonDrpValues == Constants.REST_CALL_FAILURE)
                //{
                //    //  radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                //    //  radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                //}
                //else
                //{
                //    List<Models.VerticalMaster> lstVerticalMaster = JsonConvert.DeserializeObject<List<Models.VerticalMaster>>(commonDrpValues);
                //    cmbVertical.DataTextField = "Name";
                //    cmbVertical.DataValueField = "VerticalId";
                //    cmbVertical.DataSource = lstVerticalMaster;
                //    cmbVertical.DataBind();
                //    cmbVertical.Items.Insert(0, new RadComboBoxItem("Select Vertical", String.Empty));
                //    cmbVertical.SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void cmbVertical_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindMainGrid();
            // lnkVerticalBudget.Text = Convert.ToString(paymentWorkflowController.getVerticalBalance(Convert.ToInt32(cmbVertical.SelectedValue)));

            RefreshCurrentAllocation();
            lblINRAmt.Text = "0";
            lblProposedAmt.Text = "0";
        }

        protected void rbtnAgainst_CheckedChanged(object sender, EventArgs e)
        {
            BindMainGrid();
            lblINRAmt.Text = "0";
            lblProposedAmt.Text = "0";
        }
       
        private void BindMainGrid()
        {
            if (String.IsNullOrEmpty(cmbVertical.SelectedValue))
                return;

            string strBillType = String.Empty;
            string strTabName = "Pending";   //String.Empty;
            RadGrid grid;

            if (rdTab.SelectedIndex == 0)
            {
                strTabName = Convert.ToInt32(TabType.PendingForApproval).ToString();
                grid = gvPendingApproval;
            }
            else if (rdTab.SelectedIndex == 1)
            {
                strTabName = Convert.ToInt32(TabType.Approved).ToString();
                grid = gvApproved;
            }
            else
            {
                strTabName = Convert.ToInt32(TabType.NeedCorrection).ToString();
                grid = gvCorrection;
            }

            if (rbtnAdvance.Checked)
            {
                strBillType = Convert.ToInt32(BillType.Advance).ToString(); // "Advance";
            }
            else
            {
                strBillType = Convert.ToInt32(BillType.Against).ToString(); // "Against";
            }

            List<GetApproverVendorData_Result> lstGetVendorData_Result = paymentWorkflowController.GetVendorReqByVerticalID(cmbVertical.SelectedValue, Convert.ToString(Session["UserId"]),
                                                                    strBillType, strTabName);
            grid.DataSource = lstGetVendorData_Result;
            grid.DataBind();
            RefreshCurrentAllocation();
        }

        private void BindDataSource()
        {
            if (String.IsNullOrEmpty(cmbVertical.SelectedValue))
                return;

            string strBillType = String.Empty;
            string strTabName = "Pending";   //String.Empty;
            RadGrid grid;

            if (rdTab.SelectedIndex == 0)
            {
                strTabName = Convert.ToInt32(TabType.PendingForApproval).ToString();
                grid = gvPendingApproval;
            }
            else if (rdTab.SelectedIndex == 1)
            {
                strTabName = Convert.ToInt32(TabType.Approved).ToString();
                grid = gvApproved;
            }
            else
            {
                strTabName = Convert.ToInt32(TabType.NeedCorrection).ToString();
                grid = gvCorrection;
            }

            if (rbtnAdvance.Checked)
            {
                strBillType = Convert.ToInt32(BillType.Advance).ToString(); // "Advance";
            }
            else
            {
                strBillType = Convert.ToInt32(BillType.Against).ToString(); // "Against";
            }

            List<GetApproverVendorData_Result> lstGetVendorData_Result = paymentWorkflowController.GetVendorReqByVerticalID(cmbVertical.SelectedValue, Convert.ToString(Session["UserId"]),
                                                                    strBillType, strTabName);
            grid.DataSource = lstGetVendorData_Result;
        }

        private void BindCompanyGridByVendor(RadGrid CompanyGrid, string VendorCode)
        {
            string GetCompanyData = String.Empty;
            string strBillType = String.Empty;
           
            string strTabName = "Pending";

            if (rdTab.SelectedIndex == 0)
            {
                strTabName = Convert.ToInt32(TabType.PendingForApproval).ToString();
            }
            else if (rdTab.SelectedIndex == 1)
            {
                strTabName = Convert.ToInt32(TabType.Approved).ToString();
            }
            else
            {
                strTabName = Convert.ToInt32(TabType.NeedCorrection).ToString();
            }

            if (rbtnAdvance.Checked)
            {
                //List<GetApproverCompByVendor_Advance_Result> lstRecords = paymentWorkflowController.GetCompaniesReqByVendor_Adv(cmbVertical.SelectedValue, VendorCode, Convert.ToString(Session["UserId"]));
                //CompanyGrid.DataSource = lstRecords;
                //CompanyGrid.DataBind();

                strBillType = Convert.ToInt32(BillType.Advance).ToString(); // "Advance";
            }
            else
            {
                //List<GetApproverCompByVendor_Against_Result> lstRecords = paymentWorkflowController.GetCompaniesReqByVendor_Aga(cmbVertical.SelectedValue, VendorCode, Convert.ToString(Session["UserId"]));
                //CompanyGrid.DataSource = lstRecords;
                //CompanyGrid.DataBind();
                strBillType = Convert.ToInt32(BillType.Against).ToString(); // "Against";

            }

            List<GetApproverCompByVendor_Result> lstRecords = paymentWorkflowController.GetCompaniesReqByVendor(cmbVertical.SelectedValue, VendorCode,
                Convert.ToString(Session["UserId"]), strBillType, strTabName);
            CompanyGrid.DataSource = lstRecords;
            CompanyGrid.DataBind();
        }

        private void BindItemsGrid(RadGrid ItemsGrid, string VendorCode, string CompanyCode)
        {
            string GetItems = String.Empty;
            string strBillType = String.Empty;
            string strTabName = "Pending";

            if (rdTab.SelectedIndex == 0)
            {
                strTabName = Convert.ToInt32(TabType.PendingForApproval).ToString();
            }
            else if (rdTab.SelectedIndex == 1)
            {
                strTabName = Convert.ToInt32(TabType.Approved).ToString();
            }
            else
            {
                strTabName = Convert.ToInt32(TabType.NeedCorrection).ToString();
            }

            if (rbtnAdvance.Checked)
            {
                //List<GetApproverEndItemsAdvance_Result> lstItems = paymentWorkflowController.GetApproverEndItemsAdvance(cmbVertical.SelectedValue, VendorCode, CompanyCode, Convert.ToString(Session["UserId"]));
                //ItemsGrid.DataSource = lstItems;
                //ItemsGrid.DataBind();
                strBillType = Convert.ToInt32(BillType.Advance).ToString(); // "Advance";
            }
            else
            {

                //List<GetApproverEndItemsAgainst_Result> lstItems = paymentWorkflowController.GetApproverEndItemsAgainst(cmbVertical.SelectedValue, VendorCode, CompanyCode, Convert.ToString(Session["UserId"]));
                //ItemsGrid.DataSource = lstItems;
                //ItemsGrid.DataBind();
                strBillType = Convert.ToInt32(BillType.Against).ToString(); //"Against";
            }

            List<GetApproverEndItems_Result> lstItems = paymentWorkflowController.GetApproverEndItems(cmbVertical.SelectedValue, VendorCode, CompanyCode,
                Convert.ToString(Session["UserId"]), strBillType, strTabName);

            ItemsGrid.DataSource = lstItems;
            ItemsGrid.DataBind();

            //22 Aug 16 For Hide Show col in Advance & Against Bill 

            if (rbtnAgainst.Checked == true)
            {
                ItemsGrid.MasterTableView.GetColumnSafe("DocumentNumber").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("DPRNumber").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("PurchasingDocument").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("ExpectedClearingDate").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("SpecialGL").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("WithholdingTaxCode").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("UnsettledOpenAdvance").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("JustificationforAdvPayment").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("TaxRate").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("TaxAmount").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("GrossAmount").Visible = false;

            }
            if (rbtnAdvance.Checked == true)
            {
                ItemsGrid.MasterTableView.GetColumnSafe("DocumentNumber").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("DPRNumber").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("PurchasingDocument").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("ExpectedClearingDate").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("SpecialGL").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("WithholdingTaxCode").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("UnsettledOpenAdvance").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("JustificationforAdvPayment").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("TaxRate").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("TaxAmount").Visible = true;
                ItemsGrid.MasterTableView.GetColumnSafe("GrossAmount").Visible = true;

                //ItemsGrid.MasterTableView.GetColumnSafe("Amount").Visible = false;
                ItemsGrid.MasterTableView.GetColumnSafe("AmountProposed").Visible = false;

            }
        }

        protected void gvPendingApproval_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                //  e.Item
                GridNestedViewItem item = ((Telerik.Web.UI.GridDataItem)(e.Item)).ChildItem as GridNestedViewItem;
               // GridNestedViewItem item = e.Item as GridNestedViewItem;
                RadGrid grid = (RadGrid)item.FindControl("gvPendingApproval1");
                string strVendor = (e.Item as GridDataItem).GetDataKeyValue("Vendor").ToString();
                string[] strArr = strVendor.Split('-');
                string VendorCode = String.Empty;

                if (strArr.Length ==2)
                {
                    VendorCode = Convert.ToString(strArr[1]);
                }

                BindCompanyGridByVendor(grid, VendorCode);

            }
        }

        protected void gvPendingApproval1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                RadGrid r = (sender as RadGrid);
                GridNestedViewItem item = ((Telerik.Web.UI.GridDataItem)(e.Item)).ChildItem as GridNestedViewItem;
                // GridNestedViewItem item = e.Item as GridNestedViewItem;
                RadGrid grid = (RadGrid)item.FindControl("gvPendingApproval2");

                string strCompany = (e.Item as GridDataItem).GetDataKeyValue("Company").ToString();
                string[] strArr = strCompany.Split('-');
                string CompCode = String.Empty;

                if (strArr.Length == 2)
                {
                    CompCode = Convert.ToString(strArr[1]);
                }

                // ((GridNestedViewItem)((GridDataItem)e.Item).OwnerTableView.NamingContainer.NamingContainer).ParentItem.GetDataKeyValue("Vendor")

                GridDataItem gridItem = ((GridDataItem)e.Item);
                GridTableView nestedview = (GridTableView)gridItem.OwnerTableView;
                GridNestedViewItem nestedviewItem = (GridNestedViewItem)nestedview.NamingContainer.NamingContainer;
                string strVendor = Convert.ToString(nestedviewItem.ParentItem.GetDataKeyValue("Vendor"));

                strArr = strVendor.Split('-');
                string VendorCode = String.Empty;

                if (strArr.Length == 2)
                {
                    VendorCode = Convert.ToString(strArr[1]);
                }

                BindItemsGrid(grid, VendorCode, CompCode);

            }
        }

        protected void gvPendingApproval2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadDropDownList cmbStage = (RadDropDownList)e.Item.FindControl("cmbStage");
                string stage = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ProfileIds"));
                string StatusId = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "StatusId"));
                string ProfileId = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ProfileID"));

                string[] strArr = stage.Split(',');

                if (ViewState["Profiles"] != null)
                {
                    List<Models.ListItem> profiles = (List<Models.ListItem>)ViewState["Profiles"];
                    profiles = (from c in profiles
                                where strArr.Contains(c.Id) && c.Id != Convert.ToString(Session["ProfileId"])
                                select c).ToList();

                    cmbStage.DataSource = profiles;
                    cmbStage.DataTextField = "Name";
                    cmbStage.DataValueField = "Id";
                    cmbStage.DataBind();

                    if (!String.IsNullOrEmpty(ProfileId))
                       cmbStage.SelectedValue = ProfileId;
                }


                RadDropDownList cmbApproval = (RadDropDownList)e.Item.FindControl("cmbApproval");
                

                if (ViewState["Status"] != null)
                {
                    List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];

                    cmbApproval.DataSource = lststatus;
                    cmbApproval.DataTextField = "Name";
                    cmbApproval.DataValueField = "Id";
                    cmbApproval.DataBind();
                    cmbApproval.SelectedValue = "1";

                    if (!String.IsNullOrEmpty(StatusId))
                        cmbApproval.SelectedValue = StatusId;
                }

                if (cmbApproval.SelectedItem.Text.ToLower().Contains("correction"))
                {
                    cmbStage.Enabled = true;
                }
                else
                {
                    cmbStage.Enabled = false;
                }

                HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
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
                string ProfileName = Convert.ToString(HttpContext.Current.Session["ProfileName"]);
                RadNumericTextBox txtAprovedAmount = (RadNumericTextBox)e.Item.FindControl("tbApprovedAmount");
                if (rbtnAdvance.Checked)
                    txtAprovedAmount.Enabled = false;
                else if (ProfileName == "Auditor")                                                        
                        txtAprovedAmount.Enabled = false;

                RadButton btnRefresh = (RadButton)e.Item.FindControl("btnRefresh");
                if (rbtnAdvance.Checked && (ProfileName == "F&A SSC-DT" || ProfileName == "F&A SSC DT"))
                {
                    GridTableView detailTable = (GridTableView)e.Item.OwnerTableView;
                    detailTable.GetColumn("Refresh").Display = true;
                }
                else
                {
                    GridTableView detailTable = (GridTableView)e.Item.OwnerTableView;
                    detailTable.GetColumn("Refresh").Display = false;

                }


            }
            if (e.Item is GridHeaderItem && rbtnAdvance.Checked == true)
            {
                GridHeaderItem header = (GridHeaderItem)e.Item;
                header["ApprovedAmount"].Text = "Net Amount";
            }
        }

        protected void tbApprovedAmount_TextChanged(object sender, EventArgs e)
        {
            string strAppAmount = "0";
            RadNumericTextBox rdNum = (RadNumericTextBox)sender;
         
          //  string strTotalApproved = ((GridDataItem)((GridNestedViewItem)rdNum.Parent.NamingContainer.NamingContainer.NamingContainer.NamingContainer).ParentItem)["ApprovedTotal"].Text;
            decimal dTotalApproved = 0; // Convert.ToDecimal(strTotalApproved);
          
            // ((RadNumericTextBox)((Telerik.Web.UI.GridTableView)(rdNum.Parent.NamingContainer.NamingContainer)).Items[1].FindControl("tbApprovedAmount")).Text

            GridDataItemCollection collection = ((Telerik.Web.UI.GridTableView)rdNum.Parent.NamingContainer.NamingContainer).Items;
            foreach(GridDataItem item in collection)
            {
                if (!String.IsNullOrEmpty(((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text) && 
                    ((RadDropDownList)item.FindControl("cmbApproval")).SelectedText.Trim().ToLower() == "approved")
                {
                    strAppAmount=((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                    dTotalApproved += Convert.ToDecimal(strAppAmount);
                }
            }

            ((GridDataItem)((GridNestedViewItem)rdNum.Parent.NamingContainer.NamingContainer.NamingContainer.NamingContainer).ParentItem)["ApprovedTotal"].Text = Convert.ToString(dTotalApproved);
         //   ((RadLabel)((GridDataItem)((GridNestedViewItem)rdNum.Parent.NamingContainer.NamingContainer.NamingContainer.NamingContainer).ParentItem).FindControl("lbAmountApproved")).Text = dApprovedAmount.ToString();

        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
             string strGridName = String.Empty;
             LinkButton lnk = sender as LinkButton;
             decimal? dAmount=0;
             GetNextPaymentWorkFlowId_Result objResult = new GetNextPaymentWorkFlowId_Result();
             int iOutPut = -1;
             bool IsOpenAdvance = false;
             bool DailyLimitCrossed = false;

            if (rdTab.SelectedIndex == 0)
            {
                strGridName = "gvPendingApproval2";
            }
            else if (rdTab.SelectedIndex == 2)
            {
                strGridName = "gvCorrection2";
            }

            GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl(strGridName)).Items;
            if (!CheckSelected(collection))
                return;

            if (rbtnAgainst.Checked)
            {
                List<PaymentDetailAgainstBillTxn> lstTXns = GetTransactionsAgainstList(lnk, strGridName); // It looks no loop is required in the function
                List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];
                int ApprovedStatusId = Convert.ToInt32(lststatus.Where(c => c.Name.ToLower() == "approved").FirstOrDefault().Id);
                dAmount = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Sum(c => c.AppovedAmount);
                PaymentDetailAgainstBillTxn objApproved = lstTXns.Where(c => c.StatusId == ApprovedStatusId).FirstOrDefault<PaymentDetailAgainstBillTxn>();

                string strProfileName = Convert.ToString(Session["ProfileName"]);
                strProfileName = strProfileName.Replace(" ", String.Empty);
                iOutPut = GetNextPaymentWorkFlowId(lnk, Convert.ToDecimal(dAmount), false, out objResult);
                // Commented by Gaurang
                //if (strProfileName.Trim().ToLower().Contains("controller") || strProfileName.Trim().ToLower().Contains("exceptional"))
                //{
                //    if (Convert.ToBoolean(objResult.IsLimitCrossed) && objApproved != null)
                //    {
                //        Session["ClickedButton"] = lnk;
                //        RadAjaxManager1.ResponseScripts.Add("saveConfirmation('save')");
                //    }
                //    else
                //    {
                //        SaveRecords(lnk);
                //    }
                //}
                //else
                {
                    SaveRecords(lnk);
                }
            }
            else
            {
                List<PaymentDetailAdvanceTxn> lstTXns = GetTransactionsList(lnk, strGridName, out IsOpenAdvance);
                List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];
                int ApprovedStatusId = Convert.ToInt32(lststatus.Where(c => c.Name.ToLower() == "approved").FirstOrDefault().Id);
                dAmount = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Sum(c => c.AppovedAmount);
                PaymentDetailAdvanceTxn objApproved = lstTXns.Where(c => c.StatusId == ApprovedStatusId).FirstOrDefault<PaymentDetailAdvanceTxn>();

                string strProfileName = Convert.ToString(Session["ProfileName"]);
                iOutPut = GetNextPaymentWorkFlowId(lnk, Convert.ToDecimal(dAmount), IsOpenAdvance, out objResult);
                // Commented By Gaurang
                //if (strProfileName.Trim().ToLower().Contains("controller") || strProfileName.Trim().ToLower().Contains("exceptional"))
                //{
                //    if (Convert.ToBoolean(objResult.IsLimitCrossed) && objApproved != null)
                //    {
                //        Session["ClickedButton"] = lnk;
                //        RadAjaxManager1.ResponseScripts.Add("saveConfirmation('save')");
                //    }
                //    else
                //    {
                //        SaveRecords(lnk);
                //    }
                //}
                //else
                {
                    SaveRecords(lnk);
                }
            }
        }

        private void SaveRecords(LinkButton lnk)
        {
           // LinkButton lnk = (LinkButton)(Session["ClickedButton"]);
            
            string strGridName = String.Empty;
            string ProfileId = String.Empty;

            if (rdTab.SelectedIndex == 0)
            {
                strGridName = "gvPendingApproval2";
            }
            else if (rdTab.SelectedIndex == 2)
            {
                strGridName = "gvCorrection2";
            }

            GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl(strGridName)).Items;


            if (rbtnAgainst.Checked)
            {

                List<PaymentDetailAgainstBill> lstPaymentDetailAdgainst = new List<PaymentDetailAgainstBill>();
                foreach (GridDataItem item in collection)
                {
                    if (item.Selected)
                    {
                        string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                        //if (String.IsNullOrEmpty(ApprovedAmount.Trim()))
                        //    continue;

                        PaymentDetailAgainstBill objPaymentDetailAdvance = new PaymentDetailAgainstBill();
                        objPaymentDetailAdvance.SAPAgainstBillPaymentId = Convert.ToInt32(item.GetDataKeyValue("Id"));
                        objPaymentDetailAdvance.ApprovedAmount = Convert.ToDecimal(ApprovedAmount);
                        objPaymentDetailAdvance.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                        objPaymentDetailAdvance.ModifiedOn = DateTime.Now;
                        string Remark = ((RadTextBox)item.FindControl("tbRemark")).Text;
                        objPaymentDetailAdvance.Comment = Remark;
                        objPaymentDetailAdvance.SaveMode = true;
                        string StatusId = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue;

                        if (((RadDropDownList)item.FindControl("cmbApproval")).SelectedText.ToLower().Contains("correction"))
                        {
                            ProfileId = ((RadDropDownList)item.FindControl("cmbStage")).SelectedValue;
                        }
                        else
                        {
                            ProfileId = Convert.ToString(Session["ProfileId"]);
                        }

                        objPaymentDetailAdvance.StatusId = Convert.ToInt32(StatusId);
                        objPaymentDetailAdvance.ProfileID = Convert.ToInt32(ProfileId);
                        lstPaymentDetailAdgainst.Add(objPaymentDetailAdvance);
                    }
                }

                paymentWorkflowController.UpdateAmountInPaymentDetailAgainst(lstPaymentDetailAdgainst);
            }
            else
            {
                List<PaymentDetailAdvance> lstPaymentDetailAdvance = new List<PaymentDetailAdvance>();
                foreach (GridDataItem item in collection)
                {
                    if (item.Selected)
                    {
                        string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                        //if (String.IsNullOrEmpty(ApprovedAmount.Trim()))
                        //    continue;

                        PaymentDetailAdvance objPaymentDetailAdvance = new PaymentDetailAdvance();
                        objPaymentDetailAdvance.SAPAdvancePaymentId = Convert.ToInt32(item.GetDataKeyValue("Id"));
                        objPaymentDetailAdvance.ApprovedAmount = Convert.ToDecimal(ApprovedAmount);
                        objPaymentDetailAdvance.ModifiedBy  = Convert.ToInt32(Session["UserId"]);
                        objPaymentDetailAdvance.ModifiedOn = DateTime.Now;
                        objPaymentDetailAdvance.SaveMode = true;
                        string Remark = ((RadTextBox)item.FindControl("tbRemark")).Text;
                        objPaymentDetailAdvance.Comment = Remark;

                        string StatusId = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue;

                        if (((RadDropDownList)item.FindControl("cmbApproval")).SelectedText.ToLower().Contains("correction"))
                        {
                            ProfileId = ((RadDropDownList)item.FindControl("cmbStage")).SelectedValue;
                        }
                        else
                        {
                            ProfileId = Convert.ToString(Session["ProfileId"]);
                        }

                        objPaymentDetailAdvance.StatusId = Convert.ToInt32(StatusId);
                        objPaymentDetailAdvance.ProfileID = Convert.ToInt32(ProfileId);

                        lstPaymentDetailAdvance.Add(objPaymentDetailAdvance);
                    }
                }

                paymentWorkflowController.UpdateAmountInPaymentDetailAdvance(lstPaymentDetailAdvance);
            }

            BindMainGrid();
        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            LinkButton lnk;
            var XMLPayment = string.Empty;
            if (String.Compare(Convert.ToString(ViewState["Sender"]), "ACT") == 0)
            {
                sender = (LinkButton)Session["btSender"];
            }
            else
            {
                Session["btSender"] = sender;
            }
            lnk = (LinkButton)sender;
            
            GetNextPaymentWorkFlowId_Result objResult = new GetNextPaymentWorkFlowId_Result();
            
            string strGridName = String.Empty;
            int iOutPut= 0;
            bool IsOpenAdvance =false;
            bool DailyLimitCrossed = false;
            string strProfileName = Convert.ToString(Session["ProfileName"]);
            if (rdTab.SelectedIndex == 0)
            {
                strGridName = "gvPendingApproval2";
            }
            else if (rdTab.SelectedIndex == 2)
            {
                strGridName = "gvCorrection2";
            }
           
            GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl(strGridName)).Items;
            if (!CheckSelected(collection))
                return;

            try
            {
                decimal? dAmount=0;
                
                if (rbtnAgainst.Checked)
                {
                    List<PaymentDetailAgainstBillTxn> lstTXns = GetTransactionsAgainstList(lnk, strGridName); // It looks no loop is required in the function
                    List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];
                    int ApprovedStatusId = Convert.ToInt32(lststatus.Where(c => c.Name.ToLower() == "approved").FirstOrDefault().Id);
                 //    ViewState["ApprovedCount"] = Convert.ToString(lststatus.Where(c => c.Name.ToLower() == "approved").Count());
                    dAmount = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Sum(c => c.AppovedAmount);

                    PaymentDetailAgainstBillTxn objApproved = lstTXns.Where(c => c.StatusId == ApprovedStatusId).FirstOrDefault<PaymentDetailAgainstBillTxn>();

                   
                    if (!Convert.ToString(Session["ProfileName"]).Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                    {
                        iOutPut = GetNextPaymentWorkFlowId(lnk, Convert.ToDecimal(dAmount), IsOpenAdvance, out objResult);
                        if (Convert.ToBoolean(objResult.IsLimitCrossed) && objApproved != null && 
                            (strProfileName.Trim().ToLower().Contains("controller") || strProfileName.Trim().ToLower().Contains("exceptional")))
                        {
                            Session["ClickedButton"] = lnk;
                            DailyLimitCrossed = true;
                            RadAjaxManager1.ResponseScripts.Add("saveConfirmation('submit')");
                        }
                    }

                    if (!DailyLimitCrossed)
                    {
                        //List<PaymentDetailAgainstBill> lstPaymentDetailAgainst = new List<PaymentDetailAgainstBill>();
                        //foreach (PaymentDetailAgainstBillTxn obj in lstTXns)
                        //{
                        //    PaymentDetailAgainstBill objPaymentDetailAgainst = new PaymentDetailAgainstBill();
                        //    objPaymentDetailAgainst.SAPAgainstBillPaymentId = obj.SAPAgainstBillPaymentId;

                        //    if (obj.StatusId == ApprovedStatusId)
                        //        objPaymentDetailAgainst.PaymentWorkflowStatusId = (iOutPut == 0) ? obj.PaymentWorkflowStatusId : iOutPut;    // need to changed as per logged in user   
                        //    else
                        //        objPaymentDetailAgainst.PaymentWorkflowStatusId = obj.PaymentWorkflowStatusId;

                        //    objPaymentDetailAgainst.ApprovedAmount = obj.AppovedAmount;
                        //    objPaymentDetailAgainst.StatusId = obj.StatusId;

                        //    objPaymentDetailAgainst.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                        //    objPaymentDetailAgainst.ModifiedOn = DateTime.Now;
                        //    objPaymentDetailAgainst.Comment = obj.Comment;
                        //    objPaymentDetailAgainst.ProfileID = obj.ProfileId;
                        //    objPaymentDetailAgainst.SubVerticalId = obj.SubVerticalId;
                        //    lstPaymentDetailAgainst.Add(objPaymentDetailAgainst);
                        //}

                        //Session["SubmitPayment"] = lstPaymentDetailAgainst;
                        //XMLPayment = Convert.ToString(new XElement("Root", from i in lstPaymentDetailAgainst
                        //                                                   select new XElement("Payment",
                        //                                                   new XElement("SAPPaymentId", i.SAPAgainstBillPayment),
                        //                                                   new XElement("SAPAmount", i.ApprovedAmount))));
                        //List<PaymentAllocationDetail> objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
                        //paymentWorkflowController.SavePaymentAgainstDetailTxn_Adv(lstTXns); // SavePaymentTransactions(lstTXns);
                        //paymentWorkflowController.UpdatePaymentDetail_Agnt(lstPaymentDetailAgainst, objAllocation); //UpdatePaymentDetail_Adv(lstPaymentDetailAdvance);
                        //BindMainGrid();
                        SubmitRecords(lnk,Constants.SUBMIT_MODE);
                    }
                }
                else
                {
                    List<PaymentDetailAdvanceTxn> lstTXns = GetTransactionsList(lnk, strGridName, out IsOpenAdvance);
                    List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];
                    int ApprovedStatusId = Convert.ToInt32(lststatus.Where(c => c.Name.ToLower() == "approved").FirstOrDefault().Id);
                //    ViewState["ApprovedCount"] = Convert.ToString(lststatus.Where(c => c.Name.ToLower() == "approved").Count());
                    dAmount = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Sum(c => c.AppovedAmount);
                    PaymentDetailAdvanceTxn objApproved = lstTXns.Where(c => c.StatusId == ApprovedStatusId).FirstOrDefault<PaymentDetailAdvanceTxn>();

                    
                    if (!strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                    {
                        iOutPut = GetNextPaymentWorkFlowId(lnk, Convert.ToDecimal(dAmount), IsOpenAdvance, out objResult);

                        if (Convert.ToBoolean(objResult.IsLimitCrossed) && objApproved != null && 
                            (strProfileName.Trim().ToLower().Contains("controller") || strProfileName.Trim().ToLower().Contains("exceptional")
                            ||strProfileName.Trim().ToUpper().Contains("F&A SSC-DT") || strProfileName.Trim().ToUpper().Contains("F&A SSC DT")))
                        {
                            Session["ClickedButton"] = lnk;
                            DailyLimitCrossed = true;
                            RadAjaxManager1.ResponseScripts.Add("saveConfirmation('submit')");
                        }
                    }

                    if (!DailyLimitCrossed)
                    {

                        //List<PaymentDetailAdvance> lstPaymentDetailAdvance = new List<PaymentDetailAdvance>();
                        //foreach (PaymentDetailAdvanceTxn obj in lstTXns)
                        //{
                        //    PaymentDetailAdvance objPaymentDetailAdvance = new PaymentDetailAdvance();
                        //    objPaymentDetailAdvance.SAPAdvancePaymentId = obj.SAPAdvancePaymentId;

                        //    if (obj.StatusId == ApprovedStatusId)
                        //        objPaymentDetailAdvance.PaymentWorkFlowStatusId = (iOutPut == 0) ? obj.PaymentWorkflowStatusId : iOutPut;    // need to changed as per logged in user   
                        //    else
                        //        objPaymentDetailAdvance.PaymentWorkFlowStatusId = obj.PaymentWorkflowStatusId;

                        //    objPaymentDetailAdvance.ApprovedAmount = obj.AppovedAmount;
                        //    objPaymentDetailAdvance.StatusId = obj.StatusId;

                        //    objPaymentDetailAdvance.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                        //    objPaymentDetailAdvance.ModifiedOn = DateTime.Now;

                        //    objPaymentDetailAdvance.Comment = obj.Comment;
                        //    objPaymentDetailAdvance.ProfileID = obj.ProfileId;
                        //    objPaymentDetailAdvance.SubVerticalId = obj.SubVerticalId;
                        //    lstPaymentDetailAdvance.Add(objPaymentDetailAdvance);
                        //}

                        //Session["SubmitPayment"] = lstPaymentDetailAdvance;
                        //XMLPayment = Convert.ToString(new XElement("Root", from i in lstPaymentDetailAdvance
                        //                                                   select new XElement("Payment",
                        //                                                   new XElement("SAPPaymentId", i.SAPAdvancePaymentId),
                        //                                                   new XElement("SAPAmount", i.ApprovedAmount))));
                        //if (Session["LinkButton"] != null)
                        //{
                        //    List<PaymentAllocationDetail> objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
                        //    paymentWorkflowController.SavePaymentDetailTxn_Adv(lstTXns); // SavePaymentTransactions(lstTXns);
                        //    paymentWorkflowController.UpdatePaymentDetail_Adv(lstPaymentDetailAdvance, objAllocation); //UpdatePaymentDetail_Adv(lstPaymentDetailAdvance);
                        //                                                                                               //BindMainGrid();
                        //}

                        //BindMainGrid();

                        SubmitRecords(lnk,Constants.SUBMIT_MODE);

                    }
                }

                Boolean IsCallToAllocation = false;

                //if (strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                //{
                //    IsCallToAllocation = true;
                //}
                //else
                //{
                //    String isValidRecords = paymentWorkflowController.ValidPaymentAllocation((rbtnAgainst.Checked ? "AG" : "AD"), Convert.ToString(XMLPayment));
                //    if (isValidRecords == "T")
                //    {
                //        IsCallToAllocation = true;
                //    }
                //    else
                //    {
                //        IsCallToAllocation = false;
                //    }
                //}


                //if (IsCallToAllocation == true)
                //{
                //    ViewState["TotalUserAmount"] = Convert.ToString(dAmount);
                //    Session["AllocationTable"] = null;
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openAllocationPopup('" + Convert.ToString(dAmount) + "')", true);
                //}
                //DataTable dt = new DataTable();
                //grdBudgetAllocation.DataSource = null;
                //grdBudgetAllocation.Rebind();
                //Session["LinkButton"] = lnk;
               
            }
            catch(Exception ex)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(ex.Message);
            }
        }

        protected Boolean IsCallAllocation()
        {
            try
            {
                string strProfileName = Convert.ToString(Session["ProfileName"]);
                if (strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                {
                    return true;
                }
                else
                {
                        
                }
            }
            catch (Exception)
            {

                
                throw;
            }
            return true;
        }

        private void SubmitRecords(LinkButton lnk, string SaveMode)
        {
            //  LinkButton lnk = (LinkButton)(Session["ClickedButton"]);
            var XMLPayment = string.Empty;
            Boolean IsCallToAllocation = false;
            string strProfileName = Convert.ToString(Session["ProfileName"]);
            GetNextPaymentWorkFlowId_Result objResult = new GetNextPaymentWorkFlowId_Result();

            string strGridName = String.Empty;
            int iOutPut = 0;
            bool IsOpenAdvance = false;

            if (rdTab.SelectedIndex == 0)
            {
                strGridName = "gvPendingApproval2";
            }
            else if (rdTab.SelectedIndex == 2)
            {
                strGridName = "gvCorrection2";
            }

            try
            {
                decimal? dAmount = 0;

                if (rbtnAgainst.Checked)
                {
                    List<PaymentDetailAgainstBillTxn> lstTXns = GetTransactionsAgainstList(lnk, strGridName); // It looks no loop is required in the function
                    List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];
                    int ApprovedStatusId = Convert.ToInt32(lststatus.Where(c => c.Name.ToLower() == "approved").FirstOrDefault().Id);
                    dAmount = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Sum(c => c.AppovedAmount);
                    ViewState["ApprovedCount"] = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Count();



                    if (!strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                    {
                        iOutPut = GetNextPaymentWorkFlowId(lnk, Convert.ToDecimal(dAmount), IsOpenAdvance, out objResult);
                    }

                    List<PaymentDetailAgainstBill> lstPaymentDetailAgainst = new List<PaymentDetailAgainstBill>();
                    foreach (PaymentDetailAgainstBillTxn obj in lstTXns)
                    {
                        PaymentDetailAgainstBill objPaymentDetailAgainst = new PaymentDetailAgainstBill();
                        objPaymentDetailAgainst.SAPAgainstBillPaymentId = obj.SAPAgainstBillPaymentId;

                        if (obj.StatusId == ApprovedStatusId)
                            objPaymentDetailAgainst.PaymentWorkflowStatusId = (iOutPut == 0) ? obj.PaymentWorkflowStatusId : iOutPut;    // need to changed as per logged in user   
                        else
                            objPaymentDetailAgainst.PaymentWorkflowStatusId = obj.PaymentWorkflowStatusId;

                        objPaymentDetailAgainst.ApprovedAmount = obj.AppovedAmount;
                        objPaymentDetailAgainst.StatusId = obj.StatusId;

                        objPaymentDetailAgainst.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                        objPaymentDetailAgainst.ModifiedOn = DateTime.Now;
                        objPaymentDetailAgainst.Comment = obj.Comment;
                        objPaymentDetailAgainst.ProfileID = obj.ProfileId;
                        objPaymentDetailAgainst.SaveMode = false;
                        lstPaymentDetailAgainst.Add(objPaymentDetailAgainst);


                        Session["SubmitPayment"] = lstPaymentDetailAgainst;
                        XMLPayment = Convert.ToString(new XElement("Root", from i in lstPaymentDetailAgainst
                                                                           select new XElement("Payment",
                                                                           new XElement("SAPPaymentId", i.SAPAgainstBillPaymentId),
                                                                           new XElement("SAPAmount", i.ApprovedAmount))));
                        ViewState["SelectedSAPIDs"] = Convert.ToString(XMLPayment);
                    }

                    //////////////////////////////////////////

                    if (strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                    {
                        IsCallToAllocation = true;
                    }
                    else
                    {
                        String isValidRecords = paymentWorkflowController.ValidPaymentAllocation((rbtnAgainst.Checked ? "AG" : "AD"), Convert.ToString(XMLPayment));
                        if (isValidRecords == "T")
                        {
                            IsCallToAllocation = false;
                            ViewState["Sender"] = "ACT";
                        }
                        else
                        {
                            IsCallToAllocation = true;
                        }
                    }

                    /////////////////////////////////////////


                    if (Convert.ToString(ViewState["Sender"]) == "ACT")
                    {
                        List<PaymentAllocationDetail> objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);                        
                        string result= paymentWorkflowController.UpdatePaymentDetail_Agnt(lstPaymentDetailAgainst, objAllocation, Constants.SAVE_MODE); //UpdatePaymentDetail_Adv(lstPaymentDetailAdvance);
                        if (result == Constants.DETAIL_SAVE_SUCCESS)
                        {
                            paymentWorkflowController.SavePaymentAgainstDetailTxn_Adv(lstTXns); // SavePaymentTransactions(lstTXns);
                            BindMainGrid();
                            ViewState["ApprovedCount"] = "0";
                            RefreshCurrentAllocation();
                            Session["HouseBank"] = string.Empty;
                            ViewState["Sender"] = "";
                        }

                        radMessage.Show(result);
                        return;
                    }

                    if (IsCallToAllocation == true && Convert.ToInt32(ViewState["ApprovedCount"]) > 0)
                    {
                        ViewState["TotalUserAmount"] = Convert.ToString(dAmount);
                        Session["AllocationTable"] = null;
                        decimal PaidCreditAmount = Convert.ToDecimal(ViewState["PaymentAllicationAmount"]);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openAllocationPopup('" + Convert.ToString(dAmount) + "')", true);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openAllocationPopup('" + Convert.ToString(PaidCreditAmount) + "')", true);
                        grdBudgetAllocation.DataSource = paymentWorkflowController.GetBudgetAllocationDetails((rbtnAgainst.Checked ? "AG" : "AD"), Convert.ToString(ViewState["SelectedSAPIDs"])); ;
                        grdBudgetAllocation.DataBind();
                    }
                    else
                    {
                        List<PaymentAllocationDetail> objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
                        
                        string result= paymentWorkflowController.UpdatePaymentDetail_Agnt(lstPaymentDetailAgainst, objAllocation, Constants.SAVE_MODE); //UpdatePaymentDetail_Adv(lstPaymentDetailAdvance);
                        if (result == Constants.DETAIL_SAVE_SUCCESS)
                        {
                            paymentWorkflowController.SavePaymentAgainstDetailTxn_Adv(lstTXns); // SavePaymentTransactions(lstTXns);
                            BindMainGrid();
                            ViewState["ApprovedCount"] = "0";

                            RefreshCurrentAllocation();
                            Session["HouseBank"] = string.Empty;
                        }
                        radMessage.Show(result);
                    }

                }
                else
                {
                    List<PaymentDetailAdvanceTxn> lstTXns = GetTransactionsList(lnk, strGridName, out IsOpenAdvance);
                    List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];
                    int ApprovedStatusId = Convert.ToInt32(lststatus.Where(c => c.Name.ToLower() == "approved").FirstOrDefault().Id);
                    dAmount = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Sum(c => c.AppovedAmount);
                    ViewState["ApprovedCount"] = lstTXns.Where(c => c.StatusId == ApprovedStatusId).Count();
                    if (!strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                    {
                        iOutPut = GetNextPaymentWorkFlowId(lnk, Convert.ToDecimal(dAmount), IsOpenAdvance, out objResult);
                    }

                    List<PaymentDetailAdvance> lstPaymentDetailAdvance = new List<PaymentDetailAdvance>();
                    foreach (PaymentDetailAdvanceTxn obj in lstTXns)
                    {
                        PaymentDetailAdvance objPaymentDetailAdvance = new PaymentDetailAdvance();
                        objPaymentDetailAdvance.SAPAdvancePaymentId = obj.SAPAdvancePaymentId;

                        if (obj.StatusId == ApprovedStatusId)
                            objPaymentDetailAdvance.PaymentWorkFlowStatusId = (iOutPut == 0) ? obj.PaymentWorkflowStatusId : iOutPut;    // need to changed as per logged in user   
                        else
                            objPaymentDetailAdvance.PaymentWorkFlowStatusId = obj.PaymentWorkflowStatusId;

                        objPaymentDetailAdvance.ApprovedAmount = obj.AppovedAmount;
                        objPaymentDetailAdvance.StatusId = obj.StatusId;

                        objPaymentDetailAdvance.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                        objPaymentDetailAdvance.ModifiedOn = DateTime.Now;
                        objPaymentDetailAdvance.Comment = obj.Comment;
                        objPaymentDetailAdvance.ProfileID = obj.ProfileId;
                        objPaymentDetailAdvance.SaveMode = false;
                        lstPaymentDetailAdvance.Add(objPaymentDetailAdvance);

                        Session["SubmitPayment"] = lstPaymentDetailAdvance;
                        XMLPayment = Convert.ToString(new XElement("Root", from i in lstPaymentDetailAdvance
                                                                           select new XElement("Payment",
                                                                           new XElement("SAPPaymentId", i.SAPAdvancePaymentId),
                                                                           new XElement("SAPAmount", i.ApprovedAmount))));
                        ViewState["SelectedSAPIDs"] = Convert.ToString(XMLPayment);

                        ////////////////////////////////////
                        if (strProfileName.Trim().ToLower().Contains(UserProfileEnum.Aggregator.ToString().ToLower()))
                        {
                            IsCallToAllocation = true;
                        }
                        else
                        {
                            String isValidRecords = paymentWorkflowController.ValidPaymentAllocation((rbtnAgainst.Checked ? "AG" : "AD"), Convert.ToString(XMLPayment));
                            if (isValidRecords == "T")
                            {
                                IsCallToAllocation = false;
                                ViewState["Sender"] = "ACT";
                            }
                            else
                            {
                                IsCallToAllocation = true;
                            }
                        }
                        ///////////////////////////////////


                    }
                    if (Convert.ToString(ViewState["Sender"]) == "ACT")
                    {
                        List<PaymentAllocationDetail> objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);
                        
                        string result = paymentWorkflowController.UpdatePaymentDetail_Adv(lstPaymentDetailAdvance, objAllocation,Constants.SAVE_MODE); //UpdatePaymentDetail_Adv(lstPaymentDetailAdvance);
                        if (result == Constants.DETAIL_SAVE_SUCCESS)
                        {
                            paymentWorkflowController.SavePaymentDetailTxn_Adv(lstTXns); // SavePaymentTransactions(lstTXns);
                            BindMainGrid();
                            ViewState["ApprovedCount"] = "0";
                            RefreshCurrentAllocation();
                            ViewState["Sender"] = "";
                        }
                        radMessage.Show(result);
                        return;
                    }


                    if (IsCallToAllocation == true && Convert.ToInt32(ViewState["ApprovedCount"]) > 0)
                    {
                        ViewState["TotalUserAmount"] = Convert.ToString(dAmount);
                        Session["AllocationTable"] = null;
                        decimal PaidCreditAmount = Convert.ToDecimal(ViewState["PaymentAllicationAmount"]);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openAllocationPopup('" + Convert.ToString(PaidCreditAmount) + "')", true);
                        grdBudgetAllocation.DataSource = paymentWorkflowController.GetBudgetAllocationDetails((rbtnAgainst.Checked ? "AG" : "AD"), Convert.ToString(ViewState["SelectedSAPIDs"])); ;
                        grdBudgetAllocation.DataBind();
                    }
                    else
                    {
                        List<PaymentAllocationDetail> objAllocation = (List<PaymentAllocationDetail>)(Session["AllocationTable"]);                        
                        string result = paymentWorkflowController.UpdatePaymentDetail_Adv(lstPaymentDetailAdvance, objAllocation, Constants.SAVE_MODE); //UpdatePaymentDetail_Adv(lstPaymentDetailAdvance);
                        if (result == Constants.DETAIL_SAVE_SUCCESS)
                        {
                            paymentWorkflowController.SavePaymentDetailTxn_Adv(lstTXns); // SavePaymentTransactions(lstTXns);

                            BindMainGrid();
                            ViewState["ApprovedCount"] = "0";
                        }
                        radMessage.Show(result);
                    }
                }

                

                ViewState["Sender"] = "";
            }
            catch(Exception ex)
            {

            }
        }

        protected void RefreshCurrentAllocation()
        {
            string VerticalBudget = Convert.ToString(paymentWorkflowController.getVerticalBalance(Convert.ToInt32(cmbVertical.SelectedValue)));
            string formattedNumber = string.Format("{0:##,#0.00}", decimal.Parse(VerticalBudget), System.Globalization.CultureInfo.InvariantCulture);
            lnkVerticalBudget.Text = formattedNumber;
        }

        private int GetNextPaymentWorkFlowId(LinkButton lnk, decimal Amount, bool IsOpenAdvance,  out GetNextPaymentWorkFlowId_Result objResult)
        {
            string strProfileName = Convert.ToString(Session["ProfileName"]);
            GridDataItem dataItem = ((GridDataItem)((GridNestedViewItem)lnk.NamingContainer).ParentItem);
            string strCompCode = Convert.ToString(dataItem.GetDataKeyValue("Company"));
            string strVendorde = Convert.ToString(((GridNestedViewItem)dataItem.Parent.NamingContainer.Parent.NamingContainer).ParentItem.GetDataKeyValue("Vendor"));
            objResult = new GetNextPaymentWorkFlowId_Result();

            decimal dDailyAmount = 0, dTotalDailyAmount =0;
            decimal dDailyPaymtLimit = 0;


                //  dDailyAmount = paymentWorkflowController.GetTodaysTransAmountByAggregator(strCompCode, strVendorde, rbtnAdvance.Checked ? BillType.Advance : BillType.Against);

                List<GetNextPaymentWorkFlowId_Result> lstResult = paymentWorkflowController.GetNextPaymentWorkFlowId(strCompCode, strVendorde, rbtnAdvance.Checked ? BillType.Advance : BillType.Against,
                     Convert.ToInt32(Session["ProfileId"]), IsOpenAdvance, Amount);

                if (lstResult.Count > 0)
                {
                    objResult = lstResult[0];


                   // return Convert.ToInt32(objResult.NextPaymentWorkFlowId);
                    //if (Convert.ToBoolean(obj.IsLimitCrossed))
                    //{
                    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Confirm", "ConfirmMessage( " + Convert.ToString(obj.TodaysApprovedPayment) + "," + Convert.ToString(dDailyPaymtLimit) + ")", true);
                    //    if (txtconformmessageValue.Value == "Yes")
                    //    {
                    //        return Convert.ToInt32(obj.NextPaymentWorkFlowId);
                    //    }
                    //    else
                    //    {
                    //        return -1; // Cancel Saving
                    //    }
                    //}
                    //else
                    return Convert.ToInt32(objResult.NextPaymentWorkFlowId);
                }
                else
                {
                    return -1;  // Cancel Saving
                }

                //dTotalDailyAmount = dDailyAmount + Amount;
                //dDailyPaymtLimit = 0; // Convert.ToDecimal(Session["DailyPaymentLimit"]);

                //if (dDailyPaymtLimit < dTotalDailyAmount)
                //{
                //    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                //    radMessage.Show(String.Format(Constants.DAILYPAYMTLIMITCROSSED, dDailyPaymtLimit.ToString()));

                //  //  RadWindowManager1.RadConfirm("Are you sure?", "confirmCallBackFn", 300, 100, null, "My Confirm", "myConfirmImage.png");

                //    if (true)
                //        return paymentWorkflowController.GetPaymentWorkflowStatusId("Pending Approval by Auditor");
                //    else
                //        return 0;
                //}

        }

        private List<PaymentDetailAdvanceTxn> GetTransactionsList(LinkButton lnk, string GridName, out bool IsOpenAdvance)
        {
            GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl(GridName)).Items;
            List<PaymentDetailAdvanceTxn> lstTXns = new List<PaymentDetailAdvanceTxn>();
            List<PaymentWorkflowStatu> lstPaymentWorkflowStatu = (List<PaymentWorkflowStatu>)Session["PaymentWorkflowStatus"];
            IsOpenAdvance = false;

            foreach (GridDataItem item in collection)
            {
                if (item.Selected)
                {
                    string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                    if (String.IsNullOrEmpty(ApprovedAmount.Trim()))
                        continue;

                    string Remark = ((RadTextBox)item.FindControl("tbRemark")).Text;
                    string Approval = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue;
                    string ProfileId = String.Empty;
                    string SAPPaymentId = item.GetDataKeyValue("Id").ToString();
                    string SubVerticalId = item.GetDataKeyValue("SubVerticalId").ToString();
                    string StatusId = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue;

                    if (((RadDropDownList)item.FindControl("cmbApproval")).SelectedText.ToLower().Contains("correction"))
                    {
                        ProfileId = ((RadDropDownList)item.FindControl("cmbStage")).SelectedValue;
                    }
                    else
                    {
                        ProfileId = Convert.ToString(Session["ProfileId"]);
                    }

                    if (((RadDropDownList)item.FindControl("cmbApproval")).SelectedText.ToLower().Contains("approved"))
                    {
                        string OpenAdvance = Convert.ToString(item.GetDataKeyValue("OpenAdvance"));

                        if (String.IsNullOrEmpty(OpenAdvance.Trim()))
                            IsOpenAdvance = false;

                        else if (Convert.ToDecimal(item.GetDataKeyValue("OpenAdvance")) > 0)
                        {
                            IsOpenAdvance = true;
                        }
                    }

                    PaymentDetailAdvanceTxn obj = new PaymentDetailAdvanceTxn();
                    obj.AppovedAmount = Convert.ToDecimal(ApprovedAmount);
                    obj.SAPAdvancePaymentId = Convert.ToInt32(SAPPaymentId);
                    obj.Comment = Remark;
                    obj.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    obj.CreatedOn = DateTime.Now;
                    obj.StatusId = Convert.ToInt32(StatusId);
                    obj.ProfileId = Convert.ToInt32(ProfileId);
                    obj.SubVerticalId = Convert.ToInt32(SubVerticalId);

                    PaymentWorkflowStatu objStatus =  lstPaymentWorkflowStatu.Where(c => c.StatusId == obj.StatusId && c.ProfileId == obj.ProfileId).FirstOrDefault<PaymentWorkflowStatu>();
                    if(objStatus != null)
                        obj.PaymentWorkflowStatusId = objStatus.PaymentWorkflowStatusId; // paymentWorkflowController.GetPaymentWorkflowStatusId(Convert.ToInt32(StatusId), Convert.ToInt32(ProfileId));

                    lstTXns.Add(obj);
                }
            }
            return lstTXns;
        }

        private List<PaymentDetailAgainstBillTxn> GetTransactionsAgainstList(LinkButton lnk, string GridName)
        {
            GridDataItemCollection collection = ((RadGrid)lnk.NamingContainer.FindControl(GridName)).Items;
            List<PaymentDetailAgainstBillTxn> lstTXns = new List<PaymentDetailAgainstBillTxn>();
            List<PaymentWorkflowStatu> lstPaymentWorkflowStatu = (List<PaymentWorkflowStatu>)Session["PaymentWorkflowStatus"];

            foreach (GridDataItem item in collection)
            {
                if (item.Selected)
                {
                    string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                    if (String.IsNullOrEmpty(ApprovedAmount.Trim()))
                        continue;

                    string Remark = ((RadTextBox)item.FindControl("tbRemark")).Text;
                    string Approval = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue;
                    string ProfileId = String.Empty;
                    string SAPPaymentId = item.GetDataKeyValue("Id").ToString();
                    string StatusId = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedValue;

                    if (((RadDropDownList)item.FindControl("cmbApproval")).SelectedText.ToLower().Contains("correction"))
                    {
                        ProfileId = ((RadDropDownList)item.FindControl("cmbStage")).SelectedValue;
                    }
                    else
                    {
                        ProfileId = Convert.ToString(Session["ProfileId"]);
                    }

                    //if (((RadDropDownList)item.FindControl("cmbApproval")).SelectedText.ToLower().Contains("approved"))
                    //{
                    //    string OpenAdvance = Convert.ToString(item.GetDataKeyValue("OpenAdvance"));
                    //    if (String.IsNullOrEmpty(OpenAdvance.Trim()))
                    //        IsOpenAdvance = false;
                    //    else if (Convert.ToDecimal(item.GetDataKeyValue("OpenAdvance")) > 0)
                    //    {
                    //        IsOpenAdvance = true;
                    //    }
                    //}

                    PaymentDetailAgainstBillTxn obj = new PaymentDetailAgainstBillTxn();
                    obj.AppovedAmount = Convert.ToDecimal(ApprovedAmount);
                    obj.SAPAgainstBillPaymentId = Convert.ToInt32(SAPPaymentId);
                    obj.Comment = Remark;
                    obj.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    obj.CreatedOn = DateTime.Now;
                    obj.StatusId = Convert.ToInt32(StatusId);
                    obj.ProfileId = Convert.ToInt32(ProfileId);

                    PaymentWorkflowStatu objStatus = lstPaymentWorkflowStatu.Where(c => c.StatusId == obj.StatusId && c.ProfileId == obj.ProfileId).FirstOrDefault<PaymentWorkflowStatu>();
                    if (objStatus != null)
                        obj.PaymentWorkflowStatusId = objStatus.PaymentWorkflowStatusId; // paymentWorkflowController.GetPaymentWorkflowStatusId(Convert.ToInt32(StatusId), Convert.ToInt32(ProfileId));

                    lstTXns.Add(obj);
                }
            }
            return lstTXns;
        }


        private bool CheckSelected(GridDataItemCollection collection)
        {
            bool selected = false; bool IsAmtEntered = true;
            bool isValidAmount = true;
            bool IsEnterRemark = true;
            bool IsAmtLess = true;
            string strMessage = String.Empty;

            decimal DebitAmt = 0;
            decimal CreditAmt = 0;

            foreach (GridDataItem item in collection)
            {
                if (item.Selected)
                {
                    selected = true;
                    string Stage = ((RadDropDownList)item.FindControl("cmbApproval")).SelectedItem.Text;
                    string ApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount")).Text;
                    string Remarks = ((RadTextBox)item.FindControl("tbRemark")).Text;
                    string Amount = item["Amount"] == null ? String.Empty : item["Amount"].Text.Trim();
                    decimal SAPAmt = 0;

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
                    


                    if (!String.IsNullOrEmpty(Amount.Trim()))
                    {
                        SAPAmt = Convert.ToDecimal(Amount);
                    }

                    decimal ApprovedAmt = 0;

                    if (String.IsNullOrEmpty(ApprovedAmount.Trim()) && Stage.Trim().ToLower().Contains("approved"))
                    {
                        if (IsAmtEntered)
                            strMessage += "<br />" + Constants.ENTERAMOUNT;
                        else
                            IsAmtEntered = false;
                      //  return false;
                    }
                    else
                    {
                        ApprovedAmt = Convert.ToDecimal(ApprovedAmount);
                    }

                    //if (String.IsNullOrEmpty(Remarks.Trim()) && IsEnterRemark)
                    //{         
                    //    strMessage += "<br />" + Constants.SELECTREMARK;
                    //    IsEnterRemark = false;
                    //}

                    if (ApprovedAmt > SAPAmt && IsAmtLess)
                    {
                        strMessage += "<br />" + Constants.APPROVEDAMOUNTVALIDATION;
                        IsAmtLess = false;
                    }

                    if (!IsAmtLess && !IsAmtEntered && !IsEnterRemark)
                        break;

                    if (String.IsNullOrEmpty(Convert.ToString(ApprovedAmount)))
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.SELECTAMOUNTREMARK);
                        return false;
                    }

                    if (Convert.ToDecimal(ApprovedAmount) < 1)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.SELECTAMOUNTREMARK);
                        return false;
                    }

                }
            }
 
            if (!selected)
            {

                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.SELECT_RECORD);
                return false;
            }
            if (DebitAmt >= CreditAmt)
            {
                ViewState["PaymentAllicationAmount"] = 0;
                radMessage.Title = "Alert";
                radMessage.Show("Its a debit balance.Hence could not proceed");
                return false;
            }
            else
            {
                ViewState["PaymentAllicationAmount"] = Convert.ToDecimal(CreditAmt - DebitAmt);
            }

            if (!String.IsNullOrEmpty(strMessage))
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(strMessage);
                return false;
            }

            return true;
        }
     

        protected void gvCorrection2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadDropDownList cmbStage = (RadDropDownList)e.Item.FindControl("cmbStage");
                string stage = (string)DataBinder.Eval(e.Item.DataItem, "ProfileIds").ToString();
                string StatusId = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "StatusId"));
                string ProfileId = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ProfileID"));

                string[] strArr = stage.Split(',');

                if (ViewState["Profiles"] != null)
                {
                    List<Models.ListItem> profiles = (List<Models.ListItem>)ViewState["Profiles"];
                    profiles = (from c in profiles
                                where strArr.Contains(c.Id) && c.Id != Convert.ToString(Session["ProfileId"])
                                select c).ToList();

                    cmbStage.DataSource = profiles;
                    cmbStage.DataTextField = "Name";
                    cmbStage.DataValueField = "Id";
                    cmbStage.DataBind();

                    if (!String.IsNullOrEmpty(ProfileId))
                        cmbStage.SelectedValue = ProfileId;
                }

                RadDropDownList cmbApproval = (RadDropDownList)e.Item.FindControl("cmbApproval");

                if (ViewState["Status"] != null)
                {
                    List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];

                    cmbApproval.DataSource = lststatus;
                    cmbApproval.DataTextField = "Name";
                    cmbApproval.DataValueField = "Id";
                    cmbApproval.DataBind();
                    cmbApproval.SelectedValue = "1";

                    // Commenetd by gaurang to show approved by default on Need Correction tab
                  //if(!String.IsNullOrEmpty(StatusId))
                  //cmbApproval.SelectedValue = StatusId;
                }

                if (cmbApproval.SelectedItem.Text.ToLower().Contains("correction"))
                {
                    cmbStage.Enabled = true;
                }
                else
                {
                    cmbStage.Enabled = false;
                }

                HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
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

                RadNumericTextBox txtAprovedAmount = (RadNumericTextBox)e.Item.FindControl("tbApprovedAmount");
                if (rbtnAdvance.Checked)
                    txtAprovedAmount.Enabled = false;
                else if (Convert.ToString(HttpContext.Current.Session["ProfileName"]) == "Auditor")
                    txtAprovedAmount.Enabled = false;

            }

            if (e.Item is GridHeaderItem && rbtnAdvance.Checked == true)
            {
                GridHeaderItem header = (GridHeaderItem)e.Item;
                header["ApprovedAmount"].Text = "Net Amount";
            }
        }

        protected void gvCorrection1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                RadGrid r = (sender as RadGrid);
                GridNestedViewItem item = ((Telerik.Web.UI.GridDataItem)(e.Item)).ChildItem as GridNestedViewItem;
                // GridNestedViewItem item = e.Item as GridNestedViewItem;
                RadGrid grid = (RadGrid)item.FindControl("gvCorrection2");

                string strCompany = (e.Item as GridDataItem).GetDataKeyValue("Company").ToString();
                string[] strArr = strCompany.Split('-');
                string CompCode = String.Empty;

                if (strArr.Length == 2)
                {
                    CompCode = Convert.ToString(strArr[1]);
                }

                // ((GridNestedViewItem)((GridDataItem)e.Item).OwnerTableView.NamingContainer.NamingContainer).ParentItem.GetDataKeyValue("Vendor")

                GridDataItem gridItem = ((GridDataItem)e.Item);
                GridTableView nestedview = (GridTableView)gridItem.OwnerTableView;
                GridNestedViewItem nestedviewItem = (GridNestedViewItem)nestedview.NamingContainer.NamingContainer;
                string strVendor = Convert.ToString(nestedviewItem.ParentItem.GetDataKeyValue("Vendor"));

                strArr = strVendor.Split('-');
                string VendorCode = String.Empty;

                if (strArr.Length == 2)
                {
                    VendorCode = Convert.ToString(strArr[1]);
                }

                BindItemsGrid(grid, VendorCode, CompCode);
            }
        }

        protected void gvCorrection_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                GridNestedViewItem item = ((Telerik.Web.UI.GridDataItem)(e.Item)).ChildItem as GridNestedViewItem;
                // GridNestedViewItem item = e.Item as GridNestedViewItem;
                RadGrid grid = (RadGrid)item.FindControl("gvCorrection1");
                string strVendor = (e.Item as GridDataItem).GetDataKeyValue("Vendor").ToString();
                string[] strArr = strVendor.Split('-');
                string VendorCode = String.Empty;

                if (strArr.Length == 2)
                {
                    VendorCode = Convert.ToString(strArr[1]);
                }

                BindCompanyGridByVendor(grid, VendorCode);
            }
        }

        protected void gvApproved_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                GridNestedViewItem item = ((Telerik.Web.UI.GridDataItem)(e.Item)).ChildItem as GridNestedViewItem;
                // GridNestedViewItem item = e.Item as GridNestedViewItem;
                RadGrid grid = (RadGrid)item.FindControl("gvApproved1");
                string strVendor = (e.Item as GridDataItem).GetDataKeyValue("Vendor").ToString();
                string[] strArr = strVendor.Split('-');
                string VendorCode = String.Empty;

                if (strArr.Length == 2)
                {
                    VendorCode = Convert.ToString(strArr[1]);
                }

                BindCompanyGridByVendor(grid, VendorCode);
            }
        }

        protected void gvApproved1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                RadGrid r = (sender as RadGrid);
                GridNestedViewItem item = ((Telerik.Web.UI.GridDataItem)(e.Item)).ChildItem as GridNestedViewItem;
                // GridNestedViewItem item = e.Item as GridNestedViewItem;
                RadGrid grid = (RadGrid)item.FindControl("gvApproved2");

                string strCompany = (e.Item as GridDataItem).GetDataKeyValue("Company").ToString();
                string[] strArr = strCompany.Split('-');
                string CompCode = String.Empty;

                if (strArr.Length == 2)
                {
                    CompCode = Convert.ToString(strArr[1]);
                }

                // ((GridNestedViewItem)((GridDataItem)e.Item).OwnerTableView.NamingContainer.NamingContainer).ParentItem.GetDataKeyValue("Vendor")

                GridDataItem gridItem = ((GridDataItem)e.Item);
                GridTableView nestedview = (GridTableView)gridItem.OwnerTableView;
                GridNestedViewItem nestedviewItem = (GridNestedViewItem)nestedview.NamingContainer.NamingContainer;
                string strVendor = Convert.ToString(nestedviewItem.ParentItem.GetDataKeyValue("Vendor"));

                strArr = strVendor.Split('-');
                string VendorCode = String.Empty;

                if (strArr.Length == 2)
                {
                    VendorCode = Convert.ToString(strArr[1]);
                }

                BindItemsGrid(grid, VendorCode, CompCode);
            }
        }

        protected void gvApproved2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadDropDownList cmbStage = (RadDropDownList)e.Item.FindControl("cmbStage");
                string stage = (string)DataBinder.Eval(e.Item.DataItem, "ProfileIds").ToString();
                string StatusId = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "StatusId"));
                string ProfileId = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ProfileID"));

                string[] strArr = stage.Split(',');

                if (ViewState["Profiles"] != null)
                {
                    List<Models.ListItem> profiles = (List<Models.ListItem>)ViewState["Profiles"];
                    profiles = (from c in profiles
                                where strArr.Contains(c.Id) && c.Id != Convert.ToString(Session["ProfileId"])
                                select c).ToList();

                    cmbStage.DataSource = profiles;
                    cmbStage.DataTextField = "Name";
                    cmbStage.DataValueField = "Id";
                    cmbStage.DataBind();

                    if (!String.IsNullOrEmpty(ProfileId))
                        cmbStage.SelectedValue = ProfileId;
                }


                RadDropDownList cmbApproval = (RadDropDownList)e.Item.FindControl("cmbApproval");


                if (ViewState["Status"] != null)
                {
                    List<Models.ListItem> lststatus = (List<Models.ListItem>)ViewState["Status"];

                    cmbApproval.DataSource = lststatus;
                    cmbApproval.DataTextField = "Name";
                    cmbApproval.DataValueField = "Id";
                    cmbApproval.DataBind();
                    cmbApproval.SelectedValue = "1";

                    if (!String.IsNullOrEmpty(StatusId))
                        cmbApproval.SelectedValue = StatusId;

                    //if(cmbApproval.SelectedItem.Text.ToLower().Contains("correction"))
                    //{
                    //    cmbStage.Enabled = true;
                    //}
                    //else
                    //{
                    //    cmbStage.Enabled = false;
                    //}
                    cmbStage.Enabled = false;
                }


                HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                commentLink.Attributes["href"] = "javascript:void(0);";
                commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);


            }

            if (e.Item is GridHeaderItem && rbtnAdvance.Checked == true)
            {
                GridHeaderItem header = (GridHeaderItem)e.Item;
                header["ApprovedAmount"].Text = "Net Amount";
            }
        }

        protected void cmbApproval_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            RadDropDownList dstatus = (sender as RadDropDownList);
          //  string StatusId = dstatus.SelectedValue;
            string statusText = dstatus.SelectedItem.Text.Trim();
            GridDataItem item = (GridDataItem)dstatus.Parent.NamingContainer;
            RadDropDownList cmbStage = ((RadDropDownList)item.FindControl("cmbStage"));
            RadNumericTextBox tbApprovedAmount = ((RadNumericTextBox)item.FindControl("tbApprovedAmount"));

            if(statusText.ToLower().Contains("correction"))  //Need correction
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

        protected void cmbApproval1_SelectedIndexChanged(object sender, DropDownListEventArgs e)
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
        protected void rdTab_TabClick(object sender, RadTabStripEventArgs e)
        {
            BindMainGrid();
        }

        protected void grdComment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["PaymentId"])))
                bindComment(Convert.ToInt32(ViewState["PaymentId"]));
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
                    if (Session["ClickedButton"] != null)
                    {
                        LinkButton lnk = (LinkButton)(Session["ClickedButton"]);
                        if (parameters[1] == "save")
                            SaveRecords(lnk);
                        else if (parameters[1] == "submit")
                            SubmitRecords(lnk, Constants.SUBMIT_MODE);

                        Session["ClickedButton"] = null;
                    }
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
                List<PaymentComments> lstComments = paymentWorkflowController.GetPaymentWorkFlowComment(paymentId, rbtnAgainst.Checked ? "AgainstBill" : "Advance");
                grdComment.DataSource = lstComments;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
          //  LinkButton lnk = (LinkButton)sender;
            //lnk.Parent.NamingContainer
           //     GridDataItem item = (GridDataItem)lnk.Parent.NamingContainer;
            //    item.Expanded = false;
        }

        protected void gvPendingApproval2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
               
                RadGrid masterGrid = (RadGrid)sender;
                string result = string.Empty;
                String Id = e.CommandArgument.ToString();
                if (e.CommandName == "Attachment")
                {
                    
                    //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                    RadAjaxManager1.ResponseScripts.Add("openRadWin('FNAApprover','update','" + Convert.ToString(Id) + "','true','true','true','false','PaymentInitiator');");
                }
                if (e.CommandName == "Refresh")
                {
                    result = paymentWorkflowController.RefreshAdavanceDataFormSAP(Convert.ToInt32(Id));
                    if (result == Constants.SAP_SUCCESS)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show("Data refresh done sucessfully");
                        GridTableView Child1View = e.Item.OwnerTableView; //get the Child1 from the currnet item
                        RadGrid gvapproval1 =(RadGrid) Child1View.Parent;

                        SAPAdvancePayment obj = paymentWorkflowController.GetSapAdvanceDetailById(Convert.ToInt32(Id));                       
                        BindItemsGrid(gvapproval1, obj.VendorCode, obj.CompanyCode);
                        gvapproval1.Rebind();
                      
                    }
                    else
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(result);
                        
                    }
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

                if (e.CommandName == "Attachment")
                {
                    String Id = e.CommandArgument.ToString();
                    //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                    RadAjaxManager1.ResponseScripts.Add("openRadWin('FNAApprover','update','" + Convert.ToString(Id) + "','true','false','true','false','PaymentInitiator');");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvCorrection2_ItemCommand(object sender, GridCommandEventArgs e)
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

        //protected void gvPendingApproval2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        //{
        //    BindDataSource();
        //}

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
                    grdBudgetAllocation.DataSource = paymentWorkflowController.GetBudgetAllocationDetails((rbtnAgainst.Checked ? "AG" : "AD"), Convert.ToString(ViewState["SelectedSAPIDs"]));
                  

                }
            }
            catch (Exception)
            {

                throw;
            }
            //if (ViewState["SelectedSAPIDs"] != null)
           
        }

        protected void gvApproved_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindDataSource();
        }

        protected void gvCorrection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindDataSource();
        }

        protected void gvPendingApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindDataSource();
        }

        //protected void txtCurrentAllocation_TextChanged(object sender, EventArgs e)
        //{
        //    RadNumericTextBox numbox = (RadNumericTextBox)sender;
        //    GridDataItem item = (GridDataItem)numbox.NamingContainer;
        //    Decimal CurrentBalanceAmount = (decimal)(((SuzlonBPP.Models.GetBudgetAllocationDetails_Result)item.DataItem).BalanceAmount);

        //    if (Convert.ToDecimal(numbox.Text) > CurrentBalanceAmount)
        //    {
        //        numbox.Text = "0.00";
        //        lblError.Text = "Please Add Value Less Than Or Equal To Balance Amount.";
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openAllocationPopup()", true);
        //    }
        //}

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

                numCurrentAllocation.Text = String.Empty;

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
                footerItem["TemplateBalanceAmount"].Text =string.Format("{0:##,#0.00}", double.Parse(TotalBalance.ToString()), System.Globalization.CultureInfo.InvariantCulture);
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
                    List<PaymentDetailAdvance> PaymentRecords = PaymentRecords = ((List<PaymentDetailAdvance>)Session["SubmitPayment"]).OrderBy(a => a.SubVerticalId).ToList();

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
                SubmitRecords((LinkButton)Session["btSender"], Constants.SAVE_MODE);
                BindMainGrid();
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
                //BalanceAmount = Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(CurrentUserAllocation.Text)) ? "0" : CurrentUserAllocation

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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportDataToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void ExportDataToExcel()
        {
            string billType = string.Empty;
            string tabType = string.Empty;

            if (rbtnAgainst.Checked)
                billType = "1";
            else if (rbtnAdvance.Checked)
                billType = "2";

            if (rdTab.SelectedIndex == 0)
                tabType = "1";
            else if (rdTab.SelectedIndex == 1)
                tabType = "2";
            else if (rdTab.SelectedIndex == 2)
                tabType = "3";

            List<GetItemsForExportToExcel_Result> lstGetCompanyData = CommonFunctions.GetItemsForExportToExcel(Convert.ToInt32(cmbVertical.SelectedValue),
                                                            Convert.ToInt32(Session["UserId"]), billType, tabType);
            DataTable dt = CommonFunctions.ConvertListToDataTable(lstGetCompanyData);
            if (billType == "1")
                dt.Columns["DPR_Number"].ColumnName = "Document Number";

            RemoveColumnFromDatatable(billType, tabType, dt);
            CommonFunctions.ExporttoExcel(dt, Response, "Report.xls");
        }

        private static void RemoveColumnFromDatatable(string billType, string tabType, DataTable dt)
        {
            if (billType == "1")
            {
                if (tabType == "1" || tabType == "3")
                    dt.Columns.Remove("Status");
                dt.Columns.Remove("Expected_Clearing_Date");
                dt.Columns.Remove("House_Bank");
                dt.Columns.Remove("Vertical");
                dt.Columns.Remove("OpenAdvance");
                dt.Columns.Remove("Purchasing_Document");
                dt.Columns.Remove("Special_GL");
                dt.Columns.Remove("Withholding_Tax_Code");
                dt.Columns.Remove("Un_settled_Open_Advance__INR_");
                dt.Columns.Remove("Justification_for_Adv_Payment");
                dt.Columns.Remove("Gross_Amount");
                dt.Columns.Remove("Tax_Rate");
                dt.Columns.Remove("Tax_Amount");
                //dt.Columns.Remove("Remark");

                if (tabType == "2")
                {
                    dt.Columns.Remove("Remark");
                    dt.Columns.Remove("Approval");
                }

               else if (tabType != "3")
                    dt.Columns.Remove("Assigned_By");
            }
            else if (billType == "2")
            {
                if (tabType == "1")
                {
                    dt.Columns.Remove("Status");
                    dt.Columns.Remove("Assigned_By");
                }

                else if (tabType == "2")
                {
                    dt.Columns.Remove("Assigned_By");
                    //dt.Columns.Remove("Status");
                    dt.Columns.Remove("Remark");
                    dt.Columns.Remove("Approval");
                }
                else if (tabType == "3")
                {
                    dt.Columns.Remove("Status");
                    //dt.Columns.Remove("Assigned_By");
                }

                dt.Columns.Remove("House_Bank");
                dt.Columns.Remove("Vertical");
                dt.Columns.Remove("OpenAdvance");
            }
        }

        protected void lnkVerticalBudget_Click(object sender, EventArgs e)
        {
            try
            {               
                grdVerticalBudget.DataSource = paymentWorkflowController.getVerticalBalanceDetails(Convert.ToInt32(cmbVertical.SelectedValue));
                grdVerticalBudget.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openVerticalBudgerPopup();", true);
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
    }

    public class PaymentComments
    {
        public string UserName { get; set; }
        public string WorkFlowStatus { get; set; }
        public decimal AppovedAmount { get; set; }
        public string Comment { get; set; }        
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }

}