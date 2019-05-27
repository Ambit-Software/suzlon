using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuzlonBPP
{
    public partial class TreasuryPaymentReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                dtToDate.SelectedDate = DateTime.Now;
                BindVerticalDropdown();
                BindPaymentLotNoDropdown();
                BindCompanyCodeDropdown();
            }
        }

        protected void drpVerticals_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            try
            {
                BindTreasuryNoDropdown();
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
                grdTreasuryReport.DataSource = GetReportData();
                grdTreasuryReport.DataBind();
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
                grdTreasuryReport.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdTreasuryReport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdTreasuryReport.DataSource = GetReportData();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private List<usp_GetTreasuryPaymentReportData_Result> GetReportData()
        {
            int verticalId = drpVerticals.SelectedValue != string.Empty ? Convert.ToInt32(drpVerticals.SelectedValue) : 0;
            string companyCode = drpCompanyCode.SelectedValue != string.Empty ? drpCompanyCode.SelectedValue : "All";
            string paymentNo = drpPayment.SelectedValue != string.Empty ? drpPayment.SelectedValue : "All";
            string treasuryNo = Convert.ToString(drpTreasuryNo.SelectedValue);
            if (string.IsNullOrEmpty(treasuryNo))
                treasuryNo = "All";
            DateTime FromDate = dtFromDate.SelectedDate.Value;
            DateTime ToDate = dtToDate.SelectedDate.Value;
            return TreasuryWorkflowModel.GetTreasuryPaymentReportData(companyCode,verticalId,paymentNo, treasuryNo, FromDate, ToDate);
        }

        private void BindVerticalDropdown()
        {
            VerticalModel verticalModel = new VerticalModel();
            List<SuzlonBPP.Models.VerticalMaster> lstVertical = verticalModel.GetVerticals();
            drpVerticals.DataValueField = "VerticalId";
            drpVerticals.DataTextField = "Name";
            drpVerticals.DataSource = lstVertical;
            drpVerticals.DataBind();
        }

        private void BindCompanyCodeDropdown()
        {
            int verticalId = drpVerticals.SelectedValue != string.Empty ? Convert.ToInt32(drpVerticals.SelectedValue) : 0;
            VerticalModel verticalModel = new VerticalModel();
            List<SuzlonBPP.Models.ListItem> lstVertical = TreasuryDetailModel.GetTreasuryCompanyCodeNumber(verticalId);
            drpCompanyCode.DataValueField = "Id";
            drpCompanyCode.DataTextField = "Name";
            drpCompanyCode.DataSource = lstVertical;
            drpCompanyCode.DataBind();
        }

        private void BindPaymentLotNoDropdown()
        {
            VerticalModel verticalModel = new VerticalModel();
            List<SuzlonBPP.Models.ListItem> lstVertical = TreasuryDetailModel.GetTreasuryPaymentLotNumber();
            drpPayment.DataValueField = "Id";
            drpPayment.DataTextField = "Name";
            drpPayment.DataSource = lstVertical;
            drpPayment.DataBind();
        }

        private void BindTreasuryNoDropdown()
        {
            int verticalId = Convert.ToInt32(drpVerticals.SelectedValue);
            List<Models.ListItem> lstVertical = TreasuryDetailModel.GetTreasuryAllocationNumber(verticalId);
            drpTreasuryNo.DataValueField = "Id";
            drpTreasuryNo.DataTextField = "Name";
            drpTreasuryNo.DataSource = lstVertical;
            drpTreasuryNo.DataBind();
        }

        protected void drpCompanyCode_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {

        }

        protected void drpPayment_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {

        }
    }
}