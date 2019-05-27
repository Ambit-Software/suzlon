using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Configuration;

namespace SolarPMS
{
    public partial class IssueManagementDetail : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();     
        DateTime issueExpCloseDate;
       
        List<CommentDtl> CommentLst = new List<CommentDtl>();
        List<CommentDtl> CommentLstAdd = new List<CommentDtl>();

        List<Attachement> AttachmentLst = new List<Attachement>();
        List<Attachement> AllAttachmentlst = new List<Attachement>();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                if (Request.Url.ToString().Contains("IssueManagementDetail.aspx?"))
                    this.MasterPageFile = "~/MasterPages/BlankMaster.Master";
                else
                    this.MasterPageFile = "~/MasterPages/SolarPMS.Master";
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null && !string.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                Response.Redirect(ConfigurationManager.AppSettings["WebsiteUrl"] + "/Login.aspx?ReturnURL=" + HttpContext.Current.Request.Url.AbsoluteUri, true);
            }
            else
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.ISSUEMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");

                if (!string.IsNullOrEmpty(Request.QueryString.ToString()) && Request.Url.ToString().Contains("TimesheetManagement.aspx?"))
                {
                    string[] queryString = Request.QueryString.ToString().Split('?');
                    string IssueId = Crypto.Instance.Decrypt(HttpUtility.UrlDecode(queryString[0])).Split('=')[1];
                    Session["IssueId"] = IssueId;
                }
                if (!IsPostBack)
                {
                    BindUserDropdown();

                    if (Convert.ToInt32(Session["IssueId"]) != 0)
                    {
                        lblIssueMode.Text = "Edit Issue";
                        BindEditDetail(Convert.ToInt32(Session["IssueId"]));
                        lblAssignFrom.Visible = true;
                        txtAssignFrom.Visible = true;
                        txtAssignFrom.Enabled = false;
                        drpIssueCategory.Enabled = true;
                        lblStatus.Visible = true;
                        drpStatus.Visible = true;
                        BindField();
                    }
                    else
                    {
                        lblIssueMode.Text = "Add Issue";
                        drpIssueCategory.Enabled = true;
                        lblAssignFrom.Visible = false;
                        txtAssignFrom.Visible = false;
                        lblStatus.Visible = false;
                        drpStatus.Visible = false;
                        issueDate.SelectedDate = System.DateTime.Now;
                        BtnSubmit.Visible = true;
                        BtnSave.Visible = true;

                        if (Convert.ToString(Session["ProfileName"]) == "Admin")
                        {
                            issueDate.MaxDate = DateTime.Today;
                        }
                        else
                        {
                            issueDate.MaxDate = DateTime.Today;
                            issueDate.MinDate = DateTime.Today.AddDays(-1);
                        }
                        expectedClosureDate.MinDate = DateTime.Today;

                    }

                }
            }
        }
        private void BindField()
        {
            if (Convert.ToString(ViewState["IssueStatus"]) == "Closed")
            {
                BtnSubmit.Visible = false;
                BtnSave.Visible = false;
            }
            else
            {
                if (Convert.ToInt32(ViewState["CreatedBy"]) == Convert.ToInt32(HttpContext.Current.Session["UserId"]))
                {
                    if ((Convert.ToString(ViewState["IssueStatus"]) == "Draft"
                        && Convert.ToInt32(ViewState["AssignTo"]) == Convert.ToInt32(HttpContext.Current.Session["UserId"])))
                    {
                        lblAssignFrom.Visible = true;
                        txtAssignFrom.Visible = true;
                        txtAssignFrom.Enabled = false;
                        drpIssueCategory.Enabled = true;
                        lblStatus.Visible = true;
                        drpStatus.Visible = true;
                        drpLocation.Enabled = true;
                        issueDate.Enabled = true;
                        expectedClosureDate.Enabled = true;
                        txtDecription.Enabled = true;
                        drpAssignTo.Enabled = true;
                        txtResolution.Enabled = true;
                        drpStatus.Enabled = true;
                        BtnSubmit.Visible = true;
                        BtnSave.Visible = true;
                        txtCommentBox.Enabled = true;
                    }
                    else
                    {
                        if (Convert.ToString(ViewState["IssueStatus"]) == "Resolved")
                        {
                            lblAssignFrom.Visible = true;
                            txtAssignFrom.Visible = true;
                            txtAssignFrom.Enabled = false;
                            lblStatus.Visible = true;
                            drpStatus.Visible = true;
                            drpIssueCategory.Enabled = false;
                            drpLocation.Enabled = false;
                            issueDate.Enabled = false;
                            expectedClosureDate.Enabled = false;
                            txtDecription.Enabled = false;
                            drpAssignTo.Enabled = false;
                            txtResolution.Enabled = false;
                            BtnImport1.Enabled = false;
                            txtCommentBox.Enabled = true;
                            BtnSubmit.Visible = true;
                            BtnSave.Visible = true;
                            drpStatus.Enabled = true;
                            drpAssignTo.Enabled = true;
                        }
                        else
                        {
                            lblAssignFrom.Visible = true;
                            txtAssignFrom.Visible = true;
                            txtAssignFrom.Enabled = false;
                            lblStatus.Visible = true;
                            drpStatus.Visible = true;
                            drpIssueCategory.Enabled = false;
                            drpLocation.Enabled = false;
                            issueDate.Enabled = false;
                            expectedClosureDate.Enabled = false;
                            txtDecription.Enabled = false;
                            drpAssignTo.Enabled = false;
                            txtResolution.Enabled = false;
                            drpStatus.Enabled = false;
                            BtnImport1.Enabled = false;
                            //txtCommentBox.Enabled = false;
                            BtnSubmit.Visible = false;
                            BtnSave.Visible = false;
                            if (Convert.ToInt32(ViewState["AssignTo"]) == Convert.ToInt32(HttpContext.Current.Session["UserId"]))
                            {
                                BtnSubmit.Visible = true;
                                drpAssignTo.Enabled = true;
                                drpStatus.Enabled = true;
                            }
                        }
                    }
                }

                if (Convert.ToInt32(ViewState["CreatedBy"]) != Convert.ToInt32(HttpContext.Current.Session["UserId"]))
                {
                    if (Convert.ToInt32(ViewState["AssignTo"]) == Convert.ToInt32(HttpContext.Current.Session["UserId"]))
                    {
                        lblAssignFrom.Visible = true;
                        txtAssignFrom.Visible = true;
                        txtAssignFrom.Enabled = false;
                        drpIssueCategory.Enabled = true;
                        lblStatus.Visible = true;
                        drpStatus.Visible = true;
                        drpLocation.Enabled = true;
                        issueDate.Enabled = false;
                        drpIssueCategory.Enabled = false;
                        expectedClosureDate.Enabled = true;
                        txtDecription.Enabled = true;
                        drpAssignTo.Enabled = true;
                        txtResolution.Enabled = true;
                        drpStatus.Enabled = true;
                        BtnImport1.Enabled = false;
                        drpStatus.Items.Remove(new System.Web.UI.WebControls.ListItem("Closed", "Closed"));

                        if (Convert.ToString(ViewState["IssueStatus"]) == "Resolved")
                        {
                            BtnSave.Visible = false;
                            BtnSubmit.Visible = false;
                        }
                    }
                    else
                    {
                        lblAssignFrom.Visible = true;
                        txtAssignFrom.Visible = true;
                        txtAssignFrom.Enabled = false;
                        lblStatus.Visible = true;
                        drpStatus.Visible = true;
                        drpIssueCategory.Enabled = false;
                        drpLocation.Enabled = false;
                        issueDate.Enabled = false;
                        expectedClosureDate.Enabled = false;
                        txtDecription.Enabled = false;
                        drpAssignTo.Enabled = false;
                        txtResolution.Enabled = false;
                        drpStatus.Enabled = false;
                      
                        BtnSave.Visible = false;
                        BtnSubmit.Visible = false;
                        BtnImport1.Enabled = false;
                        if (Convert.ToString(Session["ProfileName"]) == "Admin")
                        {
                            drpIssueCategory.Enabled = true;
                            BtnSubmit.Visible = true;
                            txtCommentBox.Enabled = true;
                        }

                    }

                }
            }


        }

        private void BindEditDetail(int issueId)
        {
            string result1 = commonFunctions.RestServiceCall(string.Format(Constants.ISSUE_MGT_GET_ISSUEDETAIL, issueId), string.Empty);

            if (result1 != null)
            {
                IssueManagementModelDtl issueDetail = JsonConvert.DeserializeObject<IssueManagementModelDtl>(result1);
                ViewState["CreatedBy"] = issueDetail.CreatedBy;
                ViewState["IssueStatus"] = issueDetail.IssueStatus;
                ViewState["AssignTo"] = issueDetail.AssignedTo;
               // issueDate.SelectedDate = issueDetail.IssueDate;
                //issueRaiseDate = Convert.ToDateTime(issueDetail.IssueDate);
                drpIssueCategory.SelectedValue = Convert.ToString(issueDetail.CategoryId);
                drpLocation.SelectedValue = Convert.ToString(issueDetail.LocationId);
                txtDecription.Text = Convert.ToString(issueDetail.Description);
                expectedClosureDate.SelectedDate = issueDetail.ExpectedClosureDate;
                issueExpCloseDate = Convert.ToDateTime(issueDetail.ExpectedClosureDate);
                txtResolution.Text = Convert.ToString(issueDetail.Resolution);
                drpAssignTo.SelectedValue = Convert.ToString(issueDetail.AssignedTo);
                txtAssignFrom.Text = Convert.ToString(issueDetail.name);
                drpStatus.SelectedValue = Convert.ToString(issueDetail.IssueStatus);
                txtLocationType.Text = issueDetail.LocationType;
                List<IssueCommentModelDtl> issueCommentList = new List<IssueCommentModelDtl>(issueDetail.IssueComments);

                foreach (var cmmt in issueCommentList)
                {
                    CommentDtl cmtdtl = new CommentDtl();
                    cmtdtl.UserName = cmmt.name.ToString();
                    cmtdtl.Comment = cmmt.Comment;
                    CommentLst.Add(cmtdtl);
                }

                StringBuilder CommentStr = new StringBuilder();
                CommentStr = CommentStr.Append("<head> <table>");
                foreach (var Comlst in CommentLst)
                {
                    CommentStr.Append(" <tr><td> " + Comlst.UserName + " </td><td></td></tr>");
                    CommentStr.Append("<tr><td></td><td>" + Comlst.Comment + "</td></tr>");
                }
                CommentStr.Append("</table></head>");
                lblCommentDetail.Text = CommentStr.ToString();
                ViewState["Comment"] = CommentLst;

                List<IssueAttachment> issueAttachment = new List<IssueAttachment>(issueDetail.IssueAttachments);
                foreach (var attachment in issueAttachment)
                {                   
                    Attachement attachmentDetail = new Attachement() { FileName = attachment.FilePath };
                    //AttachmentLst.Add(attachmentDetail);
                    AllAttachmentlst.Add(attachmentDetail);
                }

                ViewState["AllAttachmentName"] = AllAttachmentlst;
                grdIssueAttachment.DataSource = AllAttachmentlst;
                grdIssueAttachment.DataBind();

                if (Convert.ToString(Session["ProfileName"]) == "Admin")
                {
                    issueDate.MaxDate = Convert.ToDateTime(issueDetail.IssueDate);
                    issueDate.SelectedDate = Convert.ToDateTime(issueDetail.IssueDate);
                }
                else
                {

                    issueDate.MaxDate = Convert.ToDateTime(issueDetail.IssueDate); 
                    issueDate.MinDate = Convert.ToDateTime(issueDetail.IssueDate);
                    issueDate.SelectedDate = Convert.ToDateTime(issueDetail.IssueDate);
                }
               
                expectedClosureDate.MinDate = Convert.ToDateTime(issueDetail.ExpectedClosureDate);
                expectedClosureDate.SelectedDate = Convert.ToDateTime(issueDetail.ExpectedClosureDate);

            }

        }
        private void BindUserDropdown()
        {
            try
            {
                string drpname = "location,users,category";
                string result1 = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);

                if (!string.IsNullOrEmpty(result1))
                {
                    DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result1);
                    drpIssueCategory.DataTextField = "Name";
                    drpIssueCategory.DataValueField = "Id";
                    drpIssueCategory.DataSource = ddValues.category;
                    drpIssueCategory.DataBind();
                    //drpIssueCategory.Items.Insert(0, new RadComboBoxItem() { Text = "Select Issue Category", Value = String.Empty });
                    //drpIssueCategory.SelectedIndex = 0;

                    drpLocation.DataTextField = "Name";
                    drpLocation.DataValueField = "Id";
                    drpLocation.DataSource = ddValues.location;
                    drpLocation.DataBind();
                    //drpLocation.Items.Insert(0, new RadComboBoxItem() { Text = "Select Location", Value = String.Empty });
                    drpLocation.SelectedValue = Convert.ToString(Session["LoginUserLocationId"]);


                    drpAssignTo.DataTextField = "Name";
                    drpAssignTo.DataValueField = "Id";
                    drpAssignTo.DataSource = ddValues.users;
                    drpAssignTo.DataBind();
                    //drpAssignTo.Items.Insert(0, new RadComboBoxItem() { Text = "Select Assign To", Value = String.Empty });
                    //drpAssignTo.SelectedIndex = 0;
                }
                else
                {
                    drpIssueCategory.DataSource = new DataTable();
                    drpIssueCategory.DataBind();
                    drpLocation.DataSource = new DataTable();
                    drpLocation.DataBind();
                    drpAssignTo.DataSource = new DataTable();
                    drpAssignTo.DataBind();
                    
                }

                drpIssueCategory.EmptyMessage = "Select Issue Category";
                drpAssignTo.EmptyMessage = "Select Assign To";
                drpLocation.EmptyMessage = "Select Location";

            }
            catch (Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
            }
        }
                
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ViewState["AttachmentName"]) != "")
                {
                    AttachmentLst = (List<Attachement>)ViewState["AttachmentName"];
                }
                if (Convert.ToString(ViewState["CommentAdd"]) != "")
                {
                    CommentLst = (List<CommentDtl>)ViewState["CommentAdd"];
                }

                IssueAttachment issueAttachment = new IssueAttachment();
                IssueComment issueComment = new IssueComment();

                List<IssueAttachment> issueAttachmentList = new List<IssueAttachment>();
                List<IssueComment> issueCommentList = new List<IssueComment>();

                foreach (var attch in AttachmentLst)
                {
                    issueAttachment = new IssueAttachment()
                    {
                        FilePath = attch.FileName.ToString()
                    };
                    issueAttachmentList.Add(issueAttachment);
                }

                foreach (var cmmt in CommentLst)
                {
                    issueComment = new IssueComment()
                    {
                        Comment = cmmt.Comment
                    };
                    issueCommentList.Add(issueComment);
                }

                if (issueCommentList.Count == 0)
                {
                    radMesaage.Title = "Alert";
                    radMesaage.Show(Constants.ISSUE_MGT_ENTER_COMMENT);
                    return;
                }


                string issueStatus = string.Empty;
                if (Convert.ToInt32(Session["IssueId"]) == 0)
                {
                    issueStatus = "Open";

                }
                else
                {
                    issueStatus = Convert.ToString(drpStatus.SelectedValue);
                }

                int activityId = 0;
                int subActivityId = 0;
                if (Session[Constants.CONST_SESSION_ACTIVITY_ID] != null)
                    activityId = Convert.ToInt32(Session[Constants.CONST_SESSION_ACTIVITY_ID]);

                if (Session[Constants.CONST_SESSION_SUBACTIVITY_ID] != null)
                    subActivityId = Convert.ToInt32(Session[Constants.CONST_SESSION_SUBACTIVITY_ID]);


                IssueManagement issueManagement = new IssueManagement()
                {
                    IssueId = Convert.ToInt32(Session["IssueId"]),
                    IssueStatus = issueStatus,
                    IssueDate = Convert.ToDateTime(issueDate.SelectedDate),
                    CategoryId = Convert.ToInt32(drpIssueCategory.SelectedValue),
                    LocationId = string.IsNullOrEmpty(drpLocation.SelectedValue) ? 0 : Convert.ToInt32(drpLocation.SelectedValue),
                    Description = Convert.ToString(txtDecription.Text),
                    ExpectedClosureDate = Convert.ToDateTime(expectedClosureDate.SelectedDate),
                    AssignedTo = Convert.ToInt32(drpAssignTo.SelectedValue),
                    Resolution = Convert.ToString(txtResolution.Text),
                    IssueAttachments = issueAttachmentList,
                    IssueComments = issueCommentList,
                    ActivityId = activityId,
                    SubActivityId = subActivityId,
                    LocationType = txtLocationType.Text.Trim()
                };

                string jsonInputParameter = JsonConvert.SerializeObject(issueManagement);
                string serviceResult = string.Empty;


                if (Convert.ToInt32(Session["IssueId"]) == 0)
                    serviceResult = commonFunctions.RestServiceCall(Constants.ISSUE_MGT_ADD_ISSUE, Crypto.Instance.Encrypt(jsonInputParameter));
                else
                    serviceResult = commonFunctions.RestServiceCall(Constants.ISSUE_MGT_UPDATE_ISSUE, Crypto.Instance.Encrypt(jsonInputParameter));


                if (string.Compare(serviceResult, Constants.REST_CALL_FAILURE, true) == 0)
                {
                    radMesaage.Title = "Alert";
                    radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);
                    return;
                }
                else
                {
                    issueManagement = JsonConvert.DeserializeObject<Models.IssueManagement>(serviceResult);
                    string activityDescription = string.Empty;
                    bool notificationResult = false;
                    if (Session[Constants.CONST_SESSION_ACTIVITY_DESCRIPTION] != null)
                        activityDescription = Convert.ToString(Session[Constants.CONST_SESSION_ACTIVITY_DESCRIPTION]);

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendIssueAssignedNotification"])
                            && Convert.ToInt32(Session["IssueId"]) == 0 && issueManagement.IssueStatus == "Open")
                    {
                        notificationResult = NotificationHelper.SendIssueAssignementNotification(issueManagement, Convert.ToInt32(Session["UserId"]),
                                    Convert.ToString(Session["LoginUserName"]));
                    }
                    else if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendIssueResolutionNotification"]) &&
                            issueManagement.IssueStatus == "Resolved" && issueManagement.AssignedTo == Convert.ToInt32(Session["UserId"]))
                    {
                        notificationResult = NotificationHelper.SendIssueResolutionNotification(issueManagement, Convert.ToString(Session["LoginUserName"]));
                    }
                    else if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendIssueClosedNotification"]) &&
                        issueManagement.IssueStatus == "Closed")
                    {
                        notificationResult = NotificationHelper.SendIssueClosedNotification(issueManagement, Convert.ToString(Session["LoginUserName"]));
                    }
                }

                if (activityId != 0 || subActivityId != 0)
                {
                    radMesaage.Title = "Success";
                    radMesaage.Show("Issue saved successfully.");
                    RadAjaxManager1.ResponseScripts.Add("CloseModal();");
                    Session[Constants.CONST_SESSION_ACTIVITY_ID] = null;
                    Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = null;
                    Session["IssueId"] = null;
                }
                else
                    Response.Redirect("~/IssueManagementList.aspx");
            }
            catch (Exception ex)
            {
                radMesaage.Title = "Alert";
                radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void issueDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (string.Compare(Convert.ToString(Session["ProfileName"]), "Admin", true) != 0)
            {
                var Currendate = DateTime.Now.Date;
                var issDate = Convert.ToDateTime(issueDate.SelectedDate).AddDays(1);
                if (Currendate > issDate)
                {
                    issueDate.SelectedDate = null;
                }
             

            }
        }

        protected void expectedClosureDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            //var issueDate = DateTime.Now.Date;
            var slectedIssueDate = Convert.ToDateTime(issueDate.SelectedDate);
            var expCloseDate = Convert.ToDateTime(expectedClosureDate.SelectedDate);

            if (slectedIssueDate >= expCloseDate)
            {
                expectedClosureDate.SelectedDate = null;
            }
        }

        protected void BtnImport1_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
        {
            try
            {
                String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                string newfilename = (e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension()).Replace(" ", "_");
                e.File.SaveAs(Path.Combine(Server.MapPath(BtnImport1.TargetFolder), newfilename));

                if (Convert.ToString(ViewState["AllAttachmentName"]) != "")
                {                    
                    AllAttachmentlst = (List<Attachement>)ViewState["AllAttachmentName"];
                }
               

                if (Convert.ToString(ViewState["AttachmentName"]) != "")
                {
                    AttachmentLst = (List<Attachement>)ViewState["AttachmentName"];
                }                

                AttachmentLst.Add(new Attachement()
                {
                    FileName = newfilename
                });

                AllAttachmentlst.Add(new Attachement()
                {
                    FileName = newfilename
                });
                ViewState["AllAttachmentName"] = AllAttachmentlst;
                ViewState["AttachmentName"] = AttachmentLst;

                grdIssueAttachment.DataSource = AllAttachmentlst;
                grdIssueAttachment.DataBind();

            }
            catch(Exception ex)
            {
                Utility.WriteErrorLog(ex.Message, ex.StackTrace);
            }
        }

        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(ViewState["Comment"]) != "")
            {
                CommentLst = (List<CommentDtl>)ViewState["Comment"];
            }

            if (Convert.ToString(ViewState["CommentAdd"]) != "")
            {
                CommentLstAdd = (List<CommentDtl>)ViewState["CommentAdd"];
            }

            CommentDtl cmtdtl = new CommentDtl();
            if (Convert.ToString(txtCommentBox.Text) != "")
            {
                cmtdtl.UserName = Convert.ToString(Session["UserName"]);
                cmtdtl.Comment = Convert.ToString(txtCommentBox.Text);
                cmtdtl.CreateUserId = Convert.ToInt32(Session["UserId"]);
                CommentLst.Add(cmtdtl);
                CommentLstAdd.Add(cmtdtl);
            }
            ViewState["Comment"] = CommentLst;
            ViewState["CommentAdd"] = CommentLstAdd;


            StringBuilder CommentStr = new StringBuilder();
            CommentStr = CommentStr.Append("<head> <table>");
            foreach (var Comlst in CommentLst)
            {
                CommentStr.Append(" <tr><td> " + Comlst.UserName + " </td><td></td></tr>");
                CommentStr.Append("<tr><td></td><td>" + Comlst.Comment + "</td></tr>");
            }
            CommentStr.Append("</table></head>");
            lblCommentDetail.Text = CommentStr.ToString();

            txtCommentBox.Text = string.Empty;


        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            if (Session[Constants.CONST_SESSION_ACTIVITY_ID] != null || Session[Constants.CONST_SESSION_SUBACTIVITY_ID] != null)
            {
                RadAjaxManager1.ResponseScripts.Add("CloseModal();");
                Session[Constants.CONST_SESSION_ACTIVITY_ID] = null;
                Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = null;
                Session["IssueId"] = null;
            }
            else
                Response.Redirect("~/IssueManagementList.aspx");
        }

        protected void grdIssueAttachment_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    GridDataItem item = e.Item as GridDataItem;
                    string RemoveFileName = Convert.ToString(item.GetDataKeyValue("FileName"));

                    if (Convert.ToString(ViewState["AttachmentName"]) != "")
                    {
                        AttachmentLst = (List<Attachement>)ViewState["AttachmentName"];
                    }
                    if (Convert.ToString(ViewState["AllAttachmentName"]) != "")
                    {
                        AllAttachmentlst = (List<Attachement>)ViewState["AllAttachmentName"];
                    }

                    var attachment = AttachmentLst.Where(x => x.FileName == RemoveFileName).FirstOrDefault();
                    var allattachment = AllAttachmentlst.Where(x => x.FileName == RemoveFileName).FirstOrDefault();

                    if (attachment != null)
                        AttachmentLst.Remove(attachment);
                    if (allattachment != null)
                        AllAttachmentlst.Remove(allattachment);
                    ViewState["AllAttachmentName"] = AllAttachmentlst;
                    ViewState["AttachmentName"] = AttachmentLst;
                    grdIssueAttachment.DataSource = AllAttachmentlst;
                    grdIssueAttachment.DataBind();
                }

             }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdIssueAttachment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (Convert.ToInt32(Session["IssueId"]) != 0)
            {
                if (e.Item is GridDataItem)
                {
                    GridColumn Remove = grdIssueAttachment.MasterTableView.GetColumn("Remove");                    
                    Remove.Visible = false;
                 }

                }
          }

        protected void btnDocument_Click(object sender, EventArgs e)
        {

            try
            {
                LinkButton lb = (LinkButton)sender;
                GridDataItem item = (GridDataItem)lb.NamingContainer;
                string FileName = Convert.ToString(item.GetDataKeyValue("FileName"));

                string filePath = "Upload/Attachment/" + FileName;
                Response.ContentType = "doc/docx";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ViewState["AttachmentName"]) != "")
                {
                    AttachmentLst = (List<Attachement>)ViewState["AttachmentName"];
                }
                if (Convert.ToString(ViewState["CommentAdd"]) != "")
                {
                    CommentLst = (List<CommentDtl>)ViewState["CommentAdd"];
                }

                IssueAttachment issueAttachment = new IssueAttachment();
                IssueComment issueComment = new IssueComment();

                List<IssueAttachment> issueAttachmentList = new List<IssueAttachment>();
                List<IssueComment> issueCommentList = new List<IssueComment>();

                foreach (var attch in AttachmentLst)
                {
                    issueAttachment = new IssueAttachment()
                    {
                        FilePath = attch.FileName.ToString()
                    };
                    issueAttachmentList.Add(issueAttachment);
                }

                foreach (var cmmt in CommentLst)
                {
                    issueComment = new IssueComment()
                    {
                        Comment = cmmt.Comment
                    };
                    issueCommentList.Add(issueComment);
                }
                string issueStatus = string.Empty;
               
                issueStatus = "Draft";              

                int activityId = 0;
                int subActivityId = 0;
                if (Session[Constants.CONST_SESSION_ACTIVITY_ID] != null)
                    activityId = Convert.ToInt32(Session[Constants.CONST_SESSION_ACTIVITY_ID]);

                if (Session[Constants.CONST_SESSION_SUBACTIVITY_ID] != null)
                    subActivityId = Convert.ToInt32(Session[Constants.CONST_SESSION_SUBACTIVITY_ID]);


                IssueManagement issueManagement = new IssueManagement()
                {
                    IssueId = Convert.ToInt32(Session["IssueId"]),
                    IssueStatus = issueStatus,
                    IssueDate = Convert.ToDateTime(issueDate.SelectedDate),
                    CategoryId = Convert.ToInt32(drpIssueCategory.SelectedValue),
                    LocationId = string.IsNullOrEmpty(drpLocation.SelectedValue) ? 0 : Convert.ToInt32(drpLocation.SelectedValue),
                    Description = Convert.ToString(txtDecription.Text),
                    ExpectedClosureDate = Convert.ToDateTime(expectedClosureDate.SelectedDate),
                    AssignedTo = Convert.ToInt32(HttpContext.Current.Session["UserId"]), //Convert.ToInt32(drpAssignTo.SelectedValue),
                    Resolution = Convert.ToString(txtResolution.Text),
                    IssueAttachments = issueAttachmentList,
                    IssueComments = issueCommentList,
                    ActivityId = activityId,
                    SubActivityId = subActivityId,
                    LocationType = txtLocationType.Text.Trim()
                };

                string jsonInputParameter = JsonConvert.SerializeObject(issueManagement);
                string serviceResult = string.Empty;


                if (Convert.ToInt32(Session["IssueId"]) == 0)
                    serviceResult = commonFunctions.RestServiceCall(Constants.ISSUE_MGT_ADD_ISSUE, Crypto.Instance.Encrypt(jsonInputParameter));
                else
                    serviceResult = commonFunctions.RestServiceCall(Constants.ISSUE_MGT_UPDATE_ISSUE, Crypto.Instance.Encrypt(jsonInputParameter));


                if (string.Compare(serviceResult, Constants.REST_CALL_FAILURE, true) == 0)
                {
                    radMesaage.Title = "Alert";
                    radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);
                    return;
                }
                else
                {
                    //if (Convert.ToInt32(Session["IssueId"]) == 0 && issueManagement.IssueStatus == "Open")
                    //NotificationHelper.SendIssueAssignementNotification();
                }

                if (activityId != 0 || subActivityId != 0)
                {
                    radMesaage.Title = "Success";
                    radMesaage.Show("Issue saved successfully.");
                    RadAjaxManager1.ResponseScripts.Add("CloseModal();");
                    Session[Constants.CONST_SESSION_ACTIVITY_ID] = null;
                    Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = null;
                    Session["IssueId"] = null;
                }
                else
                    Response.Redirect("~/IssueManagementList.aspx");
            }
            catch (Exception ex)
            {
                radMesaage.Title = "Alert";
                radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }

    [Serializable]
    public class CommentDtl
    {
        public int CreateUserId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
    }
    [Serializable]
    public class Attachement
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
    }

    public class IssueManagementModelDtl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IssueManagementModelDtl()
        {
            this.IssueComments = new HashSet<IssueCommentModelDtl>();
            this.IssueAttachments = new HashSet<IssueAttachment>();

        }

        public int IssueId { get; set; }
        public System.DateTime IssueDate { get; set; }
        public System.DateTime ExpectedClosureDate { get; set; }
        public string Site { get; set; }
        public string Area { get; set; }
        public int CategoryId { get; set; }
        public string Activity { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationType { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        public string Resolution { get; set; }
        public Nullable<System.DateTime> ActualClosingDate { get; set; }
        public Nullable<System.TimeSpan> LeadTime { get; set; }
        public string IssueStatus { get; set; }
        public bool Status { get; set; }
      
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }

        public virtual IssueCategory IssueCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        // public virtual ICollection<IssueComment> IssueComments { get; set; }
        public virtual ICollection<IssueCommentModelDtl> IssueComments { get; set; }

        public virtual LocationMaster LocationMaster { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IssueAttachment> IssueAttachments { get; set; }

        public string name { get; set; }

    }

    public partial class IssueCommentModelDtl
    {
        public int IssueCommentId { get; set; }
        public int IssueId { get; set; }
        public string Comment { get; set; }

        public string LocationType { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }

        public string name { get; set; }
        // public virtual IssueManagementModelDtl IssueManagement { get; set; }
        public virtual IssueManagement IssueManagement { get; set; }
    }


}
