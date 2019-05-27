using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class TreasuryListVertical : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.VERTICALCONTROLLER, menuList)) Response.Redirect("~/webNoAccess.aspx");


            if (!IsPostBack)
            {
                if (Session["ActiveTab"] != null)
                    hidTabActive.Value = Convert.ToString(Session["ActiveTab"]);

                BindMyRequestGrid();
                grdMyRequest.DataBind();
                BindMyApprovedRequest();
                grdMyApprovedRequest.DataBind();
                BindMyAddendumRequest();
                grdMyAddendum.DataBind();
            }
        }

        private void BindMyRequestGrid()
        {
            try
            {
                string resulTreasury = commonFunctions.RestServiceCall(Constants.GET_TREASURY_MYREQUEST, string.Empty);
                if (resulTreasury == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    grdMyRequest.DataSource = new DataTable();
                }
                else { 
                if (!string.IsNullOrEmpty(resulTreasury))
                {
                    List<GetTreasuryMyRequest_Result> lstMyRequest = JsonConvert.DeserializeObject<List<GetTreasuryMyRequest_Result>>(resulTreasury);
                    grdMyRequest.DataSource = lstMyRequest;
                    lblMyRequest.Text = Convert.ToString(lstMyRequest.Count);                      
                }
                else
                {
                    grdMyRequest.DataSource = new DataTable();
                    lblMyRequest.Text = "0";
                }
              }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }        
    }
        private void BindMyApprovedRequest()
        {
            try
            {
                string resulTreasury = commonFunctions.RestServiceCall(Constants.GET_TREASURY_MYRAPPROVEREQUEST, string.Empty);
                if (resulTreasury == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    grdMyApprovedRequest.DataSource = new DataTable();
                }
                else
                {
                    if (!string.IsNullOrEmpty(resulTreasury))
                    {
                        List<GetTreasuryMyApprovedRequest_Result> lstMyRequest = JsonConvert.DeserializeObject<List<GetTreasuryMyApprovedRequest_Result>>(resulTreasury);
                        grdMyApprovedRequest.DataSource = lstMyRequest;
                        lblApprovedRequest.Text = Convert.ToString(lstMyRequest.Count);
                    }
                    else
                    {
                        grdMyApprovedRequest.DataSource = new DataTable();
                        lblApprovedRequest.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private void BindMyAddendumRequest()
        {
            try
            {
                string resulTreasury = commonFunctions.RestServiceCall(Constants.GET_TREASURY_MYADDENDUMREQUEST, string.Empty);
                if (resulTreasury == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    grdMyAddendum.DataSource = new DataTable();
                }
                else
                {
                    if (!string.IsNullOrEmpty(resulTreasury))
                    {
                        List<GetTreasuryMyAddendumRequest_Result> lstMyRequest = JsonConvert.DeserializeObject<List<GetTreasuryMyAddendumRequest_Result>>(resulTreasury);
                        grdMyAddendum.DataSource = lstMyRequest;
                        lblAddendumRequest.Text = Convert.ToString(lstMyRequest.Count);
                    }
                    else
                    {
                        grdMyAddendum.DataSource = new DataTable();
                        lblAddendumRequest.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void grdMyRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindMyRequestGrid();
        }
        protected void linkToAdd_Click(object sender, EventArgs e)
        {
            Session["TreasuryId"] = "0";
            Response.Redirect("VerticalControllerDetail.aspx");
        }
        protected void grdMyRequest_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if(e.CommandName== "Redirect")
            {
                Session["ActiveTab"] = "MyRequest";
                Session["TreasuryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"];             
                Response.Redirect("VerticalControllerDetail.aspx");
            }
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                string status = Convert.ToString(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Status"]);

                bool canAdd = true;
                bool canDelete = true;

                if (status.Trim() == "Pending Approval by Treasury" || status.Trim()== "Rejected By Treasury")
                {
                    canAdd = false;
                    canDelete = false;
                }
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('TreasuryListVertical','update','" + Convert.ToString(Id) + "','"+canAdd+"','"+canDelete+"','true','true','Treasury');");
            }
        }
        protected void grdMyRequest_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var myRequest = e.Item.DataItem as GetTreasuryMyRequest_Result;
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
       

        protected void grdMyApprovedRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindMyApprovedRequest();
        }

        protected void grdMyApprovedRequest_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "MyApproveRequest";
                Session["TreasuryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"];
                Response.Redirect("VerticalControllerDetail.aspx");
            }
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('TreasuryListVertical','update','" + Convert.ToString(Id) + "','false','true','true','true','Treasury');");
            }
        }

        protected void grdMyApprovedRequest_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                var myApproveRequest = e.Item.DataItem as GetTreasuryMyApprovedRequest_Result;
                if (e.Item is GridDataItem)
                {
                    LinkButton lnkAllocationNo = (LinkButton)e.Item.FindControl("treasoryAllocationNo");
                    lnkAllocationNo.Text = myApproveRequest.AllocationNumber;
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void grdMyAddendum_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindMyAddendumRequest();
        }

        protected void grdMyAddendum_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                Session["ActiveTab"] = "MyAddendumRequest";
                Session["TreasuryId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"];
                Response.Redirect("VerticalControllerDetail.aspx");
            }
            if (e.CommandName == "Attachment")
            {
                String Id = e.CommandArgument.ToString();
                //screen, mode, entityId, canAdd,canDelete,isMultiFileUpload,showDocumentType,EntityName
                RadAjaxManager1.ResponseScripts.Add("openRadWin('TreasuryListVertical','update','" + Convert.ToString(Id) + "','true','true','true','true','Treasury');");
            }
        }

        protected void grdMyAddendum_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var myRequest = e.Item.DataItem as GetTreasuryMyAddendumRequest_Result;
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

        protected void grdMyAddendum_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridItem)
            {
                var PendingDetailModel = e.Item.DataItem as GetTreasuryMyAddendumRequest_Result;
                if (PendingDetailModel != null)
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);
                }
            }
        }

        protected void grdMyApprovedRequest_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridItem)
            {
                var PendingDetailModel = e.Item.DataItem as GetTreasuryMyApprovedRequest_Result;
                if (PendingDetailModel != null)
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);
                }
            }
        }

        protected void grdMyRequest_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridItem)
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    double RequestedAmount = Convert.ToDouble(dataItem["RequestedAmount"].Text);
                    dataItem["RequestedAmount"].Text = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(RequestedAmount));

                }
                var PendingDetailModel = e.Item.DataItem as GetTreasuryMyRequest_Result;
                if (PendingDetailModel != null)
                {
                    HyperLink commentLink = (HyperLink)e.Item.FindControl("viewComment");
                    commentLink.Attributes["href"] = "javascript:void(0);";
                    commentLink.Attributes["onclick"] = String.Format("return ShowComments('{0}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TreasuryDetailId"]);
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
        protected void grdComment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (ViewState["treasurtRequestId"] != null)
                bindComment(Convert.ToString(ViewState["treasurtRequestId"]));
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
    }
}