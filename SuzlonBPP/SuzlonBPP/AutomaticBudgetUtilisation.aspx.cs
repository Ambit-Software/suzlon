using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SuzlonBPP.Controllers;

namespace SuzlonBPP
{
    public partial class AutomaticBudgetUtilisation : System.Web.UI.Page
    {
        PaymentWorkflowController paymentWorkflowController = new PaymentWorkflowController();

        protected void Page_Load(object sender, EventArgs e)
        {
            rgridAutomaticUtilisation.DataBind();
        }

        protected void rgridAutomaticUtilisation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgridAutomaticUtilisation.DataSource = paymentWorkflowController.getBalanceDetailsByTreasury(Convert.ToInt32(Session["TreasuryId"]));
        }
    }

    
}