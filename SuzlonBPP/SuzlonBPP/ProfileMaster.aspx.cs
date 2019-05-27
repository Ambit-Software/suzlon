using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Newtonsoft.Json;
using Cryptography;
using System.Collections;
using System.Data;

namespace SuzlonBPP.Admin
{
    public partial class ProfileMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.PROFILE_MASTER, menuList)) Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {
                bindProfileGrid();
                grdProfile.DataBind();
            }
        }
        /// <summary>
        /// Insert new record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdProfile_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveProfile(e, Constants.CONST_NEW_MODE);
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
        protected void grdProfile_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bindProfileGrid();
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdProfile_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                SaveProfile(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        
        protected void grdProfile_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
        
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                if (e.Item is GridDataInsertItem!=true)
                {
                    int profileid = Convert.ToInt32(item.GetDataKeyValue("profileId"));
                    if (profileid < 11)
                    {
                        item["EditCommandColumn"].Enabled = false;
                    }
                }

               
               


                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    try
                    {
                        string mnuAccess = string.Empty;
                       // GridDataItem item = e.Item as GridDataItem;
                        Label mnuAccessLabel = item["MenuAuthorization"].FindControl("lblMenuAccess") as Label;
                        mnuAccess = mnuAccessLabel.Text;

                        GridEditableItem editItem = (GridEditableItem)e.Item;
                        RadComboBox comboMnu = (RadComboBox)editItem.FindControl("cmbMenuAccess");
                        comboMnu.DataTextField = "menuName";
                        comboMnu.DataValueField = "menuId";
                        comboMnu.DataSource = binddrpMenuAuth();
                        comboMnu.DataBind();

                        if (mnuAccess != string.Empty)
                        {
                            foreach (RadComboBoxItem itm in comboMnu.Items)
                            {
                                if (mnuAccess.Contains(itm.Value.Trim()))
                                {
                                    itm.Checked = true;
                                }
                            }
                        }
                        CheckBox chkStatus = (CheckBox)editItem.FindControl("chkStatus");
                        chkStatus.Attributes.Add("onclick", "oncheckedChaned(this);");

                       


                    }
                    catch (Exception ex)
                    {
                        CommonFunctions.WriteErrorLog(ex);
                    }
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

        #endregion "Events"

        #region "Private Methods"

        /// <summary>
        /// Get data to Bind Profile Grid
        /// </summary>
        /// <returns></returns>
        /// 
        private List<Models.Menu> binddrpMenuAuth()
        {
            string result = commonFunctions.RestServiceCall(Constants.PROFILE_GET_MENULIST, string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                List<Models.Menu> lstmnu = JsonConvert.DeserializeObject<List<Models.Menu>>(result);
                return lstmnu;
            }
            else
            {
                return null;
            }
        }
        private void bindProfileGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.PROFILE_GET_ALL, string.Empty);
                if (result != Constants.REST_CALL_FAILURE)
                {
                    List<Models.ProfileModel> lstProfile = JsonConvert.DeserializeObject<List<Models.ProfileModel>>(result);
                    grdProfile.DataSource = lstProfile;
                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    grdProfile.DataSource = new DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }
        /// <summary>
        /// Add/Update Profile details.
        /// </summary>
        /// <param name="e"></param>
        private void SaveProfile(GridCommandEventArgs e, string editMode)
        {
            try
            { 
            string mnuAuthorization = string.Empty;
            int profileId = 0;

            GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    string profileName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("Profile")).TextBoxControl.Text;
                    // bool status = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                    bool status = (editableItem.FindControl("chkStatus") as CheckBox).Checked;
                    TextBox txttestmul = editableItem.FindControl("txtDescription") as TextBox;
                    string profileDescription = txttestmul.Text;

                    RadComboBox mnuSelector = editableItem.FindControl("cmbMenuAccess") as RadComboBox;
                    var collection = mnuSelector.CheckedItems;
                    foreach (var selectedItem in collection)
                    {
                        mnuAuthorization = mnuAuthorization + "," + Convert.ToString(selectedItem.Value);
                    }



                    if (editMode == Constants.CONST_EDIT_MODE)
                        profileId = Convert.ToInt32(editableItem.GetDataKeyValue("profileId"));

                    Models.ProfileModel profileMaser = new Models.ProfileModel()
                    {
                        ProfileId = profileId,
                        ProfileName = profileName,
                        Description = profileDescription,
                        MenuIds= mnuAuthorization.Substring(1, mnuAuthorization.Length - 1),
                        Status = status
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(profileMaser);
                    string result = string.Empty;

                    //result = commonFunctions.RestServiceCall(string.Format(Constants.CHECK_PROFILE_EXIST, profileName, profileId), string.Empty);
                    result = commonFunctions.RestServiceCall(string.Format(Constants.CHECK_PROFILE_EXIST), Crypto.Instance.Encrypt(jsonInputParameter));
                    bool isExist = Convert.ToBoolean(result);
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    if (isExist)
                    {
                        radMessage.Show(Constants.PROFILE_EXIST_MESSAGE);
                        e.Canceled = true;
                        return;
                    }

                    if (editMode == Constants.CONST_EDIT_MODE)
                        result = commonFunctions.RestServiceCall(Constants.PROFILE_UPDATE, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        result = commonFunctions.RestServiceCall(Constants.PROFILE_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
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

        protected void grdProfile_ItemCreated(object sender, GridItemEventArgs e)
        {
            //try
            //{ 
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    CheckBox chkbox = (CheckBox)item["chkStatus"].Controls[0]; 
            //    chkbox.Attributes.Add("onclick", "oncheckedChaned(this);");
            //}
            //}
            //catch (Exception ex)
            //{
            //    CommonFunctions.WriteErrorLog(ex);
            //}
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grdProfile.MasterTableView.ClearEditItems();
            grdProfile.MasterTableView.IsItemInserted = true;
            grdProfile.MasterTableView.Rebind();
        }

        protected void grdProfile_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (Convert.ToString(e.CommandName))
            {
                case "InitInsert":
                    grdProfile.MasterTableView.ClearEditItems();
                    break;
                case "Edit":
                    e.Item.OwnerTableView.IsItemInserted = false;
                    break;
                case "Filter":
                    grdProfile.MasterTableView.ClearEditItems();
                    break;
            }
        }
    }
}
