using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SolarPMS.Models.TaskModel;
using System.Data;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class TaskAllocationDetails : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown(drpSite);
                grdAllocated.DataSource = new DataTable();
                grdAllocated.DataBind();
                grdNotAllocated.DataSource = new DataTable();
                grdNotAllocated.DataBind();
                grdActivityDeleted.DataSource = new DataTable();
                grdActivityDeleted.DataBind();
            }
        }

        public void bindProjDropDown(RadComboBox drpProj, string siteId)
        {
            string result = commonFunctions.RestServiceCall(string.Format(Constants.TABLE_GET_PROJECTBYSITE, Convert.ToString(siteId)), string.Empty);
            drpProj.ClearSelection();
            drpProj.Items.Clear();
            if (!string.IsNullOrEmpty(result))
            {
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                drpProj.DataTextField = "Name";
                drpProj.DataValueField = "Id";
                drpProj.DataSource = ddValues.project;
                drpProj.DataBind();
                //drpProj.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Project", String.Empty));
                //drpProj.Items.Insert(0, new RadComboBoxItem() { Text = "Select Project", Value = string.Empty });
                drpProj.EmptyMessage = "Select Project";
                //drpProj.SelectedIndex = 0;
            }
            else
            {
                drpProj.DataSource = new DataTable();
                //drpProj.Items.Insert(0, new System.Web.UI.WebControls.ListItem() { Text = "Select Project", Value = string.Empty });
                drpProj.EmptyMessage = "Select Project";
                drpProj.DataBind();
            }
        }

        private void BindDropDown(RadComboBox drpSite)
        {
            string drpname = "site";
            string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                drpSite.DataTextField = "Name";
                drpSite.DataValueField = "Id";
                drpSite.DataSource = ddValues.site;
                drpSite.DataBind();
                //drpSite.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Site", String.Empty));
                //drpSite.Items.Insert(0, new RadComboBoxItem() { Text = "Select Site", Value = string.Empty });
                drpSite.EmptyMessage = "Select Site";
                //drpSite.SelectedIndex = 0;
            }
            else
            {
                drpSite.DataSource = new DataTable();
                drpSite.EmptyMessage = "Select Site";
                drpSite.DataBind();
            }
        }

        protected void drpSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(drpSite.SelectedValue))
                bindProjDropDown(drpProject, drpSite.SelectedValue);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpSite.SelectedValue != "" && drpProject.SelectedValue != "")
                {
                    bindgrdAllocated(Convert.ToString(drpProject.SelectedValue), Convert.ToString(drpSite.SelectedValue));
                    grdAllocated.DataBind();

                    bindNotAllocatedActivity(Convert.ToString(drpProject.SelectedValue), Convert.ToString(drpSite.SelectedValue));
                    grdNotAllocated.DataBind();

                    bindDeletedActivity(Convert.ToString(drpProject.SelectedValue), Convert.ToString(drpSite.SelectedValue));
                    grdActivityDeleted.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void bindgrdAllocated(string projectId, string sapSite)
        {
            string getAllAssignedActivity = commonFunctions.RestServiceCall(string.Format(Constants.GET_TASK_ALLOCATION_DETAIL, projectId, sapSite), string.Empty);
            if (!string.IsNullOrEmpty(getAllAssignedActivity))
            {
                List<TaskAllocatedDetail> lstIssueCategory = JsonConvert.DeserializeObject<List<TaskAllocatedDetail>>(getAllAssignedActivity);
                grdAllocated.DataSource = lstIssueCategory;

            }
            else
            {
                grdAllocated.DataSource = new DataTable();
            }

        }

        protected void grdAllocated_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(drpProject.SelectedValue) && !string.IsNullOrEmpty(drpSite.SelectedValue))
                bindgrdAllocated(Convert.ToString(drpProject.SelectedValue), Convert.ToString(drpSite.SelectedValue));
        }


        private void bindNotAllocatedActivity(string projectId, string sapSite)
        {
           
            string getNotAllocatedActivity = commonFunctions.RestServiceCall(string.Format(Constants.GET_NOT_ALLOCATED_TASK, projectId, sapSite), string.Empty);
            if (!string.IsNullOrEmpty(getNotAllocatedActivity))
            {
                List<SAPActivitiesNotAllocated> lstNotAllocatedActivity = JsonConvert.DeserializeObject<List<SAPActivitiesNotAllocated>>(getNotAllocatedActivity);
                grdNotAllocated.DataSource = lstNotAllocatedActivity;

            }
            else
            {
                grdNotAllocated.DataSource = new DataTable();
            }
         
        }

        protected void grdNotAllocated_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(drpProject.SelectedValue) && !string.IsNullOrEmpty(drpSite.SelectedValue))
                bindNotAllocatedActivity(Convert.ToString(drpProject.SelectedValue), Convert.ToString(drpSite.SelectedValue));
        }

        private void bindDeletedActivity(string projectId, string sapSite)
        {
            List<ActivityDeleted> lstNotAll = new List<ActivityDeleted>();
            lstNotAll.Add(new ActivityDeleted()
            {
                SAPSite = "Test",
                ProjectDescription = "desc",
                WBSArea = "Area",
                SAPNetWork = "Networ",
                SAPActivity = "Activity",
                SAPSubActivity = "SubActivity",
                EstEndtDate = DateTime.Now,
                EstStartDate = DateTime.Now,
                Quantity = "100",
                UOM = "100",
                SiteFunctionalUser = "User1",
                SiteFunctionalManager = "User2",
                SiteQualityManager = "User3",
                StateProjectHead = "User4",
                DesignEngineer = "User5",
                DesignManager = "User6",

            });
            lstNotAll.Add(new ActivityDeleted()
            {
                SAPSite = "Test2",
                ProjectDescription = "desc2",
                WBSArea = "Area2",
                SAPNetWork = "Networ2",
                SAPActivity = "Activity2",
                SAPSubActivity = "SubActivity2",
                EstEndtDate = DateTime.Now,
                EstStartDate = DateTime.Now,
                Quantity = "100",
                UOM = "100",
                SiteFunctionalUser = "User1",
                SiteFunctionalManager = "User2",
                SiteQualityManager = "User3",
                StateProjectHead = "User4",
                DesignEngineer = "User5",
                DesignManager = "User6",
            });

            grdActivityDeleted.DataSource = lstNotAll;
        }

        protected void grdActivityDeleted_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!string.IsNullOrEmpty(drpProject.SelectedValue) && !string.IsNullOrEmpty(drpSite.SelectedValue))
                bindDeletedActivity(Convert.ToString(drpProject.SelectedValue), Convert.ToString(drpSite.SelectedValue));
        }

        protected void grdAllocated_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                //GridDataItem dataItem = (GridDataItem)e.Item;

                //HyperLink editLink = (HyperLink)e.Item.FindControl("ViewHistory");
                //editLink.Attributes["href"] = "javascript:void(0);";

                //editLink.Attributes["onclick"] = String.Format("return ShowAssignHistory('{0}','{1}','{2}','{3}','{4}','{5}','{6}');",
                //    e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["sapSite"],
                //    dataItem["Project"].Text, dataItem["Area"].Text,
                //    dataItem["Network"].Text, dataItem["Activities"].Text,
                //     dataItem["SubActivities"].Text, e.Item.ItemIndex);

            }
        }

        protected void grdActivityDeleted_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                //HyperLink viewLink = (HyperLink)e.Item.FindControl("ViewHistory");
                //viewLink.Attributes["href"] = "javascript:void(0);";
                //viewLink.Attributes["onclick"] = String.Format("return ShowAssignHistory('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["sapSite"], e.Item.ItemIndex);
              
                HyperLink timeSheetLink = (HyperLink)e.Item.FindControl("ViewTimesheet");
                timeSheetLink.Attributes["href"] = "javascript:void(0);";
                timeSheetLink.Attributes["onclick"] = String.Format("return ShowTimeSheet('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["sapSite"], e.Item.ItemIndex);

                HyperLink issueLink = (HyperLink)e.Item.FindControl("ViewIssue");
                issueLink.Attributes["href"] = "javascript:void(0);";
                issueLink.Attributes["onclick"] = String.Format("return ShowActivityIssue('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["sapSite"], e.Item.ItemIndex);
            }
        }

        protected void btnExportAssignTask_Click(object sender, EventArgs e)
        {
            try
            {
                //grdAllocated.ExportSettings.ExportOnlyData = true;
                //grdAllocated.ExportSettings.IgnorePaging = true;
                //grdAllocated.ExportSettings.OpenInNewWindow = true;
                //grdAllocated.ExportSettings.FileName = "Activity Allocatted";
                //grdAllocated.MasterTableView.ExportToExcel();
                grdAllocated.MasterTableView.AllowFilteringByColumn = false;
                grdAllocated.MasterTableView.AllowSorting = false;
                grdAllocated.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                grdAllocated.ExportSettings.FileName = "Activity Allocatted";
                grdAllocated.Rebind();
                grdAllocated.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnExportNotAssign_Click(object sender, EventArgs e)
        {
            try
            {
                //grdNotAllocated.ExportSettings.ExportOnlyData = true;
                //grdNotAllocated.ExportSettings.IgnorePaging = true;
                //grdNotAllocated.ExportSettings.OpenInNewWindow = true;
                //grdNotAllocated.ExportSettings.FileName = "Activity Not Allocatted";
                //grdNotAllocated.MasterTableView.ExportToExcel();
                grdNotAllocated.MasterTableView.AllowFilteringByColumn = false;
                grdNotAllocated.MasterTableView.AllowSorting = false;
                grdNotAllocated.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                grdNotAllocated.ExportSettings.FileName = "Activity Not Allocatted";
                grdNotAllocated.Rebind();
                grdNotAllocated.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnExportDeleteActivity_Click(object sender, EventArgs e)
        {
            try
            {
                grdActivityDeleted.ExportSettings.ExportOnlyData = true;
                grdActivityDeleted.ExportSettings.IgnorePaging = true;
                grdActivityDeleted.ExportSettings.OpenInNewWindow = true;
                grdActivityDeleted.ExportSettings.FileName = "Activity Deleted";
                grdActivityDeleted.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            string values = e.Argument;
            string[] parameters = values.Split('#');

            if (e.Argument.Contains("AssignHistory"))
            {
                bindAssignHistory(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
                grdAssignHistory.DataBind();
            }
        }

        public void bindAssignHistory(string sapSiteId, string sapProjId, string sapArea, string sapNetwork, string sapActivity, string subActivity)
        {
            List<AssignHistory> lstHistory = new List<AssignHistory>();
            lstHistory.Add(new AssignHistory()
            {
                ProfileName="Test",
                UserName="User1",
                Status="Add",
                ModifyDate=DateTime.Now

            });
            lstHistory.Add(new AssignHistory()
            {
                ProfileName = "Test2",
                UserName = "User2",
                Status = "Update",
                ModifyDate = DateTime.Now
            });

            grdAssignHistory.DataSource = lstHistory;
            grdAssignHistory.DataBind();
        }
    }

    public class AssignHistory
    {
        public string ProfileName { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}