using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SuzlonBPP
{
    public partial class VendorBankMaster : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Hashtable menuList = (Hashtable)Session["MenuSecurity"];
                if (menuList == null) Response.Redirect("~/Login.aspx", false);
                if (!PageSecurity.IsAccessGranted(PageSecurity.VENDOR_BANK_MASTER, menuList)) Response.Redirect("~/webNoAccess.aspx");

                if (!IsPostBack)
                {
                    bindVendorGrid();
                    grdVendor.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Get data to bind vendor details to Grid.
        /// </summary>
        /// <returns></returns>
        private void bindVendorGrid()
        {
            try
            {
                string result = commonFunctions.RestServiceCall(Constants.Vendor_BANK_GET_ALL, string.Empty);
                if (result != Constants.REST_CALL_FAILURE)
                {
                    List<Models.VendorBankMaster> lstVendor = JsonConvert.DeserializeObject<List<Models.VendorBankMaster>>(result);
                    grdVendor.DataSource = lstVendor;
                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    grdVendor.DataSource = new System.Data.DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }


        protected void grdVendor_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bindVendorGrid();
        }
    }
}