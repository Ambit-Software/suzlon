using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class BankDetailList : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserId"] == null && !string.IsNullOrEmpty(Request.QueryString.ToString()))
                {
                    Response.Redirect(ConfigurationManager.AppSettings["WebsiteUrl"] + "/Login.aspx/?ReturnURL=" + HttpContext.Current.Request.Url.AbsoluteUri, true);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString.ToString()))



                    {
                        string[] queryString = Request.QueryString.ToString().Split('?');
                        string selectedTab = Crypto.Instance.Decrypt(HttpUtility.UrlDecode(queryString[0])).Split('=')[1];
                        Session["ActiveTab"] = selectedTab;
                    }
                }

                    if (!IsPostBack)
                {
                    if (Session["ActiveTab"] != null)
                        hidTabActive.Value = Convert.ToString(Session["ActiveTab"]);

                    GetAssignedToValues();
                    //GetVendorCode();
                    hidSerachUserId.Value = "0";
                    int profileId = Convert.ToInt32(Session["ProfileId"]);
                    if (profileId == (int)UserProfileEnum.Vendor)
                    {
                        hidSerachUserId.Value = Convert.ToString(Session["UserId"]);
                        lblMyRecord.InnerText = "My Records ";
                        tabPending.Visible = false;                        
                    }
                    if (profileId != (int)UserProfileEnum.CB)
                        tabCBdocPending.Visible = false;
                    DrpCompanyCode.EmptyMessage = "Select Company Code";
                   
                    string result = commonFunctions.RestServiceCall(Constants.GET_INITIATOR_ACCESS, string.Empty);
                    bool isExist = Convert.ToBoolean(result);
                    if (isExist)
                    {
                        linkToAdd.Visible = false;
                        tabMyRecord.Visible = false;
                        hidTabActive.Value = "PendingApproval";
                        tabIniitatorDocPending.Visible = false;
                    }
                    else
                    {
                        
                        linkToAdd.Visible = true;
                        tabPending.Visible = false;
                        tabIniitatorDocPending.Visible = true;
                    }
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
            try
            {
                string result = string.Empty;
                List<Models.ListItem> lstCompanyCode = new List<Models.ListItem>();
                if (DrpVendorCode.SelectedValue != string.Empty)
                {
                    result = commonFunctions.RestServiceCall(string.Format(Constants.GET_COMPANY_CODE_BY_VENDOR_CODE, DrpVendorCode.SelectedValue), string.Empty);

                    if (result == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    }
                    else
                    {
                        DrpCompanyCode.DataSource = new DataTable();
                        DrpCompanyCode.DataBind();
                        lstCompanyCode = JsonConvert.DeserializeObject<List<Models.ListItem>>(result);
                        DrpCompanyCode.EmptyMessage = "Select Company Code";
                        DrpCompanyCode.DataTextField = "Name";
                        DrpCompanyCode.DataValueField = "Id";
                        DrpCompanyCode.DataSource = lstCompanyCode;
                       
                        DrpCompanyCode.SelectedIndex =-1;
                        DrpCompanyCode.ClearSelection();
                        DrpCompanyCode.Text = "";
                        DrpCompanyCode.DataBind();
                    }
                }
                else
                {
                    DrpCompanyCode.DataSource = lstCompanyCode;
                    DrpCompanyCode.DataBind();
                    //DrpCompanyCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Company Code", string.Empty));
                    //DrpCompanyCode.SelectedIndex = 0;
                    DrpCompanyCode.EmptyMessage = "Select Company Code";
                }
                txtVendorName.Text = string.Empty;
                txtPanNo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        //protected void DrpCompanyCode_SelectedIndexChanged(object sender, EventArgs e)
        protected void DrpCompanyCode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                string result = string.Empty;
                if (DrpCompanyCode.SelectedValue != string.Empty)
                {
                    result = commonFunctions.RestServiceCall(string.Format(Constants.GET_VENDORDETAILS_FOR_WORKFLOW, DrpVendorCode.SelectedValue, DrpCompanyCode.SelectedValue), string.Empty);

                    if (result == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    }
                    else
                    {
                        Models.ListItem vendorDetails = JsonConvert.DeserializeObject<Models.ListItem>(result);
                        txtVendorName.Text = vendorDetails.Id;
                        txtPanNo.Text = vendorDetails.Name;
                    }
                }
                else
                {
                    txtVendorName.Text = string.Empty;
                    txtPanNo.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdMyRecords_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            GetMyRecords();
        }

        protected void grdPending_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetPendingRecords();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdPending_PreRender(object sender, EventArgs e)
        {
            try
            {
                int profileId = Convert.ToInt32(Session["ProfileId"]);
                if (profileId != (int)UserProfileEnum.Vendor)
                {
                    foreach (GridItem item in grdPending.MasterTableView.Items)
                    {
                        if (item is GridEditableItem)
                        {
                            GridEditableItem editableItem = item as GridDataItem;
                            editableItem.Edit = true;
                        }
                    }
                    grdPending.Rebind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdPending_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                int profileId = Convert.ToInt32(Session["ProfileId"]);
                if (profileId != (int)UserProfileEnum.Vendor)
                {
                    GridEditableItem editableItem = (GridEditableItem)e.Item;
                    if (editableItem != null)
                    {
                        GridDataItem item = (GridDataItem)e.Item;
                        TextBox txtVendorCode = (TextBox)(item["VendorCode"].Controls[0]);
                        TextBox txtCompanyCode = (TextBox)(item["CompanyCode"].Controls[0]);
                        string vendorCode = txtVendorCode.Text;
                        string companyCode = txtCompanyCode.Text;
                        string comment = ((TextBox)(editableItem.FindControl("txtComment"))).Text;
                        int? assignedTo = null;
                        int status = (int)Status.Approved;
                        bool isApproved = (editableItem.FindControl("rdbApproved") as RadioButton).Checked;
                        if (isApproved)
                            status = (int)Status.Approved;
                        bool isRejected = (editableItem.FindControl("rdbReject") as RadioButton).Checked;
                        if (isRejected)
                            status = (int)Status.Rejected;
                        bool isNeedCorrection = (editableItem.FindControl("rdbNeedCorrection") as RadioButton).Checked;

                        if (isNeedCorrection)
                        {
                            status = (int)Status.NeedCorrection;
                            assignedTo = Convert.ToInt32(((RadDropDownList)editableItem.FindControl("drpAssignedTo")).SelectedItem.Value);
                        }
                        Models.BankPendingApprovalDetail bankPendingApprovalDetail = new Models.BankPendingApprovalDetail()
                        {
                            BankDetailId = Convert.ToInt32(editableItem.GetDataKeyValue("BankDetailId")),
                            Comment = comment,
                            Status = status,
                            AssignedTo = assignedTo
                        };

                        string jsonInputParameter = JsonConvert.SerializeObject(bankPendingApprovalDetail);
                        string result = string.Empty;
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;

                        //Commented by santosh 21 Jul
                        result = commonFunctions.RestServiceCall(string.Format(Constants.CHECK_BANK_VENDOR_COMPANY_CODE_BLOCK, vendorCode, companyCode), Crypto.Instance.Encrypt(jsonInputParameter));
                        if (result == Constants.REST_CALL_FAILURE)
                            radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                        else
                        {
                            result = JsonConvert.DeserializeObject<string>(result);
                            if (result == Constants.SUCCESS)
                            {
                                result = commonFunctions.RestServiceCall(Constants.UPDATE_BANK_PENDING_APPROVAL_DETAILS, Crypto.Instance.Encrypt(jsonInputParameter));
                                if (result == Constants.REST_CALL_FAILURE)
                                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                                else
                                {
                                    string isSaved = JsonConvert.DeserializeObject<string>(result);
                                    if (isSaved == Constants.SUCCESS)
                                    {
                                        radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                                        GetPendingRecords();
                                        grdPending.DataBind();

                                        CBPendingForDocReceive();
                                        grdCBDocPending.DataBind();
                                    }
                                    else
                                        radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                                }
                            }

                            else
                            {
                                radMessage.Show(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                CommonFunctions.WriteErrorLog(ex);
                e.Canceled = true;
            }
        }
        protected void grdPending_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                int profileId = Convert.ToInt32(Session["ProfileId"]);
                if (profileId != (int)UserProfileEnum.Vendor)
                {
                    if (e.Item is GridEditableItem && e.Item.IsInEditMode && !e.Item.OwnerTableView.IsItemInserted)
                    {
                        var bankDetailModel = e.Item.DataItem as GetBankPendingRecords_Result;
                        GridEditableItem editItem = (GridEditableItem)e.Item;
                        //Access the EditButton and change ImageUrl
                        ImageButton imgEdit = (ImageButton)editItem["EditCommandColumn"].Controls[0];
                        ImageButton imgCancel = (ImageButton)editItem["EditCommandColumn"].Controls[2];
                        var result = ViewState["lstAssignedTo"] as string;
                        List<Models.ListItem> lstAssignedTo = JsonConvert.DeserializeObject<List<Models.ListItem>>(result);
                        TextBox comment = (TextBox)(editItem.FindControl("txtComment"));
                        RadioButton rdbApproved = (RadioButton)editItem.FindControl("rdbApproved");
                        RadioButton rdbReject = (RadioButton)editItem.FindControl("rdbReject");
                        RadioButton rdbNeedCorrection = (RadioButton)editItem.FindControl("rdbNeedCorrection");
                        CustomValidator custValidatorAssignedTo = (CustomValidator)editItem.FindControl("custValidatorAssignedTo");
                        CustomValidator CustValidatorComment = (CustomValidator)editItem.FindControl("CustValidatorComment");
                        rdbApproved.Checked = true;
                        RadDropDownList drpAssignedTo = (RadDropDownList)editItem.FindControl("drpAssignedTo");
                        drpAssignedTo.DataValueField = "Id";
                        drpAssignedTo.DataTextField = "Name";
                        if (bankDetailModel != null)
                        {
                            if (bankDetailModel.IsNew && (profileId >= (int)UserProfileEnum.Treasury))
                            {
                                lstAssignedTo.RemoveAt(4);//Group Controller
                                if (profileId > (int)UserProfileEnum.ManagementAssurance)
                                    lstAssignedTo.RemoveAt(5);//Management Assurance
                                if (profileId > (int)UserProfileEnum.FASSC)
                                    lstAssignedTo.RemoveAt(5);//FASSC
                            }
                            if (bankDetailModel.WorkFlowStatusId.HasValue && bankDetailModel.IsCeatedByVendor == 0 && bankDetailModel.WorkFlowStatusId.HasValue && (profileId >= (int)UserProfileEnum.VerticalController))
                            {
                                lstAssignedTo.RemoveAt(2);//Remove Validator for Suzlon Employee
                                lstAssignedTo[1].Name = "Intiator";//Update Text if created by Suzlon Employee
                            }
                        }
                        drpAssignedTo.DataSource = lstAssignedTo;
                        drpAssignedTo.DataBind();
                        if (imgCancel != null)
                        {
                            imgCancel.Visible = false;
                            imgCancel.Attributes.CssStyle.Add("display", "none");
                        }
                        if (imgEdit != null)
                        {
                            imgEdit.ImageUrl = "Content/images/save.png";
                            imgEdit.Attributes.Add("onclick", "return Validate('" + rdbApproved.ClientID + "','" + rdbReject.ClientID + "','" +
                                rdbNeedCorrection.ClientID + "','" + custValidatorAssignedTo.ClientID + "','" +
                                CustValidatorComment.ClientID + "','" + drpAssignedTo.ClientID + "','" + comment.ClientID + "')");
                        }

                        if (bankDetailModel != null)
                        {
                            LinkButton vendorName = (LinkButton)e.Item.FindControl("vendorName");
                            vendorName.Text = bankDetailModel.VendorName;

                            HyperLink attachmentLink = (HyperLink)e.Item.FindControl("viewAttachment");
                            attachmentLink.Attributes["href"] = "javascript:void(0);";
                            attachmentLink.Attributes["onclick"] = String.Format("return ShowAttachment('{0}','{1}','{2}');", Constants.VENDOR_BANK_ATTACHMENT_PATH, bankDetailModel.Attachment1, bankDetailModel.Attachment2);

                            HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                            commentLink.Attributes["href"] = "javascript:void(0);";
                            commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdMyRecords_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "MyRecord";
                Session["CBPendingDoc"] = false;
                Session["BankDetailId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailId"];
                Session["PreviousPage"] = (int)PageIndex.BankDetailListVendor;
                Response.Redirect("AddVendorBankDetail.aspx");
            }
        }
        protected void grdMyRecords_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var bankDetailModel = e.Item.DataItem as BankDetailModel;
                    if (bankDetailModel != null)
                    {
                        LinkButton vendorName = (LinkButton)e.Item.FindControl("vendorName");
                        vendorName.Text = bankDetailModel.VendorName;
                        HyperLink attachmentLink = (HyperLink)e.Item.FindControl("viewAttachment");
                        attachmentLink.Attributes["href"] = "javascript:void(0);";
                        attachmentLink.Attributes["onclick"] = String.Format("return ShowAttachment('{0}','{1}','{2}');", Constants.VENDOR_BANK_ATTACHMENT_PATH, bankDetailModel.Attachment1, bankDetailModel.Attachment2);

                        HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                        commentLink.Attributes["href"] = "javascript:void(0);";
                        commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", bankDetailModel.BankDetailId);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            try
            {
                if (e.Argument.Contains("Comment"))
                {

                    string values = e.Argument;
                    string[] parameters = values.Split('#');
                    bindComment(parameters[1]);
                    grdComment.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdComment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (ViewState["bankDetailId"] != null)
                bindComment(Convert.ToString(ViewState["bankDetailId"]));
        }
        private void bindComment(string bankDetailId)
        {
            try
            {
                string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_COMMENTS, bankDetailId), string.Empty);
                ViewState["bankDetailId"] = bankDetailId;
                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    if (string.IsNullOrEmpty(result))
                        grdComment.DataSource = new DataTable();
                    else
                    {
                        List<GetBankComment_Result> lstBankComment = JsonConvert.DeserializeObject<List<GetBankComment_Result>>(result);
                        grdComment.DataSource = lstBankComment;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetMyRecords();
                grdMyRecords.DataBind();
                GetPendingRecords();
                grdPending.DataBind();
                CBPendingForDocReceive();
                grdCBDocPending.DataBind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void GetAssignedToValues()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_ASSIGNED_TO_VALUES, Session["ProfileId"]), string.Empty);

                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    ViewState["lstAssignedTo"] = result;
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

                result = commonFunctions.RestServiceCall(string.Format(Constants.GET_VENDOR_CODE_WORKFLOW, userId), string.Empty);

                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    List<GetVendorCodeForWorkflow_Result> lstVendorCode = JsonConvert.DeserializeObject<List<GetVendorCodeForWorkflow_Result>>(result);
                    DrpVendorCode.DataTextField = "Name";
                    DrpVendorCode.DataValueField = "VendorCode";
                    DrpVendorCode.DataSource = lstVendorCode;
                    DrpVendorCode.DataBind();
                    DrpVendorCode.EmptyMessage = "Select Vendor Code";
                    //DrpVendorCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Vendor Code", string.Empty));
                    //DrpVendorCode.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        /// <summary>
        /// This method is used to get pending records.
        /// </summary>
        private void GetPendingRecords()
        {
            try
            {
                int profileId = Convert.ToInt32(Session["ProfileId"]);
                if (profileId != (int)UserProfileEnum.Vendor)
                {
                    string result = string.Empty;
                    // string vendorCode = DrpVendorCode.SelectedIndex > 0 ? DrpVendorCode.SelectedValue : string.Empty;
                    // string companyCode = DrpCompanyCode.SelectedIndex > 0 ? DrpCompanyCode.SelectedValue: string.Empty;
                    string vendorCode = Convert.ToString(DrpVendorCode.SelectedValue); 
                    string companyCode = Convert.ToString(DrpCompanyCode.SelectedValue);

                    result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_PENDING_RECORDS, vendorCode, companyCode), string.Empty);

                    if (result == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                        grdPending.DataSource = new DataTable();
                    }
                    else
                    {
                        List<GetBankPendingRecords_Result> pendingRecords = JsonConvert.DeserializeObject<List<GetBankPendingRecords_Result>>(result);
                        grdPending.DataSource = pendingRecords;
                        lblPendingCount.InnerText = Convert.ToString(pendingRecords.Count);
                    }
                }
                else
                    grdPending.DataSource = new DataTable();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// This method is used to get my records.
        /// </summary>
        private void GetMyRecords()
        {
            try
            {
                string result = string.Empty;
                string vendorCode = Convert.ToString(DrpVendorCode.SelectedValue); //DrpVendorCode.SelectedValue!=""  ? DrpVendorCode.SelectedValue : string.Empty;
                string companyCode = Convert.ToString(DrpCompanyCode.SelectedValue); //DrpVendorCode.SelectedValue != "" ? DrpCompanyCode.SelectedValue : string.Empty;
                result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_MY_RECORDS, vendorCode, companyCode), string.Empty);

                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    grdMyRecords.DataSource = new DataTable();
                    lblMyRecordCount.InnerText = "0";
                }
                else
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        grdMyRecords.DataSource = new DataTable();
                        lblMyRecordCount.InnerText = "0";
                    }
                    else
                    {
                        List<BankDetailModel> myRecords = JsonConvert.DeserializeObject<List<BankDetailModel>>(result);
                        grdMyRecords.DataSource = myRecords;
                        lblMyRecordCount.InnerText = Convert.ToString(myRecords.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private void CBPendingForDocReceive()
        {
            try
            {
                int profileId = Convert.ToInt32(Session["ProfileId"]);
                if (profileId != (int)UserProfileEnum.Vendor)
                {
                    string result = string.Empty;
                    //string vendorCode = DrpVendorCode.SelectedIndex > 0 ? DrpVendorCode.SelectedValue : string.Empty;
                    //string companyCode = DrpCompanyCode.SelectedIndex > 0 ? DrpCompanyCode.SelectedValue : string.Empty;
                    string vendorCode = Convert.ToString(DrpVendorCode.SelectedValue);
                    string companyCode = Convert.ToString(DrpCompanyCode.SelectedValue);
                    result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_PENDING_RECORDSFOR_DOC, vendorCode, companyCode), string.Empty);

                    if (result == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                        grdCBDocPending.DataSource = new DataTable();
                    }
                    else
                    {
                        List<usp_Get_BankDoc_Pending_Record_Result> pendingRecords = JsonConvert.DeserializeObject<List<usp_Get_BankDoc_Pending_Record_Result>>(result);
                        grdCBDocPending.DataSource = pendingRecords;
                        lblDocPendingCount.InnerText = Convert.ToString(pendingRecords.Count);
                    }
                }
                else
                    grdCBDocPending.DataSource = new DataTable();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private void InitiatorPendingForDocSend()
        {
            try
            {
                
                    string result = string.Empty;                   
                    string vendorCode = Convert.ToString(DrpVendorCode.SelectedValue);
                    string companyCode = Convert.ToString(DrpCompanyCode.SelectedValue);
                    result = commonFunctions.RestServiceCall(string.Format(Constants.GET_INITIATORDOCSENDPENDING, vendorCode, companyCode), string.Empty);

                    if (result == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                        grdInitiatorPendingDoc.DataSource = new DataTable();
                    }
                    else
                    {
                        List<usp_GetInitiatorDocSentPending_Result> pendingRecords = JsonConvert.DeserializeObject<List<usp_GetInitiatorDocSentPending_Result>>(result);
                        grdInitiatorPendingDoc.DataSource = pendingRecords;
                    lblDocSendPendingCount.InnerText = Convert.ToString(pendingRecords.Count);
                    }
             
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void linkToAdd_Click(object sender, EventArgs e)
        {
            Session["BankDetailId"] = 0;
            Session["PreviousPage"] = (int)PageIndex.BankDetailListVendor;
            Response.Redirect("AddVendorBankDetail.aspx");
        }

        protected void grdPending_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["CBPendingDoc"] = false;
                Session["ActiveTab"] = "PendingApproval";
                Session["BankDetailId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailId"];
                Response.Redirect("AddVendorBankDetail.aspx");
            }
        }

        protected void grdCBDocPending_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            CBPendingForDocReceive();
        }

        protected void grdCBDocPending_PreRender(object sender, EventArgs e)
        {
            try
            {

                foreach (GridItem item in grdCBDocPending.MasterTableView.Items)
                {
                    if (item is GridEditableItem)
                    {
                        GridEditableItem editableItem = item as GridDataItem;
                        editableItem.Edit = true;
                    }
                }
                grdCBDocPending.Rebind();

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdCBDocPending_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                int profileId = Convert.ToInt32(Session["ProfileId"]);
                if (profileId != (int)UserProfileEnum.Vendor)
                {
                    if (e.Item is GridEditableItem && e.Item.IsInEditMode && !e.Item.OwnerTableView.IsItemInserted)
                    {
                        var bankDetailModel = e.Item.DataItem as usp_Get_BankDoc_Pending_Record_Result;
                        GridEditableItem editItem = (GridEditableItem)e.Item;
                        //Access the EditButton and change ImageUrl
                        ImageButton imgEdit = (ImageButton)editItem["EditCommandColumn"].Controls[0];
                        ImageButton imgCancel = (ImageButton)editItem["EditCommandColumn"].Controls[2];

                        RadDatePicker docReceiveDate = (RadDatePicker)(editItem.FindControl("ReceiveDate"));
                        CheckBox chkRecived = (CheckBox)editItem.FindControl("chkDocReceived");

                        Label custValidatorchk = (Label)editItem.FindControl("CustValidatorchk");
                        CustomValidator CustValidatorDate = (CustomValidator)editItem.FindControl("CustValidatorRecDoc");

                        if (imgCancel != null)
                        {
                            imgCancel.Visible = false;
                            imgCancel.Attributes.CssStyle.Add("display", "none");
                        }
                        if (imgEdit != null)
                        {
                            imgEdit.ImageUrl = "Content/images/save.png";
                            imgEdit.Attributes.Add("onclick", "return ValidateCBDoc('" + docReceiveDate.ClientID + "','" + chkRecived.ClientID + "','" + custValidatorchk.ClientID + "','" + CustValidatorDate.ClientID + "')");
                        }

                        if (bankDetailModel != null)
                        {
                            LinkButton vendorName = (LinkButton)e.Item.FindControl("vendorName");
                            vendorName.Text = bankDetailModel.vendorname;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdCBDocPending_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    RadDatePicker docReceiveDate = (RadDatePicker)(editableItem.FindControl("ReceiveDate"));
                    CheckBox chkRecived = (CheckBox)editableItem.FindControl("chkDocReceived");

                    int bankHistoryId = Convert.ToInt32(editableItem.GetDataKeyValue("BankDetailHistoryId"));

                    BankDetailHistoryModel docReceivedDtl = new BankDetailHistoryModel()
                    {
                        BankDetailHistoryId = bankHistoryId,
                        OriginalDocumentsReceived = true,
                        ReceivedDate = Convert.ToDateTime(docReceiveDate.SelectedDate)
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(docReceivedDtl);
                    string result = string.Empty;
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;


                    result = commonFunctions.RestServiceCall(string.Format(Constants.UPDATE_BANK_DOC_RECEIVED), Crypto.Instance.Encrypt(jsonInputParameter));
                    if (result == Constants.REST_CALL_FAILURE)
                        radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    else
                    {
                        result = JsonConvert.DeserializeObject<string>(result);
                        if (result == Constants.SUCCESS)
                        {
                            radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                            CBPendingForDocReceive();
                            grdCBDocPending.DataBind();
                        }
                        else
                            radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    }
                }
            }
            catch (Exception ex)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                CommonFunctions.WriteErrorLog(ex);
                e.Canceled = true;
            }
        }

        protected void grdCBDocPending_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "CBDocPending";
                Session["BankDetailId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailId"];
                Session["BankDetailHistoryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailHistoryId"];
                Session["CBPendingDoc"] = true;
                Response.Redirect("AddVendorBankDetail.aspx");
            }
        }

        protected void grdInitiatorPendingDoc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            InitiatorPendingForDocSend();
        }

        protected void grdInitiatorPendingDoc_PreRender(object sender, EventArgs e)
        {
            try
            {

                foreach (GridItem item in grdInitiatorPendingDoc.MasterTableView.Items)
                {
                    if (item is GridEditableItem)
                    {
                        GridEditableItem editableItem = item as GridDataItem;
                        editableItem.Edit = true;
                    }
                }
                grdInitiatorPendingDoc.Rebind();

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdInitiatorPendingDoc_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
            if (editableItem != null)
            {
                GridDataItem item = (GridDataItem)e.Item;                   

                    RadDatePicker docSendDate = (RadDatePicker)(editableItem.FindControl("SendDate"));
                    CheckBox chkSend = (CheckBox)editableItem.FindControl("chkDocsend");
                    int bankHistoryId = Convert.ToInt32(editableItem.GetDataKeyValue("BankDetailHistoryId"));

                    BankDetailHistoryModel docReceivedDtl = new BankDetailHistoryModel()
                    {
                        BankDetailHistoryId = bankHistoryId,
                        OriginalDocumentsReceived = chkSend.Checked,
                        ReceivedDate = (docSendDate.SelectedDate.HasValue ? Convert.ToDateTime(docSendDate.SelectedDate) : docSendDate.SelectedDate)
                    };

                string jsonInputParameter = JsonConvert.SerializeObject(docReceivedDtl);
                string result = string.Empty;
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;


                result = commonFunctions.RestServiceCall(string.Format(Constants.UPDATE_BANK_DOC_SEND), Crypto.Instance.Encrypt(jsonInputParameter));
                if (result == Constants.REST_CALL_FAILURE)
                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                else
                {
                    result = JsonConvert.DeserializeObject<string>(result);
                    if (result == Constants.SUCCESS)
                    {
                        radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                            InitiatorPendingForDocSend();
                            grdInitiatorPendingDoc.DataBind();
                    }
                    else
                        radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                }
            }
        }
            catch (Exception ex)
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                CommonFunctions.WriteErrorLog(ex);
                e.Canceled = true;
            }
}

        protected void grdInitiatorPendingDoc_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "IniitatorDocPending";
                Session["BankDetailId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailId"];
                Session["BankDetailHistoryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailHistoryId"];
                Session["CBPendingDoc"] = true;
                Response.Redirect("AddVendorBankDetail.aspx");
            }
        }

        protected void grdInitiatorPendingDoc_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridEditableItem && e.Item.IsInEditMode && !e.Item.OwnerTableView.IsItemInserted)
                {
                    var bankDetailModel = e.Item.DataItem as usp_GetInitiatorDocSentPending_Result;
                    GridEditableItem editItem = (GridEditableItem)e.Item;
                    //Access the EditButton and change ImageUrl
                    ImageButton imgEdit = (ImageButton)editItem["EditCommandColumn"].Controls[0];
                    ImageButton imgCancel = (ImageButton)editItem["EditCommandColumn"].Controls[2];

                    RadDatePicker docSendDate = (RadDatePicker)(editItem.FindControl("SendDate"));
                    CheckBox chkSend = (CheckBox)editItem.FindControl("chkDocsend");

                    Label custValidatorchk = (Label)editItem.FindControl("CustValidatorchkInitiator");
                    CustomValidator CustValidatorDate = (CustomValidator)editItem.FindControl("CustValidatorSentDoc");

                    if (imgCancel != null)
                    {
                        imgCancel.Visible = false;
                        imgCancel.Attributes.CssStyle.Add("display", "none");
                    }
                    if (imgEdit != null)
                    {
                        imgEdit.ImageUrl = "Content/images/save.png";
                        int profileId = Convert.ToInt32(Session["ProfileId"]);
                        if (profileId != (int)UserProfileEnum.Validator)
                        {
                            imgEdit.Attributes.Add("onclick", "return ValidateCBDoc('" + docSendDate.ClientID + "','" + chkSend.ClientID + "','" + custValidatorchk.ClientID + "','" + CustValidatorDate.ClientID + "')");
                        }
                    }

                    if (bankDetailModel != null)
                    {
                        LinkButton vendorName = (LinkButton)e.Item.FindControl("vendorName");
                        vendorName.Text = bankDetailModel.vendorname;
                        if (bankDetailModel.OriginalDocumentsSent != null && bankDetailModel.OriginalDocumentsSent.Value)
                        {
                            chkSend.Checked = bankDetailModel.OriginalDocumentsSent.Value;
                            if (bankDetailModel.SendDate.HasValue)
                                docSendDate.SelectedDate = bankDetailModel.SendDate.Value.Date;
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }

        protected void grdMyRecords_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }
    }

    //public class DocReceivedDtl{
    //    public int BankDetailHistoryId { get; set; }
    //    public int BankDetailId { get; set; }
    //    public Nullable<System.DateTime> ReceivedDate { get; set; }

    //}
}