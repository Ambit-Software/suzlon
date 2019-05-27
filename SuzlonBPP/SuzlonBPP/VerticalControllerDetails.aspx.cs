using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using SuzlonBPP.Models;
using Newtonsoft.Json;
using Telerik.Web.Data.Extensions;
using Cryptography;
using System.Text;

namespace SuzlonBPP
{

    public partial class VerticalControllerDetails : System.Web.UI.Page
    {
        public DataTable dt;
        String Mode = String.Empty;
        CommonFunctions commonFunctions = new CommonFunctions();
        Int32 profileId = 0;
        List<CommentDtl> CommentLstShow = new List<CommentDtl>();
        List<CommentDtl> CommentLstAdd = new List<CommentDtl>();
        protected void Page_Load(object sender, EventArgs e)
        {

            //Session["ProfileId"] = 2; // 2:VC // 4:TR // 7:SB
            profileId = Convert.ToInt32(Session["ProfileId"]);
            BudgetPopup.VisibleOnPageLoad = false;

            if (!IsPostBack)
            {
                // Session["TreasuryId"] = "23";
                // Mode = (String.IsNullOrEmpty(Convert.ToString(Session["TreasuryId"]))) ? "Insert" : "Update";
                Mode = (Convert.ToString(Session["TreasuryId"]) == "0") ? "Insert" : "Update";
                rdpCreatedDate.SelectedDate = DateTime.Now; //if mutilple values, move to function
                bindDropdow();

                if ((String.Compare(Mode, "update", true) == 0))
                {
                    bindVerticalControllerDetail();
                    if (profileId == (int)UserProfileEnum.CB)
                    {
                        bindBudgetUtilisationDetail(Convert.ToInt32(Session["TreasuryId"]));
                        rgridManualUtilisation.DataBind();
                    }
                }
                else
                {
                    rdpPayDate.MinDate = DateTime.Today.Date;
                    EnableDisableFormControls((TreasuryWorkFlowStatusEnum)1);                  

                }
            }
        }


        public void   bindBudgetUtilisationDetail(int TreasuryDetailId)
        {

            try
            {
                string result = commonFunctions.RestServiceCall(string.Format(Constants.GETBUDGETUTILISATION_DETAIL, TreasuryDetailId), string.Empty);
                if (result != Constants.REST_CALL_FAILURE)
                {
                    List<TreasuryBudgetUtilisationModel> lstBudgetUtilisation = JsonConvert.DeserializeObject<List<TreasuryBudgetUtilisationModel>>(result);
                    rgridManualUtilisation.DataSource = lstBudgetUtilisation;
                }
                else
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                    rgridManualUtilisation.DataSource = new System.Data.DataTable();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        private DataTable AddRow(DataTable dt)
        {
            // method to create row        
            DataRow dr = dt.NewRow();

            dr["NatureOfRequest"] = "";
            dr["Amount"] = 3.0;
            dr["ApprovedAmount"] = 5.5;
            dt.Rows.Add(dr);
            return dt;
        }

        protected void gridInverter_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

        }

        protected void btnAddRange_Click(object sender, EventArgs e)
        {
            try
            {
                rgridManualUtilisation.MasterTableView.ClearEditItems();
                rgridManualUtilisation.MasterTableView.IsItemInserted = true;
                rgridManualUtilisation.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
            }

        }

        protected void gridInverter_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {


        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {

        }

        private void bindDropdow()
        {
            try
            {
                string vendorCode = string.Empty;
                string drpname = "subvertical,company,nature-of-request,workflow-status,";
                string commonDrpValues = commonFunctions.RestServiceCall(Constants.USERDETAIL_GETDROPDOWNVALUE + drpname + "", string.Empty);
                if (commonDrpValues == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(commonDrpValues);

                    drpSubVertical.DataSource = ddValues.SubVertical;
                    drpSubVertical.DataBind();

                    radcomboCmpy.DataSource = ddValues.Company;
                    radcomboCmpy.DataBind();

                    rcbStatus.DataSource = ddValues.WorkFlowStatus;
                    rcbStatus.DataBind();

                    ViewState["ddNOR"] = (ddValues.NatureOfRequest);
                }



            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void drpSubVertical_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(drpSubVertical.SelectedValue))
                GetVerticals(drpSubVertical.SelectedValue);
        }

        protected void GetVerticals(string subVerticalId)
        {
            try
            {
                var result = commonFunctions.RestServiceCall(string.Format(Constants.VENDOR_BANK_GETVERTICAl_BYSUBVERTICAL, subVerticalId), string.Empty);

                if (result == Constants.REST_CALL_FAILURE)
                {
                    drpVertical.DataSource = null;
                    drpVertical.DataBind();
                    drpVertical.Items.Insert(0, new RadComboBoxItem("Select Vertical", String.Empty));
                    drpVertical.SelectedIndex = 0;
                }
                else
                {
                    DropdownValues ddValues = JsonConvert.DeserializeObject<DropdownValues>(result);
                    drpVertical.DataTextField = "Name";
                    drpVertical.DataValueField = "Id";
                    drpVertical.DataSource = ddValues.Vertical;
                    drpVertical.DataBind();
                    if (drpVertical.Items.Count > 0) drpVertical.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }


        }

        protected void GetNaturOfRequest()
        {
            if ((DataTable)(ViewState["NOR"]) == null)
            {
                gridNatureOfRequest.DataSource = new DataTable();
            }
            else
            {
                gridNatureOfRequest.DataSource = (DataTable)(ViewState["NOR"]);
            }
        }

        protected void OnItemDataBoundHandler(object sender, GridItemEventArgs e)
        {
            RadLabel RlblAmount = new RadLabel();
            if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                EnableDisableGridControls(item);
            }
            else if (!(e.Item is IGridInsertItem) && (e.Item.IsInEditMode))
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                RadComboBox RadCmbNatureOfRequest = (RadComboBox)item.FindControl("RadComboNatureOfRequest");
                RadNumericTextBox RNumReqAmount = (RadNumericTextBox)item.FindControl("RadNumAmount");
                RadNumericTextBox RNumAppAmount = (RadNumericTextBox)item.FindControl("RadNumApprovedAmount");

                EnableDisableGridControls(item);

                RadCmbNatureOfRequest.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "NatureOfRequest").ToString();
                RNumReqAmount.Text = (string)DataBinder.Eval(e.Item.DataItem, "Amount").ToString();
                RNumAppAmount.Text = (string)DataBinder.Eval(e.Item.DataItem, "ApprovedAmount").ToString();

                
            }
            else if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                RadComboBox PageSizeComboBox = (RadComboBox)pager.FindControl("PageSizeComboBox");
                PageSizeComboBox.Visible = false;
            }
        }

        protected void RadGvScope_PreRender(object sender, System.EventArgs e) // Remove if Not Required
        {
            try
            {
                if (!IsPostBack)
                {
                   
                    gridNatureOfRequest.Rebind();
                }
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridNatureOfRequest_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                RadComboBox RadComboNatureOfRequest = new RadComboBox();
                RadNumericTextBox RadNumAmount = new RadNumericTextBox();
                RadNumericTextBox RadNumApprovedAmount = new RadNumericTextBox();

                gridNatureOfRequest.MasterTableView.ClearEditItems();
                e.Item.Edit = false;

                foreach (GridDataItem item in gridNatureOfRequest.MasterTableView.Items)
                {
                    if (item.FindControl("RadComboNatureOfRequest") != null)
                        RadComboNatureOfRequest = item.FindControl("RadComboNatureOfRequest") as RadComboBox;

                    if (item.FindControl("RadNumAmount") != null)
                        RadNumAmount = item.FindControl("RadNumAmount") as RadNumericTextBox;

                    if (item.FindControl("RadNumApprovedAmount") != null)
                        RadNumApprovedAmount = item.FindControl("RadNumApprovedAmount") as RadNumericTextBox;
                }

                DataTable dtX = (DataTable)ViewState["NOR"];
                int curXOffset = (e.Item.RowIndex - 2) + (gridNatureOfRequest.CurrentPageIndex * gridNatureOfRequest.PageSize);
                dtX.Rows[curXOffset]["IsDirty"] = "X";
                ViewState["NOR"] = dtX;

                if (IsValidRequest(Convert.ToString(RadComboNatureOfRequest.SelectedValue),
                   Convert.ToDecimal(RadNumAmount.Text),
                   (String.IsNullOrEmpty(Convert.ToString(RadNumApprovedAmount.Text).Trim()) ? 0 : Convert.ToDecimal(RadNumApprovedAmount.Text)), "update"))
                {
                    DataTable dt = (DataTable)ViewState["NOR"];
                    int curOffset = (e.Item.RowIndex - 2) + (gridNatureOfRequest.CurrentPageIndex * gridNatureOfRequest.PageSize);
                    dt.Rows[curOffset]["NatureOfRequest"] = RadComboNatureOfRequest.SelectedValue;
                    dt.Rows[curOffset]["NatureOfRequestText"] = RadComboNatureOfRequest.Text;
                    dt.Rows[curOffset]["Amount"] = RadNumAmount.Text;
                    dt.Rows[curOffset]["ApprovedAmount"] = (String.IsNullOrEmpty(Convert.ToString(RadNumApprovedAmount.Text).Trim()) ? "0" : Convert.ToString(RadNumApprovedAmount.Text).Trim());
                    dt.Rows[curOffset]["IsDirty"] = "T";
                    ViewState["NOR"] = dt;
                }
                else
                {
                    gridNatureOfRequest.MasterTableView.ClearEditItems();
                    e.Item.Edit = true;
                    radMessage.Show(Constants.RECORD_EXIST_MESSAGE);
                    return;
                }

                CalculateAmount();
                GetNaturOfRequest();
                gridNatureOfRequest.DataBind();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridNatureOfRequest_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                String strCommand = ((System.Web.UI.WebControls.LinkButton)((e.CommandSource))).Text;
                switch (strCommand)
                {
                    case "Cancel":
                        gridNatureOfRequest.Rebind();
                        break;
                    case "Edit":
                        break;
                    case "Delete":
                        DataTable dt = (DataTable)ViewState["NOR"];
                        dt.Rows.RemoveAt(((GridDataItem)e.Item).ItemIndex + (gridNatureOfRequest.CurrentPageIndex * gridNatureOfRequest.PageSize));
                        CalculateAmount();
                        break;
                }

            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void gridNatureOfRequest_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataInsertItem item = (GridDataInsertItem)gridNatureOfRequest.MasterTableView.GetInsertItem();
                RadComboBox RadComboNatureOfRequest = new RadComboBox();
                RadNumericTextBox RadNumAmount = new RadNumericTextBox();
                RadNumericTextBox RadNumApprovedAmount = new RadNumericTextBox();

                if ((RadComboBox)item.FindControl("RadComboNatureOfRequest") != null)
                    RadComboNatureOfRequest = (RadComboBox)item.FindControl("RadComboNatureOfRequest") as RadComboBox;

                if (item.FindControl("RadNumAmount") != null)
                    RadNumAmount = item.FindControl("RadNumAmount") as RadNumericTextBox;

                if (item.FindControl("RadNumApprovedAmount") != null)
                    RadNumApprovedAmount = item.FindControl("RadNumApprovedAmount") as RadNumericTextBox;

                DataTable dt = new DataTable();
                if (ViewState["NOR"] != null)
                {
                    dt = (DataTable)ViewState["NOR"];
                }
                else {
                    dt = CreateDTSchema(dt);
                }

                if (IsValidRequest(Convert.ToString(RadComboNatureOfRequest.SelectedValue),
                    Convert.ToDecimal(RadNumAmount.Text),
                    (String.IsNullOrEmpty(Convert.ToString(RadNumApprovedAmount.Text).Trim()) ? 0 : Convert.ToDecimal(RadNumApprovedAmount.Text)), "insert"))
                {
                    dt.Rows.Add(new object[] {null,Convert.ToString(RadComboNatureOfRequest.SelectedValue),Convert.ToString(RadComboNatureOfRequest.Text),
                        Convert.ToString(RadNumAmount.Text),
                        (String.IsNullOrEmpty(Convert.ToString(RadNumApprovedAmount.Text).Trim()) ? "0" : Convert.ToString(RadNumApprovedAmount.Text).Trim()),
                        ((String.Compare(Mode,"Insert",true) == 0) ? "F" : "T")});
                    ViewState["NOR"] = dt;

                    e.Canceled = true;
                    gridNatureOfRequest.MasterTableView.IsItemInserted = false;
                    gridNatureOfRequest.MasterTableView.Rebind();

                }
                else
                {
                    radMessage.Show(Constants.RECORD_EXIST_MESSAGE);
                    return;
                }

                CalculateAmount();
                GetNaturOfRequest();
                gridNatureOfRequest.DataBind();

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected bool IsValidRequest(string strNature, decimal dcAmount, decimal dcAppAmount, string strMode)
        {
            DataRow[] foundRows;
            try
            {
                DataTable dt = (DataTable)ViewState["NOR"];
                if (dt == null) return true;

                //string expression = "NatureOfRequest = '" + strNature + "' AND Amount = " + dcAmount + " AND ApprovedAmount = " + dcAppAmount;
                //  string expression = " NatureOfRequest = '" + strNature + "' and IsDirty <> 0 ";
                string expression = "NatureOfRequest = '" + strNature + "' and  (IsDirty <> 'X' OR IsDirty is Null)";
                foundRows = dt.Select(expression, string.Empty);
                if (String.Compare(strMode, "insert", true) == 0)
                {
                    return (foundRows.Count() > 0) ? false : true;
                }
                else
                {
                    return (foundRows.Count() > 0) ? false : true; //?
                }


            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        protected DataTable CreateDTSchema(DataTable dt)
        {
            if (dt == null) dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("NatureOfRequest", typeof(string)));
            dt.Columns.Add(new DataColumn("NatureOfRequestText", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt.Columns.Add(new DataColumn("ApprovedAmount", typeof(decimal)));
            dt.Columns.Add(new DataColumn("IsDirty", typeof(string)));
            return dt;
        }

        protected void gridNatureOfRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GetNaturOfRequest();
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
                gridNatureOfRequest.MasterTableView.ClearEditItems();
                gridNatureOfRequest.MasterTableView.IsItemInserted = true;
                gridNatureOfRequest.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void CalculateAmount()
        {
            try
            {
                object TotalAmount = 0;
                object TotalApprovedAmount = 0;
                if ((DataTable)ViewState["NOR"] != null)
                {
                    DataTable dt = (DataTable)ViewState["NOR"];
                    if (dt.Rows.Count > 0)
                    {
                        TotalAmount = dt.Compute("Sum(Amount)", "");
                        TotalApprovedAmount = dt.Compute("Sum(ApprovedAmount)", "");
                    }
                }

                lblInitAmount.Text = Convert.ToString(TotalApprovedAmount);
                lblRequestedAmount.Text = Convert.ToString(TotalAmount);
                lblFinalAmt.Text = decimal.Round(Convert.ToDecimal(String.IsNullOrEmpty(lblAddendumAmt.Text) ? "0" : lblAddendumAmt.Text) +
                    Convert.ToDecimal(TotalApprovedAmount)
                    , 2, MidpointRounding.AwayFromZero).ToString();
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected Boolean ValidateForm()
        {
            try
            {

                if (rdpStart.SelectedDate != null && rdpEnd.SelectedDate != null)
                {
                    TimeSpan tsTreasure = (rdpEnd.SelectedDate - rdpStart.SelectedDate).Value;
                    if (tsTreasure.Days < 0)
                    {
                        radMessage.Title = "Alert";
                        radMessage.Show(Constants.TREASURE_DATE_MSG);
                        return false;
                    }
                }

                if (gridNatureOfRequest.MasterTableView.Items.Count < 1)
                {
                    radMessage.Title = "Alert";
                    radMessage.Show(Constants.TREASURE_NATURE_REQUEST_MSG);
                    return false;
                }

                if (profileId == (int)UserProfileEnum.Treasury)
                {
                    DataTable dt = (DataTable)(ViewState["NOR"]);
                    DataRow[] dr = dt.Select("ApprovedAmount = '0'");
                    if (dr.Count() > 0)
                    {
                        radMessage.Show(Constants.TREASURE_APPAMNT_MSG);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }



        protected void lbSubmit_Click(object sender, EventArgs e)
        {

            String jsonInputParameter = String.Empty;
            String serviceResult = String.Empty;
            TreasuryDetailModel saveModel;
            try
            {
                if (ValidateForm())
                {
                    saveModel = fillModel();
                    jsonInputParameter = JsonConvert.SerializeObject(saveModel);

                    if (Convert.ToInt32(Session["TreasuryId"]) == 0)
                    {
                        serviceResult = commonFunctions.RestServiceCall(Constants.TREASURE_CONTROLLER_DETAIL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                    }
                    else
                    {
                        serviceResult = commonFunctions.RestServiceCall(Constants.TREASURE_CONTROLLER_DETAIL_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    }

                    if (profileId == (int)UserProfileEnum.VerticalController)
                    {
                        Response.Redirect("TreasuryListVertical.aspx");
                    }
                    else if (profileId == (int)UserProfileEnum.Treasury || profileId == (int)UserProfileEnum.CB)
                    {
                        Response.Redirect("TreasuryList.aspx");
                    }

                }


            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void lbAddendum_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void lbSave_Click(object sender, EventArgs e)
        {
            String jsonInputParameter = String.Empty;
            String serviceResult = String.Empty;
            TreasuryDetailModel saveModel;
            try
            {
                if (ValidateForm())
                {
                    saveModel = fillModel();
                    jsonInputParameter = JsonConvert.SerializeObject(saveModel);

                    if (Convert.ToInt32(Session["TreasuryId"]) == 0)
                    {
                        serviceResult = commonFunctions.RestServiceCall(Constants.TREASURE_CONTROLLER_DETAIL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                    }
                    else
                    {
                        serviceResult = commonFunctions.RestServiceCall(Constants.TREASURE_CONTROLLER_DETAIL_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                    }

                    if (profileId == (int)UserProfileEnum.VerticalController)
                    {
                        Response.Redirect("TreasuryListVertical.aspx");
                    }
                    else if (profileId == (int)UserProfileEnum.Treasury || profileId == (int)UserProfileEnum.CB)
                    {
                        Response.Redirect("TreasuryList.aspx");
                    }
                }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected List<TreasuryRequestModel> NatureOfRequest()
        {
            DataTable dt = (DataTable)ViewState["NOR"];
            List<TreasuryRequestModel> NatureOfRequest = new List<TreasuryRequestModel>();
            NatureOfRequest = (from DataRow row in dt.Rows
                               select new TreasuryRequestModel
                               {
                                   TreasuryRequestId = Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(row["Id"])) ? 0 : row["Id"]),
                                   natureOfRequest = Convert.ToInt32(row["NatureOfRequest"]),
                                   natureOfRequestText = Convert.ToString(row["NatureOfRequestText"]),
                                   requestedAmount = Convert.ToDecimal(row["Amount"]),
                                   approvedAmount = Convert.ToDecimal(row["ApprovedAmount"]),
                                   isDirty = Convert.ToString(row["IsDirty"]),
                               }).ToList();
            return NatureOfRequest;
        }

        protected List<FileUploadModel> FileUploadList()
        {
            DataTable dt = (DataTable)Session["ATTACHMENT"];
            List<FileUploadModel> fileUpload = new List<FileUploadModel>();
            fileUpload = (from DataRow row in dt.Rows
                          select new FileUploadModel
                          {
                              //FileUploadId = Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(row["FileuploadId"])) ? 0 : row["Id"]),
                              //EntityId = Convert.ToInt32(row[""]),
                              DisplayName = Convert.ToString(row["DisplayName"]),
                              FileName = Convert.ToString(row["FileName"]),
                              EntityName = "Treasury"
                          }).ToList();
            return fileUpload;
        }

        protected void EnableDisableFormControls(TreasuryWorkFlowStatusEnum status)
        {
            try
            {
                SettingModel AppSetting = new SettingModel();
                SuzlonBPP.Models.ApplicationConfiguration AppCongif = AppSetting.GetApplSettings();

                rdpCreatedDate.Enabled = false;
                drpVertical.Enabled = false;
                if (profileId == (int)UserProfileEnum.VerticalController)
                {
                    // Draft: Enable All
                    // PendingForApproval: Disable All
                    // Approved By Treasury : Disable All
                    // Rejected By Treasury : Diable All
                    // Need Correction By Treasury : Enable All
                    if (status == TreasuryWorkFlowStatusEnum.DraftByVerticalController
                        || status == TreasuryWorkFlowStatusEnum.NeedCorrectionByTreasury)
                    {
                        EnableDisableForm(true);
                        divUtilzationPeriod.Visible = false;
                    }
                    else if (status == TreasuryWorkFlowStatusEnum.PendingForApproval
                        || status == TreasuryWorkFlowStatusEnum.RejectedByTreasury)
                    {
                        EnableDisableForm(false);
                        gridNatureOfRequest.Columns[3].Visible = false;
                        gridNatureOfRequest.Columns[4].Visible = false;
                        divUtilzationPeriod.Visible = false;
                    }
                    else if (status == TreasuryWorkFlowStatusEnum.ApprovedByTreasury)
                    {
                        EnableDisableForm(false);
                        if (AppCongif.Addendum) hrefaddendum.Visible = true;
                        gridNatureOfRequest.Columns[3].Visible = false;
                        gridNatureOfRequest.Columns[4].Visible = false;
                        if (AppCongif.Addendum) hrefaddendum.Visible = true;
                        divUtilzationPeriod.Visible = true;
                    }

                    lbTreasuryAllocNo.Enabled = false;
                    lkbtnAddBudeget.Visible = false;
                    divStatus.Visible = false;
                    RFVStatus.Enabled = false;
                    if (String.Compare(Mode, "Insert", true) == 0) divAllocationNo.Visible = false;

                }

                if (profileId == (int)UserProfileEnum.Treasury)
                {
                    // Draft: NA
                    // PendingForApproval: Enable All
                    // Approved By Treasury : Disable All
                    // Rejected By Treasury:  Disable All 
                    // Need Correction By Treasury : Disable All

                    if (status == TreasuryWorkFlowStatusEnum.DraftByVerticalController
                        || status == TreasuryWorkFlowStatusEnum.RejectedByTreasury
                        || status == TreasuryWorkFlowStatusEnum.NeedCorrectionByTreasury)
                    {
                        EnableDisableForm(false);

                        gridNatureOfRequest.Columns[3].Visible = false;
                        gridNatureOfRequest.Columns[4].Visible = false;
                        divStatus.Visible = false;
                        RFVStatus.Enabled = false;
                        divUtilzationPeriod.Visible = false;
                    }
                    else if (status == TreasuryWorkFlowStatusEnum.PendingForApproval)
                    {
                        EnableDisableForm(true);
                        gridNatureOfRequest.Columns[4].Visible = false;
                        lbSave.Visible = false;
                        divUtilzationPeriod.Visible = true;
                    }
                    else if (status == TreasuryWorkFlowStatusEnum.ApprovedByTreasury)
                    {
                        EnableDisableForm(false);
                        gridNatureOfRequest.Columns[3].Visible = false;
                        gridNatureOfRequest.Columns[4].Visible = false;
                        divStatus.Visible = false;
                        RFVStatus.Enabled = false;
                        if (AppCongif.Addendum) hrefaddendum.Visible = true;
                        divUtilzationPeriod.Visible = true;
                    }



                    btnAddNew.Enabled = false;
                    rdpPayDate.Enabled = false;
                    RFVrdpPayDate.Enabled = false;
                    radcomboCmpy.Enabled = false;
                    drpSubVertical.Enabled = false;
                    radComboRequestType.Enabled = false;
                    rdpStart.Enabled = false;
                    rdpEnd.Enabled = false;
                    RFVStatus.Enabled = true;
                    lkbtnAddBudeget.Visible = false;
                   // lbSubmit.Visible = false;
                    //lbSave.Visible = false;

                }
                if (profileId == (int)UserProfileEnum.CB)
                {
                    // Draft: NA
                    // PendingForApproval: NA
                    // Approved By Treasury : Disable all except Add budjet untisation
                    // Rejected By Treasury: NA 
                    // Need Correction By Treasury : NA

                    lbTreasuryAllocNo.Enabled = false;
                    lkbtnAddBudeget.Visible = false;
                    divStatus.Visible = true;
                    RFVStatus.Enabled = false;
                    btnAddNew.Enabled = false;
                    rdpPayDate.Enabled = false;
                    RFVrdpPayDate.Enabled = false;
                    radcomboCmpy.Enabled = false;
                    drpSubVertical.Enabled = false;
                    radComboRequestType.Enabled = false;
                    rdpStart.Enabled = false;
                    rdpEnd.Enabled = false;
                    //lbSave.Visible = false;
                    lbSubmit.Visible = false;
                    rcbStatus.Enabled = false;
                    gridNatureOfRequest.Columns[3].Visible = false;
                    gridNatureOfRequest.Columns[4].Visible = false;
                    lkbtnAddBudeget.Visible = true;
                    divStatus.Visible = false;
                    RFVStatus.Enabled = false;
                    divUtilzationPeriod.Visible = false;
                    if (status == TreasuryWorkFlowStatusEnum.ApprovedByTreasury)
                    {
                        if (AppCongif.Addendum) hrefaddendum.Visible = true;
                    }

                }

                var adndstatus = (AddendomWorkFlowStatusEnum)1;
                // if (hrefaddendum.Visible == true)
                // load only when VC:state/draft/need correction TR:pending approvel



            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void EnableDisableForm(bool flag)
        {
            try
            {
                lbTreasuryAllocNo.Enabled = flag;
                lkbtnAddBudeget.Visible = flag;
                RFVStatus.Enabled = flag;
                btnAddNew.Enabled = flag;
                rdpPayDate.Enabled = flag;
                radcomboCmpy.Enabled = flag;
                drpSubVertical.Enabled = flag;
                radComboRequestType.Enabled = flag;
                rdpStart.Enabled = flag;
                rdpEnd.Enabled = flag;
                lbSubmit.Visible = flag;
                rcbStatus.Enabled = flag;
                RFVrdpPayDate.Enabled = flag;
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void EnableDisableGridControls(GridEditableItem item)
        {
            RadComboBox RadCmbNatureOfRequest = (RadComboBox)item.FindControl("RadComboNatureOfRequest");
            RequiredFieldValidator RFVNaturOfReq = (RequiredFieldValidator)item.FindControl("RFVNaturOfReq");

            RadNumericTextBox RNumReqAmount = (RadNumericTextBox)item.FindControl("RadNumAmount");
            RequiredFieldValidator RFVReqAmount = (RequiredFieldValidator)item.FindControl("RFVAmount");
            CompareValidator CmpReqAmount = (CompareValidator)item.FindControl("CmpAmount");

            RadNumericTextBox RNumAppAmount = (RadNumericTextBox)item.FindControl("RadNumApprovedAmount");
            RequiredFieldValidator RFVApprovedAmount = (RequiredFieldValidator)item.FindControl("RFVApprovedAmount");
            CompareValidator CmpApprovedAmount = (CompareValidator)item.FindControl("CmpApprovedAmount");


            if (profileId == (int)UserProfileEnum.VerticalController)
            {
                RadCmbNatureOfRequest.Enabled = true;
                RFVNaturOfReq.Enabled = true;

                RNumReqAmount.Enabled = true;
                RFVReqAmount.Enabled = true;
                CmpReqAmount.Enabled = true;

                RNumAppAmount.Enabled = false;
                RFVApprovedAmount.Enabled = false;
                CmpApprovedAmount.Enabled = false;

            }
            else
            {
                RadCmbNatureOfRequest.Enabled = false;
                RFVNaturOfReq.Enabled = false;

                RNumReqAmount.Enabled = false;
                RFVReqAmount.Enabled = false;
                CmpReqAmount.Enabled = false;

                RNumAppAmount.Enabled = true;
                RFVApprovedAmount.Enabled = true;
                CmpApprovedAmount.Enabled = true;
            }


            if (profileId == (int)UserProfileEnum.Treasury)
            {
                RadCmbNatureOfRequest.Enabled = false;
                RNumReqAmount.Enabled = false;
                RFVApprovedAmount.Enabled = true;
                CmpApprovedAmount.Enabled = true;
            }
            else
            {
                RadCmbNatureOfRequest.Enabled = true;
                RNumReqAmount.Enabled = true;
                RFVApprovedAmount.Enabled = false;
                CmpApprovedAmount.Enabled = false;
            }

            RadCmbNatureOfRequest.DataTextField = "Name";
            RadCmbNatureOfRequest.DataValueField = "Id";
            RadCmbNatureOfRequest.DataSource = ViewState["ddNOR"];
            RadCmbNatureOfRequest.DataBind();
        }

        protected string generateAllocationNumber()
        {
            try
            {
                return Convert.ToString(radcomboCmpy.Text) + "-" + Convert.ToString(drpSubVertical.Text) + "-" +
                       Convert.ToString(radComboRequestType.Text).Substring(0, 1) + "-" + formatString(Convert.ToDateTime(rdpPayDate.SelectedDate).Day) +
                       formatString(Convert.ToDateTime(rdpPayDate.SelectedDate).Month) + formatString(Convert.ToDateTime(rdpPayDate.SelectedDate).Year);
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
                return null;
            }
        }

        protected string formatString(int input)
        {
            return (input < 10 ? "0" + Convert.ToString(input) : Convert.ToString(input));
        }

        protected TreasuryDetailModel fillModel()
        {
            try
            {
                TreasuryDetailModel detailModel = new TreasuryDetailModel()
                {
                    TreasuryDetailId = Convert.ToInt32(Session["TreasuryId"]),
                    VendorCreatedOn = Convert.ToDateTime(rdpCreatedDate.SelectedDate),
                    CompanyCode = Convert.ToString(radcomboCmpy.SelectedValue),
                    ProposedDate = Convert.ToDateTime(rdpPayDate.SelectedDate),
                    SubVerticalId = Convert.ToInt32(drpSubVertical.SelectedValue),
                    VerticalId = Convert.ToInt32(drpSubVertical.SelectedValue),
                    AllocationNumber = generateAllocationNumber(),
                    RequestType = Convert.ToString(radComboRequestType.SelectedValue),
                    RequestedAmount = Convert.ToDecimal(lblRequestedAmount.Text),
                    InitApprovedAmount = Convert.ToDecimal(lblInitAmount.Text),
                    AddendumTotal = Convert.ToDecimal(lblAddendumAmt.Text),
                    FinalAmount = Convert.ToDecimal(lblFinalAmt.Text),
                    UtilsationStartDate = Convert.ToDateTime(rdpStart.SelectedDate),
                    UtilsationEndDate = Convert.ToDateTime(rdpEnd.SelectedDate),
                    BalanceAmount = Convert.ToDecimal(lblBalanceAmount.Text),
                    Status = Convert.ToString(rcbStatus.SelectedValue),
                    NatureOfRequest = NatureOfRequest(),
                    verticalControllerComment = TreasuryComment(),
                    WorkflowStatusID = getStatus(),
                    FileUpload = FileUploadList()

                };
                if ((TreasuryWorkFlowStatusEnum)detailModel.WorkflowStatusID == TreasuryWorkFlowStatusEnum.RejectedByTreasury)
                {
                    //detailModel.InitApprovedAmount = 0;
                    detailModel.FinalAmount = 0;
                }
                return detailModel;
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
                return null;
            }
        }
        protected AddandumModel fillAddandumModel()
        {
            try
            {
                Decimal ApprAmount = 0;
                try // temp Fix
                {
                    ApprAmount = Convert.ToDecimal(radNumAddendumAppAmount.Text);
                }
                catch (Exception)
                {
                    ApprAmount = 0;
                }
                AddandumModel addandumDetail = new AddandumModel()
                {
                    Id = Convert.ToInt32(ViewState["CurrentAddendumId"]),
                    AddandomStatus = Convert.ToString(radCmbAddenumStatus.SelectedValue),
                    Amount = Convert.ToDecimal(radNumAddendumAmount.Text),
                    ApprovedAmount = ApprAmount,
                    NatureOfRequestId = Convert.ToInt32(radCmbAdnNatureOfRequest.SelectedValue),
                    Reason = Convert.ToString(tbaddendumReason.Text),
                    TreasuryDetailId = Convert.ToInt32(Session["TreasuryId"]),
                    AddandomWorkflowStatusId = getAddandumStatus(),
                    AddandomDate = Convert.ToDateTime(rdpAddendumDate.SelectedDate)
                };

                if ((AddendomWorkFlowStatusEnum)addandumDetail.AddandomWorkflowStatusId == AddendomWorkFlowStatusEnum.RejectedByTreasury)
                {
                    addandumDetail.ApprovedAmount = 0;
                }

                return addandumDetail;
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
                return null;
            }
        }
        protected int getStatus()
        {
            Control control = null;
            try
            {
                string ctrlname = Page.Request.Params["__EVENTTARGET"];
                if (ctrlname != null && ctrlname != String.Empty)
                {
                    control = Page.FindControl(ctrlname);
                }

                if (profileId == (int)UserProfileEnum.VerticalController)
                {
                    if (control.ID == "lbSave")
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(ViewState["WorkflowStatusID"])) ||
                            (TreasuryWorkFlowStatusEnum)ViewState["WorkflowStatusID"] == TreasuryWorkFlowStatusEnum.DraftByVerticalController)
                        {
                            return (int)TreasuryWorkFlowStatusEnum.DraftByVerticalController;
                        }
                        else
                        {
                            return (int)(TreasuryWorkFlowStatusEnum)ViewState["WorkflowStatusID"];
                        }


                    }
                    else if (control.ID == "lbSubmit")
                    {
                        return (int)TreasuryWorkFlowStatusEnum.PendingForApproval;
                    }
                }

                //1   Approved
                //2   Rejected
                //3   Need Correction

                if (profileId == (int)UserProfileEnum.Treasury)
                {
                    if (String.Compare(rcbStatus.SelectedValue, "1") == 0)
                    {
                        return (int)TreasuryWorkFlowStatusEnum.ApprovedByTreasury;
                    }
                    else if (String.Compare(rcbStatus.SelectedValue, "2") == 0)
                    {
                        return (int)TreasuryWorkFlowStatusEnum.RejectedByTreasury;
                    }
                    else if (String.Compare(rcbStatus.SelectedValue, "3") == 0)
                    {
                        return (int)TreasuryWorkFlowStatusEnum.NeedCorrectionByTreasury;
                    }
                    else if (String.IsNullOrEmpty(rcbStatus.SelectedValue))
                    {
                        return (int)(TreasuryWorkFlowStatusEnum)ViewState["WorkflowStatusID"];
                    }
                }

                if (profileId == (int)UserProfileEnum.CB)
                    return (int)(TreasuryWorkFlowStatusEnum)ViewState["WorkflowStatusID"];

                return 0;
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
                return 0;
            }
        }

        protected int getAddandumStatus()
        {
            Control control = null;
            try
            {
                string ctrlname = Page.Request.Params["__EVENTTARGET"];
                if (ctrlname != null && ctrlname != String.Empty)
                {
                    control = Page.FindControl(ctrlname);
                }

                if (profileId == (int)UserProfileEnum.VerticalController)
                {
                    if (control.ID == "lnkSaveAddendum")
                    {
                        return (int)AddendomWorkFlowStatusEnum.DraftByVerticalController;
                    }
                    else if (control.ID == "lnkSubmitAddendum")
                    {
                        return (int)AddendomWorkFlowStatusEnum.PendingForApproval;
                    }
                }

                //1   Approved
                //2   Rejected
                //3   Need Correction

                if (profileId == (int)UserProfileEnum.Treasury)
                {
                    if (String.Compare(radCmbAddenumStatus.SelectedValue, "1") == 0)
                    {
                        return (int)AddendomWorkFlowStatusEnum.ApprovedByTreasury;
                    }
                    else if (String.Compare(radCmbAddenumStatus.SelectedValue, "2") == 0)
                    {
                        return (int)AddendomWorkFlowStatusEnum.RejectedByTreasury;
                    }
                    else if (String.Compare(radCmbAddenumStatus.SelectedValue, "3") == 0)
                    {
                        return (int)AddendomWorkFlowStatusEnum.NeedCorrectionByTreasury;
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
                return 0;
            }
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            try
            {

                if (Convert.ToString(txtCommentBox.Text) != "")
                {
                    if (Convert.ToString(ViewState["TreasuryCommentShow"]) != "")
                    {
                        CommentLstShow = (List<CommentDtl>)ViewState["TreasuryCommentShow"];
                    }

                    if (Convert.ToString(ViewState["TreasuryComment"]) != "")
                    {
                        CommentLstAdd = (List<CommentDtl>)ViewState["TreasuryComment"];
                    }

                    CommentDtl cmdtl = new CommentDtl();
                    cmdtl.UserName = Convert.ToString(Session["LoginUserName"]);
                    cmdtl.Comment = Convert.ToString(txtCommentBox.Text);
                    cmdtl.CreateUserId = Convert.ToInt32(Session["UserId"]);
                    cmdtl.CreatedOn = DateTime.Now;

                    CommentLstAdd.Add(cmdtl);
                    ViewState["TreasuryComment"] = CommentLstAdd;
                    CommentLstShow.Add(cmdtl);
                    ViewState["TreasuryCommentShow"] = CommentLstShow;



                    StringBuilder CommentStr = new StringBuilder();
                    CommentStr = CommentStr.Append("<head> <table>");
                    foreach (var Comlst in CommentLstShow)
                    {
                        CommentStr.Append(" <tr><td> " + Comlst.UserName + "&nbsp;&nbsp;&nbsp;" + Comlst.CreatedOn.ToShortDateString() + "&nbsp" + Comlst.CreatedOn.ToShortTimeString() + " </td><td></td></tr>");
                        CommentStr.Append("<tr><td>&nbsp;&nbsp;&nbsp" + Comlst.Comment + "</td></tr>");
                    }
                    CommentStr.Append("</table></head>");
                    lblCommentDetail.Text = CommentStr.ToString();
                    txtCommentBox.Text = string.Empty;

                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        public List<TreasuryCommentModel> TreasuryComment()
        {

            if (Convert.ToString(ViewState["TreasuryComment"]) != "")
            {
                CommentLstAdd = (List<CommentDtl>)ViewState["TreasuryComment"];
            }

            List<TreasuryCommentModel> TreasuryCommentList = new List<TreasuryCommentModel>();
            TreasuryCommentModel TreasuryComment = new TreasuryCommentModel();

            foreach (var cmmt in CommentLstAdd)
            {
                TreasuryComment = new TreasuryCommentModel()
                {
                    Comment = cmmt.Comment
                };
                TreasuryCommentList.Add(TreasuryComment);
            }

            return TreasuryCommentList;
        }

        protected void LoadAddandonData(TreasuryDetailModel treasuryDetail, int id, AddandomDetail currentAddandom)
        {
            try
            {
                // Addandom Logic
                if ((profileId == (int)UserProfileEnum.VerticalController && currentAddandom.AddandomWorkflowStatusId == (int)AddendomWorkFlowStatusEnum.DraftByVerticalController)
                   || (profileId == (int)UserProfileEnum.VerticalController && currentAddandom.AddandomWorkflowStatusId == (int)AddendomWorkFlowStatusEnum.NeedCorrectionByTreasury)
                   || (profileId == (int)UserProfileEnum.Treasury && currentAddandom.AddandomWorkflowStatusId == (int)AddendomWorkFlowStatusEnum.PendingForApproval)
                   )
                {

                    ViewState["CurrentAddendumId"] = currentAddandom.AddandomDetailId;
                    radCmbAdnNatureOfRequest.SelectedValue = Convert.ToString(currentAddandom.NatureOfRequestId);
                    rdpAddendumDate.SelectedDate = currentAddandom.AddandomDate;
                    radNumAddendumAmount.Text = Convert.ToString(currentAddandom.Amount);
                    radNumAddendumAppAmount.Text = Convert.ToString(currentAddandom.ApprovedAmount);

                    //1:Approved
                    //2:Rejected
                    //3:Need Correction

                    radCmbAddenumStatus.SelectedValue = String.Empty;
                    tbaddendumReason.Text = currentAddandom.Reason;
                   

                }

            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void lnkSaveAddendum_Click(object sender, EventArgs e)
        {
            String jsonInputParameter = String.Empty;
            String serviceResult = String.Empty;
            AddandumModel saveModel;
            try
            {
                saveModel = fillAddandumModel();
                jsonInputParameter = JsonConvert.SerializeObject(saveModel);

                if (Convert.ToInt32(ViewState["CurrentAddendumId"]) == 0)
                {
                    serviceResult = commonFunctions.RestServiceCall(Constants.ADDANDUM_DETAIL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                }
                else
                {
                    serviceResult = commonFunctions.RestServiceCall(Constants.ADDANDUM_DETAIL_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                }

                if (serviceResult == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {   // call to get calculated total addandum and final ammount
                    string resultTreasuryDetail = commonFunctions.RestServiceCall(String.Format(Constants.TREASURE_CONTROLLER_GETDETAIL, Convert.ToInt32(Session["TreasuryId"])), string.Empty);
                    if (resultTreasuryDetail == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    }
                    else
                    {
                        TreasuryDetailModel treasuryDetail = JsonConvert.DeserializeObject<TreasuryDetailModel>(resultTreasuryDetail);
                        lblAddendumAmt.Text = Convert.ToString(treasuryDetail.AddendumTotal);
                        lblFinalAmt.Text = Convert.ToString(treasuryDetail.FinalAmount);

                    }
                }

                bindVerticalControllerDetail();

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void lnkSubmitAddendum_Click(object sender, EventArgs e)
        {
            String jsonInputParameter = String.Empty;
            String serviceResult = String.Empty;
            AddandumModel saveModel;
            try
            {
                saveModel = fillAddandumModel();
                jsonInputParameter = JsonConvert.SerializeObject(saveModel);

                if (Convert.ToInt32(ViewState["CurrentAddendumId"]) == 0)
                {
                    serviceResult = commonFunctions.RestServiceCall(Constants.ADDANDUM_DETAIL_ADD, Crypto.Instance.Encrypt(jsonInputParameter));
                }
                else
                {
                    serviceResult = commonFunctions.RestServiceCall(Constants.ADDANDUM_DETAIL_EDIT, Crypto.Instance.Encrypt(jsonInputParameter));
                }

                if (serviceResult == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {   // call to get calculated total addandum and final ammount
                    string resultTreasuryDetail = commonFunctions.RestServiceCall(String.Format(Constants.TREASURE_CONTROLLER_GETDETAIL, Convert.ToInt32(Session["TreasuryId"])), string.Empty);
                    if (resultTreasuryDetail == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                        radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                    }
                    else
                    {
                        TreasuryDetailModel vendorDetail = JsonConvert.DeserializeObject<TreasuryDetailModel>(resultTreasuryDetail);
                        lblAddendumAmt.Text = Convert.ToString(vendorDetail.AddendumTotal);
                        lblFinalAmt.Text = Convert.ToString(vendorDetail.FinalAmount);

                    }
                }

                bindVerticalControllerDetail();
                CalculateAmount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void bindVerticalControllerDetail()
        {
            try
            {

                string resultTreasuryDetail = commonFunctions.RestServiceCall(String.Format(Constants.TREASURE_CONTROLLER_GETDETAIL, Convert.ToInt32(Session["TreasuryId"])), string.Empty);
                if (resultTreasuryDetail == Constants.REST_CALL_FAILURE)
                {
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    radMessage.Show(Constants.ERROR_OCC_WHILE_GETTING_DETAILS);
                }
                else
                {
                    TreasuryDetailModel treasuryDetail = JsonConvert.DeserializeObject<TreasuryDetailModel>(resultTreasuryDetail);
                    ViewState["UtilsationStartDate"] = treasuryDetail.UtilsationStartDate;
                    ViewState["UtilsationEndDate"] = treasuryDetail.UtilsationEndDate;

                    rdpCreatedDate.SelectedDate = treasuryDetail.VendorCreatedOn;
                    rdpPayDate.SelectedDate = treasuryDetail.ProposedDate;
                    radcomboCmpy.SelectedValue = treasuryDetail.CompanyCode;
                    drpSubVertical.SelectedValue = Convert.ToString(treasuryDetail.SubVerticalId);

                    ViewState["WorkflowStatusID"] = treasuryDetail.WorkflowStatusID;

                    if (!string.IsNullOrEmpty(drpSubVertical.SelectedValue))
                        GetVerticals(drpSubVertical.SelectedValue);

                    drpVertical.SelectedValue = Convert.ToString(treasuryDetail.VerticalId);
                    lbTreasuryAllocNo.Text = Convert.ToString(treasuryDetail.AllocationNumber);
                    radComboRequestType.SelectedValue = Convert.ToString(treasuryDetail.RequestType);
                    lblRequestedAmount.Text = Convert.ToString(treasuryDetail.RequestedAmount);
                    lblInitAmount.Text = Convert.ToString(treasuryDetail.InitApprovedAmount);
                    lblAddendumAmt.Text = Convert.ToString(treasuryDetail.AddendumTotal);
                    lblFinalAmt.Text = Convert.ToString(treasuryDetail.FinalAmount);
                    rdpStart.SelectedDate = treasuryDetail.UtilsationStartDate;
                    rdpEnd.SelectedDate = treasuryDetail.UtilsationEndDate;
                    lblBalanceAmount.Text = Convert.ToString(treasuryDetail.BalanceAmount);
                    //rcbStatus.SelectedValue = treasuryDetail.Status;
                    lblAddendumAllocationNo.Text = treasuryDetail.AllocationNumber;
                    rcbStatus.SelectedValue = String.Empty;
                    DataTable dt = CreateDTSchema(null);

                    foreach (var req in treasuryDetail.NatureOfRequest)
                    {
                        dt.Rows.Add(req.TreasuryRequestId, req.natureOfRequest, req.natureOfRequestText, req.requestedAmount, req.approvedAmount);
                    }

                    ViewState["NOR"] = dt;

                    List<TreasuryCommentModel> TreasuryCommentList = treasuryDetail.verticalControllerComment;
                    foreach (var comment in TreasuryCommentList)
                    {
                        CommentDtl cmtdtl = new CommentDtl();
                        cmtdtl.UserName = comment.CommentBy;
                        cmtdtl.Comment = comment.Comment;
                        cmtdtl.CreatedOn = comment.CreatedOn;
                        CommentLstShow.Add(cmtdtl);
                    }

                    ViewState["TreasuryCommentShow"] = CommentLstShow;
                    StringBuilder CommentStr = new StringBuilder();
                    CommentStr = CommentStr.Append("<head> <table>");
                    foreach (var Comlst in CommentLstShow)
                    {
                        CommentStr.Append(" <tr><td> " + Comlst.UserName + "&nbsp;&nbsp;&nbsp" + Comlst.CreatedOn.ToShortDateString() + "&nbsp" + Comlst.CreatedOn.ToShortTimeString() + " </td><td></td></tr>");
                        CommentStr.Append("<tr><td>&nbsp;&nbsp;&nbsp" + Comlst.Comment + " </td></tr>");
                    }
                    CommentStr.Append("</table></head>");
                    lblCommentDetail.Text = CommentStr.ToString();


                    AddandomDetail currentAddandom = treasuryDetail.getCurrentAddendum(Convert.ToInt32(Session["TreasuryId"]));

                    DataTable dtNOR = (DataTable)ViewState["NOR"];
                    DataView view = new DataView(dtNOR);
                    DataTable distinctValues = view.ToTable(true, "NatureOfRequest", "NatureOfRequestText");
                    radCmbAdnNatureOfRequest.DataTextField = "NatureOfRequestText";
                    radCmbAdnNatureOfRequest.DataValueField = "NatureOfRequest";
                    radCmbAdnNatureOfRequest.DataSource = distinctValues;
                    radCmbAdnNatureOfRequest.DataBind();
                    radCmbAddenumStatus.SelectedValue = String.Empty;

                    if (currentAddandom != null)//added by santosh
                    {
                        if ((profileId == (int)UserProfileEnum.VerticalController && currentAddandom.AddandomWorkflowStatusId == (int)AddendomWorkFlowStatusEnum.DraftByVerticalController)
                           || (profileId == (int)UserProfileEnum.VerticalController && currentAddandom.AddandomWorkflowStatusId == (int)AddendomWorkFlowStatusEnum.NeedCorrectionByTreasury)
                           || (profileId == (int)UserProfileEnum.Treasury && currentAddandom.AddandomWorkflowStatusId == (int)AddendomWorkFlowStatusEnum.PendingForApproval)
                           )
                        {
                            LoadAddandonData(treasuryDetail, Convert.ToInt32(Session["TreasuryId"]), currentAddandom);
                        }
                    }

                    // load history
                    List<AddandumDisplayModel> historyAddandom = treasuryDetail.getHistoryAddendum(Convert.ToInt32(Session["TreasuryId"]));
                    rgridAddandumHistory.DataSource = historyAddandom;
                    rgridAddandumHistory.DataBind();

                    EnableDisableFormControls((TreasuryWorkFlowStatusEnum)treasuryDetail.WorkflowStatusID);
                    if (currentAddandom != null)
                    {
                        EnableDisableAddendumControls((AddendomWorkFlowStatusEnum)currentAddandom.AddandomWorkflowStatusId);
                    }
                    else
                    {
                        EnableDisableAddendumControls((AddendomWorkFlowStatusEnum)1);
                    }

                }



            }
            catch (Exception ex)
            {

                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void EnableDisableAddendumControls(AddendomWorkFlowStatusEnum status)
        {
            try
            {
                //  EnableDisable Logic
                // Check of last addendum request is open, do not allow to add new one

                //DraftByVerticalController = 1,
                //PendingForApproval = 2,
                //ApprovedByTreasury = 3,
                //RejectedByTreasury = 4,
                //NeedCorrectionByTreasury = 5

                if (profileId == (int)UserProfileEnum.VerticalController)
                {
                    if (status == AddendomWorkFlowStatusEnum.DraftByVerticalController || status == AddendomWorkFlowStatusEnum.NeedCorrectionByTreasury)
                    {
                        EnableDisableAddendonAllFields(true);
                        lnkSaveAddendum.Visible = true;
                    }

                    if (status == AddendomWorkFlowStatusEnum.PendingForApproval ||
                        status == AddendomWorkFlowStatusEnum.RejectedByTreasury)
                    {
                        EnableDisableAddendonAllFields(false);
                        lnkSaveAddendum.Visible = false;
                        lnkSubmitAddendum.Visible = false;
                    }
                    radNumAddendumAppAmount.Enabled = false;
                    RFVNumAddendumAppAmount.Enabled = false;
                    divAddendumStatus.Visible = false;
                    divAddendumStatus.Visible = false;
                    RFVAddenumStatus.Enabled = false;
                }
                if (profileId == (int)UserProfileEnum.Treasury)
                {
                    if (status == AddendomWorkFlowStatusEnum.DraftByVerticalController
                        || status == AddendomWorkFlowStatusEnum.ApprovedByTreasury
                        || status == AddendomWorkFlowStatusEnum.NeedCorrectionByTreasury
                        || status == AddendomWorkFlowStatusEnum.RejectedByTreasury)
                    {
                        EnableDisableAddendonAllFields(false);
                        lnkSubmitAddendum.Visible = false;
                        divAddendumStatus.Visible = false;
                        RFVAddenumStatus.Enabled = false;
                    }
                    lnkSubmitAddendum.Visible = false;
                    lnkSaveAddendum.Visible = false;
                    if (status == AddendomWorkFlowStatusEnum.PendingForApproval)
                    {
                        EnableDisableAddendonAllFields(false);
                        radNumAddendumAppAmount.Enabled = true;
                        RFVNumAddendumAppAmount.Enabled = true;
                        lnkSubmitAddendum.Visible = true;
                        radCmbAddenumStatus.Enabled = true;
                        RFVAddenumStatus.Enabled = true;
                    }
                    

                    if (ViewState["CurrentAddendumId"] == null) lnkSubmitAddendum.Visible = false; // if no addandum for approval

                }
                if (profileId == (int)UserProfileEnum.CB)
                {
                    EnableDisableAddendonAllFields(false);
                    lnkSubmitAddendum.Visible = false;
                    lnkSaveAddendum.Visible = false;
                }

            }
            catch (Exception)
            {


                throw;
            }
        }

        protected void EnableDisableAddendonAllFields(Boolean flag)
        {
            try
            {
                rdpAddendumDate.Enabled = flag;
                RFVAddendumDate.Enabled = flag;

                lblAddendumAllocationNo.Enabled = false;

                radNumAddendumAmount.Enabled = flag;
                RFVNumAddendumAmount.Enabled = flag;

                radNumAddendumAppAmount.Enabled = flag;
                RFVNumAddendumAppAmount.Enabled = flag;

                radCmbAddenumStatus.Enabled = flag;
                RFVAddenumStatus.Enabled = flag;
                
                radCmbAdnNatureOfRequest.Enabled = flag;
                RFVAdnNatureOfRequest.Enabled = flag;

                tbaddendumReason.Enabled = flag;
            }
            catch (Exception)
            {

                throw;
            }
        }

    

        protected void rgridManualUtilisation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bindBudgetUtilisationDetail(Convert.ToInt32(Session["TreasuryId"]));
        }

        protected void rgridManualUtilisation_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.CommandName))
                {
                    case "InitInsert":
                        rgridManualUtilisation.MasterTableView.ClearEditItems();
                        break;
                    case "Edit":
                        e.Item.OwnerTableView.IsItemInserted = false;
                        break;
                    case "Filter":
                        rgridManualUtilisation.MasterTableView.ClearEditItems();
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rgridManualUtilisation_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            if (item != null)
            {
                SaveBudgetUtilisation(e, Constants.CONST_NEW_MODE);
            }
            }
        protected void lbCancel_Click(object sender, EventArgs e)
        {
            if (profileId == (int)UserProfileEnum.VerticalController)
            {
                Response.Redirect("TreasuryListVertical.aspx");
            }
            else if (profileId == (int)UserProfileEnum.Treasury || profileId == (int)UserProfileEnum.CB)
            {
                Response.Redirect("TreasuryList.aspx");
            }
        }

        protected DataTable SchemaBudgetUtilizationDt()
        {
            if (dt == null) dt = new DataTable();
           
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("PaymentDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("NatureOfRequest", typeof(string)));
            dt.Columns.Add(new DataColumn("NatureOfRequestText", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt.Columns.Add(new DataColumn("AccountType", typeof(string)));
            dt.Columns.Add(new DataColumn("AcountCode", typeof(string)));
            dt.Columns.Add(new DataColumn("DocumentNo", typeof(string)));
            dt.Columns.Add(new DataColumn("IsDirty", typeof(string)));
            //DataRow dr = dt.NewRow();
            //dr["Id"] = "1";
            //dr["PaymentDate"] = DateTime.Now;
            //dr["NatureOfRequest"] = "1";
            //dr["NatureOfRequestText"] = "1";
            //dr["Amount"] = "1";
            //dr["AccountType"] = "1";
            //dr["AcountCode"] = "1";
            //dr["DocumentNo"] = "1";
            //dr["IsDirty"] = "1";
            //dt.Rows.Add(dr);
            return dt;
        }

        protected void lkbtnAddBudeget_Click(object sender, EventArgs e)
        {
            //DataTable dt = SchemaBudgetUtilizationDt();
            // rgridManualUtilisation.DataSource = new DataTable();
            //rgridManualUtilisation.DataBind();

            RadAjaxManager1.ResponseScripts.Add("OpenBudgetUtilization()");



        }

        protected void rgridManualUtilisation_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    RadComboBox RadCmbNatureOfRequest = (RadComboBox)item.FindControl("radComboNatureOfRequest");
                    RadCmbNatureOfRequest.DataTextField = "Name";
                    RadCmbNatureOfRequest.DataValueField = "Id";
                    RadCmbNatureOfRequest.DataSource = ViewState["ddNOR"];
                    RadCmbNatureOfRequest.DataBind();

                    RadLabel lblNatureOfRequestId = (RadLabel)item.FindControl("lblNatureOfRequestId");
                    if (!string.IsNullOrEmpty(Convert.ToString(lblNatureOfRequestId.Text)))
                        RadCmbNatureOfRequest.SelectedValue = lblNatureOfRequestId.Text;
                                        

                    RadLabel lblAmount = (RadLabel)item.FindControl("lblAmountVal");
                    RadNumericTextBox RadNumAmount = (RadNumericTextBox)item.FindControl("RadNumAmount");
                    RadNumAmount.Text = lblAmount.Text;


                    RadLabel lblAccountCode = (RadLabel)item.FindControl("lblAccountCodeVal");
                    //RadNumericTextBox RadNumAccountCode = (RadNumericTextBox)item.FindControl("RadNumAccountCode");
                    RadTextBox RadNumAccountCode = (RadTextBox)item.FindControl("RadNumAccountCode");
                    RadNumAccountCode.Text = lblAccountCode.Text;


                    RadLabel lblDocumentCode = (RadLabel)item.FindControl("lblDocumentCodeVal");
                    // RadNumericTextBox RadNumDocumentCode = (RadNumericTextBox)item.FindControl("RadNumDocumentCode");
                    RadTextBox RadNumDocumentCode = (RadTextBox)item.FindControl("RadNumDocumentCode");
                    RadNumDocumentCode.Text = lblDocumentCode.Text;


                    RadLabel lblAccountTypeId = (RadLabel)item.FindControl("lblAccountTypeId");
                    RadComboBox radComboAccountType = (RadComboBox)item.FindControl("radComboAccountType");
                    if (!string.IsNullOrEmpty(Convert.ToString(lblAccountTypeId.Text)))
                        radComboAccountType.SelectedValue = lblAccountTypeId.Text;

                    RadLabel lblPaymentDateVal = (RadLabel)item.FindControl("lblPaymentDateVal");
                    RadDatePicker dpkPaymentDate = (RadDatePicker)item.FindControl("dpkPaymentDate");
                    if (!string.IsNullOrEmpty(Convert.ToString(lblPaymentDateVal.Text)))
                        dpkPaymentDate.SelectedDate = Convert.ToDateTime(lblPaymentDateVal.Text);

                    dpkPaymentDate.MinDate = Convert.ToDateTime(ViewState["UtilsationStartDate"]);
                    dpkPaymentDate.MaxDate = Convert.ToDateTime(ViewState["UtilsationEndDate"]);


                }
            }
           
        }
        private void SaveBudgetUtilisation(GridCommandEventArgs e, string editMode)
        {
            try
            {
                int budgetUtilisationId = 0;

                GridEditableItem item = (GridEditableItem)e.Item;
                if (item != null)
                {
                    RadComboBox RadCmbNatureOfRequest = (RadComboBox)item.FindControl("radComboNatureOfRequest");
                    RadDatePicker dpkPaymentDate = (RadDatePicker)item.FindControl("dpkPaymentDate");
                    RadNumericTextBox RadNumAmount = (RadNumericTextBox)item.FindControl("RadNumAmount");
                    RadComboBox radComboAccountType = (RadComboBox)item.FindControl("radComboAccountType");
                    // RadNumericTextBox RadNumAccountCode = (RadNumericTextBox)item.FindControl("RadNumAccountCode");
                    //RadNumericTextBox RadNumDocumentCode = (RadNumericTextBox)item.FindControl("RadNumDocumentCode");
                    RadTextBox RadNumAccountCode = (RadTextBox)item.FindControl("RadNumAccountCode");
                    RadTextBox RadNumDocumentCode = (RadTextBox)item.FindControl("RadNumDocumentCode");

                    if (editMode == Constants.CONST_EDIT_MODE)
                        budgetUtilisationId = Convert.ToInt32(item.GetDataKeyValue("TreasuryBudgetUtilisationId"));

                    TreasuryBudgetUtilisationModel treasuryBudgetUtilisationModel = new TreasuryBudgetUtilisationModel()
                    {
                        TreasuryBudgetUtilisationId = budgetUtilisationId,
                        NatureOfReqestId = Convert.ToInt32(RadCmbNatureOfRequest.SelectedValue),
                        Amount = Convert.ToDecimal(RadNumAmount.Text),
                        AccountType = radComboAccountType.SelectedValue,
                        AccountCode = Convert.ToString(RadNumAccountCode.Text),
                        DocumentNo = Convert.ToString(RadNumDocumentCode.Text),
                        TreasuryDetailId = Convert.ToInt32(Session["TreasuryId"]),
                        PaymentDate = Convert.ToDateTime(dpkPaymentDate.SelectedDate)

                    };

                    string jsonInputParameter = JsonConvert.SerializeObject(treasuryBudgetUtilisationModel);
                    string result = string.Empty;

                    result = commonFunctions.RestServiceCall(Constants.BUDGETUTILISATION_DETAILEXIST, Crypto.Instance.Encrypt(jsonInputParameter));
                    bool isExist = Convert.ToBoolean(result);
                    radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                    if (isExist)
                    {
                        radMessage.Show(Constants.BUDGETUTILISATION_DETAILEXISTMSG);
                        e.Canceled = true;
                        return;
                    }





                    if (editMode == Constants.CONST_EDIT_MODE)
                        result = commonFunctions.RestServiceCall(Constants.UPDATEBUDGETUTILISATION_DETAIL, Crypto.Instance.Encrypt(jsonInputParameter));
                    else
                        result = commonFunctions.RestServiceCall(Constants.ADDBUDGETUTILISATION_DETAIL, Crypto.Instance.Encrypt(jsonInputParameter));
                    if (result == Constants.REST_CALL_FAILURE)
                        if (editMode == Constants.CONST_EDIT_MODE)
                        {
                            e.Canceled = true;
                            radMessage.Show(Constants.ERROR_OCC_WHILE_UPDATING);
                           
                        }
                        else
                        {
                            e.Canceled = true;
                            radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                            
                        }
                    else
                    {
                        
                      
                        rgridManualUtilisation.MasterTableView.ClearEditItems();
                        rgridManualUtilisation.Rebind();
                        radMessage.Show(Constants.DETAIL_SAVE_SUCCESS);
                    }

                    bindBudgetUtilisationDetail(Convert.ToInt32(Session["TreasuryId"]));
                    rgridManualUtilisation.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void rgridManualUtilisation_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            SaveBudgetUtilisation(e, Constants.CONST_EDIT_MODE);
        }

        protected void rgridManualUtilisation_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item =(GridDataItem)e.Item;
            int budgetUtilisationId = Convert.ToInt32(item.GetDataKeyValue("TreasuryBudgetUtilisationId"));

            string result = commonFunctions.RestServiceCall(string.Format(Constants.DELETEBUDGETUTILISATION_DETAIL, budgetUtilisationId), string.Empty);
            if (result != Constants.REST_CALL_FAILURE)
            {
                bindBudgetUtilisationDetail(Convert.ToInt32(Session["TreasuryId"]));
                rgridManualUtilisation.DataBind();
            }
            else
            {
                radMessage.Title = Constants.RAD_MESSAGE_TITLE;
                radMessage.Show(Constants.ERROR_OCC_WHILE_SAVING);
                rgridManualUtilisation.DataSource = new System.Data.DataTable();
            }
        }

        protected void btnAddAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                //screen, mode, entityId, canModify, isMultiFileUpload,showDocumentType
                RadAjaxManager1.ResponseScripts.Add("openRadWin('VC','" + (String.IsNullOrEmpty(Convert.ToString(Session["TreasuryId"])) || Convert.ToString(Session["TreasuryId"]) == "0" ? "insert" : "update") + "','" + Convert.ToString(Session["TreasuryId"]) + "','true','true','true');");
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

    }
}
