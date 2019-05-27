using Cryptography;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace SuzlonBPP.Models
{
    public class NotificationModel
    {
        #region "Workflow"

        public void SendSecActorAssignNotification(SecUserNotification secUserNotification, SuzlonBPPEntities suzlonBPPEntities, string workflowName)
        {
            User userDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == secUserNotification.AssignedUserId);
            if (userDetail != null)
            {
                string priUserEmailId = string.Empty;
                User priUserDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == secUserNotification.PriActorUserId);
                if (priUserDetail != null)
                    priUserEmailId = priUserDetail.EmailId;
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append("Dear Sir/Madam,");
                mailBody.AppendFormat("<br><br>User " + userDetail.Name + " has been assigned as secondary user to "+ workflowName + " Workflow and " + secUserNotification.SubVerticalName + " for the period of " + secUserNotification.FromDate + " to " + secUserNotification.ToDate + ".");
                mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);
                string mailSubject = workflowName + " Workflow: Secondary user Assigned to stage " + secUserNotification.StageName + " for " + secUserNotification.SubVerticalName;
                CommonFunctions.SendEmail(userDetail.EmailId + ";" + priUserEmailId, string.Empty, mailBody.ToString(), mailSubject, null, "");
            }
        }

        public void SendSecActorUnAssignNotification(SecUserNotification secUserNotification, SuzlonBPPEntities suzlonBPPEntities, string workflowName)
        {
            User userDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == secUserNotification.AssignedUserId);
            if (userDetail != null)
            {
                string priUserEmailId = string.Empty;
                User priUserDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == secUserNotification.PriActorUserId);
                if (priUserDetail != null)
                    priUserEmailId = priUserDetail.EmailId;
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append("Dear Sir/Madam,");
                mailBody.AppendFormat("<br><br>User " + userDetail.Name + " has been un-assigned as secondary user to " + workflowName + " Workflow and " + secUserNotification.SubVerticalName + ".");
                mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);
                string mailSubject = workflowName + " Workflow: Secondary user Un-Assigned to stage " + secUserNotification.StageName + " for " + secUserNotification.SubVerticalName;
                CommonFunctions.SendEmail(userDetail.EmailId + ";" + priUserEmailId, string.Empty, mailBody.ToString(), mailSubject, null, "");
            }
        }

        public static bool SendTreasuryInitiationNotification(string TreasuryAlloctionNo, string InitiatorUserName, string RequestedAmount, string ToEmailId,string TreasuryId)
        {
            try
            {
                StringBuilder messageBody = new StringBuilder();
                StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("TreasuryId=" + TreasuryId));
                UrlPath = "<a href='"+url+"'>" + url + "</a>";
                int notificationId = Convert.ToInt32(NotificationTypeEnum.TreasuryRequestSubmit);

                DataRow template = ReadNotificationTemplate(notificationId);
                messageBody.AppendFormat(template[2].ToString(), InitiatorUserName, TreasuryAlloctionNo, RequestedAmount, UrlPath);

                subject.AppendFormat(template[1].ToString(), TreasuryAlloctionNo);

                return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        public static bool SendTreasuryApproveNotification(string TreasuryAlloctionNo, string ApproverName, string RequestedAmount, string ApprovedAmout, string ToEmailId,string CBEmailId, string TreasuryId)
        {
            try
            { 
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("TreasuryId=" + TreasuryId));
             UrlPath = "<a href='" + url + "'>" + url + "</a>";
                int notificationId = Convert.ToInt32(NotificationTypeEnum.TreasuryRequestApproved);

            DataRow template = ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(),TreasuryAlloctionNo, ApproverName, RequestedAmount, ApprovedAmout, UrlPath);

            subject.AppendFormat(template[1].ToString(), TreasuryAlloctionNo, ApproverName);

            return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString(),null,string.Empty,CBEmailId);
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        public static bool SendTreasuryRejectNotification(string TreasuryAlloctionNo, string ApproverName,string Comment,string ToEmailId, string TreasuryId)
        {
            try
            { 
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("TreasuryId=" + TreasuryId));
                UrlPath = "<a href='" + url + "'>" + url + "</a>";
                int notificationId = Convert.ToInt32(NotificationTypeEnum.TreasuryRequestReject);

            DataRow template = ReadNotificationTemplate(notificationId);

            messageBody.AppendFormat(template[2].ToString(), TreasuryAlloctionNo, ApproverName, Comment, UrlPath);

            subject.AppendFormat(template[1].ToString(), TreasuryAlloctionNo, ApproverName);

            return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        public static bool SendTreasuryReworkNotification(string TreasuryAlloctionNo, string ApproverName, string Comment, string ToEmailId, string TreasuryId)
        {
            try
            { 
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("TreasuryId=" + TreasuryId));
                UrlPath = "<a href='" + url + "'>" + url + "</a>";
                int notificationId = Convert.ToInt32(NotificationTypeEnum.TreasuryRequestNeedCorrection);

            DataRow template = ReadNotificationTemplate(notificationId);

            messageBody.AppendFormat(template[2].ToString(), TreasuryAlloctionNo, Comment, UrlPath);

            subject.AppendFormat(template[1].ToString(), TreasuryAlloctionNo, ApproverName);

            return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }
        public static bool SendAddendumInitiationNotification(string TreasuryAlloctionNo, string InitiatorUserName, string ToEmailId, string TreasuryId)
        {
            try
            { 
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("TreasuryId=" + TreasuryId));
                UrlPath = "<a href='" + url + "'>" + url + "</a>";
                int notificationId = Convert.ToInt32(NotificationTypeEnum.AddendumRequestSubmit);

            DataRow template = ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(), InitiatorUserName, TreasuryAlloctionNo, UrlPath);

            subject.AppendFormat(template[1].ToString(), TreasuryAlloctionNo);

            return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        public static bool SendAddendumApproveNotification(string TreasuryAlloctionNo, string ApproverName, string RequestedAmount, string ApprovedAmout, string ToEmailId, string TreasuryId)
        {
            try
            { 
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("TreasuryId=" + TreasuryId));
                UrlPath = "<a href='" + url + "'>" + url + "</a>";
                int notificationId = Convert.ToInt32(NotificationTypeEnum.AddendumRequestApproved);

            DataRow template = ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(), TreasuryAlloctionNo, ApproverName, RequestedAmount, ApprovedAmout, UrlPath);

            subject.AppendFormat(template[1].ToString(), TreasuryAlloctionNo);

            return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        public static bool SendAddendumRejectNotification(string TreasuryAlloctionNo, string ApproverName,string Comment , string ToEmailId, string TreasuryId)
        {
            try
            { 
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("TreasuryId=" + TreasuryId));
                UrlPath = "<a href='" + url + "'>" + url + "</a>";
                int notificationId = Convert.ToInt32(NotificationTypeEnum.AddendumRequestReject);

            DataRow template = ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(), TreasuryAlloctionNo, ApproverName,Comment, UrlPath);

            subject.AppendFormat(template[1].ToString(), TreasuryAlloctionNo);

            return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }
        public static bool SendAddendumReworkNotification(string TreasuryAlloctionNo, string Comment, string ToEmailId, string TreasuryId)
        {
            try
            { 
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("TreasuryId=" + TreasuryId));
                UrlPath = "<a href='" + url + "'>" + url + "</a>";
                int notificationId = Convert.ToInt32(NotificationTypeEnum.AddendumRequestNeedCorrection);

            DataRow template = ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(), TreasuryAlloctionNo,Comment, UrlPath);

            subject.AppendFormat(template[1].ToString(), TreasuryAlloctionNo);

            return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        public static bool SendPaymentInitiationNotification(string InitiatorName, string DocumentNumber,string BillType, string CompanyCode,string VendorName, string RequestedAmount, string ToEmailId)
        {
            try
            { 
            StringBuilder messageBody = new StringBuilder();
            StringBuilder subject = new StringBuilder();
                string UrlPath = string.Empty;
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?";

            int notificationId = Convert.ToInt32(NotificationTypeEnum.PaymentRequestSubmit);

            DataRow template = ReadNotificationTemplate(notificationId);
            messageBody.AppendFormat(template[2].ToString(),InitiatorName, BillType, DocumentNumber,CompanyCode, VendorName, RequestedAmount, url);

            subject.AppendFormat(template[1].ToString(), InitiatorName, DocumentNumber, BillType,CompanyCode, VendorName);

            return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }
        public static bool SendPaymentApproveNotification(string PreviousUserName, string DocumentNumber, string BillType, string VendorName, string RequestedAmount,string ApproveAmount,string ToEmailId)
        {
            try
            {
                StringBuilder messageBody = new StringBuilder();
                StringBuilder subject = new StringBuilder();

                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?";

                int notificationId = Convert.ToInt32(NotificationTypeEnum.PaymentRequestApprove);

                DataRow template = ReadNotificationTemplate(notificationId);
                messageBody.AppendFormat(template[2].ToString(), VendorName, BillType, DocumentNumber, PreviousUserName, RequestedAmount, ApproveAmount, url);

                subject.AppendFormat(template[1].ToString(), VendorName,BillType, DocumentNumber, PreviousUserName);

                return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        public static bool SendPaymentRejectNotification(string ApproverName, string DocumentNumber, string BillType,string VendorName, string RejectionComment,string ToEmailId)
        {
            try
            {
                StringBuilder messageBody = new StringBuilder();
                StringBuilder subject = new StringBuilder();
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?";

                int notificationId = Convert.ToInt32(NotificationTypeEnum.PaymentRequestReject);

                DataRow template = ReadNotificationTemplate(notificationId);
                messageBody.AppendFormat(template[2].ToString(), BillType, DocumentNumber, ApproverName, RejectionComment, url);

                subject.AppendFormat(template[1].ToString(), VendorName, BillType, DocumentNumber, ApproverName);

                return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

        public static bool SendPaymentNeedCorrectionNotification(string ApproverName, string DocumentNumber, string BillType, string VendorName, string CorrectionComment, string ToEmailId)
        {
            try
            {
                StringBuilder messageBody = new StringBuilder();
                StringBuilder subject = new StringBuilder();
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?";

                int notificationId = Convert.ToInt32(NotificationTypeEnum.PaymentRequestReject);

                DataRow template = ReadNotificationTemplate(notificationId);
                messageBody.AppendFormat(template[2].ToString(), VendorName, BillType, DocumentNumber, CorrectionComment, url);

                subject.AppendFormat(template[1].ToString(), VendorName, BillType, DocumentNumber, ApproverName);

                return CommonFunctions.SendEmail(ToEmailId, string.Empty, messageBody.ToString(), subject.ToString());
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return false;
            }
        }

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
                template[2] = template[2].ToString().Replace("\n", "<br/>");
                return template;
            }

            return null;
        }
        #endregion
    }
}