using Newtonsoft.Json;
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
    public partial class MaterialMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridMaterialMaster_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                gridMaterialMaster.DataSource = MaterialMasterModel.GetMaterialData();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridMaterialMaster_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveMaterialCode(e,Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridMaterialMaster_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveMaterialCode(e,Constants.CONST_NEW_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridMaterialMaster_CancelCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridMaterialMaster_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.CommandName))
                {
                    case "InitInsert":
                        gridMaterialMaster.MasterTableView.ClearEditItems();
                        break;
                    case "Edit":
                        e.Item.OwnerTableView.IsItemInserted = false;
                        break;
                    case "Filter":
                        gridMaterialMaster.MasterTableView.ClearEditItems();
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpSite_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            try
            {
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
                //bindProjDropDown();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridMaterialMaster_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
                else
                {
                    if (e.Item is GridEditableItem)
                    {
                        Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                        string text = lblStatus.Text;
                        lblStatus.Text = text == "True" ? "Active" : "InActive";
                    }
                }
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

        private void SaveMaterialCode(GridCommandEventArgs e, string EditMode)
        {
            try
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;

                if (editableItem != null)
                {
                    int Id = 0;
                    RadDropDownList drpSite = (RadDropDownList)editableItem.FindControl("drpSite");
                    RadTextBox txtMaterialCode = (RadTextBox)editableItem.FindControl("txtMaterialCode");
                    RadDropDownList drpProject = (RadDropDownList)editableItem.FindControl("drpProject");
                    CheckBox chkStatus = (CheckBox)editableItem.FindControl("chkStatus");

                    if (EditMode == Constants.CONST_EDIT_MODE)
                        Id = Convert.ToInt32(editableItem.GetDataKeyValue("Id"));

                    Material objMaterialMaster = new Material()
                    {
                        Id = Id,
                        Site = drpSite.SelectedValue.ToString(),
                        ProjectId = drpProject.SelectedValue.ToString(),
                        MaterialCode = txtMaterialCode.Text,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsActive = chkStatus.Checked
                    };

                    bool isExist = MaterialMasterModel.IsMaterialCodeAlreadyExists(objMaterialMaster);
                    if (!isExist)
                    {
                        MaterialMasterModel.SaveMaterialData(objMaterialMaster);
                        radNotificationMessage.Title = "Success";
                        radNotificationMessage.Show("Material code saved successfully");
                    }
                    else
                    {

                        radNotificationMessage.Title = "Error";
                        radNotificationMessage.Show("Material code already exists.");
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
                gridMaterialMaster.MasterTableView.ClearEditItems();
                gridMaterialMaster.MasterTableView.IsItemInserted = true;
                gridMaterialMaster.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}