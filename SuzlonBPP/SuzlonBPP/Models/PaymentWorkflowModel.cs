using SAP.Connector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SuzlonBPP.Models
{
    public class PaymentWorkflowModel
    {
        public PaymentWorkflow paymentWorkflow;
        public SubVerticalMaster subVerticalMaster;

        public List<PaymentWorkflowModel> GetPaymentWorkflow(string subVerticalIds)
        {
            List<int> subVertical = subVerticalIds.Split(',').Select(Int32.Parse).ToList();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var workflowDetails = (from subvertical in suzlonBPPEntities.SubVerticalMasters
                                       join workflow in suzlonBPPEntities.PaymentWorkflows
                                       on subvertical.SubVerticalId equals workflow.SubVerticalId into sw
                                       from workflow in sw.DefaultIfEmpty()
                                       where subVertical.Contains(subvertical.SubVerticalId)
                                       select new PaymentWorkflowModel
                                       {
                                           subVerticalMaster = subvertical,
                                           paymentWorkflow = workflow
                                       }).ToList();
                return workflowDetails;
            }
        }

        /// <summary>
        /// This method is used to save/update payment workflow details.
        /// </summary>
        /// <param name="paymentWorkflow"></param>
        /// <returns></returns>
        public bool SavePaymentWorkflow(BankWorkflow paymentWorkflow, int userId)
        {
            bool isSave = false;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                PaymentWorkflow workflow = suzlonBPPEntities.PaymentWorkflows.AsNoTracking().FirstOrDefault(w => w.SubVerticalId == paymentWorkflow.SubVerticalId);
                PaymentWorkflow oldWorkflowDetails = suzlonBPPEntities.PaymentWorkflows.AsNoTracking().FirstOrDefault(w => w.SubVerticalId == paymentWorkflow.SubVerticalId);
                if (workflow == null)
                {
                    PaymentWorkflow workfloWtoAdd = new PaymentWorkflow();
                    workfloWtoAdd.SubVerticalId = paymentWorkflow.SubVerticalId;
                    workfloWtoAdd.VerticalId = paymentWorkflow.VerticalId;
                    workfloWtoAdd.PriAggUserId = paymentWorkflow.PriVerContUserId;
                    workfloWtoAdd.PriContUserId = paymentWorkflow.PriGrpContUserId;
                    workfloWtoAdd.PriExpAppUserId = paymentWorkflow.PriTreasuryUserId;
                    workfloWtoAdd.PriAuditorUserId = paymentWorkflow.PriMgmtAssUserId;
                    workfloWtoAdd.PriFASSCDTUserId = paymentWorkflow.PriFASSCUserId;
                    workfloWtoAdd.PriFASSCCBUserId = paymentWorkflow.PriCBUserId;
                    workfloWtoAdd.SecAggUserId = paymentWorkflow.SecVerContUserId;
                    workfloWtoAdd.SecContUserId = paymentWorkflow.SecGrpContUserId;
                    workfloWtoAdd.SecExpAppUserId = paymentWorkflow.SecTreasuryUserId;
                    workfloWtoAdd.SecAuditorUserId = paymentWorkflow.SecMgmtAssUserId;
                    workfloWtoAdd.SecFASSCDTUserId = paymentWorkflow.SecFASSCUserId;
                    workfloWtoAdd.SecFASSCCBUserId = paymentWorkflow.SecCBUserId;
                    workfloWtoAdd.SecAggFromDt = paymentWorkflow.SecVerContFromDt;
                    workfloWtoAdd.SecAggToDt = paymentWorkflow.SecVerContToDt;
                    workfloWtoAdd.SecContFromDt = paymentWorkflow.SecGrpContFromDt;
                    workfloWtoAdd.SecContToDt = paymentWorkflow.SecGrpContToDt;
                    workfloWtoAdd.SecExpAppFromDt = paymentWorkflow.SecTreasuryFromDt;
                    workfloWtoAdd.SecExpAppToDt = paymentWorkflow.SecTreasuryToDt;
                    workfloWtoAdd.SecAuditorFromDt = paymentWorkflow.SecMgmtAssFromDt;
                    workfloWtoAdd.SecAuditorToDt = paymentWorkflow.SecMgmtAssToDt;
                    workfloWtoAdd.SecFASSCDTFromDt = paymentWorkflow.SecFASSCFromDt;
                    workfloWtoAdd.SecFASSCDTToDt = paymentWorkflow.SecFASSCToDt;
                    workfloWtoAdd.SecFASSCCBFromDt = paymentWorkflow.SecCBFromDt;
                    workfloWtoAdd.SecFASSCCBToDt = paymentWorkflow.SecCBToDt;
                    workfloWtoAdd.CreatedBy = userId;
                    workfloWtoAdd.CreatedOn = DateTime.Now;
                    workfloWtoAdd.ModifiedBy = userId;
                    workfloWtoAdd.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.PaymentWorkflows.Add(workfloWtoAdd);
                }
                else
                {
                    workflow.SubVerticalId = paymentWorkflow.SubVerticalId;
                    workflow.VerticalId = paymentWorkflow.VerticalId;
                    workflow.PriAggUserId = paymentWorkflow.PriVerContUserId;
                    workflow.PriContUserId = paymentWorkflow.PriGrpContUserId;
                    workflow.PriExpAppUserId = paymentWorkflow.PriTreasuryUserId;
                    workflow.PriAuditorUserId = paymentWorkflow.PriMgmtAssUserId;
                    workflow.PriFASSCDTUserId = paymentWorkflow.PriFASSCUserId;
                    workflow.PriFASSCCBUserId = paymentWorkflow.PriCBUserId;
                    workflow.SecAggUserId = paymentWorkflow.SecVerContUserId;
                    workflow.SecContUserId = paymentWorkflow.SecGrpContUserId;
                    workflow.SecExpAppUserId = paymentWorkflow.SecTreasuryUserId;
                    workflow.SecAuditorUserId = paymentWorkflow.SecMgmtAssUserId;
                    workflow.SecFASSCDTUserId = paymentWorkflow.SecFASSCUserId;
                    workflow.SecFASSCCBUserId = paymentWorkflow.SecCBUserId;
                    workflow.SecAggFromDt = paymentWorkflow.SecVerContFromDt;
                    workflow.SecAggToDt = paymentWorkflow.SecVerContToDt;
                    workflow.SecContFromDt = paymentWorkflow.SecGrpContFromDt;
                    workflow.SecContToDt = paymentWorkflow.SecGrpContToDt;
                    workflow.SecExpAppFromDt = paymentWorkflow.SecTreasuryFromDt;
                    workflow.SecExpAppToDt = paymentWorkflow.SecTreasuryToDt;
                    workflow.SecAuditorFromDt = paymentWorkflow.SecMgmtAssFromDt;
                    workflow.SecAuditorToDt = paymentWorkflow.SecMgmtAssToDt;
                    workflow.SecFASSCDTFromDt = paymentWorkflow.SecFASSCFromDt;
                    workflow.SecFASSCDTToDt = paymentWorkflow.SecFASSCToDt;
                    workflow.SecFASSCCBFromDt = paymentWorkflow.SecCBFromDt;
                    workflow.SecFASSCCBToDt = paymentWorkflow.SecCBToDt;
                    workflow.ModifiedBy = userId;
                    workflow.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(workflow).State = EntityState.Modified;
                   
                }
                suzlonBPPEntities.SaveChanges();
                SecUserNotification SecUserNotification = new SecUserNotification();
                NotificationModel notificationModel = new NotificationModel();
                string subVerticalName = string.Empty;
                string workFlowName = "Payment";
                var subVertical = suzlonBPPEntities.SubVerticalMasters.FirstOrDefault(u => u.SubVerticalId == paymentWorkflow.SubVerticalId);
                if (subVertical != null)
                    subVerticalName = subVertical.Name;
                SecUserNotification.SubVerticalName = subVerticalName;
                if ((oldWorkflowDetails == null && paymentWorkflow.SecVerContUserId.HasValue) ||
                    (oldWorkflowDetails != null && ((oldWorkflowDetails.SecAggUserId == null && paymentWorkflow.SecVerContUserId.HasValue) ||
                    (oldWorkflowDetails.SecAggUserId.HasValue && paymentWorkflow.SecVerContUserId.HasValue && oldWorkflowDetails.SecAggUserId.Value != paymentWorkflow.SecVerContUserId.Value))))
                {
                    SecUserNotification.StageName = "Aggregator";
                    SecUserNotification.FromDate = (paymentWorkflow.SecVerContFromDt.HasValue ? paymentWorkflow.SecVerContFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (paymentWorkflow.SecVerContToDt.HasValue ? paymentWorkflow.SecVerContToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = paymentWorkflow.SecVerContUserId.Value;
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriVerContUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (oldWorkflowDetails != null && (oldWorkflowDetails.SecAggUserId.HasValue && paymentWorkflow.SecVerContUserId.HasValue && oldWorkflowDetails.SecAggUserId.Value != paymentWorkflow.SecVerContUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = oldWorkflowDetails.SecAggUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (oldWorkflowDetails != null && ((oldWorkflowDetails.SecAggUserId.HasValue && paymentWorkflow.SecVerContUserId == null)))
                {
                    SecUserNotification.StageName = "Aggregator";
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriVerContUserId;
                    SecUserNotification.AssignedUserId = oldWorkflowDetails.SecAggUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((oldWorkflowDetails == null && paymentWorkflow.SecGrpContUserId.HasValue) ||
                    (oldWorkflowDetails != null && ((oldWorkflowDetails.SecContUserId == null && paymentWorkflow.SecGrpContUserId.HasValue) ||
                    (oldWorkflowDetails.SecContUserId.HasValue && paymentWorkflow.SecGrpContUserId.HasValue && oldWorkflowDetails.SecContUserId.Value != paymentWorkflow.SecGrpContUserId.Value))))
                {
                    SecUserNotification.StageName = "Controller";
                    SecUserNotification.FromDate = (paymentWorkflow.SecGrpContFromDt.HasValue ? paymentWorkflow.SecGrpContFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (paymentWorkflow.SecGrpContToDt.HasValue ? paymentWorkflow.SecGrpContToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = paymentWorkflow.SecGrpContUserId.Value;
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriGrpContUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (oldWorkflowDetails != null && (oldWorkflowDetails.SecContUserId.HasValue && paymentWorkflow.SecGrpContUserId.HasValue && oldWorkflowDetails.SecContUserId.Value != paymentWorkflow.SecGrpContUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = oldWorkflowDetails.SecContUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (oldWorkflowDetails != null && ((oldWorkflowDetails.SecContUserId.HasValue && paymentWorkflow.SecGrpContUserId == null)))
                {
                    SecUserNotification.StageName = "Controller";
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriGrpContUserId;
                    SecUserNotification.AssignedUserId = oldWorkflowDetails.SecContUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((oldWorkflowDetails == null && paymentWorkflow.SecTreasuryUserId.HasValue) ||
                    (oldWorkflowDetails != null && ((oldWorkflowDetails.SecExpAppUserId == null && paymentWorkflow.SecTreasuryUserId.HasValue) ||
                    (oldWorkflowDetails.SecExpAppUserId.HasValue && paymentWorkflow.SecTreasuryUserId.HasValue && oldWorkflowDetails.SecExpAppUserId.Value != paymentWorkflow.SecTreasuryUserId.Value))))
                {
                    SecUserNotification.StageName = "Exceptional Approver";
                    SecUserNotification.FromDate = (paymentWorkflow.SecTreasuryFromDt.HasValue ? paymentWorkflow.SecTreasuryFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (paymentWorkflow.SecTreasuryToDt.HasValue ? paymentWorkflow.SecTreasuryToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = paymentWorkflow.SecTreasuryUserId.Value;
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriTreasuryUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (oldWorkflowDetails != null && (oldWorkflowDetails.SecExpAppUserId.HasValue && paymentWorkflow.SecTreasuryUserId.HasValue && oldWorkflowDetails.SecExpAppUserId.Value != paymentWorkflow.SecTreasuryUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = oldWorkflowDetails.SecExpAppUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (oldWorkflowDetails != null && ((oldWorkflowDetails.SecExpAppUserId.HasValue && paymentWorkflow.SecTreasuryUserId == null)))
                {
                    SecUserNotification.StageName = "Exceptional Approver";
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriTreasuryUserId;
                    SecUserNotification.AssignedUserId = oldWorkflowDetails.SecExpAppUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((oldWorkflowDetails == null && paymentWorkflow.SecMgmtAssFromDt.HasValue) ||
                    (oldWorkflowDetails != null && ((oldWorkflowDetails.SecAuditorUserId == null && paymentWorkflow.SecMgmtAssUserId.HasValue) ||
                    (oldWorkflowDetails.SecAuditorUserId.HasValue && paymentWorkflow.SecMgmtAssUserId.HasValue && oldWorkflowDetails.SecAuditorUserId.Value != paymentWorkflow.SecMgmtAssUserId.Value))))
                {
                    SecUserNotification.StageName = "Auditor";
                    SecUserNotification.FromDate = (paymentWorkflow.SecMgmtAssFromDt.HasValue ? paymentWorkflow.SecMgmtAssFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (paymentWorkflow.SecMgmtAssToDt.HasValue ? paymentWorkflow.SecMgmtAssToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = paymentWorkflow.SecMgmtAssUserId.Value;
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriMgmtAssUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (oldWorkflowDetails != null && (oldWorkflowDetails.SecAuditorUserId.HasValue && paymentWorkflow.SecMgmtAssUserId.HasValue && oldWorkflowDetails.SecAuditorUserId.Value != paymentWorkflow.SecMgmtAssUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = oldWorkflowDetails.SecAuditorUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (oldWorkflowDetails != null && ((oldWorkflowDetails.SecAuditorUserId.HasValue && paymentWorkflow.SecMgmtAssUserId == null)))
                {
                    SecUserNotification.StageName = "Auditor";
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriMgmtAssUserId;
                    SecUserNotification.AssignedUserId = oldWorkflowDetails.SecAuditorUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((oldWorkflowDetails == null && paymentWorkflow.SecFASSCUserId.HasValue) ||
                    (oldWorkflowDetails != null && ((oldWorkflowDetails.SecFASSCDTUserId == null && paymentWorkflow.SecFASSCUserId.HasValue) ||
                    (oldWorkflowDetails.SecFASSCDTUserId.HasValue && paymentWorkflow.SecFASSCUserId.HasValue && oldWorkflowDetails.SecFASSCDTUserId.Value != paymentWorkflow.SecFASSCUserId.Value))))
                {
                    SecUserNotification.StageName = "F&A SSC-DT";
                    SecUserNotification.FromDate = (paymentWorkflow.SecFASSCFromDt.HasValue ? paymentWorkflow.SecFASSCFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (paymentWorkflow.SecFASSCToDt.HasValue ? paymentWorkflow.SecFASSCToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = paymentWorkflow.SecFASSCUserId.Value;
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriFASSCUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (oldWorkflowDetails != null && (oldWorkflowDetails.SecFASSCDTUserId.HasValue && paymentWorkflow.SecFASSCUserId.HasValue && oldWorkflowDetails.SecFASSCDTUserId.Value != paymentWorkflow.SecFASSCUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = oldWorkflowDetails.SecFASSCDTUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (oldWorkflowDetails != null && ((oldWorkflowDetails.SecFASSCDTUserId.HasValue && paymentWorkflow.SecFASSCUserId == null)))
                {
                    SecUserNotification.StageName = "F&A SSC-DT";
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriFASSCUserId;
                    SecUserNotification.AssignedUserId = oldWorkflowDetails.SecFASSCDTUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((oldWorkflowDetails == null && paymentWorkflow.SecCBUserId.HasValue) ||
                    (oldWorkflowDetails != null && ((oldWorkflowDetails.SecFASSCCBUserId == null && paymentWorkflow.SecCBUserId.HasValue) ||
                    (oldWorkflowDetails.SecFASSCCBUserId.HasValue && paymentWorkflow.SecCBUserId.HasValue && oldWorkflowDetails.SecFASSCCBUserId.Value != paymentWorkflow.SecCBUserId.Value))))
                {
                    SecUserNotification.StageName = "F&A SSC-C&B";
                    SecUserNotification.FromDate = (paymentWorkflow.SecCBFromDt.HasValue ? paymentWorkflow.SecCBFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (paymentWorkflow.SecCBToDt.HasValue ? paymentWorkflow.SecCBToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = paymentWorkflow.SecCBUserId.Value;
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriCBUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (oldWorkflowDetails != null && (oldWorkflowDetails.SecFASSCCBUserId.HasValue && paymentWorkflow.SecCBUserId.HasValue && oldWorkflowDetails.SecFASSCCBUserId.Value != paymentWorkflow.SecCBUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = oldWorkflowDetails.SecFASSCCBUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (oldWorkflowDetails != null && ((oldWorkflowDetails.SecFASSCCBUserId.HasValue && paymentWorkflow.SecCBUserId == null)))
                {
                    SecUserNotification.StageName = "F&A SSC-C&B";
                    SecUserNotification.PriActorUserId = paymentWorkflow.PriCBUserId;
                    SecUserNotification.AssignedUserId = oldWorkflowDetails.SecFASSCCBUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                isSave = true;
            }
            return isSave;
        }

        public List<GetSAPAgainstBillIntialData_Result> getAgainstBillPaymentDetails(string companycode, string vendorcode,string type,int userId)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.GetSAPAgainstBillIntialData(vendorcode, companycode, type, userId).ToList();
            }

        }

        public List<GetSAPAdvanceIntialData_Result> getAdvancePaymentDetails(string companycode, string vendorcode, string type, int userId)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;

                return suzlonBPPEntities.GetSAPAdvanceIntialData(vendorcode, companycode,type,userId).ToList();
            }

        }

        public bool AgainstBillTransaction(ArrayList values)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                using (var trans = Entities.Database.BeginTransaction())
                {
                    try
                    {
                        int Id = Convert.ToInt32(values[0]);
                        int sts = 0;

                        SAPAgainstBillPayment SAP = new SAPAgainstBillPayment();

                        SAP = Entities.SAPAgainstBillPayments.FirstOrDefault(s => s.SAPAgainstBillPaymentId == Id);
                        SAP.ModifiedBy = Convert.ToInt32(values[8].ToString());
                        SAP.ModifiedOn = DateTime.Now;
                        SAP.AmountProposed = Convert.ToDecimal(values[3].ToString());
                        SAP.NatureofRequestId = Convert.ToInt32(values[2].ToString());
                        Entities.Entry(SAP).State = EntityState.Modified;
                        Func<PaymentDetailAgainstBill, bool> conds = s => s.SAPAgainstBillPaymentId == Id && s.StatusId == sts && s.PaymentWorkflowStatusId == sts;
                        PaymentDetailAgainstBill Pay;
                        Pay = Entities.PaymentDetailAgainstBills.FirstOrDefault(conds);
                        if (Pay == null)
                        {
                            Pay = new PaymentDetailAgainstBill();
                            Pay.CreatedBy = Convert.ToInt32(values[8].ToString());
                            Pay.CreatedOn = DateTime.Now;
                            Entities.PaymentDetailAgainstBills.Add(Pay);
                        }
                        else
                        {
                            Entities.Entry(Pay).State = EntityState.Modified;
                        }

                        Pay.ModifiedBy = Convert.ToInt32(values[8].ToString());
                        Pay.ModifiedOn = DateTime.Now;
                        Pay.Comment = values[4].ToString();
                        Pay.ProfileID = Convert.ToInt32(values[7].ToString());

                        Pay.SAPAgainstBillPaymentId = Convert.ToInt32(values[0].ToString());
                        Pay.SubVerticalId = Convert.ToInt32(values[1].ToString());
                        Pay.VerticalId = Convert.ToInt32(values[6].ToString());
                        Pay.ApprovedAmount = Convert.ToDecimal(values[3].ToString());
                        Pay.PaymentWorkflowStatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 0);
                        Pay.StatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 0);

                        PaymentDetailAgainstBillTxn Txn;
                        Func<PaymentDetailAgainstBillTxn, bool> condition = s => s.SAPAgainstBillPaymentId == Id && s.StatusId == 0 && s.PaymentWorkflowStatusId == 0;
                        Txn = Entities.PaymentDetailAgainstBillTxns.FirstOrDefault(condition);
                        if (Txn == null)
                        {
                            Txn = new PaymentDetailAgainstBillTxn();
                            Txn.CreatedBy = Convert.ToInt32(values[8].ToString());
                            Txn.CreatedOn = DateTime.Now;
                            Entities.PaymentDetailAgainstBillTxns.Add(Txn);
                        }
                        else
                        {
                            Entities.Entry(Txn).State = EntityState.Modified;
                        }

                        Txn.SAPAgainstBillPaymentId = Convert.ToInt32(values[0].ToString());
                        Txn.ProfileId = Convert.ToInt32(values[7].ToString());
                        Txn.AppovedAmount = Convert.ToDecimal(values[3].ToString());
                        Txn.PaymentWorkflowStatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 0);
                        Txn.StatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 0);
                        Txn.Comment = values[4].ToString();

                        Entities.SaveChanges();
                        trans.Commit();

                        if (Convert.ToBoolean(values[5].ToString()) == true)
                        {
                            PaymentInitiationMail ObjMail = new PaymentInitiationMail();
                            ObjMail.SAPPaymentId = Pay.SAPAgainstBillPaymentId;
                            ObjMail.PaymentId = Pay.PaymentDetailAgainstBillId;
                            ObjMail.BillType = "Against Bill";
                            ObjMail.SubVertical = Pay.SubVerticalId;
                            ObjMail.VendorCode = SAP.VendorCode;
                            ObjMail.VendorName = SAP.VendorName;
                            ObjMail.CompanyCode = SAP.CompanyCode;
                            ObjMail.IsMailSend = false;
                            ObjMail.CreatedBy = Convert.ToInt32(values[8].ToString());
                            ObjMail.CreatedOn = DateTime.Now;
                            ObjMail.ModifyBy = Convert.ToInt32(values[8].ToString());
                            ObjMail.ModifyOn = DateTime.Now;
                            Entities.PaymentInitiationMails.Add(ObjMail);
                            Entities.SaveChanges();
                        }

                        return true;

                    }
                    catch (Exception ex)
                    {

                        trans.Rollback();
                        return false;
                    }
                }
            }
        }
        public bool UpdateSaveStatusAdvance(ArrayList values)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                string VendorCode = values[0].ToString();
                string CompCode = values[1].ToString();
                var SapPaymentId = (from P in Entities.PaymentDetailAdvances join
                                                                      S in Entities.SAPAdvancePayments on P.SAPAdvancePaymentId equals S.SAPAdvancePaymentId
                                                                      where P.StatusId == 0 && P.PaymentWorkFlowStatusId == 0 && P.SaveMode == true
                                                                      && S.VendorCode == VendorCode && S.CompanyCode == CompCode
                                    select new {
                                                                          P.SAPAdvancePaymentId }
                                                                      ).ToList();
                foreach (var SapId in SapPaymentId)
                {
                    Func<PaymentDetailAdvance, bool> conds = s => s.SAPAdvancePaymentId == SapId.SAPAdvancePaymentId;
                    PaymentDetailAdvance Pay;
                    Pay = Entities.PaymentDetailAdvances.FirstOrDefault(conds);
                    if (Pay != null)
                    {
                        Pay.SaveMode = false;
                        Entities.Entry(Pay).State = EntityState.Modified;
                        Entities.SaveChanges();
                    }

                }

                Convert.ToString(values[2]).Split(',').ToList().ForEach(sapPayID =>
                {
                    if (!string.IsNullOrEmpty(sapPayID))
                    {
                        Func<PaymentDetailAdvance, bool> conds = s => s.SAPAdvancePaymentId == Convert.ToInt32(sapPayID);
                        PaymentDetailAdvance Pay;
                        Pay = Entities.PaymentDetailAdvances.FirstOrDefault(conds);
                        if (Pay != null)
                        {
                            Pay.SaveMode = true;
                            Entities.Entry(Pay).State = EntityState.Modified;
                            Entities.SaveChanges();
                        }
                    }
                });

                return true;

            }
        }
        public bool UpdateSaveStatusAgainst(ArrayList values)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                string VendorCode = values[0].ToString();
                string CompCode = values[1].ToString();
                var SapPaymentId = (from P in Entities.PaymentDetailAgainstBills
                                    join
                             S in Entities.SAPAgainstBillPayments on P.SAPAgainstBillPaymentId equals S.SAPAgainstBillPaymentId
                                    where P.StatusId == 0 && P.PaymentWorkflowStatusId == 0 && P.SaveMode == true
                                    && S.VendorCode == VendorCode && S.CompanyCode == CompCode
                                    select new
                                    {
                                        P.SAPAgainstBillPaymentId
                                    }
                                                                      ).ToList();
                foreach (var SapId in SapPaymentId)
                {
                    Func<PaymentDetailAgainstBill, bool> conds = s => s.SAPAgainstBillPaymentId == SapId.SAPAgainstBillPaymentId;
                    PaymentDetailAgainstBill Pay;
                    Pay = Entities.PaymentDetailAgainstBills.FirstOrDefault(conds);
                    if (Pay != null)
                    {
                        Pay.SaveMode = false;
                        Entities.Entry(Pay).State = EntityState.Modified;
                        Entities.SaveChanges();
                    }

                }

                Convert.ToString(values[2]).Split(',').ToList().ForEach(sapPayID =>
                {
                    if (!string.IsNullOrEmpty(sapPayID))
                    {
                        Func<PaymentDetailAgainstBill, bool> conds = s => s.SAPAgainstBillPaymentId == Convert.ToInt32(sapPayID);
                        PaymentDetailAgainstBill Pay;
                        Pay = Entities.PaymentDetailAgainstBills.FirstOrDefault(conds);
                        if (Pay != null)
                        {
                            Pay.SaveMode = true;
                            Entities.Entry(Pay).State = EntityState.Modified;
                            Entities.SaveChanges();
                        }
                    }
                });

                return true;

            }
        }
        public bool AdvancePaymentTransaction(ArrayList values)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                using (var trans = Entities.Database.BeginTransaction())
                {
                    try
                    {
                        int Id = Convert.ToInt32(values[0]);
                        int sts = 0;

                        SAPAdvancePayment SAP = new SAPAdvancePayment();
                        SAP = Entities.SAPAdvancePayments.FirstOrDefault(s => s.SAPAdvancePaymentId == Id);
                        SAP.ModifiedBy = Convert.ToInt32(values[8].ToString());
                        SAP.ModifiedOn = DateTime.Now;
                        SAP.AmountProposed = Convert.ToDecimal(values[3].ToString());
                        SAP.Natureofrequest = Convert.ToInt32(values[2].ToString());
                        SAP.WithholdingTaxCode = values[9].ToString();
                        SAP.JustificationforAdvPayment = values[10].ToString();
                        Entities.Entry(SAP).State = EntityState.Modified;

                        Func<PaymentDetailAdvance, bool> conds = s => s.SAPAdvancePaymentId == Id && s.StatusId == sts && s.PaymentWorkFlowStatusId == sts;

                        PaymentDetailAdvance Pay;
                        Pay = Entities.PaymentDetailAdvances.FirstOrDefault(conds);
                        if (Pay == null)
                        {
                            Pay = new PaymentDetailAdvance();
                            Pay.CreatedBy = Convert.ToInt32(values[8].ToString());
                            Pay.CreatedOn = DateTime.Now;
                            Pay.Comment = values[4].ToString();
                            Pay.ProfileID = Convert.ToInt32(values[7].ToString());
                            Entities.PaymentDetailAdvances.Add(Pay);
                        }
                        else
                        {
                            Entities.Entry(Pay).State = EntityState.Modified;
                        }

                        Pay.ModifiedBy = Convert.ToInt32(values[8].ToString());
                        Pay.ModifiedOn = DateTime.Now;

                        Pay.SAPAdvancePaymentId = Convert.ToInt32(values[0].ToString());
                        Pay.SubVerticalId = Convert.ToInt32(values[1].ToString());
                        Pay.VerticalId = Convert.ToInt32(values[6].ToString());
                        Pay.ApprovedAmount = Convert.ToDecimal(values[3].ToString());
                        Pay.PaymentWorkFlowStatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 0);
                        Pay.StatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 0);

                        Func<PaymentDetailAdvanceTxn, bool> condition = s => s.SAPAdvancePaymentId == Id && s.StatusId == 0 && s.PaymentWorkflowStatusId == 0;
                        PaymentDetailAdvanceTxn Txn;
                        Txn = Entities.PaymentDetailAdvanceTxns.FirstOrDefault(condition);
                        if (Txn == null)
                        {
                            Txn = new PaymentDetailAdvanceTxn();
                            Txn.CreatedBy = Convert.ToInt32(values[8].ToString());
                            Txn.CreatedOn = DateTime.Now;
                            Entities.PaymentDetailAdvanceTxns.Add(Txn);
                        }
                        else
                        {
                            Entities.Entry(Txn).State = EntityState.Modified;
                        }

                        Txn.SAPAdvancePaymentId = Convert.ToInt32(values[0].ToString());
                        Txn.ProfileId = Convert.ToInt32(values[7].ToString());
                        Txn.AppovedAmount = Convert.ToDecimal(values[3].ToString());
                        Txn.PaymentWorkflowStatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 0);
                        Txn.StatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 0);
                        Txn.Comment = values[4].ToString();

                        Entities.SaveChanges();
                        trans.Commit();

                        if (Convert.ToBoolean(values[5].ToString()) == true)
                        {
                            PaymentInitiationMail ObjMail = new PaymentInitiationMail();
                            ObjMail.SAPPaymentId = Pay.SAPAdvancePaymentId;
                            ObjMail.PaymentId = Pay.PaymentDetailAdvanceId;
                            ObjMail.BillType = "Advance";
                            ObjMail.SubVertical = Pay.SubVerticalId;
                            ObjMail.VendorCode = SAP.VendorCode;
                            ObjMail.VendorName = SAP.VendorName;
                            ObjMail.CompanyCode = SAP.CompanyCode;
                            ObjMail.IsMailSend = false;
                            ObjMail.CreatedBy = Convert.ToInt32(values[8].ToString());
                            ObjMail.CreatedOn = DateTime.Now;
                            ObjMail.ModifyBy = Convert.ToInt32(values[8].ToString());
                            ObjMail.ModifyOn = DateTime.Now;
                            Entities.PaymentInitiationMails.Add(ObjMail);
                            Entities.SaveChanges();
                        }



                        return true;

                    }
                    catch (Exception)
                    {

                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool AgainstBillCorrection(ArrayList values)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                using (var trans = Entities.Database.BeginTransaction())
                {
                    try
                    {
                        int Id = Convert.ToInt32(values[0]);
                        int sts = 3;
                        int ws = 19;

                        SAPAgainstBillPayment SAP = new SAPAgainstBillPayment();

                        SAP = Entities.SAPAgainstBillPayments.FirstOrDefault(s => s.SAPAgainstBillPaymentId == Id);
                        SAP.ModifiedBy = Convert.ToInt32(values[8].ToString());
                        SAP.ModifiedOn = DateTime.Now;
                        SAP.AmountProposed = Convert.ToDecimal(values[3].ToString());
                        SAP.NatureofRequestId = Convert.ToInt32(values[2].ToString());
                        Entities.Entry(SAP).State = EntityState.Modified;
                        Func<PaymentDetailAgainstBill, bool> conds = s => s.SAPAgainstBillPaymentId == Id && s.StatusId == sts && s.PaymentWorkflowStatusId == ws;
                        PaymentDetailAgainstBill Pay;
                        Pay = Entities.PaymentDetailAgainstBills.FirstOrDefault(conds);
                        if (Pay == null)
                        {
                            Pay = new PaymentDetailAgainstBill();
                            Pay.CreatedBy = Convert.ToInt32(values[8].ToString());
                            Pay.CreatedOn = DateTime.Now;
                            Entities.PaymentDetailAgainstBills.Add(Pay);
                        }
                        else
                        {
                            Entities.Entry(Pay).State = EntityState.Modified;
                        }

                        Pay.ModifiedBy = Convert.ToInt32(values[8].ToString());
                        Pay.ModifiedOn = DateTime.Now;
                        Pay.Comment = values[4].ToString();
                        Pay.ProfileID = Convert.ToInt32(values[7].ToString());

                        Pay.SAPAgainstBillPaymentId = Convert.ToInt32(values[0].ToString());
                        Pay.SubVerticalId = Convert.ToInt32(values[1].ToString());
                        Pay.VerticalId = Convert.ToInt32(values[6].ToString());
                        Pay.ApprovedAmount = Convert.ToDecimal(values[3].ToString());
                        Pay.PaymentWorkflowStatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 19);
                        Pay.StatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 3);

                        PaymentDetailAgainstBillTxn Txn;
                        Func<PaymentDetailAgainstBillTxn, bool> condition = s => s.SAPAgainstBillPaymentId == Id && s.StatusId == sts && s.PaymentWorkflowStatusId == ws;
                        Txn = Entities.PaymentDetailAgainstBillTxns.FirstOrDefault(condition);
                        if (Txn == null)
                        {
                            Txn = new PaymentDetailAgainstBillTxn();
                            Txn.CreatedBy = Convert.ToInt32(values[8].ToString());
                            Txn.CreatedOn = DateTime.Now;
                            Entities.PaymentDetailAgainstBillTxns.Add(Txn);
                        }
                        else
                        {
                            Entities.Entry(Txn).State = EntityState.Modified;
                        }

                        Txn.SAPAgainstBillPaymentId = Convert.ToInt32(values[0].ToString());
                        Txn.ProfileId = Convert.ToInt32(values[7].ToString());
                        Txn.AppovedAmount = Convert.ToDecimal(values[3].ToString());
                        Txn.PaymentWorkflowStatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 19);
                        Txn.StatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 3);
                        Txn.Comment = values[4].ToString();

                        Entities.SaveChanges();
                        trans.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {

                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool AdvancePaymentCorrection(ArrayList values)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                using (var trans = Entities.Database.BeginTransaction())
                {
                    try
                    {
                        int Id = Convert.ToInt32(values[0]);
                        int sts = 3;
                        int ws = 19;
                        SAPAdvancePayment SAP = new SAPAdvancePayment();
                        SAP = Entities.SAPAdvancePayments.FirstOrDefault(s => s.SAPAdvancePaymentId == Id);
                        SAP.ModifiedBy = Convert.ToInt32(values[8].ToString());
                        SAP.ModifiedOn = DateTime.Now;
                        SAP.AmountProposed = Convert.ToDecimal(values[3].ToString());
                        SAP.Natureofrequest = Convert.ToInt32(values[2].ToString());
                        SAP.WithholdingTaxCode = values[9].ToString();
                        SAP.JustificationforAdvPayment = values[10].ToString();
                        Entities.Entry(SAP).State = EntityState.Modified;

                        Func<PaymentDetailAdvance, bool> conds = s => s.SAPAdvancePaymentId == Id && s.StatusId == sts && s.PaymentWorkFlowStatusId == ws;

                        PaymentDetailAdvance Pay;
                        Pay = Entities.PaymentDetailAdvances.FirstOrDefault(conds);
                        if (Pay == null)
                        {
                            Pay = new PaymentDetailAdvance();
                            Pay.CreatedBy = Convert.ToInt32(values[8].ToString());
                            Pay.CreatedOn = DateTime.Now;
                            Entities.PaymentDetailAdvances.Add(Pay);
                        }
                        else
                        {
                            Entities.Entry(Pay).State = EntityState.Modified;
                        }

                        Pay.ModifiedBy = Convert.ToInt32(values[8].ToString());
                        Pay.ModifiedOn = DateTime.Now;
                        Pay.Comment = values[4].ToString();
                        Pay.ProfileID = Convert.ToInt32(values[7].ToString());

                        Pay.SAPAdvancePaymentId = Convert.ToInt32(values[0].ToString());
                        Pay.SubVerticalId = Convert.ToInt32(values[1].ToString());
                        Pay.VerticalId = Convert.ToInt32(values[6].ToString());
                        Pay.ApprovedAmount = Convert.ToDecimal(values[3].ToString());
                        Pay.PaymentWorkFlowStatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 19);
                        Pay.StatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 3);

                        Func<PaymentDetailAdvanceTxn, bool> condition = s => s.SAPAdvancePaymentId == Id && s.StatusId == sts && s.PaymentWorkflowStatusId == ws;
                        PaymentDetailAdvanceTxn Txn;
                        Txn = Entities.PaymentDetailAdvanceTxns.FirstOrDefault(condition);
                        if (Txn == null)
                        {
                            Txn = new PaymentDetailAdvanceTxn();
                            Txn.CreatedBy = Convert.ToInt32(values[8].ToString());
                            Txn.CreatedOn = DateTime.Now;
                            Entities.PaymentDetailAdvanceTxns.Add(Txn);
                        }
                        else
                        {
                            Entities.Entry(Txn).State = EntityState.Modified;
                        }

                        Txn.SAPAdvancePaymentId = Convert.ToInt32(values[0].ToString());
                        Txn.ProfileId = Convert.ToInt32(values[7].ToString());
                        Txn.AppovedAmount = Convert.ToDecimal(values[3].ToString());
                        Txn.PaymentWorkflowStatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 19);
                        Txn.StatusId = ((Convert.ToBoolean(values[5].ToString()) == true) ? 1 : 3);
                        Txn.Comment = values[4].ToString();

                        Entities.SaveChanges();
                        trans.Commit();
                        return true;

                    }
                    catch (Exception)
                    {

                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        public List<PaymentDetailAgainstBillTxn> GetCommentsForMyRequest(bool needCorrection)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                if (needCorrection)
                {

                    return (from Txn in suzlonBPPEntities.PaymentDetailAgainstBillTxns
                            join P in suzlonBPPEntities.PaymentDetailAgainstBills on Txn.SAPAgainstBillPaymentId equals P.SAPAgainstBillPaymentId
                            where P.PaymentWorkflowStatusId == 19 && P.StatusId == 3
                            select Txn).ToList();

                }
                else
                {
                    return (from Txn in suzlonBPPEntities.PaymentDetailAgainstBillTxns
                            join P in suzlonBPPEntities.PaymentDetailAgainstBills on Txn.SAPAgainstBillPaymentId equals P.SAPAgainstBillPaymentId
                            where P.PaymentWorkflowStatusId == 0 && P.StatusId == 0
                            select Txn).ToList();
                }
            }
        }

        public List<PaymentDetailAdvanceTxn> GetCommentsForAdvanceMyRequest(bool needCorrection)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {

                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                if (needCorrection)
                {

                    return (from Txn in suzlonBPPEntities.PaymentDetailAdvanceTxns
                            join P in suzlonBPPEntities.PaymentDetailAdvances on Txn.SAPAdvancePaymentId equals P.SAPAdvancePaymentId
                            where P.PaymentWorkFlowStatusId == 19 && P.StatusId == 3
                            select Txn).ToList();

                }
                else
                {
                    return (from Txn in suzlonBPPEntities.PaymentDetailAdvanceTxns
                            join P in suzlonBPPEntities.PaymentDetailAdvances on Txn.SAPAdvancePaymentId equals P.SAPAdvancePaymentId
                            where P.PaymentWorkFlowStatusId == 0 && P.StatusId == 0
                            select Txn).ToList();
                }
            }
        }

        public SAPAgainstBillPayment GetAgainstBillPay(int Id)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.SAPAgainstBillPayments.FirstOrDefault(s => s.SAPAgainstBillPaymentId == Id);

            }
        }

        public SAPAdvancePayment GetAdvancePay(int Id)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.SAPAdvancePayments.FirstOrDefault(s => s.SAPAdvancePaymentId == Id);

            }
        }

        public bool UpdateAgainstBillPayment(SAPAgainstBillPayment sapAgainstBillPayment)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                SAPAgainstBillPayment SAPAgainstBillPayment = suzlonBPPEntities.SAPAgainstBillPayments.FirstOrDefault(l => l.SAPAgainstBillPaymentId == sapAgainstBillPayment.SAPAgainstBillPaymentId);
                if (SAPAgainstBillPayment != null)
                {
                    SAPAgainstBillPayment.NatureofRequestId = sapAgainstBillPayment.NatureofRequestId;
                    SAPAgainstBillPayment.AmountProposed = sapAgainstBillPayment.AmountProposed;
                    suzlonBPPEntities.Entry(SAPAgainstBillPayment).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool UpdateAdvanceBillPayment(SAPAdvancePayment sapAdvancePayment)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                SAPAdvancePayment SAPAdvancePayment = suzlonBPPEntities.SAPAdvancePayments.FirstOrDefault(l => l.SAPAdvancePaymentId == sapAdvancePayment.SAPAdvancePaymentId);
                if (SAPAdvancePayment != null)
                {
                    SAPAdvancePayment.Natureofrequest = sapAdvancePayment.Natureofrequest;
                    SAPAdvancePayment.AmountProposed = sapAdvancePayment.AmountProposed;
                    suzlonBPPEntities.Entry(SAPAdvancePayment).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool InsertAgainstBillPayment(PaymentDetailAgainstBill paymentDetailAgainstBill)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.PaymentDetailAgainstBills.Add(paymentDetailAgainstBill);
                int count = suzlonBPPEntities.SaveChanges();
                return (count > 0) ? false : true;
            }
        }

        public bool InsertAdvancePayment(PaymentDetailAdvance paymentDetailAdvance)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.PaymentDetailAdvances.Add(paymentDetailAdvance);
                int count = suzlonBPPEntities.SaveChanges();
                return (count > 0) ? false : true;
            }
        }

        public List<sp_GetInitiator_AgainstBill_InProcess_Data_Result> GetInitiatorAgainstBillInProcessData(string VendorCode, string CompanyCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.sp_GetInitiator_AgainstBill_InProcess_Data(VendorCode, CompanyCode).ToList();

            }
        }

        public List<sp_GetInitiator_Advance_InProcess_Data_Result> GetInitiatorAdvanceInProcessData(string VendorCode, string CompanyCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.sp_GetInitiator_Advance_InProcess_Data(VendorCode, CompanyCode).ToList();
            }
        }

        public List<sp_GetInitiator_AgainstBill_Correction_Data_Result> GetInitiatorAgainstBillCorrectionData(string VendorCode, string CompanyCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.sp_GetInitiator_AgainstBill_Correction_Data(VendorCode, CompanyCode).ToList();

            }
        }

        public List<sp_GetInitiator_Advance_Correction_Data_Result> GetInitiatorAdvanceCorrectionData(string VendorCode, string CompanyCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.sp_GetInitiator_Advance_Correction_Data(VendorCode, CompanyCode).ToList();
            }
        }

        public List<GetVendorData_Result> GetPendingReqByVerticalID(int VerticalID)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<GetVendorData_Result> lstGetVendorData_Result = suzlonBPPEntities.GetVendorData(VerticalID, String.Empty).ToList<GetVendorData_Result>();
                return lstGetVendorData_Result;
            }
        }

        public List<GetApproverVendorData_Result> GetApproverVendorData(int VerticalID, int UserId, string BillType, string TabName)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<GetApproverVendorData_Result> lstGetVendorData_Result = suzlonBPPEntities.GetApproverVendorData(VerticalID, UserId, BillType, TabName).ToList<GetApproverVendorData_Result>();
                return lstGetVendorData_Result;
            }
        }

        public List<GetApproverCompByVendor_Result> GetApproverCompByVendor(int VerticalID, string VendorCode, int UserId,
             string BillType, string TabName)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<GetApproverCompByVendor_Result> lstGetComp_Result = suzlonBPPEntities.GetApproverCompByVendor(VerticalID, VendorCode, UserId, BillType, TabName).ToList<GetApproverCompByVendor_Result>();
                return lstGetComp_Result;
            }
        }

        public List<GetApproverEndItems_Result> GetApproverEndItems(int VerticalID, string VendorCode, string CompanyCode, int UserId,
             string BillType, string TabName)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<GetApproverEndItems_Result> lstGetItems_Result = suzlonBPPEntities.GetApproverEndItems(VerticalID, CompanyCode, VendorCode, UserId, BillType, TabName).ToList<GetApproverEndItems_Result>();
                return lstGetItems_Result;
            }
        }

        public int GetPaymentWorkflowStatusId(int StatusId, int ProfileId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                PaymentWorkflowStatu objPaymentWorkflowStatus = suzlonBPPEntities.PaymentWorkflowStatus.Where(c => c.ProfileId == ProfileId && c.StatusId == StatusId).FirstOrDefault();
                if (objPaymentWorkflowStatus != null)
                    return objPaymentWorkflowStatus.PaymentWorkflowStatusId;
                else
                    return 0;
            }
        }

        public int GetPaymentWorkflowStatusId(string Status)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                PaymentWorkflowStatu objPaymentWorkflowStatus = suzlonBPPEntities.PaymentWorkflowStatus.Where(c => c.Status.ToLower().Trim() == Status.ToLower().Trim()).FirstOrDefault();
                if (objPaymentWorkflowStatus != null)
                    return objPaymentWorkflowStatus.PaymentWorkflowStatusId;
                else
                    return 0;
            }
        }

        public List<GetCompanyData_Result> GetPendingCompanyDataForFnA(int VerticalID)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetCompanyData(VerticalID).ToList();
            }
        }

        public bool UpdateAmountInPaymentDetailAdvance(List<PaymentDetailAdvance> Transactions)
        {
            int count = 0;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                try
                {
                    foreach (PaymentDetailAdvance obj in Transactions)
                    {
                        PaymentDetailAdvance objTemp = suzlonBPPEntities.PaymentDetailAdvances.Where(c => c.SAPAdvancePaymentId == obj.SAPAdvancePaymentId).FirstOrDefault();
                        objTemp.ApprovedAmount = obj.ApprovedAmount;
                        objTemp.ModifiedBy = obj.ModifiedBy;
                        objTemp.ModifiedOn = obj.ModifiedOn;
                        objTemp.Comment = obj.Comment;
                        objTemp.ProfileID = obj.ProfileID;
                        objTemp.StatusId = obj.StatusId;
                        objTemp.SaveMode = obj.SaveMode;
                        suzlonBPPEntities.Entry(objTemp).State = EntityState.Modified;
                        suzlonBPPEntities.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    string str = ex.ToString();
                }
                finally { }
                return (count > 0) ? false : true;

            }
        }

        public bool UpdateAmountInPaymentDetailAgainst(List<PaymentDetailAgainstBill> Transactions)
        {
            int count = 0;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                try
                {
                    foreach (PaymentDetailAgainstBill obj in Transactions)
                    {
                        PaymentDetailAgainstBill objTemp = suzlonBPPEntities.PaymentDetailAgainstBills.Where(c => c.SAPAgainstBillPaymentId == obj.SAPAgainstBillPaymentId).FirstOrDefault();
                        objTemp.ApprovedAmount = obj.ApprovedAmount;
                        objTemp.ModifiedBy = obj.ModifiedBy;
                        objTemp.ModifiedOn = obj.ModifiedOn;
                        objTemp.Comment = obj.Comment;
                        objTemp.ProfileID = obj.ProfileID;
                        objTemp.StatusId = obj.StatusId;
                        objTemp.SaveMode = obj.SaveMode;
                        suzlonBPPEntities.Entry(objTemp).State = EntityState.Modified;
                        suzlonBPPEntities.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    string str = ex.ToString();
                }
                finally { }
                return (count > 0) ? false : true;

            }
        }

        public List<GetVendorDataForFnA_Result> GetPendingVendorDataForFnA(int VerticalID, string CompanyId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetVendorDataForFnA(VerticalID, CompanyId).ToList();
            }
        }

        public List<GetVentorItemsForFnA_Result> GetPendingVendorItemsDataForFnA(int VerticalID, string CompanyId, string VendorId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetVentorItemsForFnA(VerticalID, CompanyId, VendorId).ToList();
            }
        }

        public List<GetCompanyDataAgainstBillFnA_Result> GetPendingCompanyDataAgainstBillForFnA(int VerticalID)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetCompanyDataAgainstBillFnA(VerticalID).ToList();
            }
        }

        public List<GetVendorDataAgainstBillFnA_Result> GetPendingVendorDataAgainstBillForFnA(int VerticalID, string CompanyId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetVendorDataAgainstBillFnA(VerticalID, CompanyId).ToList();
            }
        }

        public List<GetVentorItemsAgainstBillForFnA_Result1> GetPendingVendorItemsDataAgainstBillForFnA(int VerticalID, string CompanyId, string VendorId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetVentorItemsAgainstBillForFnA(VerticalID, CompanyId, VendorId).ToList();
            }
        }

        public bool SavePaymentDetailTxn_Adv(List<PaymentDetailAdvanceTxn> Transactions)
        {
            int count = 0;

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                try
                {
                    foreach (PaymentDetailAdvanceTxn obj in Transactions)
                    {
                        SAPAdvancePayment objSAP = obj.SAPAdvancePayment;
                        if (objSAP != null)
                        {
                            suzlonBPPEntities.Entry(objSAP).State = EntityState.Modified;
                            suzlonBPPEntities.SaveChanges();
                        }

                    }


                    suzlonBPPEntities.PaymentDetailAdvanceTxns.AddRange(Transactions);
                    count = suzlonBPPEntities.SaveChanges();



                }
                catch (Exception ex)
                {
                    string str = ex.ToString();
                }
                finally { }
                return (count > 0) ? false : true;

            }
        }

        public bool SavePaymentAgainstDetailTxn_Adv(List<PaymentDetailAgainstBillTxn> Transactions)
        {
            int count = 0;

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                try
                {
                    foreach (PaymentDetailAgainstBillTxn obj in Transactions)
                    {
                        SAPAgainstBillPayment objSAP = obj.SAPAgainstBillPayment;
                        if (objSAP != null)
                        {
                            suzlonBPPEntities.Entry(objSAP).State = EntityState.Modified;
                            suzlonBPPEntities.SaveChanges();
                        }

                    }

                    suzlonBPPEntities.PaymentDetailAgainstBillTxns.AddRange(Transactions);
                    count = suzlonBPPEntities.SaveChanges();
                    //suzlonBPPEntities.SAPAdvancePayments();
                   


                }
                catch (Exception ex)
                {
                    string str = ex.ToString();
                }
                finally { }
                return (count > 0) ? false : true;

            }
        }

        private static string PushAgainstBillPaymentDataToSAP(List<PaymentDetailAgainstBill> TransactionsToPush, string HouseBank)
        {
            StreamWriter sw1 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
            sw1.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                            + "PushAgainstBillPaymentDataToSAP Start" + Convert.ToInt32(TransactionsToPush[0].SAPAgainstBillPaymentId)));
            sw1.Close();

            SAPAgainstBillPayment SAPAgainstBillPayment = null;

            List<PaymentDetailAgainstBill> Transactions = new List<PaymentDetailAgainstBill>();
            Transactions = (from t in TransactionsToPush
                            where t.PaymentWorkflowStatusId == 17 select t).ToList();

            if (Transactions.Count > 0)
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    int SAPAgainstBillPaymentId = Convert.ToInt32(Transactions[0].SAPAgainstBillPaymentId);
                    SAPAgainstBillPayment = suzlonBPPEntities.SAPAgainstBillPayments.FirstOrDefault(s => s.SAPAgainstBillPaymentId == SAPAgainstBillPaymentId);
                }

                bool IsRestrictToSAP = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRestrictToSAP"]);

                if (IsRestrictToSAP)
                    return Constants.SAP_SUCCESS;

                SAPHelper objSAP = new SAPHelper();

                StreamWriter sw2 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                sw2.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                + "SAPHelper initialized"));
                sw2.Close();
                using (SAPConnection connection = objSAP.GetSAPConnection())
                {
                    try
                    {
                        StreamWriter sw3 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw3.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "inside GetSAPConnection"));
                        sw3.Close();
                        string requestId = "";                        
                        decimal postingAmount = 0;
                        string companyCode = SAPAgainstBillPayment.CompanyCode;
                        string vendorCode = SAPAgainstBillPayment.VendorCode;
                        string documentDate = DateTime.Now.ToString("yyyyMMdd");
                        string postDate = DateTime.Now.ToString("yyyyMMdd");
                        string paymentMethod = "N";
                        string houseBank = HouseBank;
                        string checkLotNumber = "0001";
                        string documentNumber = "";
                       
                        CashAndBank_SAP.payment obj = new CashAndBank_SAP.payment();
                        obj.Connection = connection;
                        try
                        {
                            obj.Connection.Open();
                        }
                        catch (Exception ex)
                        {
                            StreamWriter sw5 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                            sw5.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                            + "SAP_CONNECTION_FAILURE"));
                            sw5.Close();
                            CommonFunctions.WriteErrorLog(ex);
                            return Constants.SAP_CONNECTION_FAILURE;
                        }
                        CashAndBank_SAP.BAPIRET2Table bapiResult = new CashAndBank_SAP.BAPIRET2Table();
                        CashAndBank_SAP.YFIS_AP_ACC_DOC_CLEAR1Table IT_INV_REF = new CashAndBank_SAP.YFIS_AP_ACC_DOC_CLEAR1Table();

                        foreach (var transaction in Transactions)
                        {
                            SAPAgainstBillPayment objForDocumentNumber = null;

                            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                            {
                                int SAPAgainstBillPaymentId = Convert.ToInt32(transaction.SAPAgainstBillPaymentId);
                                objForDocumentNumber = suzlonBPPEntities.SAPAgainstBillPayments.FirstOrDefault(s => s.SAPAgainstBillPaymentId == SAPAgainstBillPaymentId);
                            }

                            CashAndBank_SAP.YFIS_AP_ACC_DOC_CLEAR1 dataTable = new CashAndBank_SAP.YFIS_AP_ACC_DOC_CLEAR1();
                            dataTable.Amount = transaction.ApprovedAmount.Value;
                            dataTable.Doc_Number = objForDocumentNumber.DocumentNumber;

                            requestId += "," + transaction.ApprovedLineGroupID;
                            postingAmount += transaction.ApprovedAmount.Value;
                            IT_INV_REF.Add(dataTable);
                        }

                        StreamWriter sw6 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw6.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "Yfif_Ap_Acc_Doc_Clear1 start"));
                        

                        try
                        {
                            for (int i = 0; i < IT_INV_REF.Count; i++)
                            {
                                sw6.WriteLine(Convert.ToString("Table" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                                 + "Amount" + IT_INV_REF[i].Amount + Environment.NewLine.Trim()
                                                                 + "DocumentNumber" + IT_INV_REF[i].Doc_Number + Environment.NewLine.Trim()
                                                                 ));


                            }
                        }
                        catch (Exception ex)
                        {
                            sw6.WriteLine("Error while writing Table.");
                        }

                        sw6.Close();

                        StreamWriter sw4 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw4.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "postingAmount = "+ postingAmount + Environment.NewLine.Trim()
                                                        + " CompanyCode =" + companyCode + Environment.NewLine.Trim()                                                      
                                                        + "; checkLotNumber = " + checkLotNumber + Environment.NewLine.Trim()
                                                        + "; documentDate = " + documentDate + Environment.NewLine.Trim()
                                                        + "; HouseBank =" + HouseBank + Environment.NewLine.Trim()
                                                        + "; PaymentMethod=" + paymentMethod + Environment.NewLine.Trim()
                                                        + "; postDate=" + postDate + Environment.NewLine.Trim()
                                                        + "; requestId=" + requestId.TrimEnd(',').TrimStart(',') + Environment.NewLine.Trim()
                                                          + "; VendorCode = " + vendorCode + Environment.NewLine.Trim()
                                                        + "; documentNumber=" + documentNumber + Environment.NewLine.Trim()
                                                        + "; SAPAgainstBillPaymentId =" + Transactions[0].SAPAgainstBillPaymentId + Environment.NewLine.Trim()
                                                        + "; ApprovedLineGroupID = " + Transactions[0].ApprovedLineGroupID + Environment.NewLine.Trim()
                                                        + "; DocumentNumber = " + SAPAgainstBillPayment.DocumentNumber + Environment.NewLine.Trim()
                                                        + "; ApprovedAmount = " + Transactions[0].ApprovedAmount));
                        sw4.Close();
                        //requestId = requestId.Length > 0 ? requestId.Substring(1, 25) : requestId;
                        obj.Yfif_Ap_Acc_Doc_Clear1(postingAmount, companyCode, checkLotNumber, documentDate, houseBank, paymentMethod,
                            postDate, requestId.TrimEnd(',').TrimStart(','), vendorCode, out documentNumber, out companyCode, ref IT_INV_REF, ref bapiResult);

                        StreamWriter sw7 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw7.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "Yfif_Ap_Acc_Doc_Clear1 end bapiResult.Count" + bapiResult.Count));
                        sw7.Close();
                        if (bapiResult != null && bapiResult.Count > 0)
                        {
                            if (ConfigurationManager.AppSettings["TraceLog"].ToString().ToLower() == "true")
                            {
                                StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                                sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                                + "BAPI :Yfif_Ap_Acc_Doc_Clear1"
                                                                + "Type " + bapiResult[0]["Type"]) + Environment.NewLine
                                                                + "Message" + bapiResult[0]["Message"]
                                                                + "DocumentNumber" + documentNumber
                                                                );

                                try
                                {
                                    for (int i = 0; i < bapiResult.Count; i++)
                                    {
                                        sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                                    + "BAPI Result :"
                                                                    + "Type " + bapiResult[i]["Type"]) + Environment.NewLine
                                                                    + "Message" + bapiResult[i]["Message"]
                                                                    );
                                    }
                                }
                                catch (Exception ex)
                                {
                                    sw.WriteLine("Error while writing result :" + ex.Message);
                                }
                                sw.Close();
                            }

                            string result = Convert.ToString(bapiResult[0]["Type"]);
                            if (result.ToUpper() != Constants.SAP_SUCCESS && result.ToUpper() != Constants.SAP_WARNING)
                            {
                                return Convert.ToString(bapiResult[0]["Message"]);
                            }
                            else
                            {
                                foreach (var transaction in Transactions)
                                {
                                    PaymentDetailAgainstBill objPaymentDetail = null;
                                    using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                                    {
                                        try
                                        {
                                            int SAPAgainstBillPaymentId = Convert.ToInt32(transaction.SAPAgainstBillPaymentId);
                                            objPaymentDetail = suzlonBPPEntities.PaymentDetailAgainstBills.FirstOrDefault(s => s.SAPAgainstBillPaymentId == SAPAgainstBillPaymentId);
                                            if (objPaymentDetail != null && documentNumber != "")
                                            {
                                                objPaymentDetail.DocumentClearingNo = documentNumber;
                                                suzlonBPPEntities.Entry(objPaymentDetail).State = EntityState.Modified;
                                                suzlonBPPEntities.SaveChanges();
                                            }
                                        }
                                        catch(Exception ex)
                                        { }
                                    }                                  
                                }

                                return result.ToUpper();
                            }
                        }
                        else
                        {
                            StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                            sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                            + "BAPI :Yfif_Ap_Acc_Doc_Clear1 " + Constants.SAP_ERROR));
                            sw.WriteLine(Convert.ToString(bapiResult));
                            sw.Close();
                            return Constants.SAP_ERROR;
                        }
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "Error " + ex.Message + "" + ex.StackTrace.ToString()));
                        sw.Close();
                        CommonFunctions.WriteErrorLog(ex);
                        return Constants.SAP_ERROR;
                    }
                }
            }
            else
            {
                StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                + "BAPI :Yfif_Ap_Acc_Doc_Clear1 : Count is 0"));
                sw.Close();

                if (TransactionsToPush.Count > 0)
                    return Constants.SAP_SUCCESS;
                else
                    return Constants.DETAIL_SAVE_FAILURE;
            }
        }

        private static string PushAdvancePaymentDataToSAP(List<PaymentDetailAdvance> TransactionsToPush, string HouseBank)
        {
            StreamWriter sw1 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
            sw1.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                            + "PushAdvancePaymentDataToSAP Start" + Convert.ToInt32(TransactionsToPush[0].SAPAdvancePaymentId)));
            sw1.Close();

            SAPAdvancePayment SAPAdvancePayment = null;

            List<PaymentDetailAdvance> Transactions = new List<PaymentDetailAdvance>();
            Transactions = (from t in TransactionsToPush
                            where t.PaymentWorkFlowStatusId == 17
                            select t).ToList();

            if (Transactions.Count > 0)
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    int SAPAgainstBillPaymentId = Convert.ToInt32(Transactions[0].SAPAdvancePaymentId);
                    SAPAdvancePayment = suzlonBPPEntities.SAPAdvancePayments.FirstOrDefault(s => s.SAPAdvancePaymentId == SAPAgainstBillPaymentId);
                }

                bool IsRestrictToSAP = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRestrictToSAP"]);

                if (IsRestrictToSAP)
                    return Constants.SAP_SUCCESS;

                SAPHelper objSAP = new SAPHelper();
                StreamWriter sw2 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                sw2.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                + "SAPHelper initialized"));
                sw2.Close();
                using (SAPConnection connection = objSAP.GetSAPConnection())
                {
                    try
                    {
                        StreamWriter sw3 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw3.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "inside GetSAPConnection"));
                        sw3.Close();
                        string requestId = "";

                        decimal postingAmount = 0;
                        string companyCode = SAPAdvancePayment.CompanyCode;
                        string vendorCode = SAPAdvancePayment.VendorCode;
                        string documentDate = DateTime.Now.ToString("yyyyMMdd");
                        string postDate = DateTime.Now.ToString("yyyyMMdd");
                        string documentNumber = "";// ref parameter

                        CashAndBank_SAP.AdvancePayment obj = new CashAndBank_SAP.AdvancePayment();
                        obj.Connection = connection;
                        try
                        {
                            obj.Connection.Open();
                        }
                        catch (Exception ex)
                        {
                            StreamWriter sw5 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                            sw5.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                            + "SAP_CONNECTION_FAILURE"));
                            sw5.Close();
                            CommonFunctions.WriteErrorLog(ex);
                            return Constants.SAP_CONNECTION_FAILURE;
                        }
                        CashAndBank_SAP.BAPIRET2Table bapiResult = new CashAndBank_SAP.BAPIRET2Table();
                        CashAndBank_SAP.YFIS_AP_ACC_DOC_CLEAR1Table IT_INV_REF = new CashAndBank_SAP.YFIS_AP_ACC_DOC_CLEAR1Table();

                        foreach (var transaction in Transactions)
                        {
                            SAPAdvancePayment objForDPRNumber = null;
                            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                            {
                                int SAPAgainstBillPaymentId = Convert.ToInt32(transaction.SAPAdvancePaymentId);
                                objForDPRNumber = suzlonBPPEntities.SAPAdvancePayments.FirstOrDefault(s => s.SAPAdvancePaymentId == SAPAgainstBillPaymentId);
                            }

                            CashAndBank_SAP.YFIS_AP_ACC_DOC_CLEAR1 dataTable = new CashAndBank_SAP.YFIS_AP_ACC_DOC_CLEAR1();
                            // dataTable.Amount = transaction.ApprovedAmount.Value; as per tax cr we pass the gross amount to tax 13 dec
                            dataTable.Amount =Convert.ToDecimal(objForDPRNumber.GrossAmount);
                            dataTable.Doc_Number = objForDPRNumber.DPRNumber;

                            try
                            {
                                StreamWriter swr = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                                swr.WriteLine(Convert.ToString("Inside Loop Amount" + transaction.ApprovedAmount.Value
                                                                + "DocNumber" + dataTable.Doc_Number));
                                swr.Close();
                            }
                            catch(Exception ex)
                            {

                            }

                            requestId += "," + transaction.ApprovedLineGroupID;
                            postingAmount += transaction.ApprovedAmount.Value;
                            IT_INV_REF.Add(dataTable);
                        }

                        StreamWriter sw6 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw6.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "Yfif_Ap_Adv_F_48_Bapi start"));

                        try
                        {
                            for (int i = 0; i < IT_INV_REF.Count; i++)
                            {
                                sw6.WriteLine(Convert.ToString("Table" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                                 + "Amount" + IT_INV_REF[i].Amount + Environment.NewLine.Trim()
                                                                 + "DocumentNumber" + IT_INV_REF[i].Doc_Number + Environment.NewLine.Trim()
                                                                 ));


                            }
                        }
                        catch (Exception ex)
                        {
                            sw6.WriteLine("Error while writing Table.");
                        }

                        sw6.Close();
                        StreamWriter sw4 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw4.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "postingAmount = " + postingAmount + Environment.NewLine.Trim()
                                                        + " CompanyCode =" + companyCode + Environment.NewLine.Trim()
                                                        + "; documentDate = " + documentDate + Environment.NewLine.Trim()
                                                        + "; HouseBank = " + HouseBank + Environment.NewLine.Trim()
                                                        + "; postDate=" + postDate + Environment.NewLine.Trim()
                                                        + "; requestId=" + requestId.TrimEnd(',').TrimStart(',') + Environment.NewLine.Trim()
                                                          + "; VendorCode = " + vendorCode + Environment.NewLine.Trim()
                                                        + "; SAPAdvancePaymentId =" + Transactions[0].SAPAdvancePaymentId + Environment.NewLine.Trim()
                                                        + "; ApprovedLineGroupID = " + Transactions[0].ApprovedLineGroupID + Environment.NewLine.Trim()
                                                        + "; SpecialGL = " + SAPAdvancePayment.SpecialGL + Environment.NewLine.Trim()
                                                        + "; DocumentNumber = " + SAPAdvancePayment.DPRNumber + Environment.NewLine.Trim()
                                                        + "; ApprovedAmount = " + Transactions[0].ApprovedAmount));

                       

                        sw4.Close();
                        //requestId = requestId.Length > 0 ? requestId.Substring(1, 25) : requestId;
                        obj.Yfif_Ap_Adv_F_48_Bapi(companyCode, documentDate, HouseBank,"001", postDate, requestId.TrimEnd(',').TrimStart(','),
                              SAPAdvancePayment.SpecialGL, vendorCode, out companyCode, out documentNumber, ref IT_INV_REF, ref bapiResult);

                        StreamWriter sw7 = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw7.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "Yfif_Ap_Adv_F_48_Bapi end bapi Result.Count" + bapiResult.Count));
                        sw7.Close();
                        if (bapiResult != null && bapiResult.Count > 0)
                        {
                            if (ConfigurationManager.AppSettings["TraceLog"].ToString().ToLower() == "true")
                            {
                                StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                                sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                                + "BAPI :Yfif_Ap_Acc_Doc_Clear1"
                                                                + "Type " + bapiResult[0]["Type"]) + Environment.NewLine
                                                                + "Message" + bapiResult[0]["Message"]
                                                                + "DocumentNumber" + documentNumber
                                                                );

                                try
                                {
                                    for (int i = 0; i < bapiResult.Count; i++)
                                    {
                                        sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                                    + "BAPI Result :"
                                                                    + "Type " + bapiResult[i]["Type"]) + Environment.NewLine
                                                                    + "Message" + bapiResult[i]["Message"]
                                                                    );
                                    }
                                }
                                catch (Exception ex)
                                {
                                    sw.WriteLine("Error while writing result :" + ex.Message);
                                }
                                sw.Close();
                            }


                            string result = Convert.ToString(bapiResult[0]["Type"]);
                            if (result.ToUpper() != Constants.SAP_SUCCESS && result.ToUpper() != Constants.SAP_WARNING)
                            {
                                return Convert.ToString(bapiResult[0]["Message"]);
                            }
                            else
                            {
                                foreach (var transaction in Transactions)
                                {
                                    PaymentDetailAdvance objPaymentDetail = null;
                                    using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                                    {
                                        try
                                        {
                                          
                                            int SAPAdvanceBillPaymentId = Convert.ToInt32(transaction.SAPAdvancePaymentId);
                                            objPaymentDetail = suzlonBPPEntities.PaymentDetailAdvances.FirstOrDefault(s => s.SAPAdvancePaymentId == SAPAdvanceBillPaymentId);
                                            if (objPaymentDetail != null && documentNumber != "")
                                            {
                                                objPaymentDetail.DocumentClearingNo = documentNumber;
                                                suzlonBPPEntities.Entry(objPaymentDetail).State = EntityState.Modified;
                                                suzlonBPPEntities.SaveChanges();
                                            }
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                }


                                return result.ToUpper();
                            }
                        }
                        else
                        {
                            StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                            sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                            + "BAPI :Yfif_Ap_Acc_Doc_Clear1 " + Constants.SAP_ERROR));
                            sw.WriteLine(Convert.ToString(bapiResult));
                            sw.Close();
                            return Constants.SAP_ERROR;
                        }
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                        sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                        + "Error " + ex.Message + "" + ex.StackTrace.ToString()));
                        sw.Close();
                        CommonFunctions.WriteErrorLog(ex);
                        return Constants.SAP_ERROR;
                    }
                }
            }
            else
            {
                StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                + "BAPI :Yfif_Ap_Acc_Doc_Clear1 : Count is 0"));
                sw.Close();

                if (TransactionsToPush.Count > 0)
                    return Constants.SAP_SUCCESS;
                else
                    return Constants.DETAIL_SAVE_FAILURE;

               
            }
        }


        public string PushDataToSAPReversePaymentAdvance(string ClearingDocumentNo, string FiscalYear, string CompCode, string ReverseReason, DateTime PostingDate)
        {
            SAPHelper objSAP = new SAPHelper();
            string companyCode = string.Empty;
            string InvoiceDocNo = string.Empty;
            CashAndBank_SAP.BAPIRET2Table bapiRetResult = new CashAndBank_SAP.BAPIRET2Table();
            using (SAPConnection connection = objSAP.GetSAPConnection())
            {
                CashAndBank_SAP.AdvancePayment obj = new CashAndBank_SAP.AdvancePayment();
                obj.Connection = connection;
                try
                {
                    obj.Connection.Open();
                }
                catch (Exception ex)
                {               
                    CommonFunctions.WriteErrorLog(ex);
                    return Constants.SAP_CONNECTION_FAILURE;
                }
                try
                {
                    //obj.Yfif_Bpm_Reset_Clr_Itm(CompCode, ClearingDocumentNo, FiscalYear, PostingDate.ToString("yyyyMMdd"), ReverseReason, out companyCode, out InvoiceDocNo, ref bapiResult);
                    obj.Yfif_Bpm_Fb08_Reset("09", CompCode, ClearingDocumentNo, FiscalYear, PostingDate.ToString("yyyyMMdd"), ReverseReason,out companyCode,out InvoiceDocNo, ref bapiRetResult);

                    System.Data.DataTable bapiResult = bapiRetResult.ToADODataTable();


                    if (bapiResult != null && bapiResult.Rows.Count > 0)
                    {


                        if (ConfigurationManager.AppSettings["TraceLog"].ToString().ToLower() == "true")
                        {
                            StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                            try
                            {

                                sw.WriteLine(Convert.ToString(Environment.NewLine.Trim()+"Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                            + "BAPIAGAINST :Yfif_Bpm_Fb08_Reset" + Environment.NewLine));
                                sw.WriteLine(Convert.ToString("TYPE" + bapiResult.Rows[0]["TYPE"] + Environment.NewLine.Trim()));
                                sw.WriteLine(Convert.ToString("MSGNO" + bapiResult.Rows[0]["LOG_MSG_NO"] + Environment.NewLine.Trim()));
                                sw.WriteLine(Convert.ToString("NUMBER" + bapiResult.Rows[0]["NUMBER"] + Environment.NewLine.Trim()));
                                sw.WriteLine(Convert.ToString("Message" + bapiResult.Rows[0]["MESSAGE"] + Environment.NewLine.Trim()));

                                for (int i = 0; i < bapiResult.Rows.Count; i++)
                                {
                                    sw.WriteLine(Convert.ToString(Environment.NewLine.Trim()+"Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()));
                                    sw.WriteLine(Convert.ToString("TYPE" + bapiResult.Rows[i]["TYPE"] + Environment.NewLine.Trim()));
                                    sw.WriteLine(Convert.ToString("MSGNO" + bapiResult.Rows[i]["LOG_MSG_NO"] + Environment.NewLine.Trim()));
                                    sw.WriteLine(Convert.ToString("NUMBER" + bapiResult.Rows[i]["NUMBER"] + Environment.NewLine.Trim()));
                                    sw.WriteLine(Convert.ToString("Message" + bapiResult.Rows[i]["MESSAGE"] + Environment.NewLine.Trim()));

                                    DataRow[] dr1 = bapiResult.Select("NUMBER='312' AND TYPE='S'");
                                    if (dr1 != null && dr1.Length > 0)
                                    {
                                        sw.WriteLine(Convert.ToString(Environment.NewLine.Trim() + "Suceesfully got record row"+ dr1.Length + Environment.NewLine.Trim()));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                sw.WriteLine("Error while writing result :" + ex.Message);
                            }
                            sw.Close();
                        }

                        DataRow[] dr = bapiResult.Select("NUMBER='312' AND TYPE='S'");
                        if (dr != null && dr.Length > 0)
                        {
                            return Constants.SAP_SUCCESS;
                        }
                        else
                        {
                            return Convert.ToString(bapiResult.Rows[0]["MESSAGE"]);
                        }

                        //string result = Convert.ToString(bapiResult.Rows[1]["NUMBER"]);
                        //if (result == "312")
                        //{
                        //    return Constants.SAP_SUCCESS;
                        //}
                        //else
                        //    return Convert.ToString(bapiResult.Rows[1]["MESSAGE"]);

                    }
                    else
                    {
                        return Constants.SAP_ERROR;
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                    return ex.Message;
                }

            }
        }
        public string PushDataToSAPReversePaymentAgainst(string ClearingDocumentNo, string FiscalYear, string CompCode, string ReverseReason, DateTime PostingDate)
        {
            SAPHelper objSAP = new SAPHelper();
            string companyCode = string.Empty;
            string InvoiceDocNo = string.Empty;
            CashAndBank_SAP.BAPIRET2Table bapiRetResult = new CashAndBank_SAP.BAPIRET2Table();
            using (SAPConnection connection = objSAP.GetSAPConnection())
            {
                CashAndBank_SAP.payment obj = new CashAndBank_SAP.payment();
                obj.Connection = connection;
                try
                {
                    obj.Connection.Open();
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                    return Constants.SAP_CONNECTION_FAILURE;
                }
                try
                {
                    obj.Yfif_Bpm_Reset_Clr_Itm(CompCode, ClearingDocumentNo, FiscalYear, PostingDate.ToString("yyyyMMdd"), ReverseReason, out companyCode, out InvoiceDocNo, ref bapiRetResult);

                    System.Data.DataTable bapiResult = bapiRetResult.ToADODataTable();


                    if (bapiResult != null && bapiResult.Rows.Count > 0)
                    {


                        if (ConfigurationManager.AppSettings["TraceLog"].ToString().ToLower() == "true")
                        {
                            StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                            try
                            {
                             
                            sw.WriteLine(Convert.ToString(Environment.NewLine.Trim()+"Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                            + "BAPIAGAINST :Yfif_Bpm_Reset_Clr_Itm" + Environment.NewLine)  );
                                sw.WriteLine(Convert.ToString("TYPE" + bapiResult.Rows[0]["TYPE"] + Environment.NewLine.Trim()));
                                sw.WriteLine(Convert.ToString("MSGNO" + bapiResult.Rows[0]["LOG_MSG_NO"] + Environment.NewLine.Trim()));
                                sw.WriteLine(Convert.ToString("NUMBER" + bapiResult.Rows[0]["NUMBER"] + Environment.NewLine.Trim()));
                                sw.WriteLine(Convert.ToString("Message" + bapiResult.Rows[0]["MESSAGE"] + Environment.NewLine.Trim()));

                                for (int i = 0; i < bapiResult.Rows.Count; i++)
                                {
                                    sw.WriteLine(Convert.ToString(Environment.NewLine.Trim()+"Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()));
                                    sw.WriteLine(Convert.ToString("TYPE" + bapiResult.Rows[i]["TYPE"] + Environment.NewLine.Trim()));
                                    sw.WriteLine(Convert.ToString("MSGNO" + bapiResult.Rows[i]["LOG_MSG_NO"] + Environment.NewLine.Trim()));
                                    sw.WriteLine(Convert.ToString("NUMBER" + bapiResult.Rows[i]["NUMBER"] + Environment.NewLine.Trim()));
                                    sw.WriteLine(Convert.ToString("Message" + bapiResult.Rows[i]["MESSAGE"] + Environment.NewLine.Trim()));

                                    DataRow[] dr1 = bapiResult.Select("NUMBER='602' AND TYPE='S'");
                                    if (dr1 != null && dr1.Length > 0)
                                    {
                                        sw.WriteLine(Convert.ToString(Environment.NewLine.Trim() + "Suceesfully got record row"+ dr1.Length + Environment.NewLine.Trim()));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                sw.WriteLine("Error while writing result :" + ex.Message);
                            }
                            sw.Close();
                        }

                        DataRow[] dr = bapiResult.Select("NUMBER='602' AND TYPE='S'");
                        if (dr!=null && dr.Length > 0)
                        {
                            return Constants.SAP_SUCCESS;
                        }
                        else
                        {
                            return Convert.ToString(bapiResult.Rows[0]["MESSAGE"]);
                        }
                        
                        //string result = Convert.ToString(bapiResult.Rows[1]["NUMBER"]);
                        //if (result == "312")
                        //{
                        //    return Constants.SAP_SUCCESS;
                        //}
                        //else
                        //    return Convert.ToString(bapiResult.Rows[1]["Message"]);


                    }
                    else
                    {
                        return Constants.SAP_ERROR;
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                    return ex.Message;
                }

            }
        }
        public SAPAdvancePayment GetSapAdvanceDetailById(int sapID)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                SAPAdvancePayment obj= suzlonBPPEntities.SAPAdvancePayments.AsNoTracking().FirstOrDefault(S => S.SAPAdvancePaymentId == sapID);
                return obj;
            }
        }

        public string RefreshAdavanceDataFormSAP(int sapAdvanceId)
        {
            SAPHelper objSAP = new SAPHelper();
            string companyCode = string.Empty;
            string InvoiceDocNo = string.Empty;
            string DocumentNo; string CompCode; string FiscalYear; string VendorCode;


            CashAndBank_SAP.YFIS_ADVANCE_PAY_DOCTable paymentAdvanceTableDetail = new CashAndBank_SAP.YFIS_ADVANCE_PAY_DOCTable();
            CashAndBank_SAP.BAPIRETURNTable bapiResult = new CashAndBank_SAP.BAPIRETURNTable();

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                SAPAdvancePayment SAPAdvancePayment = suzlonBPPEntities.SAPAdvancePayments.AsNoTracking().FirstOrDefault(S => S.SAPAdvancePaymentId == sapAdvanceId);

                if (SAPAdvancePayment != null)
                {
                    DocumentNo = Convert.ToString(SAPAdvancePayment.DPRNumber);
                    CompCode = Convert.ToString(SAPAdvancePayment.CompanyCode);
                    FiscalYear = Convert.ToString(SAPAdvancePayment.FiscalYear);
                    VendorCode = Convert.ToString(SAPAdvancePayment.VendorCode);

                    using (SAPConnection connection = objSAP.GetSAPConnection())
                    {
                        CashAndBank_SAP.AdvancePayment obj = new CashAndBank_SAP.AdvancePayment();
                        obj.Connection = connection;
                        try
                        {
                            obj.Connection.Open();
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                            return Constants.SAP_CONNECTION_FAILURE;
                        }
                        try
                        {
                            obj.Yfif_Advance_Pay_Doc_Refresh(DocumentNo, CompCode, FiscalYear, VendorCode, ref paymentAdvanceTableDetail, ref bapiResult);

                            System.Data.DataTable bapiRetResult = bapiResult.ToADODataTable();
                            System.Data.DataTable BapiRetPaymentDetail = paymentAdvanceTableDetail.ToADODataTable();

                            if (bapiRetResult != null && bapiRetResult.Rows.Count > 0)
                            {
                                if (ConfigurationManager.AppSettings["TraceLog"].ToString().ToLower() == "true")
                                {
                                    StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                                    try
                                    {
                                       
                                        sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                                        + "BAPI :Yfif_Advance_Pay_Doc_Refresh" + Environment.NewLine)
                                                                        + "Type" + bapiRetResult.Rows[0]["Type"]
                                                                         + "Message" + bapiRetResult.Rows[0]["Message"]
                                                                        + "Net Amount" + BapiRetPaymentDetail.Rows[0]["DMBTR"]
                                                                        + "TAX_RATE" + BapiRetPaymentDetail.Rows[0]["TAX_RATE"]
                                                                        + "TAX_AMT" + BapiRetPaymentDetail.Rows[0]["TAX_AMT"]
                                                                        + "GROSS" + BapiRetPaymentDetail.Rows[0]["GROSS"]
                                                                        );
                                        for (int i = 0; i < bapiRetResult.Rows.Count; i++)
                                        {
                                            sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                                                        + "BAPI Result :") + Environment.NewLine
                                                                        + "Type" + bapiRetResult.Rows[0]["Type"]
                                                                        + "Message" + bapiRetResult.Rows[0]["Message"]
                                                                        + "Net Amount" + BapiRetPaymentDetail.Rows[0]["DMBTR"]
                                                                        + "TAX_RATE" + BapiRetPaymentDetail.Rows[0]["TAX_RATE"]
                                                                        + "TAX_AMT" + BapiRetPaymentDetail.Rows[0]["TAX_AMT"]
                                                                        + "GROSS" + BapiRetPaymentDetail.Rows[0]["GROSS"]
                                                                        );
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sw.WriteLine("Error while writing result :" + ex.Message);
                                    }
                                    sw.Close();
                                }

                                string response = Convert.ToString(bapiRetResult.Rows[0]["Type"]);
                                if (response.ToUpper() == "E")
                                    return Convert.ToString(bapiRetResult.Rows[0]["Message"]);
                                else
                                {

                                    SAPAdvancePayment.TaxRate = Convert.ToDecimal(BapiRetPaymentDetail.Rows[0]["TAX_RATE"]);
                                    SAPAdvancePayment.TaxAmount = Convert.ToDecimal(BapiRetPaymentDetail.Rows[0]["TAX_AMT"]);
                                    SAPAdvancePayment.GrossAmount = Convert.ToDecimal(BapiRetPaymentDetail.Rows[0]["GROSS"]);
                                    SAPAdvancePayment.Amount = Convert.ToDecimal(BapiRetPaymentDetail.Rows[0]["DMBTR"]);
                                    SAPAdvancePayment.AmountProposed = Convert.ToDecimal(BapiRetPaymentDetail.Rows[0]["DMBTR"]);
                                    SAPAdvancePayment.ModifiedOn = DateTime.Now;
                                    suzlonBPPEntities.Entry(SAPAdvancePayment).State = EntityState.Modified;
                                    suzlonBPPEntities.SaveChanges();

                                   PaymentDetailAdvance AdvancePayment = suzlonBPPEntities.PaymentDetailAdvances.AsNoTracking().FirstOrDefault(S => S.SAPAdvancePaymentId == sapAdvanceId);
                                    AdvancePayment.ApprovedAmount= Convert.ToDecimal(BapiRetPaymentDetail.Rows[0]["DMBTR"]);
                                    AdvancePayment.ModifiedOn = DateTime.Now;
                                    suzlonBPPEntities.Entry(AdvancePayment).State = EntityState.Modified;
                                    suzlonBPPEntities.SaveChanges();


                                    return Constants.SAP_SUCCESS;


                                }

                            }

                            else
                            {
                                return Constants.SAP_RESULT;
                            }
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                            return Constants.SAP_RESULT;
                        }

                    }
                }
                else
                {
                    return Constants.SAP_SUCCESS;
                } 
            }
        }

        public List<ListItem> GetStatusList()
        {
            List<ListItem> lstItems = new List<ListItem>();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                try
                {
                    lstItems = (from c in suzlonBPPEntities.StatusMasters
                                where c.ShowInDropDown == true
                                select new ListItem()
                                {
                                    Id = c.StatusId.ToString(),
                                    Name = c.Status
                                }).ToList<ListItem>();
                }
                catch (Exception ex)
                {
                    string str = ex.ToString();
                }
                finally { }
                return lstItems;
            }
        }

        public string UpdatePaymentDetailAdvance(List<PaymentDetailAdvance> Transactions, List<PaymentAllocationDetail> Allocations, string saveMode, string HouseBank = "")
        {
            string sapDataPushResult = saveMode == Constants.SAVE_MODE ? Constants.SAP_SUCCESS : PushAdvancePaymentDataToSAP(Transactions, HouseBank);
            if (sapDataPushResult == Constants.SAP_SUCCESS || sapDataPushResult == Constants.SAP_WARNING)
            {
                //int count = 0;
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    try
                    {
                        foreach (PaymentDetailAdvance obj in Transactions)
                        {
                            PaymentDetailAdvance objTemp = suzlonBPPEntities.PaymentDetailAdvances.Where(c => c.SAPAdvancePaymentId == obj.SAPAdvancePaymentId).FirstOrDefault();
                            objTemp.ApprovedAmount = obj.ApprovedAmount;
                            objTemp.PaymentWorkFlowStatusId = obj.PaymentWorkFlowStatusId;
                            objTemp.StatusId = obj.StatusId;
                            objTemp.ModifiedBy = obj.ModifiedBy;
                            objTemp.ModifiedOn = obj.ModifiedOn;
                            objTemp.Comment = obj.Comment;
                            objTemp.ProfileID = obj.ProfileID;
                            objTemp.ApprovedLineGroupID = obj.ApprovedLineGroupID;
                            objTemp.SaveMode = obj.SaveMode;
                            suzlonBPPEntities.Entry(objTemp).State = EntityState.Modified;

                            suzlonBPPEntities.SaveChanges();
                        }
                        if (Allocations != null)
                        {
                            List<int?> delItems = Allocations.Select(o => o.SAPPaymentID).Distinct().ToList();
                            foreach (int Items in delItems)
                            {
                                List<PaymentAllocationDetail> objTemp = suzlonBPPEntities.PaymentAllocationDetails.Where(c => c.SAPPaymentID == Items).ToList();
                                //suzlonBPPEntities.PaymentAllocationDetails.RemoveRange(objTemp); // delete range not working
                                foreach (PaymentAllocationDetail obj in objTemp)
                                {
                                    suzlonBPPEntities.PaymentAllocationDetails.Remove(obj);
                                    suzlonBPPEntities.SaveChanges();
                                }
                            }

                            foreach (PaymentAllocationDetail obj in Allocations)
                            {
                                PaymentAllocationDetail objTempAllocation = new PaymentAllocationDetail();
                                objTempAllocation.AllocatedAmount = obj.AllocatedAmount;
                                objTempAllocation.CreatedBy = obj.CreatedBy;
                                objTempAllocation.CreatedOn = obj.CreatedOn;
                                objTempAllocation.ModifiedBy = obj.ModifiedBy;
                                objTempAllocation.SAPPaymentID = obj.SAPPaymentID;
                                objTempAllocation.SAPPaymentTYPE = obj.SAPPaymentTYPE;
                                objTempAllocation.TreasuryDetailId = obj.TreasuryDetailId;
                                suzlonBPPEntities.PaymentAllocationDetails.Add(objTempAllocation);
                                suzlonBPPEntities.SaveChanges();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                        CommonFunctions.WriteErrorLog(ex);
                        return Constants.DETAIL_SAVE_FAILURE;
                    }
                    finally { }
                    //return (count > 0) ? false : true;
                    return Constants.DETAIL_SAVE_SUCCESS;
                }
            }
            else
            {
                return sapDataPushResult;
            }
        }


        public string UpdatePaymentDetailAgainst(List<PaymentDetailAgainstBill> Transactions, List<PaymentAllocationDetail> Allocations, string saveMode, string HouseBank = "")
        {

            StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
            sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                            + "Model : UpdatePaymentDetailAgainst start"));
            sw.Close();

            string sapDataPushResult = saveMode == Constants.SAVE_MODE ? Constants.SAP_SUCCESS : PushAgainstBillPaymentDataToSAP(Transactions, HouseBank);
            if (sapDataPushResult == Constants.SAP_SUCCESS || sapDataPushResult == Constants.SAP_WARNING)
            {
                //int count = 0;
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    try
                    {
                        foreach (PaymentDetailAgainstBill obj in Transactions)
                        {
                            PaymentDetailAgainstBill objTemp = suzlonBPPEntities.PaymentDetailAgainstBills.Where(c => c.SAPAgainstBillPaymentId == obj.SAPAgainstBillPaymentId).FirstOrDefault();
                            objTemp.ApprovedAmount = obj.ApprovedAmount;
                            objTemp.PaymentWorkflowStatusId = obj.PaymentWorkflowStatusId;
                            objTemp.StatusId = obj.StatusId;
                            objTemp.ModifiedBy = obj.ModifiedBy;
                            objTemp.ModifiedOn = obj.ModifiedOn;
                            objTemp.ProfileID = obj.ProfileID;
                            objTemp.Comment = obj.Comment;
                            objTemp.ApprovedLineGroupID = obj.ApprovedLineGroupID;
                            objTemp.SaveMode = obj.SaveMode;
                            suzlonBPPEntities.Entry(objTemp).State = EntityState.Modified;
                            suzlonBPPEntities.SaveChanges();
                            /////////////


                            ////////////////////
                        }
                        if (Allocations != null)
                        {
                            List<int?> delItems = Allocations.Select(o => o.SAPPaymentID).Distinct().ToList();
                            foreach (int Items in delItems)
                            {
                                List<PaymentAllocationDetail> objTemp = suzlonBPPEntities.PaymentAllocationDetails.Where(c => c.SAPPaymentID == Items).ToList();
                                //suzlonBPPEntities.PaymentAllocationDetails.RemoveRange(objTemp); // delete range not working
                                foreach (PaymentAllocationDetail obj in objTemp)
                                {
                                    suzlonBPPEntities.PaymentAllocationDetails.Remove(obj);
                                    suzlonBPPEntities.SaveChanges();
                                }
                            }

                            foreach (PaymentAllocationDetail obj in Allocations)
                            {
                                PaymentAllocationDetail objTempAllocation = new PaymentAllocationDetail();
                                objTempAllocation.AllocatedAmount = obj.AllocatedAmount;
                                objTempAllocation.CreatedBy = obj.CreatedBy;
                                objTempAllocation.CreatedOn = obj.CreatedOn;
                                objTempAllocation.ModifiedBy = obj.ModifiedBy;
                                objTempAllocation.SAPPaymentID = obj.SAPPaymentID;
                                objTempAllocation.SAPPaymentTYPE = obj.SAPPaymentTYPE;
                                objTempAllocation.TreasuryDetailId = obj.TreasuryDetailId;
                                suzlonBPPEntities.PaymentAllocationDetails.Add(objTempAllocation);
                                suzlonBPPEntities.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                        CommonFunctions.WriteErrorLog(ex);
                        return Constants.DETAIL_SAVE_FAILURE;
                    }
                    finally { }
                    // return (count > 0) ? false : true;
                    return Constants.DETAIL_SAVE_SUCCESS;
                }
            }
            else
            {
                return sapDataPushResult;
            }
        }

        public List<PaymentComments> getPaymentComment(int Payment, string paymentbillType)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                if (paymentbillType == "AgainstBill")
                {
                    var paymentComment = (from P in suzlonBPPEntities.PaymentDetailAgainstBillTxns
                                          join u in suzlonBPPEntities.Users on P.CreatedBy equals u.UserId
                                          join PS in suzlonBPPEntities.PaymentWorkflowStatus on P.PaymentWorkflowStatusId equals PS.PaymentWorkflowStatusId
                                          where P.SAPAgainstBillPaymentId == Payment
                                          select new PaymentComments
                                          {
                                              UserName = u.Name,
                                              AppovedAmount = (decimal)P.AppovedAmount,
                                              Comment = P.Comment,
                                              WorkFlowStatus = PS.Status,
                                              CreatedOn = P.CreatedOn
                                          }).ToList();
                    return paymentComment;
                }
                else
                {
                    var paymentComment = (from P in suzlonBPPEntities.PaymentDetailAdvanceTxns
                                          join u in suzlonBPPEntities.Users on P.CreatedBy equals u.UserId
                                          join PS in suzlonBPPEntities.PaymentWorkflowStatus on P.PaymentWorkflowStatusId equals PS.PaymentWorkflowStatusId
                                          where P.SAPAdvancePaymentId == Payment
                                          select new PaymentComments
                                          {
                                              UserName = u.Name,
                                              AppovedAmount = (decimal)P.AppovedAmount,
                                              Comment = P.Comment,
                                              WorkFlowStatus = PS.Status,
                                              CreatedOn = P.CreatedOn
                                          }).ToList();
                    return paymentComment;
                }


            }

        }

        //public decimal GetTodaysTransAmountByAggregator(string Compcode, string VendorCode, BillType billType)
        //{
        //    using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
        //    {
        //        int paymentworkflowStatusId = GetPaymentWorkflowStatusId(Convert.ToInt32(Status.Approved), Convert.ToInt32(UserProfileEnum.Aggregator));
        //        decimal? amt = 0;

        //        try
        //        {
        //            if (billType == BillType.Advance)
        //                amt = (from c in suzlonBPPEntities.PaymentDetailAdvanceTxns
        //                       join d in suzlonBPPEntities.SAPAdvancePayments on c.SAPAdvancePaymentId equals d.SAPAdvancePaymentId
        //                       where c.PaymentWorkflowStatusId == paymentworkflowStatusId && c.CreatedOn.Date == DateTime.Today
        //                       && d.CompanyCode == Compcode && d.VendorCode == VendorCode
        //                       select c.AppovedAmount).Sum();
        //            else
        //                amt = (from c in suzlonBPPEntities.PaymentDetailAgainstBillTxns
        //                       join d in suzlonBPPEntities.SAPAgainstBillPayments on c.SAPAgainstBillPaymentId equals d.SAPAgainstBillPaymentId
        //                       where c.PaymentWorkflowStatusId == paymentworkflowStatusId && c.CreatedOn.Date == DateTime.Today
        //                       && d.CompanyCode == Compcode && d.VendorCode == VendorCode
        //                       select c.AppovedAmount).Sum();

        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //        return Convert.ToDecimal(amt);
        //    }
        //}

        public decimal GetDailyPaymentLimit()
        {
            decimal? DailyPaymtLimit = 0;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                try
                {
                    DailyPaymtLimit = (from c in suzlonBPPEntities.ApplicationConfigurations
                                       select c.DailyPaymentLimit).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return Convert.ToDecimal(DailyPaymtLimit);
        }


        public List<GetNextPaymentWorkFlowId_Result> GetNextPaymentWorkFlowId(string CompCode, string VendorCode, BillType BillType, int ProfileId, bool IsOpenAdvnce, decimal AmountToBeApproved)
        {
            List<GetNextPaymentWorkFlowId_Result> lstGetNextPaymentWorkFlowId_Result = new List<GetNextPaymentWorkFlowId_Result>();

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                lstGetNextPaymentWorkFlowId_Result = suzlonBPPEntities.sp_GetNextPaymentWorkFlowId(CompCode, VendorCode, Convert.ToInt32(BillType).ToString(),
                    ProfileId, IsOpenAdvnce, AmountToBeApproved).ToList<GetNextPaymentWorkFlowId_Result>(); ;
            }

            return lstGetNextPaymentWorkFlowId_Result;
        }

        public List<GetBudgetAllocationDetails_Result> GetBudgetAllocationDetails(string mode, string SAPPaymentID)
        {
            List<GetBudgetAllocationDetails_Result> lstGetNextPaymentWorkFlowId_Result = new List<GetBudgetAllocationDetails_Result>();

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                lstGetNextPaymentWorkFlowId_Result = suzlonBPPEntities.GetBudgetAllocationDetails(mode, SAPPaymentID).ToList<GetBudgetAllocationDetails_Result>();
            }
            return lstGetNextPaymentWorkFlowId_Result;
        }

        public string ValidPaymentAllocation(string mode, string PaymentIds)
        {
            string lstGetNextPaymentWorkFlowId_Result = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                lstGetNextPaymentWorkFlowId_Result = suzlonBPPEntities.ValidateAllocationRecords(mode, PaymentIds).ToList().FirstOrDefault(); ;
            }
            return lstGetNextPaymentWorkFlowId_Result;
        }

        public List<PaymentWorkflowStatu> GetPaymentWorkFlowStatus()
        {
            List<PaymentWorkflowStatu> lstPaymentWorkflowStatus = new List<PaymentWorkflowStatu>();

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                try
                {
                    lstPaymentWorkflowStatus = (from c in suzlonBPPEntities.PaymentWorkflowStatus
                                                select c).ToList<PaymentWorkflowStatu>();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return lstPaymentWorkflowStatus;
        }

        public List<GetPaymentWorkFlowInProcessDetail_Result> getPaymentWorkFlowInProcess(string billType)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetPaymentWorkFlowInProcessDetail(billType).ToList();
            }

        }
        public bool isVendorBankUpdationInProcess(string vendorCode, string CompCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                //return suzlonBPPEntities.BankDetails.Where(b => b.CompanyCode == CompCode && b.VendorCode == vendorCode && b.WorkFlowStatusId!=19) != null;
                return suzlonBPPEntities.BankDetails.FirstOrDefault(b => b.CompanyCode == CompCode && b.VendorCode == vendorCode && b.WorkFlowStatusId != 19) != null;
            }
        }

        #region "SAP Connectivity"

        public string SAP_HouseBankKeyData(string companyCode, string houseKey, bool isNeedToUpdateHouseBank)
        {
            string result = string.Empty;
            try
            {
                SAPHelper objSAP = new SAPHelper();
                using (SAPConnection connection = objSAP.GetSAPConnection())
                {
                    CashAndBank_SAP.payment obj = new CashAndBank_SAP.payment();
                    obj.Connection = connection;
                    obj.Connection.Open();
                    CashAndBank_SAP.YFIS_HOUSE_BANKTable houseBankTableDetail = new CashAndBank_SAP.YFIS_HOUSE_BANKTable();
                    CashAndBank_SAP.BAPIRETURN bapiResult = new CashAndBank_SAP.BAPIRETURN();
                    obj.Yfif_Bpm_House_Bank("*", "*", companyCode, houseKey, "*", out bapiResult, ref houseBankTableDetail);
                   
                    if (bapiResult != null)
                    {
                        try
                        {
                            if (ConfigurationManager.AppSettings["TraceLog"].ToString().ToLower() == "true")
                            {
                                string SAPResult = Environment.NewLine + "House Bank Result - " + Environment.NewLine + "Result:" + bapiResult.Type + Environment.NewLine + " Message:" + bapiResult.Message + Environment.NewLine + Environment.NewLine;
                                System.IO.StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                                sw.WriteLine();
                                sw.Close();
                            }
                            if (bapiResult.Type == Constants.SAP_SUCCESS)
                            {
                                if (isNeedToUpdateHouseBank)
                                {
                                    if (houseBankTableDetail != null && houseBankTableDetail.Count > 0)
                                    {
                                        DataTable dt = houseBankTableDetail.ToADODataTable();
                                        using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                                        {
                                            suzlonBPPEntities.UpdateHouseBank_FromSAP(Convert.ToString(dt.Rows[0]["Bankl"]), Convert.ToString(dt.Rows[0]["HBKID"]));
                                        }
                                    }
                                }
                                return Constants.SUCCESS;
                            }
                            else
                                return bapiResult.Message;
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                            return Constants.ERROR_WHILE_UPDATING;
                        }
                    }
                    else
                        return Constants.SAP_RESULT;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return Constants.SAP_CONNECTION_FAILURE;
            }
        }


        public string SAP_GetPaymentData(string vendorCode, string companyCode, bool isAgainstBill)
        {
            try
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    if (isAgainstBill == true)
                        suzlonBPPEntities.DeletePaymentStaggingData("AgainstBill", companyCode, vendorCode);
                    else
                        suzlonBPPEntities.DeletePaymentStaggingData("Advance", companyCode, vendorCode);
                }
                string result = string.Empty;
                SAPHelper objSAP = new SAPHelper();
                string strConnString = Convert.ToString(ConfigurationManager.ConnectionStrings["ConnectionString"]);
                using (SAPConnection connection = objSAP.GetSAPConnection())
                {
                    CashAndBank_SAP.payment obj = new CashAndBank_SAP.payment();
                    obj.Connection = connection;
                    obj.Connection.Open();
                    CashAndBank_SAP.YFIS_AGAINST_BILL_DOCTable paymentAgainstBillTableDetail = new CashAndBank_SAP.YFIS_AGAINST_BILL_DOCTable();
                    CashAndBank_SAP.YFIS_ADVANCE_PAY_DOCTable paymentAdvanceTableDetail = new CashAndBank_SAP.YFIS_ADVANCE_PAY_DOCTable();
                    CashAndBank_SAP.BAPIRETURNTable bapiResult = new CashAndBank_SAP.BAPIRETURNTable();
                    if (isAgainstBill)
                        obj.Yfif_Against_Bill_Doc(companyCode, vendorCode, ref paymentAgainstBillTableDetail, ref bapiResult);
                    else
                        obj.Yfif_Advance_Pay_Doc(companyCode, vendorCode, ref paymentAdvanceTableDetail, ref bapiResult);
                    System.Data.DataTable bapiRetResult = bapiResult.ToADODataTable();
                    if (bapiRetResult != null && bapiRetResult.Rows.Count > 0)
                    {
                        string response = Convert.ToString(bapiRetResult.Rows[0]["Type"]);
                        if (response.ToUpper() == "E")
                            return Convert.ToString(bapiRetResult.Rows[0]["Message"]);
                        try
                        {
                            if (isAgainstBill)
                            {
                                DataTable aginstBillDataTable = paymentAgainstBillTableDetail.ToADODataTable();
                                if (aginstBillDataTable.Rows.Count > 0)
                                {
                                    SqlBulkCopy sqlcopy = new SqlBulkCopy(strConnString);
                                    sqlcopy.DestinationTableName = "SAPAgainstBillPayment_Stagging";
                                    sqlcopy.WriteToServer(aginstBillDataTable);
                                    using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                                    {
                                        suzlonBPPEntities.InsertAgainstBillPaymentFromStaging();
                                    }
                                }
                            }
                            else
                            {
                                DataTable aginstBillDataTable = paymentAdvanceTableDetail.ToADODataTable();
                                if (aginstBillDataTable.Rows.Count > 0)
                                {
                                    SqlBulkCopy sqlcopy = new SqlBulkCopy(strConnString);
                                    sqlcopy.DestinationTableName = "SAPAdvancePayment_Stagging";
                                    sqlcopy.WriteToServer(aginstBillDataTable);
                                    using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                                    {
                                        suzlonBPPEntities.InsertAdvancePaymentFromStaging();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            CommonFunctions.WriteErrorLog(ex);
                            return Constants.ERROR_WHILE_INSERTING;
                        }
                        return Constants.SUCCESS;
                    }
                    else
                        return Constants.SAP_RESULT;
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteErrorLog(ex);
                return Constants.SAP_CONNECTION_FAILURE;
            }
        }

        #endregion "SAP Connectivity"

        #region "Notifications"       

        public void sendPayReqSubmitNotification(SAPAgainstBillPayment AgainstBill, SAPAdvancePayment Advance, int userId, string BillType)
        {
            string InitiatorUserName = string.Empty;
            string AggregatorUserEmail = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == userId);
                if (user != null)
                    InitiatorUserName = user.Name;

                if (BillType == "Against Bill")
                {
                    var AggregatorDetail = (from S in suzlonBPPEntities.SAPAgainstBillPayments
                                            join AgBill in suzlonBPPEntities.PaymentDetailAgainstBills on S.SAPAgainstBillPaymentId equals AgBill.SAPAgainstBillPaymentId
                                            join PW in suzlonBPPEntities.PaymentWorkflows on AgBill.SubVerticalId equals PW.SubVerticalId
                                            join U in suzlonBPPEntities.Users on PW.PriAggUserId equals U.UserId
                                            select new
                                            {
                                                U.Name,
                                                U.EmailId
                                            }).FirstOrDefault();

                    if (AggregatorDetail != null)
                        AggregatorUserEmail = AggregatorDetail.EmailId;

                    NotificationModel.SendPaymentInitiationNotification(InitiatorUserName, Convert.ToString(AgainstBill.DocumentNumber), BillType, Convert.ToString(AgainstBill.CompanyCode), Convert.ToString(AgainstBill.VendorName), Convert.ToString(AgainstBill.AmountProposed), AggregatorUserEmail);
                }
                if (BillType == "Advance")
                {
                    var AggregatorDetail = (from S in suzlonBPPEntities.SAPAdvancePayments
                                            join AgBill in suzlonBPPEntities.PaymentDetailAgainstBills on S.SAPAdvancePaymentId equals AgBill.SAPAgainstBillPaymentId
                                            join PW in suzlonBPPEntities.PaymentWorkflows on AgBill.SubVerticalId equals PW.SubVerticalId
                                            join U in suzlonBPPEntities.Users on PW.PriAggUserId equals U.UserId
                                            select new
                                            {
                                                U.Name,
                                                U.EmailId
                                            }).FirstOrDefault();

                    if (AggregatorDetail != null)
                        AggregatorUserEmail = AggregatorDetail.EmailId;

                    NotificationModel.SendPaymentInitiationNotification(InitiatorUserName, Convert.ToString("doc no"), BillType, Convert.ToString(Advance.CompanyCode), Convert.ToString(Advance.VendorName), Convert.ToString(Advance.AmountProposed), AggregatorUserEmail);

                }

            }
        }


        #endregion "Notifications"


        public decimal getVerticalBalance(Int32 VerticalID)
        {
            decimal getVerticalBalance_Result = 0;

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                getVerticalBalance_Result = (decimal)suzlonBPPEntities.sp_getVerticalBalance((Nullable<int>)VerticalID).FirstOrDefault(); ;
            }
            return getVerticalBalance_Result;
        }
        public List<sp_getVerticalBalanceDetails_Result> getVerticalBalanceDetails(Int32 VerticalID)
        {
            List<sp_getVerticalBalanceDetails_Result> getVerticalBalanceDetails = new List<sp_getVerticalBalanceDetails_Result>();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                getVerticalBalanceDetails = suzlonBPPEntities.sp_getVerticalBalanceDetails((Nullable<int>)VerticalID).ToList<sp_getVerticalBalanceDetails_Result>(); ;
            }
            return getVerticalBalanceDetails;
        }

        public Decimal getBalanceByTreasury(Int32 TreasuryID)
        {
            Decimal getBalanceByTreasury_Result = 0;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                getBalanceByTreasury_Result = (Decimal)suzlonBPPEntities.sp_getBalanceByTreasury(TreasuryID).FirstOrDefault();
            }
            return getBalanceByTreasury_Result;
        }

        public List<sp_getBalanceDetailsByTreasury_Result> getBalanceDetailsByTreasury(Int32 TreasuryID)
        {
            List<sp_getBalanceDetailsByTreasury_Result> getBalanceDetailsByTreasury_Result = new System.Collections.Generic.List<sp_getBalanceDetailsByTreasury_Result>();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                getBalanceDetailsByTreasury_Result = suzlonBPPEntities.sp_getBalanceDetailsByTreasury(TreasuryID).ToList();
            }
            return getBalanceDetailsByTreasury_Result;
        }


        public List<usp_GetPaymentInitiatorPopupData_Result> GetPaymentInitiatorPopupData(string VendorCode, string CompanyCode, string BillType)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<usp_GetPaymentInitiatorPopupData_Result> lstdata = suzlonBPPEntities.GetPaymentInitiatorPopupData(VendorCode.Trim(), CompanyCode.Trim(), BillType.Trim()).ToList();
                return lstdata;
            }
        }
        public List<usp_GetPaymentInitiatorPopupData_Advance_Result> GetPaymentInitiatorPopupDataAdvance(string VendorCode, string CompanyCode, string BillType)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<usp_GetPaymentInitiatorPopupData_Advance_Result> lstdata = suzlonBPPEntities.GetPaymentInitiatorPopupData_Advance(VendorCode.Trim(), CompanyCode.Trim(), BillType.Trim()).ToList();
                return lstdata;
            }
        }
        
        public SAPAgainstBillPayment SAPAgainstBillSubmit(ArrayList values)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                  try
                    {
                        int Id = 0;
                        int sts = 0;
                        SAPAgainstBillPayment SAP = new SAPAgainstBillPayment();
                        SAP.CompanyCode = Convert.ToString(values[0]);
                        SAP.VendorCode = Convert.ToString(values[1]);
                        SAP.VendorName = Convert.ToString(values[2]);
                        SAP.NatureofRequestId = Convert.ToInt32(values[3]);
                        SAP.DocumentNumber = Convert.ToString(values[4]);
                        SAP.Reference = Convert.ToString(values[5]);
                        SAP.FiscalYear = Convert.ToInt32(values[6]);
                    if (!string.IsNullOrEmpty(Convert.ToString(values[7])))
                        SAP.PostDate = Convert.ToDateTime(values[7]);
                        SAP.Amount = Convert.ToDecimal(values[8]);
                        SAP.AmountProposed = Convert.ToDecimal(values[9]);
                        SAP.Currency = Convert.ToString(values[10]);
                    if (!string.IsNullOrEmpty(Convert.ToString(values[11])))
                        SAP.DueDays = Convert.ToInt32(values[11]);

                    if (!string.IsNullOrEmpty(Convert.ToString(values[12])))
                        SAP.BaseLineDate = Convert.ToDateTime(values[12]);
                        SAP.BusinessArea = Convert.ToString(values[13]);
                        SAP.ProfitCentre = Convert.ToString(values[14]);
                    if (!string.IsNullOrEmpty(Convert.ToString(values[15])))
                        SAP.DocumentDate = Convert.ToDateTime(values[15]);
                        SAP.PaymentMethod = Convert.ToString(values[16]);                      
                        SAP.IsWIP = false;
                        SAP.CreatedBy = Convert.ToInt32(values[17]);
                        SAP.CreatedOn = DateTime.Now;
                        SAP.ModifiedBy = Convert.ToInt32(values[17]);
                        SAP.ModifiedOn = DateTime.Now;
                        SAP.DCFLag = Convert.ToString(values[19]);

                        Entities.SAPAgainstBillPayments.Add(SAP);
                        Entities.SaveChanges();

                    List<FileUploadModel> lstFileUpload = new List<FileUploadModel>();
                    if(!string.IsNullOrEmpty(Convert.ToString(values[18])))
                    lstFileUpload = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadModel>>(values[18].ToString()); 

                    if (lstFileUpload != null && lstFileUpload.Count > 0)
                    {
                        lstFileUpload.ForEach(request =>
                        {
                            var appPath = AppDomain.CurrentDomain.BaseDirectory;
                            FileUpload requestModel = new FileUpload();
                            requestModel.EntityId = SAP.SAPAgainstBillPaymentId;
                            requestModel.EntityName = request.EntityName;
                            requestModel.FileName = request.FileName;
                            requestModel.DisplayName = request.DisplayName;
                            requestModel.CreatedBy = Convert.ToInt32(values[17]);
                            requestModel.CreatedOn = DateTime.Now;
                            requestModel.ModifiedBy = Convert.ToInt32(values[17]);
                            requestModel.ModifiedOn = DateTime.Now;
                            Entities.FileUploads.Add(requestModel);
                            Entities.SaveChanges();
                            var newFileName = request.FileName.Replace(Convert.ToInt32(values[4]) + "!", requestModel.FileUploadId + "!");
                            System.IO.File.Move(appPath + Constants.VENDOR_BANK_ATTACHMENT_PATH_TEMP + request.FileName, appPath + Constants.VENDOR_BANK_ATTACHMENT_PATH + newFileName);
                            requestModel.FileName = newFileName;
                            Entities.SaveChanges();
                        });
                    }

                    return SAP;
                    }
                    catch (Exception ex)
                    {
                    CommonFunctions.WriteErrorLog(ex);
                    return null;
                    }
                }
            }


        public SAPAdvancePayment SAPAdvanceBillSubmit(ArrayList values)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                try
                {
                    int Id = 0;
                    int sts = 0;
                    SAPAdvancePayment SAP = new SAPAdvancePayment();
                    SAP.CompanyCode = Convert.ToString(values[0]);
                    SAP.VendorCode = Convert.ToString(values[1]);
                    SAP.VendorName = Convert.ToString(values[2]);
                    SAP.Natureofrequest = Convert.ToInt32(values[3]);
                    SAP.DPRNumber = Convert.ToString(values[4]);
                    SAP.Reference = Convert.ToString(values[5]);
                    SAP.FiscalYear = Convert.ToInt32(values[6]);
                    if (!string.IsNullOrEmpty(Convert.ToString(values[7]))) 
                    SAP.PostingDate = Convert.ToDateTime(values[7]);

                    SAP.Amount = Convert.ToDecimal(values[8]);
                    SAP.AmountProposed = Convert.ToDecimal(values[9]);
                    SAP.Currency = Convert.ToString(values[10]);
                    if (!string.IsNullOrEmpty(Convert.ToString(values[11])))
                        SAP.ExpectedClearingDate = Convert.ToDateTime(values[11]);
                    if(!string.IsNullOrEmpty(Convert.ToString(values[12])))
                    SAP.POLineItemNo = Convert.ToInt32(values[12]);

                    SAP.BusinessArea = Convert.ToString(values[13]);
                    SAP.ProfitCentre = Convert.ToString(values[14]);
                    SAP.PurchasingDocument = Convert.ToString(values[15]);
                    SAP.PaymentMethod = Convert.ToString(values[16]); 
                    SAP.POItemText = Convert.ToString(values[17]);
                    SAP.SpecialGL= Convert.ToString(values[18]);
                    SAP.WithholdingTaxCode= Convert.ToString(values[19]);
                    SAP.UnsettledOpenAdvance= Convert.ToDecimal(values[20]);
                    SAP.JustificationforAdvPayment= Convert.ToString(values[21]);
                    if (!string.IsNullOrEmpty(Convert.ToString(values[22])))
                        SAP.Documentdate = Convert.ToDateTime(values[22]);
                    SAP.HouseBank = Convert.ToString(values[23]);                    
                    SAP.IsWIP = false;
                    SAP.CreatedBy = Convert.ToInt32(values[24]);
                    SAP.CreatedOn = DateTime.Now;
                    SAP.ModifiedBy = Convert.ToInt32(values[24]);
                    SAP.ModifiedOn = DateTime.Now;
                    SAP.DCFLag= Convert.ToString(values[26]);
                    SAP.TaxRate = Convert.ToDecimal(values[27]);
                    SAP.TaxAmount = Convert.ToDecimal(values[28]);
                    SAP.GrossAmount = Convert.ToDecimal(values[29]);
                    Entities.SAPAdvancePayments.Add(SAP);
                    Entities.SaveChanges();

                    List<FileUploadModel> lstFileUpload = new List<FileUploadModel>();
                    if (!string.IsNullOrEmpty(Convert.ToString(values[25])))
                        lstFileUpload = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadModel>>(values[25].ToString());

                    if (lstFileUpload != null || lstFileUpload.Count > 0)
                    {
                        lstFileUpload.ForEach(request =>
                        {
                            var appPath = AppDomain.CurrentDomain.BaseDirectory;
                            FileUpload requestModel = new FileUpload();
                            requestModel.EntityId = SAP.SAPAdvancePaymentId;
                            requestModel.EntityName = request.EntityName;
                            requestModel.FileName = request.FileName;
                            requestModel.DisplayName = request.DisplayName;
                            requestModel.CreatedBy = Convert.ToInt32(values[24]);
                            requestModel.CreatedOn = DateTime.Now;
                            requestModel.ModifiedBy = Convert.ToInt32(values[24]);
                            requestModel.ModifiedOn = DateTime.Now;
                            Entities.FileUploads.Add(requestModel);
                            Entities.SaveChanges();
                            var newFileName = request.FileName.Replace(Convert.ToInt32(values[4]) + "!", requestModel.FileUploadId + "!");
                            System.IO.File.Move(appPath + Constants.VENDOR_BANK_ATTACHMENT_PATH_TEMP + request.FileName, appPath + Constants.VENDOR_BANK_ATTACHMENT_PATH + newFileName);
                            requestModel.FileName = newFileName;
                            Entities.SaveChanges();
                        });
                    }


                    return SAP;
                }
                catch (Exception ex)
                {
                    CommonFunctions.WriteErrorLog(ex);
                    return null;
                }
            }
        }

        public List<sp_GetReverseCaseData_Result> getAdvanceReverseCaseDetail(int sapAdvanceId,string billType)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                Entities.Configuration.ProxyCreationEnabled = false;
                return  Entities.sp_GetReverseCaseData(sapAdvanceId, billType).ToList();
           }
        }

        public List<ListItem> fillReverseReason()
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {

                var ReasonList = (from R in Entities.PayReverseReasons
                                  select new ListItem
                                  {
                                      Id = R.ReasonID.ToString(),
                                      Name = R.Reason
                                  }).ToList();
                
                return ReasonList;                
            }
        }

        public List<sp_GetReversePaymentDetail_Result> fillReversePaymentDetail(int VerticalId,string BillType)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                return Entities.sp_GetReversePaymentDetail(VerticalId, BillType).ToList();
            }
        }

        public bool UpdateReverseCaseDetail(string DocumentClearingNo, int ReverseReasonId, string billType,DateTime ReverseDate)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                 Entities.sp_UpdateReversePayment(DocumentClearingNo, ReverseReasonId, billType, ReverseDate);
                return true;
            }
        }

        public List<Usp_GetReversedPaymentDone_Result> GetReversedDetail(int VerticalId, string BillType)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                return Entities.Usp_GetReversedPaymentDone(VerticalId, BillType).ToList();
            }
        }

        public DropdownValues GetVendorCodeForReport(string CompantCode)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                DropdownValues ddValues = new DropdownValues();
                ddValues.VendorCode = new List<ListItem>();
                ddValues.VendorCode.Add(new ListItem() { Id = "All", Name = "All" });
                (from V in Entities.VendorMasters
                 where V.CompanyCode == CompantCode
                 select new ListItem()
                 {
                     Id = V.VendorCode.ToString(),
                     Name = V.VendorCode
                 }).OrderBy(s => s.Name).Distinct().ToList().ForEach(item =>
                 {
                     if (item != null)
                         ddValues.VendorCode.Add(item);
                 });

                return ddValues;
            }
        }

        public DropdownValues GetVendorNameForReport(string CompantCode)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                DropdownValues ddValues = new DropdownValues();
                ddValues.VendorName = new List<ListItem>();
                ddValues.VendorName.Add(new ListItem() { Id = "All", Name = "All" });
                (from V in Entities.VendorMasters
                 where V.CompanyCode == CompantCode
                 select new ListItem()
                 {
                     Id = V.VendorName.ToString(),
                     Name = V.VendorName
                 }).OrderBy(s => s.Name).Distinct().ToList().ForEach(item =>
                 {
                     if (item != null)
                         ddValues.VendorName.Add(item);
                 });

                return ddValues;
            }
        }

        public List<Usp_PaymentDetailReportData_Result> GetPaymentReportDataAgainst(string SubVerticalId, string CompCode,string vendorcode, DateTime FromDate,DateTime ToDate,string UserId)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                return Entities.Usp_PaymentDetailReportData("Against", SubVerticalId, CompCode, vendorcode, FromDate, ToDate, UserId).ToList(); ;
            }
        }

        public List<Usp_PaymentDetailAdvanceReportData_Result> GetPaymentReportDataAdvance(string SubVerticalId, string CompCode, string vendorcode, DateTime FromDate, DateTime ToDate, string UserId)
        {
            using (SuzlonBPPEntities Entities = new SuzlonBPPEntities())
            {
                return Entities.Usp_PaymentDetailAdvanceReportData("Advance", SubVerticalId, CompCode, vendorcode, FromDate, ToDate, UserId).ToList(); ;
            }
        }

    }

    }

