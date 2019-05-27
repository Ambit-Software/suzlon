using Cryptography;
using Microsoft.Reporting.WebForms;
using SolarPMS;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class ReportList : System.Web.UI.Page
    {
        #region "Data Member"
        public int UserId;
        public int ReportId;
        public string ReportFilename;
        public string ReportName;
        #endregion

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    InitializeControls();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void radlistboxReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSapSite.ClearSelection();
                ddlSapProjects.ClearSelection();
                ddlArea.ClearSelection();
                ddlNetwork.ClearSelection();
                ddlType.ClearSelection();
                SetControlsAccordingToReport();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlSapSite_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                FillSapProjectDropdown();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateReports();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlSapProjects_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindAreas();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindNetwork(string.Empty);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion

        #region "Private Methods"
        private void InitializeControls()
        {
            SetReportVariables();
            spanReporName.InnerText = ReportName;            
            BindSiteData();
            BindTypes();
            ddlArea.EmptyMessage = Constants.CONST_ALL_TEXT;
            ddlNetwork.EmptyMessage = Constants.CONST_ALL_TEXT;
            SetControlsAccordingToReport();
        }

        private void SetReportVariables()
        {
            if (Session["UserId"] != null)
                UserId = Convert.ToInt32(Session["UserId"]);

            string queryString = Crypto.Instance.Decrypt(HttpUtility.UrlDecode(Request.QueryString.ToString()));
            string[] queryStringValues = queryString.Split('&');


            if (queryStringValues.Count() > 0)
            {
                ViewState["ReportId"] = ReportId = Convert.ToInt32((queryString.Split('&'))[0].Split('=')[1]);
                ReportFilename = (queryString.Split('&'))[1].Split('=')[1];
                ReportName = (queryString.Split('&'))[2].Split('=')[1];
            }

            GetReportDetails();
        }

        private void GetReportDetails()
        {
            Session["ReportParameters"] = ReportsModel.GetReportParameterList(Convert.ToInt32(ViewState["ReportId"]));
        }

        private void BindSiteData()
        {
            TaskModel taskModel = new TaskModel();
            List<UserAssignedTaskDataForDropdown> lstSite = Convert.ToInt32(ViewState["ReportId"]) != 11 
                                                            ? CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), string.Empty, string.Empty, string.Empty, "site")
                                                            : CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), string.Empty, string.Empty, string.Empty, "site", true, true);
            int siteCount = lstSite.Count;
            if (siteCount < 1 || siteCount > 1)
                ddlSapSite.EmptyMessage = Constants.CONST_ALL_TEXT;
            ddlSapSite.DataSource = lstSite;
            ddlSapSite.DataTextField = "Value";
            ddlSapSite.DataValueField = "Id";
            ddlSapSite.DataBind();

            if (siteCount == 1)
            {
                ddlSapSite.SelectedIndex = 0;
                FillSapProjectDropdown();
            }
            else
                ddlSapProjects.EmptyMessage = Constants.CONST_ALL_TEXT;
        }

        private void FillSapProjectDropdown()
        {
            TaskModel taskModel = new TaskModel();
            string siteId = ddlSapSite.SelectedItem.Text.Trim();
            ReportId = Convert.ToInt32(ViewState["ReportId"]);
            if (!string.IsNullOrEmpty(siteId))
            {
                List<UserAssignedTaskDataForDropdown> lstProjects = Convert.ToInt32(ViewState["ReportId"]) != 11
                                                                    ? CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), siteId, string.Empty, string.Empty, "project")
                                                                    : CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), siteId, string.Empty, string.Empty, "project", true, true);
                ddlSapProjects.ClearSelection();
                ddlSapProjects.Items.Clear();
                ddlSapProjects.DataSource = lstProjects;
                ddlSapProjects.DataTextField = "Value";
                ddlSapProjects.DataValueField = "Id";
                if (lstProjects.Count > 1 || lstProjects.Count < 1)
                {
                    ddlSapProjects.EmptyMessage = Constants.CONST_ALL_TEXT;
                    ddlSapProjects.SelectedIndex = -1;
                }
                else
                {
                    ddlSapProjects.SelectedValue = lstProjects[0].Id;
                    ddlSapProjects.SelectedIndex = 0;

                    if (ReportId == Convert.ToInt32(Constants.ReportName.LandAnalysis))
                        BindNetwork("LAND");
                    else
                        BindAreas();
                }
            }
            else
            {
                ddlSapProjects.EmptyMessage = Constants.CONST_ALL_TEXT;
                ddlSapProjects.ClearSelection();
                ddlSapProjects.Items.Clear();
            }

            ddlSapProjects.DataBind();
        }

        private void BindAreas()
        {
            ReportId = Convert.ToInt32(ViewState["ReportId"]);
            CommonFunctions commonFunctions = new CommonFunctions();
            List<UserAssignedTaskDataForDropdown> lstProjects = Convert.ToInt32(ViewState["ReportId"]) != 11
                                                                ?CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), ddlSapSite.SelectedItem.Text.Trim(),ddlSapProjects.SelectedValue.Trim(), string.Empty, "area")
                                                                : CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), ddlSapSite.SelectedItem.Text.Trim(), ddlSapProjects.SelectedValue.Trim(), string.Empty, "area",true,true);
            int count = lstProjects.Count;
            ddlArea.DataSource = lstProjects;
            ddlArea.DataTextField = "Value";
            ddlArea.DataValueField = "Id";
            ddlArea.DataBind();

            if (count > 1 || count < 1)
            {
                ddlArea.EmptyMessage = Constants.CONST_ALL_TEXT;
                ddlArea.SelectedIndex = -1;
            }
            else if (ReportId == Convert.ToInt32(Constants.ReportName.LandAnalysis))
            {
                ddlArea.SelectedValue = "1";
                BindNetwork("LAND");
            }
            else if (ReportId == Convert.ToInt32(Constants.ReportName.DE))
            {
                ddlArea.SelectedValue = "2";
                BindNetwork("E&C");
            }
            else
            {
                ddlArea.SelectedValue = lstProjects[0].Id;
                ddlArea.SelectedIndex = 0;
                BindNetwork(string.Empty);
            }
        }

        private void BindNetwork(string area)
        {
            area = area != string.Empty ? area : ddlArea.SelectedItem.Text.Trim();
            CommonFunctions commonFunctions = new CommonFunctions();
            List<UserAssignedTaskDataForDropdown> lstNetwork = Convert.ToInt32(ViewState["ReportId"]) != 11
                                                                ? CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), ddlSapSite.SelectedItem.Text.Trim(), ddlSapProjects.SelectedValue.Trim(), area, "network")
                                                                : CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), ddlSapSite.SelectedItem.Text.Trim(), ddlSapProjects.SelectedValue.Trim(), area, "network", true, true);
            int count = lstNetwork.Count;
            ddlNetwork.DataSource = lstNetwork;
            ddlNetwork.DataTextField = "Value";
            ddlNetwork.DataValueField = "Id";
            ddlNetwork.DataBind();

            if (count > 1 || count < 1)
            {
                ddlNetwork.EmptyMessage = Constants.CONST_ALL_TEXT;
                ddlNetwork.SelectedIndex = -1;
            }
            else
            {
                ddlNetwork.SelectedValue = lstNetwork[0].Id;
                ddlNetwork.SelectedIndex = 0;
            }

        }

        private void BindTypes()
        {
            List<Models.ListItem> lstTypes = new List<Models.ListItem>();
            lstTypes.Add(new Models.ListItem() { Id = "1", Name = "Block" });
            lstTypes.Add(new Models.ListItem() { Id = "2", Name = "Inverter" });
            lstTypes.Add(new Models.ListItem() { Id = "3", Name = "Table" });
            lstTypes.Add(new Models.ListItem() { Id = "4", Name = "SCB" });

            ddlType.DataSource = lstTypes;
            ddlType.DataTextField = "Name";
            ddlType.DataValueField = "Id";
            ddlType.EmptyMessage = Constants.CONST_ALL_TEXT;
            ddlType.DataBind();
        }

        private void SetControlsAccordingToReport()
        {
            List<Models.ReportParameter> lstReportParameter = (List<Models.ReportParameter>)Session["ReportParameters"];
            List<Models.ReportParameter> selectedReportParameter = lstReportParameter.Where(r => r.ReportId == ReportId && r.IsEnabled).ToList();

            divDrpSite.Style.Add("visibility", "hidden");
            divDrpProject.Style.Add("visibility", "hidden");
            divDrpArea.Style.Add("display", "none");
            divDrpNetwork.Style.Add("visibility", "hidden");
            divDrpType.Style.Add("visibility", "hidden");
            divBtnSearch.Style.Add("visibility", "hidden");
            foreach (Models.ReportParameter parameter in selectedReportParameter)
            {
                switch (parameter.ParameterName.ToLower())
                {
                    case "site":
                        divDrpSite.Style.Add("visibility", "visible");
                        break;
                    case "project":
                        divDrpProject.Style.Add("visibility", "visible");
                        break;
                    case "area":
                        divDrpArea.Style.Add("display", "block");
                        break;
                    case "network":
                        divDrpNetwork.Style.Add("visibility", "visible");
                        break;
                    case "type":
                        divDrpType.Style.Add("visibility", "visible");
                        break;
                }

                divBtnSearch.Style.Add("visibility", "visible");
            }
            if (ReportId == Convert.ToInt32(Constants.ReportName.Material))
                divBtnSearch.Style.Add("visibility", "visible");
        }

        private void GenerateReports()
        {
            string strUserName = ConfigurationManager.AppSettings["UserName"];
            string strPassword = ConfigurationManager.AppSettings["Password"];
            string strDomain = ConfigurationManager.AppSettings["Domain"];
            string strWebsiteIp = ConfigurationManager.AppSettings["WebIP"];

            SetReportVariables();

            string site = ddlSapSite.SelectedItem == null ? Constants.CONST_ALL_TEXT : ddlSapSite.SelectedItem.Text.Trim();
            string project = ddlSapProjects.SelectedValue == string.Empty ? Constants.CONST_ALL_TEXT : ddlSapProjects.SelectedValue.Trim();
            string area = ddlArea.SelectedValue == string.Empty ? Constants.CONST_ALL_TEXT : ddlArea.SelectedValue.Trim();
            string network = Constants.CONST_ALL_TEXT;

            if (ddlNetwork.CheckedItems.Count > 0)
            {
                network = string.Empty;
                foreach (RadComboBoxItem selectedNetwork in ddlNetwork.CheckedItems)
                    network += selectedNetwork.Value.Trim() + ",";
                network = network.TrimEnd(new char[] { ',' });
            }

            string type = ddlType.SelectedValue == string.Empty ? Constants.CONST_ALL_TEXT : ddlType.SelectedValue.Trim();
            List<Models.ReportParameter> lstReportParameter = (List<Models.ReportParameter>)Session["ReportParameters"];

            List<Models.ReportParameter> selectedReportParameter = lstReportParameter.Where(r => r.ReportId == ReportId && r.IsEnabled).ToList();
            Microsoft.Reporting.WebForms.ReportParameter[] parm = { };
            if (selectedReportParameter.Count > 0)
            {
                parm = new Microsoft.Reporting.WebForms.ReportParameter[selectedReportParameter.Count];
                int count = 0;
                foreach (Models.ReportParameter parameter in selectedReportParameter)
                {
                    switch (parameter.ParameterName.ToLower())
                    {
                        case "site":
                            parm[count] = new Microsoft.Reporting.WebForms.ReportParameter(parameter.ParameterName, site);
                            count++;
                            break;
                        case "project":
                            parm[count] = new Microsoft.Reporting.WebForms.ReportParameter(parameter.ParameterName, project);
                            count++;
                            break;
                        case "area":
                            parm[count] = new Microsoft.Reporting.WebForms.ReportParameter(parameter.ParameterName, area);
                            count++;
                            break;
                        case "network":
                            parm[count] = new Microsoft.Reporting.WebForms.ReportParameter(parameter.ParameterName, network);
                            count++;
                            break;
                        case "type":
                            parm[count] = new Microsoft.Reporting.WebForms.ReportParameter(parameter.ParameterName, type);
                            count++;
                            break;
                        case "userid":
                            parm[count] = new Microsoft.Reporting.WebForms.ReportParameter(parameter.ParameterName, Convert.ToInt32(Session["UserId"]).ToString());
                            count++;
                            break;
                    }
                }
            }

            RPTViewer.ServerReport.ReportPath = "/Suzzlon/" + ReportFilename;
            RPTViewer.ProcessingMode = ProcessingMode.Remote;
            RPTViewer.ServerReport.ReportServerCredentials = new ReportCredentials(strUserName, strPassword, strDomain);
            RPTViewer.ServerReport.ReportServerUrl = new System.Uri(strWebsiteIp);
            RPTViewer.ServerReport.SetParameters(parm);
            RPTViewer.ServerReport.Refresh();
            RPTViewer.ShowParameterPrompts = false;
        }

        #endregion
    }
}