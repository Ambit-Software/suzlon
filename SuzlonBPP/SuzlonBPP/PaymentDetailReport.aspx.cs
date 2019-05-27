using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class PaymentDetailReport : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        PaymentWorkflowModel objPayment = new PaymentWorkflowModel();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindDropdownData();
                dtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                dtToDate.SelectedDate = DateTime.Now;
            }
        }
        private void BindDropdownData()
        {
            try
            {
                string drpname = "subvertical,company";
                string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);                
                drpSubVerticals.DataSource = ddValues.SubVertical;
                drpSubVerticals.DataTextField = "Name";
                drpSubVerticals.DataValueField = "Id";
                drpSubVerticals.DataBind();
                DropDownListItem li = new DropDownListItem("All", "");
                drpSubVerticals.Items.Insert(0, li);

                drpCompanyCode.DataSource = ddValues.Company;
                drpCompanyCode.DataTextField = "Name";
                drpCompanyCode.DataValueField = "Id";
                drpCompanyCode.DataBind();
                DropDownListItem li1 = new DropDownListItem("All", "");
                drpCompanyCode.Items.Insert(0, li1);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }


        protected void drpCompanyCode_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            //if (drpCompanyCode.SelectedValue != "")
            //{
                DropdownValues ddValues = objPayment.GetVendorCodeForReport(drpCompanyCode.SelectedValue);
                drpVendor.DataSource = ddValues.VendorCode;
                drpVendor.DataTextField = "Name";
                drpVendor.DataValueField = "Id";
                drpVendor.DataBind();
                drpVendor.SelectedText = "All";
                drpVendor.SelectedIndex = -1;
            //}
            //else
            //{
            //    drpVendor.SelectedText = "All";
            //    drpVendor.SelectedIndex = -1;
            //}
        }
        

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpBillType.SelectedValue == "")
                {
                    radMessage.Title = "Alert";
                    radMessage.Show("Please select bill type");
                    return;
                }
                if (drpBillType.SelectedValue == "Against")
                {
                    grdPaymentReportAgainst.Visible = true;
                    grdPaymentReportAgainst.DataSource = GetReportDataAgainst();
                    grdPaymentReportAgainst.DataBind();
                    grdPaymentReportAdvance.Visible = false;
                }
                if (drpBillType.SelectedValue == "Advance")
                {
                    grdPaymentReportAdvance.Visible = true;
                    grdPaymentReportAdvance.DataSource = GetReportDataAdvance();
                    grdPaymentReportAdvance.DataBind();
                    grdPaymentReportAgainst.Visible = false;

                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpBillType.SelectedValue == "Against")
                {
                    grdPaymentReportAgainst.AllowFilteringByColumn = false;
                    grdPaymentReportAgainst.Rebind();
                    grdPaymentReportAgainst.ExportToExcel();
                }
                if (drpBillType.SelectedValue == "Advance")
                {
                    grdPaymentReportAdvance.AllowFilteringByColumn = false;
                    grdPaymentReportAdvance.Rebind();
                    grdPaymentReportAdvance.ExportToExcel();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        
        protected void grdPaymentReportAgainst_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if(drpBillType.SelectedValue=="Against")
                grdPaymentReportAgainst.DataSource = GetReportDataAgainst();
               
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private List<Usp_PaymentDetailReportData_Result> GetReportDataAgainst()
        {
              string subverticalId = drpSubVerticals.SelectedValue != string.Empty ? Convert.ToString(drpSubVerticals.SelectedValue) :"All";
            string CompCode = drpCompanyCode.SelectedValue != string.Empty ? Convert.ToString(drpCompanyCode.SelectedValue) : "All";
            string VendorCode = drpVendor.SelectedValue != string.Empty ? Convert.ToString(drpVendor.SelectedValue) : "All";
            DateTime FromDate = dtFromDate.SelectedDate.Value;
            DateTime ToDate = dtToDate.SelectedDate.Value;
            return objPayment.GetPaymentReportDataAgainst(subverticalId, CompCode, VendorCode, FromDate, ToDate,"");
        }

        private List<Usp_PaymentDetailAdvanceReportData_Result> GetReportDataAdvance()
        {
            string subverticalId = drpSubVerticals.SelectedValue != string.Empty ? Convert.ToString(drpSubVerticals.SelectedValue) : "All";
            string CompCode = drpCompanyCode.SelectedValue != string.Empty ? Convert.ToString(drpCompanyCode.SelectedValue) : "All";
            string VendorCode = drpVendor.SelectedValue != string.Empty ? Convert.ToString(drpVendor.SelectedValue) : "All";
            DateTime FromDate = dtFromDate.SelectedDate.Value;
            DateTime ToDate = dtToDate.SelectedDate.Value;
            return objPayment.GetPaymentReportDataAdvance(subverticalId, CompCode, VendorCode, FromDate, ToDate, "");
        }
        protected void grdPaymentReportAdvance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (drpBillType.SelectedValue == "Advance")
                grdPaymentReportAgainst.DataSource = GetReportDataAdvance();
        }
    }
}