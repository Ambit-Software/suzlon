using Cryptography;
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
    public partial class CompanyMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();

        #region "Events"

        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);
            if (!PageSecurity.IsAccessGranted(PageSecurity.COMPANY_MASTER, menuList)) Response.Redirect("~/webNoAccess.aspx");

            if (!IsPostBack)
            {
                bindCompanyGrid();
                grdCompany.DataBind();
            }
        }

        #endregion "Events"

        #region "Private Methods"

        /// <summary>
        /// Get data to Bind Company Grid
        /// </summary>
        /// <returns></returns>
        private void bindCompanyGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.Company_GET_ALL, string.Empty);
                List<Models.CompanyMaster> lstCompany = JsonConvert.DeserializeObject<List<Models.CompanyMaster>>(result);
                grdCompany.DataSource = lstCompany;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion "Private Methods"

        protected void grdCompany_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindCompanyGrid();
        }
    }
}