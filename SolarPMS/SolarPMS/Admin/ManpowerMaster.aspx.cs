using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS.Admin
{
    public partial class ManpowerMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);

                if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPower_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                gridManPower.DataSource = ManPowerModel.GetManPowerData();
            }
            catch(Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPower_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveManPowerDetails(e, Constants.CONST_NEW_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPower_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveManPowerDetails(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPower_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.CommandName))
                {
                    case "InitInsert":
                        gridManPower.MasterTableView.ClearEditItems();
                        break;
                    case "Edit":
                        e.Item.OwnerTableView.IsItemInserted = false;
                        break;
                    case "Filter":
                        gridManPower.MasterTableView.ClearEditItems();
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPower_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    string SiteId = Constants.CONST_SELECT_SITE_TEXT;
                    string VillageId = string.Empty;
                    string projId = string.Empty;
                    GridDataItem item = e.Item as GridDataItem;

                    GridEditableItem editItem = (GridEditableItem)e.Item;
                    RadDropDownList drpSite = (RadDropDownList)editItem.FindControl("drpSite");

                    Label lblSite = item["Site"].FindControl("lblSite") as Label;
                    SiteId = Convert.ToString(lblSite.Text.Trim());
                    BindDropDown(drpSite);
                    drpSite.SelectedValue = SiteId == string.Empty ? Constants.CONST_SELECT_SITE_TEXT : SiteId;

                    RadDropDownList drpProj = (RadDropDownList)editItem.FindControl("drpProject");
                    Label lblProject = item["Project"].FindControl("lblProject") as Label;
                    projId = lblProject.Text;

                    if (!string.IsNullOrEmpty(SiteId.Trim()))
                    {
                        bindProjDropDown(drpProj, SiteId.Trim());
                        drpProj.SelectedValue = projId.Trim();
                    }
                }
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
                gridManPower.MasterTableView.ClearEditItems();
                gridManPower.MasterTableView.IsItemInserted = true;
                gridManPower.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpSite_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            try {
                RadDropDownList drpsite = (RadDropDownList)sender;
                GridDataItem dataItem = (GridDataItem)drpsite.NamingContainer;
                string strText = Convert.ToString(drpsite.SelectedValue);

                GridEditableItem editedItem = (sender as RadDropDownList).NamingContainer as GridEditableItem;
                if (editedItem != null && !string.IsNullOrEmpty(strText))
                {
                    RadDropDownList drpProj = (RadDropDownList)editedItem.FindControl("drpProject");
                    bindProjDropDown(drpProj, strText);
                    drpProj.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                gridManPower.MasterTableView.AllowFilteringByColumn = false;
                gridManPower.MasterTableView.AllowSorting = false;
                gridManPower.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                gridManPower.ExportSettings.FileName = "ManPower";
                gridManPower.Rebind();
                gridManPower.MasterTableView.ExportToExcel();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindDropDown(RadDropDownList drpSite)
        {
            string drpname = "site";
            string result1 = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
            DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result1);

            drpSite.DefaultMessage = Constants.CONST_SELECT_SITE_TEXT;
            drpSite.DataTextField = "Name";
            drpSite.DataValueField = "Id";
            drpSite.DataSource = ddValues.site;
            drpSite.DataBind();
        }

        public void bindProjDropDown(RadDropDownList drpProj, string siteId)
        {
            string result = commonFunctions.RestServiceCall(string.Format(Constants.TABLE_GET_PROJECTBYSITE, Convert.ToString(siteId)), string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                drpProj.DataTextField = "Name";
                drpProj.DataValueField = "Id";
                drpProj.DataSource = ddValues.project;
                drpProj.DataBind();
            }
        }

        private void SaveManPowerDetails(GridCommandEventArgs e, string editMode)
        {
            try
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;

                if (editableItem != null)
                {
                    int Id = 0;
                    RadDropDownList drpSite = (RadDropDownList)editableItem.FindControl("drpSite");
                    RadTextBox txtName = (RadTextBox)editableItem.FindControl("txtName");
                    RadDropDownList drpProject = (RadDropDownList)editableItem.FindControl("drpProject");

                    if (editMode == Constants.CONST_EDIT_MODE)
                        Id = Convert.ToInt32(editableItem.GetDataKeyValue("Id"));

                    ManPowerMaster objManPowerMaster = new ManPowerMaster()
                    {
                        Id = Id,
                        Site = drpSite.SelectedValue.ToString(),
                        Project = drpProject.SelectedValue.ToString(),
                        Name = txtName.Text,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now
                    };

                    bool isExist = ManPowerModel.IsContractorAlreadyExists(objManPowerMaster);
                    if (!isExist)
                    {
                        bool result = ManPowerModel.SaveContractor(objManPowerMaster);
                        radNotificationMessage.Title = "Success";
                        radNotificationMessage.Show("Contractor details saved successfully");
                    }
                    else
                    {

                        radNotificationMessage.Title = "Error";
                        radNotificationMessage.Show("Contractor name already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}