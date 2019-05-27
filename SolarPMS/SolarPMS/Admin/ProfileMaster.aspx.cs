using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web;
using Newtonsoft.Json;
using System.Web.UI;
using Cryptography;
using System.Collections;
using System.Data;

namespace SolarPMS.Admin
{
    public partial class ProfileMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");

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
        /// <summary>
        /// Bind The DropDown Of menuaothorization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdProfile_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
              if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    try
                    {
                        string mnuAccess = string.Empty;
                        GridDataItem item = e.Item as GridDataItem;
                        Label mnuAccessLabel = item["MenuAuthorization"].FindControl("lblMenuAccess") as Label;
                        mnuAccess = mnuAccessLabel.Text;


                        GridEditableItem editItem = (GridEditableItem)e.Item;
                        RadComboBox comboMnu = (RadComboBox)editItem.FindControl("cmbMenuAccess");
                        comboMnu.DataTextField = "menuName";
                        comboMnu.DataValueField = "menuId";
                        comboMnu.DataSource = binddrpMenuAuth();
                        comboMnu.DataBind();

                        Label lblDocumentUpload = (Label)e.Item.FindControl("lblDocumentUpload");
                        
                        RadDropDownList ddlDocumentAccess = (RadDropDownList)editItem.FindControl("ddlDocumentAccess");
                        ddlDocumentAccess.DataTextField = "Name";
                        ddlDocumentAccess.DataValueField = "Id";
                        ddlDocumentAccess.DataSource = GetDocumnetAccessList();
                        ddlDocumentAccess.DataBind();
                        ddlDocumentAccess.SelectedValue = ((SolarPMS.Models.ProfileModel)e.Item.DataItem).DEDocumentUploadAccess.ToString();

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

                    Label lblDocumentUpload = (Label)dataItem.FindControl("lblDocumentUpload");
                    text = lblDocumentUpload.Text;
                    switch (text)
                    {
                        case "0":
                            lblDocumentUpload.Text = Constants.DEDocumentUploadAccess.None.ToString();
                            break;
                        case "1":
                            lblDocumentUpload.Text = Constants.DEDocumentUploadAccess.View.ToString();
                            break;
                        case "2":
                            lblDocumentUpload.Text = Constants.DEDocumentUploadAccess.Full.ToString();
                            break;
                    }
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

        private static List<Models.ListItem> GetDocumnetAccessList()
        {
            List<Models.ListItem> ddlDocumnetAccessValue = new List<Models.ListItem>();
            ddlDocumnetAccessValue.Add(new Models.ListItem() { Id = "0", Name = Constants.DEDocumentUploadAccess.None.ToString() });
            ddlDocumnetAccessValue.Add(new Models.ListItem() { Id = "1", Name = Constants.DEDocumentUploadAccess.View.ToString() });
            ddlDocumnetAccessValue.Add(new Models.ListItem() { Id = "2", Name = Constants.DEDocumentUploadAccess.Full.ToString() });
            return ddlDocumnetAccessValue;
        }

        /// <summary>
        /// Hide error message on cancel click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdProfile_CancelCommand(object sender, GridCommandEventArgs e)
        {
            //lblErrorMessage.Visible = false;
        }
        #endregion "Events"

        #region "Private Methods"
        /// <summary>
        /// Get data to Bind Dropdown 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Get data to Bind Profile Grid
        /// </summary>
        /// <returns></returns>
        private void bindProfileGrid()
        {
            string getAllProfileResult = commonFunctions.RestServiceCall(Constants.PROFILE_GET_ALL, string.Empty);
            if (!string.IsNullOrEmpty(getAllProfileResult))
            {

                List<Models.ProfileModel> lstProfile = JsonConvert.DeserializeObject<List<Models.ProfileModel>>(getAllProfileResult);
                grdProfile.DataSource = lstProfile;
            }
            else
            {
                grdProfile.DataSource = new DataTable();
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
                    string profileName = ((GridTextBoxColumnEditor)editableItem.EditManager.GetColumnEditor("Prfile")).TextBoxControl.Text;
                    //bool status = (editableItem["chkStatus"].Controls[0] as CheckBox).Checked;
                    bool status = (editableItem.FindControl("chkStatus") as CheckBox).Checked;


                    TextBox txttestmul = editableItem.FindControl("txtDescription") as TextBox;
                    string profileDescription = txttestmul.Text;

                    RadComboBox mnuSelector = editableItem.FindControl("cmbMenuAccess") as RadComboBox;
                    RadDropDownList ddlDocumentAccess = editableItem.FindControl("ddlDocumentAccess") as RadDropDownList;
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
                        Status = status,
                        MenuIds = mnuAuthorization.Substring(1, mnuAuthorization.Length - 1),
                        DEDocumentUploadAccess = Convert.ToInt32(ddlDocumentAccess.SelectedValue)
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(profileMaser);
                    string result1 = string.Empty;

                    result1 = commonFunctions.RestServiceCall(string.Format(Constants.CHECK_PROFILE_EXIST, profileName, profileId), string.Empty);
                    bool isExist = Convert.ToBoolean(result1);

                    if (isExist)
                    {
                        radMesaage.Title = "Alert";
                        radMesaage.Show(Constants.PROFILE_EXIST_MESSAGE);
                        e.Canceled = true;
                        return;
                    }

                    string serviceResult = string.Empty;

                    if (editMode == Constants.CONST_EDIT_MODE)
                        serviceResult = commonFunctions.RestServiceCall(Constants.PROFILE_UPDATE, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        serviceResult = commonFunctions.RestServiceCall(Constants.PROFILE_ADD, Crypto.Instance.Encrypt(jsonInputParameter));

                    if (string.Compare(serviceResult, Constants.REST_CALL_FAILURE, true) == 0)
                    {
                        radMesaage.Title = "Alert";
                        radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);
                        e.Canceled = true;
                        return;

                    }
                    else
                    {
                        radMesaage.Title = "Sucesss";
                        radMesaage.Show(Constants.RECORD_SAVE_SUCESSFULLY);
                    }

                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion "Private Methods"

        protected void grdProfile_ItemCreated(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    CheckBox chkbox = (CheckBox)item.FindControl("chkStatus");               
            //    chkbox.Attributes.Add("onclick", "oncheckedChaned(this);");
            //}
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

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grdProfile.MasterTableView.ClearEditItems();
            grdProfile.MasterTableView.IsItemInserted = true;
            grdProfile.MasterTableView.Rebind();
        }


    }
}
