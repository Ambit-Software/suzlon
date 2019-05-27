using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                    treasuryWorkflow.WorkFlowId = workflow.WorkFlowId;
                    treasuryWorkflow.CreatedBy = workflow.CreatedBy;
                    treasuryWorkflow.CreatedOn = workflow.CreatedOn;
                    treasuryWorkflow.ModifiedBy = userId;
                    treasuryWorkflow.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(treasuryWorkflow).State = EntityState.Modified;
                }
                suzlonBPPEntities.SaveChanges();
                isSave = true;
            }
            return isSave;
        }


        //TreasuryWorkflowModel
        public VendorControllerDetailModel AddVendorControllerDetail(VendorControllerDetailModel vendorControllerDetailModel, int userId)
        {
            VerticalControllerDetail verticalControllerDetails = new VerticalControllerDetail();
            SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities();


            verticalControllerDetails.CompanyCode = vendorControllerDetailModel.CompanyCode;
            verticalControllerDetails.CreatedBy = userId;
            verticalControllerDetails.CreatedOn = DateTime.Now;
            verticalControllerDetails.ModifiedBy = userId;
            verticalControllerDetails.ModifiedOn = DateTime.Now;
            verticalControllerDetails.VendorCreatedOn = vendorControllerDetailModel.VendorCreatedOn;
            verticalControllerDetails.SubVerticalId = vendorControllerDetailModel.SubVerticalId;
            verticalControllerDetails.VerticalId = vendorControllerDetailModel.VerticalId;
            verticalControllerDetails.AllocationNumber = vendorControllerDetailModel.AllocationNumber + "-" + vendorControllerDetailModel.generateNextOffset();
            verticalControllerDetails.RequestedAmount = vendorControllerDetailModel.RequestedAmount;
            verticalControllerDetails.InitApprovedAmount = vendorControllerDetailModel.InitApprovedAmount;
            verticalControllerDetails.AddendumTotal = vendorControllerDetailModel.AddendumTotal;
            verticalControllerDetails.FinalAmount = vendorControllerDetailModel.FinalAmount;
            verticalControllerDetails.UtilsationStartDate = vendorControllerDetailModel.UtilsationStartDate;
            verticalControllerDetails.UtilsationEndDate = vendorControllerDetailModel.UtilsationEndDate;
            verticalControllerDetails.ProposedDate = vendorControllerDetailModel.ProposedDate;
            verticalControllerDetails.RequestType = vendorControllerDetailModel.RequestType;
            verticalControllerDetails.BalanceAmount = vendorControllerDetailModel.BalanceAmount;
            verticalControllerDetails.WorkflowStatusId = vendorControllerDetailModel.WorkflowStatusID;
            verticalControllerDetails.Status = vendorControllerDetailModel.Status;

            suzlonBPPEntities.VerticalControllerDetails.Add(verticalControllerDetails);
            suzlonBPPEntities.SaveChanges();

            if ((vendorControllerDetailModel.NatureOfRequest != null && vendorControllerDetailModel.NatureOfRequest.Count > 0))
            {
                vendorControllerDetailModel.NatureOfRequest.ForEach(request =>
                {
                    VerticalControllerVendorReqest requestModel = new VerticalControllerVendorReqest();
                    requestModel.VerticalControllerId = verticalControllerDetails.VerticalControllerId;
                    requestModel.NatureOfReqestId = request.natureOfRequest;
                    requestModel.NatureOfReqest = request.natureOfRequestText;
                    requestModel.ApprovedAmount = request.approvedAmount;
                    requestModel.RequestedAmount = request.requestedAmount;
                    requestModel.CreatedBy = userId;
                    requestModel.CreatedOn = DateTime.Now;
                    requestModel.ModifiedBy = userId;
                    requestModel.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.VerticalControllerVendorReqests.Add(requestModel);
                    suzlonBPPEntities.SaveChanges();
                });
            }

            if ((vendorControllerDetailModel.verticalControllerComment != null && vendorControllerDetailModel.verticalControllerComment.Count > 0))
            {
                vendorControllerDetailModel.verticalControllerComment.ForEach(request =>
                {
                    VerticalControllerComment requestModel = new VerticalControllerComment();
                    requestModel.VerticalControllerId = verticalControllerDetails.VerticalControllerId;
                    requestModel.Comment = request.Comment;
                    requestModel.CreatedBy = userId;
                    requestModel.CreatedOn = DateTime.Now;
                    requestModel.ModifiedBy = userId;
                    requestModel.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.VerticalControllerComments.Add(requestModel);
                    suzlonBPPEntities.SaveChanges();
                });
            }

            return vendorControllerDetailModel;
        }

        public VendorControllerDetailModel EditVendorControllerDetail(VendorControllerDetailModel vendorControllerDetailModel, int userId)
        {


            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                VerticalControllerDetail verticalControllerDetails = suzlonBPPEntities.VerticalControllerDetails.FirstOrDefault(u => u.VerticalControllerId == vendorControllerDetailModel.VendorControllerId);
                verticalControllerDetails.CompanyCode = vendorControllerDetailModel.CompanyCode;
                verticalControllerDetails.ModifiedBy = userId;
                verticalControllerDetails.ModifiedOn = DateTime.Now;
                verticalControllerDetails.VendorCreatedOn = vendorControllerDetailModel.VendorCreatedOn;
                verticalControllerDetails.SubVerticalId = vendorControllerDetailModel.SubVerticalId;
                verticalControllerDetails.VerticalId = vendorControllerDetailModel.VerticalId;
                verticalControllerDetails.AllocationNumber = vendorControllerDetailModel.AllocationNumber;
                verticalControllerDetails.RequestedAmount = vendorControllerDetailModel.RequestedAmount;
                verticalControllerDetails.InitApprovedAmount = vendorControllerDetailModel.InitApprovedAmount;
                verticalControllerDetails.AddendumTotal = vendorControllerDetailModel.AddendumTotal;
                verticalControllerDetails.FinalAmount = vendorControllerDetailModel.FinalAmount;
                verticalControllerDetails.UtilsationStartDate = vendorControllerDetailModel.UtilsationStartDate;
                verticalControllerDetails.UtilsationEndDate = vendorControllerDetailModel.UtilsationEndDate;
                verticalControllerDetails.ProposedDate = vendorControllerDetailModel.ProposedDate;
                verticalControllerDetails.RequestType = vendorControllerDetailModel.RequestType;
                verticalControllerDetails.BalanceAmount = vendorControllerDetailModel.BalanceAmount;
                verticalControllerDetails.WorkflowStatusId = vendorControllerDetailModel.WorkflowStatusID;
                verticalControllerDetails.Status = vendorControllerDetailModel.Status;
                suzlonBPPEntities.SaveChanges();

                List<VendorControllerRequestModel> newRequest = vendorControllerDetailModel.NatureOfRequest.Where(r => r.Id == 0).ToList();
                if ((newRequest != null && newRequest.Count > 0))
                {
                    newRequest.ForEach(request =>
                        {
                            VerticalControllerVendorReqest requestModel = new VerticalControllerVendorReqest();
                            requestModel.VerticalControllerId = verticalControllerDetails.VerticalControllerId;
                            requestModel.NatureOfReqestId = request.natureOfRequest;
                            requestModel.NatureOfReqest = request.natureOfRequestText;
                            requestModel.ApprovedAmount = request.approvedAmount;
                            requestModel.RequestedAmount = request.requestedAmount;
                            requestModel.CreatedBy = userId;
                            requestModel.CreatedOn = DateTime.Now;
                            requestModel.ModifiedBy = userId;
                            requestModel.ModifiedOn = DateTime.Now;
                            suzlonBPPEntities.VerticalControllerVendorReqests.Add(requestModel);
                            suzlonBPPEntities.SaveChanges();
                        });
                }

                List<VendorControllerRequestModel> updateRequest = vendorControllerDetailModel.NatureOfRequest.Where((r => r.Id != 0 && r.isDirty == "T")).ToList();
                if ((updateRequest != null && updateRequest.Count > 0))
                {
                    updateRequest.ForEach(request =>
                    {
                        VerticalControllerVendorReqest currentRequest = suzlonBPPEntities.VerticalControllerVendorReqests.FirstOrDefault(u => u.VerticalControllerId == vendorControllerDetailModel.VendorControllerId);
                        currentRequest.VerticalControllerId = verticalControllerDetails.VerticalControllerId;
                        currentRequest.NatureOfReqestId = request.natureOfRequest;
                        currentRequest.NatureOfReqest = request.natureOfRequestText;
                        currentRequest.ApprovedAmount = request.approvedAmount;
                        currentRequest.RequestedAmount = request.requestedAmount;
                        currentRequest.ModifiedBy = userId;
                        currentRequest.ModifiedOn = DateTime.Now;
                        suzlonBPPEntities.SaveChanges();
                    });
                }

                if ((vendorControllerDetailModel.verticalControllerComment != null && vendorControllerDetailModel.verticalControllerComment.Count > 0))
                {
                    vendorControllerDetailModel.verticalControllerComment.ForEach(request =>
                    {
                        VerticalControllerComment requestModel = new VerticalControllerComment();
                        requestModel.VerticalControllerId = verticalControllerDetails.VerticalControllerId;
                        requestModel.Comment = request.Comment;
                        requestModel.CreatedBy = userId;
                        requestModel.CreatedOn = DateTime.Now;
                        requestModel.ModifiedBy = userId;
                        requestModel.ModifiedOn = DateTime.Now;
                        suzlonBPPEntities.VerticalControllerComments.Add(requestModel);
                        suzlonBPPEntities.SaveChanges();
                    });
                }
            }

            return vendorControllerDetailModel;
        }

        public dynamic GetVendorControllerDetail(int VendorDetailId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var vendorDetail = suzlonBPPEntities.VerticalControllerDetails.FirstOrDefault(v => v.VerticalControllerId == VendorDetailId);
                var VendorDetail = (from verticalController in suzlonBPPEntities.VerticalControllerDetails
                                    where verticalController.VerticalControllerId == VendorDetailId
                                    select new
                                    {
                                        verticalController.VerticalControllerId,
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
                                        NatureOfRequest = (from request in suzlonBPPEntities.VerticalControllerVendorReqests
                                                           where request.VerticalControllerId == VendorDetailId
                                                           select new VendorControllerRequestModel
                                                           {
                                                               Id = request.NatureOfReqestId,
                                                               natureOfRequest = request.NatureOfReqestId,
                                                               natureOfRequestText = request.NatureOfReqest,
                                                               approvedAmount = (decimal)request.ApprovedAmount,
                                                               requestedAmount = (decimal)request.RequestedAmount
                                                           }).ToList(),

                                        VerticalControllerComment = (from comment in suzlonBPPEntities.VerticalControllerComments
                                                                     join user in suzlonBPPEntities.Users on comment.CreatedBy equals user.UserId
                                                                     where comment.VerticalControllerId == VendorDetailId
                                                                     // && (isLoggedInUserVendor ? (comment.CreatedBy == userId || comment.BankWorkflowStatusId == (int)WorkFlowStatusEnum.NeedCorrectionByVendor) : true)
                                                                     select new BankCommentModel { Comment = comment.Comment, CommentBy = user.Name }).ToList(),
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
            suzlonBPPEntities.AddandomDetails.Add(addandumDetails);
            suzlonBPPEntities.SaveChanges();
            addandumDetailModel.UpdateAddandumAmount(addandumDetailModel.TreasuryDetailId);


            return addandumDetailModel;
        }

        public AddandumModel EditAddandumdDetails(AddandumModel addandumDetailModel, int userId)
        {

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                AddandomDetail addandumDetails = suzlonBPPEntities.AddandomDetails.FirstOrDefault(u => u.AddandomDetailId == addandumDetailModel.Id);
                addandumDetails.AddandomStatus = addandumDetailModel.AddandomStatus;
                addandumDetails.ModifiedBy = userId;
                addandumDetails.ModifiedOn = DateTime.Now;
                addandumDetails.AddandomWorkflowStatusId = addandumDetailModel.AddandomWorkflowStatusId;
                addandumDetails.Amount = addandumDetailModel.Amount;
                addandumDetails.ApprovedAmount = addandumDetailModel.ApprovedAmount;
                addandumDetails.NatureOfRequestId = addandumDetailModel.NatureOfRequestId;
                addandumDetails.Reason = addandumDetailModel.Reason;
                addandumDetails.TreasuryDetailId = addandumDetailModel.TreasuryDetailId;
                suzlonBPPEntities.SaveChanges();
                addandumDetailModel.UpdateAddandumAmount(addandumDetailModel.TreasuryDetailId);
            }

            return addandumDetailModel;
        }
    }



    public class VendorControllerDetailModel
    {
        public int VendorControllerId { get; set; }
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
        public virtual List<VendorControllerRequestModel> NatureOfRequest { get; set; }
        public virtual List<VendorControlleCommentModel> verticalControllerComment { get; set; }
        public string generateNextOffset()
        {
            String nextFinIndex;
            Int32 lastIndex = 0;
            Int32 nextIndex = 1;

            try
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    var latestVendorDetail = suzlonBPPEntities.VerticalControllerDetails.OrderByDescending(v => v.CreatedOn).FirstOrDefault();

                    if ((Convert.ToDateTime(latestVendorDetail.CreatedOn).Month == 4
                        && Convert.ToDateTime(latestVendorDetail.CreatedOn).Year == DateTime.Now.Year) ||
                        (latestVendorDetail == null || String.IsNullOrEmpty(latestVendorDetail.AllocationNumber)))
                    {
                        lastIndex = 0;
                    }
                    else
                    {
                        lastIndex = Convert.ToInt32(latestVendorDetail.AllocationNumber.Substring(latestVendorDetail.AllocationNumber.Length - 6, 6));
                    }

                    nextIndex = Convert.ToInt32(lastIndex + 1);
                    nextFinIndex = Convert.ToString(nextIndex).PadLeft(7 - Convert.ToString(nextIndex + 1).Length, '0');
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
                var vendorDetail = suzlonBPPEntities.VerticalControllerDetails.FirstOrDefault(v => v.VerticalControllerId == Id);
                AddandumModel AddandumRequestModel = new AddandumModel();
                List<AddandumDisplayModel> historyAddendum = (from addandum in suzlonBPPEntities.AddandomDetails
                                                              join addandumStatus in suzlonBPPEntities.AddandomWorkflowStatus on addandum.AddandomWorkflowStatusId equals addandumStatus.AddandomWorkflowStausId
                                                              join naturofrequest in suzlonBPPEntities.NatureRequestMasters on addandum.NatureOfRequestId equals naturofrequest.RequestId
                                                              where addandum.TreasuryDetailId == Id && (addandum.AddandomWorkflowStatusId == 3 || addandum.AddandomWorkflowStatusId == 4)
                                                              orderby addandum.CreatedOn descending
                                                              select new AddandumDisplayModel
                                                              {
                                                                  AddandomWorkflowStatus = addandumStatus.Status,
                                                                  Amount = (Decimal)addandum.Amount,
                                                                  AddandomStatus = addandum.AddandomStatus,
                                                                  ApprovedAmount = (Decimal)addandum.ApprovedAmount,
                                                                  NatureOfRequest = naturofrequest.Name,
                                                                  Reason = addandum.Reason,
                                                                  AllocationNo = vendorDetail.AllocationNumber,
                                                                  CreatedOn = (DateTime)addandum.CreatedOn,
                                                              }).ToList();

                return historyAddendum;
            }
        }
    }
}

public class VendorControllerRequestModel
{
    public int Id { get; set; }
    public int natureOfRequest { get; set; }
    public string natureOfRequestText { get; set; }
    public decimal requestedAmount { get; set; }
    public decimal approvedAmount { get; set; }
    public string isDirty { get; set; }
}

public class VendorControlleCommentModel
{
    public string Comment { get; set; }
    public string CommentBy { get; set; }
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
    public decimal UpdateAddandumAmount(Int32 TreasuryId)
    {
        decimal totalApprovedAddandumAmount = 0;
        decimal finalAmount = 0;
        using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
        {
            AddandumModel AddandumRequestModel = new AddandumModel();
            VerticalControllerDetail verticalControllerDetails = suzlonBPPEntities.VerticalControllerDetails.FirstOrDefault(u => u.VerticalControllerId == TreasuryId);

            List<AddandomDetail> test = suzlonBPPEntities.AddandomDetails.Where(v => (v.TreasuryDetailId == TreasuryId)).ToList();
            List<AddandomDetail> test1 = suzlonBPPEntities.AddandomDetails.Where(v => (v.TreasuryDetailId == TreasuryId) && (v.AddandomWorkflowStatusId == 3)).ToList();

            totalApprovedAddandumAmount = (Decimal)suzlonBPPEntities.AddandomDetails.Where(v => (v.TreasuryDetailId == TreasuryId) && (v.AddandomWorkflowStatusId == 3)).Sum(v => v.ApprovedAmount);
            finalAmount = totalApprovedAddandumAmount + (Decimal)verticalControllerDetails.InitApprovedAmount;
            verticalControllerDetails.AddendumTotal = totalApprovedAddandumAmount;
            verticalControllerDetails.FinalAmount = finalAmount;
            suzlonBPPEntities.SaveChanges();
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
