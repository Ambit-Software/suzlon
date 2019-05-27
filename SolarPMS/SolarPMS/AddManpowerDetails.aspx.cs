using Cryptography;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class AddManpowerDetails : System.Web.UI.Page
    {
        #region "Data Member"
        public string Site = string.Empty;
        public string Project = string.Empty;
        public int Area = 0;
        public string Network = string.Empty;
        public DateTime Date = DateTime.Now;

        #endregion

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string queryString = Crypto.Instance.Decrypt(HttpUtility.UrlDecode(Request.QueryString.ToString()));
                string[] queryStringValues = queryString.Split('&');
                if (queryStringValues.Length > 2)
                {
                    Site = queryStringValues[0].Split('=')[1];
                    Project = queryStringValues[1].Split('=')[1]; ;
                    Area = Convert.ToInt32(queryStringValues[2].Split('=')[1]);
                    Network = queryStringValues[3].Split('=')[1];
                    Date = Convert.ToDateTime(queryStringValues[4].Split('=')[1]);
                }
                
                dtDetailsDate.MaxDate = DateTime.Now;
                if (!IsPostBack)
                {
                    if (Site != string.Empty)
                    {
                        lblIssueMode.Text = "Edit Details";
                        SetControlsEditMode();
                    }
                    else
                    {
                        dtDetailsDate.MinDate = DateTime.Now.AddDays(-7);
                        InitializeControls();                        
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlSapSite_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                FillSapProjectDropdown();
                FillContractorDropdown();
                gridManPowerDetails.MasterTableView.ClearEditItems();
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlSapProjects_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                FillSapAreaDropdown();
                FillContractorDropdown();
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                FillSapNetworkDropdown();
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/ContractorManagement.aspx");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPowerDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (ddlSapSite.SelectedIndex != -1)
                    Site = ddlSapSite.SelectedValue.Trim();
                if (ddlSapProjects.SelectedIndex != -1)
                    Project = ddlSapProjects.SelectedValue.Trim();
                if (ddlArea.SelectedIndex != -1)
                    Area = Convert.ToInt32(ddlArea.SelectedValue.Trim());
                if (ddlNetwork.SelectedIndex != -1)
                    Network = ddlNetwork.SelectedValue.Trim();
                Date = dtDetailsDate.SelectedDate.Value;
                gridManPowerDetails.DataSource = ManPowerModel.GetContractorDetails(Site, Project, Area, Network, Date, Convert.ToInt32(Session["UserId"]));
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void gridManPowerDetails_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveManPowerDetails(Constants.CONST_NEW_MODE, e);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPowerDetails_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveManPowerDetails(Constants.CONST_EDIT_MODE, e);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPowerDetails_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }

        protected void gridManPowerDetails_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {

                    string SiteId = string.Empty;
                    string VillageId = string.Empty;
                    string projId = string.Empty;

                    GridDataItem item = e.Item as GridDataItem;
                    GridEditableItem editItem = (GridEditableItem)e.Item;
                    RadDropDownList ddlContractor = (RadDropDownList)editItem.FindControl("ddlContractor");
                    RadDropDownList drpShift = (RadDropDownList)editItem.FindControl("drpShift");
                    Label lblShift = item["Shift"].FindControl("lblShift") as Label;
                    Label contractorId = item["Contractor"].FindControl("lblContractorId") as Label;
                    RadTextBox txtLaborCount = item["UnskilledLabourCount"].FindControl("txtLaborCount") as RadTextBox;
                    RadTextBox txtBlockNumbers = item["BlockNumbers"].FindControl("txtBlockNumbers") as RadTextBox;
                    RadTextBox txtComments = item["Comments"].FindControl("txtComments") as RadTextBox;
                    //Label lblLaborCount = item["LabourCount"].FindControl("txtComments") as Label;
                    ContractorLaborDetails dataItem = editItem.DataItem as ContractorLaborDetails;
                    (item["UserName"].Controls[0]).Visible = false;
                    BindDropDown(drpShift, ddlContractor);
                    ddlContractor.SelectedValue = contractorId.Text;
                    drpShift.SelectedValue = lblShift.Text;
                    txtLaborCount.Text = dataItem.UnskilledLabourCount.ToString();
                    //txtLaborCount.Text = lblLaborCount.Text;
                    txtBlockNumbers.Text = dataItem.BlockNumbers;
                    txtComments.Text = dataItem.Comments;
                }
                else
                {
                    GridDataItem item = e.Item as GridDataItem;
                    if (item != null)
                    {
                        ContractorLaborDetails dataItem = item.DataItem as ContractorLaborDetails;
                        List<ManPowerDetail> lst = ViewState["AddedDetails"] != null ? ViewState["AddedDetails"] as List<ManPowerDetail> : new List<ManPowerDetail>();
                        int Id = Convert.ToInt32(item.GetDataKeyValue("Id"));
                        ContractorLaborDetails obj = item.DataItem as ContractorLaborDetails;
                        ManPowerDetail objManPower = lst != null && lst.Count > 0 ? lst.FirstOrDefault(l => l.ContractorId == obj.ContractorId && l.Shift == obj.Shift && l.UnskilledLabourCount == obj.UnskilledLabourCount) : null;

                        if (Convert.ToInt32(dataItem.CanDelete) == 1 || objManPower != null)
                            (item["DeleteColumn"].Controls[0]).Visible = true;
                        else
                            (item["DeleteColumn"].Controls[0]).Visible = false;

                        if (Convert.ToInt32(dataItem.CanEdit) == 1 || objManPower != null)
                            (item["EditColumn"].Controls[0]).Visible = true;
                        else
                            (item["EditColumn"].Controls[0]).Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindDropDown(RadDropDownList ddlShift, RadDropDownList ddlContractor)
        {
            try
            {
                string siteId = ddlSapSite.SelectedValue.Trim();
                string projectId = ddlSapProjects.SelectedValue.Trim();
                ddlContractor.DataSource = ManPowerModel.GetContractorList(siteId, projectId);
                ddlContractor.DataTextField = "Name";
                ddlContractor.DataValueField = "Id";
                ddlContractor.DataBind();

                List<Models.ListItem> lstShift = new List<Models.ListItem>();
                Enum.GetValues(typeof(Constants.Shift))
                                       .Cast<Constants.Shift>()
                                       .Select(v => v.ToString())
                                       .ToList().ForEach(r => lstShift.Add(new Models.ListItem() { Id = r, Name = r }));
                ddlShift.DataTextField = "Name";
                ddlShift.DataValueField = "Id";
                ddlShift.DataSource = lstShift;
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlSapSite.SelectedValue.Trim() != string.Empty
               && ddlSapProjects.SelectedValue.Trim() != string.Empty
               && ddlNetwork.SelectedValue.Trim() != string.Empty
               && ddlArea.SelectedValue.Trim() != string.Empty
               && dtDetailsDate.SelectedDate.ToString() != string.Empty)
                {
                    gridManPowerDetails.MasterTableView.ClearEditItems();
                    gridManPowerDetails.MasterTableView.IsItemInserted = true;
                    gridManPowerDetails.MasterTableView.Rebind();
                }
                else
                {
                    radNotificationMessage.Title = "Error";
                    radNotificationMessage.Show("Please select Site, Project, Area, Network and Date.");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPowerDetails_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                List<ManPowerDetail> lst = ViewState["AddedDetails"] as List<ManPowerDetail>; 
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item.GetDataKeyValue("Id"));
                int canDelete = Convert.ToInt32(item.GetDataKeyValue("CanDelete"));
                ContractorLaborDetails obj = item.DataItem as ContractorLaborDetails;
                ManPowerDetail objManPower = null;
                if (lst != null && lst.Count > 0)
                    objManPower = lst.FirstOrDefault(l => l.Id == id);

                if (objManPower != null || canDelete == 1)
                {
                    ManPowerModel.Delete(id, Convert.ToInt32(Session["UserId"]));
                    radNotificationMessage.Show("Record deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void dtDetailsDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            try
            {
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlNetwork_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindDataToGrid();
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
            int siteCount = FillSapSiteDropdown();

            if (siteCount == 1)
                FillSapProjectDropdown();
            FillShiftDropdown();

        }

        private int FillSapSiteDropdown()
        {
            List<UserAssignedTaskDataForDropdown> lstSite = CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]),
                string.Empty, string.Empty, string.Empty, "site", false);
            int siteCount = lstSite.Count;
            if (siteCount < 1 || siteCount > 1)
            {
                ddlSapSite.EmptyMessage = Constants.CONST_SELECT_TEXT;
                ddlSapSite.Text = string.Empty;
            }
            ddlSapSite.DataSource = lstSite;
            ddlSapSite.DataTextField = "Value";
            ddlSapSite.DataValueField = "Id";
            ddlSapSite.DataBind();

            dtDetailsDate.SelectedDate = DateTime.Now;
            ddlSapProjects.EmptyMessage = Constants.CONST_SELECT_TEXT;
            ddlArea.EmptyMessage = Constants.CONST_SELECT_TEXT;
            ddlNetwork.EmptyMessage = Constants.CONST_SELECT_TEXT;
            return siteCount;
        }

        private void FillSapProjectDropdown(string Site = "")
        {
            ddlSapProjects.ClearSelection();
            ddlSapProjects.Items.Clear();
            TaskModel taskModel = new TaskModel();
            string siteId = ddlSapSite.SelectedValue.Trim();
            List<UserAssignedTaskDataForDropdown> lstProjects = CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), siteId,
                                                                                                                 string.Empty, string.Empty, "project", false);
            ddlSapProjects.DataSource = lstProjects;
            ddlSapProjects.DataTextField = "Value";
            ddlSapProjects.DataValueField = "Id";

            int projectCnt = lstProjects.Count;
            if (projectCnt > 1 || projectCnt < 1)
            {
                ddlSapProjects.EmptyMessage = Constants.CONST_SELECT_TEXT;
                ddlSapProjects.SelectedIndex = -1;
                ddlSapProjects.Text = string.Empty;
            }
            else
                ddlSapProjects.SelectedIndex = 0;
            ddlSapProjects.DataBind();

            if (projectCnt == 1)
                FillSapAreaDropdown();
            gridManPowerDetails.MasterTableView.ClearEditItems();
        }

        private void FillSapNetworkDropdown()
        {
            ddlNetwork.Items.Clear();
            ddlNetwork.ClearSelection();
            CommonFunctions commonFunction = new CommonFunctions();
            string siteId = ddlSapSite.SelectedValue.Trim();
            string projectId = ddlSapProjects.SelectedValue.Trim();
            string areaId = ddlArea.SelectedItem.Text.Trim();
            List<UserAssignedTaskDataForDropdown> lstNetwork = CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), siteId,
                                                                      projectId, areaId, "network", false);
            ddlNetwork.DataSource = lstNetwork;
            ddlNetwork.DataTextField = "Value";
            ddlNetwork.DataValueField = "Id";
            if (lstNetwork.Count > 1 || lstNetwork.Count < 1)
            {
                ddlNetwork.EmptyMessage = Constants.CONST_SELECT_TEXT;
                ddlNetwork.Text = string.Empty;
                ddlNetwork.SelectedIndex = -1;
            }
            else
            {
                ddlNetwork.SelectedIndex = 0;
            }
            ddlNetwork.DataBind();

            if (lstNetwork.Count == 1)
                FillContractorDropdown();
        }

        private void FillSapAreaDropdown()
        {
            ddlArea.Items.Clear();
            ddlArea.ClearSelection();
            CommonFunctions commonFunction = new CommonFunctions();
            string siteId = ddlSapSite.SelectedValue.Trim();
            string projectId = ddlSapProjects.SelectedValue.Trim();
            List<UserAssignedTaskDataForDropdown> lstArea = CommonFunctions.GetUserTaskDropdownDataForReport(Convert.ToInt32(Session["UserId"]), siteId,
                                                               projectId, string.Empty, "area", false);
            ddlArea.DataSource = lstArea;
            ddlArea.DataTextField = "Value";
            ddlArea.DataValueField = "Id";

            int areaCnt = lstArea.Count;
            if (areaCnt > 1 || areaCnt < 1)
            {
                ddlArea.EmptyMessage = Constants.CONST_SELECT_TEXT;
                ddlArea.Text = string.Empty;
                ddlArea.SelectedIndex = -1;
            }
            else
            {
                ddlArea.SelectedIndex = 0;
                ddlArea.SelectedValue = lstArea[0].Id;
            }
            ddlArea.DataBind();

            if (areaCnt == 1)
                FillSapNetworkDropdown();
        }

        private void FillContractorDropdown()
        {
            string siteId = ddlSapSite.SelectedValue.Trim();
            string projectId = ddlSapProjects.SelectedValue.Trim();
        }

        private void FillShiftDropdown()
        {
            List<Models.ListItem> lstShift = new List<Models.ListItem>();
            Enum.GetValues(typeof(Constants.Shift))
                                   .Cast<Constants.Shift>()
                                   .Select(v => v.ToString())
                                   .ToList().ForEach(r => lstShift.Add(new Models.ListItem() { Id = r, Name = r }));
            //ddlShift.DataSource = lstShift;
            //ddlShift.SelectedIndex = -1;
            //ddlShift.DataValueField = "Id";
            //ddlShift.DataTextField = "Name";
            //ddlShift.DataBind();
        }
        private int SaveLaborDetails(ManPowerDetail details)
        {

            if (!ManPowerModel.IsManPowerDetailsExists(details))
            {
                int Id = ManPowerModel.SaveManPowerDetails(details);
                if (Id != 0)
                {
                    radNotificationMessage.Title = "Success";
                    radNotificationMessage.Show("Details saved successfully.");
                    return Id;
                }
                else
                {
                    radNotificationMessage.Title = "Error";
                    radNotificationMessage.Show("Error while saving data.");
                    return -1;
                }
            }
            else
            {
                radNotificationMessage.Title = "Error";
                radNotificationMessage.Show("Details already exists.");
                return -1;
            }
        }

        private void SetControlsEditMode()
        {
            FillSapSiteDropdown();
            ddlSapSite.SelectedValue = Site;
            if (ddlSapProjects.Items.Count < 1)
                FillSapProjectDropdown();
            ddlSapProjects.SelectedValue = Project;
            if (ddlArea.Items.Count < 1)
                FillSapAreaDropdown();
            ddlArea.SelectedValue = Area.ToString();
            if (ddlNetwork.Items.Count < 1)
                FillSapNetworkDropdown();
            ddlNetwork.SelectedValue = Network;
            dtDetailsDate.SelectedDate = Date;

            int dateDiff = Convert.ToInt32((DateTime.Now - Date).TotalDays);
            if (dateDiff > 7)
                btnAddNew.Visible = false;
            if (Site != string.Empty || dateDiff > 7)
                DisableSubmit();
        }

        private void DisableSubmit()
        {
            ddlArea.Enabled = ddlNetwork.Enabled = ddlSapProjects.Enabled = ddlSapSite.Enabled = false;
            dtDetailsDate.Enabled = false;
        }

        private void SaveManPowerDetails(string EditMode, GridCommandEventArgs e)
        {
            if (ddlSapSite.SelectedValue.Trim() != string.Empty
                && ddlSapProjects.SelectedValue.Trim() != string.Empty
                && ddlNetwork.SelectedValue.Trim() != string.Empty
                && ddlArea.SelectedValue.Trim() != string.Empty
                && dtDetailsDate.SelectedDate.ToString() != string.Empty)
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                RadDropDownList ddlContractor = (RadDropDownList)editableItem.FindControl("ddlContractor");
                RadDropDownList ddlShift = (RadDropDownList)editableItem.FindControl("drpShift");
                RadTextBox txtLaborCount = (RadTextBox)editableItem.FindControl("txtLaborCount");
                RadTextBox txtMechanicalLabourCount = (RadTextBox)editableItem.FindControl("txtMechanicalLabourCount");
                RadTextBox txtCivilLabourCount = (RadTextBox)editableItem.FindControl("txtCivilLabourCount");
                RadTextBox txtElectricalLabourCount = (RadTextBox)editableItem.FindControl("txtElectricalLabourCount");
                RadTextBox txtComments = (RadTextBox)editableItem.FindControl("txtComments");
                RadTextBox txtBlockNumbers = (RadTextBox)editableItem.FindControl("txtBlockNumbers");

                int unskilledCount = txtLaborCount.Text.Trim() != string.Empty ? int.Parse(txtLaborCount.Text) : 0;
                int mechanicalCount = txtMechanicalLabourCount.Text.Trim() != string.Empty ? int.Parse(txtMechanicalLabourCount.Text) : 0;
                int electricalCount = txtElectricalLabourCount.Text.Trim() != string.Empty ? int.Parse(txtElectricalLabourCount.Text) : 0;
                int civilCount = txtCivilLabourCount.Text.Trim() != string.Empty ? int.Parse(txtCivilLabourCount.Text) : 0;

                if (unskilledCount > 0 || mechanicalCount > 0
                    || electricalCount > 0 || civilCount > 0)
                {
                    int Id = 0;
                    if (EditMode == Constants.CONST_EDIT_MODE)
                        Id = Convert.ToInt32(editableItem.GetDataKeyValue("Id"));

                    ManPowerDetail details = new ManPowerDetail()
                    {
                        Id = Id,
                        UnskilledLabourCount = unskilledCount,
                        CivilLabourCount= civilCount,
                        MechanicalLabourCount = mechanicalCount,
                        ElectricalLabourCount= electricalCount,
                        Site = ddlSapSite.SelectedValue,
                        Project = ddlSapProjects.SelectedValue,
                        AreaId = Convert.ToInt32(ddlArea.SelectedValue),
                        Network = ddlNetwork.SelectedValue,
                        Shift = ddlShift.SelectedValue,
                        ContractorId = Convert.ToInt32(ddlContractor.SelectedValue),
                        Date = dtDetailsDate.SelectedDate.Value,
                        Comments = txtComments.Text,
                        BlockNumbers = txtBlockNumbers.Text,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now
                    };

                    Id = SaveLaborDetails(details);
                    if (Id != -1 && Id != 0)
                        details.Id = Id;
                    gridManPowerDetails.MasterTableView.ClearEditItems();
                    Site = details.Site;
                    Project = details.Project;
                    Area = details.AreaId;
                    Network = details.Network;
                    Date = details.Date;
                    if (EditMode == Constants.CONST_EDIT_MODE)
                        BindDataToGrid(details.Id);
                    else
                    {
                        List<ManPowerDetail> lst = ViewState["AddedDetails"] != null ? ViewState["AddedDetails"] as List<ManPowerDetail> : new List<ManPowerDetail>();
                        lst.Add(details);
                        ViewState["AddedDetails"] = lst;
                        BindDataToGrid(0);
                    }
                }
                else
                {
                    radNotificationMessage.Title = "Error";
                    radNotificationMessage.Show("Please enter valid labour count.");
                }
            }
            else
            {
                radNotificationMessage.Title = "Error";
                radNotificationMessage.Show("Please select Site, Project, Area, Network and Date.");
            }
        }

        private void BindDataToGrid(int Id = 0)
        {
            Site = ddlSapSite.SelectedValue.Trim();
            Project = ddlSapProjects.SelectedValue.Trim();
            Area = Convert.ToInt32(ddlArea.SelectedValue.Trim());
            Network = ddlNetwork.SelectedValue.Trim();
            Date = dtDetailsDate.SelectedDate.Value;
            List<ContractorLaborDetails> lstDataSource = ManPowerModel.GetContractorDetails(Site, Project, Area, Network, Date, Convert.ToInt32(Session["UserId"]));
            List<ManPowerDetail> lstAddedDeatils = ViewState["AddedDetails"] != null ? ViewState["AddedDetails"] as List<ManPowerDetail> : new List<ManPowerDetail>();

            if (Id == 0)
            {
                foreach (ManPowerDetail obj in lstAddedDeatils)
                {
                    if (obj != null)
                    {
                        int index = lstDataSource.FindIndex(l => l.Id == obj.Id);
                        lstDataSource[index].CanDelete = 1;
                        lstDataSource[index].CanEdit = 1;
                    }
                }
            }
            gridManPowerDetails.DataSource = lstDataSource;
            gridManPowerDetails.DataBind();
        }

        #endregion

       
    }
}