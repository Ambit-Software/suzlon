using Cryptography;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolarPMS.Models;

namespace SolarPMS.MasterPages
{
    public partial class SolarPMS : System.Web.UI.MasterPage
    {
        //public string ApplicationPath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Constants.ApplicationPath = "http://" + Request.Url.Authority + "/" + Request.ApplicationPath + "/"; //System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"].ToString();
                if (!IsPostBack)
                {
                    SetMenuSecurity();
                    lblUserName.Text = Session["LoginUserName"] != null ? "Welcome " + Session["LoginUserName"].ToString() : string.Empty;
                    impProfile.Src = Session["PhotpPath"] != null ? Session["PhotpPath"].ToString() : "../Content/images/profile.jpg";

                    List<Models.Report> lstReport = ReportsModel.GetReportList();
                    foreach (Models.Report report in lstReport)
                    {
                        string url ="../ReportList.aspx?"
                            + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("ReportId=" + report.Id + "&Filename=" + report.ReportFilename + "&ReportName=" + report.Name));
                        reportList.InnerHtml += "<li><a href='" + url + "'>" + report.Name + " </a ></ li > ";
                    }

                    if (Session[Constants.CONST_SESSION_DEDOCUMENT_ACEESS] != null)
                    {
                        int documentAccess = Convert.ToInt32(Session[Constants.CONST_SESSION_DEDOCUMENT_ACEESS]);
                        if (documentAccess == Convert.ToInt32(Constants.DEDocumentUploadAccess.View)
                            || documentAccess == Convert.ToInt32(Constants.DEDocumentUploadAccess.Full))
                            menuDesignDocumentUpload.Visible = true;
                        else
                            menuDesignDocumentUpload.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void lnlLogout_Click(object sender, EventArgs e)
        {
            try
            {
                Logout();
            }
            catch (Exception ex)
            { }
        }

        private void Logout()
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx", false);
        }

        private void SetMenuSecurity()
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);

            if (PageSecurity.IsAccessGranted(PageSecurity.DASHBOARD, menuList))
                menuDashboard.Visible = true;
            else
                menuDashboard.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.TODOTASK, menuList))
                menuToDoTask.Visible = true;
            else
                menuToDoTask.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.ISSUEMANAGEMENT, menuList))
                menuIssueManagement.Visible = true;
            else
                menuIssueManagement.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.USERMANAGEMENT, menuList))
                menuUserManagement.Visible = true;
            else
                menuUserManagement.Visible = false;

            if (PageSecurity.IsAccessGranted(PageSecurity.MANPOWER, menuList))
                menuContractManagement.Visible = true;
            else
                menuContractManagement.Visible = false;
        }
    }
}