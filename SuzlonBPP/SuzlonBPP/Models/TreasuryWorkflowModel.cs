using Cryptography;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;

namespace SuzlonBPP.Models
{
    public class TreasuryWorkflowModel
    {
        public Models.TreasuryWorkflow treasuryWorkflow;
        public SubVerticalMaster subVerticalMaster;

        public List<TreasuryWorkflowModel> GetTreasuryWorkflow(string subVerticalIds)
        {
            List<int> subVertical = subVerticalIds.Split(',').Select(Int32.Parse).ToList();
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var workflowDetails = (from subvertical in suzlonBPPEntities.SubVerticalMasters
                                       join workflow in suzlonBPPEntities.TreasuryWorkflows
                                       on subvertical.SubVerticalId equals workflow.SubVerticalId into sw
                                       from workflow in sw.DefaultIfEmpty()
                                       where subVertical.Contains(subvertical.SubVerticalId)
                                       select new TreasuryWorkflowModel
                                       {
                                           subVerticalMaster = subvertical,
                                           treasuryWorkflow = workflow
                                       }).ToList();
                return workflowDetails;
            }
        }

        /// <summary>
        /// This method is used to save/update bank workflow details.
        /// </summary>
        /// <param name="treasuryWorkflow"></param>
        /// <returns></returns>
        public bool SaveTreasuryWorkflow(TreasuryWorkflow treasuryWorkflow, int userId)
        {
            bool isSave = false;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                TreasuryWorkflow workflow = suzlonBPPEntities.TreasuryWorkflows.AsNoTracking().FirstOrDefault(w => w.SubVerticalId == treasuryWorkflow.SubVerticalId);
                if (workflow == null)
                {
                    treasuryWorkflow.CreatedBy = userId;
                    treasuryWorkflow.CreatedOn = DateTime.Now;
                    treasuryWorkflow.ModifiedBy = userId;
                    treasuryWorkflow.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.TreasuryWorkflows.Add(treasuryWorkflow);
                }
                else
                {
                    treasuryWorkflow.TreasuryWorkFlowId = workflow.TreasuryWorkFlowId;
                    treasuryWorkflow.CreatedBy = workflow.CreatedBy;
                    treasuryWorkflow.CreatedOn = workflow.CreatedOn;
                    treasuryWorkflow.ModifiedBy = userId;
                    treasuryWorkflow.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(treasuryWorkflow).State = EntityState.Modified;
                }
                suzlonBPPEntities.SaveChanges();
                SecUserNotification SecUserNotification = new SecUserNotification();
                NotificationModel notificationModel = new NotificationModel();
                string subVerticalName = string.Empty;
                string workFlowName = "Treasury";
                var subVertical = suzlonBPPEntities.SubVerticalMasters.FirstOrDefault(u => u.SubVerticalId == treasuryWorkflow.SubVerticalId);
                if (subVertical != null)
                    subVerticalName = subVertical.Name;
                SecUserNotification.SubVerticalName = subVerticalName;
                if ((workflow == null && treasuryWorkflow.SecVerContUserId.HasValue) ||
                    (workflow != null && ((workflow.SecVerContUserId == null && treasuryWorkflow.SecVerContUserId.HasValue) ||
                    (workflow.SecVerContUserId.HasValue && treasuryWorkflow.SecVerContUserId.HasValue && workflow.SecVerContUserId.Value != treasuryWorkflow.SecVerContUserId.Value))))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.FromDate = (treasuryWorkflow.SecVerContFromDt.HasValue ? treasuryWorkflow.SecVerContFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (treasuryWorkflow.SecVerContToDt.HasValue ? treasuryWorkflow.SecVerContToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = treasuryWorkflow.SecVerContUserId.Value;
                    SecUserNotification.PriActorUserId = treasuryWorkflow.PriVerContUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecVerContUserId.HasValue && treasuryWorkflow.SecVerContUserId.HasValue && workflow.SecVerContUserId.Value != treasuryWorkflow.SecVerContUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecVerContUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecVerContUserId.HasValue && treasuryWorkflow.SecVerContUserId == null)))
                {
                    SecUserNotification.StageName = "Vertical Controller";
                    SecUserNotification.PriActorUserId = treasuryWorkflow.PriVerContUserId;
                    SecUserNotification.AssignedUserId = workflow.SecVerContUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((workflow == null && treasuryWorkflow.SecTreasuryUserId.HasValue) ||
                    (workflow != null && ((workflow.SecTreasuryUserId == null && treasuryWorkflow.SecTreasuryUserId.HasValue) ||
                    (workflow.SecTreasuryUserId.HasValue && treasuryWorkflow.SecTreasuryUserId.HasValue && workflow.SecTreasuryUserId.Value != treasuryWorkflow.SecTreasuryUserId.Value))))
                {
                    SecUserNotification.StageName = "Treasury";
                    SecUserNotification.FromDate = (treasuryWorkflow.SecTreasuryFromDt.HasValue ? treasuryWorkflow.SecTreasuryFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (treasuryWorkflow.SecTreasuryToDt.HasValue ? treasuryWorkflow.SecTreasuryToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = treasuryWorkflow.SecTreasuryUserId.Value;
                    SecUserNotification.PriActorUserId = treasuryWorkflow.PriTreasuryUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecTreasuryUserId.HasValue && treasuryWorkflow.SecTreasuryUserId.HasValue && workflow.SecTreasuryUserId.Value != treasuryWorkflow.SecTreasuryUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecTreasuryUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecTreasuryUserId.HasValue && treasuryWorkflow.SecTreasuryUserId == null)))
                {
                    SecUserNotification.StageName = "Treasury";
                    SecUserNotification.PriActorUserId = treasuryWorkflow.PriTreasuryUserId;
                    SecUserNotification.AssignedUserId = workflow.SecTreasuryUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }

                if ((workflow == null && treasuryWorkflow.SecCBUserId.HasValue) ||
                    (workflow != null && ((workflow.SecCBUserId == null && treasuryWorkflow.SecCBUserId.HasValue) ||
                    (workflow.SecCBUserId.HasValue && treasuryWorkflow.SecCBUserId.HasValue && workflow.SecCBUserId.Value != treasuryWorkflow.SecCBUserId.Value))))
                {
                    SecUserNotification.StageName = "C&B";
                    SecUserNotification.FromDate = (treasuryWorkflow.SecCBFromDt.HasValue ? treasuryWorkflow.SecCBFromDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.ToDate = (treasuryWorkflow.SecCBToDt.HasValue ? treasuryWorkflow.SecCBToDt.Value.ToString("yyyy-MM-dd") : string.Empty);
                    SecUserNotification.AssignedUserId = treasuryWorkflow.SecCBUserId.Value;
                    SecUserNotification.PriActorUserId = treasuryWorkflow.PriCBUserId;
                    notificationModel.SendSecActorAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    if (workflow != null && (workflow.SecCBUserId.HasValue && treasuryWorkflow.SecCBUserId.HasValue && workflow.SecCBUserId.Value != treasuryWorkflow.SecCBUserId.Value))
                    {
                        SecUserNotification.AssignedUserId = workflow.SecCBUserId.Value;
                        notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                    }
                }
                else if (workflow != null && ((workflow.SecCBUserId.HasValue && treasuryWorkflow.SecCBUserId == null)))
                {
                    SecUserNotification.StageName = "C&B";
                    SecUserNotification.PriActorUserId = treasuryWorkflow.PriCBUserId;
                    SecUserNotification.AssignedUserId = workflow.SecCBUserId.Value;
                    notificationModel.SendSecActorUnAssignNotification(SecUserNotification, suzlonBPPEntities, workFlowName);
                }



                isSave = true;
            }
            return isSave;
        }


        //TreasuryWorkflowModel
        public TreasuryDetailModel AddTreasuryDetail(TreasuryDetailModel treasuryDetailModel, int userId)
        {

            TreasuryDetail TreasuryDetails = new TreasuryDetail();
            SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities();


            TreasuryDetails.CompanyCode = treasuryDetailModel.CompanyCode;
            TreasuryDetails.CreatedBy = userId;
            TreasuryDetails.CreatedOn = DateTime.Now;
            TreasuryDetails.ModifiedBy = userId;
            TreasuryDetails.ModifiedOn = DateTime.Now;
            TreasuryDetails.VendorCreatedOn = treasuryDetailModel.VendorCreatedOn;
            TreasuryDetails.SubVerticalId = treasuryDetailModel.SubVerticalId;
            TreasuryDetails.VerticalId = treasuryDetailModel.VerticalId;
            TreasuryDetails.AllocationNumber = treasuryDetailModel.AllocationNumber + "-" + treasuryDetailModel.generateNextOffset();
            TreasuryDetails.RequestedAmount = treasuryDetailModel.RequestedAmount;
            TreasuryDetails.InitApprovedAmount = treasuryDetailModel.InitApprovedAmount;
            TreasuryDetails.AddendumTotal = treasuryDetailModel.AddendumTotal;
            TreasuryDetails.FinalAmount = treasuryDetailModel.FinalAmount;
            TreasuryDetails.UtilsationStartDate = (treasuryDetailModel.UtilsationStartDate == DateTime.MinValue) ? DateTime.Now : treasuryDetailModel.UtilsationStartDate;
            TreasuryDetails.UtilsationEndDate = (treasuryDetailModel.UtilsationEndDate == DateTime.MinValue) ? DateTime.Now : treasuryDetailModel.UtilsationEndDate;
            TreasuryDetails.ProposedDate = treasuryDetailModel.ProposedDate;
            TreasuryDetails.RequestType = treasuryDetailModel.RequestType;
            TreasuryDetails.BalanceAmount = treasuryDetailModel.BalanceAmount;
            TreasuryDetails.WorkflowStatusId = treasuryDetailModel.WorkflowStatusID;
            TreasuryDetails.Status = treasuryDetailModel.Status;

            suzlonBPPEntities.TreasuryDetails.Add(TreasuryDetails);
            suzlonBPPEntities.SaveChanges();

            if ((treasuryDetailModel.NatureOfRequest != null && treasuryDetailModel.NatureOfRequest.Count > 0))
            {
                treasuryDetailModel.NatureOfRequest.ForEach(request =>
                {
                    TreasuryNatureRequestDetail requestModel = new TreasuryNatureRequestDetail();
                    requestModel.TreasuryId = TreasuryDetails.TreasuryDetailId;
                    requestModel.NatureOfReqestId = request.natureOfRequest;
                    requestModel.NatureOfReqest = request.natureOfRequestText;
                    requestModel.ApprovedAmount = request.approvedAmount;
                    requestModel.RequestedAmount = request.requestedAmount;
                    requestModel.CreatedBy = userId;
                    requestModel.CreatedOn = DateTime.Now;
                    requestModel.ModifiedBy = userId;
                    requestModel.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.TreasuryNatureRequestDetails.Add(requestModel);
                    suzlonBPPEntities.SaveChanges();
                });
            }

            if ((treasuryDetailModel.verticalControllerComment != null && treasuryDetailModel.verticalControllerComment.Count > 0))
            {
                treasuryDetailModel.verticalControllerComment.ForEach(request =>
                {
                    TreasuryComment requestModel = new TreasuryComment();
                    requestModel.TreasuryId = TreasuryDetails.TreasuryDetailId;
                    requestModel.Comment = request.Comment;
                    requestModel.CreatedBy = userId;
                    requestModel.CreatedOn = DateTime.Now;
                    requestModel.ModifiedBy = userId;
                    requestModel.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.TreasuryComments.Add(requestModel);
                    suzlonBPPEntities.SaveChanges();
                });
            }

            //TBM
            if ((treasuryDetailModel.FileUpload != null && treasuryDetailModel.FileUpload.Count > 0))
            {
                treasuryDetailModel.FileUpload.ForEach(request =>
                {
                    var appPath = AppDomain.CurrentDomain.BaseDirectory;
                    FileUpload requestModel = new FileUpload();
                    requestModel.EntityId = TreasuryDetails.TreasuryDetailId;
                    requestModel.EntityName = request.EntityName;
                    requestModel.FileName = request.FileName;
                    requestModel.DisplayName = request.DisplayName;
                    requestModel.CreatedBy = userId;
                    requestModel.CreatedOn = DateTime.Now;
                    requestModel.ModifiedBy = userId;
                    requestModel.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.FileUploads.Add(requestModel);
                    suzlonBPPEntities.SaveChanges();
                    var newFileName = request.FileName.Replace(userId + "!", requestModel.FileUploadId + "!");
                    System.IO.File.Move(appPath + Constants.VENDOR_BANK_ATTACHMENT_PATH_TEMP + request.FileName, appPath + Constants.VENDOR_BANK_ATTACHMENT_PATH + newFileName);
                    requestModel.FileName = newFileName;
                    suzlonBPPEntities.SaveChanges();
                });
            }
            if(TreasuryDetails.WorkflowStatusId==2)
            SendInitiationNotification(TreasuryDetails, userId);


            return treasuryDetailModel;
        }

        public TreasuryDetailModel EditTreasuryrDetail(TreasuryDetailModel treasuryDetailModel, int userId)
        {

            int WorkFlowStatus = 0;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                TreasuryDetail treasuryDetail = suzlonBPPEntities.TreasuryDetails.FirstOrDefault(u => u.TreasuryDetailId == treasuryDetailModel.TreasuryDetailId);
                WorkFlowStatus = treasuryDetail.WorkflowStatusId;

                treasuryDetail.CompanyCode = treasuryDetailModel.CompanyCode;
                treasuryDetail.ModifiedBy = userId;
                treasuryDetail.ModifiedOn = DateTime.Now;
                treasuryDetail.VendorCreatedOn = treasuryDetailModel.VendorCreatedOn;
                treasuryDetail.SubVerticalId = treasuryDetailModel.SubVerticalId;
                treasuryDetail.VerticalId = treasuryDetailModel.VerticalId;
                //treasuryDetail.AllocationNumber = treasuryDetailModel.AllocationNumber;
                treasuryDetail.RequestedAmount = treasuryDetailModel.RequestedAmount;
                treasuryDetail.InitApprovedAmount = treasuryDetailModel.InitApprovedAmount;
                treasuryDetail.AddendumTotal = treasuryDetailModel.AddendumTotal;
                treasuryDetail.FinalAmount = treasuryDetailModel.FinalAmount;
                treasuryDetail.UtilsationStartDate = treasuryDetailModel.UtilsationStartDate;
                treasuryDetail.UtilsationEndDate = treasuryDetailModel.UtilsationEndDate;
                treasuryDetail.ProposedDate = treasuryDetailModel.ProposedDate;
                treasuryDetail.RequestType = treasuryDetailModel.RequestType;
                treasuryDetail.BalanceAmount = treasuryDetailModel.BalanceAmount;
                treasuryDetail.WorkflowStatusId = treasuryDetailModel.WorkflowStatusID;
                treasuryDetail.Status = treasuryDetailModel.Status;
                suzlonBPPEntities.SaveChanges();

                List<TreasuryRequestModel> newRequest = treasuryDetailModel.NatureOfRequest.Where(r => r.TreasuryRequestId == 0).ToList();
                if ((newRequest != null && newRequest.Count > 0))
                {
                    newRequest.ForEach(request =>
                        {
                            TreasuryNatureRequestDetail requestModel = new TreasuryNatureRequestDetail();
                            requestModel.TreasuryId = treasuryDetail.TreasuryDetailId;
                            requestModel.NatureOfReqestId = request.natureOfRequest;
                            requestModel.NatureOfReqest = request.natureOfRequestText;
                            requestModel.ApprovedAmount = request.approvedAmount;
                            requestModel.RequestedAmount = request.requestedAmount;
                            requestModel.CreatedBy = userId;
                            requestModel.CreatedOn = DateTime.Now;
                            requestModel.ModifiedBy = userId;
                            requestModel.ModifiedOn = DateTime.Now;
                            suzlonBPPEntities.TreasuryNatureRequestDetails.Add(requestModel);
                            suzlonBPPEntities.SaveChanges();
                        });
                }

                List<TreasuryRequestModel> updateRequest = treasuryDetailModel.NatureOfRequest.Where((r => r.TreasuryRequestId != 0 && r.isDirty == "T")).ToList();
                if ((updateRequest != null && updateRequest.Count > 0))
                {
                    updateRequest.ForEach(request =>
                    {
                        TreasuryNatureRequestDetail currentRequest = suzlonBPPEntities.TreasuryNatureRequestDetails.FirstOrDefault(u => u.TreasuryRequestId == request.TreasuryRequestId);
                        currentRequest.TreasuryId = treasuryDetail.TreasuryDetailId;
                        currentRequest.NatureOfReqestId = request.natureOfRequest;
                        currentRequest.NatureOfReqest = request.natureOfRequestText;
                        currentRequest.ApprovedAmount = request.approvedAmount;
                        currentRequest.RequestedAmount = request.requestedAmount;
                        currentRequest.ModifiedBy = userId;
                        currentRequest.ModifiedOn = DateTime.Now;
                        suzlonBPPEntities.SaveChanges();
                    });
                }

                if ((treasuryDetailModel.verticalControllerComment != null && treasuryDetailModel.verticalControllerComment.Count > 0))
                {
                    treasuryDetailModel.verticalControllerComment.ForEach(request =>
                    {
                        TreasuryComment requestModel = new TreasuryComment();
                        requestModel.TreasuryId = treasuryDetail.TreasuryDetailId;
                        requestModel.Comment = request.Comment;
                        requestModel.WorkflowStatusId = treasuryDetail.WorkflowStatusId;
                        requestModel.CreatedBy = userId;
                        requestModel.CreatedOn = DateTime.Now;
                        requestModel.ModifiedBy = userId;
                        requestModel.ModifiedOn = DateTime.Now;
                        suzlonBPPEntities.TreasuryComments.Add(requestModel);
                        suzlonBPPEntities.SaveChanges();
                    });
                }
                if (treasuryDetail.WorkflowStatusId == 2)
                    SendInitiationNotification(treasuryDetail, userId);

                if (treasuryDetail.WorkflowStatusId == 3 && treasuryDetail.WorkflowStatusId != WorkFlowStatus)
                    SendApproveNotification(treasuryDetail, userId);

                if (treasuryDetail.WorkflowStatusId == 4)
                    SendRejectNotification(treasuryDetail, userId);

                if (treasuryDetail.WorkflowStatusId == 5)
                    SendNeedCorrectionNotification(treasuryDetail, userId);
            }
            

            return treasuryDetailModel;
        }

        public dynamic GetTreasuryDetail(int TreasuryId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var vendorDetail = suzlonBPPEntities.TreasuryDetails.FirstOrDefault(v => v.TreasuryDetailId == TreasuryId);
                var VendorDetail = (from verticalController in suzlonBPPEntities.TreasuryDetails
                                    where verticalController.TreasuryDetailId == TreasuryId
                                    select new
                                    {
                                        verticalController.TreasuryDetailId,
                                        verticalController.VendorCreatedOn,
                                        verticalController.ProposedDate,
                                        verticalController.CompanyCode,
                                        verticalController.VerticalId,
                                        verticalController.SubVerticalId,
                                        verticalController.AllocationNumber,
                                        verticalController.RequestType,
                                        verticalController.RequestedAmount,
                                        verticalController.InitApprovedAmount,
                                        verticalController.AddendumTotal,
                                        verticalController.FinalAmount,
                                        verticalController.UtilsationStartDate,
                                        verticalController.UtilsationEndDate,
                                        verticalController.BalanceAmount,
                                        verticalController.WorkflowStatusId,
                                        verticalController.Status,
                                        NatureOfRequest = (from request in suzlonBPPEntities.TreasuryNatureRequestDetails
                                                           where request.TreasuryId == TreasuryId
                                                           select new TreasuryRequestModel
                                                           {
                                                               TreasuryRequestId = request.TreasuryRequestId,
                                                               natureOfRequest = request.NatureOfReqestId,
                                                               natureOfRequestText = request.NatureOfReqest,
                                                               approvedAmount = (decimal)request.ApprovedAmount,
                                                               requestedAmount = (decimal)request.RequestedAmount
                                                           }).ToList(),

                                        VerticalControllerComment = (from comment in suzlonBPPEntities.TreasuryComments
                                                                     join user in suzlonBPPEntities.Users on comment.CreatedBy equals user.UserId
                                                                     where comment.TreasuryId == TreasuryId
                                                                     // && (isLoggedInUserVendor ? (comment.CreatedBy == userId || comment.BankWorkflowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor) : true)
                                                                     select new TreasuryCommentModel { Comment = comment.Comment, CommentBy = user.Name, CreatedOn = comment.CreatedOn }).ToList(),
                                    }).FirstOrDefault();
                return VendorDetail;
            }
        }


        public AddandumModel AddAdandumdDetails(AddandumModel addandumDetailModel, int userId)
        {
            AddandomDetail addandumDetails = new AddandomDetail();
            SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities();

            addandumDetails.AddandomStatus = addandumDetailModel.AddandomStatus;
            addandumDetails.CreatedBy = userId;
            addandumDetails.CreatedOn = DateTime.Now;
            addandumDetails.ModifiedBy = userId;
            addandumDetails.ModifiedOn = DateTime.Now;
            addandumDetails.AddandomWorkflowStatusId = addandumDetailModel.AddandomWorkflowStatusId;
            addandumDetails.Amount = addandumDetailModel.Amount;
            addandumDetails.ApprovedAmount = addandumDetailModel.ApprovedAmount;
            addandumDetails.NatureOfRequestId = addandumDetailModel.NatureOfRequestId;
            addandumDetails.Reason = addandumDetailModel.Reason;
            addandumDetails.TreasuryDetailId = addandumDetailModel.TreasuryDetailId;
            addandumDetails.AddandomDate = addandumDetailModel.AddandomDate;
            suzlonBPPEntities.AddandomDetails.Add(addandumDetails);
            suzlonBPPEntities.SaveChanges();
            addandumDetailModel.UpdateAddandumAmount(addandumDetailModel.TreasuryDetailId);
            addandumDetailModel.Id = addandumDetails.AddandomDetailId;

            if (!string.IsNullOrEmpty(addandumDetailModel.Comment))
                AddAddendumComment(addandumDetailModel, userId);

            if (addandumDetails.AddandomWorkflowStatusId == 2)
                SendAddendumInitiationNotification(addandumDetails, userId);


            return addandumDetailModel;
        }

        public bool AddAddendumComment(AddandumModel addandumDetailModel, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                AddendumComment addendumComment = new AddendumComment();
                addendumComment.AddendumId = addandumDetailModel.Id;
                addendumComment.TreasuryId = addandumDetailModel.TreasuryDetailId;
                addendumComment.Comment = addandumDetailModel.Comment;
                addendumComment.WorkFlowStatusId = addandumDetailModel.AddandomWorkflowStatusId;
                addendumComment.CreatedBy = userId;
                addendumComment.CreatedOn = DateTime.Now;
                addendumComment.ModifideBy = userId;
                addendumComment.ModifiedOn = DateTime.Now;
                suzlonBPPEntities.AddendumComments.Add(addendumComment);
                suzlonBPPEntities.SaveChanges();
                return true;
            }
                
        }

        public dynamic GetAddendumComment(int AddendumId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
               var comment= (from c in suzlonBPPEntities.AddendumComments
                 join u in suzlonBPPEntities.Users on c.CreatedBy equals u.UserId
                where c.AddendumId== AddendumId  orderby c.CreatedOn descending
                 select new 
                 {
                     Comment = c.Comment,
                     CreatedOn =c.CreatedOn,
                     UserName = u.Name
                 }).ToList();
                return comment;
            }
        }

        public AddandumModel EditAddandumdDetails(AddandumModel addandumDetailModel, int userId)
        {
            int addendumWorkflowStatusId = 0;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                AddandomDetail addandumDetails = suzlonBPPEntities.AddandomDetails.FirstOrDefault(u => u.AddandomDetailId == addandumDetailModel.Id);
                if(addandumDetails!=null)
                addendumWorkflowStatusId =Convert.ToInt32(addandumDetails.AddandomWorkflowStatusId);

                addandumDetails.AddandomStatus = addandumDetailModel.AddandomStatus;
                addandumDetails.ModifiedBy = userId;
                addandumDetails.ModifiedOn = DateTime.Now;
                addandumDetails.AddandomWorkflowStatusId = addandumDetailModel.AddandomWorkflowStatusId;
                addandumDetails.Amount = addandumDetailModel.Amount;
                addandumDetails.ApprovedAmount = addandumDetailModel.ApprovedAmount;
                addandumDetails.NatureOfRequestId = addandumDetailModel.NatureOfRequestId;
                addandumDetails.Reason = addandumDetailModel.Reason;
                addandumDetails.AddandomDate = addandumDetailModel.AddandomDate;
                addandumDetails.TreasuryDetailId = addandumDetailModel.TreasuryDetailId;
                suzlonBPPEntities.SaveChanges();
                addandumDetailModel.UpdateAddandumAmount(addandumDetailModel.TreasuryDetailId);
                if (!string.IsNullOrEmpty(addandumDetailModel.Comment))
                    AddAddendumComment(addandumDetailModel, userId);

                if (addandumDetails.AddandomWorkflowStatusId == 2)
                    SendAddendumInitiationNotification(addandumDetails, userId);
                if (addandumDetails.AddandomWorkflowStatusId == 3 && addandumDetailModel.AddandomWorkflowStatusId != addendumWorkflowStatusId)
                    SendAddendumApproveNotification(addandumDetails, userId);
                if (addandumDetails.AddandomWorkflowStatusId == 4)
                    SendAddendumRejectNotification(addandumDetails, userId);
                if (addandumDetails.AddandomWorkflowStatusId == 5)
                    SendAddendumReworkNotification(addandumDetails, userId);
            }

            return addandumDetailModel;
        }

        public List<GetTreasuryMyRequest_Result> GetMyRequestTreasury(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lstBankDetailModel = suzlonBPPEntities.GetTreasuryMyRequest(userId).ToList();
                return lstBankDetailModel;
            }
        }
        public List<GetTreasuryMyApprovedRequest_Result> GetMyApproveRequestTreasury(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lstBankDetailModel = suzlonBPPEntities.usp_GetTreasuryMyApprovedRequest(userId).ToList();
                return lstBankDetailModel;
            }
        }

        public List<GetTreasuryMyAddendumRequest_Result> GetMyAddendumTreasuryRequest(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lsttreasurymodel = suzlonBPPEntities.GetTreasuryMyAddendumRequest(userId).ToList();
                return lsttreasurymodel;
            }
        }
        public List<GetTreasuryPendingRequest_Result> GetTreasuryPendingRequest(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lsttreasurymodel = suzlonBPPEntities.GetTreasuryPendingRequest(userId).ToList();
                return lsttreasurymodel;
            }
        }

        public dynamic GetTreasuryRequestComment(int treasuryrequestid)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var treasuryComment = (from cmt in suzlonBPPEntities.TreasuryComments
                                       join u in suzlonBPPEntities.Users on cmt.CreatedBy equals u.UserId
                                       where cmt.TreasuryId == treasuryrequestid
                                       select new
                                       {
                                           u.Name,
                                           cmt.Comment
                                       }).ToList();
                return treasuryComment;
            }
        }
        public dynamic GetTreasuryRequestAttachment(int treasuryrequestid)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                //var treasuryComment = (from file in suzlonBPPEntities.FileUploads
                //                       join u in suzlonBPPEntities.Users on file.CreatedBy equals u.UserId
                //                       where file.EntityId == treasuryrequestid && file.EntityName== "Cash"
                //                       select new
                //                       {
                //                           u.Name,
                //                           file.FileName
                //                       }).ToList();
                var treasuryComment = (from b in suzlonBPPEntities.BankDetails
                                       join u in suzlonBPPEntities.Users on b.CreatedBy equals u.UserId
                                       select new
                                       {
                                           b.Attachment1,
                                           u.Name
                                       }).ToList();

                return treasuryComment;
            }
        }
        public List<GetTreasuryPendingAddendumRequest_Result> GetTreasuryPendingAddendumRequest(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lsttreasurymodel = suzlonBPPEntities.GetTreasuryPendingAddendumRequest(userId).ToList();
                return lsttreasurymodel;
            }
        }
        public List<GetTreasuryRequestApproveByMe_Result> GetTreasuryApproveByMeRequest(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lsttreasurymodel = suzlonBPPEntities.GetTreasuryRequestApproveByMe(userId).ToList();
                return lsttreasurymodel;
            }
        }
        public List<GetTreasuryRequestToCB_Result> GetTreasuryRequestToCB(int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var lsttreasurymodel = suzlonBPPEntities.GetTreasuryRequestToCB(userId).ToList();
                return lsttreasurymodel;
            }
        }



        private void SendInitiationNotification(TreasuryDetail treasuryDetailModel, int userId)
        {
            
            string InitiatorUserName = string.Empty;
            string TreasuryUserEmailId = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == userId);
                if (user != null)
                    InitiatorUserName = user.Name;

                var Treasury = (from T in suzlonBPPEntities.TreasuryDetails
                                join TW in suzlonBPPEntities.TreasuryWorkflows
                                on T.SubVerticalId equals TW.SubVerticalId
                                join U in suzlonBPPEntities.Users
                                on TW.PriTreasuryUserId equals U.UserId
                                where T.TreasuryDetailId == treasuryDetailModel.TreasuryDetailId
                                select new
                                {
                                    U.EmailId
                                }).FirstOrDefault();

                if (Treasury != null)
                    TreasuryUserEmailId = Treasury.EmailId;
               
                NotificationModel.SendTreasuryInitiationNotification(treasuryDetailModel.AllocationNumber, InitiatorUserName,Convert.ToString(treasuryDetailModel.RequestedAmount), TreasuryUserEmailId, Convert.ToString(treasuryDetailModel.TreasuryDetailId));

            }
        }

        private void SendApproveNotification(TreasuryDetail treasuryDetailModel, int userId)
        {
            string InitiatorUserName = string.Empty;
            string InitiatorUserEmail = string.Empty;
            string TreasuryUserEmailId = string.Empty;
            string TreasuryUserName = string.Empty;
            string CBUserMailId = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == treasuryDetailModel.CreatedBy);
                if (user != null)
                {
                    InitiatorUserName = user.Name;
                    InitiatorUserEmail = user.EmailId;
                }
                var Treasury = (from T in suzlonBPPEntities.TreasuryDetails
                                join TW in suzlonBPPEntities.TreasuryWorkflows
                                on T.SubVerticalId equals TW.SubVerticalId
                                join U in suzlonBPPEntities.Users
                                on TW.PriTreasuryUserId equals U.UserId
                                where T.TreasuryDetailId == treasuryDetailModel.TreasuryDetailId
                                select new
                                {
                                    U.EmailId,
                                    U.Name
                                }).FirstOrDefault();

                if (Treasury != null)
                {
                    TreasuryUserEmailId = Treasury.EmailId;
                    TreasuryUserName = Treasury.Name;
                }

                if(treasuryDetailModel.RequestType== "Manual" && treasuryDetailModel.WorkflowStatusId==3)
                {
                    var CbUser = (from T in suzlonBPPEntities.TreasuryDetails
                                    join TW in suzlonBPPEntities.TreasuryWorkflows
                                    on T.SubVerticalId equals TW.SubVerticalId
                                    join U in suzlonBPPEntities.Users
                                    on TW.PriCBUserId equals U.UserId
                                    where T.TreasuryDetailId == treasuryDetailModel.TreasuryDetailId
                                    select new
                                    {
                                        U.EmailId,
                                        U.Name
                                    }).FirstOrDefault();

                    if (Treasury != null)
                    {
                        CBUserMailId = CbUser.EmailId;
                     
                    }

                }

                NotificationModel.SendTreasuryApproveNotification(treasuryDetailModel.AllocationNumber, TreasuryUserName,Convert.ToString(treasuryDetailModel.RequestedAmount),Convert.ToString(treasuryDetailModel.InitApprovedAmount), InitiatorUserEmail, CBUserMailId, Convert.ToString(treasuryDetailModel.TreasuryDetailId));
               
            }
        }

        private void SendRejectNotification(TreasuryDetail treasuryDetailModel, int userId)
        {

            string InitiatorUserName = string.Empty;
            string InitiatorUserEmail = string.Empty;
            string TreasuryUserEmailId = string.Empty;
            string TreasuryUserName = string.Empty;
            string RejectionComment = string.Empty;

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == treasuryDetailModel.CreatedBy);
                var comment = suzlonBPPEntities.TreasuryComments.FirstOrDefault(U => U.TreasuryId == treasuryDetailModel.TreasuryDetailId && U.WorkflowStatusId==4);
                if (comment != null)
                    RejectionComment = comment.Comment;

                if (user != null)
                {
                    InitiatorUserName = user.Name;
                    InitiatorUserEmail = user.EmailId;
                }

                var Treasury = (from T in suzlonBPPEntities.TreasuryDetails
                                join TW in suzlonBPPEntities.TreasuryWorkflows
                                on T.SubVerticalId equals TW.SubVerticalId
                                join U in suzlonBPPEntities.Users
                                on TW.PriTreasuryUserId equals U.UserId
                                where T.TreasuryDetailId == treasuryDetailModel.TreasuryDetailId
                                select new
                                {
                                    U.EmailId,
                                    U.Name
                                }).FirstOrDefault();

                if (Treasury != null)
                {
                    TreasuryUserEmailId = Treasury.EmailId;
                    TreasuryUserName = Treasury.Name;
                }
                NotificationModel.SendTreasuryRejectNotification(treasuryDetailModel.AllocationNumber, TreasuryUserName, RejectionComment, InitiatorUserEmail, Convert.ToString(treasuryDetailModel.TreasuryDetailId));
            }
        }

        private void SendNeedCorrectionNotification(TreasuryDetail treasuryDetailModel, int userId)
        {
            string InitiatorUserName = string.Empty;
            string InitiatorUserEmail = string.Empty;
            string TreasuryUserEmailId = string.Empty;
            string TreasuryUserName = string.Empty;
            string NeedCorrectionComment = string.Empty;

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == treasuryDetailModel.CreatedBy);
                var comment = suzlonBPPEntities.TreasuryComments.FirstOrDefault(U => U.TreasuryId == treasuryDetailModel.TreasuryDetailId && U.WorkflowStatusId == 5);
                if (comment != null)
                    NeedCorrectionComment = comment.Comment;

                if (user != null)
                {
                    InitiatorUserName = user.Name;
                    InitiatorUserEmail = user.EmailId;
                }

                var Treasury = (from T in suzlonBPPEntities.TreasuryDetails
                                join TW in suzlonBPPEntities.TreasuryWorkflows
                                on T.SubVerticalId equals TW.SubVerticalId
                                join U in suzlonBPPEntities.Users
                                on TW.PriTreasuryUserId equals U.UserId
                                where T.TreasuryDetailId == treasuryDetailModel.TreasuryDetailId
                                select new
                                {
                                    U.EmailId,
                                    U.Name
                                }).FirstOrDefault();

                if (Treasury != null)
                {
                    TreasuryUserEmailId = Treasury.EmailId;
                    TreasuryUserName = Treasury.Name;
                }

                NotificationModel.SendTreasuryReworkNotification(treasuryDetailModel.AllocationNumber, TreasuryUserName, NeedCorrectionComment, InitiatorUserEmail, Convert.ToString(treasuryDetailModel.TreasuryDetailId));
            }
        }


        private void SendAddendumInitiationNotification(AddandomDetail addendumDetailModel, int userId)
        {

            string InitiatorUserName = string.Empty;
            string TreasuryUserEmailId = string.Empty;
            string AllocationNo = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == userId);
                if (user != null)
                    InitiatorUserName = user.Name;

                var Addendum = (from T in suzlonBPPEntities.TreasuryDetails
                                join TW in suzlonBPPEntities.TreasuryWorkflows
                                on T.SubVerticalId equals TW.SubVerticalId
                                join U in suzlonBPPEntities.Users
                                on TW.PriTreasuryUserId equals U.UserId
                                where T.TreasuryDetailId == addendumDetailModel.TreasuryDetailId
                                select new
                                {
                                    U.EmailId,
                                    T.AllocationNumber
                                }).FirstOrDefault();

                if (Addendum != null)
                {
                    TreasuryUserEmailId = Addendum.EmailId;
                    AllocationNo = Addendum.AllocationNumber;
                }
                NotificationModel.SendAddendumInitiationNotification(AllocationNo, InitiatorUserName,TreasuryUserEmailId, Convert.ToString(addendumDetailModel.TreasuryDetailId));

            }
        }

        private void SendAddendumApproveNotification(AddandomDetail addendumDetailModel, int userId)
        {
            string InitiatorUserName = string.Empty;
            string InitiatorUserEmail = string.Empty;
            string TreasuryUserEmailId = string.Empty;
            string TreasuryUserName = string.Empty;
            string CBUserMailId = string.Empty;
            string AllocationNo = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == addendumDetailModel.CreatedBy);
                if (user != null)
                {
                    InitiatorUserName = user.Name;
                    InitiatorUserEmail = user.EmailId;
                }
                var Treasury = (from T in suzlonBPPEntities.TreasuryDetails
                                join TW in suzlonBPPEntities.TreasuryWorkflows
                                on T.SubVerticalId equals TW.SubVerticalId
                                join U in suzlonBPPEntities.Users
                                on TW.PriTreasuryUserId equals U.UserId
                                where T.TreasuryDetailId == addendumDetailModel.TreasuryDetailId
                                select new
                                {
                                    U.EmailId,
                                    U.Name,
                                    T.AllocationNumber
                                }).FirstOrDefault();

                if (Treasury != null)
                {
                    TreasuryUserEmailId = Treasury.EmailId;
                    TreasuryUserName = Treasury.Name;
                    AllocationNo = Treasury.AllocationNumber;
                }       

                NotificationModel.SendAddendumApproveNotification(AllocationNo, TreasuryUserName,Convert.ToString(addendumDetailModel.Amount),Convert.ToString(addendumDetailModel.ApprovedAmount), InitiatorUserEmail, Convert.ToString(addendumDetailModel.TreasuryDetailId));
            }
        }
        private void SendAddendumRejectNotification(AddandomDetail addendumDetailModel, int userId)
        {
            string InitiatorUserName = string.Empty;
            string InitiatorUserEmail = string.Empty;
            string TreasuryUserEmailId = string.Empty;
            string TreasuryUserName = string.Empty;
            string CBUserMailId = string.Empty;
            string AllocationNo = string.Empty;
            string RejectionComment = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == addendumDetailModel.CreatedBy);
                if (user != null)
                {
                    InitiatorUserName = user.Name;
                    InitiatorUserEmail = user.EmailId;
                }
                var Treasury = (from T in suzlonBPPEntities.TreasuryDetails
                                join TW in suzlonBPPEntities.TreasuryWorkflows
                                on T.SubVerticalId equals TW.SubVerticalId
                                join U in suzlonBPPEntities.Users
                                on TW.PriTreasuryUserId equals U.UserId
                                where T.TreasuryDetailId == addendumDetailModel.TreasuryDetailId
                                select new
                                {
                                    U.EmailId,
                                    U.Name,
                                    T.AllocationNumber
                                }).FirstOrDefault();

                if (Treasury != null)
                {
                    TreasuryUserEmailId = Treasury.EmailId;
                    TreasuryUserName = Treasury.Name;
                    AllocationNo = Treasury.AllocationNumber;
                }

                var comments= suzlonBPPEntities.AddendumComments.OrderByDescending(C=>C.CreatedOn).FirstOrDefault(C=>C.AddendumId == addendumDetailModel.AddandomDetailId && C.TreasuryId== addendumDetailModel.TreasuryDetailId && C.WorkFlowStatusId==4);
                if (comments != null)
                    RejectionComment = comments.Comment;
                NotificationModel.SendAddendumRejectNotification(AllocationNo, TreasuryUserName,Convert.ToString(RejectionComment), InitiatorUserEmail,Convert.ToString(addendumDetailModel.TreasuryDetailId));
            }
        }
        private void SendAddendumReworkNotification(AddandomDetail addendumDetailModel, int userId)
        {
            string InitiatorUserName = string.Empty;
            string InitiatorUserEmail = string.Empty;
            string TreasuryUserEmailId = string.Empty;
            string TreasuryUserName = string.Empty;
            string CBUserMailId = string.Empty;
            string AllocationNo = string.Empty;
            string CorrectionComment = string.Empty;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var user = suzlonBPPEntities.Users.FirstOrDefault(U => U.UserId == addendumDetailModel.CreatedBy);
                if (user != null)
                {
                    InitiatorUserName = user.Name;
                    InitiatorUserEmail = user.EmailId;
                }
                var Treasury = (from T in suzlonBPPEntities.TreasuryDetails
                                join TW in suzlonBPPEntities.TreasuryWorkflows
                                on T.SubVerticalId equals TW.SubVerticalId
                                join U in suzlonBPPEntities.Users
                                on TW.PriTreasuryUserId equals U.UserId
                                where T.TreasuryDetailId == addendumDetailModel.TreasuryDetailId
                                select new
                                {
                                    U.EmailId,
                                    U.Name,
                                    T.AllocationNumber
                                }).FirstOrDefault();

                if (Treasury != null)
                {
                    TreasuryUserEmailId = Treasury.EmailId;
                    TreasuryUserName = Treasury.Name;
                    AllocationNo = Treasury.AllocationNumber;
                }
                var comments = suzlonBPPEntities.AddendumComments.OrderByDescending(C => C.CreatedOn).FirstOrDefault(C => C.AddendumId == addendumDetailModel.AddandomDetailId && C.TreasuryId == addendumDetailModel.TreasuryDetailId && C.WorkFlowStatusId == 5);
                if (comments != null)
                    CorrectionComment = comments.Comment;

                NotificationModel.SendAddendumReworkNotification(AllocationNo, Convert.ToString(CorrectionComment), InitiatorUserEmail, Convert.ToString(addendumDetailModel.TreasuryDetailId));
            }
        }

        #region "Report"

        public static List<usp_GetTreasuryReportData_Result> GetTreasuryReportData(int VerticalId, string AllocationNumber, DateTime FromDate, DateTime ToDate)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.usp_GetTreasuryReportData(VerticalId, AllocationNumber,FromDate,ToDate).ToList();
            }
        }

        public static List<usp_GetTreasuryPaymentReportData_Result> GetTreasuryPaymentReportData(string CompanyCode ,int VerticalId, string PaymentNumber,string TreasuryNo, DateTime FromDate, DateTime ToDate)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.usp_GetTreasuryPaymentReportData(CompanyCode,VerticalId, TreasuryNo, PaymentNumber, FromDate, ToDate).ToList();
            }
        }
        #endregion
    }





    public class TreasuryDetailModel
    {
        public int TreasuryDetailId { get; set; }
        public System.DateTime VendorCreatedOn { get; set; }
        public string CompanyCode { get; set; }
        public Int32 VerticalId { get; set; }
        public Int32 SubVerticalId { get; set; }
        public string AllocationNumber { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal InitApprovedAmount { get; set; }
        public decimal AddendumTotal { get; set; }
        public decimal FinalAmount { get; set; }
        public System.DateTime UtilsationStartDate { get; set; }
        public System.DateTime UtilsationEndDate { get; set; }
        public System.DateTime ProposedDate { get; set; }
        public string RequestType { get; set; }
        public decimal BalanceAmount { get; set; }
        public int WorkflowStatusID { get; set; }
        public string Status { get; set; }
        // public AddandomDetail CurrentAddandom { get; set; }
        public virtual List<TreasuryRequestModel> NatureOfRequest { get; set; }
        public virtual List<TreasuryCommentModel> verticalControllerComment { get; set; }
        // TBM
        public virtual List<FileUploadModel> FileUpload { get; set; }
        public string generateNextOffset()
        {
            String nextFinIndex;
            Int32 lastIndex = 0;
            Int32 nextIndex = 1;

            try
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    var latestVendorDetail = suzlonBPPEntities.TreasuryDetails.OrderByDescending(v => v.CreatedOn).FirstOrDefault();
                    DateTime cutoffDate = DateTime.Now;

                    if (latestVendorDetail != null)
                    {
                        if (Convert.ToDateTime(latestVendorDetail.CreatedOn).Month < 4)
                        {
                            cutoffDate = new DateTime(Convert.ToDateTime(latestVendorDetail.CreatedOn).Year, 4, 1);
                        }
                        else
                        {
                            cutoffDate = new DateTime(Convert.ToDateTime(latestVendorDetail.CreatedOn).Year + 1, 4, 1);
                        }
                    }


                    if (latestVendorDetail == null || String.IsNullOrEmpty(latestVendorDetail.AllocationNumber))
                    {
                        lastIndex = 0;
                    }
                    else if (DateTime.Now > cutoffDate)
                    {
                        lastIndex = 0;
                    }
                    else
                    {
                        lastIndex = Convert.ToInt32(latestVendorDetail.AllocationNumber.Substring(latestVendorDetail.AllocationNumber.Length - 6, 6));
                    }

                    nextIndex = Convert.ToInt32(lastIndex + 1);
                    nextFinIndex = Convert.ToString(nextIndex).PadLeft(6, '0');
                }
            }
            catch (Exception)
            {

                throw;
            }

            return nextFinIndex;
        }
        public AddandomDetail getCurrentAddendum(int Id)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                AddandumModel AddandumRequestModel = new AddandumModel();
                List<AddandomDetail> currentAddendum = suzlonBPPEntities.AddandomDetails.Where(v => (v.TreasuryDetailId == Id) && (v.AddandomWorkflowStatusId != 3 && v.AddandomWorkflowStatusId != 4)).ToList();
                return currentAddendum.OrderByDescending(v => v.CreatedOn).FirstOrDefault();
            }

        }

        public List<AddandumDisplayModel> getHistoryAddendum(int Id)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var vendorDetail = suzlonBPPEntities.TreasuryDetails.FirstOrDefault(v => v.TreasuryDetailId == Id);
                AddandumModel AddandumRequestModel = new AddandumModel();
                List<AddandumDisplayModel> historyAddendum = (from addandum in suzlonBPPEntities.AddandomDetails
                                                              join addandumStatus in suzlonBPPEntities.AddandomWorkflowStatus on addandum.AddandomWorkflowStatusId equals addandumStatus.AddandomWorkflowStausId
                                                              join naturofrequest in suzlonBPPEntities.NatureRequestMasters on addandum.NatureOfRequestId equals naturofrequest.RequestId
                                                              where addandum.TreasuryDetailId == Id && (addandum.AddandomWorkflowStatusId == 3 || addandum.AddandomWorkflowStatusId == 4)
                                                              orderby addandum.CreatedOn descending
                                                              select new AddandumDisplayModel
                                                              {
                                                                  Id= addandum.AddandomDetailId,
                                                                  AddandomWorkflowStatus = addandumStatus.Status,
                                                                  Amount = (Decimal)addandum.Amount,
                                                                  AddandomStatus = addandum.AddandomStatus,
                                                                  ApprovedAmount = (Decimal)addandum.ApprovedAmount,
                                                                  NatureOfRequest = naturofrequest.Name,
                                                                  Reason = addandum.Reason,
                                                                  AllocationNo = vendorDetail.AllocationNumber,
                                                                  CreatedOn = (DateTime)addandum.CreatedOn,
                                                              }).OrderByDescending(s => s.CreatedOn).ToList();

                return historyAddendum;
            }
        }

        public List<TreasuryBudgetUtilisationModel> getTresuryUtilizationDtl(int TreasuryDetailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                var utilisationList = (from uti in suzlonBPPEntities.TreasuryBudgetUtilisations
                                       join N in suzlonBPPEntities.NatureRequestMasters on uti.NatureOfReqestId equals N.RequestId
                                       where uti.TreasuryDetailId == TreasuryDetailId
                                       orderby uti.CreatedOn descending
                                       select new TreasuryBudgetUtilisationModel()
                                       {
                                           PaymentDate = uti.PaymentDate,
                                           NatureOfReqestId = uti.NatureOfReqestId,
                                           NatureOfReqest = N.Name,
                                           Amount = uti.Amount,
                                           AccountCode = uti.AccountCode,
                                           DocumentNo = uti.DocumentNo,
                                           AccountType = uti.AccountType,
                                           TreasuryBudgetUtilisationId = uti.TreasuryBudgetUtilisationId
                                       }).ToList();

                return utilisationList;
            }
        }

        public bool checkutilisationExist(TreasuryBudgetUtilisation BudUtilisation)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.TreasuryBudgetUtilisations.FirstOrDefault(b => b.PaymentDate == BudUtilisation.PaymentDate
                && b.NatureOfReqestId == BudUtilisation.NatureOfReqestId && b.AccountType == BudUtilisation.AccountType
                && b.AccountCode == BudUtilisation.AccountCode && b.DocumentNo == BudUtilisation.DocumentNo && b.TreasuryBudgetUtilisationId != BudUtilisation.TreasuryBudgetUtilisationId
                ) != null;
            }

        }

        public TreasuryBudgetUtilisation addUtilisation(TreasuryBudgetUtilisation BudUtilisation, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                BudUtilisation.CreatedBy = userId;
                BudUtilisation.CreatedOn = DateTime.Now;
                BudUtilisation.ModifiedBy = userId;
                BudUtilisation.ModifiedOn = DateTime.Now;
                suzlonBPPEntities.TreasuryBudgetUtilisations.Add(BudUtilisation);
                suzlonBPPEntities.SaveChanges();
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                suzlonBPPEntities.Configuration.LazyLoadingEnabled = false;
                return BudUtilisation;
            }
        }

        public bool updateUtilisation(TreasuryBudgetUtilisation BudUtilisation, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                TreasuryBudgetUtilisation utilisation = suzlonBPPEntities.TreasuryBudgetUtilisations.FirstOrDefault(u => u.TreasuryBudgetUtilisationId == BudUtilisation.TreasuryBudgetUtilisationId);
                if (utilisation != null)
                {
                    utilisation.AccountCode = BudUtilisation.AccountCode;
                    utilisation.AccountType = BudUtilisation.AccountType;
                    utilisation.DocumentNo = BudUtilisation.DocumentNo;
                    utilisation.PaymentDate = BudUtilisation.PaymentDate;
                    utilisation.NatureOfReqestId = BudUtilisation.NatureOfReqestId;
                    utilisation.Amount = BudUtilisation.Amount;
                    utilisation.ModifiedBy = userId;
                    utilisation.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(utilisation).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool deleteutilisation(int BudUtilisationid)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.TreasuryBudgetUtilisations.RemoveRange(suzlonBPPEntities.TreasuryBudgetUtilisations.Where(u => u.TreasuryBudgetUtilisationId == BudUtilisationid).ToList());
                suzlonBPPEntities.SaveChanges();
                return true;
            }
        }

        public static List<ListItem> GetTreasuryCompanyCodeNumber(int VerticalId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<string> lstTrasury = suzlonBPPEntities.TreasuryDetails.Select(t => t.CompanyCode).Distinct().ToList();
                List<ListItem> lstCompanyCode = new List<ListItem>();
                lstCompanyCode.Add(new ListItem() { Id = "All", Name = "All" });

                if (lstTrasury.Count > 0)
                    lstTrasury.ForEach(l => lstCompanyCode.Add(new ListItem() { Id = l.ToString(), Name = l.ToString() }));
                return lstCompanyCode;
            }
        }

        public static List<ListItem> GetTreasuryPaymentLotNumber()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<string> lstPaymentDetails = suzlonBPPEntities.PaymentDetailAgainstBills.Where(l => l.ApprovedLineGroupID != null).Select(t => t.ApprovedLineGroupID).Distinct().ToList();
                List<ListItem> lstPaymentLotNo = new List<ListItem>();
                lstPaymentLotNo.Add(new ListItem() { Id = "All", Name = "All" });

                if (lstPaymentDetails.Count > 0)
                    lstPaymentDetails.ForEach(l => lstPaymentLotNo.Add(new ListItem() { Id = l.ToString(), Name = l.ToString() }));
                return lstPaymentLotNo;
            }
        }
        public static List<ListItem> GetTreasuryAllocationNumber(int VerticalId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                List<TreasuryDetail> lstTreasury = suzlonBPPEntities.TreasuryDetails.Where(t => t.VerticalId == VerticalId).Distinct().ToList();
                List<ListItem> lstAllocationNumber = new List<ListItem>();
                lstAllocationNumber.Add(new ListItem() { Id = "All", Name = "All" });

                if (lstTreasury.Count > 0)
                    lstTreasury.ForEach(l => lstAllocationNumber.Add(new ListItem() { Id = l.AllocationNumber, Name = l.AllocationNumber }));
                return lstAllocationNumber;
            }
        }
        
    }

    public class FileUploadModel
    {
        public int FileUploadId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string FileName { get; set; }
        public string DisplayName { get; set; }
    }
    public partial class TreasuryBudgetUtilisationModel
    {
        public int TreasuryBudgetUtilisationId { get; set; }
        public int TreasuryDetailId { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public int NatureOfReqestId { get; set; }
        public string NatureOfReqest { get; set; }
        public decimal Amount { get; set; }
        public string AccountType { get; set; }
        public string AccountCode { get; set; }
        public string DocumentNo { get; set; }

    }

    public class TreasuryRequestModel
    {
        public int TreasuryRequestId { get; set; }
        public int natureOfRequest { get; set; }
        public string natureOfRequestText { get; set; }
        public decimal requestedAmount { get; set; }
        public decimal approvedAmount { get; set; }
        public string isDirty { get; set; }
    }


    public class TreasuryCommentModel
    {
        public string Comment { get; set; }
        public string CommentBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class AddandumModel
    {
        public int Id { get; set; }
        public int TreasuryDetailId { get; set; }
        public decimal Amount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public string AddandomStatus { get; set; }
        public string Reason { get; set; }
        public int NatureOfRequestId { get; set; }
        public int AddandomWorkflowStatusId { get; set; }
        public DateTime AddandomDate { get; set; }
        public string Comment { get; set; }
        public decimal UpdateAddandumAmount(Int32 TreasuryId)
        {
            decimal totalApprovedAddandumAmount = 0;
            decimal finalAmount = 0;
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                AddandumModel AddandumRequestModel = new AddandumModel();
                TreasuryDetail treasureDetails = suzlonBPPEntities.TreasuryDetails.FirstOrDefault(u => u.TreasuryDetailId == TreasuryId);

                // List<AddandomDetail> test = suzlonBPPEntities.AddandomDetails.Where(v => (v.TreasuryDetailId == TreasuryId)).ToList();
                List<AddandomDetail> rec = suzlonBPPEntities.AddandomDetails.Where(v => (v.TreasuryDetailId == TreasuryId) && (v.AddandomWorkflowStatusId == 3)).ToList();
                if (rec.Count() > 0)
                {
                    totalApprovedAddandumAmount = (Decimal)suzlonBPPEntities.AddandomDetails.Where(v => (v.TreasuryDetailId == TreasuryId) && (v.AddandomWorkflowStatusId == 3)).Sum(v => v.ApprovedAmount);
                    finalAmount = totalApprovedAddandumAmount + (Decimal)treasureDetails.InitApprovedAmount;
                    treasureDetails.AddendumTotal = Convert.ToDecimal(String.Format("{0:0.00}", totalApprovedAddandumAmount));
                    treasureDetails.FinalAmount = Convert.ToDecimal(String.Format("{0:0.00}", finalAmount));
                    suzlonBPPEntities.SaveChanges();
                }
            }
            return totalApprovedAddandumAmount;
        }
    }

    public class AddandumDisplayModel
    {
        public int Id { get; set; }
        public int TreasuryDetailId { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedOn { get; set; }
        public decimal ApprovedAmount { get; set; }
        public string AddandomStatus { get; set; }
        public string Reason { get; set; }
        public string NatureOfRequest { get; set; }
        public string AddandomWorkflowStatus { get; set; }
        public string AllocationNo { get; set; }
    }
}
