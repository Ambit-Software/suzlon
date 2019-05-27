using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolarPMS.MasterPages
{
    public partial class BlankMaster : System.Web.UI.MasterPage
    {
        public string ApplicationPath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //ApplicationPath = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"].ToString();
                Constants.ApplicationPath = "http://" + Request.Url.Authority + "/" + Request.ApplicationPath + "/";
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}