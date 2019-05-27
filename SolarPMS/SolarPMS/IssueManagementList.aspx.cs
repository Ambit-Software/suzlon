using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class IssueManagementList : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.ISSUEMANAGEMENT, menuList))
                Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {
                if (Constants.ISSUE_MGT_All_ISSUE_ACCESSADMIN == Convert.ToString(Session["ProfileName"]).ToUpper() ||
                   Constants.ISSUE_MGT_All_ISSUE_ACCESS_FUNCTIONAL_MANAGER == Convert.ToString(Session["ProfileName"]).ToUpper() ||
                  Constants.ISSUE_MGT_All_ISSUE_ACCESS_STATE_HEAD == Convert.ToString(Session["ProfileName"]).ToUpper() ||
                  Constants.ISSUE_MGT_All_ISSUE_CO_ORDINATOR == Convert.ToString(Session["ProfileName"]).ToUpper())
                {
                    ProfileName.Value = "ShowAllIssue";
                }
                else
                {                  
                   ProfileName.Value = "HideAllIssue";
                }
                BindUserDropdown();

                BindAssignToMe("");
                grdAssignToMe.DataBind();

                BindTRaiseToMe("");
                grdRaisedByMe.DataBind();
                                
                if (ProfileName.Value== "ShowAllIssue")
                { 
                BindALlIssues("");
                grdAllIssue.DataBind();
                }
            }
        }
        private void BindUserDropdown()
        {
            try
            {
                //string drpname = "category";
                //string resultDropDown = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);

                //if (!string.IsNullOrEmpty(resultDropDown))
                //{
                //    DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(resultDropDown);
                //    drpSearchCategorys.DataTextField = "Name";
                //    drpSearchCategorys.DataValueField = "Id";
                //    drpSearchCategorys.DataSource = ddValues.category;
                //    drpSearchCategorys.DataBind();
                //    drpSearchCategorys.Items.Insert(0, new RadComboBoxItem() { Text = "All", Value = String.Empty });
                //    drpSearchCategorys.SelectedIndex = 0;
                //}
                //else
                //{
                //    drpSearchCategorys.DataSource =new DataTable();
                //    drpSearchCategorys.DataBind();
                //}



            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        private void BindALlIssues(string searchtxt)
        {
            string resultAllIssues = commonFunctions.RestServiceCall(string.Format(Constants.ISSUE_MGT_GET_ALL_ISSUES,searchtxt.Replace("All","")), string.Empty);

            if (!string.IsNullOrEmpty(resultAllIssues))
            {
                List<IssueMangementModel> AllIssues = JsonConvert.DeserializeObject<List<IssueMangementModel>>(resultAllIssues);
                grdAllIssue.DataSource = AllIssues;
                lblAllissueCount.Text= Convert.ToString(AllIssues.Count);
            }
            else
            {
                grdAllIssue.DataSource = new DataTable();
                lblAllissueCount.Text = "0";
            }
        }
        private void BindAssignToMe(string searchtxt)
        {
            string resultAssinnIssues = commonFunctions.RestServiceCall(string.Format(Constants.ISSUE_MGT_GET_ASSSIGN_TO_ME , searchtxt.Replace("All", "")), string.Empty);
            if (!string.IsNullOrEmpty(resultAssinnIssues))
            {
                List<IssueMangementModel> Assigntome = JsonConvert.DeserializeObject<List<IssueMangementModel>>(resultAssinnIssues);
                grdAssignToMe.DataSource = Assigntome;
                lblAssignIssueCount.Text = Convert.ToString(Assigntome.Count);
            }
            else
            {
                grdAssignToMe.DataSource = new DataTable();
                lblAssignIssueCount.Text = "0";
            }
        }

        private void BindTRaiseToMe(string searchtxt)
        {
            string resultRaisedByMe = commonFunctions.RestServiceCall(string.Format(Constants.ISSUE_MGT_GET_ASSSIGN_BY_ME, searchtxt.Replace("All", "")), string.Empty);
            if (!string.IsNullOrEmpty(resultRaisedByMe))
            {
            List<IssueMangementModel> RaisedByMe = JsonConvert.DeserializeObject<List<IssueMangementModel>>(resultRaisedByMe);
            grdRaisedByMe.DataSource = RaisedByMe;
             lblRaisedByMeCount.Text= Convert.ToString(RaisedByMe.Count);
            }
            else
            {
                grdRaisedByMe.DataSource = new DataTable();
                lblRaisedByMeCount.Text = "";
            }

        }

       

        public class IssueMangementModel
        {
            public int IssueId { get; set; }
            public string IssueDate { get; set; }
            public string Description { get; set; }
            public string CategoryName { get; set; }
            public string ExpectedClosureDate { get; set; }
            public string UserName { get; set; }
            public string IssueStatus { get; set; }
        }
     
        protected void grdAssignToMe_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //var test = drpSearchCategorys.SelectedValue.ToString();

            BindAssignToMe("");
        }

        protected void grdRaisedByMe_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindTRaiseToMe("");
        }


        protected void grdAllIssue_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindALlIssues("");
        }

        protected void grdAssignToMe_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                HyperLink editLink = (HyperLink)e.Item.FindControl("ViewHistory");
                editLink.Attributes["href"] = "javascript:void(0);";
                editLink.Attributes["onclick"] = String.Format("return ShowAssignHistory('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IssueId"], e.Item.ItemIndex);
            }
        }
        protected void grdRaisedByMe_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {

                HyperLink editLink = (HyperLink)e.Item.FindControl("ViewHistory");
                editLink.Attributes["href"] = "javascript:void(0);";
                editLink.Attributes["onclick"] = String.Format("return ShowAssignHistory('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IssueId"], e.Item.ItemIndex);

           }
        }
        protected void grdAllIssue_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                HyperLink editLink = (HyperLink)e.Item.FindControl("ViewHistory");
                editLink.Attributes["href"] = "javascript:void(0);";
                editLink.Attributes["onclick"] = String.Format("return ShowAssignHistory('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IssueId"], e.Item.ItemIndex);
            }
        }

        //protected void drpSearchCategorys_SelectedIndexChanged(object sender, EventArgs e)
        //{          
            //BindAssignToMe(Convert.ToString(drpSearchCategorys.SelectedItem.Text));
            //grdAssignToMe.DataBind();

            //BindTRaiseToMe(Convert.ToString(drpSearchCategorys.SelectedItem.Text));
            //grdRaisedByMe.DataBind();

            //if (ProfileName.Value == "ShowAllIssue")
            //{
            //    BindALlIssues(Convert.ToString(drpSearchCategorys.SelectedItem.Text));
            //    grdAllIssue.DataBind();
            //}
        //}

        protected void grdAssignToMe_EditCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            if (editableItem != null)
            {
                Session["IssueId"] = Convert.ToInt32(editableItem.GetDataKeyValue("IssueId"));
                Session["EditBy"] = "AssignToMe";
                Response.Redirect("~/IssueManagementDetail.aspx");
            }
        }

        protected void grdRaisedByMe_EditCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            if (editableItem != null)
            {
                Session["IssueId"] = Convert.ToInt32(editableItem.GetDataKeyValue("IssueId"));
                Session["EditBy"] = "RaisedByMe";
                Response.Redirect("~/IssueManagementDetail.aspx");
            }
        }
        protected void grdAllIssue_EditCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            if (editableItem != null)
            {
                Session["IssueId"] = Convert.ToInt32(editableItem.GetDataKeyValue("IssueId"));
                Session["EditBy"] = "AllUser";
                Response.Redirect("~/IssueManagementDetail.aspx");
            }
        }
        protected void btnAddNewIssue_Click(object sender, EventArgs e)
        {
            Session["IssueId"] = 0;
            Response.Redirect("~/IssueManagementDetail.aspx");
        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    BindAssignToMe(Convert.ToString(drpSearchCategorys.SelectedItem.Text));
        //    grdAssignToMe.DataBind();

        //    BindTRaiseToMe(Convert.ToString(drpSearchCategorys.SelectedItem.Text));
        //    grdRaisedByMe.DataBind();

        //    if (ProfileName.Value == "ShowAllIssue")
        //    {
        //        BindALlIssues(Convert.ToString(drpSearchCategorys.SelectedItem.Text));
        //        grdAllIssue.DataBind();
        //    }
        //}

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("AssignHistory"))
            {
                try
                {
                    string values = e.Argument;
                    string[] parameters = values.Split('#');
                    ViewState["issueId"] = parameters[1];
                    bindAssignHitory(parameters[1]);
                    grdIssueHistory.CurrentPageIndex = 0;
                    grdIssueHistory.DataBind();
                   
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                }
            }
        }

        private void bindAssignHitory(string IssueId)
        {
            int issueid = Convert.ToInt32(IssueId);
            string resultAssignHistory = commonFunctions.RestServiceCall(string.Format(Constants.ISSUE_MGT_GET_ASSIGNHISTORY, issueid), string.Empty);
            if (!string.IsNullOrEmpty(resultAssignHistory))
            {
                List<IssueAssignHistory> AssigntoHistory = JsonConvert.DeserializeObject<List<IssueAssignHistory>>(resultAssignHistory);
                grdIssueHistory.DataSource = AssigntoHistory;
            }
            else
                grdIssueHistory.DataSource = new DataTable();

        }



        protected void grdIssueHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if(!string.IsNullOrEmpty(Convert.ToString(ViewState["issueId"])))
            bindAssignHitory(Convert.ToString(ViewState["issueId"]));
           
        }

        protected void grdAssignToMe_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                DateTime issueDate = Convert.ToDateTime(dataItem["IssueDate"].Text);
                dataItem["IssueDate"].Text = String.Format("{0:dd-MMM-yyyy}", issueDate);

                DateTime closureDate = Convert.ToDateTime(dataItem["ExpectedClosureDate"].Text);
                dataItem["ExpectedClosureDate"].Text = String.Format("{0:dd-MMM-yyyy}", closureDate);  

            }
        }

        protected void grdRaisedByMe_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                DateTime issueDate = Convert.ToDateTime(dataItem["IssueDate"].Text);
                dataItem["IssueDate"].Text = String.Format("{0:dd-MMM-yyyy}", issueDate);

                DateTime closureDate = Convert.ToDateTime(dataItem["ExpectedClosureDate"].Text);
                dataItem["ExpectedClosureDate"].Text = String.Format("{0:dd-MMM-yyyy}", closureDate);

            }
        }

        protected void grdAllIssue_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                DateTime issueDate = Convert.ToDateTime(dataItem["IssueDate"].Text);
                dataItem["IssueDate"].Text = String.Format("{0:dd-MMM-yyyy}", issueDate);

                DateTime closureDate = Convert.ToDateTime(dataItem["ExpectedClosureDate"].Text);
                dataItem["ExpectedClosureDate"].Text = String.Format("{0:dd-MMM-yyyy}", closureDate);

            }
        }

        
    }
    public class IssueAssignHistory
    {
       public string name { get; set; }
        // public DateTime assignedDate { get; set; }
        public Nullable<System.DateTime> assignedDate { get; set; }

    }

       
}