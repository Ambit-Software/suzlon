using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SuzlonBPP
{
    public partial class WelcomeScreen : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        List<CommentDtl> CommentLstAdd = new List<CommentDtl>();
        List<CommentDtl> CommentLstShow = new List<CommentDtl>();


        string AttachmentChq = string.Empty;
        string AttachmentCert = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable menuList = (Hashtable)Session["MenuSecurity"];
            if (menuList == null) Response.Redirect("~/Login.aspx", false);            
            if (!IsPostBack)
            {

            }
        }
    }
}