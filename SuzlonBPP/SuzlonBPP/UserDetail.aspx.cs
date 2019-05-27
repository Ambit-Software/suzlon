using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

namespace SuzlonBPP.Admin
{
    public partial class UserDetail : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.USER_MANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");
                string Password = TxtPassword.Text;
                TxtPassword.Attributes.Add("value", Password);
                if (!IsPostBack)
                {
                    hidSerachUserId.Value = "0";
                    int profileId = Convert.ToInt32(Session["ProfileId"]);
                    if (profileId == (int)UserProfileEnum.Vendor)
                        hidSerachUserId.Value = Convert.ToString(HttpContext.Current.Session["UserId"]);
                    ViewState["EditUserId"] = Convert.ToString(Session["EditUserId"]);
                    BindDropdownData();
                    TxtPassword.Text = string.Empty;
                    TxtUserId.Text = string.Empty;
                    DrpStatus.SelectedValue = "Enabled";
                    if (Convert.ToInt32(ViewState["EditUserId"]) != 0)
                        BindUserDetail(Convert.ToInt32(ViewState["EditUserId"]));
                }
                
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindUserDetail(int UserId)
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETUSERDTL + UserId + "", string.Empty);
                if (result == Constants.REST_CALL_FAILURE)
                {

                }
                else
                {
                    UserModel userModel= JsonConvert.DeserializeObject<UserModel>(result);

                    if (userModel != null)
                    {
                        User UserInfo = userModel.UserDetail;
                        if (UserInfo != null)
                        {

                            DrpAuthentication.SelectedValue = Convert.ToString(UserInfo.Authentication);
                            TxtUserId.Text = Convert.ToString(UserInfo.UserName);
                            TxtPassword.Attributes["value"] = UserInfo.Password;
                            TxtUserName.Text = Convert.ToString(UserInfo.Name);
                            TxtEmployeeID.Text = Convert.ToString(UserInfo.EmployeeId);
                            TxtEmailId.Text = Convert.ToString(UserInfo.EmailId);
                            TxtMobileNo.Text = Convert.ToString(UserInfo.MobileNo);
                            DrpStatus.SelectedValue = Convert.ToString(UserInfo.Status == true ? "Enabled" : "Disabled");
                            DrpProfile.SelectedValue = Convert.ToString(UserInfo.ProfileId);
                            HideShowFields(UserInfo.ProfileId);
                            if (UserInfo.Company != null)
                            {
                                var array = UserInfo.Company.Split(',');
                                if (array != null && array.Length > 0)
                                {
                                    for (int count = 0; count < array.Length; count++)
                                    {
                                        int index = DrpCompany.FindItemIndexByValue(array[count]);
                                        var item = DrpCompany.Items[index];
                                        item.Checked = true;
                                    }
                                }
                            }
                            if (UserInfo.Vertical != null)
                            {
                                var array = UserInfo.Vertical.Split(',');
                                if (array != null && array.Length > 0)
                                {
                                    for (int count = 0; count < array.Length; count++)
                                    {
                                        int index = DrpVertical.FindItemIndexByValue(array[count]);
                                        var item = DrpVertical.Items[index];
                                        item.Checked = true;
                                    }
                                }
                            }
                            GetSubVerticals(Convert.ToString(UserInfo.Vertical));
                            if (UserInfo.SubVertical != null)
                            {
                                var array = UserInfo.SubVertical.Split(',');
                                if (array != null && array.Length > 0)
                                {
                                    for (int count = 0; count < array.Length; count++)
                                    {
                                        int index = DrpSubVertical.FindItemIndexByValue(array[count]);
                                        var item = DrpSubVertical.Items[index];
                                        item.Checked = true;
                                    }
                                }
                            }
                            if (UserInfo.VendorCode != null)
                            {
                                userModel.VendorDetails.ForEach(v =>
                               {
                                   VendorSelectedList.Items.Add(new RadListBoxItem(v.VendorName, v.VendorCode));
                               });
                            }
                            if (!(string.IsNullOrEmpty(UserInfo.Photo)))
                            {
                                ViewState["ImageName"] = Convert.ToString(UserInfo.Photo);
                                ImgEmp.ImageUrl = Convert.ToString(UserInfo.Photo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindDropdownData()
        {
            try
            {
                string drpname = "profile,company,vertical";
                string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                DrpProfile.EmptyMessage = "Select Profile";
                DrpProfile.DataSource = ddValues.Profile;
                DrpProfile.DataTextField = "Name";
                DrpProfile.DataValueField = "Id";
                DrpProfile.DataBind();
                //DrpProfile.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Profile", String.Empty));
                //DrpProfile.SelectedIndex = 0;

                DrpVertical.DataSource = ddValues.Vertical;
                DrpVertical.DataBind();

                DrpCompany.DataSource = ddValues.Company;
                DrpCompany.DataBind();

                //AutoComVendorCode.DataSource = ddValues.VendorCode;
                //AutoComVendorCode.DataBind();

                DrpSubVertical.DataSource = new System.Data.DataTable();
                DrpSubVertical.DataBind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        //private void BindVendorCode()
        //{
        //    try
        //    {
        //        string drpname = "vendorcode";
        //        string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty, this);
        //        DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
        //        AutoComVendorCode.DataSource = ddValues.VendorCode;
        //        AutoComVendorCode.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.WriteErrorLog(ex);
        //    }
        //}


        protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                if (AsyncUpload1.UploadedFiles.Count > 0)
                {
                    if (e.File.GetExtension().Trim().ToUpper()== ".JPEG"|| e.File.GetExtension().Trim().ToUpper() == ".PNG"|| e.File.GetExtension().Trim().ToUpper() == ".JPG"|| e.File.GetExtension().Trim().ToUpper() == ".GIF")
                    {
                        string FilePath = "Profile/";
                        String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                        string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
                        ViewState["ImageName"] = "Profile/" + newfilename;

                        if (!File.Exists(Server.MapPath(AsyncUpload1.TargetFolder) + newfilename))
                        {
                            AsyncUpload1.TargetFolder = FilePath;
                            e.File.SaveAs(Path.Combine(Server.MapPath(AsyncUpload1.TargetFolder), newfilename));
                            ImgEmp.ImageUrl = FilePath + newfilename;
                            lblFileUploadMsgs.Text = "*Size limit: 2MB";
                            lblFileUploadMsgs.ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            lblFileUploadMsgs.Text = Constants.USERDETAIL_MSGFILENAME;
                            lblFileUploadMsgs.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        radMesaage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMesaage.Show(Constants.USERPROFILE_IMAGEVALIDATION_MSG);
                       
                    }
                }
                else
                {
                    lblFileUploadMsgs.Text = Constants.USERDETAIL_MSGFILENAMEEMPTY;
                    lblFileUploadMsgs.ForeColor = System.Drawing.Color.Red;
                }



            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    User user = new User();
                    user.UserId = Convert.ToInt32(ViewState["EditUserId"]);
                    user.Authentication = Convert.ToString(DrpAuthentication.SelectedValue);
                    user.UserName = Convert.ToString(TxtUserId.Text);
                    user.Password = Convert.ToString(TxtPassword.Text);
                    user.EmailId = Convert.ToString(TxtEmailId.Text);
                    user.EmployeeId = Convert.ToString(TxtEmployeeID.Text);
                    user.MobileNo = Convert.ToString(TxtMobileNo.Text);
                    user.ProfileId = Convert.ToInt32(DrpProfile.SelectedValue);
                    if (user.ProfileId == (int)UserProfileEnum.Vendor)
                    {
                        user.Company = null;
                        user.Vertical = null;
                        user.SubVertical = null;
                        user.VendorCode = null;
                        if (VendorSelectedList.Items.Count > 0)
                        {
                            for (int count = 0; count < VendorSelectedList.Items.Count; count++)
                            {
                                if (user.VendorCode == null)
                                    user.VendorCode = VendorSelectedList.Items[count].Value;
                                else if (!user.VendorCode.Contains(VendorSelectedList.Items[count].Value))
                                    user.VendorCode = user.VendorCode + "," + VendorSelectedList.Items[count].Value;
                            }
                        }
                        //user.VendorCode = GetCheckedIds(AutoComVendorCode.CheckedItems);
                    }
                    else
                    {
                        user.Company = GetCheckedIds(DrpCompany.CheckedItems);
                        user.Vertical = GetCheckedIds(DrpVertical.CheckedItems);
                        user.SubVertical = GetCheckedIds(DrpSubVertical.CheckedItems);

                        user.VendorCode = null;
                    }
                    user.Name = Convert.ToString(TxtUserName.Text);
                    user.Status = (Convert.ToString(DrpStatus.SelectedValue) == "Enabled" ? true : false);
                    user.Photo = Convert.ToString(Convert.ToString(ViewState["ImageName"])).Trim();

                    string jsonInputParameter = JsonConvert.SerializeObject(user);
                    string result = string.Empty;
                    string isRecordExistResult = string.Empty;

                    isRecordExistResult = commonFunctions.RestServiceCall(Constants.CHECK_USERDTL_EXIST, Crypto.Instance.Encrypt(jsonInputParameter));
                    radMesaage.Title = Constants.RAD_MESSAGE_TITLE;
                    if (result == Constants.REST_CALL_FAILURE)
                    {
                        radMesaage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    }
                    else
                    {
                        Dictionary<string, bool> fieldStatus = JsonConvert.DeserializeObject<Dictionary<string, bool>>(isRecordExistResult);
                        string msg = string.Empty;
                        if (fieldStatus.ContainsKey("employeeId") && fieldStatus["employeeId"])
                        {
                            msg = Constants.USERDETAIL_MSGEMPPIDEXIST;
                        }
                        if (fieldStatus.ContainsKey("emailId") && fieldStatus["emailId"])
                        {
                            if (string.IsNullOrEmpty(msg))
                                msg = Constants.USERDETAIL_MSGEMAILID;
                            else
                                msg = msg + "</BR>" + Constants.USERDETAIL_MSGEMAILID;
                        }
                        if (fieldStatus.ContainsKey("userName") && fieldStatus["userName"])
                        {
                            if (string.IsNullOrEmpty(msg))
                                msg = Constants.USERDETAIL_MSGUSERNAME;
                            else
                                msg = msg + "</BR>" + Constants.USERDETAIL_MSGUSERNAME;

                        }

                        if (string.IsNullOrEmpty(msg))
                        {
                            try
                            {
                                if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
                                {
                                    result = commonFunctions.RestServiceCall(Constants.USERDETAIL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                                    if (result == Constants.REST_CALL_FAILURE)
                                        radMesaage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                                    else
                                        Response.Redirect("UserMaster.aspx");
                                }
                                else
                                {
                                    bool isUserDependencyExists = false;
                                    if (!user.Status)
                                        isUserDependencyExists = IsUserDependencyExists();
                                    if (!isUserDependencyExists)
                                    {
                                        result = commonFunctions.RestServiceCall(Constants.USERDETAIL_UPDATE, Crypto.Instance.Encrypt(jsonInputParameter));
                                        if (result == Constants.REST_CALL_FAILURE)
                                            radMesaage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                                        else
                                            Response.Redirect("UserMaster.aspx");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                radMesaage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                                CommonFunctions.WriteErrorLog(ex);
                            }
                        }
                        else
                        {
                            radMesaage.Show(msg);
                            //lblErrorMsg.Text = msg;
                        }
                    }
                }
                catch (Exception ex)
                {
                    radMesaage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    CommonFunctions.WriteErrorLog(ex);
                }
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserMaster.aspx");
        }

        protected void AutoComVendorCode_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = e.Item.Text.ToLower().Split(' ')[0];
        }

        protected void DrpVertical_SelectedIndexChanged(object sender, EventArgs e)
        {
            string verticalIds = GetCheckedIds(DrpVertical.CheckedItems);
            GetSubVerticals(verticalIds);
        }

        protected void GetSubVerticals(string verticalIds)
        {
            List<ListItem> lstSubVertical = new List<ListItem>();
            if (string.IsNullOrEmpty(verticalIds))
            {
                DrpSubVertical.DataSource = lstSubVertical;
                DrpSubVertical.DataBind();
            }
            else
            {
                var result = commonFunctions.RestServiceCall(string.Format(Constants.SUBVERTICAL_BYVERTICAL, verticalIds), string.Empty);
                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMesaage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    DrpSubVertical.DataSource = lstSubVertical;
                    DrpSubVertical.DataBind();
                }
                else
                {
                    lstSubVertical = JsonConvert.DeserializeObject<List<ListItem>>(result);
                    DrpSubVertical.DataSource = lstSubVertical;
                    DrpSubVertical.DataBind();
                }
            }
        }

        //protected void DrpProfile_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    HideShowFields(Convert.ToInt32(DrpProfile.SelectedValue));
        //}

        private void HideShowFields(int profileId)
        {
            if (profileId == (int)UserProfileEnum.Vendor)
            {
                DrpVertical.Visible = false;
                DrpSubVertical.Visible = false;
                DrpCompany.Visible = false;
                AutoComVendorCode.Visible = true;
                lblCompany.Visible = false;
                lblVertical.Visible = false;
                lblSubVertical.Visible = false;
                lblVendorCode.Visible = true;
                rfvCompany.Enabled = false;
                rfvVertical.Enabled = false;
                rfvSubVertical.Enabled = false;
                rfvVendorcode.Enabled = true;
                panelVendorCode.Visible = true;
            }
            else
            {
                DrpVertical.Visible = true;
                DrpSubVertical.Visible = true;
                DrpCompany.Visible = true;
                AutoComVendorCode.Visible = false;
                lblCompany.Visible = true;
                lblVertical.Visible = true;
                lblSubVertical.Visible = true;
                lblVendorCode.Visible = false;
                rfvCompany.Enabled = true;
                rfvVertical.Enabled = true;
                rfvSubVertical.Enabled = true;
                rfvVendorcode.Enabled = false;
                panelVendorCode.Visible = false;                
            }
        }
        private string GetCheckedIds(IList<RadComboBoxItem> collection)
        {
            string checkedIds = string.Empty;
            if (collection != null && collection.Count > 0)
            {
                for (int count = 0; count < collection.Count; count++)
                {
                    var splitArray = collection[count].Value.Split(new string[] { Constants.SEPERATOR }, StringSplitOptions.None);
                    if (string.IsNullOrEmpty(checkedIds))
                        checkedIds = splitArray[0];
                    else
                        checkedIds = checkedIds + "," + splitArray[0];
                }
            }
            return checkedIds;
        }

        private bool IsUserDependencyExists()
        {
            bool isUserAssigenedToWorkflow = true;
            string result = commonFunctions.RestServiceCall(string.Format(Constants.CHECK_USER_DEPENDENCY, Convert.ToInt32(ViewState["EditUserId"])), string.Empty);
            if (result == Constants.REST_CALL_FAILURE)
                radMesaage.Show(Constants.ERROR_OCC_WHILE_SAVING);
            else
                isUserAssigenedToWorkflow = JsonConvert.DeserializeObject<bool>(result);
            if (isUserAssigenedToWorkflow)
                radMesaage.Show(Constants.USER_ASSIGNED_TO_WORKFLOW_MSG);
            return isUserAssigenedToWorkflow;
        }

        protected void DrpCompany_DataBound(object sender, EventArgs e)
        {
            RadComboBox combo = sender as RadComboBox;
            combo.EnableCheckAllItemsCheckBox = combo.Items.Count > 0;
        }

        //protected void AutoComVendorCode_DataBound(object sender, EventArgs e)
        //{
        //    RadComboBox combo = sender as RadComboBox;
        //    combo.EnableCheckAllItemsCheckBox = combo.Items.Count > 0;
        //}

        protected void DrpVertical_DataBound(object sender, EventArgs e)
        {
            RadComboBox combo = sender as RadComboBox;
            combo.EnableCheckAllItemsCheckBox = combo.Items.Count > 0;
        }

        protected void DrpSubVertical_DataBound(object sender, EventArgs e)
        {
            RadComboBox combo = sender as RadComboBox;
            combo.EnableCheckAllItemsCheckBox = combo.Items.Count > 0;
        }

        protected void DrpProfile_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            HideShowFields(Convert.ToInt32(DrpProfile.SelectedValue));
        }

        protected void AutoComVendorCode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            AddToVendorList(e.Text);
        }

        protected void AutoComVendorCode_CheckAllCheck(object sender, RadComboBoxCheckAllCheckEventArgs e)
        {
            
        }

        private void AddToVendorList(string vendorCode)
        {
            bool isListContainValue = false;
            for (int count = 0; count < VendorSelectedList.Items.Count; count++)
            {
                if (VendorSelectedList.Items[count].Text == vendorCode)
                    isListContainValue = true;
            }
            if (!isListContainValue)
                VendorSelectedList.Items.Add(new RadListBoxItem(vendorCode, vendorCode));
        }

        protected void EntityDataSource1_Selecting(object sender, System.Web.UI.WebControls.EntityDataSourceSelectingEventArgs e)
        {
            e.DataSource.WhereParameters["searchText"].DefaultValue = txtVendorSearch.Text;
        }

        protected void txtVendorSearch_TextChanged(object sender, EventArgs e)
        {
            EntityDataSource1.WhereParameters["searchText"].DefaultValue = txtVendorSearch.Text;
        }
    }
}