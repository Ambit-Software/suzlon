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
    public partial class NatureRequest : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.NATURE_OF_REQUEST, menuList)) Response.Redirect("~/webNoAccess.aspx");

                if (!IsPostBack)
                {
                    bindRequestGrid();
                    grdRequest.DataBind();
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
        protected void grdRequest_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveRequest(e, Constants.CONST_NEW_MODE);
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
        protected void grdRequest_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bindRequestGrid();
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRequest_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveRequest(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdRequest_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    GridEditableItem editItem = (GridEditableItem)e.Item;
                    RadDropDownList drpType = (RadDropDownList)editItem.FindControl("drpType");
                    Label lblType = item["Type"].FindControl("lblType") as Label;
                    BindDropDown(drpType);
                    drpType.SelectedValue = lblType.Text;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindDropDown(RadDropDownList drpType)
        {
            drpType.DataTextField = "Name";
            drpType.DataValueField = "Id";
            drpType.DataSource = CommonFunctions.GetNatureTypes();
            drpType.DataBind();
        }

        #endregion "Events"

        #region "Private Methods"

        /// <summary>
        /// Get data to Bind Request Grid
        /// </summary>
        /// <returns></returns>
        private void bindRequestGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.NATUREREQUEST_GET, string.Empty);
                if (result != Constants.REST_CALL_FAILURE)
                {
                    List<NatureRequestMaster> lstRequest = JsonConvert.DeserializeObject<List<NatureRequestMaster>>(result);
                    grdRequest.DataSource = lstRequest;
                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    grdRequest.DataSource = new System.Data.DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }
        /// <summary>
        /// Add/Update Request details.
        /// </summary>
        /// <param name="e"></param>
        private void SaveRequest(GridCommandEventArgs e, string editMode)
        {
            try
            {
                string mnuAuthorization = string.Empty;
                int requestId = 0;

                GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    string requestName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("Request")).TextBoxControl.Text;
                    bool status = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                    RadDropDownList drpType = (RadDropDownList)editableItem.FindControl("drpType");
                    TextBox txttestmul = editableItem.FindControl("txtDescription") as TextBox;
                    string requestDesc = txttestmul.Text;
                    if (editMode == Constants.CONST_EDIT_MODE)
                        requestId = Convert.ToInt32(editableItem.GetDataKeyValue("RequestId"));

                    NatureRequestMaster requestMaster = new NatureRequestMaster()
                    {
                        RequestId = requestId,
                        Name = requestName,
                        Description = requestDesc,
                        Type = drpType.SelectedValue.Trim(),
                        Status = status
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(requestMaster);
                    string result = string.Empty;

                    result = commonFunctions.RestServiceCall(string.Format(Constants.NATUREREQUEST_EXISTS, requestName, requestId), string.Empty);
                    bool isExist = Convert.ToBoolean(result);
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    if (isExist)
                    {
                        radMessage.Show(Constants.RECORD_EXIST_MESSAGE);
                        e.Canceled = true;
                        return;
                    }

                    if (editMode == Constants.CONST_EDIT_MODE)
                        result = commonFunctions.RestServiceCall(Constants.NATUREREQUEST_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        result = commonFunctions.RestServiceCall(Constants.NATUREREQUEST_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
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

        protected void grdRequest_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    CheckBox chkbox = (CheckBox)item["chkStatus"].Controls[0];
                    chkbox.Attributes.Add("onclick", "oncheckedChaned(this);");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdRequest_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                grdRequest.MasterTableView.IsItemInserted = false;
            }
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                grdRequest.MasterTableView.ClearEditItems();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grdRequest.MasterTableView.ClearEditItems();
            grdRequest.MasterTableView.IsItemInserted = true;
            grdRequest.MasterTableView.Rebind();
        }
    }
}