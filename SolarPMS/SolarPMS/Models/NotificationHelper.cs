using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace SolarPMS.Models
{
    public class NotificationHelper
    {

        #region "Issue Notification"
        public static bool SendIssueAssignementNotification(IssueManagement issueManagement,int CreatorId, string UserName)
        {
            string activityDescription = GetActivityDescription(issueManagement);
            if (!string.IsNullOrEmpty(activityDescription))
                return SendIssueManagementNotification(issueManagement, UserName, activityDescription, Constants.NotificationType.ActivityIssueAssigned);
            else
                return SendIssueManagementNotification(issueManagement, UserName, activityDescription, Constants.NotificationType.IssueAssigned);
        }

        public static bool SendIssueResolutionNotification(IssueManagement issueManagement, string UserName)
        {
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
            ActivityModel activity = new ActivityModel();
            string activityDescription = GetActivityDescription(issueManagement);
            if (!string.IsNullOrEmpty(activityDescription))
                return SendIssueManagementNotification(issueManagement, UserName, activityDescription, Constants.NotificationType.ActivityIssueResolution);
            else
                return SendIssueManagementNotification(issueManagement, UserName, activityDescription, Constants.NotificationType.IssueResolution);
        }

        public static bool SendIssueClosedNotification(IssueManagement issueManagement, string UserName)
        {
            
            string activityDescription = GetActivityDescription(issueManagement);
            if (!string.IsNullOrEmpty(activityDescription))
                return SendIssueManagementNotification(issueManagement, UserName, activityDescription, Constants.NotificationType.ActivityIssueClosed);
            else
                return SendIssueManagementNotification(issueManagement, UserName, activityDescription, Constants.NotificationType.IssueClosed);
        }

        private static string GetActivityDescription(IssueManagement issueManagement)
        {
            ActivityModel activity = new ActivityModel();
            string activityDescription = string.Empty;

            if (issueManagement.SubActivityId != 0)
            {
                var subActivityResult = activity.GetSAPMasterBySubActitiyId(new SubActivity() { SubActivityId = issueManagement.SubActivityId.Value });
                if (subActivityResult != null)
                    activityDescription = subActivityResult.SAPSubActivityDescription;
            }
            else
            {
                var activityResult = activity.GetSAPMasterByActitiyId(new Activity() { ActivityId = issueManagement.ActivityId.Value });
                if (activityResult != null)
                    activityDescription = activityResult.ActivityDescription; ;
            }

            return activityDescription;
        }

        private static bool SendIssueManagementNotification(IssueManagement issueManagement, string UserName, string ActivityDescription, Constants.NotificationType Notification)
        {
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
            int notificationId = Convert.ToInt32(Notification);
            DataRow template = NotificationHelper.ReadNotificationTemplate(notificationId);
            switch (Notification)
            {
                case Constants.NotificationType.ActivityIssueAssigned:
                case Constants.NotificationType.ActivityIssueResolution:
                case Constants.NotificationType.ActivityIssueClosed:
                    messageBody.AppendFormat(template[2].ToString(), UserName, issueManagement.IssueId, issueManagement.Description, ActivityDescription, 
                        ConfigurationManager.AppSettings["MailUrl"] + "IssueManagementDetail.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("IssueId =" + issueManagement.IssueId)));
                    break;
                case Constants.NotificationType.IssueAssigned:
                case Constants.NotificationType.IssueResolution:
                case Constants.NotificationType.IssueClosed:
                    messageBody.AppendFormat(template[2].ToString(), UserName, issueManagement.IssueId, issueManagement.Description, 
                        ConfigurationManager.AppSettings["MailUrl"] + "IssueManagementDetail.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("IssueId =" + issueManagement.IssueId)));
                    break;
            }
            
            subject.AppendFormat(template[1].ToString(), issueManagement.IssueId);
            string userEmail = EmailAddressHelper.GetEmailIdForNotification(0, 0, 0, notificationId, issueManagement.CreatedBy, issueManagement.AssignedTo, template[3].ToString());
            return CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject.ToString());
        }

        #endregion

        #region "User Notification"
        public static bool SendUserCreationNotification(string UserName, int UserId, string LoginName, string Password)
        {
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
            int notificationId = Convert.ToInt32(Constants.NotificationType.NewUserCreation);
            DataRow template = NotificationHelper.ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(), UserName, ConfigurationManager.AppSettings["MailUrl"], LoginName, Password);

           subject.AppendFormat(template[1].ToString(), UserName);
            string userEmail = EmailAddressHelper.GetEmailIdForNotification(0, 0, 0, notificationId, UserId, 0, template[3].ToString());
            return CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject.ToString());
        }

        public static bool SendUserDeactivatedNotification(int UserId, string UserName)
        {
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
            int notificationId = Convert.ToInt32(Constants.NotificationType.UserDeactivation);
            DataRow template = NotificationHelper.ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(), UserName);
            subject.AppendFormat(template[1].ToString());
            string userEmail = EmailAddressHelper.GetEmailIdForNotification(0, 0, 0, notificationId, UserId, 0, template[3].ToString());
            return CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject.ToString());
        }

        #endregion

        #region "Activity"

        public static void SendQualityRejectNotification(int ActivityId, int SubActivityId, int TimesheetId, string QualityRemark)
        {
            string activity = GetActivityDescription(TimesheetId);
            StringBuilder messageBody = new StringBuilder();
            int notificationId = Convert.ToInt32(Constants.NotificationType.QualityRejection);
            DataRow template = NotificationHelper.ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(), activity, QualityRemark,
                ConfigurationManager.AppSettings["MailUrl"] + "TimesheetManagement.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("TimesheetId=" + TimesheetId)));
            string subject = template[1].ToString();
            string userEmail = EmailAddressHelper.GetEmailIdForNotification(ActivityId, SubActivityId, TimesheetId, notificationId, 0, 0, template[3].ToString());
            CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject.ToString());
        }

        private static string GetActivityDescription(int TimesheetId)
        {
            string activity;
            TimesheetActivityDetails activityDetails = TimesheetModel.GetTimesheetActivityDetails(TimesheetId);
            if (!string.IsNullOrEmpty(activityDetails.SAPSubActivityDescription))
                activity = activityDetails.SAPSubActivityDescription;
            else
                activity = activityDetails.ActivityDescription;
            return activity;
        }

        public static void SendQualityResoulutionNotification(int ActivityId, int SubActivityId, int TimesheetId, string UserName)
        {
            
            StringBuilder messageBody = new StringBuilder();
            int notificationId = Convert.ToInt32(Constants.NotificationType.QualityResolution);
            DataRow template = NotificationHelper.ReadNotificationTemplate(notificationId);
            string activity = GetActivityDescription(TimesheetId);
            messageBody.AppendFormat(template[2].ToString(), activity, UserName,
                ConfigurationManager.AppSettings["MailUrl"] + "TimesheetManagement.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("TimesheetId =" + TimesheetId)));
            string subject = template[1].ToString();
            string userEmail = EmailAddressHelper.GetEmailIdForNotification(ActivityId, SubActivityId, TimesheetId, notificationId,0,0,template[3].ToString());
            CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject.ToString());
        }

        public static void SendQualityIssueClosedNotification(int ActivityId, int SubActivityId, int TimesheetId)
        {
            StringBuilder messageBody = new StringBuilder();
            int notificationId = Convert.ToInt32(Constants.NotificationType.QualityRejectClosed);
            DataRow template = NotificationHelper.ReadNotificationTemplate(notificationId);
            string activity = GetActivityDescription(TimesheetId);
            messageBody.AppendFormat(template[2].ToString(), activity, 
                ConfigurationManager.AppSettings["MailUrl"] + "TimesheetManagement.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("TimesheetId =" + TimesheetId)));
            string subject = template[1].ToString();
            string userEmail = EmailAddressHelper.GetEmailIdForNotification(ActivityId, SubActivityId, TimesheetId, notificationId, 0, 0, template[3].ToString());
            CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject.ToString());
        }

        public static void SendSiteManagerRejectionNotification(int TimesheetId)
        {
            int userId = EmailAddressHelper.GetTimesheetUserId(TimesheetId);
            TimesheetActivityDetails activityDetails = TimesheetModel.GetTimesheetActivityDetails(TimesheetId);
            StringBuilder messageBody = new StringBuilder();
            DataRow template = NotificationHelper.ReadNotificationTemplate(Convert.ToInt32(Constants.NotificationType.SiteManagerRejection));
            if (!string.IsNullOrEmpty(activityDetails.SAPSubActivityDescription))
                messageBody.AppendFormat(template[2].ToString(), activityDetails.SAPSubActivityDescription, ConfigurationManager.AppSettings["MailUrl"] + "IssueManagementDetail.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("TimesheetId =" + TimesheetId)));
            else
                messageBody.AppendFormat(template[2].ToString(), activityDetails.ActivityDescription, 
                    ConfigurationManager.AppSettings["MailUrl"] + "TimesheetManagement.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("TimesheetId =" + TimesheetId)));

            string subject = template[1].ToString();
            string userEmail = EmailAddressHelper.GetEmailIdForNotification(0, 0, TimesheetId, 0, userId, 0, template[3].ToString());
            CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject.ToString());
        }

        public static void SendTaskAllocationNotification(int UserId)
        {
            List<string> mailSentToNetwork = new List<string>();
            List<string> mailSentToSite = new List<string>();
            List<ActivityToSendNotification> lstAactivity = ActivityModel.GetActivityAssignmentDetailsForNotification(UserId);
            StringBuilder messageBody = new StringBuilder();
            string url = ConfigurationManager.AppSettings["MailUrl"]
                + "ToDoList.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("FromNotification = true"));
            DataRow activityAllocationMessageBody= NotificationHelper.ReadNotificationTemplate(Convert.ToInt32(Constants.NotificationType.ActivityAllocation));
            DataRow activityRemovedMessageBody = NotificationHelper.ReadNotificationTemplate(Convert.ToInt32(Constants.NotificationType.ActivityRemoved));
            string subject = string.Empty;
            string profiles = string.Empty;
            foreach (var activity in lstAactivity)
            {
                messageBody = new StringBuilder();
                if (activity.IsAssigned)
                {
                    messageBody.AppendFormat(activityAllocationMessageBody[2].ToString(), activity.Name, activity.NetworkDescription, activity.SAPSite, url);
                    subject = activityAllocationMessageBody[1].ToString();
                    profiles = activityAllocationMessageBody[3].ToString();
                }
                else
                {
                    messageBody.AppendFormat(activityRemovedMessageBody[2].ToString(), activity.Name, activity.NetworkDescription, activity.SAPSite, url);
                    subject = activityRemovedMessageBody[1].ToString();
                    profiles = activityRemovedMessageBody[3].ToString();
                }
                
                string userEmail = EmailAddressHelper.GetEmailIdForNotification(0, 0, 0, 0, UserId, 0, profiles.ToString());
                CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject);
                mailSentToNetwork.Add(activity.SAPNetwork);
                mailSentToSite.Add(activity.SAPSite);
            }

            UpdateActivityNotificationFlag(UserId, mailSentToNetwork, mailSentToSite);
        }
        #endregion

        #region "DE Document"
        public static void SendDEDocumentNotification(int UserId, int ActivityId, int SubActivityId)
        {
            List<string> mailSentToNetwork = new List<string>();
            List<string> mailSentToSite = new List<string>();
            StringBuilder messageBody = new StringBuilder();
            string url = ConfigurationManager.AppSettings["MailUrl"]
                + "DesignDocumentUpload.aspx?" + HttpUtility.UrlEncode(Cryptography.Crypto.Instance.Encrypt("FromNotification = true"));
            int NotificationId = Convert.ToInt32(Constants.NotificationType.DEDocumentUpload);
            DataRow DEDocumentUploadMessageBody = NotificationHelper.ReadNotificationTemplate(NotificationId);
            
            string subject = string.Empty;
            string profiles = string.Empty;
            messageBody = new StringBuilder();
            ActivityModel activityModel = new ActivityModel();
            Activity activity = new Activity();
            activity.ActivityId = ActivityId;
            dynamic activityDetails = activityModel.GetSAPMasterByActitiyId(activity);
            subject = DEDocumentUploadMessageBody[1].ToString();
            messageBody.AppendFormat(DEDocumentUploadMessageBody[2].ToString(), activityDetails.ActivityDescription, activityDetails.SAPSite, url);
            profiles = DEDocumentUploadMessageBody[3].ToString() + "," + UserModel.GetDEDocumentViewAccessProfiles();

            string userEmail = EmailAddressHelper.GetEmailIdForNotification(ActivityId, SubActivityId, 0, NotificationId, UserId, 0, profiles.ToString());
            if (!string.IsNullOrEmpty(userEmail))
                CommonFunctions.SendEmail(userEmail, string.Empty, messageBody.ToString(), subject);
        }
        #endregion
        public static DataRow ReadNotificationTemplate(int TempalteId)
        {
            DataSet dataSet = new DataSet();
            string notificationFilePath = ConfigurationManager.AppSettings["NotificationFilePath"];
            dataSet.ReadXml(notificationFilePath);
            if (dataSet != null)
            {
                DataTable notificationTempalte = dataSet.Tables[0];
                DataRow template = notificationTempalte.Select("Id='" + TempalteId + "'").FirstOrDefault();
                template[1] = template[1].ToString().Replace("\n", "<br/>");
                template[2]= template[2].ToString().Replace("\n", "<br/>");
                return template;
            }

            return null;
        }

        public static void UpdateActivityNotificationFlag(int UserId, List<string> NetWorkList, List<string> SiteList)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var activityHistory = solarPMSEntities.ActivityAssignmentHistories
                                     .Where(a => a.UserId == UserId
                                     && a.IsNotificationSent == false
                                     && NetWorkList.Contains(a.SAPNetwork)
                                     && SiteList.Contains(a.SAPSite)
                                     ).ToList();
                if (activityHistory != null)
                {
                    activityHistory.ForEach(l => l.IsNotificationSent = true);
                    solarPMSEntities.SaveChanges();
                }
            }
        }
    }
}