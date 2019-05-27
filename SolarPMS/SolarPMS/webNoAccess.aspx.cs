using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolarPMS
{
    public partial class webNoAccess : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                if (Request.UrlReferrer.ToString().Contains("DesignDocumentUpload.aspx") || Request.UrlReferrer.ToString().Contains("ToDoList.aspx"))
                {
                    this.MasterPageFile = "~/MasterPages/BlankMaster.Master";
                    //BtnCancel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}