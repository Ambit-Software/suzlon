using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Newtonsoft.Json;
using Cryptography;
using SolarPMS.Models;
using System.Collections;
using System.Data;

namespace SolarPMS.Admin
{
    public partial class UserMaster : Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);

            if (!PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList)) Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {
                BindUserGrid();
                GrdUser.DataBind();
            }
        }
        public void BindUserGrid()
        {
            string result = commonFunctions.RestServiceCall(Constants.USER_GETALL, string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                List<Models.UserModel> LstUser = JsonConvert.DeserializeObject<List<Models.UserModel>>(result);
                GrdUser.DataSource = LstUser;
            }
            else
            {
                GrdUser.DataSource = new DataTable();
            }
        }

        protected void GrdUser_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert")
            {
                Session["EditUserId"] = "0";
                Response.Redirect("~/Admin/UserDetail.aspx");
            }
        }

        protected void GrdUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindUserGrid();
        }

        protected void GrdUser_EditCommand(object sender, GridCommandEventArgs e)
        {   
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            if(editableItem!=null)
            { 
            Session["EditUserId"]= Convert.ToInt32(editableItem.GetDataKeyValue("UserDetail.userId"));
             Response.Redirect("~/Admin/UserDetail.aspx");
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Session["EditUserId"] = "0";
            Response.Redirect("~/Admin/UserDetail.aspx");
        }

        protected void GrdUser_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                bool status = Convert.ToBoolean(dataItem["Status"].Text);
                dataItem["Status"].Text = status == true ? "Enabled" : "Disabled";

            }
        }
    }
}
