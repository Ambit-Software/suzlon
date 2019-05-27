using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class DesignDocumentUpload : System.Web.UI.Page
    {
        int UserId = 0;
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
                    UserId = Convert.ToInt32(Session["UserId"]);
                    if (!IsPostBack)
                    {
                        
                        int pageAccess = Convert.ToInt32(Session[Constants.CONST_SESSION_DEDOCUMENT_ACEESS]);
                        if (pageAccess == Convert.ToInt32(Constants.DEDocumentUploadAccess.None))
                            Response.Redirect("~/webNoAccess.aspx");
                        else if (pageAccess != Convert.ToInt32(Constants.DEDocumentUploadAccess.Full))
                            gridActivityDetails.MasterTableView.GetColumn("AddDocument").Visible = false;

                        InitializeControls();
                    }
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
                string area = ddlArea.SelectedIndex != -1 ? ddlArea.SelectedValue.Trim() : string.Empty;
                string site = ddlSapSite.SelectedIndex != -1 ? ddlSapSite.SelectedValue.Trim() : string.Empty;
                string project = ddlSapProjects.SelectedIndex != -1 ? ddlSapProjects.SelectedValue.Trim() : string.Empty;
                string network = ddlNetwork.SelectedIndex != -1 ? ddlNetwork.SelectedValue.Trim() : string.Empty;
                BindActivityDetails(false, area, site, project, network);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridActivityDetails_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    Models.DesignEnggActivityDetails dataItem = (Models.DesignEnggActivityDetails)item.DataItem;
                    Button btnViewDocument = item.FindControl("btnViewDocument") as Button;

                    int activityId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ActivityId"]);
                    int subActivityId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubActivityId"]);
                    int rowIndex = e.Item.ItemIndex;
                    btnViewDocument.Attributes["onclick"] = String.Format("return viewDocument('{0}','{1}','{2}');", activityId, subActivityId, rowIndex);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddDocument_Click(object sender, EventArgs e)
        {
            try
            {
                Session[Constants.CONST_SESSION_UPLOAD_DOCUMENT_PARAMETER] = null;
                Session[Constants.CONST_SESSION_DEDOCUMENT_ISREVIEW] = null;
                Session["HighestVersion"] = null;
                Session[Constants.CONST_SESSION_UPLOAD_DOCUMENT_PARAMETER] = ((GridEditableItem)(((Control)sender).NamingContainer)).KeyValues;
                ajaxManagerDocumentUpload.ResponseScripts.Add("openRadWin('document');");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ajaxManagerDocumentUpload_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            try
            {
                string values = e.Argument;
                string[] parameters = values.Split('#');

                if (parameters[0] == "ViewDocument")
                {
                    Session[Constants.CONST_SESSION_ACTIVITY_ID] = Convert.ToInt32(parameters[1]);
                    Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = Convert.ToInt32(parameters[2]);
                    Session[Constants.CONST_SESSION_TIMESHEET_ID] = null;
                }

                switch (parameters[0])
                {
                    case "ViewDocument":
                        Session[Constants.CONST_SESSION_TIMESHEET_ID] = Convert.ToInt32(parameters[1]);
                        BindDesignDocument(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]));
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    HyperLink document = item["FileName"].Controls[0] as HyperLink;
                    DesignEngineerDocuments designEngineerDocuments = item.DataItem as DesignEngineerDocuments;
                    if (Convert.ToInt32(designEngineerDocuments.UserId) == UserId)
                        (item["DeleteColumn"].Controls[0]).Visible = true;
                    else
                        (item["DeleteColumn"].Controls[0]).Visible = false;

                    document.NavigateUrl = ConfigurationManager.AppSettings["MailUrl"] + "Upload/DEDocument/" + document.Text;
                    document.Target = "new";
                }
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
                BindNetwork("");
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
                BindActivityDetails(true, ddlArea.SelectedValue.Trim(), ddlSapSite.SelectedValue.Trim(), ddlSapProjects.SelectedValue.Trim(), ddlNetwork.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (gridDocument.Items.Count > 0)
                {
                    int ActivityId = Session[Constants.CONST_SESSION_ACTIVITY_ID] != null ? Convert.ToInt32(Session[Constants.CONST_SESSION_ACTIVITY_ID]) : 0;
                    int SubActivityId = Session[Constants.CONST_SESSION_SUBACTIVITY_ID] != null ? Convert.ToInt32(Session[Constants.CONST_SESSION_SUBACTIVITY_ID]) : 0;

                    if (ActivityId != 0 || SubActivityId != 0)
                        BindDesignDocument(ActivityId, SubActivityId, false);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnReviewDocument_Click(object sender, EventArgs e)
        {
            try
            {
                Session[Constants.CONST_SESSION_DEDOCUMENT_ISREVIEW] = true;
                Session[Constants.CONST_SESSION_UPLOAD_DOCUMENT_PARAMETER] = ((GridEditableItem)(((Control)sender).NamingContainer)).KeyValues;
                ajaxManagerDocumentUpload.ResponseScripts.Add("openRadWin('document');");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridActivityDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    Button btnReviewDocument = item.FindControl("btnReviewDocument") as Button;
                    DesignEnggActivityDetails designEnggActivityDetails = item.DataItem as DesignEnggActivityDetails;
                    if (designEnggActivityDetails.Version <= 0)
                        btnReviewDocument.Visible = false;

                    Label lblReleaseToContsruction = (Label)item.FindControl("lblReleaseToContsruction");
                    string text = lblReleaseToContsruction.Text;
                    lblReleaseToContsruction.Text = text == "True" ? "Y" : "N";
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem item = (GridDataItem)e.Item;
                int DocumentDetailsId = Convert.ToInt32(item.GetDataKeyValue("DocumentDetailsId"));
                int Id = Convert.ToInt32(item.GetDataKeyValue("Id"));
                string FileName = Convert.ToString(item.GetDataKeyValue("FileName"));

                CopyFileToMainFolder(FileName);
                TaskModel.DeleteDEDocument(Id, DocumentDetailsId, UserId);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        #endregion

        #region "Method"
        private void InitializeControls()
        {
            BindSiteData();
            ddlArea.EmptyMessage = Constants.CONST_ALL_TEXT;
            ddlNetwork.EmptyMessage = Constants.CONST_ALL_TEXT;
        }

        private void BindDesignDocument(int ActivityId, int SubactivityId, bool IsDataBind = true)
        {
            gridDocument.DataSource = TaskModel.GetDesignEngineerDocuments(ActivityId, SubactivityId, UserId);
            if (IsDataBind)
            {
                gridDocument.CurrentPageIndex = 0;
                gridDocument.DataBind();
            }
        }

        private void BindActivityDetails(bool IsDataBind, string Area, string SapSite, string Project, string Network)
        {
            TaskModel taskModel = new TaskModel();
            if (SapSite == Constants.CONST_ALL_TEXT)
                SapSite = string.Empty;
            if (Area == Constants.CONST_ALL_TEXT)
                Area = string.Empty;
            if (Project == Constants.CONST_ALL_TEXT)
                Project = string.Empty;
            if (Network == Constants.CONST_ALL_TEXT)
                Network = string.Empty;

            List<Models.DesignEnggActivityDetails> lstTimesheet = taskModel.GetDesignEngggActivityRecords(UserId, Area, SapSite, Project, Network);
            gridActivityDetails.DataSource = lstTimesheet;
            if (IsDataBind)
                gridActivityDetails.DataBind();
        }
        
        private void BindSiteData()
        {
            TaskModel taskModel = new TaskModel();
            List<UserAssignedTaskDataForDropdown> lstSite = CommonFunctions.GetUserTaskDropdownDataForReport(UserId, string.Empty, string.Empty, string.Empty, "site", false, true);
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
            if (!string.IsNullOrEmpty(siteId))
            {
                List<UserAssignedTaskDataForDropdown> lstProjects = CommonFunctions.GetUserTaskDropdownDataForReport(UserId, siteId,
                                                                                                                    string.Empty, string.Empty, "project", false, true);
                ddlSapProjects.ClearSelection();
                ddlSapProjects.Items.Clear();
                ddlArea.ClearSelection();
                ddlArea.Items.Clear();
                ddlNetwork.ClearSelection();
                ddlNetwork.Items.Clear();
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
            CommonFunctions commonFunctions = new CommonFunctions();
            List<UserAssignedTaskDataForDropdown> lstProjects = CommonFunctions.GetUserTaskDropdownDataForReport(UserId, ddlSapSite.SelectedItem.Text.Trim(),
                                                                  ddlSapProjects.SelectedValue.Trim(), string.Empty, "area", false, true);
            ddlArea.ClearSelection();
            ddlArea.Items.Clear();
            ddlNetwork.ClearSelection();
            ddlNetwork.Items.Clear();
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
            List<UserAssignedTaskDataForDropdown> lstNetwork = CommonFunctions.GetUserTaskDropdownDataForReport(UserId, ddlSapSite.SelectedItem.Text.Trim(),
                                                                      ddlSapProjects.SelectedValue.Trim(), area, "network", false, true);
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

        private void CopyFileToMainFolder(string FileName)
        {
            string sourcePath = Server.MapPath(Constants.DESIGN_ENGINEER_DOC_PATH);
            string targetPath = Server.MapPath(Constants.DESIGN_ENGINEER_DELETED_DOC_PATH);

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, FileName);
            string destFile = System.IO.Path.Combine(targetPath, FileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);
        }
        #endregion
    }
}
