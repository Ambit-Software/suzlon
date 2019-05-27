using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class SubVerticalMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.SUBVERTICAL_MASTER, menuList)) Response.Redirect("~/webNoAccess.aspx");

                if (!IsPostBack)
                {
                    bindSubVerticalGrid();
                    grdSubVertical.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        /// <summary>
        /// Insert new record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSubVertical_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveSubVertical(e, Constants.CONST_NEW_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Set datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSubVertical_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bindSubVerticalGrid();
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSubVertical_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveSubVertical(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdSubVertical_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                    {
                        string VerticalId = string.Empty;
                        GridDataItem item = e.Item as GridDataItem;
                        GridEditableItem editItem = (GridEditableItem)e.Item;
                        RadDropDownList drpVertical = (RadDropDownList)editItem.FindControl("drpVertical");
                        Label lblVertical = item["Vertical"].FindControl("lblVertical") as Label;
                        VerticalId = lblVertical.Text;
                        BindDropDown(drpVertical);
                        drpVertical.SelectedValue = VerticalId;

                        CheckBox chkStatus = (CheckBox)item.FindControl("chkStatus");
                        chkStatus.Attributes.Add("onclick", "oncheckedChaned(this);");

                    }
                    else
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;
                        Label lblStatus = (Label)dataItem.FindControl("lblStatus");
                        string text = lblStatus.Text;
                        lblStatus.Text = text == "True" ? "Enabled" : "Disabled";
                    }
                    if (e.Item is GridDataInsertItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;
                        CheckBox chkStatus = (CheckBox)dataItem.FindControl("chkStatus");
                        chkStatus.Attributes.Add("onclick", "oncheckedChaned(this);");
                        chkStatus.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindDropDown(RadDropDownList drpVertical)
        {
            string drpname = "vertical";
            string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
            DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
            drpVertical.DataTextField = "Name";
            drpVertical.DataValueField = "Id";
            drpVertical.DataSource = ddValues.Vertical;
            drpVertical.DataBind();
        }

        #endregion "Events"

        #region "Private Methods"

        /// <summary>
        /// Get data to Bind SubVertical Grid
        /// </summary>
        /// <returns></returns>
        private void bindSubVerticalGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.SUBVERTICAL_GET, string.Empty);
                List<SubVerticalModel> lstSubVertical = JsonConvert.DeserializeObject<List<SubVerticalModel>>(result);
                grdSubVertical.DataSource = lstSubVertical;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }
        /// <summary>
        /// Add/Update SubVertical details.
        /// </summary>
        /// <param name="e"></param>
        private void SaveSubVertical(GridCommandEventArgs e, string editMode)
        {
            try
            {
                string mnuAuthorization = string.Empty;
                int subVerticalId = 0;

                GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    string subVerticalName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("SubVertical")).TextBoxControl.Text;
                    // bool status = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                    bool status = (editableItem.FindControl("chkStatus") as CheckBox).Checked; 

                    RadDropDownList drpVertical = (RadDropDownList)editableItem.FindControl("drpVertical");
                    TextBox txttestmul = editableItem.FindControl("txtDescription") as TextBox;
                    string subVerticalDesc = txttestmul.Text;
                    if (editMode == Constants.CONST_EDIT_MODE)
                        subVerticalId = Convert.ToInt32(editableItem.GetDataKeyValue("subVerticalId"));
                    int verticalId = (Convert.ToInt32(drpVertical.SelectedValue.Trim()));
                    Models.SubVerticalMaster SubVerticalMaster = new Models.SubVerticalMaster()
                    {
                        SubVerticalId = subVerticalId,
                        Name = subVerticalName,
                        Description = subVerticalDesc,
                        VerticalId = verticalId,
                        Status = status
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(SubVerticalMaster);
                    string result = string.Empty;

                    result = commonFunctions.RestServiceCall(string.Format(Constants.SUBVERTICAL_EXISTS, subVerticalName, subVerticalId, verticalId), string.Empty);
                    bool isExist = Convert.ToBoolean(result);
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    if (isExist)
                    {
                        radMessage.Show(Constants.SUBVERTICAL_DUPLIACATE_MSG);
                        e.Canceled = true;
                        return;
                    }

                    if (editMode == Constants.CONST_EDIT_MODE)
                        result = commonFunctions.RestServiceCall(Constants.SUBVERTICAL_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        result = commonFunctions.RestServiceCall(Constants.SUBVERTICAL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                    if (result == Constants.REST_CALL_FAILURE)
                        if (editMode == Constants.CONST_EDIT_MODE)
                            radMessage.Show(Constants.ERROR_OCC_WHILE_UPDATING);
                        else
                            radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    else
                        radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                }
            }
            catch (Exception ex)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion "Private Methods"

        protected void grdSubVertical_ItemCreated(object sender, GridItemEventArgs e)
        {
            //try
            //{
            //    if (e.Item is GridDataItem)
            //    {
            //        GridDataItem item = (GridDataItem)e.Item;
            //        CheckBox chkbox = (CheckBox)item["chkStatus"].Controls[0];
            //        chkbox.Attributes.Add("onclick", "oncheckedChaned(this);");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    CommonFunctions.WriteErrorLog(ex);
            //}
        }

        protected void grdSubVertical_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                grdSubVertical.MasterTableView.IsItemInserted = false;
            }
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                grdSubVertical.MasterTableView.ClearEditItems();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grdSubVertical.MasterTableView.ClearEditItems();
            grdSubVertical.MasterTableView.IsItemInserted = true;
            grdSubVertical.MasterTableView.Rebind();
        }
    }
}