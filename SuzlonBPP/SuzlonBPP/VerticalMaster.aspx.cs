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
    public partial class VerticalMaster : System.Web.UI.Page
    {

        CommonFunctions commonFunctions = new CommonFunctions();
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.VERTICAL_MASTER, menuList)) Response.Redirect("~/webNoAccess.aspx");

                if (!IsPostBack)
                {
                    bindVerticalGrid();
                    grdVertical.DataBind();
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
        protected void grdVertical_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveVertical(e, Constants.CONST_NEW_MODE);
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
        protected void grdVertical_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bindVerticalGrid();
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdVertical_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveVertical(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdVertical_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                if (e.Item is GridDataInsertItem)
                {
                    CheckBox chkStatus = (CheckBox)dataItem.FindControl("chkStatus");
                    chkStatus.Checked = true;
                }
                if (e.Item.IsInEditMode)
                {
                    CheckBox chkStatus = (CheckBox)dataItem.FindControl("chkStatus");
                    chkStatus.Attributes.Add("onclick", "oncheckedChaned(this);");
                }
                else
                {
                    Label lblStatus = (Label)dataItem.FindControl("lblStatus");
                    string text = lblStatus.Text;
                    lblStatus.Text = text == "True" ? "Enabled" : "Disabled";
                }

            }
        }

        #endregion "Events"

        #region "Private Methods"

        /// <summary>
        /// Get data to Bind Vertical Grid
        /// </summary>
        /// <returns></returns>
        private void bindVerticalGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.VERTICAL_GET, string.Empty);
                if (result != Constants.REST_CALL_FAILURE)
                {
                    List<Models.VerticalMaster> lstVertical = JsonConvert.DeserializeObject<List<Models.VerticalMaster>>(result);
                    grdVertical.DataSource = lstVertical;
                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    grdVertical.DataSource = new System.Data.DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }
        /// <summary>
        /// Add/Update Vertical details.
        /// </summary>
        /// <param name="e"></param>
        private void SaveVertical(GridCommandEventArgs e, string editMode)
        {
            try
            {
                string mnuAuthorization = string.Empty;
                int verticalId = 0;

                GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    string VerticalName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("Vertical")).TextBoxControl.Text;
                    // bool status = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                    bool status = (editableItem.FindControl("chkStatus") as CheckBox).Checked; 

                    TextBox txttestmul = editableItem.FindControl("txtDescription") as TextBox;
                    string VerticalDescription = txttestmul.Text;

                    if (editMode == Constants.CONST_EDIT_MODE)
                        verticalId = Convert.ToInt32(editableItem.GetDataKeyValue("VerticalId"));

                    Models.VerticalMaster verticalMaster = new Models.VerticalMaster()
                    {
                        VerticalId = verticalId,
                        Name = VerticalName,
                        Description = VerticalDescription,
                        Status = status
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(verticalMaster);
                    string result = string.Empty;

                    result = commonFunctions.RestServiceCall(string.Format(Constants.VERTICAL_EXISTS, VerticalName, verticalId), string.Empty);
                    bool isExist = Convert.ToBoolean(result);
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    if (isExist)
                    {
                        radMessage.Show(Constants.RECORD_EXIST_MESSAGE);
                        e.Canceled = true;
                        return;
                    }

                    if (editMode == Constants.CONST_EDIT_MODE)
                        result = commonFunctions.RestServiceCall(Constants.VERTICAL_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        result = commonFunctions.RestServiceCall(Constants.VERTICAL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
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

        protected void grdVertical_ItemCreated(object sender, GridItemEventArgs e)
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

        protected void grdVertical_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                grdVertical.MasterTableView.IsItemInserted = false;
            }
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                grdVertical.MasterTableView.ClearEditItems();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grdVertical.MasterTableView.ClearEditItems();
            grdVertical.MasterTableView.IsItemInserted = true;
            grdVertical.MasterTableView.Rebind();
        }
    }
}