using Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolarPMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


namespace SolarPMS
{
    public partial class UploadDEDocument : System.Web.UI.Page
    {
        #region "Data Member"
        public int ActivityId { get; set; }
        public int SubActivityId { get; set; }
        public string FieldValue { get; set; }
        public string SAPSite { get; set; }
        public string SAPProjectId { get; set; }
        public int WBSAreaId { get; set; }
        public string SAPNetwork { get; set; }
        public string SAPActivity { get; set; }
        public string SAPSubActivity { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public int UserId { get; set; }
        public int DocumentVersion { get; set; }
        public int DocumentDetailsId { get; set; }
        public int Version { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityName { get; set; }

        public static bool IsReview { get; set; }
          
        List<Document> AttachmentLst = new List<Document>();

        #endregion

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    Initialize();
                else
                    SetScreenVariables();
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Remove document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridDocuments_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    GridDataItem item = e.Item as GridDataItem;
                    string RemoveFileName = Convert.ToString(item.GetDataKeyValue("FileName"));

                    if (Convert.ToString(ViewState["AttachmentName"]) != "")
                        AttachmentLst = (List<Document>)ViewState["AttachmentName"];

                    var attachment = AttachmentLst.Where(x => x.FileName == RemoveFileName).FirstOrDefault();
                    if (attachment != null)
                    {
                        AttachmentLst.Remove(attachment);
                        System.IO.File.Delete(Server.MapPath(Constants.DESIGN_ENGINEER_DOC_PATH_TEMP + attachment.FileName));
                    }

                    ViewState["AttachmentName"] = AttachmentLst;
                    BindDataToDocumentGrid();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// set file path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridDocuments_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    HyperLink document = item["FileName"].Controls[0] as HyperLink;
                    document.NavigateUrl = ConfigurationManager.AppSettings["MailUrl"] + ((Document)e.Item.DataItem).FilePath;
                    document.Target = "new";

                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// save uploaded document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LoadingPanel1.Visible = true;
                SaveDocument();
                LoadingPanel1.Visible = false;
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        protected void btnAddDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (radUploadDocument.UploadedFiles.Count > 0)
                {
                    int version = Convert.ToInt32(txtVersion.Text.Trim());

                    foreach (UploadedFile file in radUploadDocument.UploadedFiles)
                    {
                        String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                        string newfilename = file.GetNameWithoutExtension() + "_" + timeStamp + "_" + version + file.GetExtension();
                        string fileName = Constants.DESIGN_ENGINEER_DOC_PATH_TEMP + newfilename;
                        string temporaryPath = Server.MapPath(fileName);
                        file.SaveAs(temporaryPath, true);

                        if (Convert.ToString(ViewState["AttachmentName"]) != "")
                            AttachmentLst = (List<Document>)ViewState["AttachmentName"];

                        AttachmentLst.Add(new Document()
                        {
                            FileName = newfilename,
                            FilePath = fileName,
                            Comments = txtComments.Text.Trim(),
                            Version = version
                        });

                        ViewState["AttachmentName"] = AttachmentLst;
                        BindDataToDocumentGrid();
                    }

                    txtComments.Text = string.Empty;
                }
                else
                {
                    radNotificationMessage.Title = "Error";
                    radNotificationMessage.Show("File is not selected or uploaded file size is greater than 50MB.");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
            }
        }

        #endregion

        #region "Private Methods"
        private void UploadDocument(FileUploadedEventArgs e)
        {
            if (radUploadDocument.UploadedFiles.Count > 0)
            {
                int version = Convert.ToInt32(Session["HighestVersion"]);
                String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
                string newfilename = (e.File.GetNameWithoutExtension() + "_" + timeStamp + "_" + version + e.File.GetExtension()).Replace(" ", "_");
                e.File.SaveAs(Path.Combine(Server.MapPath(radUploadDocument.TargetFolder), newfilename));

                if (Convert.ToString(ViewState["AttachmentName"]) != "")
                    AttachmentLst = (List<Document>)ViewState["AttachmentName"];

                //Session["HighestVersion"] = version + 1;
                ViewState["AttachmentName"] = AttachmentLst;
                gridDocuments.DataSource = AttachmentLst;
                gridDocuments.DataBind();
            }
        }

        private void Initialize()
        {
            UserId = Convert.ToInt32(Session["UserId"]);
            if (Session[Constants.CONST_SESSION_DEDOCUMENT_ISREVIEW] != null && Session[Constants.CONST_SESSION_DEDOCUMENT_ISREVIEW].ToString() == "True")
                IsReview = true;
            else
                IsReview = false;

            SetControlValue();
        }

        private int GetDocumentVersionNumber()
        {
           return TaskModel.GetHighestVersionOfDocument(SAPSite, SAPProjectId, WBSAreaId, SAPNetwork, SAPActivity, SAPSubActivity);
        }

        private void SetControlValue()
        {
            if (Session[Constants.CONST_SESSION_UPLOAD_DOCUMENT_PARAMETER] != null)
            {
                SetScreenVariables();
                lblActivityName.InnerText = "Activity Name: " + ActivityName;
                if (!string.IsNullOrEmpty(SubActivityName))
                    lblSubActivityName.InnerText = "Sub - Activity Name: " + SubActivityName;
                RadDatePickerEstStart.SelectedDate = EstimatedStartDate;
                RadDatePickerEstEnd.SelectedDate = EstimatedEndDate;

                if (IsReview)
                {
                    txtVersion.Text = DocumentVersion.ToString();
                    txtVersion.Enabled = false;
                    reqComments.Enabled = true;
                    divReviewDescription.Visible = true;
                    divReleaseToConstruction.Visible = false;
                    divDateControls.Visible = false;
                    lblComments.InnerText = "Review Comments";
                }
                else
                {
                    if (Session["HighestVersion"] == null)
                        txtVersion.Text = Convert.ToString(GetDocumentVersionNumber());
                    txtVersion.Enabled = true;
                    reqComments.Enabled = false;
                    divReviewDescription.Visible = false;
                    divDateControls.Visible = true;
                    lblComments.InnerText = "Comments";
                    divReleaseToConstruction.Visible = true;
                }
            }
        }

        private void SetScreenVariables()
        {
            string jsonParameter = Session[Constants.CONST_SESSION_UPLOAD_DOCUMENT_PARAMETER].ToString();
            JObject parameters = JObject.Parse(jsonParameter);
            ActivityName = parameters["ActivityDescription"].Value<string>().Trim();
            SubActivityName = parameters["SAPSubActivityDescription"].Value<string>().Trim();
            ActivityId = parameters["ActivityId"].Value<int>();
            SubActivityId = parameters["SubActivityId"].Value<int>();
            SAPSite = parameters["SAPSite"].Value<string>();
            SAPProjectId = parameters["SAPProjectId"].Value<string>();
            SAPNetwork = parameters["SAPNetwork"].Value<string>();
            WBSAreaId = parameters["WBSAreaId"].Value<int>();
            SAPNetwork = parameters["SAPNetwork"].Value<string>();
            SAPActivity = parameters["SAPActivity"].Value<string>();
            SAPSubActivity = parameters["SapSubActivity"].Value<string>();
            EstimatedStartDate = Convert.ToDateTime(parameters["ActivityPlanStartDate"]);
            EstimatedEndDate = Convert.ToDateTime(parameters["ActivityPlanFinishDate"]);
            DocumentVersion = Convert.ToInt32(parameters["Version"]);
            DocumentDetailsId = Convert.ToInt32(parameters["DocumentDetailsId"]);
        }

        private void CopyFileToMainFolder(List<Document> lstSelectedDocument)
        {
            string sourcePath = Server.MapPath(Constants.DESIGN_ENGINEER_DOC_PATH_TEMP);
            string targetPath = Server.MapPath(Constants.DESIGN_ENGINEER_DOC_PATH);

            foreach (Document doc in lstSelectedDocument)
            {
                // Use Path class to manipulate file and directory paths.
                string sourceFile = System.IO.Path.Combine(sourcePath, doc.FileName);
                string destFile = System.IO.Path.Combine(targetPath, doc.FileName);

                // To copy a folder's contents to a new location:
                // Create a new target folder, if necessary.
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                // To copy a file to another location and 
                // overwrite the destination file if it already exists.
                System.IO.File.Copy(sourceFile, destFile, true);
            }

            System.IO.DirectoryInfo di = new DirectoryInfo(sourcePath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        private void BindDataToDocumentGrid()
        {
            gridDocuments.DataSource = AttachmentLst.OrderBy(d => d.Version);
            gridDocuments.DataBind();
        }

        private void SaveDocument()
        {
            List<Document> lstSelectedDocument = (List<Document>)ViewState["AttachmentName"];
            if (IsReview)
            {
                SaveReviewDetails(lstSelectedDocument);
            }
            else
            {
                if (lstSelectedDocument != null && lstSelectedDocument.Count > 0)
                    UploadDocument(lstSelectedDocument);
                else
                {
                    radNotificationMessage.Title = "Error";
                    radNotificationMessage.Show("File is not selected.");
                }
            }
        }

        private void SaveReviewDetails(List<Document> lstSelectedDocument)
        {
            UserId = Convert.ToInt32(Session["UserId"]);
            List<DocumentReviewFile> lstDocumentReviewFiles = new List<DocumentReviewFile>();
            if (lstSelectedDocument != null && lstSelectedDocument.Count > 0)
            {
                foreach (Document attachment in lstSelectedDocument)
                {
                    lstDocumentReviewFiles.Add(
                    new DocumentReviewFile()
                    {
                        DocumentDetailsId = DocumentDetailsId,
                        FilePath = attachment.FileName,
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    });
                }
            }

            DocumentReviewDetail documentReviewDetail = new DocumentReviewDetail()
            {
                DEDocumentId = DocumentDetailsId,
                ReviewComments = txtComments.Text.Trim(),
                ReviewDescription = txtReviewDescription.Text.Trim(),
                CreatedBy = UserId,
                CreatedOn = DateTime.Now,
                DEDocumentVersion = DocumentVersion ,                
                FilePath = lstSelectedDocument != null && lstSelectedDocument.Count > 0 ? lstSelectedDocument[0].FileName : string.Empty,
                ReviewDate = DateTime.Now,
                ModifiedBy = UserId,
                ModifiedOn = DateTime.Now,
                DocumentReviewFiles = lstDocumentReviewFiles
            };

            TaskModel.SaveReviewDetails(documentReviewDetail, UserId);
            CopyFileToMainFolder(lstSelectedDocument);
            radNotificationMessage.Title = "Success";
            radNotificationMessage.Show("Records saved successfully.");
            TimesheetManagementRadAjaxManager.ResponseScripts.Add("CloseModal();");
        }

        private void UploadDocument(List<Document> lstSelectedDocument)
        {
            UserId = Convert.ToInt32(Session["UserId"]);
            List<DEDocumentFile> lstDocuments = new List<DEDocumentFile>();
            if (lstSelectedDocument != null && lstSelectedDocument.Count > 0)
            {
                foreach (Document attachment in lstSelectedDocument)
                {
                    lstDocuments.Add(new DEDocumentFile()
                    {
                        FilePath = attachment.FileName,
                        DocumentDetailsId = 0,
                        CreatedOn = DateTime.Now,
                        CreatedBy = UserId
                    });
                }
            }

            DesignEngineerDocument designEngineerDocument = new DesignEngineerDocument()
            {
                DEDocumentFiles = lstDocuments,
                Version = Convert.ToInt32(txtVersion.Text.Trim()),
                Comments = txtComments.Text.Trim(),
                ActivityId = 0,
                SubActivityId = 0,
                SAPSite = SAPSite,
                SAPProject = SAPProjectId,
                WBSAreaId = WBSAreaId,
                SAPNetwork = SAPNetwork,
                SAPActivity = SAPActivity,
                SAPSubActivity = SAPSubActivity,
                ReleaseToContsruction = Convert.ToBoolean(Convert.ToInt32(drpReleaseToConstructoin.SelectedValue)),
                CreatedBy = UserId,
                ModifiedBy = UserId,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            TaskModel.SaveDesignEngineerDocument(designEngineerDocument, UserId);
            CopyFileToMainFolder(lstSelectedDocument);

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendDEDocumentNotification"]))
                NotificationHelper.SendDEDocumentNotification(UserId, ActivityId, SubActivityId);

            radNotificationMessage.Title = "Success";
            radNotificationMessage.Show("Records saved successfully.");
            TimesheetManagementRadAjaxManager.ResponseScripts.Add("CloseModal();");
        }
        #endregion
    }

    [Serializable]
    public class Document
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Comments { get; set; }

        public int Version { get; set; }
    }
}
