using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class AddVendorBankDetail : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        List<CommentDtl> CommentLstAdd = new List<CommentDtl>();
        List<CommentDtl> CommentLstShow = new List<CommentDtl>();


        string AttachmentChq = string.Empty;
        string AttachmentCert = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.BANKDETAILLIST, menuList))Response.Redirect("~/webNoAccess.aspx");
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
                {
                    string[] queryString = Request.QueryString.ToString().Split('?');
                    string selectedId = Crypto.Instance.Decrypt(HttpUtility.UrlDecode(queryString[0])).Split('=')[1];

                    if (CheckMailBankDetailAccess(Convert.ToInt32(selectedId)))
                        Session["BankDetailId"] = selectedId;
                    else
                    {
                        Session["BankDetailId"] = "0";
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show("You are not authorized user to view requested bank detail");
                    }
                }
                bindDropdow();               
                ViewState["IsExist"] = "No";
                

                hidSerachUserId.Value = "0";
                                
                int profileId = Convert.ToInt32(Session["ProfileId"]);

                if (profileId == (int)UserProfileEnum.Vendor)
                    hidSerachUserId.Value = Convert.ToString(HttpContext.Current.Session["UserId"]);

                if (Convert.ToString(Session["BankDetailId"])!="0")
                 {
                    ViewState["RequestType"] = "Old";
                    if (Convert.ToBoolean(Session["CBPendingDoc"]))
                        BindVendorBankDetailForCB(Convert.ToInt32(Session["BankDetailHistoryId"]));
                    else
                    hideShowFieldEdit();                  
                 }
                else
                {
                    hideShowFieldAdd();
                    ViewState["RequestType"] = "New";
                }      
            }
        }
        private bool CheckMailBankDetailAccess(int bankdetailId)
        {
            string result = string.Empty;
            result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_PENDING_RECORDS, string.Empty, string.Empty), string.Empty);
            if (result == Constants.REST_CALL_FAILURE)
            {
                return false;
            }
            else
            {
                List<GetBankPendingRecords_Result> pendingRecords = JsonConvert.DeserializeObject<List<GetBankPendingRecords_Result>>(result);
                if (pendingRecords.Where(w => w.BankDetailId == bankdetailId).ToList().Count() > 0)
                    return true;
                else
                {
                     int profileId = Convert.ToInt32(Session["ProfileId"]);
                    if (profileId == 10 || profileId == 9)
                    {
                        string resultBankDetail = commonFunctions.RestServiceCall(String.Format(Constants.VENDOR_BANK_GETDETAIL, bankdetailId), string.Empty);

                        if (resultBankDetail == Constants.REST_CALL_FAILURE)
                        {
                            return false;
                        }
                        else
                        {
                            BankDetailModel bankdetail = JsonConvert.DeserializeObject<BankDetailModel>(resultBankDetail);
                            if (bankdetail.WorkFlowStatusId == 25 && bankdetail.CreatedBy == Convert.ToInt32(HttpContext.Current.Session["UserId"]))
                                return true;
                            else
                                return false;
                        }
                    }
                    else
                        return false;
                }
            }
        }


        



        private void BindVendorBankDetailForCB(int BankDetailHistoryId)
        {
            try
            {
                string resultOldBankDetail = commonFunctions.RestServiceCall(String.Format(Constants.VENDOR_BANK_GETHISTORY_CBDOC, BankDetailHistoryId), string.Empty);

                if (resultOldBankDetail == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    BankDetail oldbankdetail = JsonConvert.DeserializeObject<BankDetail>(resultOldBankDetail);
                    if (oldbankdetail != null)
                    {

                        //GetVendorCode();
                        DrpVendorCode.Items.Add(new RadComboBoxItem(oldbankdetail.VendorCode + "-" + oldbankdetail.VendorName + "-" + oldbankdetail.City, oldbankdetail.VendorCode));
                        DrpVendorCode.SelectedIndex = 0;
                        DrpVendorCode.SelectedValue = Convert.ToString(oldbankdetail.VendorCode);


                       // DrpVendorCode.SelectedValue = Convert.ToString(oldbankdetail.VendorCode);                       
                        bindCompanyCode(Convert.ToString(DrpVendorCode.SelectedValue));
                        drpCompanyCode.SelectedValue = Convert.ToString(oldbankdetail.CompanyCode);
                        txtVendorName.Text = Convert.ToString(oldbankdetail.VendorName);
                        txtPanCardNo.Text = Convert.ToString(oldbankdetail.VendorPanNo);

                        drpNewAccountType.SelectedValue = Convert.ToString(oldbankdetail.AccountType);
                        txtNewAccountNo.Text = Convert.ToString(oldbankdetail.AccountNumber);
                        txtNewIFSCCode.Text = Convert.ToString(oldbankdetail.IFSCCode);
                        txtNewBankName.Text = Convert.ToString(oldbankdetail.BankName);
                        txtNewBranch.Text = Convert.ToString(oldbankdetail.BranchName);                       
                        txtNewCity.Text = Convert.ToString(oldbankdetail.City);
                        txtNewSuzlonEmail1.Text = Convert.ToString(oldbankdetail.SuzlonEmailID1);
                        txtNewSuzlonEmail2.Text = Convert.ToString(oldbankdetail.SuzlonEmailID2);
                        txtNewVendorEmail1.Text = Convert.ToString(oldbankdetail.VendorEmailID1);
                        txtNewVendorEmail2.Text = Convert.ToString(oldbankdetail.VendorEmailID2);

                        drpSubVertical.SelectedValue = Convert.ToString(oldbankdetail.SubVerticalId);
                        GetVerticals(drpSubVertical.SelectedValue);
                        drpVertical.SelectedValue = Convert.ToString(oldbankdetail.VerticalId);
                        string FilePath = Constants.VENDOR_BANK_ATTACHMENT_PATH;
                        if (!string.IsNullOrEmpty(oldbankdetail.Attachment1))
                        {
                            attachment1.NavigateUrl  = FilePath + oldbankdetail.Attachment1;
                            //lblAttachment1.Visible = true;
                            attachment1.Visible = true;
                            ViewState["CancelCheque"] = Convert.ToString(oldbankdetail.Attachment1);
                        }
                        else
                        {
                            //lblAttachment1.Visible = false;
                            attachment1.Visible = false;
                        }

                        if (!string.IsNullOrEmpty(oldbankdetail.Attachment2))
                        {
                            attachment2.Visible = true;
                            attachment2.NavigateUrl = FilePath + oldbankdetail.Attachment2;
                            //lblAttachment2.Visible = true;
                            ViewState["AccountCertificate"] = Convert.ToString(oldbankdetail.Attachment2);
                        }
                        else
                        {
                            //lblAttachment2.Visible = false;
                            attachment2.Visible = false;
                        }

                        if (Convert.ToString(Session["ActiveTab"]) == "CBDocPending")
                        {
                            chkDocReceived.Visible = true;
                            receiveDate.Visible = true;
                            chkDocSent.Visible = true;
                            dispatchDate.Visible = true;
                            chkDocReceived.Enabled = false;
                            receiveDate.Enabled = false;
                            chkDocSent.Enabled = false;
                            dispatchDate.Enabled = false;
                        }
                        else
                        {
                            chkDocSent.Visible = true;
                            dispatchDate.Visible = true;
                            chkDocSent.Enabled = false;
                            dispatchDate.Enabled = false;
                        }

                        if (Convert.ToBoolean(oldbankdetail.OriginalDocumentsSent))
                        {
                            chkDocSent.Visible = true;
                            lblDispatchDate.Visible = true;
                            dispatchDate.Visible = true;
                            chkDocSent.Checked = true;
                            dispatchDate.SelectedDate = oldbankdetail.SendDate;

                        }
                        if (Convert.ToBoolean(oldbankdetail.OriginalDocumentsSent))
                            chkDocSent.Checked = true;
                        else
                            chkDocSent.Checked = false;
                        if (oldbankdetail.SendDate != null)
                            dispatchDate.SelectedDate = Convert.ToDateTime(oldbankdetail.SendDate);

                        if (Convert.ToBoolean(oldbankdetail.OriginalDocumentsReceived))
                            chkDocReceived.Checked = true;
                        else
                            chkDocReceived.Checked = false;
                        if (oldbankdetail.ReceivedDate != null)
                            receiveDate.SelectedDate = Convert.ToDateTime(oldbankdetail.ReceivedDate);

                        txtOldAccType.Text = (Convert.ToString(oldbankdetail.AccountType) == "SAV" ? "Saving" : "Current");
                        txtOldAccountNo.Text = Convert.ToString(oldbankdetail.AccountNumber);
                        txtOldIFSCCode.Text = Convert.ToString(oldbankdetail.IFSCCode);
                        txtOldBankName.Text = Convert.ToString(oldbankdetail.BankName);
                        txtOldBranch.Text = Convert.ToString(oldbankdetail.BranchName);                    
                        txtOldCity.Text = Convert.ToString(oldbankdetail.City);
                        txtOldSuzlonEmail1.Text = Convert.ToString(oldbankdetail.SuzlonEmailID1);
                        txtOldSuzlonEmail2.Text = Convert.ToString(oldbankdetail.SuzlonEmailID2);
                        txtOldVendorEmail1.Text = Convert.ToString(oldbankdetail.VendorEmailID1);
                        txtOldVendorEmail2.Text = Convert.ToString(oldbankdetail.VendorEmailID2);

                       


                        lblLogdtl.Visible = false;
                        grdLogDetail.Visible = false;
                        grdLogDetail.DataBind();

                        BtnSave.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void bindExisingDetail()
        {
            int profileId = Convert.ToInt32(Session["ProfileId"]);
            BindVendorBankDetail(Convert.ToInt32(Session["BankDetailId"]));

            int workStatusId = 0;
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["WorkFlowStatusId"])))
                workStatusId = Convert.ToInt32(ViewState["WorkFlowStatusId"]);


            drpNewAccountType.Enabled = true;
            txtNewAccountNo.Enabled = true;
            txtNewIFSCCode.Enabled = true;
            txtNewBankName.Enabled = true;
            txtNewBranch.Enabled = true;
            txtNewCity.Enabled = true;
            txtNewSuzlonEmail1.Enabled = true;
            txtNewSuzlonEmail2.Enabled = true;
            txtNewVendorEmail1.Enabled = true;
            txtNewVendorEmail2.Enabled = true;
            drpSubVertical.Enabled = true;
            drpVertical.Enabled = true;
            uploadCancelChq.Enabled = true;
            uploadAccCertificate.Enabled = true;
            txtCommentBox.Enabled = true;
            attachment1.Enabled = true;
            attachment2.Enabled = true;

            //ViewState["CancelCheque"] = string.Empty;
            //ViewState["AccountCertificate"] = string.Empty;
            if (workStatusId == (int)WorkFlowStatusEnum.ApprovedByCB)
            {
                chkDocSent.Checked = false;
                dispatchDate.SelectedDate = null;
            }
            if (profileId == (int)UserProfileEnum.Vendor)
            {
                drpAssignTo.Visible = false;
                lblAssignto.Visible = false;
                lblStatus.Visible = false;
                drpStatus.Visible = false;
                drpSubVertical.Visible = false;
                drpVertical.Visible = false;
                lblVertical.Visible = false;
                lblSubVertical.Visible = false;
            }

            lblRequestType.Text = "Change Request";
            lblRequestType.CssClass = "text-danger";

        }

        private void hideShowFieldAdd()
        {
           // GetVendorCode();
            int profileId = Convert.ToInt32(Session["ProfileId"]);
            lblStatus.Visible = false;
            drpStatus.Visible = false;
            drpAssignTo.Visible = false;
            lblAssignto.Visible = false;
            DrpVendorCode.Enabled = true;
            drpCompanyCode.Enabled = true;
            txtVendorName.Enabled = false;
            txtPanCardNo.Enabled = false;
            //lblAttachment1.Visible = false;
           // attachment1.Visible = false;
            //lblAttachment2.Visible = false;
            attachment2.Visible = false;
            attachment1.Enabled = false;
            lbldocSent.Visible = true;
            chkDocSent.Visible = true;
            lblDispatchDate.Visible = true;
            dispatchDate.Visible = true;



            lblRequestType.Text = "New Request";

            if (profileId == (int)UserProfileEnum.Vendor)
            {
                drpSubVertical.Visible = false;
                drpVertical.Visible = false;
                lblVertical.Visible = false;
                lblSubVertical.Visible = false;

            }
            else
            {
                drpSubVertical.Visible = true;
                drpVertical.Visible = true;
                lblVertical.Visible = true;
                lblSubVertical.Visible = true;
            }
            lblLogdtl.Visible = false;
            grdLogDetail.Visible = false;
            lblWorkFlowApproveLog.Visible = false;
            grdApprovalAuditDtl.Visible = false;
          
        }
        private void hideShowFieldEdit()
        {
            int profileId = Convert.ToInt32(Session["ProfileId"]);
            BindVendorBankDetail(Convert.ToInt32(Session["BankDetailId"]));

            DrpVendorCode.Enabled = false;
            drpCompanyCode.Enabled = false;
            txtVendorName.Enabled = false;
            txtPanCardNo.Enabled = false;
            drpAssignTo.Visible = false;
            lblAssignto.Visible = false;        

            if (profileId == (int)UserProfileEnum.Vendor)
            {
                drpAssignTo.Visible = false;
                lblAssignto.Visible = false;
                lblStatus.Visible = false;
                drpStatus.Visible = false;

                drpSubVertical.Visible = false;
                drpVertical.Visible = false;
                lblVertical.Visible = false;
                lblSubVertical.Visible = false;
            }
            else
            {
                lblStatus.Visible = true;
                drpStatus.Visible = true;
                drpSubVertical.Visible = true;
                drpVertical.Visible = true;
                lblVertical.Visible = true;
                lblSubVertical.Visible = true;
            }

            int workStatusId = 0;
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["WorkFlowStatusId"])))
                workStatusId = Convert.ToInt32(ViewState["WorkFlowStatusId"]);
            if (workStatusId == (int)WorkFlowStatusEnum.ApprovedByCB)
            {
                chkDocReceived.Checked = false;
                receiveDate.SelectedDate = null;
                chkDocSent.Checked = false;
                dispatchDate.SelectedDate = null;

            }

            if (workStatusId == (int)WorkFlowStatusEnum.ApprovedByFASSC)
            {
                chkDocReceived.Visible = true;
                receiveDate.Visible = true;
               
            }
     
            if (Convert.ToInt32(ViewState["CreatedUser"]) == Convert.ToInt32(HttpContext.Current.Session["UserId"]))
             {
                lbldocSent.Visible = true;
                chkDocSent.Visible = true;
                lblDispatchDate.Visible = true;
                dispatchDate.Visible = true;
                chkDocReceived.Visible = false;
                receiveDate.Visible = false;
            }
   


            if ((workStatusId == (int)WorkFlowStatusEnum.RejectedByValidator || workStatusId == (int)WorkFlowStatusEnum.RejectedByVerticalController
            || workStatusId == (int)WorkFlowStatusEnum.RejectedByGroupController || workStatusId == (int)WorkFlowStatusEnum.RejectedByTreasury
            || workStatusId == (int)WorkFlowStatusEnum.RejectedByManagementAssurance || workStatusId == (int)WorkFlowStatusEnum.RejectedByFASSC
            || workStatusId == (int)WorkFlowStatusEnum.RejectedByCB 
            || workStatusId == (int)WorkFlowStatusEnum.ApprovedByCB||workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor)
            && Convert.ToInt32(ViewState["CreatedUser"])== Convert.ToInt32(HttpContext.Current.Session["UserId"]))
            {
                drpNewAccountType.Enabled = true;
                txtNewAccountNo.Enabled = true;
                txtNewIFSCCode.Enabled = true;
                txtNewBankName.Enabled = true;
                txtNewBranch.Enabled = true;
                txtNewCity.Enabled = true;
                txtNewSuzlonEmail1.Enabled = true;
                txtNewSuzlonEmail2.Enabled = true;
                txtNewVendorEmail1.Enabled = true;
                txtNewVendorEmail2.Enabled = true;
                drpSubVertical.Enabled = true;
                drpVertical.Enabled = true;
                uploadCancelChq.Enabled = true;
                uploadAccCertificate.Enabled = true;
                txtCommentBox.Enabled = true;
                ViewState["CancelCheque"] = string.Empty;
                ViewState["AccountCertificate"] = string.Empty;
                if (workStatusId == (int)WorkFlowStatusEnum.ApprovedByCB)
                {
                    chkDocSent.Checked = false;
                    dispatchDate.SelectedDate = null;
                }

                lblStatus.Visible = false;
                drpStatus.Visible = false;
            }
             else if ( (workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByValidator || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVerticalController||
                workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByGroupController || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByTreasury ||
                workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByManagementAssurance || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByFASSC
                || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByCB) 
                && Convert.ToInt32(ViewState["CreatedUser"]) != Convert.ToInt32(HttpContext.Current.Session["UserId"]))
                {             
                    drpNewAccountType.Enabled = true;
                    txtNewAccountNo.Enabled = true;
                    txtNewIFSCCode.Enabled = true;
                    txtNewBankName.Enabled = true;
                    txtNewBranch.Enabled = true;
                    txtNewCity.Enabled = true;
                    txtNewSuzlonEmail1.Enabled = true;
                    txtNewSuzlonEmail2.Enabled = true;
                    txtNewVendorEmail1.Enabled = true;
                    txtNewVendorEmail2.Enabled = true;
                    drpSubVertical.Enabled = true;
                    drpVertical.Enabled = true;
                    uploadCancelChq.Enabled = true;
                    uploadAccCertificate.Enabled = true;
                    txtCommentBox.Enabled = true;
                    ViewState["CancelCheque"] = string.Empty;
                    ViewState["AccountCertificate"] = string.Empty;
                
            }
            else
            {
                drpNewAccountType.Enabled = false;
                txtNewAccountNo.Enabled = false;
                txtNewIFSCCode.Enabled = false;
                txtNewBankName.Enabled = false;
                txtNewBranch.Enabled = false;
                txtNewCity.Enabled = false;
                txtNewSuzlonEmail1.Enabled = false;
                txtNewSuzlonEmail2.Enabled = false;
                txtNewVendorEmail1.Enabled = false;
                txtNewVendorEmail2.Enabled = false;
             
                uploadCancelChq.Enabled = false;
                uploadAccCertificate.Enabled = false;
                txtCommentBox.Enabled = false;
               int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                if (Convert.ToInt32(ViewState["CreatedUser"]) == userId)
                    drpStatus.Enabled = false;
                else
                    drpStatus.Enabled = true;

                if (profileId == (int)UserProfileEnum.Validator)
                {
                    drpSubVertical.Enabled = true;
                    drpVertical.Enabled = true;
                }
                else
                {
                    drpSubVertical.Enabled = false;
                    drpVertical.Enabled = false;
                }


            }

            BindAssignTo(profileId);


            if (profileId != (int)UserProfileEnum.Vendor && Convert.ToInt32(ViewState["CreatedUser"]) == Convert.ToInt32(HttpContext.Current.Session["UserId"]))
            {
                lblLogdtl.Visible = true;
                grdLogDetail.Visible = true;
                bindLogDetails();
                grdLogDetail.DataBind();               
            }
            else
            {
                lblLogdtl.Visible = false;
                grdLogDetail.Visible = false;
               
            }

            if (profileId != (int)UserProfileEnum.Vendor)
            {
                lblLogdtl.Visible = true;
                grdLogDetail.Visible = true;
                bindLogDetails();
                grdLogDetail.DataBind();

                lblWorkFlowApproveLog.Visible = true;
                grdApprovalAuditDtl.Visible = true;
                bindApprovalLogDetails();
                grdApprovalAuditDtl.DataBind();
            }
            else
            {
                lblWorkFlowApproveLog.Visible = false;
                grdApprovalAuditDtl.Visible = false;
            }



        }
        private void bindLogDetails()
        {
            try
            {
                string getVendorBankDetailLog = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_WORKFLOWLOG, Convert.ToString(Session["BankDetailId"])), string.Empty);
                if (!string.IsNullOrEmpty(getVendorBankDetailLog))
                {
                    List<usp_GetBankDetailWorkFlowLog_Result> lstVendorBankDetailbyAcNo = JsonConvert.DeserializeObject<List<usp_GetBankDetailWorkFlowLog_Result>>(getVendorBankDetailLog);
                    grdLogDetail.DataSource = lstVendorBankDetailbyAcNo;
                }
                else
                {
                    grdLogDetail.DataSource = new DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private void bindApprovalLogDetails()
        {
            try
            {
                string getVendorBankDetailLog = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_WORKFLOWAPPROVELOG, Convert.ToString(Session["BankDetailId"])), string.Empty);
                if (!string.IsNullOrEmpty(getVendorBankDetailLog))
                {
                    List<usp_GetBankWorkFlowApprovalLog_Result> lstgetVendorBankDetailLog = JsonConvert.DeserializeObject<List<usp_GetBankWorkFlowApprovalLog_Result>>(getVendorBankDetailLog);
                    grdApprovalAuditDtl.DataSource = lstgetVendorBankDetailLog;
                }
                else
                {
                    grdApprovalAuditDtl.DataSource = new DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }


        private void bindDropdow()
        {
            try
            { 
            string vendorCode = string.Empty;
            string drpname = "workflow-status,account-type,subvertical";
            string commonDrpValues = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
            if (commonDrpValues == Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            }
            else
            { 
            DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(commonDrpValues);
            drpStatus.DataTextField = "Name";
            drpStatus.DataValueField = "Id";
            drpStatus.DataSource = ddValues.WorkFlowStatus;
            drpStatus.DataBind();
            drpStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Status", String.Empty));
            drpStatus.SelectedIndex = 0;

            drpSubVertical.DataTextField = "Name";
            drpSubVertical.DataValueField = "Id";
            drpSubVertical.DataSource = ddValues.SubVertical;
            drpSubVertical.DataBind();        

            drpNewAccountType.DataTextField = "Name";
            drpNewAccountType.DataValueField = "Id";
            drpNewAccountType.DataSource = ddValues.AccountType;
            drpNewAccountType.DataBind();
            drpNewAccountType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Account Type", String.Empty));
            drpNewAccountType.SelectedIndex = 0;
                             
            BindAssignTo(Convert.ToInt32(Session["ProfileId"]));
            }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private void GetVendorCode()
        {
            try
            { 
            string result = string.Empty;
            int userId = 0;
           int profileId = Convert.ToInt32(Session["ProfileId"]);    

            if (profileId == (int)UserProfileEnum.Vendor)
                userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

                if (Convert.ToString(ViewState["CreatedUser"]) != "")
                {
                    userId = 0;
                }

                result = commonFunctions.RestServiceCall(string.Format(Constants.GET_VENDOR_CODE_WORKFLOW, userId), string.Empty);

            if (result == Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            }
            else
            {
                List<GetVendorCodeForWorkflow_Result> lstVendorCode = JsonConvert.DeserializeObject<List<GetVendorCodeForWorkflow_Result>>(result);
                    DrpVendorCode.EmptyMessage = "Select Vendor Code";
                DrpVendorCode.DataTextField = "Name";              
                DrpVendorCode.DataValueField = "VendorCode";
                DrpVendorCode.DataSource = lstVendorCode;
                DrpVendorCode.DataBind();
                //DrpVendorCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Vendor Code", string.Empty));
                //DrpVendorCode.SelectedIndex = 0;
            }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private void BindAssignTo(int ProfileId)
        {
            try
            {
            //int profileId = Convert.ToInt32(Session["ProfileId"]);
            string result = string.Empty;           
            result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_ASSIGNED_TO_VALUES, ProfileId), string.Empty);
                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    List<Models.ListItem> lstAssignedTo = JsonConvert.DeserializeObject<List<Models.ListItem>>(result);
                    drpAssignTo.DataTextField = "Name";
                    drpAssignTo.DataValueField = "Id";
                    drpAssignTo.DataSource = lstAssignedTo;
                    drpAssignTo.DataBind();

                    if (Convert.ToBoolean(ViewState["IsNew"]) && (ProfileId >= (int)UserProfileEnum.Treasury))
                    {
                        drpAssignTo.Items.RemoveAt(4); ;//Group Controller
                        if (ProfileId > (int)UserProfileEnum.ManagementAssurance)
                            drpAssignTo.Items.RemoveAt(5); ;//Management Assurance
                        if (ProfileId > (int)UserProfileEnum.FASSC)
                            lstAssignedTo.RemoveAt(5);//FASSC
                    }

                    if ((!Convert.ToBoolean(ViewState["IsCreatedByVendor"])) && ProfileId >= (int)UserProfileEnum.VerticalController)
                    {
                        drpAssignTo.Items.RemoveAt(2);//Remove Validator for Suzlon Employee
                        drpAssignTo.Items[1].Text = "Intiator";//Update Text if created by Suzlon Employee
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void bindVenodrBankDetaill(string VendorCode, string companyCode, string AccountNo)
        {
            try
            {
                string getVendorBankDetailbyAcNo = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_BYIFSC_ACCNO, VendorCode, companyCode, AccountNo), string.Empty);
                if (!string.IsNullOrEmpty(getVendorBankDetailbyAcNo))
                {
                    //List<SameBankDtlVendor> lstVendorBankDetailbyAcNo = JsonConvert.DeserializeObject<List<SameBankDtlVendor>>(getVendorBankDetailbyAcNo);
                    List<getExistingVendorDetailByAccountNo> lstVendorBankDetailbyAcNo= JsonConvert.DeserializeObject<List<getExistingVendorDetailByAccountNo>>(getVendorBankDetailbyAcNo);
                    grdVendorBankDtl.DataSource = lstVendorBankDetailbyAcNo;
                }
                else
                {
                    grdVendorBankDtl.DataSource = new DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        private void BindVendorBankDetail(int BankDetailId)
        {
            try
            { 
            string resultBankDetail = commonFunctions.RestServiceCall(String.Format(Constants.VENDOR_BANK_GETDETAIL, BankDetailId), string.Empty);

            if (resultBankDetail == Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            }
            else
            {
                BankDetailModel bankdetail = JsonConvert.DeserializeObject<BankDetailModel>(resultBankDetail);

                if (bankdetail != null)
                {
                    ViewState["IsCreatedByVendor"] = bankdetail.IsCeatedByVendor;
                    ViewState["CreatedUser"] = bankdetail.CreatedBy;
                    ViewState["WorkFlowStatusId"] = bankdetail.WorkFlowStatusId;
                    ViewState["IsNew"] = bankdetail.IsNew;

                        if (bankdetail.IsNew)
                            lblRequestType.Text = "New Request";
                        else
                        {
                            lblRequestType.Text = "Change Request";
                            lblRequestType.CssClass = "text-danger";
                            
                        }


                        // GetVendorCode();
                        DrpVendorCode.Items.Add(new RadComboBoxItem(bankdetail.VendorCode + "-" + bankdetail.VendorName + "-" + bankdetail.City,bankdetail.VendorCode));
                        DrpVendorCode.SelectedIndex = 0;
                        DrpVendorCode.SelectedValue = Convert.ToString(bankdetail.VendorCode);
                    //bindCompanyCode(DrpVendorCode.SelectedItem.Text);
                    bindCompanyCode(Convert.ToString(DrpVendorCode.SelectedValue));
                    drpCompanyCode.SelectedValue = Convert.ToString(bankdetail.CompanyCode);
                    txtVendorName.Text = Convert.ToString(bankdetail.VendorName);
                    txtPanCardNo.Text = Convert.ToString(bankdetail.VendorPanNo);
                    drpNewAccountType.SelectedValue = Convert.ToString(bankdetail.AccountType);
                    txtNewAccountNo.Text = Convert.ToString(bankdetail.AccountNumber);
                    txtNewIFSCCode.Text = Convert.ToString(bankdetail.IFSCCode);
                    bindVenodrBankDetaill(bankdetail.VendorCode, bankdetail.CompanyCode, Convert.ToString(bankdetail.AccountNumber));
                    grdVendorBankDtl.DataBind();
                    txtNewBankName.Text = Convert.ToString(bankdetail.BankName);
                    txtNewBranch.Text = Convert.ToString(bankdetail.BranchName);
                    txtNewBranch.Text = Convert.ToString(bankdetail.BranchName);
                    txtNewCity.Text = Convert.ToString(bankdetail.City);
                    txtNewSuzlonEmail1.Text = Convert.ToString(bankdetail.SuzlonEmailID1);
                    txtNewSuzlonEmail2.Text = Convert.ToString(bankdetail.SuzlonEmailID2);
                    txtNewVendorEmail1.Text = Convert.ToString(bankdetail.VendorEmailID1);
                    txtNewVendorEmail2.Text = Convert.ToString(bankdetail.VendorEmailID2);
                    drpSubVertical.SelectedValue = Convert.ToString(bankdetail.SubVerticalId);                  
                    GetVerticals(drpSubVertical.SelectedValue);
                    drpVertical.SelectedValue = Convert.ToString(bankdetail.VerticalId);
                    string FilePath = Constants.VENDOR_BANK_ATTACHMENT_PATH;
                        if (!string.IsNullOrEmpty(bankdetail.Attachment1))
                        {
                            attachment1.NavigateUrl  = FilePath + bankdetail.Attachment1;
                            //lblAttachment1.Visible = true;
                            attachment1.Visible = true;
                            ViewState["CancelCheque"]=Convert.ToString(bankdetail.Attachment1);
                        }
                        else
                        {
                            //lblAttachment1.Visible = false;
                            attachment1.Visible = false;
                        }

                        if (!string.IsNullOrEmpty(bankdetail.Attachment2))
                        { 
                            attachment2.NavigateUrl = FilePath + bankdetail.Attachment2;
                            //lblAttachment2.Visible = true;
                            attachment2.Visible = true;
                            ViewState["AccountCertificate"] = Convert.ToString(bankdetail.Attachment2);
                        }
                        else { 
                            //lblAttachment2.Visible = false;
                            attachment2.Visible = false;
                        }


                        if (Convert.ToBoolean(bankdetail.OriginalDocumentsSent))
                    chkDocSent.Checked = true;
                    else
                        chkDocSent.Checked = false;
                    if (bankdetail.SendDate != null)
                        dispatchDate.SelectedDate = Convert.ToDateTime(bankdetail.SendDate);

                    if (Convert.ToBoolean(bankdetail.OriginalDocumentsReceived))
                        chkDocReceived.Checked = true;
                    else
                        chkDocReceived.Checked = false;
                    if (bankdetail.ReceivedDate != null)
                        receiveDate.SelectedDate = Convert.ToDateTime(bankdetail.ReceivedDate);   
                        
                    List<BankCommentModel> BankCommentList = (List<BankCommentModel>) bankdetail.BankComments;
                    foreach (var comment in BankCommentList)
                    {
                        CommentDtl cmtdtl = new CommentDtl();
                        cmtdtl.UserName = comment.CommentBy;
                        cmtdtl.Comment = comment.Comment;                            
                        CommentLstShow.Add(cmtdtl);
                    }


                    ViewState["BankCommentShow"] = CommentLstShow;
                    StringBuilder CommentStr = new StringBuilder();
                    CommentStr = CommentStr.Append("<head> <table>");
                    foreach (var Comlst in CommentLstShow)
                    {
                        CommentStr.Append(" <tr><td> " + Comlst.UserName + " </td><td></td></tr>");
                        CommentStr.Append("<tr><td></td><td>" + Comlst.Comment + "</td></tr>");
                    }
                    CommentStr.Append("</table></head>");
                    lblCommentDetail.Text = CommentStr.ToString();
                }

                    if (Convert.ToInt32(Session["ProfileId"]) == (int)UserProfileEnum.Validator && bankdetail.WorkFlowStatusId != 3 && bankdetail.WorkFlowStatusId !=null   )
                    {
                        BtnSave.Enabled = false;
                    }

            }

                bindVendorOldDtlFromMaster(Convert.ToString(DrpVendorCode.SelectedValue), Convert.ToString(drpCompanyCode.SelectedValue));

                string resultOldBankDetail = commonFunctions.RestServiceCall(String.Format(Constants.VENDOR_BANK_GETHISTORY, BankDetailId), string.Empty);
            if (resultOldBankDetail == Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            }
            else
            {
                BankDetail oldbankdetail = JsonConvert.DeserializeObject<BankDetail>(resultOldBankDetail);
                if (oldbankdetail != null)
                {
                    txtOldAccType.Text = (Convert.ToString(oldbankdetail.AccountType)=="SAV"?"Saving":"Current");
                    txtOldAccountNo.Text = Convert.ToString(oldbankdetail.AccountNumber);
                    txtOldIFSCCode.Text = Convert.ToString(oldbankdetail.IFSCCode);
                    txtOldBankName.Text = Convert.ToString(oldbankdetail.BankName);
                    txtOldBranch.Text = Convert.ToString(oldbankdetail.BranchName);
                    txtOldBranch.Text = Convert.ToString(oldbankdetail.BranchName);
                    txtOldCity.Text = Convert.ToString(oldbankdetail.City);
                    txtOldSuzlonEmail1.Text = Convert.ToString(oldbankdetail.SuzlonEmailID1);
                    txtOldSuzlonEmail2.Text = Convert.ToString(oldbankdetail.SuzlonEmailID2);
                    txtOldVendorEmail1.Text = Convert.ToString(oldbankdetail.VendorEmailID1);
                    txtOldVendorEmail2.Text = Convert.ToString(oldbankdetail.VendorEmailID2);
                }
            }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private bool suzlonMailValidate()
        {
            try
            {
                string isExistSuzlonMail = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_SUZLON_MAILEXIST, txtNewSuzlonEmail1.Text, txtNewSuzlonEmail2.Text), string.Empty);
                Dictionary<string, bool> fieldStatus = JsonConvert.DeserializeObject<Dictionary<string, bool>>(isExistSuzlonMail);
                string msg = string.Empty;
                if (fieldStatus.ContainsKey("suzlonmailid1") && fieldStatus["suzlonmailid1"] == false)
                {
                    msg = Constants.VENDOR_BANK_SUZLON_MAIL1EXISTMSG;
                }
                if (fieldStatus.ContainsKey("suzlonmailid2") && fieldStatus["suzlonmailid2"] == false)
                {
                    msg = msg + Constants.VENDOR_BANK_SUZLON_MAIL2EXISTMSG;
                }
                if (!string.IsNullOrEmpty(msg))
                {
                    radMessage.Title = "Alert";
                    radMessage.Show(msg);
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }
        
        private void uploadAttachment()
        {
            try
            { 
            String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
            foreach (UploadedFile file in uploadCancelChq.UploadedFiles)
            {
                string path = Server.MapPath(Constants.VENDOR_BANK_ATTACHMENT_PATH);
                AttachmentChq = file.GetNameWithoutExtension() + timeStamp + file.GetExtension();
               file.SaveAs(path + AttachmentChq, true);
            }
            foreach (UploadedFile file in uploadAccCertificate.UploadedFiles)
            {
                string path = Server.MapPath(Constants.VENDOR_BANK_ATTACHMENT_PATH);
                AttachmentCert = file.GetNameWithoutExtension() + timeStamp + file.GetExtension();
                file.SaveAs(path + AttachmentCert, true);
            }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        public List<BankCommentModel> bankComment()
        {

            if (Convert.ToString(ViewState["BankComment"]) != "")
            {
                CommentLstAdd = (List<CommentDtl>)ViewState["BankComment"];
            }

            List<BankCommentModel> bankCommentList = new List<BankCommentModel>();
            BankCommentModel bankComment = new BankCommentModel();

            foreach (var cmmt in CommentLstAdd)
            {
                bankComment = new BankCommentModel()
                {
                    Comment = cmmt.Comment
                };
                bankCommentList.Add(bankComment);
            }

            return bankCommentList;
        }

        private int? SetStatusByAssignedTo(int? assignedTo)
        {
            switch (assignedTo)
            {
                case ((int)UserProfileEnum.Vendor):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByVendor;
                    }
                case ((int)UserProfileEnum.Validator):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByValidator;
                    }
                case ((int)UserProfileEnum.VerticalController):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByVerticalController;
                    }
                case ((int)UserProfileEnum.GroupController):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByGroupController;
                    }
                case ((int)UserProfileEnum.Treasury):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByTreasury;
                    }
                case ((int)UserProfileEnum.ManagementAssurance):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByManagementAssurance;
                    }
                case ((int)UserProfileEnum.FASSC):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByFASSC;
                    }
                case ((int)UserProfileEnum.CB):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByCB;
                    }

                default: return null;
            }
        }

        private bool checkIFSCACNO(int BankDetailId,string IFSCCode, string AccNo)
        {
            try
            {
               
                BankDetailModel bankDetail = new BankDetailModel()
                {
                    BankDetailId = Convert.ToInt32(Session["BankDetailId"]),
                    IFSCCode = IFSCCode,
                    AccountNumber = AccNo,
                    VendorCode = Convert.ToString(DrpVendorCode.SelectedValue),
                    CompanyCode = Convert.ToString(drpCompanyCode.SelectedValue),
                    SuzlonEmailID1 = Convert.ToString(txtNewSuzlonEmail1.Text),
                    SuzlonEmailID2 = Convert.ToString(txtNewSuzlonEmail2.Text),
                    VendorEmailID1 = Convert.ToString(txtNewVendorEmail1.Text),
                    VendorEmailID2 = Convert.ToString(txtNewVendorEmail2.Text),
                    AccountType = Convert.ToString(drpNewAccountType.SelectedValue),
                    BankName= Convert.ToString(txtNewBankName.Text),
                    BranchName = Convert.ToString(txtNewBranch.Text).Trim(),
                    City = Convert.ToString(txtNewCity.Text).Trim(),
                };
                string jsonInputParameter = JsonConvert.SerializeObject(bankDetail);
                string serviceResult = string.Empty;

                string result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_CHKIFSC_ACNO_EXIST_MAIL), Crypto.Instance.Encrypt(jsonInputParameter));
                bool isExist = Convert.ToBoolean(result);
                return isExist;
            }
            catch (Exception ex)
            {                
                CommonFunctions.WriteErrorLog(ex);
                return true;
            }
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Convert.ToString(ViewState["IsExist"]) == "No")
                {
                    int profileId = Convert.ToInt32(Session["ProfileId"]);
                    int workStatusId = 0;
                    string CancelCheque = Convert.ToString(ViewState["CancelCheque"]);
                    string AccountCertificate = Convert.ToString(ViewState["AccountCertificate"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(ViewState["WorkFlowStatusId"])))
                        workStatusId = Convert.ToInt32(ViewState["WorkFlowStatusId"]);
                    
                        if (checkIFSCACNO(Convert.ToInt32(Session["BankDetailId"]), Convert.ToString(txtNewIFSCCode.Text), Convert.ToString(txtNewAccountNo.Text)))
                        {
                            radMessage.Title = "Alert";
                            radMessage.Show(Constants.VENDOR_BANK_IFSC_ACNO_EXIST);
                            return;
                        }



                    if (workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByValidator || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVerticalController ||
                        workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByGroupController || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByTreasury ||
                        workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByManagementAssurance || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByFASSC
                        || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByCB ||
                        workStatusId == (int)WorkFlowStatusEnum.RejectedByValidator || workStatusId == (int)WorkFlowStatusEnum.RejectedByVerticalController
                        || workStatusId == (int)WorkFlowStatusEnum.RejectedByGroupController || workStatusId == (int)WorkFlowStatusEnum.RejectedByTreasury
                        || workStatusId == (int)WorkFlowStatusEnum.RejectedByManagementAssurance || workStatusId == (int)WorkFlowStatusEnum.RejectedByFASSC
                        || workStatusId == (int)WorkFlowStatusEnum.RejectedByCB ||
                        workStatusId == (int)WorkFlowStatusEnum.ApprovedByCB || Convert.ToString(Session["BankDetailId"]) == "0")
                    {
                        string result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_CHKIFSC_ACNO_EXIST, Convert.ToInt32(Session["BankDetailId"]),
                            Convert.ToString(txtNewIFSCCode.Text), Convert.ToString(txtNewAccountNo.Text), Convert.ToString(DrpVendorCode.SelectedValue),
                            Convert.ToString(drpCompanyCode.SelectedValue), Convert.ToString(txtNewSuzlonEmail1.Text), Convert.ToString(txtNewSuzlonEmail2.Text)
                            , Convert.ToString(txtNewVendorEmail1.Text), Convert.ToString(txtNewVendorEmail2.Text)), string.Empty);
                        if (result == Constants.REST_CALL_FAILURE)
                        {
                            radMessage.Title = "Alert";
                            radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                        }
                        else
                        {
                            bool isExist = JsonConvert.DeserializeObject<bool>(result);
                            if (!isExist && string.IsNullOrEmpty(CancelCheque) && string.IsNullOrEmpty(AccountCertificate))
                            {
                                radMessage.Title = "Alert";
                                radMessage.Show(Constants.VENDOR_BANK_ATTACHMENT_MISSING);
                                return;
                            }
                        }
                    }
                    if (chkDocReceived.Checked == true && receiveDate.SelectedDate==null)
                    {
                        radMessage.Title = "Alert";
                        radMessage.Show(Constants.VENDOR_BANK_DOCRCEIVEDATE_NULL);
                        return;
                    }
                    if (chkDocSent.Checked == true && dispatchDate.SelectedDate == null)
                    {
                        radMessage.Title = "Alert";
                        radMessage.Show(Constants.VENDOR_BANK_DOCSENDDATE_NULL);
                        return;
                    }

                    if (drpStatus.SelectedValue != "")
                    {
                        if (Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.NeedCorrection && drpAssignTo.SelectedValue == "")
                        {
                            radMessage.Title = "Alert";
                            radMessage.Show(Constants.VENDOR_BANK_ASSIGNTO_MSG);
                            return;
                        }


                    }
                    if (profileId != (int)UserProfileEnum.Vendor && drpSubVertical.SelectedValue == "")
                    {
                        radMessage.Title = "Alert";
                        radMessage.Show(Constants.VENDOR_BANK_SUBVERTICAL_MSG);
                        return;
                    }


                    if (suzlonMailValidate())
                    {
                       // uploadAttachment();
                        List<BankCommentModel> bankCommentList = bankComment();
                        if (drpStatus.SelectedValue != "")
                        {
                            if ((Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.Approved || Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.NeedCorrection || Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.Rejected) && bankCommentList.Count == 0)
                            {
                                radMessage.Title = "Alert";
                                radMessage.Show(Constants.VENDOR_BANK_COMMENT_MSG);
                                return;
                            }
                        }



                        bool docRecived = chkDocReceived.Checked == true ? true : false;
                        bool docSent = chkDocSent.Checked == true ? true : false;


                        DateTime? DispatchDate = null;
                        DateTime? ReceiveDate = null;

                        if (dispatchDate.SelectedDate != null)
                            DispatchDate = Convert.ToDateTime(dispatchDate.SelectedDate);
                        if (receiveDate.SelectedDate != null)
                            ReceiveDate = Convert.ToDateTime(receiveDate.SelectedDate);

                        if (DispatchDate != null && ReceiveDate != null && DispatchDate> ReceiveDate)
                        {
                            radMessage.Title = "Alert";
                            radMessage.Show(Constants.VENDOR_BANK_DATE_VALIDATION);
                            return;
                        }

                        try
                        {
                            BankDetailModel bankDetail = new BankDetailModel()
                            {
                                BankDetailId=Convert.ToInt32(Session["BankDetailId"]),
                                VendorCode = Convert.ToString(DrpVendorCode.SelectedValue),
                                CompanyCode = Convert.ToString(drpCompanyCode.SelectedValue),
                                VendorPanNo = Convert.ToString(txtPanCardNo.Text),
                                VendorName = Convert.ToString(txtVendorName.Text),
                                AccountType = Convert.ToString(drpNewAccountType.SelectedValue),
                                AccountNumber = Convert.ToString(txtNewAccountNo.Text),
                                IFSCCode = Convert.ToString(txtNewIFSCCode.Text),
                                BankName = Convert.ToString(txtNewBankName.Text),
                                BranchName = Convert.ToString(txtNewBranch.Text),
                                City = Convert.ToString(txtNewCity.Text),
                                SuzlonEmailID1 = Convert.ToString(txtNewSuzlonEmail1.Text),
                                SuzlonEmailID2 = Convert.ToString(txtNewSuzlonEmail2.Text),
                                VendorEmailID1 = Convert.ToString(txtNewVendorEmail1.Text),
                                VendorEmailID2 = Convert.ToString(txtNewVendorEmail2.Text), 
                                BankComments = bankCommentList,
                                OriginalDocumentsReceived = docRecived,
                                OriginalDocumentsSent = docSent,
                                RequestType=Convert.ToString(ViewState["RequestType"])
                            };
                            if (DispatchDate.HasValue)
                                bankDetail.SendDate = DispatchDate;
                            if (ReceiveDate.HasValue)
                                bankDetail.ReceivedDate = ReceiveDate;                         
                            if (drpAssignTo.SelectedValue != "")
                                bankDetail.AssignedTo= Convert.ToInt32(drpAssignTo.SelectedValue);
                            if (drpSubVertical.SelectedValue != "")
                                bankDetail.SubVerticalId = Convert.ToInt32(drpSubVertical.SelectedValue);
                            if (drpVertical.SelectedValue != "")
                                bankDetail.VerticalId = Convert.ToInt32(drpVertical.SelectedValue);
                          
                            if(!string.IsNullOrEmpty(CancelCheque))
                                bankDetail.Attachment1 = CancelCheque;
                            if (!string.IsNullOrEmpty(AccountCertificate))
                                bankDetail.Attachment2 = AccountCertificate;
                          

                           if (Convert.ToInt32(Session["BankDetailId"]) == 0 && profileId != (int)UserProfileEnum.Vendor)
                            {
                                bankDetail.WorkFlowStatusId = 1;
                            }
                            if (drpStatus.SelectedValue != "")
                                bankDetail.Status = Convert.ToInt32(drpStatus.SelectedValue);

                            //Work Flow status logic
                            if (Convert.ToInt32(Session["BankDetailId"]) != 0)
                            {
                                if ((workStatusId == (int)WorkFlowStatusEnum.ApprovedByCB || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor
                                 || workStatusId == (int)WorkFlowStatusEnum.RejectedByValidator|| workStatusId == (int)WorkFlowStatusEnum.RejectedByVerticalController
                                || workStatusId == (int)WorkFlowStatusEnum.RejectedByGroupController || workStatusId == (int)WorkFlowStatusEnum.RejectedByTreasury
                                || workStatusId == (int)WorkFlowStatusEnum.RejectedByManagementAssurance || workStatusId == (int)WorkFlowStatusEnum.RejectedByFASSC
                                || workStatusId == (int)WorkFlowStatusEnum.RejectedByCB))
                                {
                                    if (profileId != (int)UserProfileEnum.Vendor)
                                        bankDetail.WorkFlowStatusId = 1;
                                    else
                                        bankDetail.WorkFlowStatusId = null;
                                }
                                //else if ((workStatusId == (int)WorkFlowStatusEnum.ApprovedByCB|| workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor
                                //|| workStatusId == (int)WorkFlowStatusEnum.RejectedByValidator || workStatusId == (int)WorkFlowStatusEnum.RejectedByVerticalController
                                //|| workStatusId == (int)WorkFlowStatusEnum.RejectedByGroupController || workStatusId == (int)WorkFlowStatusEnum.RejectedByTreasury
                                //|| workStatusId == (int)WorkFlowStatusEnum.RejectedByManagementAssurance || workStatusId == (int)WorkFlowStatusEnum.RejectedByFASSC
                                //|| workStatusId == (int)WorkFlowStatusEnum.RejectedByCB) && profileId == (int)UserProfileEnum.Vendor)
                                //{
                                //    bankDetail.WorkFlowStatusId = null;
                                //}    
                                else
                                {

                                    if (Convert.ToString(drpStatus.SelectedValue) != "")
                                    {
                                        if (Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.NeedCorrection)
                                        {
                                            bankDetail.WorkFlowStatusId = SetStatusByAssignedTo(Convert.ToInt32(drpAssignTo.SelectedValue));
                                        }
                                        else if (Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.Approved)
                                        {
                                            if (Convert.ToBoolean(ViewState["IsNew"]) && (workStatusId == (int)WorkFlowStatusEnum.ApprovedByVerticalController || workStatusId == (int)WorkFlowStatusEnum.ApprovedByTreasury))
                                            {
                                                if (workStatusId == (int)WorkFlowStatusEnum.ApprovedByVerticalController)
                                                    bankDetail.WorkFlowStatusId = workStatusId + 6; // Set Approved By Treasury
                                                else
                                                    bankDetail.WorkFlowStatusId = workStatusId + 9; // Set Approved by C&B
                                            }
                                            else
                                            {
                                                if (workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByValidator || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVerticalController ||
                                                workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByGroupController || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByTreasury ||
                                                workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByManagementAssurance || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByFASSC
                                                || workStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByCB)
                                                {
                                                    bankDetail.WorkFlowStatusId = workStatusId - 2;
                                                }
                                                else
                                                    if (workStatusId == 0)
                                                    bankDetail.WorkFlowStatusId = 1;
                                                else
                                                    bankDetail.WorkFlowStatusId = workStatusId + 3;
                                            }
                                        }
                                        else if (Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.Rejected)
                                        {
                                            if (Convert.ToBoolean(ViewState["IsNew"]) && (workStatusId == (int)WorkFlowStatusEnum.ApprovedByVerticalController || workStatusId == (int)WorkFlowStatusEnum.ApprovedByTreasury))
                                            {
                                                if (workStatusId == (int)WorkFlowStatusEnum.ApprovedByVerticalController)
                                                    bankDetail.WorkFlowStatusId = workStatusId + 7;// Set Rejected by Treasury
                                                else
                                                    bankDetail.WorkFlowStatusId = workStatusId + 10; // Set Rejected by C&B
                                            }
                                            else
                                            {
                                                bankDetail.WorkFlowStatusId = workStatusId + 4;
                                            }
                                        }
                                    }
                                    else
                                        if(workStatusId==0)
                                        bankDetail.WorkFlowStatusId = null;
                                    else
                                        bankDetail.WorkFlowStatusId = workStatusId;
                                }
                             }


                            string jsonInputParameter = JsonConvert.SerializeObject(bankDetail);
                            string serviceResult = string.Empty;

                            radMessage.Title = "Alert";
                            serviceResult = commonFunctions.RestServiceCall(string.Format(Constants.CHECK_BANK_VENDOR_COMPANY_CODE_BLOCK, bankDetail.VendorCode, bankDetail.CompanyCode), Crypto.Instance.Encrypt(jsonInputParameter));
                            if (serviceResult == Constants.REST_CALL_FAILURE)
                            {
                                radMessage.Title = "Alert";
                                radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                            }
                            else
                            {
                                serviceResult = JsonConvert.DeserializeObject<string>(serviceResult);
                                if (serviceResult == Constants.SUCCESS)
                                {
                                    if (Convert.ToInt32(Session["BankDetailId"]) == 0)
                                        serviceResult = commonFunctions.RestServiceCall(Constants.VENDOR_BANK_DETAIL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                                    else
                                        serviceResult = commonFunctions.RestServiceCall(Constants.VENDOR_BANK_DETAIL_UPDATE, Crypto.Instance.Encrypt(jsonInputParameter));

                                    if (string.Compare(serviceResult, Constants.REST_CALL_FAILURE, true) == 0)
                                    {
                                        radMessage.Title = "Alert";
                                        radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                                    }
                                    else
                                    {
                                        serviceResult = JsonConvert.DeserializeObject<string>(serviceResult);
                                        //if (string.Compare(serviceResult, Constants.SAP_ERROR, true) == 0)                                    
                                        //{
                                        //    radMessage.Title = "Alert";
                                        //    radMessage.Show(Constants.SAP_ERROR);
                                        //}
                                        //else if (string.Compare(serviceResult, Constants.ERROR, true) == 0)
                                        //{
                                        //    radMessage.Title = "Alert";
                                        //    radMessage.Show(Constants.ERROR);
                                        //}
                                       if (string.Compare(serviceResult, Constants.SUCCESS, true) == 0)
                                        {
                                            radMessage.Title = "Sucesss";
                                            radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                                            if (Session["PreviousPage"] != null && (int)Session["PreviousPage"] == (int)PageIndex.BankValidator)
                                                Response.Redirect("BankValidator.aspx");
                                            else
                                                Response.Redirect("BankDetailList.aspx");
                                        }
                                        else
                                        {
                                            radMessage.Title = "Alert";
                                            radMessage.Show(serviceResult);
                                        }
                                    }
                                }
                                else
                                    radMessage.Show(serviceResult);
                            }
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);

                        }
                    }
                }
                else
                {
                    radMessage.Title = "Alert";
                    radMessage.Show(Constants.VENDOR_BANK_DETAIL_EXISTMSG);
                }

            }
        }

        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            try
            {

                if (Convert.ToString(txtCommentBox.Text) != "")
                {
                    if (Convert.ToString(ViewState["BankCommentShow"])!= "")
                    {
                        CommentLstShow = (List<CommentDtl>)ViewState["BankCommentShow"];
                    }

                    if (Convert.ToString(ViewState["BankComment"]) != "")
                    {
                        CommentLstAdd = (List<CommentDtl>)ViewState["BankComment"];
                    }

                    CommentDtl cmdtl = new CommentDtl();          
                    cmdtl.UserName = Convert.ToString(Session["LoginUserName"]);
                    cmdtl.Comment = Convert.ToString(txtCommentBox.Text);
                    cmdtl.CreateUserId = Convert.ToInt32(Session["UserId"]);   
                    
                                      
                    CommentLstAdd.Add(cmdtl);
                    ViewState["BankComment"] = CommentLstAdd;
                    CommentLstShow.Add(cmdtl);
                    ViewState["BankCommentShow"] = CommentLstShow;



                    StringBuilder CommentStr = new StringBuilder();
                    CommentStr = CommentStr.Append("<head> <table>");
                    foreach (var Comlst in CommentLstShow)
                    {
                        CommentStr.Append(" <tr><td> " + Comlst.UserName + " </td><td></td></tr>");
                        CommentStr.Append("<tr><td></td><td>" + Comlst.Comment + "</td></tr>");
                    }
                    CommentStr.Append("</table></head>");
                    lblCommentDetail.Text = CommentStr.ToString();
                    txtCommentBox.Text = string.Empty;

                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        //protected void DrpVendorCode_SelectedIndexChanged(object sender, EventArgs e)
        protected void DrpVendorCode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string result = string.Empty;
            txtVendorName.Text = string.Empty;
            txtPanCardNo.Text = string.Empty;
            if (DrpVendorCode.SelectedValue != string.Empty)
            {
                bindCompanyCode(Convert.ToString(DrpVendorCode.SelectedValue));              
            }
            else
            {

                drpCompanyCode.DataSource = null;
                drpCompanyCode.DataBind();

                //drpCompanyCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Company Code", string.Empty));
                //drpCompanyCode.SelectedIndex = 0;
            }
        }
        private void bindCompanyCode(string vendorCode)
        {
            try
            { 
            string result = string.Empty;
            result = commonFunctions.RestServiceCall(string.Format(Constants.GET_COMPANY_CODE_BY_VENDOR_CODE, vendorCode), string.Empty);

                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    List<Models.ListItem> lstCompanyCode = JsonConvert.DeserializeObject<List<Models.ListItem>>(result);
                    //drpCompanyCode.EmptyMessage = "Select Company Code";
                    drpCompanyCode.DataTextField = "Name";
                    drpCompanyCode.DataValueField = "Id";
                    drpCompanyCode.Text = "";
                    drpCompanyCode.DataSource = lstCompanyCode;
                    drpCompanyCode.DataBind();   
                             
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        // protected void drpCompanyCode_SelectedIndexChanged(object sender, EventArgs e)

      private void  bindVendorOldDtlFromMaster(string vendorCode, string compCode)
        {
            try
            {
                string result = commonFunctions.RestServiceCall(string.Format(Constants.EXISTVENDOR_BANK_DETAIL, vendorCode, compCode), string.Empty);
                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    BankDetail oldbankdetail = JsonConvert.DeserializeObject<BankDetail>(result);             
                    if (oldbankdetail != null)
                    {
                        txtOldAccType.Text = Convert.ToString(oldbankdetail.AccountType);
                        txtOldAccountNo.Text = Convert.ToString(oldbankdetail.AccountNumber);
                        txtOldIFSCCode.Text = Convert.ToString(oldbankdetail.IFSCCode);
                        txtOldBankName.Text = Convert.ToString(oldbankdetail.BankName);
                        txtOldBranch.Text = Convert.ToString(oldbankdetail.BranchName);
                        txtOldBranch.Text = Convert.ToString(oldbankdetail.BranchName);
                        txtOldCity.Text = Convert.ToString(oldbankdetail.City);
                        txtOldSuzlonEmail1.Text = Convert.ToString(oldbankdetail.SuzlonEmailID1);
                        txtOldSuzlonEmail2.Text = Convert.ToString(oldbankdetail.SuzlonEmailID2);
                        txtOldVendorEmail1.Text = Convert.ToString(oldbankdetail.VendorEmailID1);
                        txtOldVendorEmail2.Text = Convert.ToString(oldbankdetail.VendorEmailID2);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }
        protected void drpCompanyCode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            { 
            string result = string.Empty;
            if (drpCompanyCode.SelectedValue != string.Empty)
            {
                
                string isExistDetail = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_DETAIL_EXIST, Convert.ToString(DrpVendorCode.SelectedValue),Convert.ToString(drpCompanyCode.SelectedValue)), string.Empty);
                bool isExistDtl = Convert.ToBoolean(isExistDetail);
                    if (isExistDtl)
                    {
                        Session["BankDetailId"] = 0;
                        ViewState["IsExist"] = "Yes";
                        radMessage.Title = "Alert";
                        radMessage.Show(Constants.VENDOR_BANK_DETAIL_EXISTMSG);

                    }
                    else
                    {
                        ViewState["IsExist"] = "No";
                        bindVendorOldDtlFromMaster(Convert.ToString(DrpVendorCode.SelectedValue), Convert.ToString(drpCompanyCode.SelectedValue));

                        result = commonFunctions.RestServiceCall(string.Format(Constants.GET_VENDORDETAILS_FOR_WORKFLOW, DrpVendorCode.SelectedValue, drpCompanyCode.SelectedValue), string.Empty);

                        if (result == Constants.REST_CALL_FAILURE)
                        {
                            radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                            radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                        }
                        else
                        {
                            Models.ListItem vendorDetails = JsonConvert.DeserializeObject<Models.ListItem>(result);
                            txtVendorName.Text = vendorDetails.Id;
                            txtPanCardNo.Text = vendorDetails.Name;
                        }
                        //Code To get BankDetail 10 Aug
                        result = commonFunctions.RestServiceCall(string.Format(Constants.GETVENDOR_BANK_DETAILID, DrpVendorCode.SelectedValue, drpCompanyCode.SelectedValue), string.Empty);
                        if (result == Constants.REST_CALL_FAILURE)
                        {
                            radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                            radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                        }
                        else
                        {
                            string bankDetailId = JsonConvert.DeserializeObject<string>(result);
                            if(!string.IsNullOrEmpty(bankDetailId))
                            {
                                try
                                {
                                    Session["BankDetailId"] = Convert.ToInt32(bankDetailId);
                                    bindExisingDetail();
                                   
                                }
                                catch (Exception ex)
                                {
                                    Session["BankDetailId"] = 0;
                                }
                            }                            
                           
                        }

                    }
            }
            else
            {
                txtVendorName.Text = string.Empty;
                txtPanCardNo.Text = string.Empty;
            }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpSubVertical_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(drpSubVertical.SelectedValue))
            GetVerticals(drpSubVertical.SelectedValue);
        }
        protected void GetVerticals(string subVerticalId)
        {
            try
            { 
            var result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_GETVERTICAl_BYSUBVERTICAL, subVerticalId), string.Empty);
            if (result == Constants.REST_CALL_FAILURE)
            {
                drpVertical.DataSource = null;
                drpVertical.DataBind();
                drpVertical.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Vertical", String.Empty));
                drpVertical.SelectedIndex = 0;
            }
            else
            {                
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                drpVertical.DataTextField = "Name";
                drpVertical.DataValueField = "Id";
                drpVertical.DataSource = ddValues.Vertical;
                drpVertical.DataBind();
              
            }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }


        }

        protected void GetSubVerticals(string verticalId)
        {
            try
            { 

            var result = commonFunctions.RestServiceCall(string.Format(Constants.SUBVERTICAL_BYVERTICAL, verticalId), string.Empty);
            if (result == Constants.REST_CALL_FAILURE)
            {
                drpSubVertical.DataSource = null;
                drpSubVertical.DataBind();
                //drpSubVertical.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Sub Vertical", String.Empty));
                //drpSubVertical.SelectedIndex = 0;
            }
            else
            {
                List<Models.ListItem> lstSubVertical = JsonConvert.DeserializeObject<List<Models.ListItem>>(result);

                drpSubVertical.DataTextField = "Name";
                drpSubVertical.DataValueField = "Id";
                drpSubVertical.DataSource = lstSubVertical;
                drpSubVertical.DataBind();
                //drpSubVertical.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Sub Vertical", String.Empty));
                //drpSubVertical.SelectedIndex = 0;
            }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        protected void txtNewIFSCCode_TextChanged(object sender, EventArgs e)
        {
            try
            { 
            string result = string.Empty;
            result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_DTL_BYIFSC, txtNewIFSCCode.Text), string.Empty);

            if (result == Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            }
            else
            {

                Models.IFSCCodeMaster ifscCodeMaster = JsonConvert.DeserializeObject<Models.IFSCCodeMaster>(result);
                    if (ifscCodeMaster != null)
                    {
                        txtNewBankName.Text = Convert.ToString(ifscCodeMaster.BankName);
                        txtNewBranch.Text = Convert.ToString(ifscCodeMaster.BranchName);
                    }
                    else
                    {
                        txtNewBankName.Text = string.Empty;
                        txtNewBranch.Text = string.Empty;
                    }
                    bindVenodrBankDetaill(DrpVendorCode.SelectedValue, drpCompanyCode.SelectedValue, Convert.ToString(txtNewAccountNo.Text));
                    grdVendorBankDtl.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAuditTrail_Click(object sender, EventArgs e)
        {
            BindAuditTrail(Convert.ToInt32(Session["BankDetailId"]));
            grdBankAuditTrial.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowAuditTrail();", true);
       }

        public void BindAuditTrail(int BankDetailId)
        {
            try
            {
             string result = string.Empty;

             result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_GETAUDIT_TRAIL, BankDetailId), string.Empty);

            if (result == Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
            }
            else
            {
                List<GetBankAuditTrailDetail_Result> lstBankDetailAuditTrial = JsonConvert.DeserializeObject<List<GetBankAuditTrailDetail_Result>>(result);
                    if (lstBankDetailAuditTrial.Count > 0)
                    {
                        grdBankAuditTrial.DataSource = lstBankDetailAuditTrial;
                      
                    }
                    else
                    {
                        grdBankAuditTrial.DataSource = new DataTable();
                        
                    }
             }  

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        protected void grdBankAuditTrial_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindAuditTrail(Convert.ToInt32(Session["BankDetailId"]));
            
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            if (Session["PreviousPage"] != null && (int)Session["PreviousPage"] == (int)PageIndex.BankValidator)
                Response.Redirect("BankValidator.aspx");
            else
            {       

                Response.Redirect("BankDetailList.aspx");

            }
        }

        protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if(drpStatus.SelectedValue!="")
            {
                txtCommentBox.Enabled = true;
                if (Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.NeedCorrection)
                {
                    drpAssignTo.Visible = true;
                    lblAssignto.Visible = true;                    
                }
            }
            else
                txtCommentBox.Enabled = false;


            //if (Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.Approved)
            //{               
            //    txtCommentBox.Enabled = true;
            //}         

            //if(Convert.ToInt32(drpStatus.SelectedValue) == (int)Status.Rejected)
            //    txtCommentBox.Enabled = true;
        }

        protected void chkDocSent_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDocSent.Checked)
                dispatchDate.SelectedDate = null;
        }

        protected void chkDocReceived_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDocReceived.Checked)
                receiveDate.SelectedDate = null;
         
        }

        protected void uploadCancelChq_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                if (e.File.GetExtension().Trim().ToUpper() == ".JPEG" || e.File.GetExtension().Trim().ToUpper() == ".PNG" || e.File.GetExtension().Trim().ToUpper() == ".JPG" || e.File.GetExtension().Trim().ToUpper() == ".GIF" || e.File.GetExtension().Trim().ToUpper() == ".PDF")
                {
                    String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                    string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
                    uploadCancelChq.TargetFolder = Constants.VENDOR_BANK_ATTACHMENT_PATH;
                    e.File.SaveAs(Path.Combine(Server.MapPath(uploadCancelChq.TargetFolder), newfilename));
                    string FilePath = Path.Combine(Server.MapPath(uploadCancelChq.TargetFolder), newfilename);
                    string result = string.Empty;
                    if (e.File.GetExtension().Trim().ToUpper() == ".PDF")
                    {
                        FilePath = ReadPdf.ReadImageFromPDF.WriteImageFile(FilePath);
                        newfilename = Path.GetFileName(FilePath);
                    }
                    commonFunctions.RestServiceCallSync(string.Format(Constants.VENDOR_BANK_OCRREADER, FilePath), string.Empty, new CommonFunctions.CallbackHandler(Call_ReadImage));
                    ViewState["CancelCheque"] = newfilename;
                    // attachment1.Visible = true;
                    attachment1.Enabled = true;

                    //lblAttachment1.Text = Constants.VENDOR_BANK_ATTACHMENT_PATH + newfilename;
                    attachment1.NavigateUrl = Constants.VENDOR_BANK_ATTACHMENT_PATH + newfilename;
                    
                    
                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.USER_IMAGEVALIDATION_MSG);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        public void Call_ReadImage(string result)
        {
            try
            {
                string bankDetailResult = string.Empty;
                string BankDetail = result.Substring(1, result.Length - 2);
                string[] values = BankDetail.Split(';');
                if (values.Count() == 2)
                {
                    txtNewIFSCCode.Text = Convert.ToString(values[0]);
                    txtNewAccountNo.Text = Convert.ToString(values[1]);

                    if (!string.IsNullOrEmpty(Convert.ToString(values[0])))
                    {
                        bankDetailResult = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_DTL_BYIFSC, txtNewIFSCCode.Text), string.Empty);
                        if (bankDetailResult == Constants.REST_CALL_FAILURE)
                        {
                            radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                            radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                        }
                        else
                        {

                            Models.IFSCCodeMaster ifscCodeMaster = JsonConvert.DeserializeObject<Models.IFSCCodeMaster>(bankDetailResult);
                            if (ifscCodeMaster != null)
                            {
                                txtNewBankName.Text = Convert.ToString(ifscCodeMaster.BankName);
                                txtNewBranch.Text = Convert.ToString(ifscCodeMaster.BranchName);
                            }
                            else
                            {
                                txtNewBankName.Text = string.Empty;
                                txtNewBranch.Text = string.Empty;
                            }
                        }
                    }
                    bindVenodrBankDetaill(DrpVendorCode.SelectedValue, drpCompanyCode.SelectedValue, Convert.ToString(txtNewAccountNo.Text));
                    grdVendorBankDtl.DataBind();
                }
            }
            catch(Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void uploadAccCertificate_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                if (e.File.GetExtension().Trim().ToUpper() == ".JPEG" || e.File.GetExtension().Trim().ToUpper() == ".PNG" || e.File.GetExtension().Trim().ToUpper() == ".JPG" || e.File.GetExtension().Trim().ToUpper() == ".GIF")
                {
                    String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                    string newfilename = e.File.GetNameWithoutExtension() + timeStamp + e.File.GetExtension();
                    uploadAccCertificate.TargetFolder = Constants.VENDOR_BANK_ATTACHMENT_PATH;
                    e.File.SaveAs(Path.Combine(Server.MapPath(uploadAccCertificate.TargetFolder), newfilename));
                    ViewState["AccountCertificate"] = newfilename;
                    attachment2.NavigateUrl  = Constants.VENDOR_BANK_ATTACHMENT_PATH + newfilename;
                    attachment2.Visible = true;

                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.USER_IMAGEVALIDATION_MSG);

                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdVendorBankDtl_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindVenodrBankDetaill(DrpVendorCode.SelectedValue, drpCompanyCode.SelectedValue, Convert.ToString(txtNewAccountNo.Text));
        }

        protected void txtNewAccountNo_TextChanged(object sender, EventArgs e)
        {
            bindVenodrBankDetaill(DrpVendorCode.SelectedValue, drpCompanyCode.SelectedValue, Convert.ToString(txtNewAccountNo.Text));
            grdVendorBankDtl.DataBind();
        }

        protected void grdLogDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindLogDetails();
        }

        protected void grdApprovalAuditDtl_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindApprovalLogDetails();
        }

        //protected void txtNewBankName_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string result = string.Empty;
        //        result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_DTL_BY_BRANCH_BANK, Convert.ToString(txtNewBankName.Text),Convert.ToString(txtNewBranch.Text)), string.Empty);

        //        if (result == Constants.REST_CALL_FAILURE)
        //        {
        //            radMessage.Title = Constants.RAD_MESSAGE_TITLE;
        //            radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
        //        }
        //        else
        //        {

        //            Models.IFSCCodeMaster ifscCodeMaster = JsonConvert.DeserializeObject<Models.IFSCCodeMaster>(result);
        //            if (ifscCodeMaster != null)
        //            {
        //                txtNewBankName.Text = Convert.ToString(ifscCodeMaster.BankName);
        //                txtNewBranch.Text = Convert.ToString(ifscCodeMaster.BranchName);
        //                txtNewIFSCCode.Text= Convert.ToString(ifscCodeMaster.IFSCCode);
        //            }
        //            else
        //            {
        //              //  txtNewBankName.Text = string.Empty;
        //              //  txtNewBranch.Text = string.Empty;
        //              //  txtNewIFSCCode.Text = string.Empty;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.WriteErrorLog(ex);
        //    }
        //}

        //protected void txtNewBranch_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string result = string.Empty;
        //        result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_DTL_BY_BRANCH_BANK, Convert.ToString(txtNewBankName.Text), Convert.ToString(txtNewBranch.Text)), string.Empty);

        //        if (result == Constants.REST_CALL_FAILURE)
        //        {
        //            radMessage.Title = Constants.RAD_MESSAGE_TITLE;
        //            radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
        //        }
        //        else
        //        {

        //            Models.IFSCCodeMaster ifscCodeMaster = JsonConvert.DeserializeObject<Models.IFSCCodeMaster>(result);
        //            if (ifscCodeMaster != null)
        //            {
        //                txtNewBankName.Text = Convert.ToString(ifscCodeMaster.BankName);
        //                txtNewBranch.Text = Convert.ToString(ifscCodeMaster.BranchName);
        //                txtNewIFSCCode.Text = Convert.ToString(ifscCodeMaster.IFSCCode);
        //            }
        //            else
        //            {
        //               // txtNewBankName.Text = string.Empty;
        //                //txtNewBranch.Text = string.Empty;
        //               // txtNewIFSCCode.Text = string.Empty;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.WriteErrorLog(ex);
        //    }
        //}
    }


    [Serializable]
    public class CommentDtl
    {
        public int CreateUserId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }

    }
    [Serializable]
    public class SameBankDtlVendor
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string CompanyCode { get; set; }    
        public string CompanyName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
    }

}