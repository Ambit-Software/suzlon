using System;
using System.Collections.Generic;
using Telerik.Web.UI;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System.Collections;
using System.Web.UI.WebControls;

namespace SuzlonBPP.Admin
{
    public partial class UserMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.USER_MANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");

                if (!IsPostBack)
                {
                    BindUserGrid();
                    GrdUser.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        public void BindUserGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.USER_GETALL, string.Empty);
                List<GetAllUserDetail_Result> LstUser = JsonConvert.DeserializeObject<List<GetAllUserDetail_Result>>(result);
                GrdUser.DataSource = LstUser;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void GrdUser_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert")
            {
                Session["EditUserId"] = "0";
                Response.Redirect("UserDetail.aspx");
            }
        }

        protected void GrdUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindUserGrid();
        }

        protected void GrdUser_EditCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    Session["EditUserId"] = Convert.ToInt32(editableItem.GetDataKeyValue("UserId"));
                    Response.Redirect("UserDetail.aspx");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Session["EditUserId"] = 0;
            Response.Redirect("UserDetail.aspx");
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //GrdUser.ExportSettings.ExportOnlyData = true;
                //GrdUser.ExportSettings.IgnorePaging = true;
                //GrdUser.ExportSettings.OpenInNewWindow = true;
                //GrdUser.ExportSettings.FileName = "UserMaster";
                //GrdUser.MasterTableView.ExportToExcel();

                GrdUser.MasterTableView.AllowFilteringByColumn = false;
                GrdUser.MasterTableView.AllowSorting = false;
                GrdUser.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                GrdUser.ExportSettings.FileName = "UserMaster";
                GrdUser.Rebind();
                GrdUser.MasterTableView.ExportToExcel();

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void GrdUser_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                //bool status = Convert.ToBoolean(dataItem["Status"].Text);
                //dataItem["Status"].Text = status==true?"Enabled": "Disabled";
                Label lblStatus = (Label)dataItem.FindControl("lblStatus");
                string text = lblStatus.Text;
                lblStatus.Text = text == "True" ? "Enabled" : "Disabled";

            }

        }
    }
}