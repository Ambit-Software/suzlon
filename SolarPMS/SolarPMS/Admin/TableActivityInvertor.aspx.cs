﻿using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolarPMS.Admin
{
    public partial class TableActivityInvertor : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSiteData();
            }
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                Models.TableActivity tableactivity = new Models.TableActivity()
                {
                    Site = (Convert.ToString(drpSite.SelectedValue.Trim())),
                    ProjectId = (Convert.ToString(drpProject.SelectedValue.Trim())),
                    AreaId = (Convert.ToInt32(drpArea.SelectedValue.Trim())),
                    NetworkId = (Convert.ToString(drpNetwork.SelectedValue.Trim())),
                    ActivityId = (Convert.ToString(drpActivity.SelectedValue.Trim())),
                    SubActivityId = (Convert.ToString(drpSubActivity.SelectedValue.Trim())),
                    Flag = "Inverter",
                    Number = Convert.ToInt32(ddlInvertorNo.Text),
                    Quantity = Convert.ToInt32(txtQuantity.Text)

                };

                string jsonInputParameter = JsonConvert.SerializeObject(tableactivity);
                string result1 = string.Empty;

                result1 = commonFunctions.RestServiceCall(Constants.TABLEACTIVITY_EXIST, Crypto.Instance.Encrypt(jsonInputParameter));
                bool isExist = Convert.ToBoolean(result1);
                
                if (isExist)
                {
                    radMesaage.Title = "Alert";
                    radMesaage.Show(Constants.TABLEACTIVITY_EXISTINVERTOR);
                    return;
                }
                result1 = commonFunctions.RestServiceCall(Constants.TABLEACTIVITY_ADD, Crypto.Instance.Encrypt(jsonInputParameter));

                if (string.Compare(result1, Constants.REST_CALL_FAILURE, true) == 0)
                {
                    radMesaage.Title = "Alert";
                    radMesaage.Show(Constants.ERROR_OCCURED_WHILE_SAVING);

                }
                else
                {
                    radMesaage.Title = "Success";
                    radMesaage.Show(Constants.INVERTOR_SAVED);
                    ResetControls();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);

            }

        }

        private void ResetControls()
        {
            drpSite.ClearSelection();
            drpProject.ClearSelection();
            drpArea.ClearSelection();
            drpNetwork.ClearSelection();
            drpActivity.ClearSelection();
            drpSubActivity.ClearSelection();
            ddlInvertorNo.ClearSelection();
            txtQuantity.Text = string.Empty;
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/TableActivity.aspx");
        }
        protected void drpSite_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindProjectData();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpProject_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindAreas();
                BindInvertorNumberData();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpArea_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindNetworks();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpNetwork_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindActivityData();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpActivity_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindSubActivityData();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion

        #region"Private Methods"
        private void BindSiteData()
        {
            drpSite.DataTextField = "Value";
            drpSite.DataValueField = "Id";
            drpSite.DataSource = TableActivityModel.GetTablemasterDataForDropdown("Inverter", "Site");
            drpSite.DataBind();
        }

        private void BindProjectData()
        {
            if (!string.IsNullOrEmpty(drpSite.SelectedValue))
            {
                drpProject.ClearSelection();
                drpProject.DataTextField = "Value";
                drpProject.DataValueField = "Id";
                drpProject.DataSource = TableActivityModel.GetTablemasterDataForDropdown("Inverter", "Project", Convert.ToString(drpSite.SelectedValue));
                drpProject.DataBind();
                drpArea.ClearSelection();
                drpNetwork.ClearSelection();
                drpActivity.ClearSelection();
                drpSubActivity.ClearSelection();
                ddlInvertorNo.ClearSelection();
            }
        }

        private void BindAreas()
        {
            if (!string.IsNullOrEmpty(drpProject.SelectedValue))
            {
                drpArea.ClearSelection();
                drpArea.DataTextField = "Value";
                drpArea.DataValueField = "Id";
                drpArea.DataSource = TableActivityModel.GetTablemasterDataForDropdown("Inverter", "Area",
                                        Convert.ToString(drpSite.SelectedValue), Convert.ToString(drpProject.SelectedValue)); ;
                drpArea.DataBind();
                
                drpNetwork.ClearSelection();
                drpActivity.ClearSelection();
                drpSubActivity.ClearSelection();
                ddlInvertorNo.ClearSelection();
            }
        }

        private void BindNetworks()
        {
            if (!string.IsNullOrEmpty(drpArea.SelectedValue))
            {
                drpNetwork.ClearSelection();
                drpNetwork.DataTextField = "Value";
                drpNetwork.DataValueField = "Id";
                drpNetwork.DataSource = TableActivityModel.GetTablemasterDataForDropdown("Inverter", "Network", Convert.ToString(drpSite.SelectedValue), Convert.ToString(drpProject.SelectedValue), drpArea.SelectedValue);
                drpNetwork.DataBind();
                
                drpActivity.ClearSelection();
                drpSubActivity.ClearSelection();
            }
        }

        private void BindActivityData()
        {
            if (!string.IsNullOrEmpty(drpNetwork.SelectedValue))
            {
                drpActivity.ClearSelection();
                drpActivity.DataTextField = "Value";
                drpActivity.DataValueField = "Id";
                drpActivity.DataSource = TableActivityModel.GetTablemasterDataForDropdown("Inverter", "Activity", Convert.ToString(drpSite.SelectedValue), Convert.ToString(drpProject.SelectedValue), drpArea.SelectedValue, drpNetwork.SelectedValue);
                drpActivity.DataBind();                
                drpSubActivity.ClearSelection();
            }
        }

        private void BindSubActivityData()
        {
            if (!string.IsNullOrEmpty(drpActivity.SelectedValue))
            {
                string result1 = commonFunctions.RestServiceCall(string.Format(Constants.TABLEACTIVITY_GETSUBACTIVITY, drpProject.SelectedValue.ToString(), Server.UrlEncode(drpArea.SelectedItem.Text.Trim()), drpNetwork.SelectedValue.ToString(), drpActivity.SelectedValue.ToString()), string.Empty);
                DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result1);
                drpSubActivity.DataTextField = "Value";
                drpSubActivity.DataValueField = "Id";
                drpSubActivity.DataSource = TableActivityModel.GetTablemasterDataForDropdown("Inverter", "SubActivity", Convert.ToString(drpSite.SelectedValue), Convert.ToString(drpProject.SelectedValue), drpArea.SelectedValue, drpNetwork.SelectedValue, drpActivity.SelectedValue);
                drpSubActivity.DataBind();
                drpSubActivity.SelectedIndex = -1;
            }
        }

        private void BindInvertorNumberData()
        {
            ddlInvertorNo.DataTextField = "Value";
            ddlInvertorNo.DataValueField = "Id";
            ddlInvertorNo.DataSource = TableActivityModel.GetTablemasterDataForDropdown("Inverter", "Number", drpSite.SelectedValue.Trim(), drpProject.SelectedValue.Trim());
            ddlInvertorNo.DataBind();
            ddlInvertorNo.SelectedIndex = -1;
        }

        #endregion
    }
}