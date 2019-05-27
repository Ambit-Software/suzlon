using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class ToDoList : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();

        #region "Events"

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserId"] == null && !string.IsNullOrEmpty(Request.QueryString.ToString()))
                {
                    Response.Redirect(ConfigurationManager.AppSettings["WebsiteUrl"] + "/Login.aspx?ReturnURL=" + HttpContext.Current.Request.Url.AbsoluteUri, true);
                }
                else
                {
                    CheckPageAccess();

                    if (!IsPostBack)
                        InitializeControls();

                    SetPendigTimesheetColumnVisibility();

                    Session[Constants.CONST_SESSION_TIMESHEET_PARAMETER] = null;
                    radNotificationMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlSapSite_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
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

        protected void ddlArea_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            try
            {
                GetAssignedNetworks();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridPendingTimesheet_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    BindEventToGridButtons(e);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void gridRejectedTimesheet_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    BindEventToGridButtons(e);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridApprovedTimesheet_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    BindEventToGridButtons(e);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridActivityDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindMyRecords(false);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridRejectedTimesheet_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindRejectedTimesheetData(false);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridApprovedTimesheet_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindApprovedTimesheetData(false);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridPendingTimesheet_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindPendingTimesheetData(false);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridActivityDetails_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    usp_ToDoListMyRecords_Result dataItem = (usp_ToDoListMyRecords_Result)item.DataItem;

                    Button btnViewAttachment = item.FindControl("btnViewAttachment") as Button;
                    Button btnAssignedHistory = item.FindControl("btnAssignedHistory") as Button;
                    Button btnViewTimesheets = item.FindControl("btnViewTimesheets") as Button;
                    Button btnAddTimesheet = item.FindControl("btnAddTimesheet") as Button;

                    if (dataItem.IsTimesheetFound != 0)
                    {
                        btnAddTimesheet.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridActivityDetails_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    Models.usp_ToDoListMyRecords_Result dataItem = (Models.usp_ToDoListMyRecords_Result)item.DataItem;
                    Button btnViewAttachment = item.FindControl("btnViewAttachment") as Button;
                    Button btnAssignedHistory = item.FindControl("btnAssignedHistory") as Button;
                    Button btnViewTimesheets = item.FindControl("btnViewTimesheets") as Button;
                    Button btnAddTimesheet = item.FindControl("btnAddAttachment") as Button;
                    Button btnRaiseIssues = item.FindControl("btnRaiseIssues") as Button;
                    Button btnViewIssues = item.FindControl("btnViewIssues") as Button;

                    int activityId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ActivityId"]);
                    int subActivityId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubActivityId"]);
                    int rowIndex = e.Item.ItemIndex;

                    btnViewAttachment.Attributes["onclick"] = String.Format("return openAttachmentForActivity('{0}','{1}','{2}');", activityId, subActivityId, rowIndex);
                    btnViewTimesheets.Attributes["onclick"] = String.Format("return viewTimesheets('{0}','{1}','{2}');", activityId, subActivityId, rowIndex);
                    btnAssignedHistory.Attributes["onclick"] = String.Format("return viewAssignedHistory('{0}','{1}','{2}');", activityId, subActivityId, rowIndex);
                    btnViewIssues.Attributes["onclick"] = String.Format("return viewIssues('{0}','{1}','{2}');", activityId, subActivityId, rowIndex);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridActivityDetails_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (Session["ProfileName"].ToString() == Constants.CONST_DESIGN_ENGINEER
                    || Session["ProfileName"].ToString() == Constants.CONST_SITE_FUNCTIONAL_USER)
                {
                    gridActivityDetails.MasterTableView.GetColumn("AddTimesheet").Visible = true;
                    //gridActivityDetails.MasterTableView.GetColumn("ViewAttachment").Visible = true;
                    //gridActivityDetails.MasterTableView.GetColumn("ViewTimesheet").Visible = true;
                }
                else
                {
                    gridActivityDetails.MasterTableView.GetColumn("AddTimesheet").Visible = false;
                    //gridActivityDetails.MasterTableView.GetColumn("ViewAttachment").Visible = false;
                    //gridActivityDetails.MasterTableView.GetColumn("ViewTimesheet").Visible = false;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ToDoListRadAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            try
            {
                string values = e.Argument;
                string[] parameters = values.Split('#');
                int userId = 0;

                if (parameters[0] == "TimesheetAttachment" || parameters[0] == "ViewInverterSCBTable"
                    || parameters[0] == "ViewSurvey" || parameters[0] == "ViewBlocks" || parameters[0] == "ViewComments")
                {
                    Session[Constants.CONST_SESSION_TIMESHEET_ID] = Convert.ToInt32(parameters[1]);
                    Session[Constants.CONST_SESSION_ACTIVITY_ID] = null;
                    Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = null;
                }
                else if (parameters[0] == "TimesheetAttachmentForActivity" || parameters[0] == "ViewTimesheet"
                    || parameters[0] == "ViewIssues" || parameters[0]== "ViewAssignedHistory")
                {
                    Session[Constants.CONST_SESSION_ACTIVITY_ID] = Convert.ToInt32(parameters[1]);
                    Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = Convert.ToInt32(parameters[2]);
                    Session[Constants.CONST_SESSION_TIMESHEET_ID] = null;
                }

                switch (parameters[0])
                {
                    case "TimesheetAttachment":
                        Session[Constants.CONST_SESSION_TIMESHEET_ID] = Convert.ToInt32(parameters[1]);
                        BindTimesheetAttachment(Convert.ToInt32(parameters[1]));
                        break;

                    case "TimesheetAttachmentForActivity":
                        BindTimesheetAttachmentForActivity(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]));
                        break;

                    case "ViewTimesheet":
                        BindTimesheetView(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]));
                        break;

                    case "ViewAssignedHistory":
                        BindTimesheetHistory(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]));
                        break;

                    case "ViewIssues":
                        BindIssueView(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]));
                        break;

                    case "ViewInverterSCBTable":
                        BindInverterSCBTable(Convert.ToInt32(parameters[1]));
                        break;

                    case "ViewSurvey":
                        BindSurveys(Convert.ToInt32(parameters[1]));
                        break;

                    case "ViewBlocks":
                        BindBlocks(Convert.ToInt32(parameters[1]));
                        break;
                    case "ViewComments":
                        BindComments(Convert.ToInt32(parameters[1]));
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridTimesheetAttachment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    HyperLink attachment = item["FileName"].Controls[0] as HyperLink;
                    attachment.NavigateUrl = ConfigurationManager.AppSettings["MailUrl"] + "Upload/Attachment/Timesheet/" + attachment.Text;
                    attachment.Target = "new";
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                int workflowStatus = 0;
                if (Session["ProfileName"].ToString() == Constants.CONST_SITE_QUALITY_MANAGER)
                    workflowStatus = Convert.ToInt32(Constants.WorkflowStatus.QAApproved);

                else if (Session["ProfileName"].ToString() == Constants.CONST_SITE_FUNCTIONAL_MANAGER
                                    || Session["ProfileName"].ToString() == Constants.CONST_DESIGN_MANAGER)
                    workflowStatus = Convert.ToInt32(Constants.WorkflowStatus.PMApproved);

                ApproveRejectTimesheet(workflowStatus);
            }
            catch (Exception ex)
            {
                radNotificationMessage.Visible = true;
                if (ex.Source == "SAP.Connector")
                    radNotificationMessage.Text = "Unable to open SAP connection.";
                else
                    radNotificationMessage.Text = "Failed to update timesheet.";
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                int workflowStatus = 0;
                if (Session["ProfileName"].ToString() == Constants.CONST_SITE_QUALITY_MANAGER)
                    workflowStatus = Convert.ToInt32(Constants.WorkflowStatus.QARejected);

                else if (Session["ProfileName"].ToString() == Constants.CONST_SITE_FUNCTIONAL_MANAGER
                                    || Session["ProfileName"].ToString() == Constants.CONST_DESIGN_MANAGER)
                    workflowStatus = Convert.ToInt32(Constants.WorkflowStatus.PMRejected);

                ApproveRejectTimesheet(workflowStatus);
            }
            catch (Exception ex)
            {
                radNotificationMessage.Visible = true;
                if (ex.Source == "SAP.Connector")
                    radNotificationMessage.Text = "Unable to open SAP connection.";
                else
                    radNotificationMessage.Text = "Failed to update timesheet.";
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void tabToDoList_TabClick(object sender, RadTabStripEventArgs e)
        {
            try
            {
                int selectedTabIndex = tabToDoList.SelectedTab.Index;
                switch (selectedTabIndex)
                {
                    case 0:
                        BindMyRecords(true);
                        HideExportButtons("MyRecords");
                        break;
                    case 1:
                        BindPendingTimesheetData(true);
                        HideExportButtons("ApprovalPending");
                        break;
                    case 2:
                        BindApprovedTimesheetData(true);
                        HideExportButtons("Approved");
                        break;
                    case 3:
                        BindRejectedTimesheetData(true);
                        HideExportButtons("Rejected");
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void HideExportButtons(string flag)
        {
            switch (flag)
            {
                case "MyRecords":
                    btnExportMyRecord.Visible = true;
                    btnExportPendingRecord.Visible = false;
                    btnExportRejectedTimesheet.Visible = false;
                    btnExportApprovedTimesheet.Visible = false;
                    break;
                case "ApprovalPending":
                    btnExportMyRecord.Visible = false;
                    btnExportPendingRecord.Visible = true;
                    btnExportRejectedTimesheet.Visible = false;
                    btnExportApprovedTimesheet.Visible = false;
                    break;
                case "Approved":
                    btnExportMyRecord.Visible = false;
                    btnExportPendingRecord.Visible = false;
                    btnExportRejectedTimesheet.Visible = false;
                    btnExportApprovedTimesheet.Visible = true;
                    break;
                case "Rejected":
                    btnExportMyRecord.Visible = false;
                    btnExportPendingRecord.Visible = false;
                    btnExportRejectedTimesheet.Visible = true;
                    btnExportApprovedTimesheet.Visible = false;
                    break;
            }
        }

        protected void btnExportMyRecord_Click(object sender, EventArgs e)
        {
            try
            {
                gridActivityDetails.ExportSettings.ExportOnlyData = true;
                gridActivityDetails.ExportSettings.IgnorePaging = true;
                gridActivityDetails.ExportSettings.OpenInNewWindow = true;
                gridActivityDetails.ExportSettings.FileName = "MyRecords";
                gridActivityDetails.MasterTableView.AllowFilteringByColumn = false;
                gridActivityDetails.MasterTableView.AllowSorting = false;
                gridActivityDetails.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridActivityDetails.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnExportPendingRecord_Click(object sender, EventArgs e)
        {
            try
            {
                gridPendingTimesheet.ExportSettings.ExportOnlyData = true;
                gridPendingTimesheet.ExportSettings.IgnorePaging = true;
                gridPendingTimesheet.ExportSettings.OpenInNewWindow = true;
                gridPendingTimesheet.ExportSettings.FileName = "TimesheetPendingForApproval";
                gridPendingTimesheet.MasterTableView.AllowFilteringByColumn = false;
                gridPendingTimesheet.MasterTableView.AllowSorting = false;
                gridPendingTimesheet.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridPendingTimesheet.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnExportApprovedTimesheet_Click(object sender, EventArgs e)
        {
            try
            {
                gridApprovedTimesheet.ExportSettings.ExportOnlyData = true;
                gridApprovedTimesheet.ExportSettings.IgnorePaging = true;
                gridApprovedTimesheet.ExportSettings.OpenInNewWindow = true;
                gridApprovedTimesheet.ExportSettings.FileName = "ApprovedTimesheet";
                gridApprovedTimesheet.MasterTableView.AllowFilteringByColumn = false;
                gridApprovedTimesheet.MasterTableView.AllowSorting = false;
                gridApprovedTimesheet.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridApprovedTimesheet.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnExportRejectedTimesheet_Click(object sender, EventArgs e)
        {
            try
            {
                gridRejectedTimesheet.ExportSettings.ExportOnlyData = true;
                gridRejectedTimesheet.ExportSettings.IgnorePaging = true;
                gridRejectedTimesheet.ExportSettings.OpenInNewWindow = true;
                gridRejectedTimesheet.ExportSettings.FileName = "RejectedTimesheet";
                gridRejectedTimesheet.MasterTableView.AllowFilteringByColumn = false;
                gridRejectedTimesheet.MasterTableView.AllowSorting = false;
                gridRejectedTimesheet.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridRejectedTimesheet.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddTimesheet_Click(object sender, EventArgs e)
        {
            try
            {
                Session[Constants.CONST_SESSION_TIMESHEET_PARAMETER] = null;
                Session[Constants.CONST_SESSION_TIMESHEET_ID] = null;
                Session[Constants.CONST_SESSION_TIMESHEET_PARAMETER] = ((GridEditableItem)(((Control)sender).NamingContainer)).KeyValues;
                ToDoListRadAjaxManager.ResponseScripts.Add("openRadWin('timesheet');");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnRaiseIssues_Click(object sender, EventArgs e)
        {
            try
            {
                GridEditableItem item = ((GridEditableItem)(((Control)sender).NamingContainer));
                if (item != null)
                {
                    Session["IssueId"] = item.GetDataKeyValue("IssueId");
                    Session[Constants.CONST_SESSION_ACTIVITY_ID] = item.GetDataKeyValue("ActivityId");
                    Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = item.GetDataKeyValue("SubActivityId");
                    Session[Constants.CONST_SESSION_ACTIVITY_DESCRIPTION] = item.GetDataKeyValue("ActivityDescription");
                    Session[Constants.CONST_SESSION_SUBACTIVITY_DESCRIPTION] = item.GetDataKeyValue("SAPSubActivityDescription");
                    ToDoListRadAjaxManager.ResponseScripts.Add("openRadWin('issue');");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnEditTimesheet_Click(object sender, EventArgs e)
        {
            try
            {
                GridEditableItem item = ((GridEditableItem)(((Control)sender).NamingContainer));
                if (item != null)
                {
                    Session[Constants.CONST_SESSION_TIMESHEET_ID] = item.GetDataKeyValue("TimeSheetId");
                    Session[Constants.CONST_SESSION_ACTIVITY_ID] = item.GetDataKeyValue("ActivityId");
                    Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = item.GetDataKeyValue("SubActivityId");
                    Session[Constants.CONST_SESSION_TIMESHEET_USERID] = item.GetDataKeyValue("UserId");
                    ToDoListRadAjaxManager.ResponseScripts.Add("openRadWin('timesheet');");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
       
        protected void gridIssues_DataBound(object sender, EventArgs e)
        {
            try
            {
                foreach (GridDataItem item in gridIssues.Items)
                {

                    string status = Convert.ToString(item.GetDataKeyValue("IssueStatus"));
                    if (status == Convert.ToString(Constants.TimesheetStatus.Closed))
                    {
                        Button btnEditIssue = item.FindControl("btnEditIssue") as Button;
                        if (btnEditIssue != null)
                            btnEditIssue.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridTimesheetView_DataBound(object sender, EventArgs e)
        {
            try
            {
                foreach (GridDataItem item in gridTimesheetView.Items)
                {
                    bool allowEdit = Convert.ToBoolean(item.GetDataKeyValue("AllowEdit"));
                    Button btnEditIssue = item.FindControl("btnEditTimesheet") as Button;
                    if (btnEditIssue != null)
                    {
                        if (allowEdit)
                            btnEditIssue.Text = "Edit";
                        else
                            btnEditIssue.Text = "View";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridTimesheetView_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (gridTimesheetView.Items.Count > 0)
                {
                    int ActivityId = Session[Constants.CONST_SESSION_ACTIVITY_ID] != null ? Convert.ToInt32(Session[Constants.CONST_SESSION_ACTIVITY_ID]) : 0;
                    int SubActivityId = Session[Constants.CONST_SESSION_SUBACTIVITY_ID] != null ? Convert.ToInt32(Session[Constants.CONST_SESSION_SUBACTIVITY_ID]) : 0;

                    if (ActivityId != 0 || SubActivityId != 0)
                        BindTimesheetView(ActivityId, SubActivityId, false);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridTimesheetAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (gridTimesheetAttachment.Items.Count > 0)
                {
                    int ActivityId = Session[Constants.CONST_SESSION_ACTIVITY_ID] != null ? Convert.ToInt32(Session[Constants.CONST_SESSION_ACTIVITY_ID]) : 0;
                    int SubActivityId = Session[Constants.CONST_SESSION_SUBACTIVITY_ID] != null ? Convert.ToInt32(Session[Constants.CONST_SESSION_SUBACTIVITY_ID]) : 0;

                    if (ActivityId != 0 || SubActivityId != 0)
                        BindTimesheetAttachmentForActivity(ActivityId, SubActivityId,false);
                    else if (Session[Constants.CONST_SESSION_TIMESHEET_ID] != null)
                    {
                        BindTimesheetAttachment(Convert.ToInt32(Session[Constants.CONST_SESSION_TIMESHEET_ID]), false);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridIssues_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (gridIssues.Items.Count > 0)
                {
                    int ActivityId = Session[Constants.CONST_SESSION_ACTIVITY_ID] != null ? Convert.ToInt32(Session[Constants.CONST_SESSION_ACTIVITY_ID]) : 0;
                    int SubActivityId = Session[Constants.CONST_SESSION_SUBACTIVITY_ID] != null ? Convert.ToInt32(Session[Constants.CONST_SESSION_SUBACTIVITY_ID]) : 0;

                    if (ActivityId != 0 || SubActivityId != 0)
                        BindIssueView(ActivityId, SubActivityId, false);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridSurveys_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (gridSurveys.Items.Count > 0)
                {
                    if (Session[Constants.CONST_SESSION_TIMESHEET_ID] != null)
                    {
                        BindSurveys(Convert.ToInt32(Session[Constants.CONST_SESSION_TIMESHEET_ID]), false);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridViewComments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (gridViewComments.Items.Count > 0)
                {
                    if (Session[Constants.CONST_SESSION_TIMESHEET_ID] != null)
                    {
                        BindComments(Convert.ToInt32(Session[Constants.CONST_SESSION_TIMESHEET_ID]), false);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridBlocks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (gridBlocks.Items.Count > 0)
                {
                    if (Session[Constants.CONST_SESSION_TIMESHEET_ID] != null)
                    {
                        BindBlocks(Convert.ToInt32(Session[Constants.CONST_SESSION_TIMESHEET_ID]), false);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridInvSCBTable_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (gridInvSCBTable.Items.Count > 0)
                {
                    if (Session[Constants.CONST_SESSION_TIMESHEET_ID] != null)
                    {
                        BindInverterSCBTable(Convert.ToInt32(Session[Constants.CONST_SESSION_TIMESHEET_ID]), false);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnPartialApprove_Click(object sender, EventArgs e)
        {
            try
            {
                GridEditableItem item = ((GridEditableItem)(((Control)sender).NamingContainer));
                if (item != null)
                {
                    Session[Constants.CONST_SESSION_TIMESHEET_ID] = item.GetDataKeyValue("TimeSheetId");
                    Session[Constants.CONST_SESSION_ACTIVITY_ID] = item.GetDataKeyValue("ActivityId");
                    Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = item.GetDataKeyValue("SubActivityId");
                    Session[Constants.CONST_SESSION_TIMESHEET_USERID] = item.GetDataKeyValue("UserId");
                    Session[Constants.CONST_SESSION_IS_PARTIAL_APPROVE] = "True";
                    ToDoListRadAjaxManager.ResponseScripts.Add("openRadWin('timesheet');");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridPendingTimesheet_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    Button btnPartialApprove = item.FindControl("btnPartialApprove") as Button;
                    ToDoListPendingForApproval toDoListPendingForApproval = item.DataItem as ToDoListPendingForApproval;
                    if (toDoListPendingForApproval.MobileFunction.ToUpper() == "A" || toDoListPendingForApproval.MobileFunction.ToUpper() == "C"
                        || toDoListPendingForApproval.MobileFunction.ToUpper() == "E" || toDoListPendingForApproval.MobileFunction.ToUpper() == "F")
                        btnPartialApprove.Visible = false;
                    else
                        btnPartialApprove.Visible = true;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridOfflineTimesheet_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var lstOffline = TimesheetModel.GetRejectedOfflineTimesheet(userId, string.Empty, string.Empty, string.Empty, string.Empty, false);
                gridOfflineTimesheet.DataSource = TimesheetModel.GetRejectedOfflineTimesheet(userId, string.Empty, string.Empty, string.Empty, string.Empty, false);
                Label lblOfflineRecordCount = tabToDoList.Tabs[04].FindControl("lblOfflineRecordCount") as Label;
                lblOfflineRecordCount.Text = lstOffline.Count.ToString();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        #endregion

        #region "Private Method"
        private void GetAreas()
        {
            string siteId = ddlSapSite.SelectedValue.Trim();
            string projectId = ddlSapProjects.SelectedValue.Trim();
            string areaList = commonFunctions.RestServiceCall(string.Format(Constants.GET_AREA_BY_SITE_PROJECT, siteId, projectId), string.Empty);
            Models.DropdownValues lstArea = JsonConvert.DeserializeObject<Models.DropdownValues>(areaList);
            ddlArea.DataSource = lstArea.area;
            int networkCount = 0;
            if (networkCount == 1)
                GetAssignedNetworks();

            if (networkCount < 1 || networkCount > 1)
                ddlArea.DefaultMessage = Constants.CONST_SELECT_TEXT;

            ddlArea.DataBind();
        }
        private void BindMyRecords(bool IsDataBind)
        {
            TaskModel taskModel = new TaskModel();
            int userId = Convert.ToInt32(Session["UserId"]);
            string network = ddlNetwork.SelectedValue;
            string area = ddlArea.SelectedValue;
            string project = ddlSapProjects.SelectedValue;
            string site = ddlSapSite.SelectedValue;

            List<Models.usp_ToDoListMyRecords_Result> lstTimesheet = taskModel.GetMyRecords(userId, true, network, area, project, site);
            gridActivityDetails.DataSource = lstTimesheet;
            if (IsDataBind)
                gridActivityDetails.DataBind();

            Label lblMyRecordCount = tabToDoList.Tabs[0].FindControl("lblMyRecordCount") as Label;
            lblMyRecordCount.Text = lstTimesheet.Count().ToString();
        }
        private void BindRejectedTimesheetData(bool IsDataBind)
        {
            TaskModel taskModel = new TaskModel();
            int userId = Convert.ToInt32(Session["UserId"]);
            string network = ddlNetwork.SelectedValue;
            string area = ddlArea.SelectedValue;
            string project = ddlSapProjects.SelectedValue;
            string site = ddlSapSite.SelectedValue;
            List<ToDoListRejected> lstRejectedTimesheet = taskModel.GetToDoRejectedList(userId, network, area, project, site, true);
            gridRejectedTimesheet.DataSource = lstRejectedTimesheet;
            if (IsDataBind)
                gridRejectedTimesheet.DataBind();
            Label lblRejectedCount = tabToDoList.Tabs[3].FindControl("lblRejectedRecordCount") as Label;
            lblRejectedCount.Text = lstRejectedTimesheet.Count().ToString();
        }
        private void BindApprovedTimesheetData(bool IsDataBind)
        {
            TaskModel taskModel = new TaskModel();
            int userId = Convert.ToInt32(Session["UserId"]);
            string network = ddlNetwork.SelectedValue;
            string area = ddlArea.SelectedValue;
            string project = ddlSapProjects.SelectedValue;
            string site = ddlSapSite.SelectedValue;
            List<ToDoListApproved> lstApprovedTimesheet = taskModel.GetToDoApprovedList(userId, network, area, project, site, true);
            gridApprovedTimesheet.DataSource = lstApprovedTimesheet;
            if (IsDataBind)
                gridApprovedTimesheet.DataBind();

            Label lblApprovedRecordCount = tabToDoList.Tabs[2].FindControl("lblApprovedRecordCount") as Label;
            lblApprovedRecordCount.Text = lstApprovedTimesheet.Count().ToString();
        }
        private void InitializeControls()
        {
            TaskModel taskModel = new TaskModel();
            List<Models.ListItem> lstSite = taskModel.GetAllSite();
            int siteCount = lstSite.Count;
            if (siteCount < 1 || siteCount > 1)
                ddlSapSite.DefaultMessage = Constants.CONST_SELECT_TEXT;
            ddlSapSite.DataSource = lstSite;
            ddlSapSite.DataTextField = "Name";
            ddlSapSite.DataValueField = "Id";
            ddlSapSite.DataBind();

            ddlSapProjects.DefaultMessage = Constants.CONST_SELECT_TEXT;
            ddlArea.DefaultMessage = Constants.CONST_SELECT_TEXT;
            ddlNetwork.DefaultMessage = Constants.CONST_SELECT_TEXT;

            if (siteCount == 1)
                FillSapProjectDropdown();
        }

        /// <summary>
        /// Fill SAP project dropdown
        /// </summary>
        private void FillSapProjectDropdown()
        {
            TaskModel taskModel = new TaskModel();
            string siteId = ddlSapSite.SelectedValue.Trim();
            List<Models.ListItem> lstProjects = taskModel.GetAllProjects(siteId);
            ddlSapProjects.DataSource = lstProjects;
            ddlSapProjects.DataTextField = "Name";
            ddlSapProjects.DataValueField = "Id";
            if (lstProjects.Count > 1 || lstProjects.Count < 1)
                ddlSapProjects.DefaultMessage = Constants.CONST_SELECT_TEXT;
            ddlSapProjects.DataBind();
        }

        private void GetAssignedNetworks()
        {
            int areaId = Convert.ToInt32(ddlArea.SelectedValue.Trim());
            int userId = Convert.ToInt32(Session["UserId"]);
            List<Models.ListItem> lstNetwork = NetworkModel.GetNetworksForArea(userId, areaId);
            ddlNetwork.DataSource = lstNetwork;
            int networkCount = 0;
            if (networkCount < 1 || networkCount > 1)
                ddlNetwork.DefaultMessage = Constants.CONST_SELECT_TEXT;
            else
                ddlNetwork.DefaultMessage = "";
            ddlNetwork.DataBind();
        }

        private void BindTimesheetAttachment(int TimesheetId, bool IsDataBind = true)
        {
            gridTimesheetAttachment.DataSource = TimesheetModel.GetTimesheetAttachmentByTimesheetId(TimesheetId);
            if (IsDataBind)
            {
                gridTimesheetAttachment.CurrentPageIndex = 0;
                gridTimesheetAttachment.DataBind();
            }
        }

        private void BindTimesheetAttachmentForActivity(int ActivityId, int SubActivityId, bool isDataBind = true)
        {
            int userId = 0;
            if (Session["ProfileName"].ToString() == Constants.CONST_SITE_FUNCTIONAL_USER || Session["ProfileName"].ToString() == Constants.CONST_DESIGN_ENGINEER)
                userId = Convert.ToInt32(Session["UserId"]);
            gridTimesheetAttachment.DataSource = TimesheetModel.GetTimesheetAttachment(ActivityId, SubActivityId, userId);
            if (isDataBind)
            {
                gridTimesheetAttachment.CurrentPageIndex = 0;
                gridTimesheetAttachment.DataBind();
            }
        }

        private void BindTimesheetView(int ActivityId, int SubActivityId, bool isDataBind = true)
        {
            int userId = 0;
            if (Session["ProfileName"].ToString() == Constants.CONST_SITE_FUNCTIONAL_USER || Session["ProfileName"].ToString() == Constants.CONST_DESIGN_ENGINEER)
                userId = Convert.ToInt32(Session["UserId"]);

            gridTimesheetView.DataSource = TimesheetModel.GetSubmittedTimesheetDatails(ActivityId, SubActivityId, userId);
            if (isDataBind)
            {
                gridTimesheetView.CurrentPageIndex = 0;
                gridTimesheetView.DataBind();
            }
        }

        private void BindTimesheetHistory(int ActivityId, int SubActivityId)
        {
            TimesheetModel timesheetModel = new TimesheetModel();
            gridAssignedHistory.DataSource = timesheetModel.GetActivityHistory(ActivityId, SubActivityId); ;
            gridAssignedHistory.DataBind();
        }

        private void BindIssueView(int ActivityId, int SubactivityId, bool IsDataBind = true)
        {
            IssueMgmtModel issueManagement = new IssueMgmtModel();
            if (SubactivityId == 0)
                gridIssues.DataSource = issueManagement.GetIssueByActivityId(ActivityId);
            else
                gridIssues.DataSource = issueManagement.GetIssueBySubActivityId(SubactivityId);
            if (IsDataBind)
            {
                gridIssues.CurrentPageIndex = 0;
                gridIssues.DataBind();
            }
        }
        private void BindInverterSCBTable(int TimesheetId, bool IsDataBind = true)
        {
            gridInvSCBTable.DataSource = TableActivityModel.GetTimesheetActivityDetails(TimesheetId);

            Int16 status = GetStatusAccordingToTab(hTableQM, hTablePM);
            List<ManagerTimesheetActivity> lstManagerActivity = TableActivityModel.GetManagerUpdatedTimesheetActivity(TimesheetId, status);
            
            List<ManagerTimesheetActivity> lstPMManagerList = lstManagerActivity.Where(l => l.ManagerType 
                                                                                            == Convert.ToInt16(Constants.ManagerType.PM)).ToList();
            List<ManagerTimesheetActivity> lstQMManagerList = lstManagerActivity.Where(l => l.ManagerType 
                                                                                            == Convert.ToInt16(Constants.ManagerType.QM)).ToList();

                gridInvSCBTableQM.DataSource = lstQMManagerList;
                gridInvSCBTableQM.DataBind();
                gridInvSCBTablePM.DataSource = lstPMManagerList;
                gridInvSCBTablePM.DataBind();

            if (IsDataBind)
            {
                gridInvSCBTable.CurrentPageIndex = 0;
                gridInvSCBTable.DataBind();
            }
        }

        private void BindSurveys(int TimesheetId, bool IsDataBind = true)
        {
            TimesheetModel TimesheetModel = new TimesheetModel();
            gridSurveys.DataSource = TimesheetModel.GetTimesheetSurvayDetails(TimesheetId);
            Int16 status = GetStatusAccordingToTab(hSurveyQM, hSurveyPM);
            List<TimesheetSurveyDetailsView> lstManagerSurvey = TimesheetModel.GetTimesheetSurvayDetails(TimesheetId, true, status);
            if (IsDataBind)
            {
                gridSurveys.CurrentPageIndex = 0;
                gridSurveys.DataBind();
            }

            
            gridSurveysQM.DataSource = lstManagerSurvey.Where(l => l.ManagerType == Convert.ToInt16(Constants.ManagerType.QM));
            gridSurveysPM.DataSource = lstManagerSurvey.Where(l => l.ManagerType == Convert.ToInt16(Constants.ManagerType.PM));

            gridSurveysQM.DataBind();
            gridSurveysPM.DataBind();

        }

        private void BindComments(int TimesheetId, bool IsDataBind = true)
        {
            TimesheetModel TimesheetModel = new TimesheetModel();
            gridViewComments.DataSource = TimesheetModel.GetTimesheetComments(TimesheetId); ;
            if (IsDataBind)
            {
                gridViewComments.CurrentPageIndex = 0;
                gridViewComments.DataBind();
            }
        }

        private void BindBlocks(int TimesheetId, bool IsDataBind = true)
        {
            TableActivityModel tableActivityModel = new TableActivityModel();
            Timesheet timesheet = new Timesheet();
            timesheet.ActivityId = TimesheetId;
            gridBlocks.DataSource = tableActivityModel.GetTimesheetBlockDetails(TimesheetId);
            Int16 status = GetStatusAccordingToTab(hBlockApprovedByQM, hBlockApprovedByPM);

            //List<ManagerTimesheetBlockDetail> lstManagerBlocks = TimesheetModel.GetManagerBlockDetails(TimesheetId, status);
            List<TimesheetBlockDetailsView> lstManagerBlocks = tableActivityModel.GetTimesheetBlockDetails(TimesheetId, true, status);
            if (IsDataBind)
            {
                gridBlocks.CurrentPageIndex = 0;
                gridBlocks.DataBind();
            }

            gridBlocksQM.DataSource = lstManagerBlocks.Where(l => l.ManagerType == Convert.ToInt16(Constants.ManagerType.QM));
            gridBlocksPM.DataSource = lstManagerBlocks.Where(l => l.ManagerType == Convert.ToInt16(Constants.ManagerType.PM));
            gridBlocksQM.DataBind();
            gridBlocksPM.DataBind();
        }

        private short GetStatusAccordingToTab( HtmlGenericControl hQM, HtmlGenericControl hPM)
        {
            short status = 0;
            if (tabToDoList.SelectedIndex == 1)
            {
                status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved);
            }
            else if (tabToDoList.SelectedIndex == 2)
            {
                status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved);
                hQM.InnerText = "Partially Approved by QM";
                hPM.InnerText = "Partially Approved by PM";
            }
            else if (tabToDoList.SelectedIndex == 3)
            {
                status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Rejected);
                hQM.InnerText = "Partially Rejected by QM";
                hPM.InnerText = "Partially Rejected by PM";
            }

            return status;
        }

        private static void BindEventToGridButtons(GridItemEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            Button btnViewSurvey = item.FindControl("btnViewSurvey") as Button;
            Button btnViewBlock = item.FindControl("btnViewBlock") as Button;
            Button btnViewInvSCBTable = item.FindControl("btnViewInvSCBTable") as Button;
            Button btnViewAttachment = item.FindControl("btnViewAttachment") as Button;
            Button btnViewComments = item.FindControl("btnViewComments") as Button;

            int timesheetId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TimeSheetId"]);
            int rowIndex = Convert.ToInt32(e.Item.ItemIndex);

            if (btnViewAttachment != null)
                btnViewAttachment.Attributes["onclick"] = String.Format("return openAttachment('{0}','{1}');", timesheetId, rowIndex);

            if (btnViewInvSCBTable != null)
                btnViewInvSCBTable.Attributes["onclick"] = String.Format("return viewInverterSCBTable('{0}','{1}');", timesheetId, rowIndex);

            if (btnViewBlock != null)
                btnViewBlock.Attributes["onclick"] = String.Format("return viewBlocks('{0}','{1}');", timesheetId, rowIndex);

            if (btnViewSurvey != null)
                btnViewSurvey.Attributes["onclick"] = String.Format("return viewSurveys('{0}','{1}');", timesheetId, rowIndex);

            if (btnViewComments != null)
                btnViewComments.Attributes["onclick"] = String.Format("return viewComments('{0}','{1}');", timesheetId, rowIndex);
        }

        private void BindPendingTimesheetData(bool IsDataBind = false)
        {
            TaskModel taskModel = new TaskModel();
            int userId = Convert.ToInt32(Session["UserId"]);
            string network = ddlNetwork.SelectedValue;
            string area = ddlArea.SelectedValue;
            string project = ddlSapProjects.SelectedValue;
            string site = ddlSapSite.SelectedValue;

            List<ToDoListPendingForApproval> lstPendingTimesheet = taskModel.GetToDoPendingList(userId, network, area, project, site, true);
            gridPendingTimesheet.DataSource = lstPendingTimesheet;
            if (IsDataBind)
                gridPendingTimesheet.DataBind();

            Label lblPendingRecordCount = tabToDoList.Tabs[1].FindControl("lblPendingRecordCount") as Label;
            lblPendingRecordCount.Text = lstPendingTimesheet.Count().ToString();
        }
        private void ApproveRejectTimesheet(int workflowStatus)
        {
            List<Timesheet> lstRejectedTimesheet = new List<Timesheet>();
            foreach (GridDataItem item in gridPendingTimesheet.Items)
            {
                CheckBox selectedTimesheet = item.FindControl("chkSelect") as CheckBox;
                RadTextBox txtComments = item.FindControl("txtTimesheetComments") as RadTextBox;
                if (selectedTimesheet != null && selectedTimesheet.Checked)
                {
                    Timesheet timesheet = new Timesheet();
                    timesheet.TimeSheetId = Convert.ToInt32(item.OwnerTableView.DataKeyValues[item.ItemIndex]["TimeSheetId"]);
                    timesheet.Comments = txtComments.Text.Trim();
                    timesheet.WorkflowStatusId = workflowStatus;                    
                    timesheet.SAPActivity = Convert.ToString(item.GetDataKeyValue("SAPActivity"));
                    timesheet.SAPNetwork = Convert.ToString(item.GetDataKeyValue("SAPNetwork"));
                    timesheet.ActivityId = Convert.ToInt32(item.GetDataKeyValue("ActivityId"));
                    timesheet.SubActivityId = Convert.ToInt32(item.GetDataKeyValue("SubActivityId"));                    
                    lstRejectedTimesheet.Add(timesheet);
                }
            }

            if (lstRejectedTimesheet.Count > 0)
            {
                TimesheetModel timesheetModel = new TimesheetModel();
                bool result = timesheetModel.ApproveRejectTimesheet(lstRejectedTimesheet, Convert.ToInt32(Session["UserId"]));
                if (result)
                {
                    BindPendingTimesheetData(true);
                    radNotificationMessage.Visible = true;
                    if (workflowStatus == Convert.ToInt32(Constants.WorkflowStatus.PMApproved) || workflowStatus == Convert.ToInt32(Constants.WorkflowStatus.QAApproved))
                    {
                        try
                        {
                            foreach (Timesheet timesheet in lstRejectedTimesheet)
                            {
                                bool isRejectedTimesheet = TimesheetModel.IsRejectedTimesheet(timesheet.TimeSheetId);
                                // if timesheet is rejected by qa and now it is approved then send notification.
                                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendQualityRejectClosedNotification"]) &&
                                    workflowStatus == Convert.ToInt32(Constants.WorkflowStatus.QAApproved)
                                        && isRejectedTimesheet)
                                    NotificationHelper.SendQualityIssueClosedNotification(timesheet.ActivityId.Value, timesheet.SubActivityId.Value, timesheet.TimeSheetId);
                            }
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                        }

                        radNotificationMessage.Text = "Timesheet approved successfully.";
                        Label lblApprovedRecordCount = tabToDoList.Tabs[2].FindControl("lblApprovedRecordCount") as Label;
                        lblApprovedRecordCount.Text = Convert.ToString(Convert.ToInt32(lblApprovedRecordCount.Text) + lstRejectedTimesheet.Count);
                    }
                    else
                    {
                        try
                        {
                            foreach (Timesheet timesheet in lstRejectedTimesheet)
                            {
                                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendQualityRejectionNotification"]) && 
                                    workflowStatus == Convert.ToInt32(Constants.WorkflowStatus.QARejected))
                                    NotificationHelper.SendQualityRejectNotification(timesheet.ActivityId.Value, timesheet.SubActivityId.Value, timesheet.TimeSheetId, timesheet.Comments);
                                else if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendSiteManagerRejectionNotification"]) && 
                                    workflowStatus == Convert.ToInt32(Constants.WorkflowStatus.PMRejected))
                                    NotificationHelper.SendSiteManagerRejectionNotification(timesheet.TimeSheetId);
                            }
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                        }

                        if (workflowStatus == Convert.ToInt32(Constants.WorkflowStatus.PMRejected) || workflowStatus == Convert.ToInt32(Constants.WorkflowStatus.QARejected))
                            radNotificationMessage.Text = "Timesheet rejected successfully.";                       

                        Label lblRejectedRecordCount = tabToDoList.Tabs[03].FindControl("lblRejectedRecordCount") as Label;
                        lblRejectedRecordCount.Text = Convert.ToString(Convert.ToInt32(lblRejectedRecordCount.Text) + lstRejectedTimesheet.Count);
                    }

                    gridPendingTimesheet.Rebind();
                }
                else
                {
                    radNotificationMessage.Visible = true;
                    radNotificationMessage.Text = "Failed to update timesheet.";
                }
            }
            else
            {
                radNotificationMessage.Visible = true;
                radNotificationMessage.Text = "Timesheet not selected.";
            }
        }

        private void SetPendigTimesheetColumnVisibility()
        {
            if (Session["ProfileName"].ToString() == Constants.CONST_SITE_QUALITY_MANAGER
                                                    || Session["ProfileName"].ToString() == Constants.CONST_SITE_FUNCTIONAL_MANAGER
                                                    || Session["ProfileName"].ToString() == Constants.CONST_DESIGN_MANAGER)
            {
                gridPendingTimesheet.MasterTableView.GetColumn("CheckboxColumn").Display = divActionButtons.Visible = true;
                gridPendingTimesheet.MasterTableView.GetColumn("CommentColumn").Display = divActionButtons.Visible = true;
                //gridTimesheetView.MasterTableView.GetColumn("EditTimesheet").Display = false;

            }
            else
            {
                gridPendingTimesheet.MasterTableView.GetColumn("CheckboxColumn").Display = divActionButtons.Visible = false;
                gridPendingTimesheet.MasterTableView.GetColumn("CommentColumn").Display = divActionButtons.Visible = false;
                gridPendingTimesheet.MasterTableView.GetColumn("ViewTimesheetPartialApprove").Display = false;
                //gridTimesheetView.MasterTableView.GetColumn("EditTimesheet").Display = true;
            }
        }

        private void CheckPageAccess()
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);

            if (!PageSecurity.IsAccessGranted(PageSecurity.TODOTASK, menuList)) Response.Redirect("~/webNoAccess.aspx");
        }

        #endregion
    }
}
