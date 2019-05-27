using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
namespace SuzlonBPP
{
    public partial class TreasuryList : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.TREASURY, menuList)) Response.Redirect("~/webNoAccess.aspx");


            if (!IsPostBack)
            {
               int profileId = Convert.ToInt32(Session["ProfileId"]);

                if (profileId == (int)UserProfileEnum.CB)                
                {
                    ProfileName.Value = "CB";
                }
                else
                {
                    ProfileName.Value = "Treasury";
                }

                if (Session["ActiveTab"] != null)
                    hidTabActive.Value = Convert.ToString(Session["ActiveTab"]);


                BindPendingRequest();
                grdPendingRequest.DataBind();
                BindPendingAddendumRequest();
                grdPendingAddendumRequest.DataBind();
                BindApprovedRequestByMe();
                grdApprovedRequestByMe.DataBind();
                BindPendingRequestCB();
                grdRequestToCB.DataBind();
            }
            
        }
        private void BindPendingRequest()
        {
            try
            {
                string resulTreasury = commonFunctions.RestServiceCall(Constants.GET_TREASURY_PENDINGREQUEST, string.Empty);
                if (resulTreasury == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    grdPendingRequest.DataSource = new DataTable();
                }
                else
                {
                    if (!string.IsNullOrEmpty(resulTreasury))
                    {
                        List<GetTreasuryPendingRequest_Result> lstMyRequest = JsonConvert.DeserializeObject<List<GetTreasuryPendingRequest_Result>>(resulTreasury);
                        grdPendingRequest.DataSource = lstMyRequest;
                        lblApprovePendingCount.InnerText = Convert.ToString(lstMyRequest.Count);
                    }
                    else
                    {
                        grdPendingRequest.DataSource = new DataTable();
                        lblApprovePendingCount.InnerText = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdPendingRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindPendingRequest();
        }
        protected void grdPendingRequest_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var myRequest = e.Item.DataItem as GetTreasuryPendingRequest_Result;
                    if (myRequest != null)
                    {

                        LinkButton lnkAllocationno = (LinkButton)e.Item.FindControl("treasoryAllocationNo");
                        lnkAllocationno.Text = myRequest.AllocationNumber;
                    }
                }

                
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }

        }
        protected void grdPendingRequest_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "PendingRequest";
                Session["TreasuryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"];
                Response.Redirect("VerticalControllerDetail.aspx");
            }
            else
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('TreasuryList','update','" + Convert.ToString(Id) + "','true','true','true','true','Treasury');");
            }
        }
        protected void grdPendingRequest_ItemDataBound(object sender, GridItemEventArgs e)
        {            
            if (e.Item is GridItem)
            {
                var PendingDetailModel = e.Item.DataItem as GetTreasuryPendingRequest_Result;
                if (PendingDetailModel != null)
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);

                    //HyperLink attachmentLink = (HyperLink)e.Item.FindControl("viewAttachment");
                    //attachmentLink.Attributes["href"] = "javascript:void(0);";
                    //attachmentLink.Attributes["onclick"] = String.Format("return ShowAttachments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);                                    
                }
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

                if (e.Argument.Contains("Attachment"))
                {

                    string values = e.Argument;
                    string[] parameters = values.Split('#');
                    //bindAttachment(parameters[1]);
                    //gridAttachment.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
      
        private void bindComment(string treasuryReqId)
        {
            try
            {
                ViewState["treasurtRequestId"] = treasuryReqId;
                string result = commonFunctions.RestServiceCall(string.Format(Constants.GET_TREASURY_REQUESTCOMMENT, treasuryReqId), string.Empty);             
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
                        List<TreasuryComment> lstBankComment = JsonConvert.DeserializeObject<List<TreasuryComment>>(result);
                        grdComment.DataSource = lstBankComment;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdComment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if(ViewState["treasurtRequestId"]!=null)
            bindComment(Convert.ToString(ViewState["treasurtRequestId"]));
        }
     
        private void BindPendingAddendumRequest()
        {
            try
            {
                string resulTreasury = commonFunctions.RestServiceCall(Constants.GET_TREASURY_ADDENDUMPENDINGREQUEST, string.Empty);
                if (resulTreasury == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    grdPendingAddendumRequest.DataSource = new DataTable();
                }
                else
                {
                    if (!string.IsNullOrEmpty(resulTreasury))
                    {
                        List<GetTreasuryPendingAddendumRequest_Result> lstMyRequest = JsonConvert.DeserializeObject<List<GetTreasuryPendingAddendumRequest_Result>>(resulTreasury);
                        grdPendingAddendumRequest.DataSource = lstMyRequest;
                        lblApprovePendingAdddendumCount.InnerText = Convert.ToString(lstMyRequest.Count);
                    }
                    else
                    {
                        grdPendingAddendumRequest.DataSource = new DataTable();
                        lblApprovePendingAdddendumCount.InnerText = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdPendingAddendumRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
          BindPendingAddendumRequest();
        }
        protected void grdPendingAddendumRequest_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var myRequest = e.Item.DataItem as GetTreasuryPendingAddendumRequest_Result;
                    if (myRequest != null)
                    {

                        LinkButton lnkAllocationno = (LinkButton)e.Item.FindControl("treasoryAllocationNoAddendum");
                        lnkAllocationno.Text = myRequest.AllocationNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdPendingAddendumRequest_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "PendingAddendumRequest";
                Session["TreasuryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"];
                Response.Redirect("VerticalControllerDetail.aspx");
            }
            else
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('TreasuryList','update','" + Convert.ToString(Id) + "','true','true','true','true','Treasury');");
            }

        }
        protected void grdPendingAddendumRequest_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridItem)
            {
                var PendingDetailModel = e.Item.DataItem as GetTreasuryPendingAddendumRequest_Result;
                if (PendingDetailModel != null)
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewCommentAddendum");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);

                    //HyperLink attachmentLink = (HyperLink)e.Item.FindControl("viewAttachmentAddendum");
                    //attachmentLink.Attributes["href"] = "javascript:void(0);";
                    //attachmentLink.Attributes["onclick"] = String.Format("return ShowAttachments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);
                }
            }
        }
        private void BindApprovedRequestByMe()
        {
            try
            {
                string resulTreasury = commonFunctions.RestServiceCall(Constants.GET_TREASURY_REQUESTAPPROVEBYME, string.Empty);
                if (resulTreasury == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    grdApprovedRequestByMe.DataSource = new DataTable();
                }
                else
                {
                    if (!string.IsNullOrEmpty(resulTreasury))
                    {
                        List<GetTreasuryRequestApproveByMe_Result> lstMyRequest = JsonConvert.DeserializeObject<List<GetTreasuryRequestApproveByMe_Result>>(resulTreasury);
                        grdApprovedRequestByMe.DataSource = lstMyRequest;
                        lblApproveRequestBymeCount.InnerText = Convert.ToString(lstMyRequest.Count);
                    }
                    else
                    {
                        grdApprovedRequestByMe.DataSource = new DataTable();
                        lblApproveRequestBymeCount.InnerText = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdApprovedRequestByMe_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindApprovedRequestByMe();
        }

        protected void grdApprovedRequestByMe_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var myRequest = e.Item.DataItem as GetTreasuryRequestApproveByMe_Result;
                    if (myRequest != null)
                    {

                        LinkButton lnkAllocationno = (LinkButton)e.Item.FindControl("treasoryAllocationNo");
                        lnkAllocationno.Text = myRequest.AllocationNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdApprovedRequestByMe_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "ApprovedRequest";
                Session["TreasuryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"];
                Response.Redirect("VerticalControllerDetail.aspx");
            }
            else
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('TreasuryList','update','" + Convert.ToString(Id) + "','false','false','true','true','Treasury');");
            }
        }

        protected void grdApprovedRequestByMe_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridItem)
            {
                var PendingDetailModel = e.Item.DataItem as GetTreasuryRequestApproveByMe_Result;
                if (PendingDetailModel != null)
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);

                    //HyperLink attachmentLink = (HyperLink)e.Item.FindControl("viewAttachment");
                    //attachmentLink.Attributes["href"] = "javascript:void(0);";
                    //attachmentLink.Attributes["onclick"] = String.Format("return ShowAttachments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);
                }
            }
        }


        private void BindPendingRequestCB()
        {
            try
            {
                string resulTreasury = commonFunctions.RestServiceCall(Constants.GET_TREASURY_REQUESTTOCB, string.Empty);
                if (resulTreasury == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    grdRequestToCB.DataSource = new DataTable();
                }
                else
                {
                    if (!string.IsNullOrEmpty(resulTreasury))
                    {
                        List<GetTreasuryRequestToCB_Result> lstMyRequest = JsonConvert.DeserializeObject<List<GetTreasuryRequestToCB_Result>>(resulTreasury);
                        grdRequestToCB.DataSource = lstMyRequest;
                        lblCBApproveRequestCount.InnerText = Convert.ToString(lstMyRequest.Count);
                    }
                    else
                    {
                        grdRequestToCB.DataSource = new DataTable();
                        lblCBApproveRequestCount.InnerText = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdRequestToCB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindPendingRequestCB();
        }

        protected void grdRequestToCB_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var myRequest = e.Item.DataItem as GetTreasuryRequestToCB_Result;
                    if (myRequest != null)
                    {

                        LinkButton lnkAllocationno = (LinkButton)e.Item.FindControl("treasoryAllocationNo");
                        lnkAllocationno.Text = myRequest.AllocationNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdRequestToCB_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "PendingRequestCB";
                Session["TreasuryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"];
                Response.Redirect("VerticalControllerDetail.aspx");
            }
            else
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('TreasuryList','update','" + Convert.ToString(Id) + "','true','true','true','true','Treasury');");
            }
        }

        protected void grdRequestToCB_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridItem)
            {
                var PendingDetailModel = e.Item.DataItem as GetTreasuryRequestToCB_Result;
                if (PendingDetailModel != null)
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);

                    //HyperLink attachmentLink = (HyperLink)e.Item.FindControl("viewAttachment");
                    //attachmentLink.Attributes["href"] = "javascript:void(0);";
                    //attachmentLink.Attributes["onclick"] = String.Format("return ShowAttachments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);
                }
            }

        }
    }

    public class TreasuryComment
    {
        public string  Name { get; set; }
        public string Comment { get; set; }
    }
    public class TreasuryAttachment
    {
        //public string FileName { get; set; }
        //public string Name { get; set; }
        public string Attachment1 { get; set; }
        public string Name { get; set; }
    }
}