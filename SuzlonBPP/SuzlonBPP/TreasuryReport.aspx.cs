using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace SuzlonBPP
{
    public partial class TreasuryReport : Page
    {
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindVerticalDropdown();
                    dtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                    dtToDate.SelectedDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdTreasury_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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

        #endregion

        #region "Methods"
        private void BindVerticalDropdown()
        {
            VerticalModel verticalModel = new VerticalModel();
            List<SuzlonBPP.Models.VerticalMaster> lstVertical = verticalModel.GetVerticals();
            drpVerticals.DataValueField = "VerticalId";
            drpVerticals.DataTextField = "Name";
            drpVerticals.DataSource = lstVertical;
            drpVerticals.DataBind();
        }

        private List<usp_GetTreasuryReportData_Result> GetReportData()
        {
            int verticalId = drpVerticals.SelectedValue != string.Empty ? Convert.ToInt32(drpVerticals.SelectedValue) : 0;
            string treasuryNo = Convert.ToString(drpTreasuryNo.SelectedValue);
            if (string.IsNullOrEmpty(treasuryNo))
                treasuryNo = "All";
            DateTime FromDate = dtFromDate.SelectedDate.Value;
            DateTime ToDate = dtToDate.SelectedDate.Value;
            return TreasuryWorkflowModel.GetTreasuryReportData(verticalId, treasuryNo, FromDate, ToDate);
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

        #endregion
    }
}