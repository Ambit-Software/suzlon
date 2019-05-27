using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SolarPMS
{
    public partial class ContractorManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                string queryString = "Id=0";
                queryString = Cryptography.Crypto.Instance.Encrypt(queryString);
                Response.Redirect(HttpUtility.UrlDecode("~/AddManpowerDetails.aspx?" + queryString));
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPowerDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                gridManPowerDetails.DataSource = ManPowerModel.GetContractorDetailsList(Convert.ToInt32(Session["UserId"]));
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPowerDetails_EditCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                if (editableItem != null)
                {
                    string site  = Convert.ToString(editableItem.GetDataKeyValue("Site"));
                    string project = Convert.ToString(editableItem.GetDataKeyValue("Project"));
                    int area = Convert.ToInt32(editableItem.GetDataKeyValue("AreaId"));
                    string network = Convert.ToString(editableItem.GetDataKeyValue("Network"));
                    DateTime date = Convert.ToDateTime(editableItem.GetDataKeyValue("Date"));
                    string queryString = "Site=" + site.Trim() + "&Project=" + project.Trim() + "&AreaId=" + area + "&Network=" + network.Trim() + "&Date=" + date;
                    queryString = Cryptography.Crypto.Instance.Encrypt(queryString);
                    Response.Redirect("~/AddManpowerDetails.aspx?" + HttpUtility.UrlEncode(queryString), true);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridManPowerDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                GridDataItem item = e.Item as GridDataItem;
                if (item != null)
                {
                    ManPowerMasterDetails obj = item.DataItem as ManPowerMasterDetails;
                    if (Convert.ToInt32(obj.CanEdit) == 1)
                        (item["EditColumn"].Controls[0]).Visible = true;
                    else
                        (item["EditColumn"].Controls[0]).Visible = false;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }
    }
}