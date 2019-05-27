using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class BankDetailsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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

        protected void grdBankDetailsReport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdBankDetailsReport.DataSource = BankWorkflowModel.GetBankDetailsReportData("All", "All", "All", "All");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpCompanyCode_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            try
            {
                PaymentWorkflowModel objPayment = new PaymentWorkflowModel();
                DropdownValues ddValues = objPayment.GetVendorCodeForReport(drpCompanyCode.SelectedValue);
                drpVendors.DataSource = ddValues.VendorCode;
                drpVendors.DataTextField = "Name";
                drpVendors.DataValueField = "Id";
                drpVendors.DataBind();
                drpVendors.SelectedText = "All";
                drpVendors.SelectedIndex = -1;

                ddValues = objPayment.GetVendorNameForReport(drpCompanyCode.SelectedValue);
                drpVendorName.DataSource = ddValues.VendorName;
                drpVendorName.DataTextField = "Name";
                drpVendorName.DataValueField = "Id";
                drpVendorName.DataBind();
                drpVendorName.SelectedText = "All";
                drpVendorName.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }


        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                string companyCode = drpCompanyCode.SelectedValue != string.Empty ? drpCompanyCode.SelectedValue.ToString() : "All";
                string vendorCode = drpVendors.SelectedValue != string.Empty ? drpVendors.SelectedValue.ToString() : "All";
                string vendorName = drpVendorName.SelectedValue != string.Empty ? drpVendorName.SelectedValue.ToString() : "All";
                string initiator = drpInitiator.SelectedValue != string.Empty ? drpInitiator.SelectedValue.ToString() : "All";

                grdBankDetailsReport.DataSource = BankWorkflowModel.GetBankDetailsReportData(companyCode, vendorCode, vendorName, initiator);
                grdBankDetailsReport.DataBind();
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
                grdBankDetailsReport.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpVendors_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpVendorName_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            try
            {
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
                CommonFunctions commonFunctions = new CommonFunctions();
                string drpname = "company,users";
                string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                drpCompanyCode.DataSource = ddValues.Company;
                drpCompanyCode.DataTextField = "Name";
                drpCompanyCode.DataValueField = "Id";
                drpCompanyCode.DataBind();
                DropDownListItem li1 = new DropDownListItem("All", "");
                drpCompanyCode.Items.Insert(0, li1);

                drpInitiator.DataSource = ddValues.Users;
                drpInitiator.DataTextField = "Name";
                drpInitiator.DataValueField = "Id";
                drpInitiator.DataBind();
                DropDownListItem li2 = new DropDownListItem("All", "");
                drpInitiator.Items.Insert(0, li2);

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}