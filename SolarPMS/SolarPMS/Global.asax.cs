using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace SolarPMS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
                new ScriptResourceDefinition
                {
                    Path = "/~Scripts/jquery-1.10.2.min.js"
                }
            );

        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            if ((Session["Token"] == null || Session["UserId"] == null) && (!Request.Url.AbsoluteUri.Contains("ResetPassword.aspx") && !Request.Url.AbsoluteUri.Contains("ForgotPassword.aspx")))
                Response.Redirect("~/Login.aspx");
        }
       
    }
}
