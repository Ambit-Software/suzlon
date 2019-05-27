using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.ImageEditor;

namespace SolarPMS.Admin
{
    public partial class UserDetail : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        string UserImgName = string.Empty;
       // int EditUserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);

            if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {               
                ViewState["EditUserId"] = Convert.ToString(Session["EditUserId"]);
                TxtPassword.Text = string.Empty;
                TxtUserId.Text = string.Empty;
               
                BindUserDropdown();         

                if (Convert.ToInt32(ViewState["EditUserId"]) !=0)
                {
                    BindUserDetail(Convert.ToInt32(ViewState["EditUserId"]));
                }           
            }
        }

        private void BindUserDetail(int UserId)
        {
            try
            {
                string result1 = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETUSERDTL + UserId + "", string.Empty);

                User UserInfo = JsonConvert.DeserializeObject<User>(result1);

                if (UserInfo != null)
                {
                    DrpAuthentication.SelectedValue = Convert.ToString(UserInfo.Authentication);
                    TxtUserId.Text = Convert.ToString(UserInfo.UserName);
                    TxtPassword.Attributes["value"] = UserInfo.Password;
                    TxtUserName.Text = Convert.ToString(UserInfo.Name);
                    TxtEmployeeID.Text = Convert.ToString(UserInfo.EmployeeId);
                    TxtEmailId.Text = Convert.ToString(UserInfo.EmailId);
                    TxtMobileNo.Text = Convert.ToString(UserInfo.MobileNo);
                    DrpLocation.SelectedValue = Convert.ToString(UserInfo.LocationId);
                    DrpStatus.SelectedValue = Convert.ToString(UserInfo.Status == true ? "Enabled" : "Disabled");
                    DrpProfile.SelectedValue = Convert.ToString(UserInfo.ProfileId);
                    DrpReportingMgr.SelectedValue = Convert.ToString(UserInfo.ReportingManagerId);
                    ImgEmp.ImageUrl = Convert.ToString(UserInfo.Photo);
                    if (!(string.IsNullOrEmpty(UserInfo.Photo)))
                        ViewState["ImageName"] = Convert.ToString(UserInfo.Photo);
                    else
                        ImgEmp.ImageUrl="~/Content/images/placeholder-icon.png";
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        private void BindUserDropdown()
        {
            try
            {
                string drpname = "location,profile,users";
                string result1 = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result1);

                DrpLocation.DataTextField = "Name";
                DrpLocation.DataValueField = "Id";
                DrpLocation.DataSource = ddValues.location;
                DrpLocation.DataBind();
                DrpLocation.EmptyMessage = "Select Location";

                DrpReportingMgr.DataTextField = "Name";
                DrpReportingMgr.DataValueField = "Id";
                DrpReportingMgr.DataSource = ddValues.users;
                DrpReportingMgr.DataBind();
                DrpReportingMgr.EmptyMessage = "Select Reporting Manager";

                DrpProfile.DataTextField = "Name";
                DrpProfile.DataValueField = "Id";
                DrpProfile.DataSource = ddValues.profile;
                DrpProfile.DataBind();
                DrpProfile.EmptyMessage = "Select Profile";
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            { 
            if (AsyncUpload1.UploadedFiles.Count > 0)
            {
                    if (e.File.GetExtension().Trim().ToUpper() == ".JPEG" || e.File.GetExtension().Trim().ToUpper() == ".PNG" || e.File.GetExtension().Trim().ToUpper() == ".JPG" || e.File.GetExtension().Trim().ToUpper() == ".GIF")
                    {
                        string FilePath = "~/Profile/";
                        String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                        string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
                        ViewState["ImageName"] = "~/Profile/" + newfilename;

                        if (!File.Exists(Server.MapPath(AsyncUpload1.TargetFolder) + newfilename))
                        {
                            AsyncUpload1.TargetFolder = FilePath;
                            e.File.SaveAs(Path.Combine(Server.MapPath(AsyncUpload1.TargetFolder), newfilename));
                            ImgEmp.ImageUrl = FilePath + newfilename;
                            Label1.Text = "*Size limit: 2MB";
                            Label1.ForeColor = System.Drawing.Color.Black;
                        }

                        else
                        {
                            Label1.Text = Constants.USERDETAIL_MSGFILENAME;
                            Label1.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        radMesaage.Title = "Alert";
                        radMesaage.Show(Constants.USER_IMAGEVALIDATION_MSG);
                    }
            }
            else
            {
                Label1.Text = Constants.USERDETAIL_MSGFILENAMEEMPTY;
                Label1.ForeColor = System.Drawing.Color.Red;
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
                    bool status;
                    bool previousStatus = false;
                    if (Convert.ToString(DrpStatus.SelectedValue) == "Enabled")
                        status = true;
                    else
                        status = false;

                    if (status==false)
                    {      
                     string result = commonFunctions.RestServiceCall(string.Format(Constants.USERDETAIL_DISABLEUSER, Convert.ToInt32(ViewState["EditUserId"])), string.Empty);

                        if (!Convert.ToBoolean(result))
                        {
                            radMesaage.Title = "Alert";
                            radMesaage.Show(Constants.USERDETAIL_MSGUSERDISABLE);
                            return;
                        }
                    }

                    User user = new User()
                    {
                        UserId = Convert.ToInt32(ViewState["EditUserId"]),
                        Authentication = Convert.ToString(DrpAuthentication.SelectedValue),
                        UserName = Convert.ToString(TxtUserId.Text),
                        Password = Convert.ToString(TxtPassword.Text),
                        EmailId = Convert.ToString(TxtEmailId.Text),
                        EmployeeId = Convert.ToString(TxtEmployeeID.Text),
                        //LocationId = Convert.ToInt32(DrpLocation.SelectedValue),
                        MobileNo = Convert.ToString(TxtMobileNo.Text),
                        ProfileId = Convert.ToInt32(DrpProfile.SelectedValue),
                        ReportingManagerId = Convert.ToInt32(DrpReportingMgr.SelectedValue),
                        Name = Convert.ToString(TxtUserName.Text),
                        Status = status,
                        Photo = Convert.ToString(Convert.ToString(ViewState["ImageName"])).Trim()

                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(user);
                    string result1 = string.Empty;
                    string recesxist = string.Empty;

                    recesxist = commonFunctions.RestServiceCall(Constants.CHECK_USERDTL_EXIST, Crypto.Instance.Encrypt(jsonInputParameter));
                    Dictionary<string, bool> fieldStatus = JsonConvert.DeserializeObject<Dictionary<string, bool>>(recesxist);
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
                        if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
                            result1 = commonFunctions.RestServiceCall(Constants.USERDETAIL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                        else
                        {
                            // if user status is disabled then check previous status
                            if (!user.Status)
                                previousStatus = UserModel.GetUserStatus(user.UserId);
                            result1 = commonFunctions.RestServiceCall(Constants.USERDETAIL_UPDATE, Crypto.Instance.Encrypt(jsonInputParameter));
                        }

                        if (string.Compare(result1, Constants.REST_CALL_FAILURE, true) == 0)
                        {
                            radMesaage.Title = "Alert";
                            radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);

                        }
                        else
                        {
                            
                            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendNewUserCreationNotification"]) && Convert.ToInt32(ViewState["EditUserId"]) == 0)
                            {
                                user = JsonConvert.DeserializeObject<Models.User>(result1);
                                NotificationHelper.SendUserCreationNotification(user.Name, user.UserId, user.UserName, Crypto.Instance.Decrypt(user.Password));
                                radMesaage.Title = "Sucesss";
                                radMesaage.Show(Constants.FAILED_TO_SEND_NOTIFICATION);
                            }
                            else if (!user.Status)
                            {
                                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendUserDeactivationNotification"]) &&  previousStatus)
                                    NotificationHelper.SendUserDeactivatedNotification(user.UserId, user.UserName);
                            }
                            Response.Redirect("~/Admin/UserMaster.aspx", true);
                        }
                    }
                    else
                    {

                        radMesaage.Title = "Alert";
                        radMesaage.Show(msg);
                    }
                }               
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                }


        }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/UserMaster.aspx");
        }

    }


}