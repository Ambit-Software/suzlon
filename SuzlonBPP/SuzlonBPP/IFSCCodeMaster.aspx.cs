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
    public partial class IFSCCodeMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.IFSC_CODE_MASTER, menuList)) Response.Redirect("~/webNoAccess.aspx");

                if (!IsPostBack)
                {
                    bindIFSCCodeGrid();
                    grdIFSCCode.DataBind();
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
        protected void grdIFSCCode_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveIFSCCode(e, Constants.CONST_NEW_MODE);
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
        protected void grdIFSCCode_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bindIFSCCodeGrid();
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdIFSCCode_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveIFSCCode(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }        

        protected void grdIFSCCode_ItemCreated(object sender, GridItemEventArgs e)
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

        #endregion "Events"

        #region "Private Methods"

        /// <summary>
        /// Get data to Bind IFSCCode Grid
        /// </summary>
        /// <returns></returns>
        private void bindIFSCCodeGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.IFSCCODE_GET, string.Empty);
                if (result != Constants.REST_CALL_FAILURE)
                {
                    List<Models.IFSCCodeMaster> lstIFSCCode = JsonConvert.DeserializeObject<List<Models.IFSCCodeMaster>>(result);
                    grdIFSCCode.DataSource = lstIFSCCode;
                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    grdIFSCCode.DataSource = new System.Data.DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }
        /// <summary>
        /// Add/Update IFSCCode details.
        /// </summary>
        /// <param name="e"></param>
        private void SaveIFSCCode(GridCommandEventArgs e, string editMode)
        {
            try
            {
                string mnuAuthorization = string.Empty;
                int iFSCCodeId = 0;

                GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    string iFSCCode = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("IFSCCode")).TextBoxControl.Text;
                    string bankName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("BankName")).TextBoxControl.Text;
                    string branchName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("BranchName")).TextBoxControl.Text;
                    bool status = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                    if (editMode == Constants.CONST_EDIT_MODE)
                        iFSCCodeId = Convert.ToInt32(editableItem.GetDataKeyValue("IFSCCodeId"));

                    Models.IFSCCodeMaster IFSCCodeMaster = new Models.IFSCCodeMaster()
                    {
                        IFSCCodeId = iFSCCodeId,
                        IFSCCode = iFSCCode,
                        BankName = bankName,
                        BranchName = branchName,
                        Status = status
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(IFSCCodeMaster);
                    string result = string.Empty;

                    result = commonFunctions.RestServiceCall(string.Format(Constants.IFSCCODE_EXISTS, iFSCCode, iFSCCodeId), string.Empty);
                    bool isExist = Convert.ToBoolean(result);
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    if (isExist)
                    {
                        radMessage.Show(Constants.RECORD_EXIST_MESSAGE);
                        e.Canceled = true;
                        return;
                    }

                    if (editMode == Constants.CONST_EDIT_MODE)
                        result = commonFunctions.RestServiceCall(Constants.IFSCCODE_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        result = commonFunctions.RestServiceCall(Constants.IFSCCODE_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
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

        protected void grdIFSCCode_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                grdIFSCCode.MasterTableView.IsItemInserted = false;
            }
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                grdIFSCCode.MasterTableView.ClearEditItems();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grdIFSCCode.MasterTableView.ClearEditItems();
            grdIFSCCode.MasterTableView.IsItemInserted = true;
            grdIFSCCode.MasterTableView.Rebind();
        }
    }
}