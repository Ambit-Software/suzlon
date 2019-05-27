using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;

namespace SuzlonBPP
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
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All;
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/jquery-1.10.2.min.js"
                }
            );
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            //if ((Session["Token"] == null || Session["UserId"] == null) && (!Request.Url.AbsoluteUri.Contains("ResetPassword.aspx") && !Request.Url.AbsoluteUri.Contains("ForgotPassword.aspx")))
            //    Response.Redirect("Login.aspx");

            if ((Session["Token"] == null || Session["UserId"] == null) && (!Request.Url.AbsoluteUri.Contains("ResetPassword.aspx") && !Request.Url.AbsoluteUri.Contains("ForgotPassword.aspx")))
            {
                if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
                    Response.Redirect("Login.aspx?" + Request.QueryString.ToString());
                else
                    Response.Redirect("Login.aspx");
            }
        }
    }
}