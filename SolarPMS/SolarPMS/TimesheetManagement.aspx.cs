using Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class TimesheetManagement : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();

        #region "Data Member"
        public  int ActivityId { get; set; }
        public  int SubActivityId { get; set; }
        public  string FieldValue { get; set; }
        public  string SAPSite { get; set; }
        public  string SAPProjectId { get; set; }
        public  int WBSAreaId { get; set; }
        public  string SAPNetwork { get; set; }
        public  string SAPActivity { get; set; }
        public  string SAPSubActivity { get; set; }
        public  DateTime EstimatedStartDate { get; set; }
        public  DateTime EstimatedEndDate { get; set; }
        public  string TabName { get; set; }
        public  int TimesheetId { get; set; }
        public  int TimesheetStatusId { get; set; }
        public  int TimesheetWorkflowStatusId { get; set; }
        public  int TimesheetUserId { get; set; }
        public  string UOM { get; set; }
        public  decimal EstimatedQuantity { get; set; }
        public  int UserId { get; set; }
        public  bool IsEdit { get; set; }
        public  bool IsPartialApprove { get; set; }
        public  bool IsPartialApproveByQM { get; set; }
        public  decimal SubmittedActualQuantity { get; set; }
        List<Attachement> AttachmentLst = new List<Attachement>();
        List<Attachement> AllAttachmentlst = new List<Attachement>();
        #endregion

        #region "Events"
        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                if (!Request.Url.ToString().Contains("TimesheetManagement.aspx?"))
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
            try
            {
                if (Session["UserId"] == null && !string.IsNullOrEmpty(Request.QueryString.ToString()))
                {
                    Response.Redirect(ConfigurationManager.AppSettings["WebsiteUrl"] + "/Login.aspx?ReturnURL=" + HttpContext.Current.Request.Url.AbsoluteUri, true);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString.ToString()) && Request.Url.ToString().Contains("TimesheetManagement.aspx?"))
                    {
                        string[] queryString = Request.QueryString.ToString().Split('?');
                        string TimesheetId = Crypto.Instance.Decrypt(HttpUtility.UrlDecode(queryString[0])).Split('=')[1];
                        Session[Constants.CONST_SESSION_TIMESHEET_ID] = TimesheetId;
                    }

                    if (!IsPostBack)
                        Initialize();
                    else
                    {
                        if (Session[Constants.CONST_SESSION_TIMESHEET_PARAMETER] != null)
                        {
                            UserId = Convert.ToInt32(Session["UserId"]);
                            SetTimesheeVariable();
                            SetPartialApproveState();
                            IsEdit = setScreenMode();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = btnSubmit.Enabled = false;
                if (CheckTimesheetDateExist())
                {
                    decimal actualQuantity = string.IsNullOrEmpty(txtActualQty.Text) ? 0 : Convert.ToDecimal(txtActualQty.Text);
                    decimal SubmittedActualQuantity = Convert.ToDecimal(ViewState["SubmittedActualQuantity"]);
                    if ((FieldValue == "b" || FieldValue == "d" || FieldValue == "g") &&
                        SubmittedActualQuantity + actualQuantity > Convert.ToDecimal(txtEstQty.Value))
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("saveConfirmation('save')");
                    else
                        SaveTimesheetDetails(true);
                }
                else
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Timesheet already exists for date.')");
                btnSave.Enabled = btnSubmit.Enabled = true;
            }
            catch (Exception ex)
            {
                if (ex.Source == "SAP.Connector")
                    radNotificationMessage.Text = "Unable to open SAP connection.";
                else
                    radNotificationMessage.Text = "Failed to update timesheet.";
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = btnSubmit.Enabled = false;
                if (CheckTimesheetDateExist())
                {
                    decimal actualQuantity = string.IsNullOrEmpty(txtActualQty.Text) ? 0 : Convert.ToDecimal(txtActualQty.Text);
                    decimal SubmittedActualQuantity  = Convert.ToDecimal(ViewState["SubmittedActualQuantity"]);
                    if (SubmittedActualQuantity + actualQuantity > Convert.ToDecimal(txtEstQty.Value)
                    && (FieldValue == "b" || FieldValue == "d" || FieldValue == "g"))
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("saveConfirmation('submit');");
                    else
                        SaveTimesheetDetails(false);
                }
                else
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Timesheet already exists for date.')");
                btnSave.Enabled = btnSubmit.Enabled = true;
            }
            catch (Exception ex)
            {
                if (ex.Source == "SAP.Connector")
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Unable to open SAP connection.')");
                else
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Failed to update timesheet.')");

                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void radUploadAttachment_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                if (radUploadAttachment.UploadedFiles.Count > 0)
                {
                    String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                    string newfilename = (e.File.GetNameWithoutExtension() + "_" + timeStamp + e.File.GetExtension()).Replace(" ", "_");
                    e.File.SaveAs(Path.Combine(Server.MapPath(radUploadAttachment.TargetFolder), newfilename));

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
                        FileName = newfilename,
                        OriginalFileName = e.File.FileName,
                        AttachmentId = 0
                    });

                    AllAttachmentlst.Add(new Attachement()
                    {
                        FileName = newfilename,
                        OriginalFileName = e.File.FileName,
                        AttachmentId = 0
                    });

                    if(TimesheetId != 0)
                    {
                        TimesheetModel timesheetModel = new TimesheetModel();
                        List<TimesheetAttachment> lstAttachment = new List<TimesheetAttachment>();
                        lstAttachment.Add(new TimesheetAttachment() { FilePath = newfilename});
                        timesheetModel.AddAttachment(lstAttachment, TimesheetId, Convert.ToInt32(Session["UserId"]));
                    }

                    ViewState["AllAttachmentName"] = AllAttachmentlst;
                    ViewState["AttachmentName"] = AttachmentLst;
                    gridTimesheetAttachment.DataSource = AllAttachmentlst;
                    gridTimesheetAttachment.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void ddlBlockNo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                RadDropDownList ddlBlockNo = (RadDropDownList)sender;
                GridDataItem dataItem = (GridDataItem)ddlBlockNo.NamingContainer;
                int tableActivityId = Convert.ToInt32(ddlBlockNo.SelectedValue);
                int blockNumber = Convert.ToInt32(ddlBlockNo.SelectedText.Trim());
                bool blockAlreadyExist = false;
                RadGrid gridObject = null;
                if (IsPartialApprove)
                    gridObject = gridManagerBlockDetails;
                else
                    gridObject = gridBlockDetails;

                if (IsPartialApprove)
                {
                    List<BlockDetails> lstBlock = Session[Constants.CONST_TIMESHEET_USER_BLOCK_DETAILS] as List<BlockDetails>;
                    if (lstBlock.Where(l => l.BlockNo == blockNumber).FirstOrDefault() == null)
                    {
                        gridObject.MasterTableView.IsItemInserted = false;
                        gridObject.MasterTableView.ClearEditItems();
                        gridObject.MasterTableView.Rebind();
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','User has not added this block number.')");
                        return;
                    }
                }

                if (gridBlockDetails.Items.Count > 0)
                {
                    foreach (GridDataItem item in gridObject.Items)
                    {
                        if (!item.Edit)
                        {
                            Label lblBlockno = item.FindControl("lblBlockno") as Label;
                            if (lblBlockno.Text.ToLower() == ddlBlockNo.SelectedText.ToLower())
                            {
                                TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Block already exists. Please select another block.')");
                                blockAlreadyExist = true;
                                break;
                            }
                        }
                    }
                }

                if (!blockAlreadyExist)
                {
                    GridEditableItem editedItem = (sender as RadDropDownList).NamingContainer as GridEditableItem;
                    if (editedItem != null && tableActivityId != 0)
                    {
                        int completedQuanity = GetCompletedQuantity(blockNumber);
                        int praposedQuantity = GetPraposedQuantity(tableActivityId);
                        RadNumericTextBox txtProposedQuantity = (RadNumericTextBox)editedItem.FindControl("txtProposedQuantity");
                        txtProposedQuantity.Text = praposedQuantity.ToString();
                        RadNumericTextBox txtCompletedQuantity = (RadNumericTextBox)editedItem.FindControl("txtCompletedQuantity");
                        txtCompletedQuantity.Text = completedQuanity.ToString();
                        RadNumericTextBox txtRemaingQuantity = (RadNumericTextBox)editedItem.FindControl("txtRemainingQuantity");
                        txtRemaingQuantity.Text = Convert.ToString(praposedQuantity - completedQuanity);                        
                    }
                }
                else
                {
                    gridObject.MasterTableView.IsItemInserted = false;
                    gridObject.MasterTableView.ClearEditItems();
                    gridObject.MasterTableView.Rebind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpVillage_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                RadDropDownList drpVillage = (RadDropDownList)sender;
                GridDataItem dataItem = (GridDataItem)drpVillage.NamingContainer;
                int selectedVillageId = Convert.ToInt32(drpVillage.SelectedValue);

                RadGrid gridObject = null;

                if (IsPartialApprove)
                {
                    gridObject = gridManagerSurvayDetails;
                    List<SurveyMasterModel> lstUserSurveyDetails = Session[Constants.CONST_TIMESHEET_USER_SURVEY_DETAILS] as List<SurveyMasterModel>;
                    if (lstUserSurveyDetails.Where(s => s.VillageId == selectedVillageId).FirstOrDefault() == null)
                    {
                        gridObject.MasterTableView.IsItemInserted = false;
                        gridObject.MasterTableView.ClearEditItems();
                        gridObject.MasterTableView.Rebind();
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','User has not added this village.')");
                        return;
                    }
                }
                else
                    gridObject = gridSurvey;

                GridEditableItem editedItem = (sender as RadDropDownList).NamingContainer as GridEditableItem;
                if (editedItem != null && selectedVillageId != 0)
                {
                    
                    RadDropDownList drpSurvay = (RadDropDownList)editedItem.FindControl("drpSurvayNo");
                    RadNumericTextBox txtPraposedArea = editedItem.FindControl("txtPraposedArea") as RadNumericTextBox;
                    RadNumericTextBox txtPraposedNoOfDivision = editedItem.FindControl("txtPraposedNoOfDivision") as RadNumericTextBox;
                    RadNumericTextBox txtCompletedArea = editedItem.FindControl("txtCompletedArea") as RadNumericTextBox;
                    RadNumericTextBox txtRemainingArea = editedItem.FindControl("txtRemainingArea") as RadNumericTextBox;
                    RadNumericTextBox txtCompletedNoOfDivision = editedItem.FindControl("txtCompletedNoOfDivision") as RadNumericTextBox;
                    RadNumericTextBox txtRemainingNoOfDivision = editedItem.FindControl("txtRemainingNoOfDivision") as RadNumericTextBox;
                    RadNumericTextBox txtNoOfDivision = editedItem.FindControl("txtNoOfDivision") as RadNumericTextBox;
                    RadNumericTextBox txtArea = editedItem.FindControl("txtArea") as RadNumericTextBox;

                    txtNoOfDivision.Text = string.Empty;
                    txtArea.Text = string.Empty;
                    txtPraposedArea.Text = string.Empty;
                    txtPraposedNoOfDivision.Text = string.Empty;
                    txtCompletedArea.Text = string.Empty;
                    txtRemainingArea.Text = string.Empty;
                    txtCompletedNoOfDivision.Text = string.Empty;
                    txtRemainingNoOfDivision.Text = string.Empty;

                    drpSurvay.DataSource = GetSurveyDetails(selectedVillageId);
                    drpSurvay.DataTextField = "SurveyNo";
                    drpSurvay.DataValueField = "SurveyId";
                    drpSurvay.DataBind();
                    drpSurvay.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void txtManualEntry_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtManualEntry.Text = txtManualEntry.Text;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnCalculatePraposedQuantity_Click(object sender, EventArgs e)
        {
            try
            {
                ValidatePraposedQuantity();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridTimesheetAttachment_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    GridDataItem item = e.Item as GridDataItem;
                    string RemoveFileName = Convert.ToString(item.GetDataKeyValue("FileName"));
                    int attachmentId = Convert.ToInt32(item.GetDataKeyValue("AttachmentId"));

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

                    if (TimesheetId != 0)
                    {
                        if (TimesheetModel.DeleteAttachment(attachmentId))
                        {
                            TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Success','Attachment removed successfully.')");
                        }
                    }

                    ViewState["AllAttachmentName"] = AllAttachmentlst;
                    ViewState["AttachmentName"] = AttachmentLst;
                    gridTimesheetAttachment.DataSource = AllAttachmentlst;
                    gridTimesheetAttachment.DataBind();
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridTimesheetAttachment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    GridColumn Remove = gridTimesheetAttachment.MasterTableView.GetColumn("Remove");
                    Remove.Visible = IsEdit;

                    HyperLink attachment = item["FileName"].Controls[0] as HyperLink;
                    attachment.NavigateUrl = ConfigurationManager.AppSettings["MailUrl"] + "Upload/Attachment/Timesheet/" + attachment.Text;
                    attachment.Target = "new";

                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.RegisterPostBackControl(attachment);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lb = (LinkButton)sender;
                GridDataItem item = (GridDataItem)lb.NamingContainer;
                string FileName = Convert.ToString(item.GetDataKeyValue("FileName"));

                string filePath = "Upload/Attachment/Timesheet/" + FileName;
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpSurvayNo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try {
                GridEditableItem item = ((Telerik.Web.UI.GridEditableItem)(((System.Web.UI.Control)sender).NamingContainer));

                RadGrid gridObject;
                if (IsPartialApprove)
                    gridObject = gridManagerSurvayDetails;
                else
                    gridObject = gridSurvey;

                if (item != null)
                {
                    RadNumericTextBox txtPraposedArea = item.FindControl("txtPraposedArea") as RadNumericTextBox;
                    RadNumericTextBox txtPraposedNoOfDivision = item.FindControl("txtPraposedNoOfDivision") as RadNumericTextBox;
                    RadNumericTextBox txtCompletedArea = item.FindControl("txtCompletedArea") as RadNumericTextBox;
                    RadNumericTextBox txtRemainingArea = item.FindControl("txtRemainingArea") as RadNumericTextBox;
                    RadNumericTextBox txtCompletedNoOfDivision = item.FindControl("txtCompletedNoOfDivision") as RadNumericTextBox;
                    RadNumericTextBox txtRemainingNoOfDivision = item.FindControl("txtRemainingNoOfDivision") as RadNumericTextBox;

                    RadDropDownList ddlSurvey = (RadDropDownList)sender;
                    int selectedSurveyId = Convert.ToInt32(ddlSurvey.SelectedValue);
                    SurveyMaster surveyMaster = TimesheetModel.GetSurveyDetailsById(selectedSurveyId);
                    List<SurveyMasterModel> lstUserSurveyDetails = null;
                    if (IsPartialApprove)
                    {
                        lstUserSurveyDetails = Session[Constants.CONST_TIMESHEET_USER_SURVEY_DETAILS] as List<SurveyMasterModel>;
                        var aa = lstUserSurveyDetails.FirstOrDefault(s => s.SurveyNo.Trim() == ddlSurvey.SelectedText.Trim()
                                                                             && s.VillageId == surveyMaster.VillageId);
                        var userbbSurvey = lstUserSurveyDetails.FirstOrDefault(s => s.SurveyNo == ddlSurvey.SelectedText.Trim());
                        var cc = lstUserSurveyDetails.FirstOrDefault(s => s.VillageId == surveyMaster.VillageId);
                        var userSurvey = lstUserSurveyDetails.FirstOrDefault(s => s.SurveyNo.Trim() == ddlSurvey.SelectedText.Trim() 
                                                                              && s.VillageId == surveyMaster.VillageId);
                        if (userSurvey == null)
                        {
                            gridObject.MasterTableView.IsItemInserted = false;
                            gridObject.MasterTableView.ClearEditItems();
                            gridObject.MasterTableView.Rebind();
                            TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','User has not added this survey number.')");
                            return;
                        }
                    }
                    
                    decimal[] completedAreaDetails = GetCompletedSurveyDetails(surveyMaster.VillageId, ddlSurvey.SelectedText.Trim());
                    txtPraposedArea.Text = Convert.ToString(surveyMaster.Area);
                    txtPraposedNoOfDivision.Text = Convert.ToString(surveyMaster.NoOfDivision);
                    txtCompletedArea.Text = completedAreaDetails[0].ToString();
                    txtCompletedNoOfDivision.Text = completedAreaDetails[1].ToString();
                    txtRemainingArea.Text = Convert.ToString(surveyMaster.Area - completedAreaDetails[0]);
                    txtRemainingNoOfDivision.Text = Convert.ToString(surveyMaster.NoOfDivision - completedAreaDetails[1]);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void TimesheetManagementRadAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            try
            {
                string values = e.Argument;
                string[] parameters = values.Split('#');
                if (parameters[0].Contains("ProceedToSaveYes"))
                {
                    if (parameters[1] == "save")
                        SaveTimesheetDetails(true);
                    else if (parameters[1] == "submit")
                        SaveTimesheetDetails(false);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnManagerManualRange_Click(object sender, EventArgs e)
        {
            try
            {
                string manualRange = txtManagerManualEntry.Text.Trim();
                int[] rangeArray = { };
                string invalidNumber = "";
                if (!string.IsNullOrEmpty(manualRange))
                    rangeArray = Array.ConvertAll(manualRange.Split(','), int.Parse);

                for (int cnt = 0; cnt < rangeArray.Count(); cnt++)
                {
                    if (!ValidateNumbersWithUserDetails(rangeArray[cnt], -1))
                        invalidNumber += "," + rangeArray[cnt];
                }
                if (!string.IsNullOrEmpty(invalidNumber))
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','User has not added given number :" + invalidNumber.Trim(',') + "')");
                else
                    ValidatePraposedQuantity();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #region Block Events
        protected void gridBlockDetails_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                ValidateManagerBlockNumber(1, 10);
                SaveBlockDetails(e, Constants.CONST_NEW_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridBlockDetails_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveBlockDetails(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridBlockDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {

                    GridEditableItem editItem = (GridEditableItem)e.Item;
                    BlockDetails blockObject = e.Item.DataItem as BlockDetails;
                    RadDropDownList ddlBlock = (RadDropDownList)editItem.FindControl("ddlBlockNo");
                    ddlBlock.DataTextField = "Number";
                    ddlBlock.DataValueField = "TableActivityId";
                    ddlBlock.DataSource = GetAllBlockDetails();
                    if (blockObject != null)
                        ddlBlock.SelectedText = Convert.ToString(blockObject.BlockNo);
                    ddlBlock.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridBlockDetails_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.CommandName))
                {
                    case "InitInsert":
                        gridBlockDetails.MasterTableView.ClearEditItems();
                        break;
                    case "Edit":
                        e.Item.OwnerTableView.IsItemInserted = false;
                        break;
                    case "Filter":
                        gridBlockDetails.MasterTableView.ClearEditItems();
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnBlock_Click(object sender, EventArgs e)
        {
            try
            {
                gridBlockDetails.MasterTableView.ClearEditItems();
                gridBlockDetails.MasterTableView.IsItemInserted = true;
                gridBlockDetails.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridBlockDetails_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                DeleteBlockDetails(e);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

       
        #endregion

        #region Survey Events
        protected void gridSurvey_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetSurveyDetails();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridSurvey_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveSurveyDetails(e, Constants.CONST_NEW_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridSurvey_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveSurveyDetails(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridSurvey_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    try
                    {
                        string SiteId = string.Empty;
                        string VillageId = string.Empty;
                        string projId = string.Empty;
                        GridDataItem item = e.Item as GridDataItem;

                        GridEditableItem editItem = (GridEditableItem)e.Item;
                        RadDropDownList drpSite = (RadDropDownList)editItem.FindControl("drpSite");
                        RadDropDownList drpVillage = (RadDropDownList)editItem.FindControl("drpVillage");
                        RadDropDownList drpSurvay = (RadDropDownList)editItem.FindControl("drpSurvayNo");


                        Label lblVillage = item["Village"].FindControl("lblVillage") as Label;
                        VillageId = lblVillage.Text;
                        Label lblSite = item["Site"].FindControl("lblSite") as Label;
                        Label lblSurveyNo = (Label)editItem.FindControl("lblSurveyNo");
                        SiteId = Convert.ToString(lblSite.Text.Trim());
                        BindDropDown(drpSite, drpVillage);
                        drpSite.SelectedValue = SiteId;
                        drpVillage.SelectedValue = VillageId;

                        RadDropDownList drpProj = (RadDropDownList)editItem.FindControl("drpProject");
                        Label lblProject = item["Project"].FindControl("lblProject") as Label;
                        projId = lblProject.Text;

                        if (!string.IsNullOrEmpty(SiteId.Trim()))
                        {
                            bindProjDropDown(drpProj, SiteId.Trim());
                            drpProj.FindItemByValue(projId).Selected = true;
                        }
                        if (!string.IsNullOrEmpty(lblVillage.Text))
                        {
                            drpSurvay.DataSource = GetSurveyDetails(Convert.ToInt32(drpVillage.SelectedValue));
                            drpSurvay.DataTextField = "SurveyNo";
                            drpSurvay.DataValueField = "SurveyId";
                            drpSurvay.SelectedText = lblSurveyNo.Text;
                            drpSurvay.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonFunctions.WriteErrorLog(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void BindDropDown(RadDropDownList drpSite, RadDropDownList drpVillage)
        {
            SurveyModel surveyModel = new SurveyModel();
            SurveyMaster surveyMaster = new SurveyMaster()
            {
                ProjectId = SAPProjectId,
                Site = SAPSite
            };

            dynamic lstVillage = surveyModel.GetVillage(surveyMaster).GroupBy(x => x.VillageId).Select(x => x.First()).ToList();
            
            drpVillage.DataTextField = "VillageName";
            drpVillage.DataValueField = "VillageId";
            drpVillage.DataSource = lstVillage;
            drpVillage.DataBind();
        }

        public void bindProjDropDown(RadDropDownList drpProj, string siteId)
        {
            string result1 = commonFunctions.RestServiceCall(string.Format(Constants.TABLE_GET_PROJECTBYSITE, Convert.ToString(siteId)), string.Empty);
            DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result1);
            drpProj.DataTextField = "Name";
            drpProj.DataValueField = "Id";
            drpProj.DataSource = ddValues.project;
            drpProj.DataBind();
        }

        protected void gridSurvey_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.CommandName))
                {
                    case "InitInsert":
                        gridSurvey.MasterTableView.ClearEditItems();
                        break;
                    case "Edit":
                        e.Item.OwnerTableView.IsItemInserted = false;
                        break;
                    case "Filter":
                        gridSurvey.MasterTableView.ClearEditItems();
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpSite_SelectedIndexChanged(object sender, DropDownListEventArgs e)
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
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnNewRow_Click(object sender, EventArgs e)
        {
            try
            {
                gridSurvey.MasterTableView.ClearEditItems();
                gridSurvey.MasterTableView.IsItemInserted = true;
                gridSurvey.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridSurvey_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                DeleteSurveyDetails(e);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        
        #endregion

        #region Table/SCB/Inverter
        protected void btnAddRange_Click(object sender, EventArgs e)
        {
            try
            {
                gridInverter.MasterTableView.ClearEditItems();
                gridInverter.MasterTableView.IsItemInserted = true;
                gridInverter.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridInverter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetInverterDetails();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridInverter_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveInverterDetails(e, Constants.CONST_NEW_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridInverter_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveInverterDetails(e, Constants.CONST_EDIT_MODE);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridInverter_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.CommandName))
                {
                    case "InitInsert":
                        gridInverter.MasterTableView.ClearEditItems();
                        break;
                    case "Edit":
                        e.Item.OwnerTableView.IsItemInserted = false;
                        break;
                    case "Filter":
                        gridInverter.MasterTableView.ClearEditItems();
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridInverter_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                DeleteInvertorDetails(e);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void DeleteInvertorDetails(GridCommandEventArgs e)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            int ID = Convert.ToInt32(editableItem.GetDataKeyValue("TimesheetActivityId").ToString());
            List<TimesheetActivity> inverterDetails = (List<TimesheetActivity>)Session["timeSheetInverterDetails"];
            TimesheetActivity inverterObj = inverterDetails.Find(x => x.TimesheetActivityId == ID);

            TableActivityRange tableActivityRange = GetActivityRangeObject(inverterObj.FromRange.Value, inverterObj.ToRange.Value);
            List<TableActivity> lstTableActivity = GetPraposedQuantityForRange(inverterObj.FromRange.Value, inverterObj.ToRange.Value, tableActivityRange);

            hidRangeQuantity.Value = Convert.ToString(Convert.ToDecimal(hidRangeQuantity.Value) - inverterObj.ProposedQuantity);
            decimal actualQuantity = Convert.ToDecimal(hidRangeQuantity.Value) + Convert.ToDecimal(hidManualEntryQuantity.Value);
            if (IsPartialApprove)
            {
                txtApprovedQuantity.Text = txtManagerQuantity.Text = actualQuantity != 0 ? actualQuantity.ToString() : string.Empty;
            }
            else
                txtActualQty.Text = txtQty.Text = actualQuantity != 0 ? actualQuantity.ToString() : string.Empty;

            inverterDetails.Remove(inverterObj);
            if (TimesheetId != 0 && !IsPartialApprove)
            {
                if (TimesheetModel.DeleteTimesheetTableActivity(ID))
                {
                    TimesheetModel.UpdateTimesheetActualQuantity(TimesheetId, Convert.ToDecimal(txtActualQty.Text), UserId);
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Success','Timesheet activity deleted')");
                }
            }

            Session["timeSheetInverterDetails"] = inverterDetails;
        }
        #endregion

        #endregion

        #region "Private Methods"
        private void Initialize()
        {
            UserId = Convert.ToInt32(Session["UserId"]);
            string tabToShow = SetControlValue();
            IsEdit = setScreenMode();
            SetTabVisibility(tabToShow);
            SetControlsVisibilityAccordingToFlag();
            BindTimesheetStatus();
            SetTimesheetDetailsInEditMode();

            if (!IsEdit)
                DisableSave();
        }

        /// <summary>
        /// Set flag to open screen in edit and view mode.
        /// </summary>
        /// <returns></returns>
        private bool setScreenMode()
        {
            string profileName = Convert.ToString(Session["ProfileName"]);
            bool isEdit = false;
            if (profileName == Constants.CONST_DESIGN_ENGINEER ||
                                                         profileName == Constants.CONST_SITE_FUNCTIONAL_USER ||
                                                         profileName == Constants.CONST_SITE_ADMIN ||
                                                         profileName == Constants.CONST_SITE_COORDINATOR)
            {
                isEdit = true;
            }

            if (TimesheetStatusId == 2 && isEdit && TimesheetUserId == UserId)
            {
                isEdit = true;
            }           
            else if (TimesheetId == 0)
            {
                IsEdit = true;
            }
            else
            {
                isEdit = false;
            }

            return isEdit;
        }

        private bool CheckTimesheetDateExist()
        {
            Timesheet timesheet = new Timesheet()
            {
                SAPActivity = SAPActivity,
                SAPSubActivity = SAPSubActivity,
                SAPNetwork = SAPNetwork,
                WBSAreaId = WBSAreaId,
                SAPProject = SAPProjectId,
                SAPSite = SAPSite
            };

            if (TimesheetId == 0)
                return TimesheetModel.CheckTimesheetExistsForDate(timesheet, RadDatePickerActualDate.SelectedDate.Value, UserId);
            else
                return true;
        }

        private void DisableSave()
        {
            txtComment.ReadOnly = true;
            txtActualQty.Enabled = false;
            txtEstQty.Disabled = true;
            RadDatePickerActualDate.Enabled = false;
            btnSave.Visible = false;
            //gridTimesheetAttachment.Visible = false;            
            //divAttachment.Visible = false;
            radUploadAttachment.Visible = false;
            btnSubmit.Visible = false;
            ddlStatus.Enabled = false;
            gridTimesheetAttachment.MasterTableView.GetColumn("Remove").Visible = false;
            divButtons.Visible = false;
        }

        private void GetSubmittedActualQuantityForActivity()
        {
            ViewState["SubmittedActualQuantity"] = SubmittedActualQuantity = TimesheetModel.GetSubmittedActualQuantityForActivity(SAPSubActivity, SAPActivity, SAPNetwork, WBSAreaId, SAPProjectId, SAPSite);
        }

        private void BindTimesheetStatus()
        {
            ddlStatus.DataSource = TimesheetModel.GetTimesheetStatus();
            ddlStatus.DataBind();
        }

        private void SetTabVisibility(string tabToShow)
        {
           
            if (FieldValue == "b" || FieldValue == "d" || FieldValue == "g")
            {
                if (!string.IsNullOrEmpty(tabToShow))
                {
                    txtActualQty.Enabled = false;
                    switch (tabToShow.ToLower())
                    {
                        case "survey":
                            SurveyTab.Visible = true;
                            divMangerSurveySection.Visible = true;
                            Session["timeSheetSurvery"] = new List<SurveyMasterModel>();
                            GetSurveyDetails();
                            gridSurvey.DataBind();
                            if (!IsEdit)
                            {
                                gridSurvey.MasterTableView.GetColumn("EditSurvey").Visible = false;
                                gridSurvey.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                                btnNewRow.Visible = false;
                            }
                            break;
                        case "block":
                            BlockTab.Visible = true;
                            
                            Session["timeSheetBlockDetails"] = new List<BlockDetails>();
                            GetBlockDetails();
                            gridBlockDetails.DataBind();

                            if (IsPartialApprove)
                            {
                                gridManagerBlockDetails.DataBind();
                                divMangerBlockSection.Visible = true;
                            }

                            if (!IsEdit)
                            {
                                gridBlockDetails.MasterTableView.GetColumn("EditBlock").Visible = false;
                                gridBlockDetails.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                                btnBlock.Visible = false;
                            }
                            break;
                        case "inverter":
                        case "scb":
                        case "table":
                            GlobalTab.Visible = true;
                            lnkTableSCBInvertor.InnerHtml = lblGlobalTab.InnerHtml = lblGlobalTab.InnerHtml.Insert(0, TabName + " Details ");                            
                            Session["timeSheetInverterDetails"] = new List<TimesheetActivity>();
                            GetInverterDetails();
                            gridInverter.DataBind();

                            if (IsPartialApprove)
                            {
                                gridManagerInvertor.DataBind();
                                divManagerActivitySection.Visible = true;
                            }

                            if (!IsEdit)
                            {
                                gridInverter.MasterTableView.GetColumn("EditTable").Visible = false;
                                gridInverter.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                                btnAddRange.Visible = false;
                                txtManualEntry.Enabled = false;
                                btnCalculatePraposedQuantity.Visible = false;
                            }
                            break;
                        case "number":
                        case "location":
                            if (IsPartialApprove)
                            {
                                divMangerNumberSection.Visible = true;
                                //divManagerNumbers.Visible = true;
                            }
                            divNumberControls.Visible = true;
                            break;
                    }
                }
                else
                {
                    txtActualQty.Enabled = true;
                    if (IsPartialApprove)
                    {
                        //divMangerNumberSection.Visible = true;
                        txtApprovedQuantity.Enabled = true;
                        //txtManagerActualQuantity.Enabled = true;
                    }
                }
            }
        }

        private void SetControlsVisibilityAccordingToFlag()
        {
            if (FieldValue == "b" || FieldValue == "d" || FieldValue == "g")
            {
                divEstQty.Visible = true;
                divActualQty.Visible = true;
                divEstUOM.Visible = true;
            }
            else if (FieldValue == "a" || FieldValue == "c" || FieldValue == "e" || FieldValue == "f")
            {
                divEstQty.Visible = false;
                divActualQty.Visible = false;
                divEstUOM.Visible = false;
                txtActualQty.Enabled = false;
            }
        }

        private string SetControlValue()
        {
            string tabToShow = string.Empty;

            if (Session[Constants.CONST_SESSION_TIMESHEET_ID] != null)
            {
                int timesheetId = Convert.ToInt32(Session[Constants.CONST_SESSION_TIMESHEET_ID]);
                int activityId = Convert.ToInt32(Session[Constants.CONST_SESSION_ACTIVITY_ID]);
                int subactivityId = Convert.ToInt32(Session[Constants.CONST_SESSION_SUBACTIVITY_ID]);

                TimesheetActivityDetails timesheetActivity = TimesheetModel.GetTimesheetActivityDetails(timesheetId);
                var serializedObject = JsonConvert.SerializeObject(timesheetActivity);
                Session[Constants.CONST_SESSION_TIMESHEET_PARAMETER] = serializedObject;
            }

            if (Session[Constants.CONST_SESSION_TIMESHEET_PARAMETER] != null)
            {
                JObject parameters = SetTimesheeVariable();

                string activityName = parameters["ActivityDescription"].Value<string>().Trim();
                string subActivityName = parameters["SAPSubActivityDescription"].Value<string>().Trim();
                tabToShow = TabName.ToLower();
                if (TimesheetId != 0)
                    RadDatePickerActualDate.SelectedDate = Convert.ToDateTime(parameters["ActualDate"]);
                else
                    RadDatePickerActualDate.SelectedDate = DateTime.Now;

                lblActivityName.InnerText = "Activity Name: " + activityName;
                txtEstQty.Value = EstimatedQuantity.ToString();
                txtUOM.Value = UOM.ToString();
                SetPartialApproveState();

                if (TimesheetId != 0)
                {
                    if (IsPartialApprove && IsPartialApproveByQM)
                    {
                        decimal? approvedQuantity = TimesheetModel.GetTimesheetApprovedQuantity(TimesheetId);
                        txtActualQty.Text = approvedQuantity != null ? Convert.ToString(approvedQuantity) : "0";
                    }
                    else
                        txtActualQty.Text = Convert.ToString(parameters["ActualQty"].Value<decimal>());

                }
                else
                {
                    txtApprovedQuantity.Text = txtActualQty.Text = string.Empty;
                }

                if (!string.IsNullOrEmpty(subActivityName))
                    lblSubActivityName.InnerText = "Sub - Activity Name: " + subActivityName;
                RadDatePickerEstStart.SelectedDate = EstimatedStartDate;
                RadDatePickerEstEnd.SelectedDate = EstimatedEndDate;

                GetSubmittedActualQuantityForActivity();
            }
            return tabToShow;
        }

        private void SetPartialApproveState()
        {
            if (Session[Constants.CONST_SESSION_IS_PARTIAL_APPROVE] != null && Session[Constants.CONST_SESSION_IS_PARTIAL_APPROVE].ToString() == "True")
            {
                IsPartialApprove = true;
                divMangerSection.Visible = true;
                IsPartialApproveByQM = TimesheetModel.IsPartailApproveByQM(TimesheetId);
            }
            else
            {
                IsPartialApprove = false;
                IsPartialApproveByQM = false;
            }
        }

        private JObject SetTimesheeVariable()
        {
            string jsonParameter = Session[Constants.CONST_SESSION_TIMESHEET_PARAMETER].ToString();
            var parameters = (JObject)JsonConvert.DeserializeObject(jsonParameter);

            TabName = parameters["BlockTable"].Value<string>().Trim();
            ActivityId = parameters["ActivityId"].Value<int>();
            SubActivityId = parameters["SubActivityId"].Value<int>();
            TimesheetId = parameters["TimeSheetId"].Value<int>();
            SAPSite = parameters["SAPSite"].Value<string>();
            SAPProjectId = parameters["SAPProjectId"].Value<string>();
            SAPNetwork = parameters["SAPNetwork"].Value<string>();
            WBSAreaId = parameters["WBSAreaId"].Value<int>();
            SAPNetwork = parameters["SAPNetwork"].Value<string>();
            SAPActivity = parameters["SAPActivity"].Value<string>();
            SAPSubActivity = parameters["SapSubActivity"].Value<string>();
            EstimatedStartDate = Convert.ToDateTime(parameters["ActivityPlanStartDate"]);
            EstimatedEndDate = Convert.ToDateTime(parameters["ActivityPlanFinishDate"]);
            FieldValue = parameters["MobileFunction"].Value<string>().ToLower().Trim();
            UOM = parameters["ActivityPlanQtyUoM"].Value<string>().ToLower().Trim();
            EstimatedQuantity = parameters["ActivityQty"].Value<decimal>();
            TimesheetWorkflowStatusId = Convert.ToInt32(parameters["WorkflowStatusId"]);
            TimesheetUserId = Convert.ToInt32(parameters["CreatedBy"]);
            TimesheetStatusId = parameters["StatusId"].Value<int>();
            RadDatePickerActualDate.MaxDate = DateTime.Now;
            return parameters;
        }

        private void SetTimesheetDetailsInEditMode()
        {
            if (TimesheetId != 0)
            {
                TimesheetModel timesheetModel = new TimesheetModel();
                Timesheet timesheet = new Timesheet();
                timesheet.TimeSheetId = Convert.ToInt32(Session[Constants.CONST_SESSION_TIMESHEET_ID]);
                var timesheetDetails = timesheetModel.GetTimesheetByTimesheetId(timesheet);
                txtComment.Text = timesheetDetails.Comments;

                ddlStatus.SelectedValue = timesheetDetails.StatusId == Convert.ToInt32(Constants.TimesheetStatus.Draft)
                                            ? Convert.ToInt32(Constants.TimesheetStatus.Ongoing).ToString()
                                            : Convert.ToString(timesheetDetails.StatusId);

                RadDatePickerActualDate.Enabled = false;
                if (IsPartialApprove && IsPartialApproveByQM)
                {
                    switch (TabName.ToLower())
                    {
                        case "table":
                        case "scb":
                        case "inverter":
                            List<ManagerTimesheetActivity> lstActivity = TimesheetModel.GetManagerActivityDetails(TimesheetId, TabName);
                            List<ManagerTimesheetActivity> lstActivityDetails = lstActivity.Where(l => l.RangeType == Constants.CONST_TIMESHEET_ACTIVITY_TYPE_RANGE).ToList();
                            List<ManagerTimesheetActivity> lstManualEntry = lstActivity.Where(l => l.RangeType == Constants.CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL).ToList();
                            Session[Constants.CONST_TIMESHEET_INVEROER_RANGE_DETAILS_MANAGER] = lstActivityDetails;
                            Session[Constants.CONST_TIMESHEET_INVEROER_MANUAL_DETAILS_MANAGER] = lstManualEntry;
                            gridInverter.DataSource = lstActivityDetails;
                            gridInverter.DataBind();

                            txtActualQty.Text = txtQty.Text = Convert.ToString(lstActivity.Sum(l => l.ProposedQuantity));
                            txtManualEntry.Text = string.Join(",", lstManualEntry.Select(l => l.FromRange).ToArray());
                            break;
                        case "block":
                            List<ManagerTimesheetBlockDetail> lstBlocks = TimesheetModel.GetManagerBlockDetails(TimesheetId, TabName);
                            List<TableActivity> lstAllBlocks = GetAllBlockDetails();
                            List<BlockDetails> lstBlockDetails = new List<BlockDetails>();
                            int proposedQuantity = 0;
                            int completedQuantity = 0;

                            foreach (var block in lstBlocks)
                            {
                                proposedQuantity = lstAllBlocks.Where(l => l.Number == block.BlockNo).Select(l => l.Quantity).FirstOrDefault();
                                completedQuantity = GetCompletedQuantity(block.BlockNo.Value);
                                lstBlockDetails.Add(new BlockDetails()
                                {
                                    ActualQuantity = block.ActualQuantity,
                                    BlockNo = block.BlockNo,
                                    ProposedQuantity = proposedQuantity,
                                    CompletedQuantity = completedQuantity + block.ActualQuantity.Value,
                                    TimesheetBlockDetailId = block.TimesheetBlockDetailId,
                                    BlockId = block.TimesheetBlockDetailId,
                                    RemainingQuantity = (proposedQuantity - completedQuantity) - block.ActualQuantity.Value
                                });
                            }

                            Session[Constants.CONST_TIMESHEET_USER_BLOCK_DETAILS] = lstBlockDetails;
                            gridBlockDetails.DataSource = lstBlockDetails;
                            gridBlockDetails.DataBind();
                            break;
                        case "survey":
                            List<SurveyMasterModel> lstSurveyDetails = new List<SurveyMasterModel>();
                            dynamic lstSurveys = TimesheetModel.GetManagerSurveyDetails(TimesheetId, TabName);
                            foreach (var survey in lstSurveys)
                            {
                                int remainingDivision = 0;
                                decimal remainingArea = 0;

                                decimal[] completedAreaDetails = GetCompletedSurveyDetails(survey.VillageId, survey.SurveyNo);
                                List<SurveyMaster> lstsurveyMaster = GetSurveyDetails(survey.VillageId);
                                SurveyMaster praposedDetails = lstsurveyMaster.Where(s => s.SurveyNo == survey.SurveyNo).FirstOrDefault();

                                remainingArea = praposedDetails != null
                                                       ? (praposedDetails.Area - completedAreaDetails[0]) - survey.ActualArea
                                                       : completedAreaDetails[0] - survey.ActualArea;

                                remainingDivision = praposedDetails != null
                                                         ? (praposedDetails.NoOfDivision - Convert.ToInt32(completedAreaDetails[1])) - survey.ActualNoOfDivision
                                                         : Convert.ToInt32(completedAreaDetails[1]) - survey.ActualNoOfDivision;

                                lstSurveyDetails.Add(new SurveyMasterModel()
                                {
                                    Area = survey.ActualArea,
                                    NoOfDivision = survey.ActualNoOfDivision,
                                    SurveyNo = survey.SurveyNo,
                                    VillageName = survey.VillageName,
                                    VillageId = survey.VillageId,
                                    SurveyId = survey.TimesheetSurveyDetailId,
                                    PraposedNoOfDivision = praposedDetails != null ? praposedDetails.NoOfDivision : 0,
                                    PraposedArea = praposedDetails != null ? praposedDetails.Area : 0,
                                    CompletedArea = completedAreaDetails[0] + survey.ActualArea,
                                    CompletedNoOfDivision = Convert.ToInt32(completedAreaDetails[1]) + survey.ActualNoOfDivision,
                                    RemainingArea = remainingArea,
                                    RemainingNoOfDivision = remainingDivision
                                });
                            }

                            Session[Constants.CONST_TIMESHEET_USER_SURVEY_DETAILS] = lstSurveyDetails;
                            gridSurvey.DataSource = lstSurveyDetails;
                            gridSurvey.DataBind();
                            break;
                        case "number": case "location":
                            List<ManagerTimesheetActivity> lstTimesheetActivityNumbers = TimesheetModel.GetManagerActivityDetails(TimesheetId, TabName);
                            txtNumber.Text = string.Join(",", lstTimesheetActivityNumbers.Select(l => l.FromRange));
                            txtActualQty.Text = Convert.ToString(lstTimesheetActivityNumbers.Count);
                            break;
                    }                   
                }
                else
                {
                    if (timesheetDetails.TimesheetSurveyDetails != null && timesheetDetails.TimesheetSurveyDetails.Count > 0)
                    {
                        List<SurveyMasterModel> lstSurveyDetails = new List<SurveyMasterModel>();
                        foreach (var survey in timesheetDetails.TimesheetSurveyDetails)
                        {
                            int remainingDivision = 0;
                            decimal remainingArea = 0;

                            decimal[] completedAreaDetails = GetCompletedSurveyDetails(survey.VillageId, survey.SurveyNo);
                            List<SurveyMaster> lstsurveyMaster = GetSurveyDetails(survey.VillageId);
                            SurveyMaster praposedDetails = lstsurveyMaster.Where(s => s.SurveyNo == survey.SurveyNo).FirstOrDefault();

                            remainingArea = praposedDetails != null
                                                   ? (praposedDetails.Area - completedAreaDetails[0]) - survey.ActualArea
                                                   : completedAreaDetails[0] - survey.ActualArea;

                            remainingDivision = praposedDetails != null
                                                     ? (praposedDetails.NoOfDivision - Convert.ToInt32(completedAreaDetails[1])) - survey.ActualNoOfDivision
                                                     : Convert.ToInt32(completedAreaDetails[1]) - survey.ActualNoOfDivision;

                            lstSurveyDetails.Add(new SurveyMasterModel()
                            {
                                Area = survey.ActualArea,
                                NoOfDivision = survey.ActualNoOfDivision,
                                SurveyNo = survey.SurveyNo,
                                VillageName = survey.VillageName,
                                VillageId = survey.VillageId,
                                SurveyId = survey.TimesheetSurveyDetailId,
                                PraposedNoOfDivision = praposedDetails != null ? praposedDetails.NoOfDivision : 0,
                                PraposedArea = praposedDetails != null ? praposedDetails.Area : 0,
                                CompletedArea = completedAreaDetails[0] + survey.ActualArea,
                                CompletedNoOfDivision = Convert.ToInt32(completedAreaDetails[1]) + survey.ActualNoOfDivision,
                                RemainingArea = remainingArea,
                                RemainingNoOfDivision = remainingDivision
                            });
                        }

                        if (IsPartialApprove)
                            Session[Constants.CONST_TIMESHEET_USER_SURVEY_DETAILS] = lstSurveyDetails;
                        else
                            Session["timeSheetSurvery"] = lstSurveyDetails;

                        gridSurvey.DataSource = lstSurveyDetails;
                        gridSurvey.DataBind();
                    }

                    if (timesheetDetails.TimesheetBlockDetails != null && timesheetDetails.TimesheetBlockDetails.Count > 0)
                    {
                        List<BlockDetails> lstBlockDetails = new List<BlockDetails>();
                        List<TableActivity> lstActivity = GetAllBlockDetails();
                        int proposedQuantity = 0;
                        int completedQuantity = 0;
                        foreach (var block in timesheetDetails.TimesheetBlockDetails)
                        {
                            proposedQuantity = lstActivity.Where(l => l.Number == block.BlockNo).Select(l => l.Quantity).FirstOrDefault();
                            completedQuantity = GetCompletedQuantity(block.BlockNo);
                            lstBlockDetails.Add(new BlockDetails()
                            {
                                ActualQuantity = block.ActualQuantity,
                                BlockNo = block.BlockNo,
                                ProposedQuantity = proposedQuantity,
                                CompletedQuantity = completedQuantity + block.ActualQuantity,
                                TimesheetBlockDetailId = block.TimesheetBlockDetailId,
                                BlockId = block.TimesheetBlockDetailId,
                                RemainingQuantity = (proposedQuantity - completedQuantity) - block.ActualQuantity
                            });
                        }

                        if (IsPartialApprove)
                            Session[Constants.CONST_TIMESHEET_USER_BLOCK_DETAILS] = lstBlockDetails; 
                        else
                            Session["timeSheetBlockDetails"] = lstBlockDetails;

                        gridBlockDetails.DataSource = lstBlockDetails;
                        gridBlockDetails.DataBind();
                    }

                    if (timesheetDetails.TimesheetActivities != null && timesheetDetails.TimesheetActivities.Count > 0)
                    {
                        List<TimesheetActivity> lstActivityDetails = new List<TimesheetActivity>();
                        List<TimesheetActivity> lstTimesheetActivity = (List<TimesheetActivity>)timesheetDetails.TimesheetActivities;

                        if (TabName.ToLower() == "number")
                        {
                            txtNumber.Text = string.Join(",", lstTimesheetActivity.Select(l => l.FromRange));
                            txtActualQty.Text = Convert.ToString(lstTimesheetActivity.Count);
                        }
                        else
                        {
                            foreach (var tableActivity in lstTimesheetActivity.Where(l => l.RangeType == Constants.CONST_TIMESHEET_ACTIVITY_TYPE_RANGE).ToList())
                            {
                                lstActivityDetails.Add(new TimesheetActivity()
                                {
                                    FromRange = tableActivity.FromRange.Value,
                                    ToRange = tableActivity.ToRange.Value,
                                    ProposedQuantity = tableActivity.ProposedQuantity.Value,
                                    TimesheetActivityId = tableActivity.TimesheetActivityId,
                                    RangeType = "Range"
                                });
                            }

                            List<TimesheetActivity> lstManualEntry = lstTimesheetActivity.Where(l => l.RangeType == Constants.CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL).ToList();
                            txtActualQty.Text = txtQty.Text = Convert.ToString(lstTimesheetActivity.Sum(l => l.ProposedQuantity));
                            txtManualEntry.Text = string.Join(",", lstManualEntry.Select(l => l.FromRange).ToArray());
                            if (IsPartialApprove)
                            {
                                Session[Constants.CONST_TIMESHEET_INVEROER_RANGE_DETAILS_MANAGER] = lstActivityDetails;
                                Session[Constants.CONST_TIMESHEET_INVEROER_MANUAL_DETAILS_MANAGER] = lstManualEntry;
                            }
                            else
                            {
                                Session["timeSheetInverterDetails"] = lstActivityDetails;
                                hidManualEntryQuantity.Value = Convert.ToString(lstManualEntry.Sum(l => l.ProposedQuantity));
                                hidRangeQuantity.Value = Convert.ToString(lstActivityDetails.Sum(l => l.ProposedQuantity));
                            }
                            gridInverter.DataSource = lstActivityDetails;
                            gridInverter.DataBind();
                        }
                    }

                    if (timesheetDetails.TimesheetAttachments != null && timesheetDetails.TimesheetAttachments.Count > 0)
                    {
                        if (timesheetDetails.TimesheetAttachments != null && timesheetDetails.TimesheetAttachments.Count > 0)
                        {
                            List<Attachement> lstAttachment = new List<Attachement>();
                            foreach (var attachment in timesheetDetails.TimesheetAttachments)
                            {
                                lstAttachment.Add(new Attachement()
                                {
                                    FileName = attachment.FilePath,
                                    OriginalFileName = attachment.FilePath,
                                    AttachmentId = attachment.TimesheetAttachmentId
                                });
                            }
                            //if (Session[Constants.CONST_SESSION_IS_PARTIAL_APPROVE] != null
                            //        && Session[Constants.CONST_SESSION_IS_PARTIAL_APPROVE].ToString() != "True")
                                ViewState["AllAttachmentName"] = ViewState["AttachmentName"] = AllAttachmentlst = AttachmentLst = lstAttachment;

                            gridTimesheetAttachment.DataSource = AllAttachmentlst;
                            gridTimesheetAttachment.DataBind();
                        }
                    }
                }
            }
        }

        private void ValidatePraposedQuantity()
        {
            string manualRange = IsPartialApprove ? txtManagerManualEntry.Text : txtManualEntry.Text.Trim();
            string[] rangeArray = { };
            if (!string.IsNullOrEmpty(manualRange))
                rangeArray = manualRange.Split(',');

            if (ValidateManualRange(rangeArray))
            {
                string[] inValidNumberArray = CalulatePraposedQuantity(rangeArray);
                if (inValidNumberArray.Count() < 1)
                {                    
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Success','Proposed quantity calculated.')");
                }
                else
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Activity range (" + string.Join(",", inValidNumberArray) + ") does not exists.')");
                if (IsPartialApprove)
                    txtManagerManualEntry.Text = string.Join(",", rangeArray.Except(inValidNumberArray));
                else
                    txtManualEntry.Text = string.Join(",", rangeArray.Except(inValidNumberArray));
            }
        }

        private string[] CalulatePraposedQuantity(string[] rangeArray)
        {
            ManualEntryRange manualEntryRange = new ManualEntryRange()
            {
                ActivityId = SAPActivity,
                Flag = TabName,
                NetworkId = SAPNetwork,
                ProjectId = SAPProjectId,
                RangeArray = Array.ConvertAll(rangeArray, int.Parse),
                Site = SAPSite,
                WBSAreaId = WBSAreaId,
                SubActivityId = SAPSubActivity
            };

            TableActivityModel tableActivity = new TableActivityModel();
            List<TableActivity> lstActivity = tableActivity.GetTableActivityByManualEntry(manualEntryRange);

            if (lstActivity != null && lstActivity.Count > 0)
            {
                decimal praposedQuantity = lstActivity.Sum(t => t.Quantity);
                hidManualEntryQuantity.Value = Convert.ToString(praposedQuantity);                
                Session["manualEntryList"] = lstActivity;
                rangeArray = rangeArray.Except(lstActivity.Select(l => l.Number.ToString()).ToArray()).ToArray();
            }
            else
                hidManualEntryQuantity.Value = "0";

            decimal actualQuantity = Convert.ToDecimal(hidRangeQuantity.Value) + Convert.ToDecimal(hidManualEntryQuantity.Value);

            if (IsPartialApprove)
                txtApprovedQuantity.Text = txtManagerQuantity.Text = Convert.ToString(actualQuantity);//actualQuantity != 0 ? Convert.ToString(actualQuantity) : string.Empty;
            else
                txtActualQty.Text = txtQty.Text = Convert.ToString(actualQuantity);//actualQuantity != 0 ? Convert.ToString(actualQuantity) : string.Empty;
            return rangeArray;
        }

        private bool ValidateManualRange(string[] rangeArray)
        {
            List<string> duplicateRange = new List<string>();
            foreach (var r in rangeArray)
            {
                if (ValidateDuplicateRange(Convert.ToInt32(r), -1))
                    duplicateRange.Add(r);
            }

            if (duplicateRange != null && duplicateRange.Count > 0)
            {
                if (IsPartialApprove)
                    txtManagerManualEntry.Text = string.Join(",", rangeArray.Except(duplicateRange));
                else
                    txtManualEntry.Text = string.Join(",", rangeArray.Except(duplicateRange));

                TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Activity range (" + string.Join(",", duplicateRange) + ") already exists.')");
                return false;
            }

            List<int> existingNumbers = TableActivityModel.CheckExistingNumbers(SAPSubActivity, SAPActivity, SAPNetwork, WBSAreaId, SAPProjectId, SAPSite, TimesheetId, Array.ConvertAll(rangeArray, int.Parse), TabName);

            if (existingNumbers.Count() > 0)
            {
                if (IsPartialApprove)
                    txtManagerManualEntry.Text = string.Join(",", rangeArray.Except(existingNumbers.ConvertAll(x => x.ToString())));
                else
                    txtManualEntry.Text = string.Join(",", rangeArray.Except(existingNumbers.ConvertAll(x => x.ToString())));
                TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Numbers (" + string.Join(",", existingNumbers) + ") already added in other timesheet.')");
                return false;
            }

            return true;
        }

        private void SaveTimesheetDetails(bool IsDraft)
        {
            //bool sendNotification = false;
            // user wants to close timesheet
            if (!string.IsNullOrEmpty(txtActualQty.Text))
            {
                decimal actualQuantity = Convert.ToDecimal(txtActualQty.Text);
                if (actualQuantity <= 0)
                {
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Actual Quantity should be greater than 0.')");
                    return;
                }
            }
            if (TimesheetStatusId == Convert.ToInt32(Constants.TimesheetStatus.Ongoing)
            && Convert.ToInt32(ddlStatus.SelectedValue) == Convert.ToInt32(Constants.TimesheetStatus.Closed))
            {
                if (TimesheetModel.CloseTimesheet(TimesheetId))
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Timesheet closed successfully.')");
                else

                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Failed to update timesheet.')");
            }
            // user has opened submitted timesheet and clicked on submit button
            else if (TimesheetId != 0 && TimesheetStatusId == Convert.ToInt32(Constants.TimesheetStatus.Ongoing) && !IsEdit
                && Convert.ToInt32(ddlStatus.SelectedValue) == Convert.ToInt32(Constants.TimesheetStatus.Ongoing))
            {
                TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Cannot modify submitted timesheet.')");
            }
            else
            {
                if ((FieldValue == "c" || FieldValue == "f" || FieldValue == "g") && gridTimesheetAttachment.Items.Count < 1)
                {
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Please select at least a single file.')");
                    return;
                }

                if (TabName.ToLower() == Constants.CONST_TABLE_ACTIVITY_TABLE.ToLower()
                    || TabName.ToLower() == Constants.CONST_TABLE_ACTIVITY_INVERTER.ToLower()
                    || TabName.ToLower() == Constants.CONST_TABLE_ACTIVITY_SCB.ToLower())
                {

                    string manualRange = txtManualEntry.Text.Trim();
                    if (!string.IsNullOrEmpty(manualRange))
                    {
                        string[] rangeArray = { };
                        if (!string.IsNullOrEmpty(manualRange))
                            rangeArray = manualRange.Split(',');

                        if (ValidateManualRange(rangeArray))
                        {
                            string[] inValidNumberArray = CalulatePraposedQuantity(rangeArray);

                            if (inValidNumberArray.Count() > 0)
                            {
                                txtManualEntry.Text = string.Join(",", inValidNumberArray);
                                TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Activity range (" + txtManualEntry.Text + ") does not exists.')");
                                return;
                            }
                        }
                        else
                            return;
                    }
                }
                else if (TabName.ToLower() == Constants.CONST_TABLE_ACTIVITY_NUMBER.ToLower()
                    || TabName.ToLower() == Constants.CONST_TABLE_ACTIVITY_LOCATION.ToLower())
                {
                    if (!string.IsNullOrEmpty(txtNumber.Text.Trim()))
                    {
                        int[] numberArray = Array.ConvertAll(txtNumber.Text.Trim().Split(',').ToArray(), int.Parse);

                        decimal estimatedQuantity = Convert.ToDecimal(txtEstQty.Value);
                        string invalidNumber = string.Empty;
                        foreach (int number in numberArray)
                        {
                            if (number > estimatedQuantity)
                                invalidNumber += "," + number;
                        }

                        if (!string.IsNullOrEmpty(invalidNumber))
                        {
                            TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Number(s) should not be greater than estimated quantity :" + invalidNumber.Trim(',') + "')");
                            return;
                        }

                        List<int> existingNumbers = TableActivityModel.CheckExistingNumbers(SAPSubActivity, SAPActivity, SAPNetwork, WBSAreaId, SAPProjectId, SAPSite, TimesheetId, numberArray, TabName);
                        if (existingNumbers.Count() > 0)
                        {
                            int[] validNumbers = numberArray.Except(existingNumbers).ToArray();
                            txtNumber.Text = string.Join(",", validNumbers);
                            txtActualQty.Text = validNumbers.Count() > 0 ? validNumbers.Length.ToString() : string.Empty;
                            TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Numbers (" + string.Join(",", existingNumbers) + ") already present in other timeshet.')");
                            return;
                        }
                    }
                }

                TimesheetModel timesheetModel = new TimesheetModel();
                Timesheet timesheet = PopulateTimesheetObject(IsDraft);

                if (TimesheetId == 0)
                    timesheetModel.AddTimesheet(timesheet, Convert.ToInt32(Session["UserId"]));
                else
                    timesheetModel.UpdateTimesheet(timesheet, Convert.ToInt32(Session["UserId"]), false);

                TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Success','Timesheet saved successfully.')");
                Session[Constants.CONST_SESSION_TIMESHEET_ID] = null;
                Session[Constants.CONST_SESSION_ACTIVITY_ID] = null;
                Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = null;
                TimesheetManagementRadAjaxManager.ResponseScripts.Add("CloseModal();");
            }
        }

        private Timesheet PopulateTimesheetObject(bool IsDraft)
        {
            Timesheet timesheet = new Timesheet();
            timesheet.ActivityId = ActivityId;
            timesheet.SubActivityId = SubActivityId;
            timesheet.ActualDate = RadDatePickerActualDate.SelectedDate.Value;
            timesheet.ActualQuantity = string.IsNullOrEmpty(txtActualQty.Text) ? 0 : Convert.ToDecimal(txtActualQty.Text);
            timesheet.SAPSite = SAPSite;
            timesheet.SAPNetwork = SAPNetwork;
            timesheet.SAPProject = SAPProjectId;
            timesheet.WBSAreaId = WBSAreaId;
            timesheet.SAPActivity = SAPActivity;
            timesheet.SAPSubActivity = SAPSubActivity;
            
            timesheet.Comments = txtComment.Text;

            if (Convert.ToInt32(ddlStatus.SelectedValue) != Convert.ToInt32(Constants.TimesheetStatus.Closed))
                timesheet.StatusId = IsDraft ? Convert.ToInt32(Constants.TimesheetStatus.Draft) : Convert.ToInt32(Constants.TimesheetStatus.Ongoing);// status - Draft or ongoing
            else
                timesheet.StatusId = Convert.ToInt32(Constants.TimesheetStatus.Closed); // closed

            timesheet.TimeSheetId = TimesheetId;

            List<BlockDetails> lstBlockDetails = (List<BlockDetails>)Session["timeSheetBlockDetails"];
            List<TimesheetActivity> inverterDetails = (List<TimesheetActivity>)Session["timeSheetInverterDetails"];
            List<SurveyMasterModel> lstSurveyDetails = (List<SurveyMasterModel>)Session["timeSheetSurvery"];

            List<TimesheetSurveyDetail> timesheetSurveyDetail = new List<TimesheetSurveyDetail>();
            List<TimesheetBlockDetail> timesheetBlockDetail = new List<TimesheetBlockDetail>();
            List<TimesheetActivity> timesheetActivityDetail = new List<TimesheetActivity>();
            List<TimesheetAttachment> lstAttachment = new List<TimesheetAttachment>();

            if (lstSurveyDetails != null)
            {
                foreach (var survey in lstSurveyDetails)
                {
                    timesheetSurveyDetail.Add(new TimesheetSurveyDetail()
                    {
                        ActualArea = survey.Area,
                        ActualNoOfDivision = survey.NoOfDivision,
                        SurveyNo = survey.SurveyNo,
                        VillageId = survey.VillageId
                    });
                }
            }
            if (lstBlockDetails != null)
            {
                foreach (var block in lstBlockDetails)
                {
                    timesheetBlockDetail.Add(new TimesheetBlockDetail()
                    {
                        ActualQuantity = block.ActualQuantity,
                        BlockNo = block.BlockNo
                    });
                }
            }

            if (inverterDetails != null)
            {
                foreach (var activity in inverterDetails)
                {
                    timesheetActivityDetail.Add(new TimesheetActivity()
                    {
                        FromRange = activity.FromRange,
                        ToRange = activity.ToRange,
                        Flag = TabName,
                        ProposedQuantity = Convert.ToInt32(activity.ProposedQuantity),
                        RangeType = activity.RangeType
                    });
                }
            }

            if (!string.IsNullOrEmpty(txtManualEntry.Text) && Session["manualEntryList"] != null)
            {
                List<TableActivity> lstActivity = (List<TableActivity>)Session["manualEntryList"];
                foreach (var activity in lstActivity)
                {
                    timesheetActivityDetail.Add(new TimesheetActivity()
                    {
                        FromRange = activity.Number,
                        ToRange = activity.Number,
                        Flag = TabName,
                        ProposedQuantity = activity.Quantity,
                        RangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL
                    });
                }
            }

            if (ViewState["AllAttachmentName"] != null && Convert.ToString(ViewState["AllAttachmentName"]) != "")
            {
                AllAttachmentlst = (List<Attachement>)ViewState["AllAttachmentName"];
                foreach (var attachment in AllAttachmentlst)
                {
                    lstAttachment.Add(new TimesheetAttachment()
                    {
                        FilePath = attachment.FileName,
                    });
                }
            }

            if (!string.IsNullOrEmpty(txtNumber.Text.Trim()))
            {
                string[] numberArray = txtNumber.Text.Trim().Split(',');
                int quantity = numberArray.Length;
                foreach (var number in numberArray)
                {
                    timesheetActivityDetail.Add(new TimesheetActivity()
                    {
                        FromRange = Convert.ToInt32(number),
                        ToRange = Convert.ToInt32(number),
                        Flag = TabName,
                        ProposedQuantity = quantity,
                        RangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL
                    });
                }
            }
            timesheet.TimesheetAttachments = lstAttachment;
            timesheet.TimesheetActivities = timesheetActivityDetail;
            timesheet.TimesheetBlockDetails = timesheetBlockDetail;
            timesheet.TimesheetSurveyDetails = timesheetSurveyDetail;
            return timesheet;
        }

        /// <summary>
        /// Get all blocks to display in drop down
        /// </summary>
        /// <returns></returns>
        private List<TableActivity> GetAllBlockDetails()
        {
            TableActivity tableActivity = new TableActivity();
            tableActivity.Flag = Constants.CONST_TABLE_ACTIVITY_BLOCK;
            tableActivity.ActivityId = SAPActivity.Trim();
            tableActivity.NetworkId = SAPNetwork.Trim();
            tableActivity.ProjectId = SAPProjectId.Trim();
            tableActivity.Site = SAPSite.Trim();
            tableActivity.ActivityId = SAPActivity.Trim();
            tableActivity.AreaId = WBSAreaId;
            tableActivity.SubActivityId = !string.IsNullOrEmpty(SAPSubActivity) && SAPSubActivity != "0" ? SAPSubActivity.Trim() : SAPActivity.Trim();
            return TableActivityModel.GetAllBlock(tableActivity);
        }

      /// <summary>
      /// Get praposed quantity for selected table activity
      /// </summary>
      /// <param name="TableActivityId"></param>
      /// <returns></returns>
        private int GetPraposedQuantity(int TableActivityId)
        {
            return TableActivityModel.GetPraposedQuantity(TableActivityId);
        }

        private int GetCompletedQuantity(int BlockNo)
        {
            return TableActivityModel.GetCompletedQuantity(SAPSubActivity, SAPActivity, SAPNetwork, WBSAreaId, SAPProjectId, SAPSite, BlockNo, TimesheetId);
        }

        private List<SurveyMaster> GetSurveyDetails(int VillageId)
        {
            return TimesheetModel.GetAllSurveyNumberForVillage(VillageId, SAPSite, SAPProjectId);
        }

        #region Survey Details

        private void GetSurveyDetails()
        {
            if (Session["timeSheetSurvery"] != null)
            {
                gridSurvey.DataSource = (List<SurveyMasterModel>)Session["timeSheetSurvery"];
                if (IsPartialApprove)
                    gridManagerSurvayDetails.DataSource = (List<SurveyMasterModel>)Session["timeSheetSurvery"];
            }
        }

        private void SaveSurveyDetails(GridCommandEventArgs e, string editMode)
        {
            try
            {
                int _SurveyId = 1;
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                List<SurveyMasterModel> SurveyDetails = (List<SurveyMasterModel>)Session["timeSheetSurvery"];

                if (SurveyDetails.Count > 0)
                    _SurveyId = SurveyDetails[SurveyDetails.Count - 1].SurveyId + 1;

                if (editableItem != null)
                {
                    RadDropDownList drpSite = (RadDropDownList)editableItem.FindControl("drpSite");
                    RadDropDownList drpVillage = (RadDropDownList)editableItem.FindControl("drpVillage");
                    RadNumericTextBox txtArea = (RadNumericTextBox)editableItem.FindControl("txtArea");
                    RadNumericTextBox txtPraposedArea = (RadNumericTextBox)editableItem.FindControl("txtPraposedArea");
                    RadDropDownList drpSurvayNo = (RadDropDownList)editableItem.FindControl("drpSurvayNo");
                    RadNumericTextBox txtNoOfDivision = (RadNumericTextBox)editableItem.FindControl("txtNoOfDivision");
                    RadNumericTextBox txtPraposedNoOfDivision = (RadNumericTextBox)editableItem.FindControl("txtPraposedNoOfDivision");
                    RadDropDownList drpProject = (RadDropDownList)editableItem.FindControl("drpProject");

                    decimal Area = Convert.ToDecimal(txtArea.Text);
                    decimal PraposedArea = Convert.ToDecimal(txtPraposedArea.Text);
                    string SurvyNo = Convert.ToString(drpSurvayNo.SelectedText);
                    string NoOfDiv = Convert.ToString(txtNoOfDivision.Text);
                    string praposedNoOfDiv = Convert.ToString(txtPraposedNoOfDivision.Text);
                    int villageId = Convert.ToInt32(drpVillage.SelectedValue);
                    bool status = true;
                    RadGrid gridObject = null;

                    if (IsPartialApprove)
                        txtApprovedQuantity.Text = Convert.ToString(SurveyDetails.Sum(l => l.Area) + Area);
                    else
                        txtActualQty.Text = Convert.ToString(SurveyDetails.Sum(l => l.Area) + Area);

                    decimal[] completedAreaDetails = GetCompletedSurveyDetails(Convert.ToInt32(drpVillage.SelectedValue), SurvyNo);

                    if (IsPartialApprove)
                    {
                        gridObject = gridManagerSurvayDetails;
                        List<SurveyMasterModel> lstUserSurveyDetails = Session[Constants.CONST_TIMESHEET_USER_SURVEY_DETAILS] as List<SurveyMasterModel>;
                        SurveyMasterModel selectedSurvey = lstUserSurveyDetails.FirstOrDefault(s => s.SurveyNo == SurvyNo && s.VillageId == villageId);
                        if (selectedSurvey != null)
                        {
                            if (selectedSurvey.Area < Area || selectedSurvey.NoOfDivision < Convert.ToInt32(NoOfDiv))
                            {
                                gridObject.MasterTableView.IsItemInserted = false;
                                gridObject.MasterTableView.ClearEditItems();
                                gridObject.MasterTableView.Rebind();
                                TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Area and No. of division should be less than Area and No. of Division entered by User.')");
                                return;
                            }
                        }
                    }
                    else
                        gridObject = gridSurvey;

                    SurveyMasterModel survey = new SurveyMasterModel()
                    {
                        SurveyId = _SurveyId,
                        Site = Convert.ToString(drpSite.SelectedValue),
                        VillageId = villageId,
                        VillageName = (Convert.ToString(drpVillage.SelectedText.Trim())),
                        SurveyNo = SurvyNo,
                        Area = Area,
                        NoOfDivision = Convert.ToInt32(NoOfDiv),
                        SAPProjectId = Convert.ToString(drpProject.SelectedValue),
                        ProjectDescription = Convert.ToString(drpProject.SelectedText.Trim()),
                        Status = status,
                        PraposedArea = PraposedArea,
                        PraposedNoOfDivision = Convert.ToInt32(praposedNoOfDiv),
                        CompletedArea = completedAreaDetails[0] + Area,
                        CompletedNoOfDivision = Convert.ToInt32(completedAreaDetails[1]) + Convert.ToInt32(NoOfDiv),
                        RemainingArea = PraposedArea - (completedAreaDetails[0] + Area),
                        RemainingNoOfDivision = Convert.ToInt32(praposedNoOfDiv) - Convert.ToInt32(Convert.ToInt32(completedAreaDetails[1]) + Convert.ToInt32(NoOfDiv))
                    };

                    if (editMode == Constants.CONST_NEW_MODE)
                        SurveyDetails.Add(survey);
                    else
                    {
                        _SurveyId = Convert.ToInt32(editableItem.GetDataKeyValue("SurveyId"));
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).SurveyNo = survey.SurveyNo;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).VillageId = survey.VillageId;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).Area = survey.Area;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).NoOfDivision = survey.NoOfDivision;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).PraposedArea = survey.PraposedArea;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).PraposedNoOfDivision = survey.PraposedNoOfDivision;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).CompletedArea = survey.CompletedArea;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).CompletedNoOfDivision = survey.CompletedNoOfDivision;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).RemainingArea = survey.RemainingArea;
                        SurveyDetails.FirstOrDefault(l => l.SurveyId == _SurveyId).RemainingNoOfDivision = survey.RemainingNoOfDivision;
                    }

                    if (TimesheetId != 0 && !IsPartialApprove)
                    {
                        TimesheetModel timesheetModel = new TimesheetModel();
                        List<TimesheetSurveyDetail> lstTimesheetSurvey = new List<TimesheetSurveyDetail>();
                        TimesheetSurveyDetail timesheetSurveyDetail = new TimesheetSurveyDetail()
                        {
                            SurveyNo = survey.SurveyNo,
                            VillageId = survey.VillageId,
                            ActualArea = survey.Area,
                            ActualNoOfDivision = survey.NoOfDivision
                        };

                        lstTimesheetSurvey.Add(timesheetSurveyDetail);

                        if (editMode == Constants.CONST_NEW_MODE)
                            timesheetModel.AddSurveyDetail(lstTimesheetSurvey, TimesheetId, UserId);
                        else
                            TimesheetModel.UpdateTimesheetSurvey(timesheetSurveyDetail);

                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Success','Survey details saved successfully.')");
                    }

                    Session["timeSheetSurvery"] = SurveyDetails;
                    CalculateSurveyActualQuantity(SurveyDetails);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void CalculateSurveyActualQuantity(List<SurveyMasterModel> SurveyDetails)
        {
            if (SurveyDetails != null && SurveyDetails.Count > 0)
            {
                if (IsPartialApprove)
                    txtApprovedQuantity.Text = Convert.ToString(SurveyDetails.Sum(l => l.Area));
                else
                    txtActualQty.Text = Convert.ToString(SurveyDetails.Sum(l => l.Area));
            }
            else
            {
                txtActualQty.Text = string.Empty;
                txtApprovedQuantity.Text = string.Empty;
            }

            if (TimesheetId != 0 && !IsPartialApprove)
                TimesheetModel.UpdateTimesheetActualQuantity(TimesheetId, Convert.ToDecimal(txtActualQty.Text), UserId);
        }

        private void DeleteSurveyDetails(GridCommandEventArgs e)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            int ID = Convert.ToInt32(editableItem.GetDataKeyValue("SurveyId").ToString());
            List<SurveyMasterModel> SurveyDetails = (List<SurveyMasterModel>)Session["timeSheetSurvery"];
            SurveyMasterModel surveyObj = SurveyDetails.Find(x => x.SurveyId == ID);

            SurveyDetails.Remove(surveyObj);
            Session["timeSheetSurvery"] = SurveyDetails;
            if (TimesheetId != 0 && !IsPartialApprove)
            {
                if (TimesheetModel.DeleteTimesheetSurvey(ID))
                {                    
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Success','Timesheet survey details deleted.')");
                }
            }

            CalculateSurveyActualQuantity(SurveyDetails);
        }

        private decimal[] GetCompletedSurveyDetails(int VillageId, string SurveyNumnber)
        {
            return SurveyModel.GetCompletedQuantity(SAPSubActivity, SAPActivity, SAPNetwork, WBSAreaId, SAPProjectId, SAPSite, VillageId, SurveyNumnber, TimesheetId);
        }

        #endregion

        #region Block Details Methods

        private void GetBlockDetails()
        {
            if (Session["timeSheetBlockDetails"] != null)
            {
                gridBlockDetails.DataSource = (List<BlockDetails>)Session["timeSheetBlockDetails"];
                if (IsPartialApprove)
                    gridManagerBlockDetails.DataSource= (List<BlockDetails>)Session["timeSheetBlockDetails"];
            }
        }

        private void SaveBlockDetails(GridCommandEventArgs e, string editMode)
        {
            try
            {
                int _blockId = 1;
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                List<BlockDetails> lstBlockDetails = (List<BlockDetails>)Session["timeSheetBlockDetails"];
                if (lstBlockDetails.Count > 0)
                    _blockId = lstBlockDetails[lstBlockDetails.Count - 1].BlockId + 1;

                RadGrid gridObject = null;
                if (IsPartialApprove)
                    gridObject = gridManagerBlockDetails;
                else
                    gridObject = gridBlockDetails;

                if (editableItem != null)
                {
                    RadDropDownList ddlBlockNo = (RadDropDownList)editableItem.FindControl("ddlBlockNo");
                    RadNumericTextBox txtProposedQuantity = (RadNumericTextBox)editableItem.FindControl("txtProposedQuantity");
                    RadNumericTextBox txtActualQuantity = (RadNumericTextBox)editableItem.FindControl("txtActualQuantity");
                    RadNumericTextBox txtCompletedQuantity = (RadNumericTextBox)editableItem.FindControl("txtCompletedQuantity");
                    RadNumericTextBox txtRemainingQuantity = (RadNumericTextBox)editableItem.FindControl("txtRemainingQuantity");

                    int BlockNo = Convert.ToInt32(ddlBlockNo.SelectedText);
                    int ActualQuantity = Convert.ToInt32(txtActualQuantity.Text);
                    int completedQuantity = GetCompletedQuantity(Convert.ToInt32(ddlBlockNo.SelectedText));

                    if (IsPartialApprove)
                    {
                        List<BlockDetails> userBlocks = Session[Constants.CONST_TIMESHEET_USER_BLOCK_DETAILS] as List<BlockDetails>;
                        int blockNumber = Convert.ToInt32(ddlBlockNo.SelectedText);
                        BlockDetails objBlock = userBlocks.Where(b => b.BlockNo == blockNumber).FirstOrDefault();
                        if (objBlock.ActualQuantity < ActualQuantity)
                        {
                            TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Quantity should not be greater than quantity entered by user.')");
                            gridObject.MasterTableView.IsItemInserted = false;
                            gridObject.MasterTableView.ClearEditItems();
                            gridObject.MasterTableView.Rebind();
                            return;
                        }
                    }

                    BlockDetails block = new BlockDetails()
                    {
                        BlockId = _blockId,
                        BlockNo = BlockNo,
                        ProposedQuantity = Convert.ToInt32(txtProposedQuantity.Text),
                        ActualQuantity = Convert.ToInt32(txtActualQuantity.Text),
                        RemainingQuantity = Convert.ToInt32(txtProposedQuantity.Text) - Convert.ToInt32(completedQuantity + Convert.ToInt32(txtActualQuantity.Text)),
                        CompletedQuantity = completedQuantity + Convert.ToInt32(txtActualQuantity.Text),
                    };

                    decimal totalAactualQuantity = Convert.ToDecimal(lstBlockDetails.Sum(l => l.ActualQuantity.Value) + block.ActualQuantity.Value);

                    if (TimesheetId != 0 && !IsPartialApprove)
                    {
                        TimesheetBlockDetail timesheetBlockDetail = new TimesheetBlockDetail()
                        {
                            BlockNo = block.BlockNo,
                            ActualQuantity = block.ActualQuantity,                            
                            TimesheetId = TimesheetId
                        };

                        if (editMode == Constants.CONST_NEW_MODE)
                        {
                            List<TimesheetBlockDetail> lstTimesheetBlockDetail = new List<TimesheetBlockDetail>();
                            lstTimesheetBlockDetail.Add(timesheetBlockDetail);

                            TimesheetModel timesheetModel = new TimesheetModel();
                            lstBlockDetails.Add(block);
                            timesheetModel.AddBlock(lstTimesheetBlockDetail, TimesheetId, UserId);
                        }
                        else
                        {
                            timesheetBlockDetail.TimesheetBlockDetailId = Convert.ToInt32(editableItem.GetDataKeyValue("BlockId"));
                            TimesheetModel.UpdateTimesheetBlock(timesheetBlockDetail);
                            _blockId = SetBlockListDetails(editableItem, lstBlockDetails, block);
                        }
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Success','Block details saved successfully.')");
                    }
                    else
                    {
                        if (editMode == Constants.CONST_NEW_MODE)
                            lstBlockDetails.Add(block);
                        else
                        {
                            _blockId = SetBlockListDetails(editableItem, lstBlockDetails, block);
                        }
                    }

                    UpdateActualQuantityForBlock(lstBlockDetails );                    
                    Session["timeSheetBlockDetails"] = lstBlockDetails;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private static int SetBlockListDetails(GridEditableItem editableItem, List<BlockDetails> lstBlockDetails, BlockDetails block)
        {
            int _blockId = Convert.ToInt32(editableItem.GetDataKeyValue("BlockId"));
            lstBlockDetails.FirstOrDefault(l => l.BlockId == _blockId).ActualQuantity = block.ActualQuantity;
            lstBlockDetails.FirstOrDefault(l => l.BlockId == _blockId).BlockNo = block.BlockNo;
            lstBlockDetails.FirstOrDefault(l => l.BlockId == _blockId).ProposedQuantity = block.ProposedQuantity;
            lstBlockDetails.FirstOrDefault(l => l.BlockId == _blockId).CompletedQuantity = block.CompletedQuantity;
            lstBlockDetails.FirstOrDefault(l => l.BlockId == _blockId).RemainingQuantity = block.RemainingQuantity;
            return _blockId;
        }

        private void UpdateActualQuantityForBlock(List<BlockDetails> lstBlockDetails)
        {
            if (lstBlockDetails != null && lstBlockDetails.Count > 0)
            {
                if (IsPartialApprove)
                    txtApprovedQuantity.Text = Convert.ToString(lstBlockDetails.Sum(l => l.ActualQuantity));
                else
                    txtActualQty.Text = Convert.ToString(lstBlockDetails.Sum(l => l.ActualQuantity));
            }
            else
            {
                txtActualQty.Text = string.Empty;
                txtApprovedQuantity.Text = string.Empty;
            }

            if (TimesheetId != 0 && !IsPartialApprove)
                TimesheetModel.UpdateTimesheetActualQuantity(TimesheetId, Convert.ToDecimal(txtActualQty.Text), UserId);
        }

        protected void gridBlockDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetBlockDetails();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void DeleteBlockDetails(GridCommandEventArgs e)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            int ID = Convert.ToInt32(editableItem.GetDataKeyValue("BlockId").ToString());
            List<BlockDetails> blockDetails = (List<BlockDetails>)Session["timeSheetBlockDetails"];
            BlockDetails blockObj = blockDetails.Find(x => x.BlockId == ID);
            //txtActualQty.Value = !string.IsNullOrEmpty(txtActualQty.Value) ? Convert.ToString(Convert.ToDecimal(txtActualQty.Value) - blockObj.ActualQuantity) : Convert.ToString(blockObj.ActualQuantity);
            blockDetails.Remove(blockObj);
            Session["timeSheetBlockDetails"] = blockDetails;
            if (TimesheetId != 0 && !IsPartialApprove)
                if (TimesheetModel.DeleteTimesheetBlock(ID))
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Success','Timesheet block deleted.')");

            UpdateActualQuantityForBlock(blockDetails);
        }

        #endregion

        #region Inverter Details

        private void GetInverterDetails()
        {
            if (Session["timeSheetInverterDetails"] != null)
            {
                gridInverter.DataSource = (List<TimesheetActivity>)Session["timeSheetInverterDetails"];
                if(IsPartialApprove)
                    gridManagerInvertor.DataSource = (List<TimesheetActivity>)Session["timeSheetInverterDetails"];
            }
        }

        private void SaveInverterDetails(GridCommandEventArgs e, string editMode)
        {
            try
            {
                RadGrid gridObject = null;
                if (IsPartialApprove)
                    gridObject = gridManagerInvertor;
                else
                    gridObject = gridInverter;

                int _inverterId = 1;
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                TimesheetActivity invDetails = e.Item.DataItem as TimesheetActivity;

                List<TimesheetActivity> lstInverterDetails = (List<TimesheetActivity>)Session["timeSheetInverterDetails"];

                if (lstInverterDetails.Count > 0)
                    if (lstInverterDetails[lstInverterDetails.Count - 1] != null)
                        _inverterId = lstInverterDetails[lstInverterDetails.Count - 1].TimesheetActivityId + 1;

                if (editableItem != null)
                {
                    RadTextBox txtFromRange = (RadTextBox)editableItem.FindControl("txtFromRange");
                    RadTextBox txtToRange = (RadTextBox)editableItem.FindControl("txtToRange");

                    int fromRange = txtFromRange != null ? Convert.ToInt32(txtFromRange.Text.Trim()) : 0;
                    int toRange = txtToRange != null ? Convert.ToInt32(txtToRange.Text.Trim()) : 0;
                    if (IsPartialApprove)
                    {
                        if (!ValidateNumbersWithUserDetails(fromRange, toRange))
                        {
                            TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','User has not added given number :" + fromRange + " and " + toRange + "')");
                            gridObject.MasterTableView.IsItemInserted = false;
                            gridObject.MasterTableView.ClearEditItems();
                            gridObject.MasterTableView.Rebind();
                            return;
                        }
                    }
                    // check range already exists or not.
                    if (ValidateDuplicateRange(fromRange, toRange) || ValidateManualRange(fromRange, toRange))
                    {
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Activity range already added.')");
                        gridObject.MasterTableView.IsItemInserted = false;
                        gridObject.MasterTableView.ClearEditItems();
                        gridObject.MasterTableView.Rebind();
                        return;
                    }

                    TableActivityRange tableActivityRange = GetActivityRangeObject(fromRange, toRange);

                    int[] missingNumbers = TableActivityModel.FindMissingTableNo(fromRange, toRange, tableActivityRange);

                    int[] numberArray = CommonFunctions.GetNumberRange(fromRange, toRange);
                    List<int> existingNumbers = TableActivityModel.CheckExistingNumbers(SAPSubActivity, SAPActivity, SAPNetwork, WBSAreaId, SAPProjectId, SAPSite, TimesheetId, numberArray, TabName);

                    if (existingNumbers.Count() > 0)
                    {
                        gridObject.MasterTableView.IsItemInserted = false;
                        gridObject.MasterTableView.ClearEditItems();
                        gridObject.MasterTableView.Rebind();
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Numbers (" + string.Join(",", existingNumbers) + ") already added in other timesheet.')");
                        return;
                    }

                    if (missingNumbers.Count() > 0)
                    {
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Following " + TabName + " number(s) not exists :" + string.Join(",", missingNumbers) + "')");
                        gridObject.MasterTableView.IsItemInserted = false;
                        gridObject.MasterTableView.ClearEditItems();
                        gridObject.MasterTableView.Rebind();
                        return;
                    }

                    List<TableActivity> lstTableActivity = GetPraposedQuantityForRange(fromRange, toRange, tableActivityRange);

                    if (lstTableActivity != null && lstTableActivity.Count > 0)
                    {
                        TimesheetActivity timesheetActivity = new TimesheetActivity()
                        {
                            FromRange = Convert.ToInt32(txtFromRange.Text),
                            ToRange = Convert.ToInt32(txtToRange.Text),
                            RangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_RANGE,
                            ProposedQuantity = lstTableActivity.Sum(t => t.Quantity),
                            Flag = TabName,
                            TimesheetActivityId = _inverterId
                        };

                        if (editMode == Constants.CONST_NEW_MODE)
                        {
                            lstInverterDetails.Add(timesheetActivity);
                        }
                        else
                        {
                            _inverterId = Convert.ToInt32(editableItem.GetDataKeyValue("TimesheetActivityId"));
                            lstInverterDetails.FirstOrDefault(l => l.TimesheetActivityId == _inverterId).FromRange = Convert.ToInt32(txtFromRange.Text);
                            lstInverterDetails.FirstOrDefault(l => l.TimesheetActivityId == _inverterId).ToRange = Convert.ToInt32(txtToRange.Text);
                            lstInverterDetails.FirstOrDefault(l => l.TimesheetActivityId == _inverterId).RangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_RANGE;
                            lstInverterDetails.FirstOrDefault(l => l.TimesheetActivityId == _inverterId).ProposedQuantity = lstTableActivity.Sum(t => t.Quantity);
                            lstInverterDetails.FirstOrDefault(l => l.TimesheetActivityId == _inverterId).RangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_RANGE;
                        }

                        if (TimesheetId != 0)
                        {
                            if (editMode == Constants.CONST_NEW_MODE)
                            {
                                TimesheetModel timesheetModel = new TimesheetModel();
                                List<TimesheetActivity> lstTimesheetActivity = new List<TimesheetActivity>();

                                lstTimesheetActivity.Add(timesheetActivity);
                                //timesheetModel.AddTimesheetActivity(lstTimesheetActivity, TimesheetId, UserId);
                            }
                        }

                        hidRangeQuantity.Value = Convert.ToString(Convert.ToDecimal(hidRangeQuantity.Value) + timesheetActivity.ProposedQuantity);
                        if (IsPartialApprove)
                        {
                            txtApprovedQuantity.Text = txtManagerQuantity.Text = Convert.ToString(Convert.ToDecimal(hidRangeQuantity.Value) + Convert.ToDecimal(hidManualEntryQuantity.Value));
                        }
                        else
                            txtActualQty.Text = txtQty.Text = Convert.ToString(Convert.ToDecimal(hidRangeQuantity.Value) + Convert.ToDecimal(hidManualEntryQuantity.Value));
                        Session["timeSheetInverterDetails"] = lstInverterDetails;
                    }
                    else
                    {
                        TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Activity range does not exists.')");
                        gridObject.MasterTableView.IsItemInserted = false;
                        gridObject.MasterTableView.ClearEditItems();
                        gridObject.MasterTableView.Rebind();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private TableActivityRange GetActivityRangeObject(int fromRange, int toRange)
        {
            return new TableActivityRange
            {
                ActivityId = SAPActivity,
                Flag = TabName,
                FromRange = fromRange,
                ToRange = toRange,
                NetworkId = SAPNetwork,
                ProjectId = SAPProjectId,
                Site = SAPSite,
                WBSAreaId = WBSAreaId,
                SubActivityId = SAPSubActivity
            };
        }

        private bool ValidateNumbersWithUserDetails(int SelectedFromRange, int SelectedToRange)
        {
            List<int> tableNumbers = GetArrayOfUserEnteredNumbers();

            if (SelectedToRange == -1)
                SelectedToRange = SelectedFromRange;
            if (tableNumbers.Contains(SelectedFromRange) && tableNumbers.Contains(SelectedToRange))
                return true;
            else
                return false;
        }

        private List<int> GetArrayOfUserEnteredNumbers()
        {
            List<int> tableNumbers = new List<int>();
            int[] tempArray = { };
            if (IsPartialApproveByQM)
            {
                List<ManagerTimesheetActivity> lstUserManualEntry = Session[Constants.CONST_TIMESHEET_INVEROER_MANUAL_DETAILS_MANAGER] as List<ManagerTimesheetActivity>;
                List<ManagerTimesheetActivity> lstUserRangeEntry = Session[Constants.CONST_TIMESHEET_INVEROER_RANGE_DETAILS_MANAGER] as List<ManagerTimesheetActivity>;
                
                foreach (var activity in lstUserRangeEntry)
                {
                    tempArray = GetNumberRange(activity.FromRange,activity.ToRange);
                    tableNumbers.AddRange(tempArray);
                }

                foreach (var activity in lstUserManualEntry)
                {
                    tempArray = GetNumberRange(activity.FromRange, activity.ToRange).ToArray();
                    tableNumbers.AddRange(tempArray);
                }
            }
            else
            {
                List<TimesheetActivity> lstUserManualEntry = Session[Constants.CONST_TIMESHEET_INVEROER_MANUAL_DETAILS_MANAGER] as List<TimesheetActivity>;
                List<TimesheetActivity> lstUserRangeEntry = Session[Constants.CONST_TIMESHEET_INVEROER_RANGE_DETAILS_MANAGER] as List<TimesheetActivity>;

                foreach (var activity in lstUserRangeEntry)
                {
                    tempArray = GetNumberRange(activity.FromRange.Value, activity.ToRange.Value).ToArray();
                    tableNumbers.AddRange(tempArray);
                    //for (int i = activity.FromRange.Value; i <= activity.ToRange; i++)
                    //    tableNumbers.Add(i);
                }

                foreach (var activity in lstUserManualEntry)
                {
                    tempArray = GetNumberRange(activity.FromRange.Value, activity.ToRange.Value).ToArray();
                    tableNumbers.AddRange(tempArray);
                    //for (int i = activity.FromRange.Value; i <= activity.ToRange; i++)
                    //    tableNumbers.Add(i);
                }
            }
            return tableNumbers;
        }

        private static int[] GetNumberRange(int FromRange, int ToRange)
        {
            return Enumerable.Range(FromRange, (ToRange - FromRange) + 1).ToArray();
        }

        private static List<TableActivity> GetPraposedQuantityForRange(int fromRange, int toRange,TableActivityRange tableActivityRange)
        {
            TableActivityModel tableActivityModel = new TableActivityModel();
            List<TableActivity> lstTableActivity = tableActivityModel.GetTableActivityByRange(tableActivityRange);
            return lstTableActivity;
        }

        private bool ValidateDuplicateRange(int SelectedFromRange, int SelectedToRange)
        {
            bool isDuplicate = false;
            if (IsPartialApprove)
            {
                if (gridManagerInvertor.Items.Count > 0)
                {
                    foreach (GridDataItem item in gridManagerInvertor.Items)
                    {
                        if (!item.Edit)
                        {
                            Label lblFromRange = (Label)item.FindControl("lblFromRange");
                            Label lblToRange = (Label)item.FindControl("lblToRange");

                            int toRange = !string.IsNullOrEmpty(lblToRange.Text) ? Convert.ToInt32(lblToRange.Text) : 0;
                            int fromRange = !string.IsNullOrEmpty(lblFromRange.Text) ? Convert.ToInt32(lblFromRange.Text) : 0;

                            List<int> rangeArray = new List<int>();
                            if (SelectedToRange == -1)
                                SelectedToRange = SelectedFromRange;
                            for (int i = fromRange; i <= toRange; i++)
                            {
                                rangeArray.Add(i);
                            }

                            if (rangeArray.Contains(SelectedFromRange) || rangeArray.Contains(SelectedToRange))
                            {
                                isDuplicate = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (gridInverter.Items.Count > 0)
                {
                    foreach (GridDataItem item in gridInverter.Items)
                    {
                        if (!item.Edit)
                        {
                            Label lblFromRange = (Label)item.FindControl("lblFromRange");
                            Label lblToRange = (Label)item.FindControl("lblToRange");

                            int toRange = !string.IsNullOrEmpty(lblToRange.Text) ? Convert.ToInt32(lblToRange.Text) : 0;
                            int fromRange = !string.IsNullOrEmpty(lblFromRange.Text) ? Convert.ToInt32(lblFromRange.Text) : 0;

                            List<int> rangeArray = new List<int>();
                            if (SelectedToRange == -1)
                                SelectedToRange = SelectedFromRange;
                            for (int i = fromRange; i <= toRange; i++)
                            {
                                rangeArray.Add(i);
                            }

                            //var numberList = Enumerable.Range(fromRange, toRange).ToList();

                            if (rangeArray.Contains(SelectedFromRange) || rangeArray.Contains(SelectedToRange))
                            {
                                isDuplicate = true;
                                break;
                            }
                        }
                    }
                }
            }
            return isDuplicate;
        }

        private bool ValidateManualRange(int SelectedFromRange, int SelectedToRange)
        {
            bool isExists = false;
            string manualRange = string.Empty;
            if (IsPartialApprove)
                manualRange = txtManagerManualEntry.Text.Trim();
            else
                manualRange = txtManualEntry.Text.Trim();

            string[] rangeArray = { };
            if (!string.IsNullOrEmpty(manualRange))
                rangeArray = manualRange.Split(',');
            if (rangeArray.Contains(SelectedFromRange.ToString()) || rangeArray.Contains(SelectedToRange.ToString()))
                isExists = true;

            return isExists;
        }
        #endregion

        #endregion

        #region "Partial Approve"
        #region "Manager Invertor"

        #region "Events"
        protected void btnCalculateNumbers_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty( txtNumber.Text.Trim()))
                {
                    string numberRange = txtNumber.Text.Trim().Trim(',');
                    string[] numberArray = numberRange.Split(',');
                    txtActualQty.Text = Convert.ToString(numberRange.Length);
                    txtActualQty.Visible = true;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManagerInvertor_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetInverterDetails();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddManagerRange_Click(object sender, EventArgs e)
        {
            try
            {
                gridManagerInvertor.MasterTableView.ClearEditItems();
                gridManagerInvertor.MasterTableView.IsItemInserted = true;
                gridManagerInvertor.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion

        #region "Methods"

        private void ValidateManagerBlockNumber(int BlockNumber, decimal ActuallyQuantity)
        {
            List<BlockDetails> gridtItems = gridBlockDetails.DataSource as List<BlockDetails>;
        }

        #endregion
        #endregion

        #region "ManagerBlockDetails"

        protected void gridManagerBlockDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetBlockDetails();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddManagerBlock_Click(object sender, EventArgs e)
        {
            try
            {
                gridManagerBlockDetails.MasterTableView.ClearEditItems();
                gridManagerBlockDetails.MasterTableView.IsItemInserted = true;
                gridManagerBlockDetails.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        #endregion

        #region "Manager Survey"
        protected void gridManagerSurvayDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetSurveyDetails();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManagerSurvayDetails_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                DeleteSurveyDetails(e);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddManagerSurvey_Click(object sender, EventArgs e)
        {
            try
            {
                gridManagerSurvayDetails.MasterTableView.ClearEditItems();
                gridManagerSurvayDetails.MasterTableView.IsItemInserted = true;
                gridManagerSurvayDetails.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        #endregion

        protected void btnPartialApprove_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(txtManagerNumber.Text.Trim()))
                //{
                Timesheet timesheet = new Timesheet();
                timesheet.TimeSheetId = TimesheetId;
                timesheet.ActivityId = ActivityId;
                timesheet.SubActivityId = SubActivityId;
                timesheet.ActualDate = RadDatePickerActualDate.SelectedDate.Value;
                //timesheet.ActualQuantity = string.IsNullOrEmpty(txtActualQty.Text) ? 0 : Convert.ToDecimal(txtActualQty.Text);
                timesheet.SAPSite = SAPSite;
                timesheet.SAPNetwork = SAPNetwork;
                timesheet.SAPProject = SAPProjectId;
                timesheet.WBSAreaId = WBSAreaId;
                timesheet.SAPActivity = SAPActivity;
                timesheet.SAPSubActivity = SAPSubActivity;
                timesheet.Comments = txtManagerComments.Text.Trim();
                timesheet.ModifiedBy = UserId;
                timesheet.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);

                if (Session["ProfileName"].ToString() == Constants.CONST_SITE_QUALITY_MANAGER)
                    timesheet.WorkflowStatusId = Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByQM);

                else if (Session["ProfileName"].ToString() == Constants.CONST_SITE_FUNCTIONAL_MANAGER
                                    || Session["ProfileName"].ToString() == Constants.CONST_DESIGN_MANAGER)
                    timesheet.WorkflowStatusId = Convert.ToInt32(Constants.WorkflowStatus.PartialApprovedByPM);

                Int16 approvedBy = GetUserProfile();

                switch (TabName.ToLower())
                {
                    case "table":
                    case "inverter":
                    case "scb":
                        SaveModifiedInvertorDetails(timesheet, approvedBy);
                        break;
                    case "block":
                        SaveModifiedBlockDetailsByManager(timesheet, approvedBy);
                        break;
                    case "survey":
                        SaveModifiedSurveyDetailsByManager(timesheet, approvedBy);
                        break;
                    case "number":
                    case "location":
                        if (!SaveModifiedNumberDetails(timesheet, approvedBy))
                            return;
                        break;
                    default:
                        SaveModifiedActualAmount(timesheet);
                        break;
                }

                TimesheetModel timesheetModel = new TimesheetModel();
                timesheetModel.PartialApproveTimesheet(timesheet);
                Session[Constants.CONST_SESSION_IS_PARTIAL_APPROVE] = null;
                TimesheetManagementRadAjaxManager.ResponseScripts.Add("CloseModal();");
                RemoveSession();
                //}
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #region "Private Methods"
        private void SaveModifiedActualAmount(Timesheet timesheet)
        {
            if (txtApprovedQuantity.Text.Trim() != string.Empty)
            {
                decimal origianalQuantity = Convert.ToDecimal(txtActualQty.Text.Trim());
                decimal modifiedQuantity = Convert.ToDecimal(txtApprovedQuantity.Text.Trim());
                if (modifiedQuantity <= 0)
                {
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Quantity should not be 0')");
                    return;
                }
                else if (modifiedQuantity > origianalQuantity)
                {
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Quantity should not be greater than quantiy entered by User')");
                    return;
                }
                else
                    timesheet.ApprovedQuantity = Convert.ToDecimal(txtApprovedQuantity.Text.Trim());
            }
            else
            {
                TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','Quantity should not be empty.')");
                return;
            }
        }
        private void SaveModifiedSurveyDetailsByManager(Timesheet timesheet, Int16 approvedBy)
        {
            List<SurveyMasterModel> lstModifiedDetails = Session["timeSheetSurvery"] as List<SurveyMasterModel>;
            List<SurveyMasterModel> lstUserSurveyDetails = Session[Constants.CONST_TIMESHEET_USER_SURVEY_DETAILS] as List<SurveyMasterModel>;
            List<ManagerTimesheetSurveyDetail> lstMangerSurveyDetails = new List<ManagerTimesheetSurveyDetail>();
            List<SurveyMasterModel> lstRejectedSureveyDetails = lstUserSurveyDetails.Except(lstModifiedDetails).ToList();

            foreach (SurveyMasterModel survey in lstModifiedDetails)
            {
                lstMangerSurveyDetails.Add(new ManagerTimesheetSurveyDetail()
                {
                    ActualArea = survey.Area,
                    ActualNoOfDivision = survey.NoOfDivision,
                    SurveyNo = survey.SurveyNo,
                    VillageId = survey.VillageId,
                    TimesheetId = TimesheetId,
                    Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved),
                    ManagerType = approvedBy,
                    CreatedBy = UserId,
                    ModifiedBy = UserId,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                });
            }

            foreach (SurveyMasterModel survey in lstRejectedSureveyDetails)
            {
                SurveyMasterModel modifiedSurvey = lstModifiedDetails.FirstOrDefault(l => l.SurveyNo == survey.SurveyNo
                                                                                       && l.VillageId == survey.VillageId);
                if (modifiedSurvey != null)
                {
                    survey.Area = survey.Area - modifiedSurvey.Area;
                    survey.NoOfDivision = survey.NoOfDivision - modifiedSurvey.NoOfDivision;
                }
                if (survey.Area > 0 || survey.NoOfDivision > 0)
                {
                    lstMangerSurveyDetails.Add(
                        new ManagerTimesheetSurveyDetail()
                        {
                            ActualArea = survey.Area,
                            ActualNoOfDivision = survey.NoOfDivision,
                            SurveyNo = survey.SurveyNo,
                            VillageId = survey.VillageId,
                            TimesheetId = TimesheetId,
                            Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Rejected),
                            ManagerType = approvedBy,
                            CreatedBy = UserId,
                            ModifiedBy = UserId,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        }
                        );
                }
            }
            timesheet.ApprovedQuantity = lstModifiedDetails.Sum(l => l.Area);
            timesheet.ManagerTimesheetSurveyDetails = lstMangerSurveyDetails;
        }

        private void SaveModifiedBlockDetailsByManager(Timesheet timesheet, Int16 approvedBy)
        {
            List<BlockDetails> lstModifiedBlock = Session["timeSheetBlockDetails"] as List<BlockDetails>;
            List<ManagerTimesheetBlockDetail> lstBlockDetails = new List<ManagerTimesheetBlockDetail>();
            List<BlockDetails> lstUserBlock = Session[Constants.CONST_TIMESHEET_USER_BLOCK_DETAILS] as List<BlockDetails>;
            List<BlockDetails> lstRejectedBlock = lstUserBlock.Except(lstModifiedBlock).ToList();

            foreach (BlockDetails block in lstModifiedBlock)
            {
                lstBlockDetails.Add(new ManagerTimesheetBlockDetail()
                {
                    ActualQuantity = block.ActualQuantity,
                    BlockNo = block.BlockNo,
                    TimesheetId = TimesheetId,
                    Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved),
                    ManagerType = approvedBy,
                    CreatedBy = UserId,
                    ModifiedBy = UserId,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                });
            }

            foreach (BlockDetails block in lstRejectedBlock)
            {
                BlockDetails modifiedBlock = lstModifiedBlock.FirstOrDefault(l => l.BlockNo == block.BlockNo);
                if (modifiedBlock != null)
                {
                    block.ActualQuantity = block.ActualQuantity - modifiedBlock.ActualQuantity;
                }

                if (block.ActualQuantity > 0)
                {
                    lstBlockDetails.Add(new ManagerTimesheetBlockDetail()
                    {
                        ActualQuantity = block.ActualQuantity,
                        BlockNo = block.BlockNo,
                        TimesheetId = TimesheetId,
                        Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Rejected),
                        ManagerType = approvedBy,
                        CreatedBy = UserId,
                        ModifiedBy = UserId,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }
            }
            timesheet.ManagerTimesheetBlockDetails = lstBlockDetails;
            timesheet.ApprovedQuantity = lstModifiedBlock.Sum(l => l.ActualQuantity);
        }

        private void SaveModifiedInvertorDetails(Timesheet timesheet, Int16 approvedBy)
        {
            List<int> lstUserEnteredNumbers = GetArrayOfUserEnteredNumbers();
            List<TimesheetActivity> inverterRangeDetails = (List<TimesheetActivity>)Session["timeSheetInverterDetails"];
            List<TableActivity> inverterManualEntryDetails = (List<TableActivity>)Session["manualEntryList"];
            List<int> tableNumbers = new List<int>();

            int[] manualEntriesByUser = { };
            if (!string.IsNullOrEmpty(txtManualEntry.Text.Trim()))
            {
                manualEntriesByUser = Array.ConvertAll(txtManualEntry.Text.Trim().Split(','), int.Parse);
            }

            int[] tempArray = { };
            if (inverterRangeDetails != null && inverterRangeDetails.Count > 0)
            {
                foreach (TimesheetActivity activity in inverterRangeDetails)
                {
                    tempArray = GetNumberRange(activity.FromRange.Value, activity.ToRange.Value).ToArray();
                    tableNumbers.AddRange(tempArray);
                }
            }

            if (inverterManualEntryDetails != null && inverterManualEntryDetails.Count > 0)
            {
                foreach (TableActivity activity in inverterManualEntryDetails)
                    tableNumbers.Add(activity.Number);
            }

            int[] rejectedNumbers = lstUserEnteredNumbers.Except(tableNumbers).ToArray();
            ManualEntryRange manualEntryRange = new ManualEntryRange()
            {
                ActivityId = SAPActivity,
                Flag = TabName,
                NetworkId = SAPNetwork,
                ProjectId = SAPProjectId,
                RangeArray = rejectedNumbers,
                Site = SAPSite,
                WBSAreaId = WBSAreaId,
                SubActivityId = SAPSubActivity
            };

            TableActivityModel tableActivity = new TableActivityModel();
            List<TableActivity> lstRejectedActivity = tableActivity.GetTableActivityByManualEntry(manualEntryRange);

            List<ManagerTimesheetActivity> timesheetActivityDetail = new List<ManagerTimesheetActivity>();
            if (inverterRangeDetails != null)
            {
                foreach (var activity in inverterRangeDetails)
                {
                    timesheetActivityDetail.Add(new ManagerTimesheetActivity()
                    {
                        TimesheetId = TimesheetId,
                        FromRange = activity.FromRange.Value,
                        ToRange = activity.ToRange.Value,
                        Flag = TabName,
                        ProposedQuantity = Convert.ToInt32(activity.ProposedQuantity),
                        RangeType = activity.RangeType,
                        Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved),
                        ManagerType = approvedBy,
                        CreatedBy = UserId,
                        ModifiedBy = UserId,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }
            }

            if (!string.IsNullOrEmpty(txtManagerManualEntry.Text) && Session["manualEntryList"] != null)
            {
                List<TableActivity> lstActivity = (List<TableActivity>)Session["manualEntryList"];
                foreach (var activity in lstActivity)
                {
                    timesheetActivityDetail.Add(new ManagerTimesheetActivity()
                    {
                        TimesheetId = TimesheetId,
                        FromRange = activity.Number,
                        ToRange = activity.Number,
                        Flag = TabName,
                        ProposedQuantity = activity.Quantity,
                        RangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL,
                        Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved),
                        ManagerType = approvedBy,
                        CreatedBy = UserId,
                        ModifiedBy = UserId,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }
            }

            string rangeType = string.Empty;
            if (lstRejectedActivity != null)
            {
                foreach (var activity in lstRejectedActivity)
                {
                    if (manualEntriesByUser.Contains(activity.Number))
                        rangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL;
                    else
                        rangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_RANGE;

                    timesheetActivityDetail.Add(new ManagerTimesheetActivity()
                    {
                        TimesheetId = TimesheetId,
                        FromRange = activity.Number,
                        ToRange = activity.Number,
                        Flag = TabName,
                        ProposedQuantity = Convert.ToInt32(activity.Quantity),
                        RangeType = rangeType,
                        Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Rejected),
                        ManagerType = approvedBy,
                        CreatedBy = UserId,
                        ModifiedBy = UserId,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }
            }

            timesheet.ApprovedQuantity = string.IsNullOrEmpty(txtManagerQuantity.Text) ? 0 : Convert.ToDecimal(txtManagerQuantity.Text);
            timesheet.ManagerTimesheetActivities = timesheetActivityDetail;
        }

        private bool SaveModifiedNumberDetails(Timesheet timesheet, Int16 approvedBy)
        {
            if (!string.IsNullOrEmpty(txtManagerNumber.Text.Trim()))
            {
                int[] userNumberArray = Array.ConvertAll(txtNumber.Text.Trim().Split(','), int.Parse);
                int[] numberArray = Array.ConvertAll(txtManagerNumber.Text.Trim().Split(','), int.Parse);

                List<int> invalidNumbers = new List<int>();

                foreach (int element in numberArray)
                {
                    if (!userNumberArray.Contains(element))
                        invalidNumbers.Add(element);
                }

                if (invalidNumbers.Count > 0)
                {
                    int[] validNumbers = numberArray.Except(invalidNumbers).ToArray();
                    txtManagerNumber.Text = string.Join(",", validNumbers);
                    txtApprovedQuantity.Text = validNumbers.Count() > 0 ? validNumbers.Length.ToString() : string.Empty;
                    TimesheetManagementRadAjaxManager.ResponseScripts.Add("showNotification('Error','User has not entered numbers :" + string.Join(",", invalidNumbers) + "')");
                    return false;
                }

                int quantity = numberArray.Length;
                List<ManagerTimesheetActivity> lstModifiedNumbers = new List<ManagerTimesheetActivity>();
                int[] rejectedNumbers = userNumberArray.Except(numberArray).ToArray();

                foreach (var number in numberArray)
                {
                    lstModifiedNumbers.Add(new ManagerTimesheetActivity()
                    {
                        FromRange = Convert.ToInt32(number),
                        ToRange = Convert.ToInt32(number),
                        Flag = TabName,
                        ProposedQuantity = quantity,
                        RangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL,
                        Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Approved),
                        ManagerType = approvedBy,
                        CreatedBy = UserId,
                        ModifiedBy = UserId,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }

                foreach (var number in rejectedNumbers)
                {
                    lstModifiedNumbers.Add(new ManagerTimesheetActivity()
                    {
                        FromRange = Convert.ToInt32(number),
                        ToRange = Convert.ToInt32(number),
                        Flag = TabName,
                        ProposedQuantity = quantity,
                        RangeType = Constants.CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL,
                        Status = Convert.ToInt16(Constants.ManagerTimesheetStatus.Rejected),
                        ManagerType = approvedBy,
                        CreatedBy = UserId,
                        ModifiedBy = UserId,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }

                timesheet.ApprovedQuantity = quantity;
                timesheet.ManagerTimesheetActivities = lstModifiedNumbers;
            }

            return true;
        }
        private Int16 GetUserProfile()
        {
            string approvedBy = Convert.ToString(Session["ProfileName"]).ToLower();
            if (approvedBy == Constants.CONST_SITE_FUNCTIONAL_MANAGER.ToLower())
                return Convert.ToInt16(Constants.ManagerType.PM);
            else if (approvedBy == Constants.CONST_SITE_QUALITY_MANAGER.ToLower())
                return Convert.ToInt16(Constants.ManagerType.QM);
            return 0;
        }

        private void RemoveSession()
        {
            Session["timeSheetSurvery"] = null;
            Session["timeSheetBlockDetails"] = null;
            Session["timeSheetInverterDetails"] = null;
            Session["manualEntryList"] = null;
            Session[Constants.CONST_TIMESHEET_USER_BLOCK_DETAILS] = null;
            Session[Constants.CONST_SESSION_TIMESHEET_ID] = null;
            Session[Constants.CONST_TIMESHEET_USER_SURVEY_DETAILS] = null;
            Session[Constants.CONST_SESSION_IS_PARTIAL_APPROVE] = null;
            Session[Constants.CONST_SESSION_ACTIVITY_ID] = null;
            Session[Constants.CONST_SESSION_SUBACTIVITY_ID] = null;
            Session[Constants.CONST_SESSION_TIMESHEET_PARAMETER] = null;
            Session[Constants.CONST_TIMESHEET_INVEROER_RANGE_DETAILS_MANAGER] = null;
            Session[Constants.CONST_TIMESHEET_INVEROER_MANUAL_DETAILS_MANAGER] = null;
        }

        #endregion

        #endregion
    }

    public class SurveyMasterModel : SurveyMaster
    {
        public string VillageName { get; set; }
        public string SAPProjectId { get; set; }
        public string ProjectDescription { get; set; }

        public decimal PraposedArea { get; set; }
        public int PraposedNoOfDivision { get; set; }

        public decimal CompletedArea { get; set; }
        public decimal RemainingArea { get; set; }
        public int CompletedNoOfDivision { get; set; }
        public int RemainingNoOfDivision { get; set; }
    }

    public class BlockDetails : TimesheetBlockDetail
    {
        public int BlockId { get; set; }
        public int ProposedQuantity { get; set; }
        public int CompletedQuantity { get; set; }
        public int RemainingQuantity { get; set; }
    }
}
