using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuzlonBPP
{
    public partial class Blank : System.Web.UI.MasterPage
    {
        public string applicationPath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            applicationPath = ConfigurationManager.AppSettings["WebsiteUrl"].ToString();
        }
    }
}