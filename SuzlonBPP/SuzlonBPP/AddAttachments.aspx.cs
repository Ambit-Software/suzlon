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


namespace SuzlonBPP
{
    public partial class AddAttachments : System.Web.UI.Page
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        String mode = String.Empty;
        String entityId = String.Empty;
        Boolean canAdd = false;
        Boolean canDelete = false;
        Boolean isMultiUpload = false;
        Boolean showDtype = false;
        String entityName = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            mode = Convert.ToString(Request.QueryString["mode"]);
            entityId = Convert.ToString(Request.QueryString["entityId"]);
            canAdd = Convert.ToBoolean(Request.QueryString["canAdd"]);
            canDelete = Convert.ToBoolean(Request.QueryString["canDelete"]);
            isMultiUpload = Convert.ToBoolean(Request.QueryString["isMultiUpload"]);
            showDtype = Convert.ToBoolean(Request.QueryString["showDtype"]);
            entityName = Convert.ToString(Request.QueryString["entityName"]);

            EnableDisableFormControl();

            if (!Page.IsPostBack)
            {
                if (entityName == "PaymentInitiatorPopUp")
                {
                    bindInsertDataPaymentInitiator();
                }
                else
                {
                    getDataSource();
                }
                grdAttachments.DataBind();
                txtDType.Text = "";

            }
        }

        private void EnableDisableFormControl()
        {
            try
            {
                if (canAdd)
                {
                    tbAttachment.Visible = true;
                    buttonSubmit.Visible = true;
                    buttomSaveClose.Visible = true;
                    btnCancel.Visible = true;
                }
                else
                {
                    tbAttachment.Visible = false;
                    buttonSubmit.Visible = false;
                    buttomSaveClose.Visible = false;
                    btnCancel.Visible = false;
                }

                if (canDelete)
                {
                    grdAttachments.MasterTableView.GetColumn("DeleteAttachment").Display = true;
                }
                else
                {
                    grdAttachments.MasterTableView.GetColumn("DeleteAttachment").Display = false;
                }

                if (isMultiUpload)
                {
                    RadAsyncUpload.MultipleFileSelection = Telerik.Web.UI.AsyncUpload.MultipleFileSelection.Automatic;
                }
                else
                {
                    RadAsyncUpload.MultipleFileSelection = Telerik.Web.UI.AsyncUpload.MultipleFileSelection.Disabled;
                }

                if (showDtype)
                {
                    txtDType.Enabled = true;
                    grdAttachments.MasterTableView.GetColumn("DocumentType").Display = true;
                }
                else
                {
                    txtDType.Enabled = false;
                    grdAttachments.MasterTableView.GetColumn("DocumentType").Display = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void getDataSource()
        {
            if (String.Compare(mode, "insert", true) == 0)
            {
                if (entityName == "PaymentInitiatorPopUp")
                {
                    bindInsertDataPaymentInitiator();
                }
                else
                bindInsertData();
            }
            else
            {
                bindEditData();
            }
        }

        private void bindInsertData()
        {
            try
            {
                DataTable dtAccatch = new DataTable();
                if (Session["ATTACHMENT"] != null)
                {
                    dtAccatch = (DataTable)Session["ATTACHMENT"];
                }
                else {
                    dtAccatch = CreateDTSchema(dtAccatch);
                }

                grdAttachments.DataSource = dtAccatch;


            }
            catch (Exception)
            {

                throw;
            }
        }
        private void bindInsertDataPaymentInitiator()
        {
            try
            {
                DataTable dtAccatch = new DataTable();
                if (Session["ATTACHMENT"] != null)
                {
                    dtAccatch = (DataTable)Session["ATTACHMENT"];
                }
                else
                {
                    dtAccatch = CreateDTSchema(dtAccatch);
                }

                grdAttachments.DataSource = dtAccatch.Select("FileuploadId="+ entityId);


            }
            catch (Exception)
            {

                throw;
            }
        }
        private void bindEditData()
        {
            List<Models.FileUpload> files = new List<Models.FileUpload>();
            DataTable dtAccatch = new DataTable();
            var result = commonFunctions.RestServiceCall(string.Format(Constants.GET_FILEUPLOADS, entityId, entityName), string.Empty);
            //  var result = commonFunctions.RestServiceCall(string.Format(Constants.GET_FILEUPLOADS, "TRS"), string.Empty);
            if (result == Constants.REST_CALL_FAILURE)
            {

                grdAttachments.DataSource = CreateDTSchema(dtAccatch);
            }
            else
            {
                // grdAttachments.DataSource = JsonConvert.DeserializeObject<List<Models.FileUpload>>(result); comment santosh 6 Sep
                grdAttachments.DataSource = JsonConvert.DeserializeObject<List<FileUploadDetails>>(result);
            }
        }





        protected void grdAttachments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            getDataSource();
        }

        protected void grdAttachments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string VerticalId = string.Empty;
                GridDataItem item = e.Item as GridDataItem;
                GridEditableItem gridRow = (GridEditableItem)e.Item;
                HyperLink lkbtnView = (HyperLink)gridRow.FindControl("lkbtnView");
                string FileName = item.GetDataKeyValue("FileName").ToString();

                lkbtnView.Target = "_blank";

                if (String.Compare(mode, "insert", true) == 0)
                {
                    lkbtnView.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/" +
                Constants.VENDOR_BANK_ATTACHMENT_PATH_TEMP.Replace("\\", "") + "/" + FileName.ToString();
                }
                else
                {
                    lkbtnView.NavigateUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath + "/" +
                Constants.VENDOR_BANK_ATTACHMENT_PATH.Replace("\\", "") + "/" + FileName.ToString();
                }

                if (Convert.ToInt32(item.GetDataKeyValue("createdBy")) != Convert.ToInt32(HttpContext.Current.Session["UserId"]))
                {
                    item["DeleteAttachment"].Enabled = false;
                }



            }
        }

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            //int Counter = 0;
            string path = String.Empty;
            string rst = string.Empty;
            foreach (UploadedFile file in RadAsyncUpload.UploadedFiles)
            {
                // String timeStamp = (DateTime.Now).AddSeconds(Counter).ToString("yyyyMMddHHmmssffff");
                string userId = Convert.ToString(HttpContext.Current.Session["UserId"]);
                string filename = string.Empty;
                if (String.Compare(mode, "insert", true) == 0)
                {
                    path = Server.MapPath(Constants.VENDOR_BANK_ATTACHMENT_PATH_TEMP);
                    
                }
                else
                {
                    path = Server.MapPath(Constants.VENDOR_BANK_ATTACHMENT_PATH);
                    
                }

                if (entityName == "PaymentInitiatorPopUp")
                    filename = entityId+ "!" + file.FileName;
                else
                filename = userId + "!" + file.FileName;

                if (String.Compare(mode, "insert", true) == 0)
                {
                    DataTable dtAccatch = new DataTable();
                    if (Session["ATTACHMENT"] != null)
                    {
                        dtAccatch = (DataTable)Session["ATTACHMENT"];
                    }
                    else {
                        dtAccatch = CreateDTSchema(dtAccatch);
                    }
                    if (entityName == "PaymentInitiatorPopUp")
                    {
                        dtAccatch.Rows.Add(new object[] { entityId, file.FileName, filename, txtDType.Text, DateTime.Now, Convert.ToInt32(HttpContext.Current.Session["UserId"]),Convert.ToString(HttpContext.Current.Session["LoginUserName"]) });
                        grdAttachments.DataSource = dtAccatch.Select("FileuploadId=" + entityId); 
                        grdAttachments.DataBind();
                        Session["ATTACHMENT"] = dtAccatch;
                        file.SaveAs(path + filename, true);
                    }
                    else
                    {
                        dtAccatch.Rows.Add(new object[] { null, file.FileName, filename, txtDType.Text, DateTime.Now, Convert.ToInt32(HttpContext.Current.Session["UserId"]) });
                        grdAttachments.DataSource = dtAccatch ;
                        grdAttachments.DataBind();
                        Session["ATTACHMENT"] = dtAccatch;
                        file.SaveAs(path + filename, true);
                    }
                }
                else
                {
                    Models.FileUpload FileUploadObj = new Models.FileUpload();
                    FileUploadObj.EntityId = String.IsNullOrEmpty(Convert.ToString(entityId)) ? 0 : Convert.ToInt32(entityId);
                    FileUploadObj.DisplayName = file.FileName;
                    FileUploadObj.FileName = filename;
                    FileUploadObj.EntityName = entityName;
                    FileUploadObj.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    FileUploadObj.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    FileUploadObj.CreatedOn = DateTime.Now;
                    FileUploadObj.ModifiedOn = DateTime.Now;
                    FileUploadObj.DocumentType = txtDType.Text;
                    string jsonParameter = JsonConvert.SerializeObject(FileUploadObj);
                     rst = commonFunctions.RestServiceCall(Constants.ADD_FILEUPLOAD, Crypto.Instance.Encrypt(jsonParameter));
                    if (rst == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = "Alert";
                        radMessage.Show("Error in Uploading Files.");
                        break;
                    }
                    file.SaveAs(path + filename.Replace(Convert.ToString(HttpContext.Current.Session["UserId"]) + "!", (rst.Replace("\"", "") + "!")),true);
                    bindEditData();
                    grdAttachments.DataBind();
                }
                
            }
            if (rst != Constants.REST_CALL_FAILURE)
            {
                radMessage.Title = "Message";
                radMessage.Show("Save Sucessfully");
                
            }

            txtDType.Text = "";
            // RadAsyncUpload.
            // RadAsyncUpload.UploadedFiles.RemoveAt(1);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "DeleteFiles()", true);
        }

        protected void grdAttachments_ItemCommand(object sender, GridCommandEventArgs e)
        {

            if (e.CommandName.Equals("Delete"))
            {
                if (String.Compare(mode, "Insert", true) == 0)
                {
                    if (entityName == "PaymentInitiatorPopUp")
                    {
                        DataTable dt = (DataTable)Session["ATTACHMENT"];

                        GridDataItem item = (GridDataItem)e.Item;
                        // DataRow[] dr = dt.Select("FileuploadId=" + entityId + " and DisplayName=" + item["DisplayName"].Text + " and CreatedOn=" + item["CreatedOn"]);

                        string fileName =Convert.ToString(item["FileName"].Text);
                      // DateTime uploadedTime= Convert.ToDateTime(item["DocumentType"].Text);
                        
                        int rowIndex = dt.Rows.IndexOf(dt.Select("FileName='" + fileName +"'")[0]);

                        dt.Rows.RemoveAt(rowIndex);
                        grdAttachments.DataSource = dt.Select("FileuploadId=" + entityId); 
                        grdAttachments.DataBind();
                    }
                    else
                    {
                        DataTable dt = (DataTable)Session["ATTACHMENT"];
                        dt.Rows.RemoveAt(((GridDataItem)e.Item).ItemIndex + (grdAttachments.CurrentPageIndex * grdAttachments.PageSize));
                        grdAttachments.DataSource = dt;
                        grdAttachments.DataBind();
                    }
                }
                else
                {

                    GridDataItem item = (GridDataItem)e.Item;
                    int Id = (int)item.GetDataKeyValue("FileuploadId");
                    int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    var rst = commonFunctions.RestServiceCall(string.Format(Constants.DELETE_FILEUPLOAD, Id, userid), string.Empty);
                    if (rst == Constants.REST_CALL_FAILURE)
                    {
                        radMessage.Title = "Alert";
                        radMessage.Show("Error in deleting File Upload.");

                    }
                    else
                    {
                        getDataSource();
                        grdAttachments.DataBind();
                    }
                }
            }
        }

        protected DataTable CreateDTSchema(DataTable dt)
        {
            if (dt == null) dt = new DataTable();
            dt.Columns.Add(new DataColumn("FileuploadId", typeof(int)));
            dt.Columns.Add(new DataColumn("DisplayName", typeof(string)));
            dt.Columns.Add(new DataColumn("FileName", typeof(string)));
            dt.Columns.Add(new DataColumn("DocumentType", typeof(string)));
            dt.Columns.Add(new DataColumn("CreatedOn", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("createdBy", typeof(Int32)));
            dt.Columns.Add(new DataColumn("name", typeof(string)));
            return dt;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
           // Session["ATTACHMENT"] = null;
        }
    }

    public partial class FileUploadDetails
    {
        public int FileUploadId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string FileName { get; set; }
        public string DisplayName { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DocumentType { get; set; }
        public string Name { get; set; }
    }
}