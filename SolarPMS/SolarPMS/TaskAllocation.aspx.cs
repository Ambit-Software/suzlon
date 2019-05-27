using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class TaskAllocation : Page
    {
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
                    Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                    if (menuList == null) Response.Redirect("~/Login.aspx", false);

                    if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");
                    if (!IsPostBack)
                    {
                        InitializeControls();
                        GetAllUsers();
                    }

                    radNotificationMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Save task details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveTaskDetailsForUser();
            }
            catch (Exception ex)
            {
                radNotificationMessage.Visible = true;
                radNotificationMessage.Text = "Failed to save records.";
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///  Fill project dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Show selected data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridTaskAllocation_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    int taskAllocationId = 0;
                    CheckBox chkParentSelected = null;

                    // If not parent grid i.e. Area
                    if (item.OwnerTableView.Name != "")
                        chkParentSelected = item.OwnerTableView.ParentItem.FindControl("chkSelect") as CheckBox;

                    TaskAllocationMasterData dataItem = (TaskAllocationMasterData)item.DataItem;
                    bool isSelected = Convert.ToBoolean(dataItem.IsSelected);
                    taskAllocationId = Convert.ToInt32(dataItem.TaskAllocationId);

                    CheckBox chkSelect = item.FindControl("chkSelect") as CheckBox;
                    if (chkSelect != null)
                    {
                        chkSelect.Checked = isSelected || (taskAllocationId == 0 && (chkParentSelected != null && chkParentSelected.Checked));
                        chkSelect.Enabled = chkSelect.Visible = Convert.ToBoolean(dataItem.IsEnabled);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// On expand get data for child - Network, Activity, Subactivity
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gridTaskAllocation_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            try
            {
                GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
                string selectedId = Convert.ToString(dataItem.GetDataKeyValue("Id"));
                int userId = Convert.ToInt32(ddlUser.SelectedValue);
                string siteId = ddlSapSite.SelectedValue.Trim();
                string projectId = ddlSapProjects.SelectedValue.Trim();

                BindDataToChildGrid(e.DetailTableView.Name, dataItem, selectedId, userId, e.DetailTableView, siteId, projectId);
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
                BindTaskAllocationDataToGrid();
                GetUsersToCopy();
                divButtons.Visible = true;
                gridTaskAllocation.Visible = true;
                radNotificationMessage.Visible = false;
                cmbManager.ClearCheckedItems();
                cmbProjectHead.ClearCheckedItems();
                cmbUser.ClearCheckedItems();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Fill users dropdwn for copying task allocation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void ddlUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GetUsersToCopy();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Copy task allocation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopyTask_Click(object sender, EventArgs e)
        {
            try
            {
                CopyTaskAllocation();
            }
            catch (Exception ex)
            {
                radNotificationMessage.Visible = true;
                radNotificationMessage.Text = "Failed to copy task allocation.";
                GetUsersToCopy();
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion

        #region "Private Methods"

        private void GetAllUsers()
        {
            TaskModel taskModel = new TaskModel();
            List<Models.ListItem> users = taskModel.GetAllUsers();
            ddlUser.DataSource = users;
            ddlUser.DataTextField = "Name";
            ddlUser.DataValueField = "Id";
            ddlUser.EmptyMessage = Constants.CONST_SELECT_TEXT;
            ddlUser.DataBind();
        }

        private void InitializeControls()
        {
            TaskModel taskModel = new TaskModel();
            List<Models.ListItem> lstSite = taskModel.GetAllSite();
            int siteCount = lstSite.Count;
            if (siteCount < 1 || siteCount > 1)
                ddlSapSite.EmptyMessage = Constants.CONST_SELECT_TEXT;
            ddlSapSite.DataSource = lstSite;
            ddlSapSite.DataTextField = "Name";
            ddlSapSite.DataValueField = "Id";
            ddlSapSite.DataBind();

            if (siteCount == 1)
                FillSapProjectDropdown();
            else
                ddlSapProjects.EmptyMessage = Constants.CONST_SELECT_TEXT;
        }

        /// <summary>
        /// Show hide copy button according to edit mode - new/ edit.
        /// Only in edit copy button is visible.
        /// </summary>
        /// <param name="lstTaskAllocationMasterData"></param>
        private void ShowHideCopyButton(List<TaskAllocationMasterData> lstTaskAllocationMasterData)
        {
            if (lstTaskAllocationMasterData != null && lstTaskAllocationMasterData.Count > 0 && lstTaskAllocationMasterData[0].TaskAllocationId != 0)
            {
                btnCopyPopup.Visible = true;
                hidEditMode.Value = "Edit";
            }
            else
            {
                btnCopyPopup.Visible = false;
                hidEditMode.Value = "New";
            }
        }
        private void SaveTaskDetailsForUser()
        {
            List<TaskAllocationData> lstSelectedTask = new List<TaskAllocationData>();
            TaskAllocationData taskAllocation = null;
            string areaId = "0";
            string networkId = string.Empty;
            string activityId = string.Empty;
            string subactivityId = "0";
            int uniqueId = 0;

            bool isAreaDeleted = false;
            bool isNetworkDeleted = false;
            bool isActivityDeleted = false;
            bool isSubactivityDeleted = false;
            bool isAlreadySaved = false;

            foreach (GridDataItem area in gridTaskAllocation.MasterTableView.Items)
            {
                var isAreaSelected = false;
                isAreaSelected = ((CheckBox)area.FindControl("chkSelect")).Checked;
                areaId = Convert.ToString(area.GetDataKeyValue("Id"));
                // promary key of table.
                uniqueId = Convert.ToInt32(area.GetDataKeyValue("UniqueId"));
                // checkbox is unchecke and taskallocation is done i.e. in edit mode.
                isAreaDeleted = !isAreaSelected && uniqueId != 0;
                if (uniqueId != 0)
                    isAlreadySaved = true;
                GridDataItemCollection networkList = area.ChildItem.NestedTableViews[0].Items;
                // If parent i.e area is exapnded and data is bound to child grid.
                if (isAreaSelected && networkList.Count > 0)
                {
                    
                    foreach (GridDataItem network in networkList)
                    {
                        var isNetwokSelected = ((CheckBox)network.FindControl("chkSelect")).Checked;
                        networkId = Convert.ToString(network.GetDataKeyValue("Id"));
                        uniqueId = Convert.ToInt32(network.GetDataKeyValue("UniqueId"));
                        isNetworkDeleted = !isNetwokSelected && uniqueId != 0;

                        GridDataItemCollection activityList = network.ChildItem.NestedTableViews[0].Items;

                        if (isNetwokSelected && activityList.Count > 0)
                        {
                            
                            foreach (GridDataItem activity in activityList)
                            {
                                var isActivitySelected = ((CheckBox)activity.FindControl("chkSelect")).Checked;
                                activityId = Convert.ToString(activity.GetDataKeyValue("Id"));
                                uniqueId = Convert.ToInt32(activity.GetDataKeyValue("UniqueId"));
                                isActivityDeleted = !isActivitySelected && uniqueId != 0;

                                GridDataItemCollection subactivityList = activity.ChildItem.NestedTableViews[0].Items;
                                subactivityId = "0";

                                if (isActivitySelected && subactivityList.Count > 0)
                                {
                                    
                                    foreach (GridDataItem subactivity in subactivityList)
                                    {
                                        var isSubActivitySelected = ((CheckBox)subactivity.FindControl("chkSelect")).Checked;

                                        uniqueId = Convert.ToInt32(subactivity.GetDataKeyValue("UniqueId"));
                                        isSubactivityDeleted = !isSubActivitySelected && uniqueId != 0;
                                        subactivityId = Convert.ToString(subactivity.GetDataKeyValue("Id"));

                                        if ((isSubActivitySelected && uniqueId == 0) || isSubactivityDeleted)
                                        {
                                            taskAllocation = PopulateTaskAllocationDetails(areaId, networkId, activityId, subactivityId, isSubActivitySelected, isSubactivityDeleted, uniqueId);
                                            lstSelectedTask.Add(taskAllocation);
                                        }
                                    }
                                }
                                else
                                {
                                    if ((isActivitySelected && uniqueId == 0) || isActivityDeleted)
                                    {
                                        subactivityId = "0";
                                        taskAllocation = PopulateTaskAllocationDetails(areaId, networkId, activityId, subactivityId, isActivitySelected, isActivityDeleted, uniqueId);
                                        lstSelectedTask.Add(taskAllocation);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if ((isNetwokSelected && uniqueId == 0) || isNetworkDeleted)
                            {
                                activityId = subactivityId = "0";
                                taskAllocation = PopulateTaskAllocationDetails(areaId, networkId, activityId, subactivityId, isNetwokSelected, isNetworkDeleted, uniqueId);
                                lstSelectedTask.Add(taskAllocation);
                            }
                        }
                    }
                }
                else
                {
                    // add to list if either area selected or deleted. dont do anything if data is not changed.
                    if ((isAreaSelected && uniqueId == 0) || isAreaDeleted)
                    {
                        networkId = activityId = subactivityId = "0";
                        taskAllocation = PopulateTaskAllocationDetails(areaId, networkId, activityId, subactivityId, isAreaSelected, isAreaDeleted, uniqueId);
                        lstSelectedTask.Add(taskAllocation);
                    }
                }
            }

            if (lstSelectedTask.Count > 0)
            {
                TaskModel taskModel = new TaskModel();
                int userId = Convert.ToInt32(ddlUser.SelectedValue);
                string siteId = ddlSapSite.SelectedValue.Trim();
                string projectId = ddlSapProjects.SelectedValue.Trim();
                int loggedInUserId = Convert.ToInt32(Session["UserId"]);

                taskModel.SaveTaskDetails(lstSelectedTask, userId, siteId, projectId, loggedInUserId);
                BindTaskAllocationDataToGrid();
                radNotificationMessage.Text = "Task allocated/removed successfully.";
                radNotificationMessage.Visible = true;

                SendNotificationToUser(userId);
            }
            else if(isAlreadySaved)
            {
                radNotificationMessage.Text = "Task allocation already saved.";
                radNotificationMessage.Visible = true;                
            }
            else
            {
                radNotificationMessage.Text = "Record is not selected.";
                radNotificationMessage.Visible = true;
            }
        }

        private void SendNotificationToUser(int UserId, string UserIds = "")
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendActivityAllocationNotification"]))
                if (UserId != 0 && string.IsNullOrEmpty(UserIds))
                    NotificationHelper.SendTaskAllocationNotification(UserId);
                else
                {
                    int[] userIdArray = Array.ConvertAll<string, int>(UserIds.Split(';'), int.Parse);
                    for (int count = 0; count < userIdArray.Count(); count++)
                        NotificationHelper.SendTaskAllocationNotification(userIdArray[0]);
                }
        }

        /// <summary>
        /// Bind task allocation data to grid
        /// </summary>
        /// <param name="taskModel"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <param name="projectId"></param>
        private void BindTaskAllocationDataToGrid()
        {
            TaskModel taskModel = new TaskModel();
            int userId = Convert.ToInt32(ddlUser.SelectedValue);
            string siteId = ddlSapSite.SelectedValue.Trim();
            string projectId = ddlSapProjects.SelectedValue.Trim();
            List<TaskAllocationMasterData> lstTaskAllocationMasterData = taskModel.GetTaskAllocationMasterData(projectId, siteId, userId, Convert.ToInt32(Constants.TASKMaster.Area));
            gridTaskAllocation.DataSource = lstTaskAllocationMasterData;
            gridTaskAllocation.DataBind();
            ShowHideCopyButton(lstTaskAllocationMasterData);
        }

        /// <summary>
        /// Fill SAP project dropdown
        /// </summary>
        private void FillSapProjectDropdown()
        {
            TaskModel taskModel = new TaskModel();
            string siteId = ddlSapSite.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(siteId))
            {
                List<Models.ListItem> lstProjects = taskModel.GetAllProjects(siteId);
                ddlSapProjects.ClearSelection();
                ddlSapProjects.Items.Clear();
                ddlSapProjects.DataSource = lstProjects;
                ddlSapProjects.DataTextField = "Name";
                ddlSapProjects.DataValueField = "Id";
                if (lstProjects.Count > 1 || lstProjects.Count < 1)
                {
                    ddlSapProjects.EmptyMessage = Constants.CONST_SELECT_TEXT;
                    ddlSapProjects.SelectedIndex = -1;
                }
                else
                {
                    ddlSapProjects.SelectedValue = lstProjects[0].Id;
                    ddlSapProjects.SelectedIndex = 0;
                }
            }
            else
            {
                ddlSapProjects.EmptyMessage = Constants.CONST_SELECT_TEXT;
                ddlSapProjects.ClearSelection();
                ddlSapProjects.Items.Clear();
            }

            ddlSapProjects.DataBind();
        }

        /// <summary>
        /// On expand get data for child - Network, Activity, Subactivity
        /// </summary>
        /// <param name="childGridName"></param>
        /// <param name="dataItem"></param>
        /// <param name="selectedId"></param>
        /// <param name="userId"></param>
        /// <param name="detailTableView"></param>
        /// <param name="siteId"></param>
        /// <param name="projectId"></param>
        /// <param name="taskModel"></param>
        private static void BindDataToChildGrid(string childGridName, GridDataItem dataItem, string selectedId, int userId,
                                                GridTableView detailTableView, string siteId, string projectId)
        {
            TaskModel taskModel = new TaskModel();
            switch (childGridName)
            {
                case "Network":
                    {
                        detailTableView.DataSource = taskModel.GetTaskAllocationMasterData(projectId, siteId, userId, Convert.ToInt32(Constants.TASKMaster.Network), selectedId);
                        break;
                    }
                case "Activity":
                    {
                        string areaId = Convert.ToString(dataItem.OwnerTableView.ParentItem.GetDataKeyValue("Id"));
                        detailTableView.DataSource = taskModel.GetTaskAllocationMasterData(projectId, siteId, userId, Convert.ToInt32(Constants.TASKMaster.Activity), areaId, selectedId);
                        break;
                    }
                case "SubActivity":
                    {
                        string networkId = Convert.ToString(dataItem.OwnerTableView.ParentItem.GetDataKeyValue("Id"));
                        string areaId = Convert.ToString(dataItem.OwnerTableView.ParentItem.OwnerTableView.ParentItem.GetDataKeyValue("Id"));
                        detailTableView.DataSource = taskModel.GetTaskAllocationMasterData(projectId, siteId, userId, Convert.ToInt32(Constants.TASKMaster.SubActivity), areaId, networkId, selectedId);
                        break;
                    }
            }
        }
        private static TaskAllocationData PopulateTaskAllocationDetails(string areaId, string networkId, string activityId,
                                                                      string subactivityId, bool isAllSelected, bool isDeleted, int uniqueId)
        {
            return new TaskAllocationData()
            {
                AreaId = areaId,
                NetworkId = networkId,
                ActivityId = activityId,
                SubActivityId = subactivityId,
                IsAllSelected = isAllSelected,
                IsDeleted = isDeleted,
                UniqueId = uniqueId
            };
        }
        /// <summary>
        /// Get user to fill dropdown to copy.
        /// </summary>
        private void GetUsersToCopy()
        {
            TaskModel taskModel = new TaskModel();
            int userId = Convert.ToInt32(ddlUser.SelectedValue);
            ProfileMaster profileMaster = taskModel.GetUserProfileDetails(userId);
            List<UserForCopy> lstUsers = taskModel.GetUsersForCopyTaskAllocation(userId);

            string managerType = profileMaster.ProfileName;

            // for ite functional manager and site functional user show both dropdown.
            // i.e userprofile and manager profile along with state head dropdown.
            // Ssme for design engineer and design manager
            // for Quality engineer only that dropdown will be visible.
            // state head dropdown will be always visible.
            if (profileMaster.ProfileName == Constants.CONST_SITE_FUNCTIONAL_USER)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setLabelOrAnyName",
                    "jQuery(function($) { $('#divManagerProfile').show();$('#divUserProfile').show();$('#lblManagerProfile').text('"
                                        + "Select " + Constants.CONST_SITE_FUNCTIONAL_MANAGER + "'); $('#lblUserProfile').text('" + "Select "
                                        + Constants.CONST_SITE_FUNCTIONAL_USER + "')});", true);
                managerType = Constants.CONST_SITE_FUNCTIONAL_MANAGER;

            }
            else if (profileMaster.ProfileName == Constants.CONST_DESIGN_ENGINEER)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setLabelOrAnyName",
                    "jQuery(function($) {  $('#divManagerProfile').show();$('#divUserProfile').show();$('#lblManagerProfile').text('" + "Select "
                    + Constants.CONST_DESIGN_MANAGER + "');$('#lblUserProfile').text('" + "Select " + Constants.CONST_DESIGN_ENGINEER + "')});", true);

                managerType = Constants.CONST_DESIGN_MANAGER;
            }
            else if (profileMaster.ProfileName == Constants.CONST_SITE_QUALITY_MANAGER)
            {
                managerType = Constants.CONST_SITE_QUALITY_MANAGER;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setLabelOrAnyName",
                "jQuery(function($) {$('#divManagerProfile').hide(); $('#lblUserProfile').text('" + "Select " + profileMaster.ProfileName + "')});", true);
            }
            else if (profileMaster.ProfileName == Constants.CONST_STATE_PROJECT_HEAD)
            {
                managerType = Constants.CONST_SITE_QUALITY_MANAGER;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setLabelOrAnyName",
                "jQuery(function($) {$('#divManagerProfile').hide(); $('#divUserProfile').hide();});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "setLabelOrAnyName",
                    "jQuery(function($) {$('#divManagerProfile').hide(); $('#divUserProfile').show();$('#lblUserProfile').text('" + "Select "
                    + profileMaster.ProfileName + "')});", true);

            }

            cmbProjectHead.DataSource = lstUsers.Where(l => l.Profile == Constants.CONST_STATE_PROJECT_HEAD).ToList();
            cmbUser.DataSource = lstUsers.Where(l => l.Profile == profileMaster.ProfileName).ToList();
            cmbManager.DataSource = lstUsers.Where(l => l.Profile == managerType).ToList();

            cmbManager.DataTextField = cmbUser.DataTextField = cmbProjectHead.DataTextField = "UserName";
            cmbManager.DataValueField = cmbUser.DataValueField = cmbProjectHead.DataValueField = "UserId";
            cmbManager.EmptyMessage = cmbUser.EmptyMessage = cmbProjectHead.EmptyMessage = Constants.CONST_SELECT_TEXT;
            cmbUser.DataBind();
            cmbManager.DataBind();
            cmbProjectHead.DataBind();
            //upnlMain.Update();
        }

        /// <summary>
        /// Copy task allocation for selected user.
        /// </summary>
        private void CopyTaskAllocation()
        {
            TaskModel taskModel = new TaskModel();
            int loggedInUserId = Convert.ToInt32(Session["UserId"]);
            int selectedUserId = Convert.ToInt32(ddlUser.SelectedValue);
            int selectedUserCount = cmbUser.CheckedItems.Count;
            int selectedManagerCount = cmbManager.CheckedItems.Count;
            int selectedHeadCount = cmbProjectHead.CheckedItems.Count;
            string siteId = ddlSapSite.SelectedValue.Trim();
            string projectId = ddlSapProjects.SelectedValue.Trim();
            string selectedUsers = string.Empty;

            // get selected user
            if (selectedUserCount > 0)
            {
                foreach (RadComboBoxItem user in cmbUser.CheckedItems)
                    selectedUsers += user.Value + ",";
            }

            if (selectedManagerCount > 0)
            {
                foreach (RadComboBoxItem user in cmbManager.CheckedItems)
                    selectedUsers += user.Value + ",";
            }

            if (selectedHeadCount > 0)
            {
                foreach (RadComboBoxItem user in cmbProjectHead.CheckedItems)
                    selectedUsers += user.Value + ",";
            }

            if (selectedUsers != string.Empty)
                taskModel.CopyTaskDetails(selectedUserId, selectedUsers, loggedInUserId, siteId, projectId);

            radNotificationMessage.Visible = true;
            radNotificationMessage.Text = "Task allocation copied successfully.";
            SendNotificationToUser(0, selectedUsers);
            cmbUser.ClearSelection();
            cmbManager.ClearSelection();
            cmbProjectHead.ClearSelection();
            //GetUsersToCopy();
            ScriptManager.RegisterStartupScript(upnl2, upnl2.GetType(), "hide", "$(function () { $('#" + panel1.ClientID + "').modal('hide'); });", true);
            upnl2.Update();
            upnlMain.Update();
        }
        #endregion

        protected void btnCopyPopup_Click(object sender, EventArgs e)
        {
            try
            {
                GetUsersToCopy();
                ScriptManager.RegisterStartupScript(upnl2, upnl2.GetType(), "show", "$(function () { $('#" + panel1.ClientID + "').modal('show'); });", true);
                upnl2.Update();
            }
            catch(Exception ex)
            {

            }
        }
    }
    /// <summary>
    /// This class is used to send data to task alloction usp
    /// </summary>
    public class TaskAllocationData
    {
        public string AreaId { get; set; }
        public string NetworkId { get; set; }
        public string ActivityId { get; set; }
        public string SubActivityId { get; set; }
        public bool IsAllSelected { get; set; }
        public bool IsDeleted { get; set; }
        public int UniqueId { get; set; }
    }

}
