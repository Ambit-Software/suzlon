using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class VendorMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();

        #region "Events"

        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.VENDOR_MASTER, menuList)) Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {
                bindVendorGrid();
                grdVendor.DataBind();
            }
        }

        #endregion "Events"

        #region "Private Methods"

        /// <summary>
        /// Get data to Bind Vendor Grid
        /// </summary>
        /// <returns></returns>
        private void bindVendorGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.Vendor_GET_ALL, string.Empty);
                List<Models.VendorMaster> lstVendor = JsonConvert.DeserializeObject<List<Models.VendorMaster>>(result);
                grdVendor.DataSource = lstVendor;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion "Private Methods"

        protected void grdVendor_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindVendorGrid();
        }
    }
}