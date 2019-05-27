using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq;
using System.Collections;

namespace SuzlonBPP
{
    public partial class VendorPaymentInitiator : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        public decimal TotolINRAmtAgainst = 0;
        public decimal TotolProposedAmtAgainst = 0;
        public decimal TotolINRAmtAdvance = 0;
        public decimal TotolProposedAmtAdvance = 0;
        public decimal TotolINRAmtAgPopup = 0;
        public decimal TotolProposedAmtAgPopup = 0;
        public decimal TotolINRAmtAdPopup = 0;
        public decimal TotolProposedAmtAdPopup = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //string parameter = Request["__EVENTARGUMENT"];
                //if (parameter == "Against"|| parameter == "Advance")
                //    btnOptions_SelectedIndexChanged(sender, e);

                if (!IsPostBack)
                {

                    pnlAgainstBill.Visible = true;
                    pnlAdvanceBill.Visible = false;               
                    btnOptions.SelectedValue = "Against";
                    GetCompany();
                    DataTable dt  =   BindSubVertical();
                    Session["InitiatorSubVerticalAndVertical"] = dt;          
                    BindNatureOfRequest();
                  //  bindRequestInProcess();
                    grdWorkFlowInProcess.DataBind();
                    Session["ATTACHMENT"] = null;

                    gvRequest.DataSource = new DataTable();
                    gvRequest.DataBind();
                    gvIntitiatorInProcess.DataSource = new DataTable();
                    gvIntitiatorInProcess.DataBind();
                    gvInitiatorNeedCorrection.DataSource = new DataTable();
                    gvInitiatorNeedCorrection.DataBind();
                    gvAllRequest.DataSource = new DataTable();
                    gvAllRequest.DataBind();
                    RadGrid1.DataSource = new DataTable();
                    RadGrid1.DataBind();
                    RadGrid2.DataSource = new DataTable();
                    RadGrid2.DataBind();
                    RadGrid3.DataSource = new DataTable();
                    RadGrid3.DataBind();
                    gvAllRequest.DataSource = new DataTable();
                    gvAllRequest.DataBind();

                    RadGrid4.DataSource = new DataTable();
                    RadGrid4.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }


        private void bindRequestInProcess()
        {
            string resultvalues = commonFunctions.RestServiceCall(string.Format(Constants.GETPAYMENTS_WORKFLOW_INPROCESSDETAIL, btnOptions.SelectedValue == "Against" ? "AgainstBill" : "Advance"), string.Empty);
            List<GetPaymentWorkFlowInProcessDetail_Result> requestDetail = JsonConvert.DeserializeObject<List<GetPaymentWorkFlowInProcessDetail_Result>>(resultvalues);
            grdWorkFlowInProcess.DataSource = requestDetail;
        }  
        private void BindNatureOfRequest()
        {
            string resultvalues = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + "nature-of-request-automatic" + "", string.Empty);
            DropdownValues ddlbValues = JsonConvert.DeserializeObject<DropdownValues>(resultvalues);
            Session["Initiatornature-of-request"] = ddlbValues.NatureOfRequest;
        }    

        private void GetCompany()
        {
            string result = string.Empty;
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
           
            string res = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETUSERDTL + Convert.ToString(HttpContext.Current.Session["UserId"]) + "", string.Empty);
            if (res != Constants.REST_CALL_FAILURE)
            {                UserModel UserInfo = JsonConvert.DeserializeObject<UserModel>(res);
                if (UserInfo != null)
                {
                    result = commonFunctions.RestServiceCall(string.Format(Constants.Company_GET_USERWISE, UserInfo.UserDetail.Company), string.Empty);
                    if (result != Constants.REST_CALL_FAILURE)
                    {
                        DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                        cmbCompany.DataSource = ddValues.Company;
                        cmbCompany.DataTextField = "Name";
                        cmbCompany.DataValueField = "Id";
                        cmbCompany.DataBind();
                        if(cmbCompany.Items.Count>0)
                            cmbCompany.Items.Insert(0, new RadComboBoxItem("All", "All"));

                    }                         
                }
            }
          
          
        }

        protected void cmbCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string result = string.Empty;
           // result = commonFunctions.RestServiceCall(string.Format(Constants.Company_GET_VENDORS, cmbCompany.SelectedValue), string.Empty);
            if (result != Constants.REST_CALL_FAILURE)
            {
             //   DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
              //  cmbVendor.DataSource = ddValues.VendorCode;
              //  cmbVendor.DataTextField = "Name";
               // cmbVendor.DataValueField = "Id";
                cmbVendor.DataBind();
                cmbVendor.SelectedIndex = -1;
                cmbVendor.ClearSelection();
                cmbVendor.Text = "";
                grdVendorSearch.DataSource = new DataTable();
                grdVendorSearch.DataBind();
            }
            txtCompanyCode.Text = cmbCompany.SelectedItem.Text;


        }        

        private DataTable BindSubVertical()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("VerticalID",typeof(Int32)));
            dt.Columns.Add(new DataColumn("VerticalName", typeof(string)));
            dt.Columns.Add(new DataColumn("SubVerticalID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("SubVerticalName", typeof(string)));


            var sub = new List<Models.ListItem>();
            try
            {
                string vendorCode = string.Empty;
             //   string drpname = "vertical";

                string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETUSERDTL + Convert.ToString(HttpContext.Current.Session["UserId"]) + "", string.Empty);
                if (result != Constants.REST_CALL_FAILURE)
                {
                    UserModel UserInfo = JsonConvert.DeserializeObject<UserModel>(result);
                    if (UserInfo != null)
                    {
                        string jsonInputParameter = JsonConvert.SerializeObject(UserInfo);
                        result = commonFunctions.RestServiceCall(Constants.SUBVERTICAL_BYUSER, Crypto.Instance.Encrypt(jsonInputParameter));
                        if (result != Constants.REST_CALL_FAILURE)
                        {
                            List<SubVerticalModel> subVerticals = JsonConvert.DeserializeObject<List<SubVerticalModel>>(result);
                            foreach (var subitem in subVerticals)
                            {
                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(Convert.ToInt32(subitem.VerticalId), subitem.VerticalName, Convert.ToInt32(subitem.SubVerticalId), subitem.Name);
                            }
                        }                   

                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
            return dt;

        }

        private void BindInProcessGrid()
        {
            var BillInProcess = commonFunctions.RestServiceCall(string.Format(Constants.GET_AGAINSTBILL_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (BillInProcess == Constants.REST_CALL_FAILURE)
            {
                radMessage.Show("Error in Getting InProcess Data");
                gvIntitiatorInProcess.DataSource = new DataTable();
            }
            else
            {
                List<sp_GetInitiator_AgainstBill_InProcess_Data_Result> lstGetCompanyData_Result = JsonConvert.DeserializeObject<List<sp_GetInitiator_AgainstBill_InProcess_Data_Result>>(BillInProcess);
                gvIntitiatorInProcess.DataSource = lstGetCompanyData_Result;

            }
        }

        private void BindAdvanceInProcessGrid()
        {
            var PaymentInProcess = commonFunctions.RestServiceCall(string.Format(Constants.GET_ADVANCE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (PaymentInProcess == Constants.REST_CALL_FAILURE)
            {
                radMessage.Show("Error in Getting InProcess Data");
                RadGrid2.DataSource  = new DataTable();
            }
            else
            {
                List<sp_GetInitiator_Advance_InProcess_Data_Result> lstGetCompanyData_Result = JsonConvert.DeserializeObject<List<sp_GetInitiator_Advance_InProcess_Data_Result>>(PaymentInProcess);
                RadGrid2.DataSource = lstGetCompanyData_Result;
            }
        }

        private void bindMyRequestGrid()
        {

            if (btnOptions.SelectedValue == "Against")
            {
                List<GetSAPAgainstBillIntialData_Result> lstAgainstBillPayments = new List<GetSAPAgainstBillIntialData_Result> ();
               
                var result = commonFunctions.RestServiceCall(string.Format(Constants.GET_AGAINSTBILL_PAYMENT_DETAILS, cmbCompany.SelectedValue, cmbVendor.SelectedValue, "MyTabTodays"), string.Empty);
                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Show("Error in Getting Requests");
                    gvRequest.DataSource = new DataTable();
                }
                else
                {
                    lstAgainstBillPayments = JsonConvert.DeserializeObject<List<GetSAPAgainstBillIntialData_Result>>(result);                   
                    gvRequest.DataSource = lstAgainstBillPayments;
                    
                }             
            }        
        }
        private void bindGridMyRequestAlldata()
        {

            if (btnOptions.SelectedValue == "Against")
            {
                List<GetSAPAgainstBillIntialData_Result> lstAgainstBillPayments = new List<GetSAPAgainstBillIntialData_Result>();
              
                var result = commonFunctions.RestServiceCall(string.Format(Constants.GET_AGAINSTBILL_PAYMENT_DETAILS, cmbCompany.SelectedValue, cmbVendor.SelectedValue, "MyTabAll"), string.Empty);
                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Show("Error in Getting Requests");
                    gvAllRequest.DataSource = new DataTable();
                }
                else
                {                    
                    lstAgainstBillPayments = JsonConvert.DeserializeObject<List<GetSAPAgainstBillIntialData_Result>>(result);
                    gvAllRequest.DataSource = lstAgainstBillPayments;

                }
            }
        }

        private void BindAgainstBillCorrectionGrid()
        {
            var AdvanceCorrection = commonFunctions.RestServiceCall(string.Format(Constants.GET_AGAINSTBILL_CORRECTION, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (AdvanceCorrection == Constants.REST_CALL_FAILURE)
            {
                gvInitiatorNeedCorrection.DataSource = new DataTable();
            }
            else
            {
                List<sp_GetInitiator_AgainstBill_Correction_Data_Result> lstGetCompanyData_Result = JsonConvert.DeserializeObject<List<sp_GetInitiator_AgainstBill_Correction_Data_Result>>(AdvanceCorrection);
                gvInitiatorNeedCorrection.DataSource = lstGetCompanyData_Result;
            }
        }

        private void BindAdvanceCorrectionGrid()
        {
            var AdvanceCorrection = commonFunctions.RestServiceCall(string.Format(Constants.GET_ADVANCE_CORRECTION, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (AdvanceCorrection == Constants.REST_CALL_FAILURE)
            {
                gvInitiatorNeedCorrection.DataSource = new DataTable();
            }
            else
            {
                List<sp_GetInitiator_Advance_Correction_Data_Result> lstGetCompanyData_Result = JsonConvert.DeserializeObject<List<sp_GetInitiator_Advance_Correction_Data_Result>>(AdvanceCorrection);
                RadGrid3.DataSource = lstGetCompanyData_Result;
            }
        }

        private void BindAdvanceMyRequestGrid()
        {
            List<GetSAPAdvanceIntialData_Result> lstAdvanceBillPayments = new List<GetSAPAdvanceIntialData_Result>();
                var result = commonFunctions.RestServiceCall(string.Format(Constants.GET_ADVANCE_PAYMENT_DETAILS, cmbCompany.SelectedValue, cmbVendor.SelectedValue,"MyTabTodays"), string.Empty);
                if (result == Constants.REST_CALL_FAILURE)
                {
                   RadGrid1.DataSource = new DataTable();
                }
                else
                {
                    lstAdvanceBillPayments = JsonConvert.DeserializeObject<List<GetSAPAdvanceIntialData_Result>>(result);
                    RadGrid1.DataSource = lstAdvanceBillPayments;
                }
           
        }
        private void BindAdvanceMyRequestAllData()
        {
            List<GetSAPAdvanceIntialData_Result> lstAdvanceBillPayments = new List<GetSAPAdvanceIntialData_Result>();
            var result = commonFunctions.RestServiceCall(string.Format(Constants.GET_ADVANCE_PAYMENT_DETAILS, cmbCompany.SelectedValue, cmbVendor.SelectedValue, "MyTabAll"), string.Empty);
            if (result == Constants.REST_CALL_FAILURE)
            {
                RadGrid4.DataSource = new DataTable();
            }
            else
            {
                lstAdvanceBillPayments = JsonConvert.DeserializeObject<List<GetSAPAdvanceIntialData_Result>>(result);
                RadGrid4.DataSource = lstAdvanceBillPayments;
            }

        }

        protected void gvRequest_ItemDataBound(object sender, GridItemEventArgs e)
        {
           
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem headerItem = (GridHeaderItem)e.Item;
                CheckBox chkBox = (CheckBox)headerItem["RequestSelect"].Controls[0];
                chkBox.Text = "Select";
            }

            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem editItem = (GridEditableItem)e.Item;
                RadDropDownList cmbSubVertical = (RadDropDownList)editItem.FindControl("cmbSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)editItem.FindControl("cmbNatureofRequest");
                RadTextBox tbRequestRemarks = (RadTextBox)editItem.FindControl("tbRequestRemarks");             
               
               
                if (cmbNatureofRequest != null)
                {
                    cmbNatureofRequest.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                    cmbNatureofRequest.DataTextField = "Name";
                    cmbNatureofRequest.DataValueField = "Id";
                    cmbNatureofRequest.DataBind();

                    if (DataBinder.Eval(e.Item.DataItem, "NatureofRequestId") != null)
                    {
                        cmbNatureofRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureofRequestId").ToString();
                        Label lblRequestNatureofRequest = (Label)editItem.FindControl("lblRequestNatureofRequest");
                        lblRequestNatureofRequest.Text = cmbNatureofRequest.SelectedText;

                        editItem["NatureOfRequest1"].Text= lblRequestNatureofRequest.Text;

                        if (Session["LastSelectedNatureofRequest" + e.Item.ItemIndex.ToString()] == null)
                            Session["LastSelectedNatureofRequest" + e.Item.ItemIndex.ToString()] = cmbNatureofRequest.SelectedText;
                    }
                }  
               
                       
               
                Label lbVertical = (Label)editItem.FindControl("lbVerticalRequest");
                if (lbVertical != null && cmbSubVertical != null)
                    lbVertical.Text = cmbSubVertical.SelectedText;
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)editItem.FindControl("tbAmountProposed");
                if (tbAmountProposed != null)
                    tbAmountProposed.MaxValue = (Double)Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Amount").ToString());
                if (cmbSubVertical != null)
                {
                    cmbSubVertical.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                    cmbSubVertical.DataTextField = "SubVerticalName";
                    cmbSubVertical.DataValueField = "SubVerticalID";
                    cmbSubVertical.DataBind();

                    if ((int)DataBinder.Eval(e.Item.DataItem, "SubVerticalId") != 0)
                    {
                        cmbSubVertical.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "SubVerticalId").ToString();
                    }

                    Label lblRequestSubVertical = (Label)editItem.FindControl("lblRequestSubVertical");
                    lblRequestSubVertical.Text = cmbSubVertical.SelectedText;
                    editItem["SubVertical1"].Text = lblRequestSubVertical.Text;

                    string SubVertical = (editItem["SubVertical"].FindControl("cmbSubVertical") as RadDropDownList).SelectedValue;
                int Id = 0;
                (editItem["Vertical"].FindControl("lbVerticalRequest") as Label).Text = GetVerticals(SubVertical, ref Id);
                (editItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
                    Label Label1 = (Label)editItem.FindControl("Label1");
                    int SubId = 0;
                    Label1.Text= GetVerticals(cmbSubVertical.SelectedValue, ref SubId);
                    if (Session["LastSelectedSubVertical" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedSubVertical" + e.Item.ItemIndex.ToString()] = cmbSubVertical.SelectedValue;
                }

                //if (Convert.ToInt32(item.GetDataKeyValue("SaveMode")) == 1)
                //{
                    item.Selected = true;
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Amount"].Text)))
                    {
                        if(Convert.ToString(item["DCFLag"].Text) == "S")
                        TotolINRAmtAgainst = TotolINRAmtAgainst - Convert.ToDecimal(Convert.ToString(item["Amount"].Text));
                        else
                        TotolINRAmtAgainst = TotolINRAmtAgainst + Convert.ToDecimal(Convert.ToString(item["Amount"].Text));
                }
                    lblINRAmt.Text = TotolINRAmtAgainst.ToString();

                    if (!string.IsNullOrEmpty(Convert.ToString(item["AmountProposed1"].Text)))
                    {
                    if (Convert.ToString(item["DCFLag"].Text) == "S")
                        TotolProposedAmtAgainst = TotolProposedAmtAgainst - Convert.ToDecimal(Convert.ToString(item["AmountProposed1"].Text));
                    else
                        TotolProposedAmtAgainst = TotolProposedAmtAgainst + Convert.ToDecimal(Convert.ToString(item["AmountProposed1"].Text));
                    }
                    lblProposedAmt.Text = TotolProposedAmtAgainst.ToString();

                //}                
                if (!string.IsNullOrEmpty(Convert.ToString(item["BaseLineDate"].Text)) &&  Convert.ToString(item["BaseLineDate"].Text)!="&nbsp;")
                {
                    DateTime D1 = Convert.ToDateTime(item["BaseLineDate"].Text);
                    int duedays = 0;
                    duedays = Convert.ToInt32(item["DueDays"].Text);
                    item["RequestNetduedate"].Text = D1.AddDays(duedays).ToShortDateString();
               }

                if (Convert.ToString(item["DCFLag"].Text) == "S")
                {
                    item["DCFLag"].Text = "Debit";
                }
                if (Convert.ToString(item["DCFLag"].Text) == "H")
                {
                    item["DCFLag"].Text = "Credit";
                }


            }
            
        }

        protected void gvRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindMyRequestGrid();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindAdvanceMyRequestGrid();
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem headerItem = (GridHeaderItem)e.Item;
                CheckBox chkBox = (CheckBox)headerItem["Select"].Controls[0];
                chkBox.Text = "Select";
            }

            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem editItem = (GridEditableItem)e.Item;
                RadDropDownList cmbSubVertical = (RadDropDownList)editItem.FindControl("cmbAdvReqSubVertical");               
                RadDropDownList cmbNatureofRequest = (RadDropDownList)editItem.FindControl("cmbNatureofRequest");         
               

                if (cmbNatureofRequest != null)
                {
                    cmbNatureofRequest.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                    cmbNatureofRequest.DataTextField = "Name";
                    cmbNatureofRequest.DataValueField = "Id";
                    cmbNatureofRequest.DataBind();

                    if (DataBinder.Eval(e.Item.DataItem, "NatureofRequest") != null)
                    {
                        cmbNatureofRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureofRequest").ToString();
                        Label lblRequestNatureofRequest = (Label)editItem.FindControl("lblRequestNatureofRequest");
                        lblRequestNatureofRequest.Text = cmbNatureofRequest.SelectedText;
                        editItem["NatureOfRequest1"].Text = lblRequestNatureofRequest.Text;
                        if (Session["LastSelectedAdvNatureofRequest" + e.Item.ItemIndex.ToString()] == null)
                            Session["LastSelectedAdvNatureofRequest" + e.Item.ItemIndex.ToString()] = cmbNatureofRequest.SelectedText;
                    }
                }

                Label lbVertical = (Label)editItem.FindControl("lbVerticalAdvReq");
                if (lbVertical != null && cmbSubVertical != null)
                    lbVertical.Text = cmbSubVertical.SelectedText;
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)editItem.FindControl("tbProposed");
                if (tbAmountProposed != null)
                    tbAmountProposed.MaxValue = (Double)Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Amount").ToString());

                if (cmbSubVertical != null)
                {
                    cmbSubVertical.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                    cmbSubVertical.DataTextField = "SubVerticalName";
                    cmbSubVertical.DataValueField = "SubVerticalID";
                    cmbSubVertical.DataBind();

                    if ((int)DataBinder.Eval(e.Item.DataItem, "SubVerticalId") != 0)
                    {
                        cmbSubVertical.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "SubVerticalId").ToString();
                    }


                    Label lblRequestSubVertical = (Label)editItem.FindControl("lblRequestSubVertical");
                    lblRequestSubVertical.Text = cmbSubVertical.SelectedText;
                    editItem["SubVertical1"].Text = lblRequestSubVertical.Text;

                    string SubVertical = (editItem["SubVertical"].FindControl("cmbAdvReqSubVertical") as RadDropDownList).SelectedValue;
                    int Id = 0;
                    (editItem["Vertical"].FindControl("lbVerticalAdvReq") as Label).Text = GetVerticals(SubVertical, ref Id);
                    (editItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
                    Label Label1 = (Label)editItem.FindControl("Label1");
                    int SubId = 0;
                    Label1.Text = GetVerticals(cmbSubVertical.SelectedValue, ref SubId);
                    if (Session["LastSelectedAdvSubVertical" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedAdvSubVertical" + e.Item.ItemIndex.ToString()] = cmbSubVertical.SelectedValue;
                }


                //if (!string.IsNullOrEmpty(Convert.ToString(Session["itemsChecked"])))
                //{
                //    List<string> checkedItem = new List<string>();
                //    checkedItem = (List<string>)Session["itemsChecked"];
                //    if (checkedItem.Count > 0)
                //    {
                //if (Convert.ToInt32(item.GetDataKeyValue("SaveMode"))==1)
                //{
                    item.Selected = true;
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Amount"].Text)))
                    {
                    if (Convert.ToString(item["DCFLag"].Text) == "S")
                        TotolINRAmtAdvance = TotolINRAmtAdvance - Convert.ToDecimal(Convert.ToString(item["Amount"].Text));
                    else
                        TotolINRAmtAdvance = TotolINRAmtAdvance + Convert.ToDecimal(Convert.ToString(item["Amount"].Text));
                }
                    Label3.Text = TotolINRAmtAdvance.ToString();
                    if (!string.IsNullOrEmpty(Convert.ToString(item["AmountProposed1"].Text)))
                    {
                    if (Convert.ToString(item["DCFLag"].Text) == "S")
                        TotolProposedAmtAdvance = TotolProposedAmtAdvance - Convert.ToDecimal(Convert.ToString(item["AmountProposed1"].Text));
                    else
                        TotolProposedAmtAdvance = TotolProposedAmtAdvance + Convert.ToDecimal(Convert.ToString(item["AmountProposed1"].Text));
                }
                    Label5.Text = TotolProposedAmtAdvance.ToString();
                //}

                //    }
                //}
                if (Convert.ToString(item["DCFLag"].Text) == "S")
                {
                    item["DCFLag"].Text = "Debit";
                }
                if (Convert.ToString(item["DCFLag"].Text) == "H")
                {
                    item["DCFLag"].Text = "Credit";
                }



            }
        }

        protected void btnOptions_SelectedIndexChanged(object sender, EventArgs e)
        {            
            var result = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (Convert.ToBoolean(result))
            {
                string msg = "For Vendor " + cmbVendor.SelectedValue.ToString() + " - " + cmbVendor.SelectedItem.Text + " and Company " + cmbCompany.SelectedItem.Text + " bank details updation is in Progress you cannot submit the record for payment.";
                radMessage.Title = "Alert";              
                radMessage.Show(msg.ToString());
                return;
            }

            pnlAgainstBill.Visible = false;
            pnlAdvanceBill.Visible = false;
            if (btnOptions.SelectedValue == "Against")
            {
                pnlAgainstBill.Visible = true;
                bindMyRequestGrid();
                gvRequest.DataBind();
                BindInProcessGrid();
                gvIntitiatorInProcess.DataBind();
                BindAgainstBillCorrectionGrid();
                gvInitiatorNeedCorrection.DataBind();

                bindGridMyRequestAlldata();
                gvAllRequest.DataBind();
            }
            else
            {
                pnlAdvanceBill.Visible = true;
                BindAdvanceMyRequestGrid();
                RadGrid1.DataBind();
                BindAdvanceInProcessGrid();
                RadGrid2.DataBind();
                BindAdvanceCorrectionGrid();
                RadGrid3.DataBind();
                BindAdvanceMyRequestAllData();
                RadGrid4.DataBind();
            }
           // bindRequestInProcess();
            grdWorkFlowInProcess.DataBind();
        }

        protected void btnRequestSave_Click(object sender, EventArgs e)
        {
            decimal DebitAmt = 0;
            decimal CreditAmt = 0;
            var BankDetailInprocess = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (Convert.ToBoolean(BankDetailInprocess))
            {
                string msg = "For Vendor " + cmbVendor.SelectedValue.ToString() + " - " + cmbVendor.SelectedItem.Text + " and Company " + cmbCompany.SelectedItem.Text + " bank details updation is in Progress you cannot submit the record for payment.";
                radMessage.Title = "Alert";
                //radMessage.Show(Constants.ISVENDORBANKDETAILUPDATE_INPROCESSMSG);
                radMessage.Show(msg.ToString());
                return;
            }

            LinkButton btn = (LinkButton)sender;
            var result = string.Empty;
            if (gvRequest.SelectedItems.Count == 0)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Please Select at least one record");
                return;
            }

            foreach (GridDataItem itm in gvRequest.SelectedItems)
            {
                ArrayList values = new ArrayList();
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposed");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarks");

                if (!string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text)))
                {               

                    if (Convert.ToString(itm["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        DebitAmt = DebitAmt + Convert.ToDecimal(tbAmountProposed.Text);
                    else if (Convert.ToString(itm["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        CreditAmt = CreditAmt + Convert.ToDecimal(tbAmountProposed.Text);

                }

            }
            if (DebitAmt >= CreditAmt)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Its a debit balance.Hence could not proceed");
                return;
            }

            foreach (GridDataItem itm in gvRequest.SelectedItems)
            {
                ArrayList values = new ArrayList();
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposed");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarks");
                int SAPAgainstBillPaymentId = (int)itm.GetDataKeyValue("SAPAgainstBillPaymentId");
                values.Add(SAPAgainstBillPaymentId);
                values.Add(cmbSubVertical.SelectedValue);
                values.Add(cmbNatureofRequest.SelectedValue);
                values.Add(tbAmountProposed.Text);
                values.Add(tbRemarks.Text);
                values.Add((btn.CommandArgument == "Submit") ? true : false);             
                DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                var rows = SubVertical.Select("SubVerticalID = '" + cmbSubVertical.SelectedValue + "'");
                values.Add((rows.Count() > 0) ? Convert.ToInt32(rows[0]["VerticalID"].ToString()) : 0);
                values.Add(Convert.ToInt32(Session["ProfileId"]));
                values.Add(HttpContext.Current.Session["UserId"]);            
                string jsonInputParameter = JsonConvert.SerializeObject(values);               
                 result = commonFunctions.RestServiceCall(Constants.AGAINST_BILL_TRANSACTIONS, Crypto.Instance.Encrypt(jsonInputParameter));
                             
            }
            


            string selPaymentId = string.Empty;            
            if (btn.CommandArgument != "Submit")
            {
                ArrayList SaveUpdateValues = new ArrayList();
                foreach (GridDataItem itms in gvRequest.Items)
                {
                    if (itms.Selected)
                        selPaymentId = selPaymentId + "," + Convert.ToString(itms.GetDataKeyValue("SAPAgainstBillPaymentId"));
                }
                SaveUpdateValues.Add(Convert.ToString(cmbVendor.SelectedValue));
                SaveUpdateValues.Add(Convert.ToString(cmbCompany.SelectedValue));

                SaveUpdateValues.Add(selPaymentId);
                string jsonInputParameter = JsonConvert.SerializeObject(SaveUpdateValues);
             string Saveresult = commonFunctions.RestServiceCall(Constants.AGAINST_BILL_SAVEUPDATEMODE, Crypto.Instance.Encrypt(jsonInputParameter));
           }

            if (result == "true")
            {
                radMessage.Title = "Message";
                if ((btn.CommandArgument == "Submit"))
                    radMessage.Show("Data submitted successfully");
                else
                    radMessage.Show("Saved Successfully");
            }

            bindMyRequestGrid();
            gvRequest.DataBind();
            BindInProcessGrid();
            gvIntitiatorInProcess.DataBind();
            bindGridMyRequestAlldata();
            gvAllRequest.DataBind();
        }

        public bool ValidateSearchFields()
        {
            bool isValid = true;
            radMessage.Title = "Alert";
            if (cmbCompany.SelectedItem == null)
            {
                radMessage.Show("Please select company");
                isValid = false;
            }

            if (cmbVendor.SelectedItem == null && cmbCompany.SelectedValue!="All") 
            {
                radMessage.Show("Please select vendor");
                isValid = false;
            }
            return isValid;
        }


        public void GetDataForSearch()
        {
            pnlAgainstBill.Visible = false;
            pnlAdvanceBill.Visible = false;
            if (btnOptions.SelectedValue == "Against")
            {
                pnlAgainstBill.Visible = true;
                bindMyRequestGrid();
                gvRequest.DataBind();
                BindInProcessGrid();
                gvIntitiatorInProcess.DataBind();
                BindAgainstBillCorrectionGrid();
                gvInitiatorNeedCorrection.DataBind();
                bindGridMyRequestAlldata();
                gvAllRequest.DataBind();
            }
            else
            {
                pnlAdvanceBill.Visible = true;
                BindAdvanceMyRequestGrid();
                RadGrid1.DataBind();
                BindAdvanceInProcessGrid();
                RadGrid2.DataBind();
                BindAdvanceCorrectionGrid();
                RadGrid3.DataBind();
                BindAdvanceMyRequestAllData();
                RadGrid4.DataBind();
            }
        }

        protected void lbSearch_Click(object sender, EventArgs e)
        {
            if (ValidateSearchFields())
            {
                GetDataForSearch();
            }
        }

        protected void btnAddMore_Click(object sender, EventArgs e)
        {            
            radMessage.Title = "Alert";
            if (cmbCompany.SelectedItem == null ||Convert.ToString(cmbCompany.SelectedValue) == "All")
            {
                radMessage.Show("Please select company");
                return;
            }

            if (cmbVendor.SelectedItem == null)
            {
                radMessage.Show("Please select vendor");
                return;
            }
                        
               TotolINRAmtAdPopup = 0;
               TotolProposedAmtAdPopup = 0;
              TotolINRAmtAgPopup = 0;
              TotolProposedAmtAgPopup = 0;
            cmbNatureofRequestPopUp.SelectedIndex = -1;
            cmbSubVerticalPopUp.SelectedIndex = -1;
            Label7.Text = "0";
            Label9.Text = "0";

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
                string SAPResult = paymentWorkflowModel.SAP_GetPaymentData(cmbVendor.SelectedValue, cmbCompany.SelectedValue, (btnOptions.SelectedValue == "Against" ? true : false));
                if (SAPResult == Constants.SUCCESS) 
                {
                    if (btnOptions.SelectedValue == "Against")
                    {
                        bindinitiatordata();
                        grdInitiator.DataBind();
                        grdInitiator_Advance.Visible = false;
                        grdInitiator.Visible = true;
                    }
                    if (btnOptions.SelectedValue == "Advance")
                    {
                        bindinitiatordata_advane();
                        grdInitiator_Advance.DataBind();
                        grdInitiator_Advance.Visible = true;
                        grdInitiator.Visible = false;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(SAPResult);
                }

                cmbNatureofRequestPopUp.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                cmbNatureofRequestPopUp.DataTextField = "Name";
                cmbNatureofRequestPopUp.DataValueField = "Id";
                cmbNatureofRequestPopUp.DataBind();
                cmbNatureofRequestPopUp.SelectedText = "";

                cmbSubVerticalPopUp.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                cmbSubVerticalPopUp.DataTextField = "SubVerticalName";
                cmbSubVerticalPopUp.DataValueField = "SubVerticalID";
                cmbSubVerticalPopUp.DataBind();
                cmbSubVerticalPopUp.SelectedText = "";

        }

        private void bindinitiatordata()
        {
            string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_PAYMENTINITIATORPOPUP_DATA, cmbVendor.SelectedValue, cmbCompany.SelectedValue, btnOptions.SelectedValue), string.Empty);
            if (result == Constants.REST_CALL_FAILURE)
            {
                grdInitiator.DataSource = new DataTable();
            }
            else
            {
                List<usp_GetPaymentInitiatorPopupData_Result> lstGetCompanyData_Result = JsonConvert.DeserializeObject<List<usp_GetPaymentInitiatorPopupData_Result>>(result);
                grdInitiator.DataSource = lstGetCompanyData_Result;

            }
        }

        private void bindinitiatordata_advane()
        {
            string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_PAYMENTINITIATORPOPUP_DATAADVANCE, cmbVendor.SelectedValue, cmbCompany.SelectedValue, btnOptions.SelectedValue), string.Empty);
            if (result == Constants.REST_CALL_FAILURE)
            {
                grdInitiator_Advance.DataSource = new DataTable();
            }
            else
            {
                List<usp_GetPaymentInitiatorPopupData_Advance_Result> lstGetCompanyDataAdvance_Result = JsonConvert.DeserializeObject<List<usp_GetPaymentInitiatorPopupData_Advance_Result>>(result);
                grdInitiator_Advance.DataSource = lstGetCompanyDataAdvance_Result;

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> checkedItem = new List<string>();
            var result = string.Empty;
            string selPaymentId = string.Empty;
            decimal DebitAmt = 0;
            decimal CreditAmt = 0;

            var BankDetailInprocess = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (Convert.ToBoolean(BankDetailInprocess))
            {

                string msg = "For Vendor " + cmbVendor.SelectedValue.ToString() + " - " + cmbVendor.SelectedItem.Text + " and Company " + cmbCompany.SelectedItem.Text + " bank details updation is in Progress you cannot submit the record for payment.";
                radMessage.Title = "Alert";
                //radMessage.Show(Constants.ISVENDORBANKDETAILUPDATE_INPROCESSMSG);
                radMessage.Show(msg.ToString());
                return;
            }


            if (RadGrid1.SelectedItems.Count == 0)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Please Select at least one record");
                return;
            }
            foreach (GridDataItem itm in RadGrid1.SelectedItems)
            {
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbAdvReqSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbProposed");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRemarks");
                RadTextBox tbWithholdingTaxCode = (RadTextBox)itm.FindControl("tbWithholdingTaxCode");
                RadTextBox tbJustificationforAdvPayment = (RadTextBox)itm.FindControl("tbJustificationforAdvPayment");

                if (!string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text)))
                {                    
                    if (Convert.ToString(itm["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        DebitAmt = DebitAmt + Convert.ToDecimal(tbAmountProposed.Text);
                    else if (Convert.ToString(itm["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        CreditAmt = CreditAmt + Convert.ToDecimal(tbAmountProposed.Text);
                }

            }
            if (DebitAmt >= CreditAmt)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Its a debit balance.Hence could not proceed");
                return;
            }


            foreach (GridDataItem itm in RadGrid1.SelectedItems)
            {
                LinkButton btn = (LinkButton)sender;
                ArrayList values = new ArrayList();
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbAdvReqSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbProposed");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRemarks");
                RadTextBox tbWithholdingTaxCode = (RadTextBox)itm.FindControl("tbWithholdingTaxCode");
                RadTextBox tbJustificationforAdvPayment = (RadTextBox)itm.FindControl("tbJustificationforAdvPayment");
                int SAPAdvancePaymentId = (int)itm.GetDataKeyValue("SAPAdvancePaymentId");
                values.Add(SAPAdvancePaymentId);
                values.Add(cmbSubVertical.SelectedValue);
                values.Add(cmbNatureofRequest.SelectedValue);
                values.Add(tbAmountProposed.Text);
                values.Add(tbRemarks.Text);
                values.Add((btn.CommandArgument == "Submit") ? true : false);
                DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                var rows = SubVertical.Select("SubVerticalID = '" + cmbSubVertical.SelectedValue + "'");
                values.Add((rows.Count() > 0) ? Convert.ToInt32(rows[0]["VerticalID"].ToString()) : 0);
                values.Add(Convert.ToInt32(Session["ProfileId"]));
                values.Add(HttpContext.Current.Session["UserId"]);
                values.Add(tbWithholdingTaxCode.Text);
                values.Add(tbJustificationforAdvPayment.Text);
                string jsonInputParameter = JsonConvert.SerializeObject(values);
                
                 result = commonFunctions.RestServiceCall(Constants.ADVANCE_BILL_TRANSACTIONS, Crypto.Instance.Encrypt(jsonInputParameter));
                          
            }   
            
            
                    
            LinkButton btntype = (LinkButton)sender;
            if (btntype.CommandArgument != "Submit")               
            {
                ArrayList SaveUpdateValues = new ArrayList();
                foreach (GridDataItem itms in RadGrid1.Items)
                {
                    if(itms.Selected)
                        selPaymentId = selPaymentId + "," + Convert.ToString(itms.GetDataKeyValue("SAPAdvancePaymentId"));
                }
                SaveUpdateValues.Add(Convert.ToString(cmbVendor.SelectedValue));
                SaveUpdateValues.Add(Convert.ToString(cmbCompany.SelectedValue));
               
                SaveUpdateValues.Add(selPaymentId);
                string jsonInputParameter = JsonConvert.SerializeObject(SaveUpdateValues);
              string   Saveresult = commonFunctions.RestServiceCall(Constants.ADVANCE_BILL_SAVEUPDATEMODE, Crypto.Instance.Encrypt(jsonInputParameter));                
            }
            if (result == "true")
            {
                radMessage.Title = "Message";
                if ((btntype.CommandArgument == "Submit"))
                    radMessage.Show("Data submitted successfully");
                else
                    radMessage.Show("Saved Successfully");
            }



            BindAdvanceMyRequestGrid();
            RadGrid1.DataBind();
            BindAdvanceInProcessGrid(); 
            RadGrid2.DataBind();

            BindAdvanceMyRequestAllData();
            RadGrid4.DataBind();
        }

        protected string GetVerticals(string subVerticalId, ref int id)
        {
            try
            {
                var result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_GETVERTICAl_BYSUBVERTICAL, subVerticalId), string.Empty);

                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Show("Error in Getting Verticals");
                    id = 0;
                    return "";                    
                }
                else
                {
                    DropdownValues ddlbverticalVals = JsonConvert.DeserializeObject<DropdownValues>(result);
                    if (ddlbverticalVals != null && ddlbverticalVals.Vertical != null && ddlbverticalVals.Vertical.Count > 0)
                    {
                        id = Convert.ToInt32(ddlbverticalVals.Vertical[0].Id);
                        return ddlbverticalVals.Vertical[0].Name.ToString();
                    }
                    else
                    {
                        id = 0;
                        return "";
                    }
                }

            }
            catch (Exception ex)
            {
                radMessage.Show("Error in Getting Verticals");
                CommonFunctions.WriteErrorLog(ex);
                return "";             
            }


        }

        protected void cmbSubVertical_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            RadDropDownList cmboBx = (sender as RadDropDownList);
            GridDataItem dataItem = cmboBx.Parent.Parent as GridDataItem;
            string item = (dataItem["SubVertical"].FindControl("cmbSubVertical") as RadDropDownList).SelectedValue;
            string itemNature = (dataItem["SubVertical"].FindControl("cmbNatureofRequest") as RadDropDownList).SelectedText;
            int Id = 0;
            (dataItem["Vertical"].FindControl("lbVerticalRequest") as Label).Text = GetVerticals(item, ref Id);
            (dataItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
            Session["LastSelectedSubVertical" + dataItem.ItemIndex.ToString()] = item;
            (dataItem["Vertical"].FindControl("Label1") as Label).Text = GetVerticals(item, ref Id);
            Label lblRequestSubVertical = (Label)dataItem.FindControl("lblRequestSubVertical");
            lblRequestSubVertical.Text = (dataItem["SubVertical"].FindControl("cmbSubVertical") as RadDropDownList).SelectedText;
            Session["LastSelectedNatureofRequest" + dataItem.ItemIndex.ToString()] = itemNature;
        }

        protected void cmbAdvReqSubVertical_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            RadDropDownList cmboBx = (sender as RadDropDownList);
            GridDataItem dataItem = cmboBx.Parent.Parent as GridDataItem;
            string item = (dataItem["SubVertical"].FindControl("cmbAdvReqSubVertical") as RadDropDownList).SelectedValue;
            string itemNature = (dataItem["SubVertical"].FindControl("cmbNatureofRequest") as RadDropDownList).SelectedText;
            int Id = 0;
            (dataItem["Vertical"].FindControl("lbVerticalAdvReq") as Label).Text = GetVerticals(item, ref Id);
            (dataItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
            Session["LastSelectedAdvSubVertical" + dataItem.ItemIndex.ToString()] = item;
            (dataItem["Vertical"].FindControl("Label1") as Label).Text = GetVerticals(item, ref Id);
            Label lblRequestSubVertical = (Label)dataItem.FindControl("lblRequestSubVertical");
            lblRequestSubVertical.Text = (dataItem["SubVertical"].FindControl("cmbAdvReqSubVertical") as RadDropDownList).SelectedText;
            Session["LastSelectedAdvNatureofRequest" + dataItem.ItemIndex.ToString()] = itemNature;
        }

        protected void btnAgainstExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (RadTabStrip1.SelectedIndex == 0)
                {
                    gvRequest.MasterTableView.AllowFilteringByColumn = false;
                    gvRequest.MasterTableView.AllowSorting = false;
                    gvRequest.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    gvRequest.ExportSettings.FileName = "My Requests";
                    gvRequest.ExportSettings.ExportOnlyData = true;
                    gvRequest.Rebind();
                    gvRequest.MasterTableView.ExportToExcel();
                }
                if (RadTabStrip1.SelectedIndex == 1)
                {
                    gvIntitiatorInProcess.MasterTableView.AllowFilteringByColumn = false;
                    gvIntitiatorInProcess.MasterTableView.AllowSorting = false;
                    gvIntitiatorInProcess.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    gvIntitiatorInProcess.ExportSettings.FileName = "In Process";
                    gvIntitiatorInProcess.ExportSettings.ExportOnlyData = true;
                    gvIntitiatorInProcess.Rebind();
                    gvIntitiatorInProcess.MasterTableView.ExportToExcel();
                }
                if (RadTabStrip1.SelectedIndex == 2)
                {
                    gvInitiatorNeedCorrection.MasterTableView.AllowFilteringByColumn = false;
                    gvInitiatorNeedCorrection.MasterTableView.AllowSorting = false;
                    gvInitiatorNeedCorrection.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    gvInitiatorNeedCorrection.ExportSettings.FileName = "Need Correction";
                    gvInitiatorNeedCorrection.ExportSettings.ExportOnlyData = true;
                    gvInitiatorNeedCorrection.Rebind();
                    gvInitiatorNeedCorrection.MasterTableView.ExportToExcel();
                }
                if (RadTabStrip1.SelectedIndex == 3)
                {
                    gvAllRequest.MasterTableView.AllowFilteringByColumn = false;
                    gvAllRequest.MasterTableView.AllowSorting = false;
                    gvAllRequest.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    gvAllRequest.ExportSettings.FileName = "Saved Request";
                    gvAllRequest.ExportSettings.ExportOnlyData = true;
                    gvAllRequest.Rebind();
                    gvAllRequest.MasterTableView.ExportToExcel();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gvRequest_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            e.Row.Cells.GetCellByName("PostDate").StyleValue = "myCustomStyle";
            //e.Row.Cells.GetCellByName("RequestNetduedate").StyleValue = "myCustomStyle";            

            if (e.RowType == GridExportExcelMLRowType.HeaderRow)
            {
            //    gvRequest.Rebind();  //workaround to get the template column's content

            //    //create new column element and a header cell
            //    e.Worksheet.Table.Columns.Add(new ColumnElement());
            //    e.Worksheet.Table.Columns.Add(new ColumnElement());
            //    e.Worksheet.Table.Columns.Add(new ColumnElement());
            //    e.Worksheet.Table.Columns.Add(new ColumnElement());
            //    e.Worksheet.Table.Columns.Add(new ColumnElement());
            //    e.Worksheet.Table.Columns.Add(new ColumnElement());
            //    CellElement cell = new CellElement();
            //    CellElement cellVerical = new CellElement();
            //    CellElement cellNature = new CellElement();
            //    CellElement AmtProposed = new CellElement();
            //    CellElement Status = new CellElement();
            //    CellElement Remarks = new CellElement();
            //    //correct the autofilter 
            //    e.Worksheet.AutoFilter.Range = String.Format("R{0}C{1}:R{0}C{2}", 1, 1, e.Worksheet.Table.Columns.Count + 6);

            //    //populate the header cell
            //    cell.Data.DataItem = (gvRequest.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["SubVertical"].Text.Trim();
            //    e.Row.Cells.Add(cell);
            //    cellVerical.Data.DataItem = (gvRequest.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["Vertical"].Text.Trim();
            //    e.Row.Cells.Add(cellVerical);
            //    cellNature.Data.DataItem = (gvRequest.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["NatureOfRequest"].Text.Trim();
            //    e.Row.Cells.Add(cellNature);

            //    AmtProposed.Data.DataItem = (gvRequest.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["AmountProposed"].Text.Trim();
            //    e.Row.Cells.Add(AmtProposed);
            //    Status.Data.DataItem = (gvRequest.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["Status"].Text.Trim();
            //    e.Row.Cells.Add(Status);
            //    Remarks.Data.DataItem = (gvRequest.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["Remarks"].Text.Trim();
            //    e.Row.Cells.Add(Remarks);
            }

            if (e.RowType == GridExportExcelMLRowType.DataRow)
            {               
                ////create cell for the current row
                //CellElement cell = new CellElement();
                //CellElement cellVerical = new CellElement();
                //CellElement cellNature = new CellElement();
                //CellElement AmtProposed = new CellElement();
                //CellElement Status = new CellElement();
                //CellElement Remarks = new CellElement();
                //int currentRow = e.Worksheet.Table.Rows.IndexOf(e.Row) - 1;
                //GridDataItem item = gvRequest.MasterTableView.Items[currentRow];
                ////populate the data cell 
                //DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                //var rows = SubVertical.Select("SubVerticalID = '" + Session["LastSelectedSubVertical" + item.ItemIndex.ToString()] + "'");
                //cell.Data.DataItem = (rows.Count() > 0) ? rows[0]["SubVerticalName"].ToString() : string.Empty;
                //e.Row.Cells.Add(cell);
                //cellVerical.Data.DataItem = (rows.Count() > 0) ? rows[0]["VerticalName"].ToString() : string.Empty;

                //e.Row.Cells.Add(cellVerical);
                //if (Session["LastSelectedNatureofRequest" + item.ItemIndex.ToString()] != null)
                //{
                //    cellNature.Data.DataItem = Session["LastSelectedNatureofRequest" + item.ItemIndex.ToString()].ToString();
                //}
                //else
                //{
                //    cellNature.Data.DataItem = string.Empty;
                //}
                
                //e.Row.Cells.Add(cellNature);
                //if (DataBinder.Eval(item.DataItem, "AmountProposed") != null)
                //AmtProposed.Data.DataItem = (Double)Convert.ToDouble(DataBinder.Eval(item.DataItem, "AmountProposed").ToString());
                //else
                //AmtProposed.Data.DataItem = (Double)Convert.ToDouble(DataBinder.Eval(item.DataItem, "Amount").ToString());
                //e.Row.Cells.Add(AmtProposed);

                //Status.Data.DataItem = "Draft";
                //e.Row.Cells.Add(Status);
                 
               
                //Remarks.Data.DataItem = (DataBinder.Eval(item.DataItem, "Comment") == null) ? string.Empty : DataBinder.Eval(item.DataItem, "Comment").ToString();
                //e.Row.Cells.Add(Remarks);

            }
        }

        protected void gvRequest_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            StyleElement myStyle = new StyleElement("myCustomStyle");
            myStyle.NumberFormat.FormatType = NumberFormatType.MediumDate;         
            e.Styles.Add(myStyle);
        }
        protected void RadGrid1_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            e.Row.Cells.GetCellByName("PostDate").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("ExpectedClearingDate").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("AdvanceDocumentDate").StyleValue = "myCustomStyle";  

            if (e.RowType == GridExportExcelMLRowType.HeaderRow)
            {
                //RadGrid1.Rebind();  //workaround to get the template column's content

                ////create new column element and a header cell
                //e.Worksheet.Table.Columns.Add(new ColumnElement());
                //e.Worksheet.Table.Columns.Add(new ColumnElement());
                //e.Worksheet.Table.Columns.Add(new ColumnElement());
                //e.Worksheet.Table.Columns.Add(new ColumnElement());
                //e.Worksheet.Table.Columns.Add(new ColumnElement());
                //e.Worksheet.Table.Columns.Add(new ColumnElement());
                //e.Worksheet.Table.Columns.Add(new ColumnElement());
                //e.Worksheet.Table.Columns.Add(new ColumnElement());
                //CellElement cell = new CellElement();
                //CellElement cellVerical = new CellElement();
                //CellElement cellNature = new CellElement();
                //CellElement AmtProposed = new CellElement();
                
                //CellElement Remarks = new CellElement();
                //CellElement taxcode = new CellElement();
                //CellElement Justification = new CellElement();
                ////correct the autofilter 
                //e.Worksheet.AutoFilter.Range = String.Format("R{0}C{1}:R{0}C{2}", 1, 1, e.Worksheet.Table.Columns.Count + 6);

                ////populate the header cell
                //cell.Data.DataItem = (RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["SubVertical"].Text.Trim();
                //e.Row.Cells.Add(cell);
                //cellVerical.Data.DataItem = (RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["Vertical"].Text.Trim();
                //e.Row.Cells.Add(cellVerical);
                //cellNature.Data.DataItem = (RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["NatureOfRequest"].Text.Trim();
                //e.Row.Cells.Add(cellNature);

                //AmtProposed.Data.DataItem = (RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["AmountProposed"].Text.Trim();
                //e.Row.Cells.Add(AmtProposed);
               
                //Remarks.Data.DataItem = (RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["Remarks"].Text.Trim();
                //e.Row.Cells.Add(Remarks);

                //taxcode.Data.DataItem = (RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["WithholdingTaxCode"].Text.Trim();
                //e.Row.Cells.Add(taxcode);
                //Justification.Data.DataItem = (RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["JustificationforAdvPayment"].Text.Trim();
                //e.Row.Cells.Add(Justification);
            }

            if (e.RowType == GridExportExcelMLRowType.DataRow)
            {
                //create cell for the current row
                //CellElement cell = new CellElement();
                //CellElement cellVerical = new CellElement();
                //CellElement cellNature = new CellElement();
                //CellElement AmtProposed = new CellElement();
                //CellElement Status = new CellElement();
                //CellElement Remarks = new CellElement();
                //CellElement taxcode = new CellElement();
                //CellElement Justification = new CellElement();
                //int currentRow = e.Worksheet.Table.Rows.IndexOf(e.Row) - 1;
                //GridDataItem item = RadGrid1.MasterTableView.Items[currentRow];
                ////populate the data cell 

                //DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                //var rows = SubVertical.Select("SubVerticalID = '" + Session["LastSelectedAdvSubVertical" + item.ItemIndex.ToString()] + "'");
                //cell.Data.DataItem = (rows.Count() > 0) ? rows[0]["SubVerticalName"].ToString() : string.Empty;
                //e.Row.Cells.Add(cell);
                //cellVerical.Data.DataItem = (rows.Count() > 0) ? rows[0]["VerticalName"].ToString() : string.Empty;

                //e.Row.Cells.Add(cellVerical);
                //if (Session["LastSelectedNatureofRequest" + item.ItemIndex.ToString()] != null)
                //{
                //    cellNature.Data.DataItem = Session["LastSelectedAdvNatureofRequest" + item.ItemIndex.ToString()].ToString();
                //}
                //else
                //{
                //    cellNature.Data.DataItem = string.Empty;
                //}
                   
                //e.Row.Cells.Add(cellNature);


                //if (DataBinder.Eval(item.DataItem, "AmountProposed") != null)
                //    AmtProposed.Data.DataItem = (Double)Convert.ToDouble(DataBinder.Eval(item.DataItem, "AmountProposed").ToString());
                //else
                //    AmtProposed.Data.DataItem = (Double)Convert.ToDouble(DataBinder.Eval(item.DataItem, "Amount").ToString());
                //e.Row.Cells.Add(AmtProposed);
             
                //Remarks.Data.DataItem = (DataBinder.Eval(item.DataItem, "Comment") == null) ? string.Empty : DataBinder.Eval(item.DataItem, "Comment").ToString();

                //e.Row.Cells.Add(Remarks);

                //taxcode.Data.DataItem = DataBinder.Eval(item.DataItem, "WithholdingTaxCode").ToString();
                //e.Row.Cells.Add(taxcode);

                //Justification.Data.DataItem = DataBinder.Eval(item.DataItem, "JustificationforAdvPayment").ToString();
                //e.Row.Cells.Add(Justification);

            }
        }

        protected void gvIntitiatorInProcess_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            
            BindInProcessGrid();
        }      

        protected void gvInitiatorNeedCorrection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
           
                BindAgainstBillCorrectionGrid();
        }

        protected void gvInitiatorNeedCorrection_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            e.Row.Cells.GetCellByName("PostDateNeedCorrection").StyleValue = "myCustomStyle";
            //e.Row.Cells.GetCellByName("NetduedateNeedCorrection").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("SubmitDate").StyleValue = "myCustomStyle";

            if (e.RowType == GridExportExcelMLRowType.HeaderRow)
            {
                gvInitiatorNeedCorrection.Rebind();  //workaround to get the template column's content

                //create new column element and a header cell
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                CellElement cell = new CellElement();
                CellElement cellVerical = new CellElement();
                CellElement cellNature = new CellElement();
                CellElement AmtProposed = new CellElement();
                CellElement Status = new CellElement();
                CellElement Remarks = new CellElement();
                //correct the autofilter 
                e.Worksheet.AutoFilter.Range = String.Format("R{0}C{1}:R{0}C{2}", 1, 1, e.Worksheet.Table.Columns.Count + 6);

                //populate the header cell
                cell.Data.DataItem = (gvInitiatorNeedCorrection.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["SubVerticalNeedCorrection"].Text.Trim();
                e.Row.Cells.Add(cell);
                cellVerical.Data.DataItem = (gvInitiatorNeedCorrection.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["VerticalNeedCorrection"].Text.Trim();
                e.Row.Cells.Add(cellVerical);
                cellNature.Data.DataItem = (gvInitiatorNeedCorrection.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["NatureOfRequestNeedCorrection"].Text.Trim();
                e.Row.Cells.Add(cellNature);

                AmtProposed.Data.DataItem = (gvInitiatorNeedCorrection.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["AmountProposedNeedCorrection"].Text.Trim();
                e.Row.Cells.Add(AmtProposed);
                //Status.Data.DataItem = (gvInitiatorNeedCorrection.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["StatusNeedCorrection"].Text.Trim();
                //e.Row.Cells.Add(Status);
                Remarks.Data.DataItem = (gvInitiatorNeedCorrection.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["RemarksNeedCorrection"].Text.Trim();
                e.Row.Cells.Add(Remarks);
            }

            if (e.RowType == GridExportExcelMLRowType.DataRow)
            {
                //create cell for the current row
                CellElement cell = new CellElement();
                CellElement cellVerical = new CellElement();
                CellElement cellNature = new CellElement();
                CellElement AmtProposed = new CellElement();
                CellElement Status = new CellElement();
                CellElement Remarks = new CellElement();
                int currentRow = e.Worksheet.Table.Rows.IndexOf(e.Row) - 1;
                GridDataItem item = gvInitiatorNeedCorrection.MasterTableView.Items[currentRow];
                //populate the data cell 
                DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                var rows = SubVertical.Select("SubVerticalID = '" + Session["LastSelectedSubVerticalNeedCorrection" + item.ItemIndex.ToString()] + "'");
                cell.Data.DataItem = (rows.Count() > 0) ? rows[0]["SubVerticalName"].ToString() : string.Empty;
                e.Row.Cells.Add(cell);
                cellVerical.Data.DataItem = (rows.Count() > 0) ? rows[0]["VerticalName"].ToString() : string.Empty;

                e.Row.Cells.Add(cellVerical);
                cellNature.Data.DataItem = Session["LastSelectedNatureofRequestNeedCorrection" + item.ItemIndex.ToString()].ToString();
                e.Row.Cells.Add(cellNature);
                if (DataBinder.Eval(item.DataItem, "AmountProposed") != null)
                    AmtProposed.Data.DataItem = (Double)Convert.ToDouble(DataBinder.Eval(item.DataItem, "AmountProposed").ToString());
                else
                    AmtProposed.Data.DataItem = (Double)Convert.ToDouble(DataBinder.Eval(item.DataItem, "Amount").ToString());
                e.Row.Cells.Add(AmtProposed);

                Status.Data.DataItem = "Need Correction";
                e.Row.Cells.Add(Status);

                Remarks.Data.DataItem = (DataBinder.Eval(item.DataItem, "Comment") == null) ? string.Empty : DataBinder.Eval(item.DataItem, "Comment").ToString();
                e.Row.Cells.Add(Remarks);

            }
        }

        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindAdvanceInProcessGrid();
        }

        protected void RadGrid3_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindAdvanceCorrectionGrid();
        }

        protected void RadGrid3_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem headerItem = (GridHeaderItem)e.Item;
                CheckBox chkBox = (CheckBox)headerItem["SelectAdvanceCorrection"].Controls[0];
                chkBox.Text = "Select";
            }
            
            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem editItem = (GridEditableItem)e.Item;
                RadDropDownList cmbSubVertical = (RadDropDownList)editItem.FindControl("cmbAdvReqSubVerticalAdvanceCorrection");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)editItem.FindControl("cmbNatureofRequestAdvanceCorrection");            

                if (cmbNatureofRequest != null)
                {
                    cmbNatureofRequest.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                    cmbNatureofRequest.DataTextField = "Name";
                    cmbNatureofRequest.DataValueField = "Id";
                    cmbNatureofRequest.DataBind();

                    cmbNatureofRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureofRequest").ToString();
                    Label lblRequestNatureofRequest = (Label)editItem.FindControl("lblRequestNatureofRequestAdvanceCorrection");
                    lblRequestNatureofRequest.Text = cmbNatureofRequest.SelectedText;
                    if (Session["LastSelectedAdvNatureofRequestAdvanceCorrection" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedAdvNatureofRequestAdvanceCorrection" + e.Item.ItemIndex.ToString()] = cmbNatureofRequest.SelectedText;
                }

                Label lbVertical = (Label)editItem.FindControl("lbVerticalAdvReqAdvanceCorrection");
                if (lbVertical != null && cmbSubVertical != null)
                    lbVertical.Text = cmbSubVertical.SelectedText;
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)editItem.FindControl("tbProposedAdvanceCorrection");
                if (tbAmountProposed != null)
                    tbAmountProposed.MaxValue = (Double)Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Amount").ToString());

                if (cmbSubVertical != null)
                {
                    cmbSubVertical.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                    cmbSubVertical.DataTextField = "SubVerticalName";
                    cmbSubVertical.DataValueField = "SubVerticalID";
                    cmbSubVertical.DataBind();

                    if ((int)DataBinder.Eval(e.Item.DataItem, "SubVerticalId") != 0)
                    {
                        cmbSubVertical.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "SubVerticalId").ToString();
                    }

                    Label lblRequestSubVertical = (Label)editItem.FindControl("lblRequestSubVerticalAdvanceCorrection");
                    lblRequestSubVertical.Text = cmbSubVertical.SelectedText;
                    string SubVertical = (editItem["SubVerticalAdvanceCorrection"].FindControl("cmbAdvReqSubVerticalAdvanceCorrection") as RadDropDownList).SelectedValue;
                    int Id = 0;
                    (editItem["VerticalAdvanceCorrection"].FindControl("lbVerticalAdvReqAdvanceCorrection") as Label).Text = GetVerticals(SubVertical, ref Id);
                    (editItem["VerticalAdvanceCorrection"].Controls[3] as Label).Text = Id.ToString();
                    Label Label1 = (Label)editItem.FindControl("Label1AdvanceCorrection");
                    int SubId = 0;
                    Label1.Text = GetVerticals(cmbSubVertical.SelectedValue, ref SubId);
                    if (Session["LastSelectedAdvSubVerticalAdvanceCorrection" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedAdvSubVerticalAdvanceCorrection" + e.Item.ItemIndex.ToString()] = cmbSubVertical.SelectedValue;
                }

                HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                commentLink.Attributes["href"] = "javascript:void(0);";
                commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SAPAdvancePaymentId"]);

                if (Convert.ToString(item["DCFLag"].Text) == "S")
                {
                    item["DCFLag"].Text = "Debit";
                }
                if (Convert.ToString(item["DCFLag"].Text) == "H")
                {
                    item["DCFLag"].Text = "Credit";
                }



            }
        }

        protected void RadGrid3_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            e.Row.Cells.GetCellByName("PostDateAdvanceCorrection").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("DocumentDateAdvanceCorrection").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("SubmitDate").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("ExpectedClearingDateAdvanceCorrection").StyleValue = "myCustomStyle";      

            if (e.RowType == GridExportExcelMLRowType.HeaderRow)
            {
                RadGrid3.Rebind();  //workaround to get the template column's content

                //create new column element and a header cell
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                e.Worksheet.Table.Columns.Add(new ColumnElement());
                CellElement cell = new CellElement();
                CellElement cellVerical = new CellElement();
                CellElement cellNature = new CellElement();
                CellElement AmtProposed = new CellElement();

                CellElement Remarks = new CellElement();
                CellElement taxcode = new CellElement();
                CellElement Justification = new CellElement();
                //correct the autofilter 
                e.Worksheet.AutoFilter.Range = String.Format("R{0}C{1}:R{0}C{2}", 1, 1, e.Worksheet.Table.Columns.Count + 6);

                //populate the header cell
                cell.Data.DataItem = (RadGrid3.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["SubVerticalAdvanceCorrection"].Text.Trim();
                e.Row.Cells.Add(cell);
                cellVerical.Data.DataItem = (RadGrid3.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["VerticalAdvanceCorrection"].Text.Trim();
                e.Row.Cells.Add(cellVerical);
                cellNature.Data.DataItem = (RadGrid3.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["NatureOfRequestAdvanceCorrection"].Text.Trim();
                e.Row.Cells.Add(cellNature);

                AmtProposed.Data.DataItem = (RadGrid3.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["AmountProposedAdvanceCorrection"].Text.Trim();
                e.Row.Cells.Add(AmtProposed);

                Remarks.Data.DataItem = (RadGrid3.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["RemarksAdvanceCorrection"].Text.Trim();
                e.Row.Cells.Add(Remarks);

                taxcode.Data.DataItem = (RadGrid3.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["WithholdingTaxCodeAdvanceCorrection"].Text.Trim();
                e.Row.Cells.Add(taxcode);
                Justification.Data.DataItem = (RadGrid3.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem)["JustificationforAdvPaymentAdvanceCorrection"].Text.Trim();
                e.Row.Cells.Add(Justification);
            }

            if (e.RowType == GridExportExcelMLRowType.DataRow)
            {
                //create cell for the current row
                CellElement cell = new CellElement();
                CellElement cellVerical = new CellElement();
                CellElement cellNature = new CellElement();
                CellElement AmtProposed = new CellElement();
                CellElement Status = new CellElement();
                CellElement Remarks = new CellElement();
                CellElement taxcode = new CellElement();
                CellElement Justification = new CellElement();
                int currentRow = e.Worksheet.Table.Rows.IndexOf(e.Row) - 1;
                GridDataItem item = RadGrid3.MasterTableView.Items[currentRow];
                //populate the data cell 

                DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                var rows = SubVertical.Select("SubVerticalID = '" + Session["LastSelectedAdvSubVerticalAdvanceCorrection" + item.ItemIndex.ToString()] + "'");
                cell.Data.DataItem = (rows.Count() > 0) ? rows[0]["SubVerticalName"].ToString() : string.Empty;
                e.Row.Cells.Add(cell);
                cellVerical.Data.DataItem = (rows.Count() > 0) ? rows[0]["VerticalName"].ToString() : string.Empty;

                e.Row.Cells.Add(cellVerical);
                cellNature.Data.DataItem = Session["LastSelectedAdvNatureofRequestAdvanceCorrection" + item.ItemIndex.ToString()].ToString();
                e.Row.Cells.Add(cellNature);


                if (DataBinder.Eval(item.DataItem, "AmountProposed") != null)
                    AmtProposed.Data.DataItem = (Double)Convert.ToDouble(DataBinder.Eval(item.DataItem, "AmountProposed").ToString());
                else
                    AmtProposed.Data.DataItem = (Double)Convert.ToDouble(DataBinder.Eval(item.DataItem, "Amount").ToString());
                e.Row.Cells.Add(AmtProposed);

                Remarks.Data.DataItem = (DataBinder.Eval(item.DataItem, "Comment") == null) ? string.Empty :DataBinder.Eval(item.DataItem, "Comment").ToString();
                e.Row.Cells.Add(Remarks);

                taxcode.Data.DataItem = DataBinder.Eval(item.DataItem, "WithholdingTaxCode").ToString();
                e.Row.Cells.Add(taxcode);

                Justification.Data.DataItem = DataBinder.Eval(item.DataItem, "JustificationforAdvPayment").ToString();
                e.Row.Cells.Add(Justification);

            }
        }

        protected void gvInitiatorNeedCorrection_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            StyleElement myStyle = new StyleElement("myCustomStyle");
            myStyle.NumberFormat.FormatType = NumberFormatType.MediumDate;
            e.Styles.Add(myStyle);
        }

        protected void gvInitiatorNeedCorrection_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem headerItem = (GridHeaderItem)e.Item;
                CheckBox chkBox = (CheckBox)headerItem["RequestNeedCorrectionSelect"].Controls[0];
                chkBox.Text = "Select";                
            }

            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem editItem = (GridEditableItem)e.Item;
                RadDropDownList cmbSubVertical = (RadDropDownList)editItem.FindControl("cmbSubVerticalNeedCorrection");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)editItem.FindControl("cmbNatureofRequestNeedCorrection");
             

                if (cmbNatureofRequest != null)
                {
                    cmbNatureofRequest.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                    cmbNatureofRequest.DataTextField = "Name";
                    cmbNatureofRequest.DataValueField = "Id";
                    cmbNatureofRequest.DataBind();


                    cmbNatureofRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureofRequestId").ToString();
                    Label lblRequestNatureofRequest = (Label)editItem.FindControl("lblRequestNatureofRequestNeedCorrection");
                    lblRequestNatureofRequest.Text = cmbNatureofRequest.SelectedText;
                    if (Session["LastSelectedNatureofRequestNeedCorrection" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedNatureofRequestNeedCorrection" + e.Item.ItemIndex.ToString()] = cmbNatureofRequest.SelectedText;
                }



                Label lbVertical = (Label)editItem.FindControl("lbVerticalRequestNeedCorrection");
                if (lbVertical != null && cmbSubVertical != null)
                    lbVertical.Text = cmbSubVertical.SelectedText;
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)editItem.FindControl("tbAmountProposedNeedCorrection");
                if (tbAmountProposed != null)
                    tbAmountProposed.MaxValue = (Double)Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Amount").ToString());
                if (cmbSubVertical != null)
                {
                    cmbSubVertical.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                    cmbSubVertical.DataTextField = "SubVerticalName";
                    cmbSubVertical.DataValueField = "SubVerticalID";
                    cmbSubVertical.DataBind();

                    if ((int)DataBinder.Eval(e.Item.DataItem, "SubVerticalId") != 0)
                    {
                        cmbSubVertical.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "SubVerticalId").ToString();
                    }

                    Label lblRequestSubVertical = (Label)editItem.FindControl("lblRequestSubVerticalNeedCorrection");
                    lblRequestSubVertical.Text = cmbSubVertical.SelectedText;
                    string SubVertical = (editItem["SubVerticalNeedCorrection"].FindControl("cmbSubVerticalNeedCorrection") as RadDropDownList).SelectedValue;
                    int Id = 0;
                    (editItem["VerticalNeedCorrection"].FindControl("lbVerticalRequestNeedCorrection") as Label).Text = GetVerticals(SubVertical, ref Id);
                    (editItem["VerticalNeedCorrection"].Controls[3] as Label).Text = Id.ToString();
                    Label Label1 = (Label)editItem.FindControl("Label1NeedCorrection");
                    int SubId = 0;
                    Label1.Text = GetVerticals(cmbSubVertical.SelectedValue, ref SubId);
                    if (Session["LastSelectedSubVerticalNeedCorrection" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedSubVerticalNeedCorrection" + e.Item.ItemIndex.ToString()] = cmbSubVertical.SelectedValue;
                }

                HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                commentLink.Attributes["href"] = "javascript:void(0);";
                commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SAPAgainstBillPaymentId"]);

                if (!string.IsNullOrEmpty(Convert.ToString(item["BaseLineDate"].Text)) && Convert.ToString(item["BaseLineDate"].Text) != "&nbsp;")
                {
                    DateTime D1 = Convert.ToDateTime(item["BaseLineDate"].Text);
                    int duedays = 0;
                    duedays = Convert.ToInt32(item["DueDays"].Text);
                    item["RequestNetduedate"].Text = D1.AddDays(duedays).ToShortDateString();
                }
                if (Convert.ToString(item["DCFLag"].Text) == "S")
                {
                    item["DCFLag"].Text = "Debit";
                }
                if (Convert.ToString(item["DCFLag"].Text) == "H")
                {
                    item["DCFLag"].Text = "Credit";
                }


            }
        }

        protected void btnAdvanceExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (RadTabStrip2.SelectedIndex == 0)
                {
                    RadGrid1.MasterTableView.AllowFilteringByColumn = false;
                    RadGrid1.MasterTableView.AllowSorting = false;
                    RadGrid1.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    RadGrid1.ExportSettings.FileName = "My Requests";
                    RadGrid1.ExportSettings.ExportOnlyData = true;
                    RadGrid1.Rebind();
                    RadGrid1.MasterTableView.ExportToExcel();
                }
                if (RadTabStrip2.SelectedIndex == 1)
                {
                    RadGrid2.MasterTableView.AllowFilteringByColumn = false;
                    RadGrid2.MasterTableView.AllowSorting = false;
                    RadGrid2.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    RadGrid2.ExportSettings.FileName = "In Process";
                    RadGrid2.ExportSettings.ExportOnlyData = true;
                    RadGrid2.Rebind();
                    RadGrid2.MasterTableView.ExportToExcel();
                }
                if (RadTabStrip2.SelectedIndex == 2)
                {
                    RadGrid3.MasterTableView.AllowFilteringByColumn = false;
                    RadGrid3.MasterTableView.AllowSorting = false;
                    RadGrid3.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    RadGrid3.ExportSettings.FileName = "Need Correction";
                    RadGrid3.ExportSettings.ExportOnlyData = true;
                    RadGrid3.Rebind();
                    RadGrid3.MasterTableView.ExportToExcel();
                }
                if (RadTabStrip2.SelectedIndex == 3)
                {
                    RadGrid4.MasterTableView.AllowFilteringByColumn = false;
                    RadGrid4.MasterTableView.AllowSorting = false;
                    RadGrid4.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    RadGrid4.ExportSettings.FileName = "Saved Request";
                    RadGrid4.ExportSettings.ExportOnlyData = true;
                    RadGrid4.Rebind();
                    RadGrid4.MasterTableView.ExportToExcel();
                }
            }
            catch (Exception ex)
            {
                radMessage.Show("Error in Exporting Grid");
                CommonFunctions.WriteErrorLog(ex);
            }

        
        }

        protected void lbSubmitAdvancedNeedCorrection_Click(object sender, EventArgs e)
        {
            decimal DebitAmt = 0;
            decimal CreditAmt = 0;
            var BankDetailInprocess = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (Convert.ToBoolean(BankDetailInprocess))
            {
                string msg = "For Vendor " + cmbVendor.SelectedValue.ToString() + " - " + cmbVendor.SelectedItem.Text + " and Company " + cmbCompany.SelectedItem.Text + " bank details updation is in Progress you cannot submit the record for payment.";
                radMessage.Title = "Alert";
                //radMessage.Show(Constants.ISVENDORBANKDETAILUPDATE_INPROCESSMSG);
                radMessage.Show(msg.ToString());
                return;
            }

            LinkButton btn = (LinkButton)sender;
            var result = string.Empty;
            if (RadGrid3.SelectedItems.Count == 0)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Please Select at least one record");
                return;
            }


            foreach (GridDataItem itm in RadGrid3.SelectedItems)
            {
                ArrayList values = new ArrayList();
                // RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposed");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbProposedAdvanceCorrection");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarks");

                if (!string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text)))
                {

                    if (Convert.ToString(itm["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        DebitAmt = DebitAmt + Convert.ToDecimal(tbAmountProposed.Text);
                    else if (Convert.ToString(itm["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        CreditAmt = CreditAmt + Convert.ToDecimal(tbAmountProposed.Text);

                }

            }
            if (DebitAmt >= CreditAmt)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Its a debit balance.Hence could not proceed");
                return;
            }

            foreach (GridDataItem itm in RadGrid3.SelectedItems)
            {              
                ArrayList values = new ArrayList();
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbAdvReqSubVerticalAdvanceCorrection");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequestAdvanceCorrection");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbProposedAdvanceCorrection");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRemarksAdvanceCorrection");
                RadTextBox tbWithholdingTaxCode = (RadTextBox)itm.FindControl("tbWithholdingTaxCodeAdvanceCorrection");
                RadTextBox tbJustificationforAdvPayment = (RadTextBox)itm.FindControl("tbJustificationforAdvPaymentAdvanceCorrection");
                int SAPAdvancePaymentId = (int)itm.GetDataKeyValue("SAPAdvancePaymentId");
                values.Add(SAPAdvancePaymentId);
                values.Add(cmbSubVertical.SelectedValue);
                values.Add(cmbNatureofRequest.SelectedValue);
                values.Add(tbAmountProposed.Text);
                values.Add(tbRemarks.Text);
                values.Add((btn.CommandArgument == "Submit") ? true : false);
                DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                var rows = SubVertical.Select("SubVerticalID = '" + cmbSubVertical.SelectedValue + "'");
                values.Add((rows.Count() > 0) ? Convert.ToInt32(rows[0]["VerticalID"].ToString()) : 0);
                values.Add(Convert.ToInt32(Session["ProfileId"]));
                values.Add(HttpContext.Current.Session["UserId"]);
                values.Add(tbWithholdingTaxCode.Text);
                values.Add(tbJustificationforAdvPayment.Text);
                string jsonInputParameter = JsonConvert.SerializeObject(values);

                 result = commonFunctions.RestServiceCall(Constants.ADVANCE_PAY_CORRECTIONS, Crypto.Instance.Encrypt(jsonInputParameter));                

            }

            if (result == "true")
            {
                radMessage.Title = "Message";
                if ((btn.CommandArgument == "Submit"))
                    radMessage.Show("Data submitted successfully");
                else
                    radMessage.Show("Saved Successfully");
            }




            BindAdvanceCorrectionGrid();
            RadGrid3.DataBind();
        }

        protected void lbSaveBillNeedCorrection_Click(object sender, EventArgs e)
        {
            decimal DebitAmt = 0;
            decimal CreditAmt = 0;
            var BankDetailInprocess = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (Convert.ToBoolean(BankDetailInprocess))
            {
                string msg = "For Vendor " + cmbVendor.SelectedValue.ToString() + " - " + cmbVendor.SelectedItem.Text + " and Company " + cmbCompany.SelectedItem.Text + " bank details updation is in Progress you cannot submit the record for payment.";
                radMessage.Title = "Alert";
                //radMessage.Show(Constants.ISVENDORBANKDETAILUPDATE_INPROCESSMSG);
                radMessage.Show(msg.ToString());
                return;
            }

            LinkButton btn = (LinkButton)sender;
            var result = string.Empty;
            if (gvInitiatorNeedCorrection.SelectedItems.Count == 0)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Please Select at least one record");
                return;
            }
            foreach (GridDataItem itm in gvInitiatorNeedCorrection.SelectedItems)
            {
                ArrayList values = new ArrayList();
                // RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposed");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposedNeedCorrection");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarks");

                if (!string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text)))
                {

                    if (Convert.ToString(itm["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        DebitAmt = DebitAmt + Convert.ToDecimal(tbAmountProposed.Text);
                    else if (Convert.ToString(itm["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        CreditAmt = CreditAmt + Convert.ToDecimal(tbAmountProposed.Text);

                }

            }
            if (DebitAmt >= CreditAmt)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Its a debit balance.Hence could not proceed");
                return;
            }


            foreach (GridDataItem itm in gvInitiatorNeedCorrection.SelectedItems)
            {
                ArrayList values = new ArrayList();
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbSubVerticalNeedCorrection");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequestNeedCorrection");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposedNeedCorrection");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarksNeedCorrection");
                int SAPAgainstBillPaymentId = (int)itm.GetDataKeyValue("SAPAgainstBillPaymentId");
                values.Add(SAPAgainstBillPaymentId);
                values.Add(cmbSubVertical.SelectedValue);
                values.Add(cmbNatureofRequest.SelectedValue);
                values.Add(tbAmountProposed.Text);
                values.Add(tbRemarks.Text);
                values.Add((btn.CommandArgument == "Submit") ? true : false);
                DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                var rows = SubVertical.Select("SubVerticalID = '" + cmbSubVertical.SelectedValue + "'");
                values.Add((rows.Count() > 0) ? Convert.ToInt32(rows[0]["VerticalID"].ToString()) : 0);
                values.Add(Convert.ToInt32(Session["ProfileId"]));
                values.Add(HttpContext.Current.Session["UserId"]);
                string jsonInputParameter = JsonConvert.SerializeObject(values);

                 result = commonFunctions.RestServiceCall(Constants.AGAINST_BILL_CORRECTIONS, Crypto.Instance.Encrypt(jsonInputParameter));
               
            }

            if (result == "true")
            {
                radMessage.Title = "Message";
                if ((btn.CommandArgument == "Submit"))
                    radMessage.Show("Data submitted successfully");
                else
                    radMessage.Show("Saved Successfully");
            }

            BindAgainstBillCorrectionGrid();
            gvInitiatorNeedCorrection.DataBind();


        }

        protected void cmbAdvReqSubVerticalAdvanceCorrection_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            RadDropDownList cmboBx = (sender as RadDropDownList);
            GridDataItem dataItem = cmboBx.Parent.Parent as GridDataItem;
            string item = (dataItem["SubVerticalAdvanceCorrection"].FindControl("cmbAdvReqSubVerticalAdvanceCorrection") as RadDropDownList).SelectedValue;
            string itemNature = (dataItem["SubVerticalAdvanceCorrection"].FindControl("cmbNatureofRequestAdvanceCorrection") as RadDropDownList).SelectedText;
            int Id = 0;
            (dataItem["VerticalAdvanceCorrection"].FindControl("lbVerticalAdvReqAdvanceCorrection") as Label).Text = GetVerticals(item, ref Id);
            (dataItem["VerticalAdvanceCorrection"].Controls[3] as Label).Text = Id.ToString();
            Session["LastSelectedAdvSubVerticalAdvanceCorrection" + dataItem.ItemIndex.ToString()] = item;
            (dataItem["VerticalAdvanceCorrection"].FindControl("Label1AdvanceCorrection") as Label).Text = GetVerticals(item, ref Id);
            Label lblRequestSubVertical = (Label)dataItem.FindControl("lblRequestSubVerticalAdvanceCorrection");
            lblRequestSubVertical.Text = (dataItem["SubVerticalAdvanceCorrection"].FindControl("cmbAdvReqSubVerticalAdvanceCorrection") as RadDropDownList).SelectedText;
            Session["LastSelectedAdvNatureofRequestAdvanceCorrection" + dataItem.ItemIndex.ToString()] = itemNature;
        }

        protected void cmbSubVerticalNeedCorrection_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            RadDropDownList cmboBx = (sender as RadDropDownList);
            GridDataItem dataItem = cmboBx.Parent.Parent as GridDataItem;
            string item = (dataItem["SubVerticalNeedCorrection"].FindControl("cmbSubVerticalNeedCorrection") as RadDropDownList).SelectedValue;
            string itemNature = (dataItem["SubVerticalNeedCorrection"].FindControl("cmbNatureofRequestNeedCorrection") as RadDropDownList).SelectedText;
            int Id = 0;
            (dataItem["VerticalNeedCorrection"].FindControl("lbVerticalRequestNeedCorrection") as Label).Text = GetVerticals(item, ref Id);
            (dataItem["VerticalNeedCorrection"].Controls[3] as Label).Text = Id.ToString();
            Session["LastSelectedSubVerticalNeedCorrection" + dataItem.ItemIndex.ToString()] = item;
            (dataItem["VerticalNeedCorrection"].FindControl("Label1NeedCorrection") as Label).Text = GetVerticals(item, ref Id);
            Label lblRequestSubVertical = (Label)dataItem.FindControl("lblRequestSubVerticalNeedCorrection");
            lblRequestSubVertical.Text = (dataItem["SubVerticalNeedCorrection"].FindControl("cmbSubVerticalNeedCorrection") as RadDropDownList).SelectedText;
            Session["LastSelectedNatureofRequestNeedCorrection" + dataItem.ItemIndex.ToString()] = itemNature;
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            try
            {
                if (e.Argument.Contains("Comment"))
                {

                    string values = e.Argument;
                    string[] parameters = values.Split('#');
                    bindComment(Convert.ToInt32(parameters[1]));
                    grdComment.DataBind();
                }

                if (e.Argument.Contains("Against") || e.Argument.Contains("Advance"))
                {
                    btnOptions_SelectedIndexChanged(sender, e);
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
                var resultLstComments = commonFunctions.RestServiceCall(string.Format(Constants.PAYMENTS_COMMENTS, paymentId, btnOptions.SelectedValue== "Against"? "AgainstBill" : "Advance"), string.Empty);
                if (resultLstComments == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Show("Error Getting Comments");
                    grdComment.DataSource = new DataTable();
                }
                else
                {
                    List<PaymentComments> lstComments  = JsonConvert.DeserializeObject<List<PaymentComments>>(resultLstComments);
                    grdComment.DataSource = lstComments;
                }
                                
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdComment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["PaymentId"])))
                bindComment(Convert.ToInt32(ViewState["PaymentId"]));
        }

        protected void gvIntitiatorInProcess_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                commentLink.Attributes["href"] = "javascript:void(0);";
                commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SAPAgainstBillPaymentId"]);
                GridDataItem item = e.Item as GridDataItem;

                if (!string.IsNullOrEmpty(Convert.ToString(item["BaseLineDate"].Text)) && Convert.ToString(item["BaseLineDate"].Text) != "&nbsp;")
                {
                    DateTime D1 = Convert.ToDateTime(item["BaseLineDate"].Text);
                    int duedays = 0;
                    duedays = Convert.ToInt32(item["DueDays"].Text);
                    item["RequestNetduedate"].Text = D1.AddDays(duedays).ToShortDateString();
                }


            }
        }

        protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                commentLink.Attributes["href"] = "javascript:void(0);";
                commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SAPAdvancePaymentId"]);

            }
        }

        protected void gvRequest_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','update','" + Convert.ToString(Id) + "','true','true','true','false','PaymentInitiator');");
            }
        }

        protected void gvIntitiatorInProcess_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','update','" + Convert.ToString(Id) + "','false','false','true','false','PaymentInitiator');");
            }
        }

        protected void gvInitiatorNeedCorrection_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','update','" + Convert.ToString(Id) + "','true','false','true','false','PaymentInitiator');");
            }
        }

        protected void grdWorkFlowInProcess_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindRequestInProcess();
        }

        protected void RadGrid3_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','update','" + Convert.ToString(Id) + "','true','true','true','false','PaymentInitiator');");
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','update','" + Convert.ToString(Id) + "','true','true','true','false','PaymentInitiator');");
            }
        }

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','update','" + Convert.ToString(Id) + "','false','false','true','false','PaymentInitiator');");
            }
        }

        protected void gvIntitiatorInProcess_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            StyleElement myStyle = new StyleElement("myCustomStyle");
            myStyle.NumberFormat.FormatType = NumberFormatType.MediumDate;       
            e.Styles.Add(myStyle);
        }

  

        protected void RadGrid1_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            StyleElement myStyle = new StyleElement("myCustomStyle");
            myStyle.NumberFormat.FormatType = NumberFormatType.MediumDate;
            e.Styles.Add(myStyle);
        }

        protected void RadGrid2_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            StyleElement myStyle = new StyleElement("myCustomStyle");
            myStyle.NumberFormat.FormatType = NumberFormatType.MediumDate;
            e.Styles.Add(myStyle);
        }

        protected void RadGrid3_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            StyleElement myStyle = new StyleElement("myCustomStyle");
            myStyle.NumberFormat.FormatType = NumberFormatType.MediumDate;
            e.Styles.Add(myStyle);
        }

        protected void RadGrid2_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            e.Row.Cells.GetCellByName("InprogressDocumentDate").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("PostDate").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("ExpectedClearingDate").StyleValue = "myCustomStyle";       
            e.Row.Cells.GetCellByName("SubmitDate").StyleValue = "myCustomStyle";            
        }

        protected void gvIntitiatorInProcess_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {       
            //e.Row.Cells.GetCellByName("Netduedate").StyleValue = "myCustomStyle";
            e.Row.Cells.GetCellByName("SubmitDate").StyleValue = "myCustomStyle";       
            
        }

        protected void btnSearchVendor_Click(object sender, EventArgs e)
        {
         string  result = commonFunctions.RestServiceCall(string.Format(Constants.Company_SEARCHVENDORS,Convert.ToString(cmbCompany.SelectedValue) , Convert.ToString(txtVendorSearch.Text)), string.Empty);

            if (result == Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            }
            else
            {
                List<VendorSearch> lstVendorSearch = JsonConvert.DeserializeObject<List<VendorSearch>>(result);
                grdVendorSearch.DataSource = lstVendorSearch;
                grdVendorSearch.DataBind();
            }
        }

        protected void grdVendorSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string result = commonFunctions.RestServiceCall(string.Format(Constants.Company_SEARCHVENDORS, Convert.ToString(cmbCompany.SelectedValue), Convert.ToString(txtVendorSearch.Text)), string.Empty);

            if (result == Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            }
            else
            {
                List<VendorSearch> lstVendorSearch = JsonConvert.DeserializeObject<List<VendorSearch>>(result);
                grdVendorSearch.DataSource = lstVendorSearch;              
            }
        }

        protected void grdVendorSearch_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridDataItem item = (GridDataItem)e.Item;
                cmbVendor.Items.Clear();
                RadComboBoxItem vendorsel = new RadComboBoxItem(Convert.ToString(item["VendorName"].Text), Convert.ToString(item.GetDataKeyValue("VendorCode")));
                cmbVendor.Items.Add(vendorsel);
                cmbVendor.DataBind();
                cmbVendor.SelectedValue =Convert.ToString(item.GetDataKeyValue("VendorCode"));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ClosePopup();", true);
            }
        }

        protected void grdInitiator_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem headerItem = (GridHeaderItem)e.Item;
                CheckBox chkBox = (CheckBox)headerItem["RequestSelect"].Controls[0];
                chkBox.Text = "Select";
            }

            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem editItem = (GridEditableItem)e.Item;
                RadDropDownList cmbSubVertical = (RadDropDownList)editItem.FindControl("cmbSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)editItem.FindControl("cmbNatureofRequest");
                RadTextBox tbRequestRemarks = (RadTextBox)editItem.FindControl("tbRequestRemarks");


                if (cmbNatureofRequest != null)
                {
                    cmbNatureofRequest.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                    cmbNatureofRequest.DataTextField = "Name";
                    cmbNatureofRequest.DataValueField = "Id";
                    cmbNatureofRequest.DataBind();

                    //if (DataBinder.Eval(e.Item.DataItem, "NatureofRequestId") != null)
                    //{
                    //    cmbNatureofRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureofRequestId").ToString();
                    //    Label lblRequestNatureofRequest = (Label)editItem.FindControl("lblRequestNatureofRequest");
                    //    lblRequestNatureofRequest.Text = cmbNatureofRequest.SelectedText;
                    //    if (Session["LastSelectedNatureofRequest" + e.Item.ItemIndex.ToString()] == null)
                    //        Session["LastSelectedNatureofRequest" + e.Item.ItemIndex.ToString()] = cmbNatureofRequest.SelectedText;
                    //}
                }



                Label lbVertical = (Label)editItem.FindControl("lbVerticalRequest");
                if (lbVertical != null && cmbSubVertical != null)
                    lbVertical.Text = cmbSubVertical.SelectedText;
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)editItem.FindControl("tbAmountProposed");
                if (tbAmountProposed != null)
                    tbAmountProposed.MaxValue = (Double)Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Amount").ToString());
                if (cmbSubVertical != null)
                {
                    cmbSubVertical.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                    cmbSubVertical.DataTextField = "SubVerticalName";
                    cmbSubVertical.DataValueField = "SubVerticalID";
                    cmbSubVertical.DataBind();

                    //if ((int)DataBinder.Eval(e.Item.DataItem, "SubVerticalId") != 0)
                    //{
                    //    cmbSubVertical.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "SubVerticalId").ToString();
                    //}

                    //Label lblRequestSubVertical = (Label)editItem.FindControl("lblRequestSubVertical");
                    //lblRequestSubVertical.Text = cmbSubVertical.SelectedText;
                    //string SubVertical = (editItem["SubVertical"].FindControl("cmbSubVertical") as RadDropDownList).SelectedValue;
                    //int Id = 0;
                    //(editItem["Vertical"].FindControl("lbVerticalRequest") as Label).Text = GetVerticals(SubVertical, ref Id);
                    //(editItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
                    //Label Label1 = (Label)editItem.FindControl("Label1");
                    //int SubId = 0;
                    //Label1.Text = GetVerticals(cmbSubVertical.SelectedValue, ref SubId);
                    //if (Session["LastSelectedSubVertical" + e.Item.ItemIndex.ToString()] == null)
                    //    Session["LastSelectedSubVertical" + e.Item.ItemIndex.ToString()] = cmbSubVertical.SelectedValue;
                }

                //if (Convert.ToInt32(item.GetDataKeyValue("SaveMode")) == 1)
                //{
                //    item.Selected = true;
                //}
                if (!string.IsNullOrEmpty(Convert.ToString(item["BaseLineDate"].Text)) && Convert.ToString(item["BaseLineDate"].Text) != "&nbsp;")
                {
                    DateTime D1 = Convert.ToDateTime(item["BaseLineDate"].Text);
                    int duedays = 0;
                    duedays = Convert.ToInt32(item["DueDays"].Text);
                    item["RequestNetduedate"].Text = D1.AddDays(duedays).ToShortDateString();
                }
                //if (!string.IsNullOrEmpty(Convert.ToString(item["Amount"].Text)))
                //{
                //    TotolINRAmtAgPopup = TotolINRAmtAgPopup + Convert.ToDecimal(Convert.ToString(item["Amount"].Text));
                //}
                Label7.Text = TotolINRAmtAgPopup.ToString();
                Label9.Text = TotolINRAmtAgPopup.ToString();

                if(Convert.ToString(item["DCFLag"].Text)=="S")
                {
                    item["DCFLag"].Text = "Debit";
                }
                if (Convert.ToString(item["DCFLag"].Text) == "H")
                {
                    item["DCFLag"].Text = "Credit";
                }
            }
        }

        protected void lnkSubmitInitiator_Click(object sender, EventArgs e)
        {
            decimal DebitAmt = 0;
            decimal CreditAmt = 0;
            int msgCount = 0;
            string msg = string.Empty;
            var BankDetailInprocess = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (Convert.ToBoolean(BankDetailInprocess))
            {
                 msg = "For Vendor " + cmbVendor.SelectedValue.ToString() + " - " + cmbVendor.SelectedItem.Text + " and Company " + cmbCompany.SelectedItem.Text + " bank details updation is in Progress you cannot submit the record for payment.";
                radMessage.Title = "Alert";            
                radMessage.Show(msg.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }

            if (btnOptions.SelectedValue == "Against")
            {
                if (grdInitiator.SelectedItems.Count == 0)
                {
                    radMessage.Title = "Alert";
                    radMessage.Show("Please Select at least one record");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
                
                foreach (GridDataItem itm in grdInitiator.SelectedItems)
                {
                    int innerCount = 0;
                    
                    RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbSubVertical");
                    RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                    RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposed");
                    RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarks");

                    if (!string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text)))
                    {
                        if (Convert.ToString(itm["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                            DebitAmt = DebitAmt + Convert.ToDecimal(tbAmountProposed.Text);
                        else if (Convert.ToString(itm["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                            CreditAmt = CreditAmt + Convert.ToDecimal(tbAmountProposed.Text);
                    }


                    if (cmbSubVertical.SelectedValue == null || cmbSubVertical.SelectedValue == "")
                    {

                        innerCount = innerCount + 1;
                        msg = msg + "Please enter values for Document Number:- " + Convert.ToString(itm["DPRNumber"].Text);
                        msg = msg + " Subvertical,";

                        msgCount = msgCount + 1;
                        
                    }
                    if (cmbNatureofRequest.SelectedValue == null || cmbNatureofRequest.SelectedValue == "")
                    {
                        if(innerCount==0)
                            msg = msg + "Please enter values for Document Number:- " + Convert.ToString(itm["DPRNumber"].Text);

                        msg = msg + " Nature Of Request,";
                        msgCount = msgCount + 1;
                        innerCount = innerCount + 1;
                    }
                    if (Convert.ToString(tbAmountProposed.Text.Trim())== null || Convert.ToString(tbAmountProposed.Text.Trim()) == "")
                    {
                        if (innerCount == 0)
                            msg = msg + "Please enter  values for Document Number:- " + Convert.ToString(itm["DPRNumber"].Text);

                        msg = msg + " Amount Proposed,";
                        msgCount = msgCount + 1;
                        innerCount = innerCount + 1;
                    }
                    //if (Convert.ToString(tbRemarks.Text.Trim()) == null || Convert.ToString(tbRemarks.Text.Trim()) == "")
                    //{
                    //    if (innerCount == 0)
                    //        msg = msg + "Please enter values for Document Number:- " + Convert.ToString(itm["DPRNumber"].Text);

                    //    msg = msg + " Remark,";
                    //    msgCount = msgCount + 1;
                    //    innerCount = innerCount + 1;
                    //}
                    if (innerCount > 0)
                        msg = msg + "</br>";                   
                }
                if (DebitAmt >= CreditAmt)
                {
                    radMessage.Title = "Alert";
                    radMessage.Show("Its a debit balance.Hence could not proceed");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
                if (msgCount > 0)
                {
                    radMessage.Title = "Alert";
                    radMessage.Show(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }

                submitInitiatorDataAgainst();
            }
            if (btnOptions.SelectedValue == "Advance")
            {
                if (grdInitiator_Advance.SelectedItems.Count == 0)
                {
                    radMessage.Title = "Alert";
                    radMessage.Show("Please Select at least one record");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }

                foreach (GridDataItem itm in grdInitiator_Advance.SelectedItems)
                {
                    int innerCount = 0;
                    RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbAdvReqSubVertical");
                    RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                    RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbProposed");
                    RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRemarks");
                    RadTextBox txtWithholdingTaxCode = (RadTextBox)itm.FindControl("tbWithholdingTaxCode");
                    RadTextBox txtJustificationforAdvPayment = (RadTextBox)itm.FindControl("tbJustificationforAdvPayment");


                    if (!string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text)))
                    {
                        if (Convert.ToString(itm["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                            DebitAmt = DebitAmt + Convert.ToDecimal(tbAmountProposed.Text);
                        else if (Convert.ToString(itm["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                            CreditAmt = CreditAmt + Convert.ToDecimal(tbAmountProposed.Text);
                    }


                    if (cmbSubVertical.SelectedValue == null || cmbSubVertical.SelectedValue == "")
                    {
                        msg = msg + "Please enter values for DPR Number:- " + Convert.ToString(itm["DPRNumber"].Text);
                        msg = msg + " Subvertical,";
                        msgCount = msgCount + 1;
                        innerCount = innerCount + 1;
                    }
                    if (cmbNatureofRequest.SelectedValue == null || cmbNatureofRequest.SelectedValue == "")
                    {
                        if (innerCount == 0)
                            msg = msg + "Please enter values for DPR Number:- " + Convert.ToString(itm["DPRNumber"].Text);

                        msg = msg + " Nature Of Request,";
                        msgCount = msgCount + 1;
                        innerCount = innerCount + 1;
                    }
                    if (Convert.ToString(tbAmountProposed.Text.Trim()) == null || Convert.ToString(tbAmountProposed.Text.Trim()) == "")
                    {
                        if (innerCount == 0)
                            msg = msg + "Please enter values for DPR Number:- " + Convert.ToString(itm["DPRNumber"].Text);

                        msg = msg + " Amount Proposed,";
                        msgCount = msgCount + 1;
                        innerCount = innerCount + 1;
                    }
                    //if (Convert.ToString(tbRemarks.Text.Trim()) == null || Convert.ToString(tbRemarks.Text.Trim()) == "")
                    //{
                    //    if (innerCount == 0)
                    //        msg = msg + "Please enter values for DPR Number:- " + Convert.ToString(itm["DPRNumber"].Text);

                    //    msg = msg + " Remark,";
                    //    msgCount = msgCount + 1;
                    //    innerCount = innerCount + 1;
                    //}
                    //if (Convert.ToString(txtWithholdingTaxCode.Text.Trim()) == null || Convert.ToString(txtWithholdingTaxCode.Text.Trim()) == "")
                    //{
                    //    if (innerCount == 0)
                    //        msg = msg + "Please enter values for DPR Number:- " + Convert.ToString(itm["DPRNumber"].Text);

                    //    msg = msg + " WithHolding Tax Code,";
                    //    msgCount = msgCount + 1;
                    //    innerCount = innerCount + 1;
                    //}
                    if (Convert.ToString(txtJustificationforAdvPayment.Text.Trim()) == null || Convert.ToString(txtJustificationforAdvPayment.Text.Trim()) == "")
                    {
                        if (innerCount == 0)
                            msg = msg + "Please enter values for DPR Number:- " + Convert.ToString(itm["DPRNumber"].Text);

                        msg = msg + "Justification for Adv Payment";
                        msgCount = msgCount + 1;
                        innerCount = innerCount + 1;

                    }
                    if(innerCount>0)
                    msg = msg + "</br>";

                }
                if (DebitAmt >= CreditAmt)
                {
                    radMessage.Title = "Alert";
                    radMessage.Show("Its a debit balance.Hence could not proceed");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
                if (msgCount > 0)
                {
                    radMessage.Title = "Alert";
                    radMessage.Show(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
    

                submitInitiatorDataAdvance();
            }
        }


        private void submitInitiatorDataAgainst()
        {
            var result = string.Empty;
           
           
            foreach (GridDataItem itm in grdInitiator.SelectedItems)
            {
                try
                {
                    ArrayList values = new ArrayList();
                    RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbSubVertical");
                    RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                    RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposed");
                    RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarks");

                    ArrayList AgainstBillEntry = new ArrayList();
                    AgainstBillEntry.Add(Convert.ToString(itm["CompanyCode"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["VendorCode"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["VendorName"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(cmbSubVertical.SelectedValue);
                    AgainstBillEntry.Add(Convert.ToString(itm["DPRNumber"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["Reference"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["FiscalYear"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["PostDate"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["Amount"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(tbAmountProposed.Text);
                    AgainstBillEntry.Add(Convert.ToString(itm["Currency"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["DueDays"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["BaseLineDate"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["BusinessArea"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["ProfitCentre"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["DocumentDate"].Text.Replace("&nbsp;", null)));
                    AgainstBillEntry.Add(Convert.ToString(itm["PaymentMethod"].Text).Replace("&nbsp;",null));
                    AgainstBillEntry.Add(HttpContext.Current.Session["UserId"]);
                    AgainstBillEntry.Add(FileUploadList(Convert.ToString(itm["DPRNumber"].Text.Replace("&nbsp;", null))));

                    if(Convert.ToString(itm["DCFLag"].Text)=="Debit")
                    AgainstBillEntry.Add("S");
                    if (Convert.ToString(itm["DCFLag"].Text) == "Credit")
                     AgainstBillEntry.Add("H");
                    if(string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text).Replace("&nbsp;", null)))
                      AgainstBillEntry.Add(null);


                    string jsonInputParameterSAP = JsonConvert.SerializeObject(AgainstBillEntry);
                    result = commonFunctions.RestServiceCall(Constants.AGAINST_BILL_SAVETOSAP, Crypto.Instance.Encrypt(jsonInputParameterSAP));

                    if (result != Constants.REST_CALL_FAILURE)
                    {
                        SAPAgainstBillPayment objsap = JsonConvert.DeserializeObject<SAPAgainstBillPayment>(result);
                        values.Add(objsap.SAPAgainstBillPaymentId);
                        values.Add(cmbSubVertical.SelectedValue);
                        values.Add(cmbNatureofRequest.SelectedValue);
                        values.Add(tbAmountProposed.Text);
                        values.Add(tbRemarks.Text);
                        values.Add(false);
                        DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                        var rows = SubVertical.Select("SubVerticalID = '" + cmbSubVertical.SelectedValue + "'");
                        values.Add((rows.Count() > 0) ? Convert.ToInt32(rows[0]["VerticalID"].ToString()) : 0);
                        values.Add(Convert.ToInt32(Session["ProfileId"]));
                        values.Add(HttpContext.Current.Session["UserId"]);
                        string jsonInputParameter = JsonConvert.SerializeObject(values);
                        result = commonFunctions.RestServiceCall(Constants.AGAINST_BILL_TRANSACTIONS, Crypto.Instance.Encrypt(jsonInputParameter));
                       
                           
                        
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                }
            }
            Session["ATTACHMENT"] = null;
            GetDataForSearch();
            gvRequest.DataBind();
            grdInitiator.DataSource = new DataTable();
            grdInitiator.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ClosePopupInitiator();", true);
        }

        private void submitInitiatorDataAdvance()
        {
            var result = string.Empty;


            foreach (GridDataItem itm in grdInitiator_Advance.SelectedItems)
            {
                try
                {
                    ArrayList values = new ArrayList();
                    RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbAdvReqSubVertical");
                    RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                    RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbProposed");
                    RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRemarks");
                    RadTextBox txtWithholdingTaxCode = (RadTextBox)itm.FindControl("tbWithholdingTaxCode");
                    RadTextBox txtJustificationforAdvPayment = (RadTextBox)itm.FindControl("tbJustificationforAdvPayment");
                    

                    ArrayList AdvanceBillEntry = new ArrayList();
                    AdvanceBillEntry.Add(Convert.ToString(itm["CompanyCode"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["VendorCode"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["VendorName"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(cmbNatureofRequest.SelectedValue);
                    AdvanceBillEntry.Add(Convert.ToString(itm["DPRNumber"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(itm["Reference"].Text.Replace("&nbsp;", ""));
                    AdvanceBillEntry.Add(Convert.ToString(itm["FiscalYear"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["PostDate"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["Amount"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(tbAmountProposed.Text);
                    AdvanceBillEntry.Add(Convert.ToString(itm["Currency"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["ExpectedClearingDate"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["POLineItemNo"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["BusinessArea"].Text.Replace("&nbsp;", null)));                    
                    AdvanceBillEntry.Add(Convert.ToString(itm["ProfitCentre"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["PurchasingDocument"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["PaymentMethod"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["POItemText"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["SpecialGL"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(txtWithholdingTaxCode.Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["UnsettledOpenAdvance"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(txtJustificationforAdvPayment.Text));                   
                    AdvanceBillEntry.Add(Convert.ToString(itm["AdvanceDocumentDate"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(Convert.ToString(itm["HouseBank"].Text.Replace("&nbsp;", null)));
                    AdvanceBillEntry.Add(HttpContext.Current.Session["UserId"]);
                    AdvanceBillEntry.Add(FileUploadList(Convert.ToString(itm["DPRNumber"].Text)));
                    //AdvanceBillEntry.Add(Convert.ToString(itm["DCFLag"].Text));
                    if (Convert.ToString(itm["DCFLag"].Text) == "Debit")
                        AdvanceBillEntry.Add("S");
                    if (Convert.ToString(itm["DCFLag"].Text) == "Credit")
                        AdvanceBillEntry.Add("H");
                    if (string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text).Replace("&nbsp;", null)))
                        AdvanceBillEntry.Add(null);

                    if (string.IsNullOrEmpty(Convert.ToString(itm["TaxRate"].Text).Replace("&nbsp;", null)))
                        AdvanceBillEntry.Add(null);
                    else
                        AdvanceBillEntry.Add(Convert.ToDecimal(itm["TaxRate"].Text));

                    if (string.IsNullOrEmpty(Convert.ToString(itm["TaxAmount"].Text).Replace("&nbsp;", null)))
                        AdvanceBillEntry.Add(null);
                    else
                        AdvanceBillEntry.Add(Convert.ToDecimal(itm["TaxAmount"].Text));

                    AdvanceBillEntry.Add(Convert.ToDecimal(itm["GrossAmount"].Text.Replace("&nbsp;", null)));

                    string jsonInputParameterSAP = JsonConvert.SerializeObject(AdvanceBillEntry);
                    result = commonFunctions.RestServiceCall(Constants.ADVANCE_BILL_SAVETOSAP, Crypto.Instance.Encrypt(jsonInputParameterSAP));

                    if (result != Constants.REST_CALL_FAILURE)
                    {
                        SAPAdvancePayment objsap = JsonConvert.DeserializeObject<SAPAdvancePayment>(result);

                        values.Add(objsap.SAPAdvancePaymentId);
                        values.Add(cmbSubVertical.SelectedValue);
                        values.Add(cmbNatureofRequest.SelectedValue);
                        values.Add(tbAmountProposed.Text);
                        values.Add(tbRemarks.Text);
                        values.Add(false);
                        DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                        var rows = SubVertical.Select("SubVerticalID = '" + cmbSubVertical.SelectedValue + "'");
                        values.Add((rows.Count() > 0) ? Convert.ToInt32(rows[0]["VerticalID"].ToString()) : 0);
                        values.Add(Convert.ToInt32(Session["ProfileId"]));
                        values.Add(HttpContext.Current.Session["UserId"]);
                        values.Add(txtWithholdingTaxCode.Text);
                        values.Add(txtJustificationforAdvPayment.Text);
                        string jsonInputParameter = JsonConvert.SerializeObject(values);

                        result = commonFunctions.RestServiceCall(Constants.ADVANCE_BILL_TRANSACTIONS, Crypto.Instance.Encrypt(jsonInputParameter));

                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                }
            }
            Session["ATTACHMENT"] = null;
            BindAdvanceMyRequestGrid();
            RadGrid1.DataBind();
            BindAdvanceMyRequestAllData();
            RadGrid4.DataBind();
            grdInitiator_Advance.DataSource = new DataTable();
            grdInitiator_Advance.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ClosePopupInitiator();", true);
        }
        protected void grdInitiator_Advance_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem headerItem = (GridHeaderItem)e.Item;
                CheckBox chkBox = (CheckBox)headerItem["Select"].Controls[0];
                chkBox.Text = "Select";
            }

            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem editItem = (GridEditableItem)e.Item;
                RadDropDownList cmbSubVertical = (RadDropDownList)editItem.FindControl("cmbAdvReqSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)editItem.FindControl("cmbNatureofRequest");


                if (cmbNatureofRequest != null)
                {
                    cmbNatureofRequest.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                    cmbNatureofRequest.DataTextField = "Name";
                    cmbNatureofRequest.DataValueField = "Id";
                    cmbNatureofRequest.DataBind();

                    //if (DataBinder.Eval(e.Item.DataItem, "NatureofRequest") != null)
                    //{
                    //    cmbNatureofRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureofRequest").ToString();
                    //    Label lblRequestNatureofRequest = (Label)editItem.FindControl("lblRequestNatureofRequest");
                    //    lblRequestNatureofRequest.Text = cmbNatureofRequest.SelectedText;
                    //    if (Session["LastSelectedAdvNatureofRequest" + e.Item.ItemIndex.ToString()] == null)
                    //        Session["LastSelectedAdvNatureofRequest" + e.Item.ItemIndex.ToString()] = cmbNatureofRequest.SelectedText;
                    //}
                }

                Label lbVertical = (Label)editItem.FindControl("lbVerticalAdvReq");
                if (lbVertical != null && cmbSubVertical != null)
                    lbVertical.Text = cmbSubVertical.SelectedText;
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)editItem.FindControl("tbProposed");
                if (tbAmountProposed != null)
                    tbAmountProposed.MaxValue = (Double)Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Amount").ToString());

                if (cmbSubVertical != null)
                {
                    cmbSubVertical.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                    cmbSubVertical.DataTextField = "SubVerticalName";
                    cmbSubVertical.DataValueField = "SubVerticalID";
                    cmbSubVertical.DataBind();

                    //if ((int)DataBinder.Eval(e.Item.DataItem, "SubVerticalId") != 0)
                    //{
                    //    cmbSubVertical.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "SubVerticalId").ToString();
                    //}                    
                    //Label lblRequestSubVertical = (Label)editItem.FindControl("lblRequestSubVertical");
                    //lblRequestSubVertical.Text = cmbSubVertical.SelectedText;

                    string SubVertical = (editItem["SubVertical"].FindControl("cmbAdvReqSubVertical") as RadDropDownList).SelectedValue;
                    int Id = 0;
                    if (cmbSubVertical.SelectedValue != "")
                        (editItem["Vertical"].FindControl("lbVerticalAdvReq") as Label).Text = GetVerticals(SubVertical, ref Id);
                    else
                        (editItem["Vertical"].FindControl("lbVerticalAdvReq") as Label).Text = "";
                    (editItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
                    Label Label1 = (Label)editItem.FindControl("Label1");
                    int SubId = 0;
                    if (cmbSubVertical.SelectedValue != "")
                        Label1.Text = GetVerticals(cmbSubVertical.SelectedValue, ref SubId);
                    else
                        Label1.Text = "";

                    if (Session["LastSelectedAdvSubVertical" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedAdvSubVertical" + e.Item.ItemIndex.ToString()] = cmbSubVertical.SelectedValue;
                }
                //if (!string.IsNullOrEmpty(Convert.ToString(item["Amount"].Text)))
                //{
                //    TotolINRAmtAdPopup = TotolINRAmtAdPopup + Convert.ToDecimal(Convert.ToString(item["Amount"].Text));
                //}
                Label7.Text = TotolINRAmtAdPopup.ToString();
                Label9.Text = TotolINRAmtAdPopup.ToString();
                if (Convert.ToString(item["DCFLag"].Text) == "S")
                {
                    item["DCFLag"].Text = "Debit";
                }
                if (Convert.ToString(item["DCFLag"].Text) == "H")
                {
                    item["DCFLag"].Text = "Credit";
                }

            }
        }

        protected void grdInitiator_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (cmbCompany.SelectedValue != "" && cmbVendor.SelectedValue != "")
                bindinitiatordata();
        }

        protected void grdInitiator_Advance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (cmbCompany.SelectedValue != "" && cmbVendor.SelectedValue != "")
                bindinitiatordata_advane();
        }

        protected void grdInitiator_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();

                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','insert','" + Convert.ToString(Id) + "','true','true','true','true','PaymentInitiatorPopUp');");
            }
        }

        protected List<FileUploadModel> FileUploadList(string dprNumber)
        {
            try
            {
                DataTable dt = null;
                DataTable dt1 = (DataTable)Session["ATTACHMENT"];
                if (dt1 != null)
                    dt = dt1.Select("FileuploadId=" + dprNumber).CopyToDataTable();

                if (dt == null) return null;
                List<FileUploadModel> fileUpload = new List<FileUploadModel>();
                fileUpload = (from DataRow row in dt.Rows
                              select new FileUploadModel
                              {
                                  FileUploadId = Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(row["FileuploadId"])) ? 0 : row["FileuploadId"]),
                                  EntityId = Convert.ToInt32(0),
                                  DisplayName = Convert.ToString(row["DisplayName"]),
                                  FileName = Convert.ToString(row["FileName"]),
                                  EntityName = "PaymentInitiator"
                              }).ToList();
                return fileUpload;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return null;
            }
        }

        protected void grdInitiator_Advance_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();

                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','insert','" + Convert.ToString(Id) + "','true','true','true','true','PaymentInitiatorPopUp');");
            }
        }

        protected void cmbNatureofRequestPopUp_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (cmbNatureofRequestPopUp.SelectedValue != null)
            {
                if (btnOptions.SelectedValue == "Against")
                {
                    foreach (GridDataItem item in grdInitiator.Items)
                    {
                        RadDropDownList drpNatureOfRequest = (RadDropDownList)item.FindControl("cmbNatureofRequest");
                        if (item.Selected)
                        {
                           
                            if (drpNatureOfRequest != null)
                                drpNatureOfRequest.SelectedValue = cmbNatureofRequestPopUp.SelectedValue;
                        }
                        else
                        {
                            //drpNatureOfRequest.SelectedValue="";
                            drpNatureOfRequest.SelectedIndex = -1;
                        }
                    }

                }
                if (btnOptions.SelectedValue == "Advance")
                {
                    foreach (GridDataItem item in grdInitiator_Advance.Items)
                    {
                        RadDropDownList drpNatureOfRequest = (RadDropDownList)item.FindControl("cmbNatureofRequest");
                        if (item.Selected)
                        {
                            if (drpNatureOfRequest != null)
                                drpNatureOfRequest.SelectedValue = cmbNatureofRequestPopUp.SelectedValue;
                        }
                        else
                        {
                            //drpNatureOfRequest.SelectedValue = string.Empty;
                            drpNatureOfRequest.SelectedIndex = -1;
                        }
                    }
                }
            }
        }

        protected void gvAllRequest_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {

        }

        protected void gvAllRequest_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem headerItem = (GridHeaderItem)e.Item;
                CheckBox chkBox = (CheckBox)headerItem["RequestSelect"].Controls[0];
                chkBox.Text = "Select";
            }

            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem editItem = (GridEditableItem)e.Item;
                RadDropDownList cmbSubVertical = (RadDropDownList)editItem.FindControl("cmbSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)editItem.FindControl("cmbNatureofRequest");
                RadTextBox tbRequestRemarks = (RadTextBox)editItem.FindControl("tbRequestRemarks");


                if (cmbNatureofRequest != null)
                {
                    cmbNatureofRequest.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                    cmbNatureofRequest.DataTextField = "Name";
                    cmbNatureofRequest.DataValueField = "Id";
                    cmbNatureofRequest.DataBind();

                    if (DataBinder.Eval(e.Item.DataItem, "NatureofRequestId") != null)
                    {
                        cmbNatureofRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureofRequestId").ToString();
                        Label lblRequestNatureofRequest = (Label)editItem.FindControl("lblRequestNatureofRequest");
                        lblRequestNatureofRequest.Text = cmbNatureofRequest.SelectedText;

                        editItem["NatureOfRequest1"].Text = lblRequestNatureofRequest.Text;

                        if (Session["LastSelectedNatureofRequest" + e.Item.ItemIndex.ToString()] == null)
                            Session["LastSelectedNatureofRequest" + e.Item.ItemIndex.ToString()] = cmbNatureofRequest.SelectedText;
                    }
                }



                Label lbVertical = (Label)editItem.FindControl("lbVerticalRequest");
                if (lbVertical != null && cmbSubVertical != null)
                    lbVertical.Text = cmbSubVertical.SelectedText;
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)editItem.FindControl("tbAmountProposed");
                if (tbAmountProposed != null)
                    tbAmountProposed.MaxValue = (Double)Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Amount").ToString());
                if (cmbSubVertical != null)
                {
                    cmbSubVertical.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                    cmbSubVertical.DataTextField = "SubVerticalName";
                    cmbSubVertical.DataValueField = "SubVerticalID";
                    cmbSubVertical.DataBind();

                    if ((int)DataBinder.Eval(e.Item.DataItem, "SubVerticalId") != 0)
                    {
                        cmbSubVertical.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "SubVerticalId").ToString();
                    }

                    Label lblRequestSubVertical = (Label)editItem.FindControl("lblRequestSubVertical");
                    lblRequestSubVertical.Text = cmbSubVertical.SelectedText;
                    editItem["SubVertical1"].Text = lblRequestSubVertical.Text;

                    string SubVertical = (editItem["SubVertical"].FindControl("cmbSubVertical") as RadDropDownList).SelectedValue;
                    int Id = 0;
                    (editItem["Vertical"].FindControl("lbVerticalRequest") as Label).Text = GetVerticals(SubVertical, ref Id);
                    (editItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
                    Label Label1 = (Label)editItem.FindControl("Label1");
                    int SubId = 0;
                    Label1.Text = GetVerticals(cmbSubVertical.SelectedValue, ref SubId);
                    if (Session["LastSelectedSubVertical" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedSubVertical" + e.Item.ItemIndex.ToString()] = cmbSubVertical.SelectedValue;
                }

                if (Convert.ToInt32(item.GetDataKeyValue("SaveMode")) == 1)
                {
                    item.Selected = true;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(item["BaseLineDate"].Text)) && Convert.ToString(item["BaseLineDate"].Text) != "&nbsp;")
                {
                    DateTime D1 = Convert.ToDateTime(item["BaseLineDate"].Text);
                    int duedays = 0;
                    duedays = Convert.ToInt32(item["DueDays"].Text);
                    item["RequestNetduedate"].Text = D1.AddDays(duedays).ToShortDateString();
                }

                //if (!string.IsNullOrEmpty(Convert.ToString(item["Amount"].Text)))
                //{
                //    TotolINRAmtAgainst = TotolINRAmtAgainst + Convert.ToDecimal(Convert.ToString(item["Amount"].Text));
                //}
                //lblINRAmt.Text = TotolINRAmtAgainst.ToString();
                //if (!string.IsNullOrEmpty(Convert.ToString(item["AmountProposed1"].Text)))
                //{
                //    TotolProposedAmtAgainst = TotolProposedAmtAgainst + Convert.ToDecimal(Convert.ToString(item["AmountProposed1"].Text));
                //}
                //lblProposedAmt.Text = TotolProposedAmtAgainst.ToString();
                if (Convert.ToString(item["DCFLag"].Text) == "S")
                {
                    item["DCFLag"].Text = "Debit";
                }
                if (Convert.ToString(item["DCFLag"].Text) == "H")
                {
                    item["DCFLag"].Text = "Credit";
                }


            }

        }

        protected void gvAllRequest_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','update','" + Convert.ToString(Id) + "','true','true','true','false','PaymentInitiator');");
            }
        }

        protected void gvAllRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindGridMyRequestAlldata();           
        }

        protected void gvAllRequest_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {

        }

        protected void SaveAllTab_Click(object sender, EventArgs e)
        {
            decimal DebitAmt = 0;
            decimal CreditAmt = 0;
            var BankDetailInprocess = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (Convert.ToBoolean(BankDetailInprocess))
            {
                string msg = "For Vendor " + cmbVendor.SelectedValue.ToString() + " - " + cmbVendor.SelectedItem.Text + " and Company " + cmbCompany.SelectedItem.Text + " bank details updation is in Progress you cannot submit the record for payment.";
                radMessage.Title = "Alert";
                //radMessage.Show(Constants.ISVENDORBANKDETAILUPDATE_INPROCESSMSG);
                radMessage.Show(msg.ToString());
                return;
            }

            LinkButton btn = (LinkButton)sender;
            var result = string.Empty;
            if (gvAllRequest.SelectedItems.Count == 0)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Please Select at least one record");
                return;
            }

            foreach (GridDataItem itm in gvAllRequest.SelectedItems)
            {
                ArrayList values = new ArrayList();
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposed");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarks");

                if (!string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text)))
                {                   
                    if (Convert.ToString(itm["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        DebitAmt = DebitAmt + Convert.ToDecimal(tbAmountProposed.Text);
                    else if (Convert.ToString(itm["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        CreditAmt = CreditAmt + Convert.ToDecimal(tbAmountProposed.Text);
                }

            }
            if (DebitAmt >= CreditAmt)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Its a debit balance.Hence could not proceed");
                return;
            }

            foreach (GridDataItem itm in gvAllRequest.SelectedItems)
            {
                ArrayList values = new ArrayList();
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbAmountProposed");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRequestRemarks");
                int SAPAgainstBillPaymentId = (int)itm.GetDataKeyValue("SAPAgainstBillPaymentId");
                values.Add(SAPAgainstBillPaymentId);
                values.Add(cmbSubVertical.SelectedValue);
                values.Add(cmbNatureofRequest.SelectedValue);
                values.Add(tbAmountProposed.Text);
                values.Add(tbRemarks.Text);
                values.Add((btn.CommandArgument == "Submit") ? true : false);
                DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                var rows = SubVertical.Select("SubVerticalID = '" + cmbSubVertical.SelectedValue + "'");
                values.Add((rows.Count() > 0) ? Convert.ToInt32(rows[0]["VerticalID"].ToString()) : 0);
                values.Add(Convert.ToInt32(Session["ProfileId"]));
                values.Add(HttpContext.Current.Session["UserId"]);
                string jsonInputParameter = JsonConvert.SerializeObject(values);
                result = commonFunctions.RestServiceCall(Constants.AGAINST_BILL_TRANSACTIONS, Crypto.Instance.Encrypt(jsonInputParameter));

            }



            string selPaymentId = string.Empty;
            if (btn.CommandArgument != "Submit")
            {
                ArrayList SaveUpdateValues = new ArrayList();
                foreach (GridDataItem itms in gvAllRequest.Items)
                {
                    if (itms.Selected)
                        selPaymentId = selPaymentId + "," + Convert.ToString(itms.GetDataKeyValue("SAPAgainstBillPaymentId"));
                }
                SaveUpdateValues.Add(Convert.ToString(cmbVendor.SelectedValue));
                SaveUpdateValues.Add(Convert.ToString(cmbCompany.SelectedValue));

                SaveUpdateValues.Add(selPaymentId);
                string jsonInputParameter = JsonConvert.SerializeObject(SaveUpdateValues);
                string Saveresult = commonFunctions.RestServiceCall(Constants.AGAINST_BILL_SAVEUPDATEMODE, Crypto.Instance.Encrypt(jsonInputParameter));
            }

            if (result == "true")
            {
                radMessage.Title = "Message";
                if ((btn.CommandArgument == "Submit"))
                    radMessage.Show("Data submitted successfully");
                else
                    radMessage.Show("Saved Successfully");
            }

            bindMyRequestGrid();
            gvRequest.DataBind();
            BindInProcessGrid();
            gvIntitiatorInProcess.DataBind();
            bindGridMyRequestAlldata();
            gvAllRequest.DataBind();
        }

        protected void RadGrid4_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {

        }

        protected void RadGrid4_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindAdvanceMyRequestAllData();
        }

        protected void RadGrid4_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {

        }

        protected void RadGrid4_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem headerItem = (GridHeaderItem)e.Item;
                CheckBox chkBox = (CheckBox)headerItem["Select"].Controls[0];
                chkBox.Text = "Select";
            }

            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem editItem = (GridEditableItem)e.Item;
                RadDropDownList cmbSubVertical = (RadDropDownList)editItem.FindControl("cmbAdvReqSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)editItem.FindControl("cmbNatureofRequest");


                if (cmbNatureofRequest != null)
                {
                    cmbNatureofRequest.DataSource = (List<Models.ListItem>)Session["Initiatornature-of-request"];
                    cmbNatureofRequest.DataTextField = "Name";
                    cmbNatureofRequest.DataValueField = "Id";
                    cmbNatureofRequest.DataBind();

                    if (DataBinder.Eval(e.Item.DataItem, "NatureofRequest") != null)
                    {
                        cmbNatureofRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureofRequest").ToString();
                        Label lblRequestNatureofRequest = (Label)editItem.FindControl("lblRequestNatureofRequest");
                        lblRequestNatureofRequest.Text = cmbNatureofRequest.SelectedText;
                        editItem["NatureOfRequest1"].Text = lblRequestNatureofRequest.Text;
                        if (Session["LastSelectedAdvNatureofRequest" + e.Item.ItemIndex.ToString()] == null)
                            Session["LastSelectedAdvNatureofRequest" + e.Item.ItemIndex.ToString()] = cmbNatureofRequest.SelectedText;
                    }
                }

                Label lbVertical = (Label)editItem.FindControl("lbVerticalAdvReq");
                if (lbVertical != null && cmbSubVertical != null)
                    lbVertical.Text = cmbSubVertical.SelectedText;
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)editItem.FindControl("tbProposed");
                if (tbAmountProposed != null)
                    tbAmountProposed.MaxValue = (Double)Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Amount").ToString());

                if (cmbSubVertical != null)
                {
                    cmbSubVertical.DataSource = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                    cmbSubVertical.DataTextField = "SubVerticalName";
                    cmbSubVertical.DataValueField = "SubVerticalID";
                    cmbSubVertical.DataBind();

                    if ((int)DataBinder.Eval(e.Item.DataItem, "SubVerticalId") != 0)
                    {
                        cmbSubVertical.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "SubVerticalId").ToString();
                    }


                    Label lblRequestSubVertical = (Label)editItem.FindControl("lblRequestSubVertical");
                    lblRequestSubVertical.Text = cmbSubVertical.SelectedText;
                    editItem["SubVertical1"].Text = lblRequestSubVertical.Text;

                    string SubVertical = (editItem["SubVertical"].FindControl("cmbAdvReqSubVertical") as RadDropDownList).SelectedValue;
                    int Id = 0;
                    (editItem["Vertical"].FindControl("lbVerticalAdvReq") as Label).Text = GetVerticals(SubVertical, ref Id);
                    (editItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
                    Label Label1 = (Label)editItem.FindControl("Label1");
                    int SubId = 0;
                    Label1.Text = GetVerticals(cmbSubVertical.SelectedValue, ref SubId);
                    if (Session["LastSelectedAdvSubVertical" + e.Item.ItemIndex.ToString()] == null)
                        Session["LastSelectedAdvSubVertical" + e.Item.ItemIndex.ToString()] = cmbSubVertical.SelectedValue;
                }


                //if (!string.IsNullOrEmpty(Convert.ToString(Session["itemsChecked"])))
                //{
                //    List<string> checkedItem = new List<string>();
                //    checkedItem = (List<string>)Session["itemsChecked"];
                //    if (checkedItem.Count > 0)
                //    {
                if (Convert.ToInt32(item.GetDataKeyValue("SaveMode")) == 1)
                {
                    item.Selected = true;
                }


                //if (!string.IsNullOrEmpty(Convert.ToString(item["Amount"].Text)))
                //{
                //    TotolINRAmtAdvance = TotolINRAmtAdvance + Convert.ToDecimal(Convert.ToString(item["Amount"].Text));
                //}
                //Label3.Text = TotolINRAmtAdvance.ToString();
                //if (!string.IsNullOrEmpty(Convert.ToString(item["AmountProposed1"].Text)))
                //{
                //    TotolProposedAmtAdvance = TotolProposedAmtAdvance + Convert.ToDecimal(Convert.ToString(item["AmountProposed1"].Text));
                //}
                //Label5.Text = TotolProposedAmtAdvance.ToString();



                if (Convert.ToString(item["DCFLag"].Text) == "S")
                {
                    item["DCFLag"].Text = "Debit";
                }
                if (Convert.ToString(item["DCFLag"].Text) == "H")
                {
                    item["DCFLag"].Text = "Credit";
                }


            }

        }

        protected void RadGrid4_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VendorPaymentInit','update','" + Convert.ToString(Id) + "','true','true','true','false','PaymentInitiator');");
            }

        }

        protected void lnkSaveAllAdvTab_Click(object sender, EventArgs e)
        {
            List<string> checkedItem = new List<string>();
            var result = string.Empty;
            string selPaymentId = string.Empty;
            decimal DebitAmt = 0;
            decimal CreditAmt = 0;

            var BankDetailInprocess = commonFunctions.RestServiceCall(string.Format(Constants.ISVENDORBANKDETAILUPDATE_INPROCESS, cmbVendor.SelectedValue, cmbCompany.SelectedValue), string.Empty);
            if (Convert.ToBoolean(BankDetailInprocess))
            {

                string msg = "For Vendor " + cmbVendor.SelectedValue.ToString() + " - " + cmbVendor.SelectedItem.Text + " and Company " + cmbCompany.SelectedItem.Text + " bank details updation is in Progress you cannot submit the record for payment.";
                radMessage.Title = "Alert";
                //radMessage.Show(Constants.ISVENDORBANKDETAILUPDATE_INPROCESSMSG);
                radMessage.Show(msg.ToString());
                return;
            }


            if (RadGrid4.SelectedItems.Count == 0)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Please Select at least one record");
                return;
            }
            foreach (GridDataItem itm in RadGrid4.SelectedItems)
            {
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbAdvReqSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbProposed");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRemarks");
                RadTextBox tbWithholdingTaxCode = (RadTextBox)itm.FindControl("tbWithholdingTaxCode");
                RadTextBox tbJustificationforAdvPayment = (RadTextBox)itm.FindControl("tbJustificationforAdvPayment");

                if (!string.IsNullOrEmpty(Convert.ToString(itm["DCFLag"].Text)))
                {          
                    if (Convert.ToString(itm["DCFLag"].Text) == "Debit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        DebitAmt = DebitAmt + Convert.ToDecimal(tbAmountProposed.Text);
                    else if (Convert.ToString(itm["DCFLag"].Text) == "Credit" && (!string.IsNullOrEmpty(Convert.ToString(tbAmountProposed.Text))))
                        CreditAmt = CreditAmt + Convert.ToDecimal(tbAmountProposed.Text);
                }

            }
            if (DebitAmt >= CreditAmt)
            {
                radMessage.Title = "Alert";
                radMessage.Show("Its a debit balance.Hence could not proceed");
                return;
            }


            foreach (GridDataItem itm in RadGrid4.SelectedItems)
            {
                LinkButton btn = (LinkButton)sender;
                ArrayList values = new ArrayList();
                RadDropDownList cmbSubVertical = (RadDropDownList)itm.FindControl("cmbAdvReqSubVertical");
                RadDropDownList cmbNatureofRequest = (RadDropDownList)itm.FindControl("cmbNatureofRequest");
                RadNumericTextBox tbAmountProposed = (RadNumericTextBox)itm.FindControl("tbProposed");
                RadTextBox tbRemarks = (RadTextBox)itm.FindControl("tbRemarks");
                RadTextBox tbWithholdingTaxCode = (RadTextBox)itm.FindControl("tbWithholdingTaxCode");
                RadTextBox tbJustificationforAdvPayment = (RadTextBox)itm.FindControl("tbJustificationforAdvPayment");
                int SAPAdvancePaymentId = (int)itm.GetDataKeyValue("SAPAdvancePaymentId");
                values.Add(SAPAdvancePaymentId);
                values.Add(cmbSubVertical.SelectedValue);
                values.Add(cmbNatureofRequest.SelectedValue);
                values.Add(tbAmountProposed.Text);
                values.Add(tbRemarks.Text);
                values.Add((btn.CommandArgument == "Submit") ? true : false);
                DataTable SubVertical = (DataTable)Session["InitiatorSubVerticalAndVertical"];
                var rows = SubVertical.Select("SubVerticalID = '" + cmbSubVertical.SelectedValue + "'");
                values.Add((rows.Count() > 0) ? Convert.ToInt32(rows[0]["VerticalID"].ToString()) : 0);
                values.Add(Convert.ToInt32(Session["ProfileId"]));
                values.Add(HttpContext.Current.Session["UserId"]);
                values.Add(tbWithholdingTaxCode.Text);
                values.Add(tbJustificationforAdvPayment.Text);
                string jsonInputParameter = JsonConvert.SerializeObject(values);

                result = commonFunctions.RestServiceCall(Constants.ADVANCE_BILL_TRANSACTIONS, Crypto.Instance.Encrypt(jsonInputParameter));

            }



            LinkButton btntype = (LinkButton)sender;
            if (btntype.CommandArgument != "Submit")
            {
                ArrayList SaveUpdateValues = new ArrayList();
                foreach (GridDataItem itms in RadGrid4.Items)
                {
                    if (itms.Selected)
                        selPaymentId = selPaymentId + "," + Convert.ToString(itms.GetDataKeyValue("SAPAdvancePaymentId"));
                }
                SaveUpdateValues.Add(Convert.ToString(cmbVendor.SelectedValue));
                SaveUpdateValues.Add(Convert.ToString(cmbCompany.SelectedValue));

                SaveUpdateValues.Add(selPaymentId);
                string jsonInputParameter = JsonConvert.SerializeObject(SaveUpdateValues);
                string Saveresult = commonFunctions.RestServiceCall(Constants.ADVANCE_BILL_SAVEUPDATEMODE, Crypto.Instance.Encrypt(jsonInputParameter));
            }
            if (result == "true")
            {
                radMessage.Title = "Message";
                if ((btntype.CommandArgument == "Submit"))
                    radMessage.Show("Data submitted successfully");
                else
                    radMessage.Show("Saved Successfully");
            }


            BindAdvanceMyRequestAllData();
            RadGrid4.DataBind();
            BindAdvanceMyRequestGrid();
            RadGrid1.DataBind();
            BindAdvanceInProcessGrid();
            RadGrid2.DataBind();

            BindAdvanceMyRequestAllData();
            RadGrid4.DataBind();

        }

        protected void cmbSubVerticalPopUp_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (cmbSubVerticalPopUp.SelectedValue != null)
            {
                if (btnOptions.SelectedValue == "Against")
                {
                    foreach (GridDataItem item in grdInitiator.Items)
                    {

                        RadDropDownList drpSubVertical = (RadDropDownList)item.FindControl("cmbSubVertical");
                        if (item.Selected)
                        {
                            if (drpSubVertical != null)
                                drpSubVertical.SelectedValue = cmbSubVerticalPopUp.SelectedValue;
                        }
                        else
                        {
                            drpSubVertical.SelectedIndex = -1;
                        }
                    }

                }
                if (btnOptions.SelectedValue == "Advance")
                {
                    foreach (GridDataItem dataItem in grdInitiator_Advance.Items)
                    {
                        RadDropDownList drpSubVertical = (RadDropDownList)dataItem.FindControl("cmbAdvReqSubVertical");

                        if (dataItem.Selected)
                        {
                            if (drpSubVertical != null)
                            {
                                drpSubVertical.SelectedValue = cmbSubVerticalPopUp.SelectedValue;

                                // GridDataItem dataItem = cmboBx.Parent.Parent as GridDataItem;
                                string item = (dataItem["SubVertical"].FindControl("cmbAdvReqSubVertical") as RadDropDownList).SelectedValue;
                                string itemNature = (dataItem["SubVertical"].FindControl("cmbNatureofRequest") as RadDropDownList).SelectedText;
                                int Id = 0;
                                (dataItem["Vertical"].FindControl("lbVerticalAdvReq") as Label).Text = GetVerticals(item, ref Id);
                                (dataItem["Vertical"].Controls[3] as Label).Text = Id.ToString();
                                // Session["LastSelectedAdvSubVertical" + dataItem.ItemIndex.ToString()] = item;
                                (dataItem["Vertical"].FindControl("Label1") as Label).Text = GetVerticals(item, ref Id);
                                Label lblRequestSubVertical = (Label)dataItem.FindControl("lblRequestSubVertical");
                                lblRequestSubVertical.Text = (dataItem["SubVertical"].FindControl("cmbAdvReqSubVertical") as RadDropDownList).SelectedText;
                                // Session["LastSelectedAdvNatureofRequest" + dataItem.ItemIndex.ToString()] = itemNature;

                            }
                        }
                        else
                        {
                            drpSubVertical.SelectedIndex = -1;
                        }
                    }
                }
            }
        }

        protected void btnRequestUnderPorcess_Click(object sender, EventArgs e)
        {
            bindRequestInProcess();
            grdWorkFlowInProcess.DataBind();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openRequestUnderProcessPopup()", true);
        }
    }

}








