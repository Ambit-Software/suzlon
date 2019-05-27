using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using Cryptography;
using System.Collections;

namespace SuzlonBPP
{
    public partial class BankValidator : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.VENDORBANKDETAILS, menuList)) Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {
                GetdropdownValues();
            }
        }
        protected void grdValidator_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetValidatorRecords();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdValidator_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (GridItem item in grdValidator.MasterTableView.Items)
                {
                    if (item is GridEditableItem)
                    {
                        GridEditableItem editableItem = item as GridDataItem;
                        editableItem.Edit = true;
                    }
                }
                grdValidator.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdValidator_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
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
                    int status = Convert.ToInt32(((RadDropDownList)editableItem.FindControl("drpStatus")).SelectedItem.Value);
                    string subVerticalId = string.Empty;
                    if (status == (int)Status.Approved)
                        subVerticalId = ((RadDropDownList)editableItem.FindControl("drpSubVertical")).SelectedItem.Value;
                    Models.BankValidatorUpdate bankValidatorUpdate = new Models.BankValidatorUpdate()
                    {
                        BankDetailId = Convert.ToInt32(editableItem.GetDataKeyValue("BankDetailId")),
                        Comment = comment,
                        Status = status,
                        SubVerticalId = (string.IsNullOrEmpty(subVerticalId) ? 0 : Convert.ToInt32(subVerticalId))
                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(bankValidatorUpdate);
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
                            result = commonFunctions.RestServiceCall(Constants.UPDATE_BANK_VALIDATOR_DETAILS, Crypto.Instance.Encrypt(jsonInputParameter));
                            if (result == Constants.REST_CALL_FAILURE)
                                radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                            else
                            {
                                bool isSaved = JsonConvert.DeserializeObject<bool>(result);
                                if (isSaved)
                                {
                                    radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                                    GetValidatorRecords();
                                    grdValidator.DataBind();
                                }
                                else
                                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                            }
                        }
                        else
                            radMessage.Show(result);
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

        protected void grdValidator_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode && !e.Item.OwnerTableView.IsItemInserted)
                {
                    GridEditableItem editItem = (GridEditableItem)e.Item;
                    TextBox comment = (TextBox)(editItem.FindControl("txtComment"));
                    RadDropDownList drpSubVertical = (RadDropDownList)editItem.FindControl("drpSubVertical");
                    RadDropDownList drpStatus = (RadDropDownList)editItem.FindControl("drpStatus");
                    CustomValidator custValidatorStatus = (CustomValidator)editItem.FindControl("custValidatorStatus");
                    CustomValidator custValidatorSubVertical = (CustomValidator)editItem.FindControl("custValidatorSubVertical");
                    CustomValidator CustValidatorComment = (CustomValidator)editItem.FindControl("CustValidatorComment");

                    List<Models.ListItem> lstSubVertical = new List<Models.ListItem>();
                    List<Models.ListItem> lstStatus = new List<Models.ListItem>();
                    if (ViewState["lstSubVertical"] != null)
                        lstSubVertical = JsonConvert.DeserializeObject<List<Models.ListItem>>(ViewState["lstSubVertical"] as string);
                    if (ViewState["lstStatus"] != null)
                        lstStatus = JsonConvert.DeserializeObject<List<Models.ListItem>>(ViewState["lstStatus"] as string);
                    lstSubVertical.Insert(0, new Models.ListItem() { Id = "", Name = "Select Sub Vertical" });
                    drpSubVertical.DataSource = lstSubVertical;
                    drpSubVertical.DataBind();
                    lstStatus.Insert(0, new Models.ListItem() { Id = "", Name = "Select Status" });
                    drpStatus.DataSource = lstStatus;
                    drpStatus.DataBind();
                    drpStatus.SelectedIndex = 0;

                    //Access the EditButton and change ImageUrl
                    ImageButton imgEdit = (ImageButton)editItem["EditCommandColumn"].Controls[0];
                    ImageButton imgCancel = (ImageButton)editItem["EditCommandColumn"].Controls[2];
                    if (imgCancel != null)
                    {
                        imgCancel.Visible = false;
                        imgCancel.Attributes.CssStyle.Add("display", "none");
                    }
                    if (imgEdit != null)
                    {
                        imgEdit.ImageUrl = "Content/images/save.png";
                        imgEdit.Attributes.Add("onclick", "return Validate('" + drpStatus.ClientID + "','" + custValidatorStatus.ClientID + "','" + drpSubVertical.ClientID + "','" +
                            custValidatorSubVertical.ClientID + "','" + comment.ClientID + "','" +
                            CustValidatorComment.ClientID + "')");
                    }
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
                        commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailId"], e.Item.ItemIndex);
                        if (bankDetailModel.SubVerticalId != null)
                            drpSubVertical.SelectedValue = Convert.ToString(bankDetailModel.SubVerticalId);
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
        private void GetValidatorRecords()
        {
            try
            {
                int profileId = Convert.ToInt32(Session["ProfileId"]);
                if (profileId == (int)UserProfileEnum.Validator)
                {
                    string result = string.Empty;
                    result = commonFunctions.RestServiceCall(string.Format(Constants.GET_BANK_VALIDATOR_DATA), string.Empty);

                    if (result == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(result))
                            grdValidator.DataSource = new DataTable();
                        else
                        {
                            List<BankDetailModel> ValidatorRecords = JsonConvert.DeserializeObject<List<BankDetailModel>>(result);
                            grdValidator.DataSource = ValidatorRecords;
                        }
                    }
                }
                else
                    grdValidator.DataSource = new DataTable();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private void GetdropdownValues()
        {
            try
            {
                string drpname = "subvertical,workflow-status";
                string result = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);

                if (result == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    ViewState["lstSubVertical"] = JsonConvert.SerializeObject(ddValues.SubVertical);
                    ViewState["lstStatus"] = JsonConvert.SerializeObject(ddValues.WorkFlowStatus);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdValidator_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["BankDetailId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BankDetailId"];
                Session["PreviousPage"] = (int)PageIndex.BankValidator;
                Response.Redirect("AddVendorBankDetail.aspx");
            }
        }

        protected void linkToAdd_Click(object sender, EventArgs e)
        {
            Session["BankDetailId"] = 0;
            Session["PreviousPage"] = (int)PageIndex.BankValidator;
            Response.Redirect("AddVendorBankDetail.aspx");
        }
    }
}