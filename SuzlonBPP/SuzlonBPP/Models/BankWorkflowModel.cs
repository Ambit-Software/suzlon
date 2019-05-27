using SAP.Connector;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Newtonsoft.Json;
using Cryptography;
using System.Web;

namespace SuzlonBPP.Models
{
    public class BankWorkflowModel
    {
        public Models.BankWorkflow bankWorkFlow;
        public SubVerticalMaster subVerticalMaster;
        public bool InitiatorAccess(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                //   return suzlonBPPEntities.ProfileMasters.FirstOrDefault(l => l.ProfileName.ToLower() == profileModel.ProfileName.ToLower() && l.ProfileId != profileModel.ProfileId) != null;
                return suzlonBPPEntities.BankWorkflows.FirstOrDefault(w => w.PriVerContUserId == userId || w.PriGrpContUserId == userId || w.PriTreasuryUserId == userId || w.PriMgmtAssUserId == userId || w.PriFASSCUserId == userId || w.PriCBUserId == userId) != null;
            }
        }

        public bool IfscAcnoExists(int BankDetailids, string IFSCCode, string AccNo, string VendorCode, string CompanyCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter IsExist = new System.Data.Entity.Core.Objects.ObjectParameter("IsExist", typeof(Boolean));
                //return suzlonBPPEntities.BankDetailHistories.FirstOrDefault(B => B.BankDetailId == BankDetailids && B.IFSCCode.Trim().ToLower() == IFSCCode.Trim().ToLower() && B.AccountNumber.Trim().ToLower() == AccNo.Trim().ToLower() && B.IsRecordPushToSAP == true) != null;
                suzlonBPPEntities.chkAccNoExistForVendorAndCompanyCode(BankDetailids, AccNo, IFSCCode, VendorCode, CompanyCode, IsExist);
                return Convert.ToBoolean(IsExist.Value);
            }
        }

        public bool IfscAcnoExistsMail(BankDetailModel bankModel)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                bool IsExist;
                IsExist = suzlonBPPEntities.BankDetailHistories.FirstOrDefault(B => B.BankDetailId == bankModel.BankDetailId && B.AccountNumber == bankModel.AccountNumber && B.IsRecordPushToSAP == true
                && B.SuzlonEmailID1 == bankModel.SuzlonEmailID1 && B.SuzlonEmailID2 == bankModel.SuzlonEmailID2 && B.VendorEmailID1 == bankModel.VendorEmailID1 && B.VendorEmailID2 == bankModel.VendorEmailID2
                && B.AccountType == bankModel.AccountType && B.BankName == bankModel.BankName && B.BranchName == bankModel.BranchName && B.City == bankModel.City) != null;
                if (!IsExist)
                    IsExist = suzlonBPPEntities.VendorBankMasters.FirstOrDefault(B => B.VendorCode == bankModel.VendorCode
                     && B.CompanyCode == bankModel.CompanyCode && B.AccountNumber == bankModel.AccountNumber
                     && B.SuzlonEmailID1 == bankModel.SuzlonEmailID1 && B.SuzlonEmailID2 == bankModel.SuzlonEmailID2 && B.VendorEmailID1 == bankModel.VendorEmailID1 && B.VendorEmailID2 == bankModel.VendorEmailID2
                    && B.AccountType == bankModel.AccountType && B.VendorBankName == bankModel.BankName && B.VendorBranchName == bankModel.BranchName && B.Location==bankModel.City) != null;
                return IsExist;
            }
        }
        public string GetBankDetailId(string VendorCode, string CompCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                //var bankDetailId = (from b in suzlonBPPEntities.BankDetails
                //                    where b.VendorCode == VendorCode && b.CompanyCode == CompCode
                //                    select new { b.BankDetailId }).ToString();
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var bankDetail = suzlonBPPEntities.BankDetails.FirstOrDefault(bankdtl => bankdtl.VendorCode == VendorCode && bankdtl.CompanyCode == CompCode);
                if (bankDetail != null)
                    return Convert.ToString(bankDetail.BankDetailId);
                else
                    return string.Empty;
            }
        }

        public List<BankWorkflowModel> GetBankWorkflow(string subVerticalIds)
        {
            List<int> subVertical = subVerticalIds.Split(',').Select(Int32.Parse).ToList();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var workflowDetails = (from subvertical in suzlonBPPEntities.SubVerticalMasters
                                       join workflow in suzlonBPPEntities.BankWorkflows
                                       on subvertical.SubVerticalId equals workflow.SubVerticalId into sw
                                       from workflow in sw.DefaultIfEmpty()
                                       where subVertical.Contains(subvertical.SubVerticalId)
                                       select new BankWorkflowModel
                                       {
                                           subVerticalMaster = subvertical,
                                           bankWorkFlow = workflow
                                       }).ToList();
                return workflowDetails;
            }
        }

        /// <summary>
        /// This method is used to save/update bank workflow details.
        /// </summary>
        /// <param name="bankWorkFlow"></param>
        /// <returns></returns>
        public bool SaveBankWorkflow(BankWorkflow bankWorkFlow, int userId)
        {
            bool isSave = false;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                BankWorkflow workflow = suzlonBPPEntities.BankWorkflows.AsNoTracking().FirstOrDefault(w => w.SubVerticalId == bankWorkFlow.SubVerticalId);
                if (workflow == null)
                {
                    bankWorkFlow.CreatedBy = userId;
                    bankWorkFlow.CreatedOn = DateTime.Now;
                    bankWorkFlow.ModifiedBy = userId;
                    bankWorkFlow.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.BankWorkflows.Add(bankWorkFlow);
                }
                else
                {
                    bankWorkFlow.BankWorkFlowId = workflow.BankWorkFlowId;
                    bankWorkFlow.CreatedBy = workflow.CreatedBy;
                    bankWorkFlow.CreatedOn = workflow.CreatedOn;
                    bankWorkFlow.ModifiedBy = userId;
                    bankWorkFlow.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(bankWorkFlow).State = EntityState.Modified;
                }
                suzlonBPPEntities.SaveChanges();
                SecUserNotification SecUserNotification = new SecUserNotification();
                NotificationModel notificationModel = new NotificationModel();
                string subVerticalName = string.Empty;
                string workFlowName = "Payment";
                var subVertical = suzlonBPPEntities.SubVerticalMasters.FirstOrDefault(u => u.SubVerticalId == bankWorkFlow.SubVerticalId);
                if (subVertical != null)
                    subVerticalName = subVertical.Name;
                SecUserNotification.SubVerticalName = subVerticalName;
                if ((workflow == null && bankWorkFlow.SecVerContUserId.HasValue) ||
                    (workflow != null && ((workflow.SecVerContUserId == null && bankWorkFlow.SecVerContUserId.HasValue) ||
                    (workflow.SecVerContUserId.HasValue && bankWorkFlow.SecVerContUserId.HasValue && workflow.SecVerContUserId.Value != bankWorkFlow.SecVerContUserId.Value))))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.FromDate = (bankWorkFlow.SecVerContFromDt.HasValue ? bankWorkFlow.SecVerContFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (bankWorkFlow.SecVerContToDt.HasValue ? bankWorkFlow.SecVerContToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = bankWorkFlow.SecVerContUserId.Value;
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriVerContUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecVerContUserId.HasValue && bankWorkFlow.SecVerContUserId.HasValue && workflow.SecVerContUserId.Value != bankWorkFlow.SecVerContUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecVerContUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecVerContUserId.HasValue && bankWorkFlow.SecVerContUserId == null)))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriVerContUserId;
                    SecUserNotification.AssignedUserId = workflow.SecVerContUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((workflow == null && bankWorkFlow.SecGrpContUserId.HasValue) ||
                    (workflow != null && ((workflow.SecGrpContUserId == null && bankWorkFlow.SecGrpContUserId.HasValue) ||
                    (workflow.SecGrpContUserId.HasValue && bankWorkFlow.SecGrpContUserId.HasValue && workflow.SecGrpContUserId.Value != bankWorkFlow.SecGrpContUserId.Value))))
                {
                    SecUserNotification.StageName = "Group Controller";
                    SecUserNotification.FromDate = (bankWorkFlow.SecGrpContFromDt.HasValue ? bankWorkFlow.SecGrpContFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (bankWorkFlow.SecGrpContToDt.HasValue ? bankWorkFlow.SecGrpContToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = bankWorkFlow.SecGrpContUserId.Value;
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriGrpContUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecGrpContUserId.HasValue && bankWorkFlow.SecGrpContUserId.HasValue && workflow.SecGrpContUserId.Value != bankWorkFlow.SecGrpContUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecGrpContUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecGrpContUserId.HasValue && bankWorkFlow.SecGrpContUserId == null)))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriGrpContUserId;
                    SecUserNotification.AssignedUserId = workflow.SecGrpContUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((workflow == null && bankWorkFlow.SecTreasuryUserId.HasValue) ||
                    (workflow != null && ((workflow.SecTreasuryUserId == null && bankWorkFlow.SecTreasuryUserId.HasValue) ||
                    (workflow.SecTreasuryUserId.HasValue && bankWorkFlow.SecTreasuryUserId.HasValue && workflow.SecTreasuryUserId.Value != bankWorkFlow.SecTreasuryUserId.Value))))
                {
                    SecUserNotification.StageName = "Treasury";
                    SecUserNotification.FromDate = (bankWorkFlow.SecTreasuryFromDt.HasValue ? bankWorkFlow.SecTreasuryFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (bankWorkFlow.SecTreasuryToDt.HasValue ? bankWorkFlow.SecTreasuryToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = bankWorkFlow.SecTreasuryUserId.Value;
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriTreasuryUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecTreasuryUserId.HasValue && bankWorkFlow.SecTreasuryUserId.HasValue && workflow.SecTreasuryUserId.Value != bankWorkFlow.SecTreasuryUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecTreasuryUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecTreasuryUserId.HasValue && bankWorkFlow.SecTreasuryUserId == null)))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriTreasuryUserId;
                    SecUserNotification.AssignedUserId = workflow.SecTreasuryUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((workflow == null && bankWorkFlow.SecMgmtAssFromDt.HasValue) ||
                    (workflow != null && ((workflow.SecMgmtAssUserId == null && bankWorkFlow.SecMgmtAssUserId.HasValue) ||
                    (workflow.SecMgmtAssUserId.HasValue && bankWorkFlow.SecMgmtAssUserId.HasValue && workflow.SecMgmtAssUserId.Value != bankWorkFlow.SecMgmtAssUserId.Value))))
                {
                    SecUserNotification.StageName = "Management Assurance";
                    SecUserNotification.FromDate = (bankWorkFlow.SecMgmtAssFromDt.HasValue ? bankWorkFlow.SecMgmtAssFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (bankWorkFlow.SecMgmtAssToDt.HasValue ? bankWorkFlow.SecMgmtAssToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = bankWorkFlow.SecMgmtAssUserId.Value;
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriMgmtAssUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecMgmtAssUserId.HasValue && bankWorkFlow.SecMgmtAssUserId.HasValue && workflow.SecMgmtAssUserId.Value != bankWorkFlow.SecMgmtAssUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecMgmtAssUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecMgmtAssUserId.HasValue && bankWorkFlow.SecMgmtAssUserId == null)))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriMgmtAssUserId;
                    SecUserNotification.AssignedUserId = workflow.SecMgmtAssUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((workflow == null && bankWorkFlow.SecFASSCUserId.HasValue) ||
                    (workflow != null && ((workflow.SecFASSCUserId == null && bankWorkFlow.SecFASSCUserId.HasValue) ||
                    (workflow.SecFASSCUserId.HasValue && bankWorkFlow.SecFASSCUserId.HasValue && workflow.SecFASSCUserId.Value != bankWorkFlow.SecFASSCUserId.Value))))
                {
                    SecUserNotification.StageName = "F&A SSC";
                    SecUserNotification.FromDate = (bankWorkFlow.SecFASSCFromDt.HasValue ? bankWorkFlow.SecFASSCFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (bankWorkFlow.SecFASSCToDt.HasValue ? bankWorkFlow.SecFASSCToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = bankWorkFlow.SecFASSCUserId.Value;
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriFASSCUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecFASSCUserId.HasValue && bankWorkFlow.SecFASSCUserId.HasValue && workflow.SecFASSCUserId.Value != bankWorkFlow.SecFASSCUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecFASSCUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecFASSCUserId.HasValue && bankWorkFlow.SecFASSCUserId == null)))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriFASSCUserId;
                    SecUserNotification.AssignedUserId = workflow.SecFASSCUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((workflow == null && bankWorkFlow.SecCBUserId.HasValue) ||
                    (workflow != null && ((workflow.SecCBUserId == null && bankWorkFlow.SecCBUserId.HasValue) ||
                    (workflow.SecCBUserId.HasValue && bankWorkFlow.SecCBUserId.HasValue && workflow.SecCBUserId.Value != bankWorkFlow.SecCBUserId.Value))))
                {
                    SecUserNotification.StageName = "C&B";
                    SecUserNotification.FromDate = (bankWorkFlow.SecCBFromDt.HasValue ? bankWorkFlow.SecCBFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (bankWorkFlow.SecCBToDt.HasValue ? bankWorkFlow.SecCBToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = bankWorkFlow.SecCBUserId.Value;
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriCBUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecCBUserId.HasValue && bankWorkFlow.SecCBUserId.HasValue && workflow.SecCBUserId.Value != bankWorkFlow.SecCBUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecCBUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecCBUserId.HasValue && bankWorkFlow.SecCBUserId == null)))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.PriActorUserId = bankWorkFlow.PriCBUserId;
                    SecUserNotification.AssignedUserId = workflow.SecCBUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                isSave = true;
            }
            return isSave;
        }

        public List<GetVendorCodeForWorkflow_Result> GetVendorCodeForWorkflow(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetVendorCodeForWorkflow(userId).ToList();
            }
        }

        public List<GetVendorDetailBasedOnSearch_Result> GetVendorDetailBasedOnSearch(string searchText, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetVendorDetailBasedOnSearch(searchText, userId).ToList();
            }
        }

        public List<ListItem> GetCompanyCodeByVendorCode(string vendorCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<ListItem> lstCompanyCode = new List<ListItem>();
                //lstCompanyCode.Add(new ListItem() { Id = "", Name = "Select Company Code" });
                suzlonBPPEntities.VendorMasters.Where(v => v.VendorCode == vendorCode).AsNoTracking()
                    .ToList().ForEach(v => { lstCompanyCode.Add(new ListItem() { Id = v.CompanyCode, Name = v.CompanyCode + "-" + v.CompanyName }); });
                return lstCompanyCode;
            }
        }

        public ListItem GetVendorDetailsForWorkflow(string vendorCode, string companyCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.VendorMasters.Where(v => v.VendorCode == vendorCode && v.CompanyCode == companyCode).Select(v => new ListItem() { Id = v.VendorName, Name = v.PanNo }).FirstOrDefault();
            }
        }

        /// <summary>
        /// This function used To Add Bank Details.
        /// </summary>
        /// <param name="bankDetailModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string AddBankDetail(BankDetailModel bankDetailModel, int userId)
        {
            BankDetail bankDetails = new BankDetail();
            bankDetails.VendorCode = bankDetailModel.VendorCode;
            bankDetails.CompanyCode = bankDetailModel.CompanyCode;
            bankDetails.VendorName = bankDetailModel.VendorName;
            bankDetails.VendorPanNo = bankDetailModel.VendorPanNo;
            bankDetails.AccountType = bankDetailModel.AccountType;
            bankDetails.AccountNumber = bankDetailModel.AccountNumber;
            bankDetails.IFSCCode = bankDetailModel.IFSCCode;
            bankDetails.BankName = bankDetailModel.BankName;
            bankDetails.BranchName = bankDetailModel.BranchName;
            bankDetails.City = bankDetailModel.City;
            bankDetails.SuzlonEmailID1 = bankDetailModel.SuzlonEmailID1;
            bankDetails.SuzlonEmailID2 = bankDetailModel.SuzlonEmailID2;
            bankDetails.VendorEmailID1 = bankDetailModel.VendorEmailID1;
            bankDetails.VendorEmailID2 = bankDetailModel.VendorEmailID2;
            bankDetails.WorkFlowStatusId = bankDetailModel.WorkFlowStatusId;
            bankDetails.OriginalDocumentsSent = bankDetailModel.OriginalDocumentsSent;
            bankDetails.SendDate = bankDetailModel.SendDate;
            bankDetails.OriginalDocumentsReceived = bankDetailModel.OriginalDocumentsReceived;
            bankDetails.ReceivedDate = bankDetailModel.ReceivedDate;
            bankDetails.VerticalId = bankDetailModel.VerticalId;
            bankDetails.SubVerticalId = bankDetailModel.SubVerticalId;
            bankDetails.Attachment1 = bankDetailModel.Attachment1;
            bankDetails.Attachment2 = bankDetailModel.Attachment2;
            // bankDetails.IsNew = true;
            bankDetails.CreatedBy = userId;
            bankDetails.CreatedOn = DateTime.Now;
            bankDetails.ModifiedBy = userId;
            bankDetails.ModifiedOn = DateTime.Now;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                bool isexist = suzlonBPPEntities.VendorBankMasters.FirstOrDefault(v => v.VendorCode.ToLower() == bankDetails.VendorCode.ToLower() && v.CompanyCode.ToLower() == bankDetails.CompanyCode.ToLower()) == null;
                bankDetails.IsNew = isexist;
                suzlonBPPEntities.BankDetails.Add(bankDetails);
                suzlonBPPEntities.SaveChanges();
                bankDetailModel.BankDetailId = bankDetails.BankDetailId;
                if (bankDetailModel.BankComments != null && bankDetailModel.BankComments.Count > 0)
                {
                    bankDetailModel.BankComments.ForEach(comment =>
                    {
                        BankComment bankComment = new BankComment();
                        bankComment.BankDetailId = bankDetails.BankDetailId;
                        bankComment.Comment = comment.Comment;
                        bankComment.BankWorkflowStatusId = bankDetails.WorkFlowStatusId;
                        bankComment.CreatedBy = userId;
                        bankComment.CreatedOn = DateTime.Now;
                        bankComment.ModifiedBy = userId;
                        bankComment.ModifiedOn = DateTime.Now;
                        suzlonBPPEntities.BankComments.Add(bankComment);
                    });
                    suzlonBPPEntities.SaveChanges();
                }
            }
            
            if (bankDetails.WorkFlowStatusId.HasValue && bankDetails.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByValidator)
                SendApprovedNotification(bankDetailModel, userId);
            else
                SendInitiationNotification(true, bankDetailModel);
            return Constants.SUCCESS;
        }

        public string UpdateBankDetail(BankDetailModel bankDetailModel, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                BankDetail bankDetail = suzlonBPPEntities.BankDetails.FirstOrDefault(u => u.BankDetailId == bankDetailModel.BankDetailId);
                int? previousStatus = bankDetail.WorkFlowStatusId;
                bool prevoiusIsNew = bankDetail.IsNew;
                if (previousStatus == (int)WorkFlowStatusEnum.ApprovedByCB)
                {
                    UpdateBankComments(bankDetail.BankDetailId);
                    UpdateBankAuditDetail(bankDetail.BankDetailId);
                }

                if (bankDetail != null)
                {
                    bankDetail.VendorCode = bankDetailModel.VendorCode;
                    bankDetail.CompanyCode = bankDetailModel.CompanyCode;
                    bankDetail.VendorName = bankDetailModel.VendorName;
                    bankDetail.VendorPanNo = bankDetailModel.VendorPanNo;
                    bankDetail.AccountType = bankDetailModel.AccountType;
                    bankDetail.AccountNumber = bankDetailModel.AccountNumber;
                    bankDetail.IFSCCode = bankDetailModel.IFSCCode;
                    bankDetail.BankName = bankDetailModel.BankName;
                    bankDetail.BranchName = bankDetailModel.BranchName;
                    bankDetail.City = bankDetailModel.City;
                    bankDetail.SuzlonEmailID1 = bankDetailModel.SuzlonEmailID1;
                    bankDetail.SuzlonEmailID2 = bankDetailModel.SuzlonEmailID2;
                    bankDetail.VendorEmailID1 = bankDetailModel.VendorEmailID1;
                    bankDetail.VendorEmailID2 = bankDetailModel.VendorEmailID2;
                    bankDetail.WorkFlowStatusId = bankDetailModel.WorkFlowStatusId;
                    bankDetail.OriginalDocumentsSent = bankDetailModel.OriginalDocumentsSent;
                    bankDetail.SendDate = bankDetailModel.SendDate;
                    bankDetail.OriginalDocumentsReceived = bankDetailModel.OriginalDocumentsReceived;
                    bankDetail.ReceivedDate = bankDetailModel.ReceivedDate;
                    bankDetail.VerticalId = bankDetailModel.VerticalId;
                    bankDetail.SubVerticalId = bankDetailModel.SubVerticalId;
                    if (!string.IsNullOrEmpty(bankDetailModel.Attachment1))
                        bankDetail.Attachment1 = bankDetailModel.Attachment1;
                    if (!string.IsNullOrEmpty(bankDetailModel.Attachment2))
                        bankDetail.Attachment2 = bankDetailModel.Attachment2;
                    bankDetail.ModifiedBy = userId;
                    bankDetail.ModifiedOn = DateTime.Now;

                    if (bankDetailModel.RequestType == "New")
                        bankDetail.CreatedBy = userId;

                    if (bankDetailModel.AssignedTo != null && bankDetailModel.WorkFlowStatusId == (int)Status.NeedCorrection)
                        bankDetail.WorkFlowStatusId = SetStatusByAssignedTo(bankDetailModel.AssignedTo);
                    if (bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByCB)
                        bankDetail.IsNew = false;
                    suzlonBPPEntities.SaveChanges();
                    if (bankDetailModel.BankComments != null && bankDetailModel.BankComments.Count > 0)
                    {
                        bankDetailModel.BankComments.ForEach(comment =>
                        {
                            BankComment bankComment = new BankComment();
                            bankComment.BankDetailId = bankDetailModel.BankDetailId;
                            bankComment.Comment = comment.Comment;
                            bankComment.BankWorkflowStatusId = bankDetail.WorkFlowStatusId;
                            bankComment.CreatedBy = userId;
                            bankComment.CreatedOn = DateTime.Now;
                            bankComment.ModifiedBy = userId;
                            bankComment.ModifiedOn = DateTime.Now;
                            suzlonBPPEntities.BankComments.Add(bankComment);
                        });
                        suzlonBPPEntities.SaveChanges();
                    }

                    var userDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId);

                    if (userDetail.ProfileId == (int)UserProfileEnum.Vendor)
                        SendInitiationNotification(false, bankDetailModel);
                    else if (bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByCB)
                    {
                        //code to push entry in history
                        int bankDetailHistoryId = AddToBankHistory(suzlonBPPEntities, bankDetail, userId);
                        string result = SAPUpdateForBankApproval(bankDetail, bankDetailHistoryId);
                        if (result != Constants.SAP_SUCCESS || result == Constants.SAP_CONNECTION_FAILURE)
                        {
                            bankDetail.WorkFlowStatusId = previousStatus;
                            bankDetail.IsNew = prevoiusIsNew;
                            BankDetailHistory BankDetailHistory = suzlonBPPEntities.BankDetailHistories.FirstOrDefault(bh => bh.BankDetailHistoryId == bankDetailHistoryId);
                            if (BankDetailHistory != null)
                                BankDetailHistory.IsRecordPushToSAP = false;
                            suzlonBPPEntities.SaveChanges();
                            return result;
                        }
                        //UpdateBankComments(bankDetail.BankDetailId);
                        //UpdateBankAuditDetail(bankDetail.BankDetailId);

                        SendApprovedNotificationToVendor(bankDetailModel, userId);
                        SendApprovedNotificationToValidator(bankDetailModel, userId);

                    }
                    else if (bankDetailModel.Status != null)
                    {
                        if (bankDetailModel.Status == (int)Status.Approved)
                            SendApprovedNotification(bankDetailModel, userId);
                        else if (bankDetailModel.Status == (int)Status.Rejected)
                            SendRejectedNotification(bankDetailModel, userId);
                        else if (bankDetailModel.Status == (int)Status.NeedCorrection)
                        {
                            if (bankDetailModel.AssignedTo == (int)UserProfileEnum.Vendor)
                                SendNeedCorrNotificationToVendor(bankDetailModel, userId);
                            else
                                SendNeedCorrNotification(bankDetailModel, userId);
                        }
                    }
                    else if (previousStatus == (int)WorkFlowStatusEnum.ApprovedByCB && bankDetailModel.WorkFlowStatusId.HasValue && bankDetailModel.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByValidator)
                        SendApprovedNotification(bankDetailModel, userId);
                    return Constants.SUCCESS;
                }
                return Constants.ERROR;
            }
        }

        public string UpdateBankPendingApprovalDetails(BankPendingApprovalDetail bankDetailModel, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                BankDetail bankDetail = suzlonBPPEntities.BankDetails.FirstOrDefault(u => u.BankDetailId == bankDetailModel.BankDetailId);
                int? previousStatus = bankDetail.WorkFlowStatusId;
                bool prevoiusIsNew = bankDetail.IsNew;
                if (bankDetail != null)
                {
                    string WorkFlowStatusText = suzlonBPPEntities.BankWorkflowStatus.AsNoTracking().FirstOrDefault(ws => ws.BankWorkflowStausId == bankDetail.WorkFlowStatusId).Status;

                    int? setStatus = Convert.ToInt32(bankDetail.WorkFlowStatusId);

                    if (bankDetailModel.Status == (int)Status.Approved)
                    {
                        if (bankDetail.IsNew && (bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByVerticalController || bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByTreasury))
                        {
                            setStatus = setStatus + 6; // Set Approved By Treasury
                            if (bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByTreasury)
                                setStatus = setStatus + 3;// Set Approved By C&B
                        }
                        else
                        {
                            if (WorkFlowStatusText.Contains("Need Correction"))
                                setStatus = setStatus - 2;
                            else
                                setStatus = setStatus + 3;
                        }
                    }
                    if (bankDetailModel.Status == (int)Status.Rejected)
                    {
                        if (bankDetail.IsNew && (bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByVerticalController || bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByTreasury))
                        {
                            setStatus = setStatus + 7;// Set Rejected by Treasury
                            if (bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByTreasury)
                                setStatus = setStatus + 3;// Set Rejected by C&B
                        }
                        else
                        {
                            if (WorkFlowStatusText.Contains("Need Correction"))
                                setStatus = setStatus - 1;
                            else
                                setStatus = setStatus + 4;
                        }
                    }

                    if (bankDetailModel.Status == (int)Status.NeedCorrection)
                        setStatus = SetStatusByAssignedTo(bankDetailModel.AssignedTo);

                    bankDetail.WorkFlowStatusId = setStatus;
                    if (setStatus == (int)WorkFlowStatusEnum.ApprovedByCB)
                    {
                        bankDetail.IsNew = false;
                    }
                    bankDetail.ModifiedBy = userId;
                    bankDetail.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(bankDetailModel.Comment))
                    {
                        BankComment bankComment = new BankComment();
                        bankComment.BankDetailId = bankDetailModel.BankDetailId;
                        bankComment.Comment = bankDetailModel.Comment;
                        bankComment.BankWorkflowStatusId = bankDetail.WorkFlowStatusId;
                        bankComment.CreatedBy = userId;
                        bankComment.CreatedOn = DateTime.Now;
                        bankComment.ModifiedBy = userId;
                        bankComment.ModifiedOn = DateTime.Now;
                        suzlonBPPEntities.BankComments.Add(bankComment);
                    }
                    suzlonBPPEntities.SaveChanges();
                    BankDetailModel bankDetailModelDetail = new BankDetailModel();
                    bankDetailModelDetail.BankDetailId = bankDetail.BankDetailId;
                    bankDetailModelDetail.VendorName = bankDetail.VendorName;
                    //santosh 19-09-16
                    bankDetailModelDetail.VendorCode = bankDetail.VendorCode; //santosh 
                    bankDetailModelDetail.SuzlonEmailID1 = bankDetail.SuzlonEmailID1;
                    bankDetailModelDetail.SuzlonEmailID2 = bankDetail.SuzlonEmailID2;
                    bankDetailModelDetail.VendorEmailID1 = bankDetail.VendorEmailID1;
                    bankDetailModelDetail.VendorEmailID2 = bankDetail.VendorEmailID2;
                    bankDetailModelDetail.BankComments = new List<BankCommentModel>();
                    bankDetailModelDetail.BankComments.Add(new BankCommentModel() { Comment = bankDetailModel.Comment });
                    if (bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByCB)
                    {
                        int bankDetailHistoryId = AddToBankHistory(suzlonBPPEntities, bankDetail, userId);
                        string result = SAPUpdateForBankApproval(bankDetail, bankDetailHistoryId);
                        if (result != Constants.SAP_SUCCESS || result == Constants.SAP_CONNECTION_FAILURE)
                        {
                            bankDetail.WorkFlowStatusId = previousStatus;
                            bankDetail.IsNew = prevoiusIsNew;
                            BankDetailHistory BankDetailHistory = suzlonBPPEntities.BankDetailHistories.FirstOrDefault(bh => bh.BankDetailHistoryId == bankDetailHistoryId);
                            if (BankDetailHistory != null)
                                BankDetailHistory.IsRecordPushToSAP = false;
                            suzlonBPPEntities.SaveChanges();
                            return result;
                        }

                        // UpdateBankComments(bankDetail.BankDetailId); //commented by santosh 1 Aug 16 
                        // UpdateBankAuditDetail(bankDetail.BankDetailId);

                        SendApprovedNotificationToVendor(bankDetailModelDetail, userId);
                        SendApprovedNotificationToValidator(bankDetailModelDetail, userId);
                    }
                    else if (bankDetail.WorkFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor)
                        SendNeedCorrNotificationToVendor(bankDetailModelDetail, userId);
                    else
                    {
                        if (bankDetailModel.Status == (int)Status.Approved)
                            SendApprovedNotification(bankDetailModelDetail, userId);
                        else if (bankDetailModel.Status == (int)Status.Rejected)
                            SendRejectedNotification(bankDetailModelDetail, userId);
                        if (bankDetailModel.Status == (int)Status.NeedCorrection)
                            SendNeedCorrNotification(bankDetailModelDetail, userId);
                    }
                    return Constants.SUCCESS;
                }
                else
                    return Constants.ERROR;
            }
        }

        private string SAPUpdateForBankApproval(BankDetail bankDetail, int requestNo)
        {
            bool IsRestrictToSAP = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRestrictToSAP"]);

            if (IsRestrictToSAP)
                return Constants.SAP_SUCCESS;

            SAPHelper objSAP = new SAPHelper();
            using (SAPConnection connection = objSAP.GetSAPConnection())
            {
                CashAndBank_SAP.Vendor obj = new CashAndBank_SAP.Vendor();
                obj.Connection = connection;
                try
                {
                    obj.Connection.Open();
                }
                catch (Exception ex)
                {
                    return Constants.SAP_CONNECTION_FAILURE;
                }


                CashAndBank_SAP.YFITVENNEFT_HDFC updateDetail = new CashAndBank_SAP.YFITVENNEFT_HDFC();
                updateDetail.Bukrs = bankDetail.CompanyCode.Trim();
                updateDetail.Lifnr = bankDetail.VendorCode.Trim();
                updateDetail.Name1 = bankDetail.VendorName.Trim();
                updateDetail.Vbank = bankDetail.AccountNumber.Trim();
                updateDetail.Email1 = bankDetail.SuzlonEmailID1.Trim();
                updateDetail.Email2 = bankDetail.SuzlonEmailID2.Trim();
                updateDetail.Email3 = bankDetail.VendorEmailID1.Trim();
                updateDetail.Email4 = bankDetail.VendorEmailID2.Trim();
                updateDetail.Ifsc_Code = bankDetail.IFSCCode.Trim();
                updateDetail.Ifsc_Code_Rtgs = bankDetail.IFSCCode.Trim();
                updateDetail.Bank_Name = bankDetail.BankName.Trim();
                updateDetail.Bank_Branch = bankDetail.BranchName.Trim();
                updateDetail.S_Date = string.Empty.Trim();
                updateDetail.S_Time = string.Empty.Trim();
                updateDetail.Uname = string.Empty.Trim();
                updateDetail.Pay_Loc = Convert.ToString(requestNo).Trim();
                updateDetail.Print_Loc = string.Empty.Trim();
                updateDetail.Bene_Acc = string.Empty.Trim();
                updateDetail.Bene_Type = string.Empty.Trim();
                updateDetail.Check1 = string.Empty.Trim();
                updateDetail.Flag = "1".Trim();
                updateDetail.Mandt = string.Empty.Trim();
                updateDetail.Bank_Loc = bankDetail.City.Trim();

                if (ConfigurationManager.AppSettings["TraceLog"].ToString().ToLower() == "true")
                {
                    string strupdateDetail = "Date" + DateTime.Now.ToString() + Environment.NewLine.Trim();
                    strupdateDetail = strupdateDetail + "Bukrs=" + updateDetail.Bukrs.Trim();
                    strupdateDetail = strupdateDetail + ";Lifnr=" + updateDetail.Lifnr.Trim();
                    strupdateDetail = strupdateDetail + ";Name1=" + updateDetail.Name1.Trim();
                    strupdateDetail = strupdateDetail + ";Vbank=" + updateDetail.Vbank.Trim();
                    strupdateDetail = strupdateDetail + ";Email1=" + updateDetail.Email1.Trim();
                    strupdateDetail = strupdateDetail + ";Email2=" + updateDetail.Email2.Trim();
                    strupdateDetail = strupdateDetail + ";Email3=" + updateDetail.Email3.Trim();
                    strupdateDetail = strupdateDetail + ";Email4=" + updateDetail.Email4.Trim();
                    strupdateDetail = strupdateDetail + ";Ifsc_Code=" + updateDetail.Ifsc_Code.Trim();
                    strupdateDetail = strupdateDetail + ";Ifsc_Code_Rtgs=" + updateDetail.Ifsc_Code_Rtgs.Trim();
                    strupdateDetail = strupdateDetail + ";Bank_Name=" + updateDetail.Bank_Name.Trim();
                    strupdateDetail = strupdateDetail + ";Bank_Branch=" + updateDetail.Bank_Branch.Trim();
                    strupdateDetail = strupdateDetail + ";S_Date=" + updateDetail.S_Date.Trim();
                    strupdateDetail = strupdateDetail + ";S_Time=" + updateDetail.S_Time.Trim();
                    strupdateDetail = strupdateDetail + ";Uname=" + updateDetail.Uname.Trim();
                    strupdateDetail = strupdateDetail + ";Pay_Loc=" + updateDetail.Pay_Loc.Trim();
                    strupdateDetail = strupdateDetail + ";Print_Loc=" + updateDetail.Print_Loc.Trim();
                    strupdateDetail = strupdateDetail + ";Bene_Acc=" + updateDetail.Bene_Acc.Trim();
                    strupdateDetail = strupdateDetail + ";Bene_Type=" + updateDetail.Bene_Type.Trim();
                    strupdateDetail = strupdateDetail + ";Check1=" + updateDetail.Check1.Trim();
                    strupdateDetail = strupdateDetail + ";Flag=" + updateDetail.Flag.Trim();
                    strupdateDetail = strupdateDetail + ";Mandt=" + updateDetail.Mandt.Trim();
                    strupdateDetail = strupdateDetail + ";Bank_Loc=" + updateDetail.Bank_Loc.Trim();
                    strupdateDetail = strupdateDetail + Environment.NewLine;

                    StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
                    sw.WriteLine(strupdateDetail);
                    sw.Close();
                }

                CashAndBank_SAP.YFITVENNEFT_HDFCTable bapiTable = new CashAndBank_SAP.YFITVENNEFT_HDFCTable();
                bapiTable.Add(updateDetail);
                CashAndBank_SAP.BAPIRETURNTable bapiRTable = new CashAndBank_SAP.BAPIRETURNTable();
                obj.Yfif_Vendor_Bank_Data_Update(ref bapiTable, ref bapiRTable);
                System.Data.DataTable bapiResult = bapiRTable.ToADODataTable();
                if (bapiResult != null && bapiResult.Rows.Count > 0)
                {
                    string result = Convert.ToString(bapiResult.Rows[0]["Type"]);
                    if (result.ToUpper() != Constants.SAP_SUCCESS)
                    {
                        return Convert.ToString(bapiResult.Rows[0]["Type"]);
                    }
                    else
                        return result.ToUpper();
                }
                else
                    return Constants.SAP_ERROR;
            }
        }

        private void UpdateBankComments(int bankDetailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.BankComments.Where(c => c.BankDetailId == bankDetailId).ToList().ForEach(bc =>
                {
                    bc.IsDeleted = true;
                }
                );
                suzlonBPPEntities.SaveChanges();
            }
        }

        private void UpdateBankAuditDetail(int bankDetailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.BankDetailAudits.Where(A => A.BankDetailId == bankDetailId).ToList().ForEach(AD =>
                {
                    AD.IsDeleted = true;
                }
                );
                suzlonBPPEntities.SaveChanges();
            }

        }
        private int AddToBankHistory(SuzlonBPPEntities suzlonBPPEntities, BankDetail bankDetail, int userId)
        {
            BankDetailHistory bankHistory = new BankDetailHistory();
            bankHistory.BankDetailId = bankDetail.BankDetailId;
            bankHistory.AccountType = bankDetail.AccountType;
            bankHistory.AccountNumber = bankDetail.AccountNumber;
            bankHistory.IFSCCode = bankDetail.IFSCCode;
            bankHistory.BankName = bankDetail.BankName;
            bankHistory.BranchName = bankDetail.BranchName;
            bankHistory.City = bankDetail.City;
            bankHistory.SuzlonEmailID1 = bankDetail.SuzlonEmailID1;
            bankHistory.SuzlonEmailID2 = bankDetail.SuzlonEmailID2;
            bankHistory.VendorEmailID1 = bankDetail.VendorEmailID1;
            bankHistory.VendorEmailID2 = bankDetail.VendorEmailID2;
            bankHistory.CreatedBy = bankDetail.CreatedBy; //userId; 10 Aug
            bankHistory.CreatedOn = DateTime.Now;
            bankHistory.ModifiedBy = userId;
            bankHistory.ModifiedOn = DateTime.Now;

            bankHistory.VendorCode = bankDetail.VendorCode;
            bankHistory.CompanyCode = bankDetail.CompanyCode;
            bankHistory.VendorName = bankDetail.VendorName;
            bankHistory.VendorPanNo = bankDetail.VendorPanNo;
            bankHistory.Attachment1 = bankDetail.Attachment1;
            bankHistory.Attachment2 = bankDetail.Attachment2;
            bankHistory.SubVerticalId = bankDetail.SubVerticalId;
            bankHistory.VerticalId = bankDetail.VerticalId;
            bankHistory.OriginalDocumentsSent = bankDetail.OriginalDocumentsSent;
            bankHistory.SendDate = bankDetail.SendDate;
            bankHistory.OriginalDocumentsReceived = bankDetail.OriginalDocumentsReceived;
            bankHistory.ReceivedDate = bankDetail.ReceivedDate;
            bankHistory.IsRecordPushToSAP = true;

            suzlonBPPEntities.BankDetailHistories.Add(bankHistory);
            suzlonBPPEntities.SaveChanges();
            return bankHistory.BankDetailHistoryId;
        }

        public List<BankDetailModel> GetBankMyRecords(string vendorCode, string companyCode, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                User user = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId);

                bool isloggedUserVendor = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId).ProfileId == (int)UserProfileEnum.Vendor;
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var lstBankDetailModel = (from workflow in suzlonBPPEntities.BankDetails
                                          join accountType in suzlonBPPEntities.AccountTypes
                                          on workflow.AccountType equals accountType.Type
                                          join vendor in suzlonBPPEntities.VendorMasters
                                          on workflow.CompanyCode equals vendor.CompanyCode
                                          join status in suzlonBPPEntities.BankWorkflowStatus
                                          on workflow.WorkFlowStatusId equals status.BankWorkflowStausId into ws
                                          from s in ws.DefaultIfEmpty()
                                          where (string.IsNullOrEmpty(vendorCode) ? true : workflow.VendorCode == vendorCode)
                                            && (string.IsNullOrEmpty(companyCode) ? true : workflow.CompanyCode == companyCode)
                                            && workflow.VendorCode == vendor.VendorCode
                                            && (workflow.CreatedBy == userId || ((workflow.SuzlonEmailID1 == user.EmailId || workflow.SuzlonEmailID2 == user.EmailId) && workflow.WorkFlowStatusId != (int)WorkFlowStatusEnum.ApprovedByCB))
                                          //  && (isloggedUserVendor || workflow.WorkFlowStatusId != (int)WorkFlowStatusEnum.ApprovedByCB) commented by santosh as discussed with rashmi 16 Aug 16
                                          select new BankDetailModel()
                                          {
                                              BankDetailId = workflow.BankDetailId,
                                              VendorName = workflow.VendorName,
                                              CompanyCode = workflow.CompanyCode,
                                              CompanyName = vendor.CompanyName,
                                              BankName = workflow.BankName,
                                              AccountNumber = workflow.AccountNumber,
                                              AccountType = accountType.Description,
                                              IFSCCode = workflow.IFSCCode,
                                              BranchName = workflow.BranchName,
                                              City = workflow.City,
                                              Attachment1 = workflow.Attachment1,
                                              Attachment2 = workflow.Attachment2,
                                              WorkFlowStatusId = workflow.WorkFlowStatusId,
                                              WorkFlowStatus = (s != null ? s.Status : string.Empty),
                                              IsNew = workflow.IsNew,
                                              Modifidate = workflow.ModifiedOn
                                          }).ToList();
                if (isloggedUserVendor)
                    lstBankDetailModel.ForEach(w => w.WorkFlowStatus = GetWorkflowStatusForVendor(w.WorkFlowStatusId));
                else
                    lstBankDetailModel.ForEach(w => w.WorkFlowStatus = GetWorkflowStatusForEmployee(w.WorkFlowStatusId, w.IsNew, w.WorkFlowStatus));

                return lstBankDetailModel.OrderByDescending(w => w.Modifidate).ToList();
            }
        }

        public List<GetBankPendingRecords_Result> GetBankPendingRecords(string vendorCode, string companyCode, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lstBankDetailModel = suzlonBPPEntities.GetBankPendingRecords((string.IsNullOrEmpty(vendorCode) ? "0" : vendorCode), (string.IsNullOrEmpty(companyCode) ? "0" : companyCode), userId).ToList();
                return lstBankDetailModel;
            }
        }

        public List<usp_Get_BankDoc_Pending_Record_Result> GetpendingForCBForDocRec(string vendorCode, string companyCode, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lstBankDetailModel = suzlonBPPEntities.usp_Get_BankDoc_Pending_Record((string.IsNullOrEmpty(vendorCode) ? "0" : vendorCode), (string.IsNullOrEmpty(companyCode) ? "0" : companyCode), userId).ToList();
                return lstBankDetailModel;
            }
        }
        public List<usp_GetInitiatorDocSentPending_Result> GetpendingForInitiatorDocSend(string vendorCode, string companyCode, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lstInitiatorDocSend = suzlonBPPEntities.GetInitiatorDocSentPending((string.IsNullOrEmpty(vendorCode) ? "0" : vendorCode), (string.IsNullOrEmpty(companyCode) ? "0" : companyCode), userId).ToList();
                return lstInitiatorDocSend;
            }
        }

        public IFSCCodeMaster GetBankBranchDtl(string IFSCCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.IFSCCodeMasters.FirstOrDefault(bankdtl => bankdtl.IFSCCode == IFSCCode);
            }
        }

        public IFSCCodeMaster GetBankDetailsByBranchBank(string BankName, string BrnachName)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.IFSCCodeMasters.FirstOrDefault(bankdtl => bankdtl.BankName.ToLower() == BankName.ToLower().Trim() && bankdtl.BranchName.ToLower() == BrnachName.ToLower().Trim());
            }
        }

        public Dictionary<string, bool> SuzlonMailExists(string suzlonmailid1, string suzlonmailid2)
        {
            Dictionary<string, bool> fieldStatus = new Dictionary<string, bool>();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                if (!string.IsNullOrEmpty(suzlonmailid1))
                    fieldStatus.Add("suzlonmailid1", (from u in suzlonBPPEntities.Users
                                                      join p in suzlonBPPEntities.ProfileMasters on u.ProfileId equals p.ProfileId
                                                      where p.ProfileName == "Validator" && u.EmailId == suzlonmailid1
                                                      select u.EmailId).AsNoTracking().ToList().Count > 0);

                if (!string.IsNullOrEmpty(suzlonmailid2))
                    fieldStatus.Add("suzlonmailid2", (from u in suzlonBPPEntities.Users
                                                      join p in suzlonBPPEntities.ProfileMasters on u.ProfileId equals p.ProfileId
                                                      where p.ProfileName == "Validator" && u.EmailId == suzlonmailid2
                                                      select u.EmailId).AsNoTracking().ToList().Count > 0);

                return fieldStatus;
            }
        }
        public bool Existvendorbankdetail(string vendorCode, string compCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                // return solarPMSEntities.LocationMasters.FirstOrDefault(l => l.LocationName.ToLower() == name.ToLower() && l.LocationId != locationId) != null;
                return suzlonBPPEntities.BankDetails.FirstOrDefault(v => v.VendorCode.ToLower() == vendorCode.ToLower() && v.CompanyCode.ToLower() == compCode.ToLower() && v.WorkFlowStatusId != 19) != null;

            }
        }

        public List<ListItem> GetBankAssignedToValues(int profileId)
        {
            List<ListItem> lstAssignedTo = new List<ListItem>();
            lstAssignedTo.Add(new ListItem() { Id = "", Name = "Select Assigned To" });

            lstAssignedTo.Add(new ListItem() { Id = "9", Name = "Vendor" });
            if (profileId != (int)UserProfileEnum.Validator)
            {
                lstAssignedTo.Add(new ListItem() { Id = "10", Name = "Validator" });
                if (profileId >= (int)UserProfileEnum.GroupController)
                    lstAssignedTo.Add(new ListItem() { Id = "2", Name = "Vertical Controller" });
                if (profileId >= (int)UserProfileEnum.Treasury)
                    lstAssignedTo.Add(new ListItem() { Id = "3", Name = "Group Controller" });
                if (profileId >= (int)UserProfileEnum.ManagementAssurance)
                    lstAssignedTo.Add(new ListItem() { Id = "4", Name = "Treasury" });
                if (profileId >= (int)UserProfileEnum.FASSC)
                    lstAssignedTo.Add(new ListItem() { Id = "5", Name = "Management Assurance" });
                if (profileId >= (int)UserProfileEnum.CB)
                    lstAssignedTo.Add(new ListItem() { Id = "6", Name = "F&A SSC" });
            }
            //if (profileId <= (int)UserProfileEnum.Payment)
            //    lstAssignedTo.Add(new ListItem() { Id = "7", Name = "CB" });
            return lstAssignedTo;
        }

        public List<GetBankComment_Result> GetBankDetailComment(int bankDetailId, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetBankComment(bankDetailId, userId).ToList();
            }
        }
        public DropdownValues GetVerticalBySubVertical(int subVerticalId)
        {
            DropdownValues ddValues = new DropdownValues();
            ddValues.Vertical = new List<ListItem>();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                //(from v in suzlonBPPEntities.VerticalMasters
                // where v.VerticalId == subVerticalId
                // select new ListItem()
                // {
                //     Id = v.VerticalId.ToString(),
                //     Name = v.Name
                // }).OrderBy(s => s.Name).ToList().ForEach(item =>
                // {
                //     ddValues.Vertical.Add(item);
                // });

                (from v in suzlonBPPEntities.VerticalMasters
                 join sv in suzlonBPPEntities.SubVerticalMasters
                 on v.VerticalId equals sv.VerticalId
                 where sv.SubVerticalId == subVerticalId
                 select new ListItem() { Id = v.VerticalId.ToString(), Name = v.Name }
                 ).OrderBy(s => s.Name).ToList().ForEach(item =>
                 {
                     ddValues.Vertical.Add(item);
                 });

                return ddValues;
            }
        }

        //public BankDetail GetVendorBankDetail1(int bankDetailId)
        //{
        //    using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
        //    {
        //        suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
        //        BankDetail bankdtl = suzlonBPPEntities.BankDetails.FirstOrDefault(b => b.BankDetailId == bankDetailId);
        //        return bankdtl;
        //    }
        //}
        public dynamic GetVendorBankDetail(int bankDetailId, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                bool isLoggedInUserVendor = false;
                var userDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId);
                if (userDetail != null && userDetail.ProfileId == (int)UserProfileEnum.Vendor)
                    isLoggedInUserVendor = true;

                var bankDetail = (from bank in suzlonBPPEntities.BankDetails
                                  where bank.BankDetailId == bankDetailId
                                  select new
                                  {
                                      bank.BankDetailId,
                                      bank.VendorCode,
                                      bank.CompanyCode,
                                      bank.VendorName,
                                      bank.VendorPanNo,
                                      bank.AccountType,
                                      bank.AccountNumber,
                                      bank.IFSCCode,
                                      bank.BankName,
                                      bank.BranchName,
                                      bank.City,
                                      bank.SuzlonEmailID1,
                                      bank.SuzlonEmailID2,
                                      bank.VendorEmailID1,
                                      bank.VendorEmailID2,
                                      bank.WorkFlowStatusId,
                                      bank.SubVerticalId,
                                      bank.VerticalId,
                                      bank.OriginalDocumentsSent,
                                      bank.SendDate,
                                      bank.OriginalDocumentsReceived,
                                      bank.ReceivedDate,
                                      bank.CreatedBy,
                                      bank.Attachment1,
                                      bank.Attachment2,
                                      bank.IsNew,
                                      bankComments = (from comment in suzlonBPPEntities.BankComments
                                                      join user in suzlonBPPEntities.Users on comment.CreatedBy equals user.UserId
                                                      where comment.BankDetailId == bankDetailId
                                                      && comment.IsDeleted == false
                                                      && (isLoggedInUserVendor ? (comment.CreatedBy == userId || comment.BankWorkflowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor) : true)
                                                      select new BankCommentModel { Comment = comment.Comment, CommentBy = user.Name }).ToList(),
                                      IsCeatedByVendor = (suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == bank.CreatedBy).ProfileId == 9 ? true : false)

                                  }).FirstOrDefault();
                return bankDetail;
            }

        }

        public List<getExistingVendorDetailByAccountNo> GetVendorDetailByIfscAcc(string vendorCode, string companyCode, string accountNo)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {

                return suzlonBPPEntities.getExistingVendorDetailByAccountNo(vendorCode, companyCode, accountNo).ToList();

                //suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                //var bankDetail = (from bank in suzlonBPPEntities.BankDetails
                //                  join comp in suzlonBPPEntities.VendorMasters
                //                  on bank.VendorCode equals comp.VendorCode
                //                  where bank.IFSCCode == IFSC_Code && bank.AccountNumber == AccountNo
                //                  && bank.CompanyCode == comp.CompanyCode
                //                  && bank.BankDetailId != BankDetailId
                //                  select new
                //                  {
                //                      bank.VendorCode,
                //                      bank.VendorName,
                //                      bank.CompanyCode,
                //                      comp.CompanyName,
                //                      bank.IFSCCode,
                //                      bank.AccountNumber
                //                  }).Distinct().ToList();
                //return bankDetail;
            }
        }

        public List<usp_GetBankDetailWorkFlowLog_Result> GetBankDetailWorkFlowLogs(int BankDetailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.usp_GetBankDetailWorkFlowLog(BankDetailId).ToList();
            }
        }
        public List<usp_GetBankWorkFlowApprovalLog_Result> GetBankApprovalWorkFlowLogs(int BankDetailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetBankWorkFlowApprovalLog(BankDetailId).ToList();
            }
        }

        public dynamic GetBankDetailHistory(int bankDetailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var bankdtl = suzlonBPPEntities.BankDetailHistories.OrderByDescending(b => b.CreatedOn).FirstOrDefault(h => h.BankDetailId == bankDetailId && h.IsRecordPushToSAP == true);
                if (bankdtl == null)
                {
                    var newbankdtl = suzlonBPPEntities.BankDetails.OrderByDescending(b => b.CreatedOn).FirstOrDefault(h => h.BankDetailId == bankDetailId);
                    BankDetail obj = GetVendorDetailsFromBankMasters(newbankdtl.VendorCode, newbankdtl.CompanyCode);
                    return obj;
                    // bankdtl = new BankDetailHistory();
                }

                return bankdtl;
            }
        }
        public BankDetail GetVendorDetailsFromBankMasters(string vendorCode, string compCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var bankdtl = suzlonBPPEntities.VendorBankMasters.OrderByDescending(b => b.CreatedDate).FirstOrDefault(B => B.VendorCode == vendorCode && B.CompanyCode == compCode);

                if (bankdtl != null)
                {
                    BankDetail obj = new BankDetail()
                    {
                        AccountType = bankdtl.AccountType,
                        AccountNumber = bankdtl.AccountNumber,
                        IFSCCode = bankdtl.IFSCCode,
                        BankName = bankdtl.VendorBankName,
                        BranchName = bankdtl.VendorBranchName,
                        City = bankdtl.Location,
                        SuzlonEmailID1 = bankdtl.SuzlonEmailID1,
                        SuzlonEmailID2 = bankdtl.SuzlonEmailID2,
                        VendorEmailID1 = bankdtl.VendorEmailID1,
                        VendorEmailID2 = bankdtl.VendorEmailID2
                    };
                    return obj;
                }
                else
                {
                    return new BankDetail();
                }

            }
        }



        public dynamic GetBankDetailHistoryCBDoc(int bankDetailHistoryId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var bankdtl = suzlonBPPEntities.BankDetailHistories.OrderByDescending(b => b.CreatedOn).FirstOrDefault(h => h.BankDetailHistoryId == bankDetailHistoryId);
                if (bankdtl == null)
                {
                    bankdtl = new BankDetailHistory();
                }

                return bankdtl;
            }
        }
        public List<BankDetailModel> GetBankValidatorData(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                string email = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId).EmailId;
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var lstBankDetailModel = (from workflow in suzlonBPPEntities.BankDetails
                                          join accountType in suzlonBPPEntities.AccountTypes
                                          on workflow.AccountType equals accountType.Type
                                          where (workflow.SuzlonEmailID1 == email || workflow.SuzlonEmailID2 == email)
                                            && (workflow.WorkFlowStatusId == null || workflow.WorkFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByValidator
                                            || (workflow.SubVerticalId == null && workflow.WorkFlowStatusId != (int)WorkFlowStatusEnum.NeedCorrectionByVendor))
                                          select new BankDetailModel()
                                          {
                                              BankDetailId = workflow.BankDetailId,
                                              VendorName = workflow.VendorName,
                                              VendorCode = workflow.VendorCode,
                                              CompanyCode = workflow.CompanyCode,
                                              BankName = workflow.BankName,
                                              SubVerticalId = workflow.SubVerticalId,
                                              AccountNumber = workflow.AccountNumber,
                                              IFSCCode = workflow.IFSCCode,
                                              BranchName = workflow.BranchName,
                                              City = workflow.City,
                                              Attachment1 = workflow.Attachment1,
                                              Attachment2 = workflow.Attachment2,
                                              WorkFlowStatusId = workflow.WorkFlowStatusId,
                                              AccountType=accountType.Description,
                                              Modifidate = workflow.ModifiedOn
                                          }).ToList();

                return lstBankDetailModel;
            }
        }
        public bool UpdateBankValidatorData(BankValidatorUpdate bankValidatorUpdate, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                BankDetail bankDetail = suzlonBPPEntities.BankDetails.FirstOrDefault(u => u.BankDetailId == bankValidatorUpdate.BankDetailId);
                if (bankDetail != null)
                {
                    if (bankValidatorUpdate.Status == (int)Status.NeedCorrection)
                        bankDetail.WorkFlowStatusId = (int)WorkFlowStatusEnum.NeedCorrectionByVendor;
                    else
                    {
                        if (bankValidatorUpdate.Status == (int)Status.Approved)
                        {
                            bankDetail.SubVerticalId = bankValidatorUpdate.SubVerticalId;
                            var subverticalRecord = suzlonBPPEntities.SubVerticalMasters.AsNoTracking().FirstOrDefault(v => v.SubVerticalId == bankValidatorUpdate.SubVerticalId);
                            if (subverticalRecord != null)
                                bankDetail.VerticalId = subverticalRecord.VerticalId;
                        }
                        bankDetail.WorkFlowStatusId = bankValidatorUpdate.Status;
                    }
                    bankDetail.ModifiedBy = userId;
                    bankDetail.ModifiedOn = DateTime.Now;
                    if (!string.IsNullOrEmpty(bankValidatorUpdate.Comment))
                    {
                        BankComment bankComment = new BankComment();
                        bankComment.BankDetailId = bankValidatorUpdate.BankDetailId;
                        bankComment.Comment = bankValidatorUpdate.Comment;
                        bankComment.BankWorkflowStatusId = bankDetail.WorkFlowStatusId;
                        bankComment.CreatedBy = userId;
                        bankComment.CreatedOn = DateTime.Now;
                        bankComment.ModifiedBy = userId;
                        bankComment.ModifiedOn = DateTime.Now;
                        suzlonBPPEntities.BankComments.Add(bankComment);
                    }
                    suzlonBPPEntities.SaveChanges();
                    BankDetailModel bankDetailModelDetail = new BankDetailModel();
                    bankDetailModelDetail.BankDetailId = bankDetail.BankDetailId;
                    bankDetailModelDetail.VendorName = bankDetail.VendorName;
                    bankDetailModelDetail.SuzlonEmailID1 = bankDetail.SuzlonEmailID1;
                    bankDetailModelDetail.SuzlonEmailID2 = bankDetail.SuzlonEmailID2;
                    bankDetailModelDetail.VendorEmailID1 = bankDetail.VendorEmailID1;
                    bankDetailModelDetail.VendorEmailID2 = bankDetail.VendorEmailID2;
                    bankDetailModelDetail.BankComments = new List<BankCommentModel>();
                    bankDetailModelDetail.BankComments.Add(new BankCommentModel() { Comment = bankValidatorUpdate.Comment });
                    if (bankValidatorUpdate.Status == (int)Status.Approved)
                        SendApprovedNotification(bankDetailModelDetail, userId);
                    else if (bankValidatorUpdate.Status == (int)Status.Rejected)
                        SendRejectedNotification(bankDetailModelDetail, userId);
                    if (bankValidatorUpdate.Status == (int)Status.NeedCorrection)
                        SendNeedCorrNotificationToVendor(bankDetailModelDetail, userId);
                    return true;
                }
                else
                    return false;
            }
        }
        private string GetWorkflowStatusForVendor(int? workFlowStatusId)
        {
            if (!workFlowStatusId.HasValue
                || workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByValidator
                || workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByValidator
                || workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByVerticalController
                || workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVerticalController
                || workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByGroupController
                || workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByGroupController
                || workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByTreasury
                || workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByTreasury
                || workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByManagementAssurance
                || workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByManagementAssurance
                || workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByFASSC
                || workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByFASSC
                || workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByCB)
                return "In Progress";
            if (workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByCB)
                return "Approved";
            if (workFlowStatusId == (int)WorkFlowStatusEnum.RejectedByValidator
                || workFlowStatusId == (int)WorkFlowStatusEnum.RejectedByVerticalController
                || workFlowStatusId == (int)WorkFlowStatusEnum.RejectedByGroupController
                || workFlowStatusId == (int)WorkFlowStatusEnum.RejectedByTreasury
                || workFlowStatusId == (int)WorkFlowStatusEnum.RejectedByManagementAssurance
                || workFlowStatusId == (int)WorkFlowStatusEnum.RejectedByFASSC
                || workFlowStatusId == (int)WorkFlowStatusEnum.RejectedByCB)
                return "Rejected";
            if (workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor)
                return "Need Correction";
            return string.Empty;
        }
        private string GetWorkflowStatusForEmployee(int? workFlowStatusId, bool isNew, string workFlowStatus)
        {
            if (workFlowStatusId.HasValue)
            {
                if (workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByValidator)
                    return "Pending for Vertical Controller";
                if (workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByVerticalController)
                    if (isNew)
                        return "Pending for Treasury";
                    else
                        return "Pending for Group Controller";
                if (workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByGroupController)
                    return "Pending for Treasury";

                if (workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByTreasury)
                    if (isNew)
                        return "Pending for C&B";
                    else
                        return "Pending for Management Assurance";
                if (workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByManagementAssurance)
                    return "Pending for FASSC";
                if (workFlowStatusId == (int)WorkFlowStatusEnum.ApprovedByFASSC)
                    return "Pending for C&B";
                if (workFlowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor)
                    return "Need Correction By Initiator";
            }
            else
                return "Pending for Validator";
            return workFlowStatus;
        }

        private int? SetStatusByAssignedTo(int? assignedTo)
        {
            switch (assignedTo)
            {
                case ((int)UserProfileEnum.Vendor):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByVendor;
                    }
                case ((int)UserProfileEnum.Validator):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByValidator;
                    }
                case ((int)UserProfileEnum.VerticalController):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByVerticalController;
                    }
                case ((int)UserProfileEnum.GroupController):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByGroupController;
                    }
                case ((int)UserProfileEnum.Treasury):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByTreasury;
                    }
                case ((int)UserProfileEnum.ManagementAssurance):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByManagementAssurance;
                    }
                case ((int)UserProfileEnum.FASSC):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByFASSC;
                    }
                case ((int)UserProfileEnum.CB):
                    {
                        return (int)WorkFlowStatusEnum.NeedCorrectionByCB;
                    }
                default: return null;
            }
        }
        public List<GetBankAuditTrailDetail_Result> GetBankDetailAuditTrail(int BankDetailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetBankAuditTrailDetail(BankDetailId).ToList();
            }
        }

        public List<GetWorkFlowkAuditTrailDetail_Result> GetBankWorkflowAuditTrail(int WorkFlowId, string SubVerticalId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetWorkFlowkAuditTrailDetail(WorkFlowId, SubVerticalId).ToList();
            }
        }

        //public async Task<string> GetBankDetailByOCRReader(string FilePath)
        //{          
        //    OCRAPI.OCRReader oOCRAPI = new OCRAPI.OCRReader();
        //    var sDetails = await oOCRAPI.ReadImageData(FilePath);
        //    return sDetails.ToString();           
        //}
        public async Task<string> GetBankDetailByOCRReader(string FilePath)
        {
            OCRAPI.OCRReader oOCRAPI = new OCRAPI.OCRReader();
            return await oOCRAPI.ReadImageData(FilePath);

        }

        public async Task<string> GetBankDetailByOCRReaderForMobile(string FileName)
        {
            String FilePath = HostingEnvironment.MapPath("~/" + Constants.VENDOR_BANK_ATTACHMENT_PATH);
            FilePath = FilePath + "/" + FileName;

            OCRAPI.OCRReader oOCRAPI = new OCRAPI.OCRReader();
            return await oOCRAPI.ReadImageData(FilePath);
        }

        public string CheckBankVendorCompCodeExistInSAP(string VendorCode, string CompanyCode)
        {
            bool IsRestrictToSAP = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRestrictToSAP"]);

            if (IsRestrictToSAP)
                return Constants.SUCCESS;


            SAPHelper objSAP = new SAPHelper();
            using (SAPConnection connection = objSAP.GetSAPConnection())
            {
                CashAndBank_SAP.Vendor obj = new CashAndBank_SAP.Vendor();
                obj.Connection = connection;
                try
                {
                    obj.Connection.Open();
                }
                catch (Exception ex)
                {
                    return Constants.SAP_CONNECTION_FAILURE;
                }
                CashAndBank_SAP.BAPIRETURNTable bapiRTable = new CashAndBank_SAP.BAPIRETURNTable();
                obj.Yfif_Vend_Block_Details(CompanyCode, VendorCode, ref bapiRTable);
                System.Data.DataTable bapiResult = bapiRTable.ToADODataTable();
                if (bapiResult != null && bapiResult.Rows.Count > 0)
                {
                    string result = Convert.ToString(bapiResult.Rows[0]["Type"]);
                    if (result.ToUpper() == "E")
                    {
                        return Convert.ToString(bapiResult.Rows[0]["Message"]);
                    }
                    return Constants.SUCCESS;

                }
                else
                    return Constants.SAP_ERROR;
            }
        }

        public string UpdateBankDetailHistory(BankDetailHistoryModel bankDetailHistoryModel)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                BankDetailHistory bankDetailHistory = suzlonBPPEntities.BankDetailHistories.FirstOrDefault(bh => bh.BankDetailHistoryId == bankDetailHistoryModel.BankDetailHistoryId);
                if (bankDetailHistory != null)
                {
                    bankDetailHistory.OriginalDocumentsReceived = true;
                    bankDetailHistory.ReceivedDate = bankDetailHistoryModel.ReceivedDate;
                    suzlonBPPEntities.SaveChanges();
                    return Constants.SUCCESS;
                }
                else
                    return Constants.ERROR;
            }
        }

        public string UpdateBankDetailHistoryDocSend(BankDetailHistoryModel bankDetailHistoryModel)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                BankDetailHistory bankDetailHistory = suzlonBPPEntities.BankDetailHistories.FirstOrDefault(bh => bh.BankDetailHistoryId == bankDetailHistoryModel.BankDetailHistoryId);
                if (bankDetailHistory != null)
                {
                    bankDetailHistory.OriginalDocumentsSent = bankDetailHistoryModel.OriginalDocumentsReceived;
                    if (bankDetailHistoryModel.OriginalDocumentsReceived)
                        bankDetailHistory.SendDate = bankDetailHistoryModel.ReceivedDate;
                    else
                        bankDetailHistory.SendDate = null;
                    suzlonBPPEntities.SaveChanges();
                    return Constants.SUCCESS;
                }
                else
                    return Constants.ERROR;
            }
        }

        public static List<usp_GetBankDetialsReportData_Result> GetBankDetailsReportData(string CompanyCode, string VendorCode, string VendorName, string Initiator)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.usp_GetBankDetialsReportData(CompanyCode, VendorCode, VendorName, Initiator).ToList();
            }
        }

        #region "Notifications"

        private void SendInitiationNotification(bool isCreated, BankDetailModel bankDetailModel)
        {
            // "PendingApproval"
            string companyName = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var vendorMaster = suzlonBPPEntities.VendorMasters.FirstOrDefault(vm => vm.VendorCode == bankDetailModel.VendorCode && vm.CompanyCode == bankDetailModel.CompanyCode);
                if (vendorMaster != null)
                    companyName = vendorMaster.CompanyName;
            }
            //string url = ConfigurationManager.AppSettings["WebUrlForMail"];
            string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("BankDeatilId=" + bankDetailModel.BankDetailId));
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("Dear Sir/Madam,");
            mailBody.AppendFormat("<br><br>Bank Details of " + bankDetailModel.VendorCode + " - " + bankDetailModel.VendorName + " has " + (isCreated ? "Created" : "Updated") + " and has come for your approval");
            mailBody.AppendFormat("<br><br>Click the link below to view the details: <br><a href='{0}'>{1}</a> <br>", url, url);
            mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);

            string mailSubject = bankDetailModel.VendorCode + " - " + bankDetailModel.VendorName + " has " + (isCreated ? "Created" : "Updated") + " the Bank Details for " + bankDetailModel.CompanyCode + " " + companyName + ".";
            CommonFunctions.SendEmail(bankDetailModel.SuzlonEmailID1 + ";" + bankDetailModel.SuzlonEmailID2, string.Empty, mailBody.ToString(), mailSubject, null, "");
        }

        private void SendApprovedNotification(BankDetailModel bankDetailModel, int userId)
        {
            string lastStageUserName = string.Empty;
            string currentStageUserEmail = string.Empty;

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lastStageUserDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId);
                if (lastStageUserDetail != null)
                    lastStageUserName = lastStageUserDetail.Name;
                currentStageUserEmail = suzlonBPPEntities.usp_GetNextStageUserEmailId(bankDetailModel.BankDetailId, true).Single<string>();
            }
            if (!string.IsNullOrEmpty(currentStageUserEmail))
            {
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("BankDeatilId=" + bankDetailModel.BankDetailId));
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append("Dear Sir/Madam,");
                mailBody.AppendFormat("<br><br>Bank Details of " + bankDetailModel.VendorCode + " - " +  bankDetailModel.VendorName + " are Approved By " + lastStageUserName + " and now has come for your approval.");
                mailBody.AppendFormat("<br><br>Click the link below to view the details: <br><a href='{0}'>{1}</a> <br>", url, url);
                mailBody.AppendFormat("<br><br><span style='color:#2B2BC6'>Please ensure you are the authorized person to approve this vendor Bank details.</span>");
                mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);
                string mailSubject = bankDetailModel.VendorCode +" - "+ bankDetailModel.VendorName + " Bank Details Approved By " + lastStageUserName + ".";
                CommonFunctions.SendEmail(currentStageUserEmail, string.Empty, mailBody.ToString(), mailSubject, null, "", bankDetailModel.SuzlonEmailID1 + ";" + bankDetailModel.SuzlonEmailID2);
            }
        }

        private void SendApprovedNotificationToVendor(BankDetailModel bankDetailModel, int userId)
        {
            //"MyRecord";   
            //string url = ConfigurationManager.AppSettings["WebUrlForMail"];
            string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("BankDeatilId=" + bankDetailModel.BankDetailId));
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("Dear Sir/Madam,");
            mailBody.AppendFormat("<br><br>Bank Details are Approved By Suzlon.");
            mailBody.AppendFormat("<br><br>Click the link below to view the details: <br><a href='{0}'>{1}</a> <br>", url, url);

            mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);

            string mailSubject = "Bank Details Approved!: Bank Details are Approved By Suzlon.";
            CommonFunctions.SendEmail(bankDetailModel.VendorEmailID1 + ";" + bankDetailModel.VendorEmailID2, string.Empty, mailBody.ToString(), mailSubject, null, "");
        }
        private void SendApprovedNotificationToValidator(BankDetailModel bankDetailModel, int userId)
        {
            string lastStageUserName = string.Empty;
            //"MyRecord";   
            //string url = ConfigurationManager.AppSettings["WebUrlForMail"];
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lastStageUserDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId);
                if (lastStageUserDetail != null)
                    lastStageUserName = lastStageUserDetail.Name;
            }
            string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("BankDeatilId=" + bankDetailModel.BankDetailId));
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("Dear Sir/Madam,");
            mailBody.AppendFormat("<br><br>Bank Details of " + bankDetailModel.VendorCode + " - " + bankDetailModel.VendorName + " has been approved by " + lastStageUserName + " and updated in SAP.");
            mailBody.AppendFormat("<br><br>Click the link below to view the details: <br><a href='{0}'>{1}</a> <br>", url, url);            
            mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);

            string mailSubject = "Bank Details Updation!: " + bankDetailModel.VendorName + " Bank Details updated.";
            CommonFunctions.SendEmail(bankDetailModel.SuzlonEmailID1 + ";" + bankDetailModel.SuzlonEmailID2, string.Empty, mailBody.ToString(), mailSubject, null, "");
        }

        private void SendRejectedNotification(BankDetailModel bankDetailModel, int userId)
        {
            //"MyRecord";
            string comments = string.Empty;
            if (bankDetailModel.BankComments != null && bankDetailModel.BankComments.Count > 0)
            {
                bankDetailModel.BankComments.ForEach(c =>
                {
                    if (!string.IsNullOrEmpty(c.Comment))
                    {
                        if (string.IsNullOrEmpty(comments))
                            comments = c.Comment;
                        else
                            comments = comments + "<br>" + c.Comment;
                    }
                });
            }
            //string url = ConfigurationManager.AppSettings["WebUrlForMail"];
            string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("BankDeatilId=" + bankDetailModel.BankDetailId));
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("Dear Sir/Madam,");
            mailBody.AppendFormat("<br><br>Bank Details of " + bankDetailModel.VendorCode +" & "+  bankDetailModel.VendorName + " " +bankDetailModel.CompanyCode+ " are Rejected by Suzlon.");
            mailBody.AppendFormat("<br><br>Rejection Comments: " + comments);
            mailBody.AppendFormat("<br><br>Click the link below to view the details: <br><a href='{0}'>{1}</a> <br>", url, url);
            mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);


            string mailSubject = "Bank Details Rejected!: " + bankDetailModel.VendorName + " Bank Details rejected by Suzlon.";
            CommonFunctions.SendEmail(bankDetailModel.VendorEmailID1 + ";" + bankDetailModel.VendorEmailID2, string.Empty, mailBody.ToString(), mailSubject, null, "");
            string lastStageUserName = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lastStageUserDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId);
                if (lastStageUserDetail != null)
                    lastStageUserName = lastStageUserDetail.Name;
            }
            mailSubject = "Bank Details Rejected!: " + bankDetailModel.VendorName + " Bank Details rejected by " + lastStageUserName + ".";
            //To Validator
            CommonFunctions.SendEmail(bankDetailModel.SuzlonEmailID1 + ";" + bankDetailModel.SuzlonEmailID1, string.Empty, mailBody.ToString(), mailSubject, null, "");
        }

        private void SendNeedCorrNotification(BankDetailModel bankDetailModel, int userId)
        {
            //"PendingApproval";
            string assignedByUser = string.Empty;
            string comments = string.Empty;
            string emailIdsToSend = string.Empty;
            if (bankDetailModel.BankComments != null && bankDetailModel.BankComments.Count > 0)
            {
                bankDetailModel.BankComments.ForEach(c =>
                {
                    if (!string.IsNullOrEmpty(c.Comment))
                    {
                        if (string.IsNullOrEmpty(comments))
                            comments = c.Comment;
                        else
                            comments = comments + "<br>" + c.Comment;
                    }
                });
            }

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lastStageUserDetail = suzlonBPPEntities.Users.FirstOrDefault(u => u.UserId == userId);
                if (lastStageUserDetail != null)
                    assignedByUser = lastStageUserDetail.Name;
                emailIdsToSend = suzlonBPPEntities.usp_GetNextStageUserEmailId(bankDetailModel.BankDetailId, false).Single<string>();
            }

            if (!string.IsNullOrEmpty(emailIdsToSend))
            {
                //string url = ConfigurationManager.AppSettings["WebUrlForMail"];
                string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("BankDeatilId=" + bankDetailModel.BankDetailId));
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append("Dear Sir/Madam,");
                mailBody.AppendFormat("<br><br>Bank Details of " + bankDetailModel.VendorCode + " - " + bankDetailModel.VendorName + " " +bankDetailModel.CompanyCode + " has come for changes.");
                mailBody.AppendFormat("<br><br>Need correction Comments: " + comments);
                mailBody.AppendFormat("<br><br>Click the link below to view the details: <br><a href='{0}'>{1}</a> <br>", url, url);
                mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);

                string mailSubject = "Bank Details for need correction: " + bankDetailModel.VendorCode + " and "  + bankDetailModel.VendorName + " assigned by " + assignedByUser + ".";
                CommonFunctions.SendEmail(emailIdsToSend, string.Empty, mailBody.ToString(), mailSubject, null, "", bankDetailModel.SuzlonEmailID1 + ";" + bankDetailModel.SuzlonEmailID2);
            }
        }

        private void SendNeedCorrNotificationToVendor(BankDetailModel bankDetailModel, int userId)
        {
            //"MyRecord";
            string comments = string.Empty;
            if (bankDetailModel.BankComments != null && bankDetailModel.BankComments.Count > 0)
            {
                bankDetailModel.BankComments.ForEach(c =>
                {
                    if (!string.IsNullOrEmpty(c.Comment))
                    {
                        if (string.IsNullOrEmpty(comments))
                            comments = c.Comment;
                        else
                            comments = comments + "<br>" + c.Comment;
                    }
                });
            }

            //string url = ConfigurationManager.AppSettings["WebUrlForMail"];
            string url = ConfigurationManager.AppSettings["WebUrlForMail"] + "/Login.aspx?" + HttpUtility.UrlEncode(Crypto.Instance.Encrypt("BankDeatilId=" + bankDetailModel.BankDetailId));
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("Dear Sir/Madam,");
            mailBody.AppendFormat("<br><br>Bank Details of " + bankDetailModel.VendorCode + " & "+ bankDetailModel.VendorName + " " + bankDetailModel.CompanyCode + " has come for changes.");
            mailBody.AppendFormat("<br><br>Need correction Comments: " + comments);
            mailBody.AppendFormat("<br><br>Click the link below to view the details: <br><a href='{0}'>{1}</a> <br>", url, url);
            mailBody.AppendFormat("<br><br>Thanks & Regards,<br>* This is a system generated mail. In case any assistance is required, please mail to " + ConfigurationManager.AppSettings["SupportEmail"]);

            string mailSubject = "Bank Details for need correction" + bankDetailModel.VendorCode + " and " + bankDetailModel.VendorName + " assigned by Suzlon.";
            CommonFunctions.SendEmail(bankDetailModel.VendorEmailID1 + ";" + bankDetailModel.VendorEmailID2, string.Empty, mailBody.ToString(), mailSubject, null, "", bankDetailModel.SuzlonEmailID1 + ";" + bankDetailModel.SuzlonEmailID2);
        }

        #endregion "Notifications"
    }

    public class BankDetailModel
    {
        public int BankDetailId { get; set; }
        public string VendorCode { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string VendorName { get; set; }
        public string VendorPanNo { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string City { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> WorkFlowStatusId { get; set; }
        public string WorkFlowStatus { get; set; }
        public string SuzlonEmailID1 { get; set; }
        public string SuzlonEmailID2 { get; set; }
        public string VendorEmailID1 { get; set; }
        public string VendorEmailID2 { get; set; }
        public Nullable<bool> OriginalDocumentsSent { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public Nullable<bool> OriginalDocumentsReceived { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public Nullable<int> AssignedTo { get; set; }
        public Nullable<int> VerticalId { get; set; }
        public Nullable<int> SubVerticalId { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        public virtual List<BankCommentModel> BankComments { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public bool IsNew { get; set; }
        public bool IsCeatedByVendor { get; set; }
        public Nullable<System.DateTime> Modifidate { get; set; }
        public string RequestType { get; set; }
    }

    public class BankCommentModel
    {
        public string Comment { get; set; }
        public string CommentBy { get; set; }
    }

    public class BankPendingApprovalDetail
    {
        public int BankDetailId { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public Nullable<int> AssignedTo { get; set; }
    }

    public class BankValidatorUpdate
    {
        public int BankDetailId { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public int SubVerticalId { get; set; }
    }

    public class SecUserNotification
    {
        public string ToEmails { get; set; }
        public string StageName { get; set; }
        public string SubVerticalName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int AssignedUserId { get; set; }
        public int PriActorUserId { get; set; }
    }

    public class BankDetailHistoryModel
    {
        public int BankDetailHistoryId { get; set; }
        public bool OriginalDocumentsReceived { get; set; }
        public DateTime? ReceivedDate { get; set; }
    }
}
