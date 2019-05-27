using System;
using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Collections;
using System.Linq; // TBM
using System.IO;
using System.Configuration;

namespace SuzlonBPP.Controllers
{
    [Authorize]
    [RoutePrefix("api/workflow")]
    public class PaymentWorkflowController : BaseApiController
    {

        #region "Payment Workflow"

        [HttpGet]
        // GET: api/workflow/getpaymentworkflow
        [Route("getAdvancePaymentDetails")]
        public IHttpActionResult getAdvancePaymentDetails(string companycode, string vendorcode,string Type)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.getAdvancePaymentDetails(companycode, vendorcode,Type, UserId));
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailsbyifsc
        [Route("getAgainstBillPaymentDetails")]
        public IHttpActionResult getAgainstBillPaymentDetails(string companycode, string vendorcode,string Type)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.getAgainstBillPaymentDetails(companycode, vendorcode, Type,UserId));

        }

        // POST: api/workflow/UpdateAgainstBillPayment
        [Route("UpdateAgainstBillPayment")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateAgainstBillPayment(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            SAPAgainstBillPayment sapAgainstBillPayment = JsonConvert.DeserializeObject<Models.SAPAgainstBillPayment>(paramDetail);
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool isUpdated = paymentWorkflowModel.UpdateAgainstBillPayment(sapAgainstBillPayment);
            return Ok(isUpdated);

        }

        // POST: api/workflow/UpdateAgainstBillPayment
        [Route("UpdateAdvanceBillPayment")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateAdvanceBillPayment(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            SAPAdvancePayment sapAdvancePayment = JsonConvert.DeserializeObject<Models.SAPAdvancePayment>(paramDetail);
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool isUpdated = paymentWorkflowModel.UpdateAdvanceBillPayment(sapAdvancePayment);
            return Ok(isUpdated);
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailsbyifsc
        [Route("GetAgainstBillPay")]
        public IHttpActionResult GetAgainstBillPay(int Id)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.GetAgainstBillPay(Id));

        }

        [HttpGet]
        // GET: api/workflow/getbankdetailsbyifsc
        [Route("GetAdvancePay")]
        public IHttpActionResult GetAdvancePay(int Id)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.GetAdvancePay(Id));

        }

        // POST: api/workflow/UpdateAgainstBillPayment
        [Route("InsertAgainstBillPayment")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult InsertAgainstBillPayment(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            PaymentDetailAgainstBill paymentDetailAgainstBill = JsonConvert.DeserializeObject<Models.PaymentDetailAgainstBill>(paramDetail);
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool isInserted = paymentWorkflowModel.InsertAgainstBillPayment(paymentDetailAgainstBill);
            return Ok(isInserted);
        }

        [Route("InsertAdvancePayment")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult InsertAdvancePayment(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            PaymentDetailAdvance paymentDetailAdvance = JsonConvert.DeserializeObject<Models.PaymentDetailAdvance>(paramDetail);
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool isInserted = paymentWorkflowModel.InsertAdvancePayment(paymentDetailAdvance);
            return Ok(isInserted);
        }

        [HttpGet]
        // GET: api/workflow/GetInitiatorAgainstBillInProcessData
        [Route("GetInitiatorAgainstBillInProcessData")]
        public List<sp_GetInitiator_AgainstBill_InProcess_Data_Result> GetInitiatorAgainstBillInProcessData(string VendorCode, string CompanyCode)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<sp_GetInitiator_AgainstBill_InProcess_Data_Result> Initiator_AgainstBill_InProcess_Result = paymentWorkflowModel.GetInitiatorAgainstBillInProcessData(VendorCode, CompanyCode);
            return Initiator_AgainstBill_InProcess_Result;
        }


        [HttpGet]
        // GET: api/workflow/GetInitiatorAdvanceInProcessData
        [Route("GetInitiatorAdvanceInProcessData")]
        public List<sp_GetInitiator_Advance_InProcess_Data_Result> GetInitiatorAdvanceInProcessData(string VendorCode, string CompanyCode)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<sp_GetInitiator_Advance_InProcess_Data_Result> Initiator_Advance_InProcess_Result = paymentWorkflowModel.GetInitiatorAdvanceInProcessData(VendorCode, CompanyCode);
            return Initiator_Advance_InProcess_Result;
        }

        [HttpGet]
        // GET: api/workflow/GetInitiatorAgainstBillInProcessData
        [Route("GetInitiatorAgainstBillCorrectionData")]
        public List<sp_GetInitiator_AgainstBill_Correction_Data_Result> GetInitiatorAgainstBillCorrectionData(string VendorCode, string CompanyCode)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<sp_GetInitiator_AgainstBill_Correction_Data_Result> Initiator_AgainstBill_Correction_Result = paymentWorkflowModel.GetInitiatorAgainstBillCorrectionData(VendorCode, CompanyCode);
            return Initiator_AgainstBill_Correction_Result;
        }

        [HttpGet]
        // GET: api/workflow/GetInitiatorAdvanceInProcessData
        [Route("GetInitiatorAdvanceCorrectionData")]
        public List<sp_GetInitiator_Advance_Correction_Data_Result> GetInitiatorAdvanceCorrectionData(string VendorCode, string CompanyCode)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<sp_GetInitiator_Advance_Correction_Data_Result> Initiator_Advance_Correction_Result = paymentWorkflowModel.GetInitiatorAdvanceCorrectionData(VendorCode, CompanyCode);
            return Initiator_Advance_Correction_Result;
        }

        [Route("AgainstBillTransaction")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult AgainstBillTransaction(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ArrayList values = JsonConvert.DeserializeObject<ArrayList>(paramDetail);

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool result = paymentWorkflowModel.AgainstBillTransaction(values);
            return Ok(result);
        }

        [Route("AdvancePaymentTransaction")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult AdvancePaymentTransaction(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ArrayList values = JsonConvert.DeserializeObject<ArrayList>(paramDetail);

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool result = paymentWorkflowModel.AdvancePaymentTransaction(values);
            return Ok(result);
        }

        [Route("AgainstBillCorrection")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult AgainstBillCorrection(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ArrayList values = JsonConvert.DeserializeObject<ArrayList>(paramDetail);

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool result = paymentWorkflowModel.AgainstBillCorrection(values);
            return Ok(result);
        }

        [Route("AdvancePaymentCorrection")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult AdvancePaymentCorrection(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ArrayList values = JsonConvert.DeserializeObject<ArrayList>(paramDetail);

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool result = paymentWorkflowModel.AdvancePaymentCorrection(values);
            return Ok(result);
        }

        [HttpGet]
        // GET: api/workflow/getpaymentworkflow
        [Route("GetCommentsForMyRequest")]
        public IHttpActionResult GetCommentsForMyRequest(bool needCorrection)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.GetCommentsForMyRequest(needCorrection));
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailsbyifsc
        [Route("GetCommentsForAdvanceMyRequest")]
        public IHttpActionResult GetCommentsForAdvanceMyRequest(bool needCorrection)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.GetCommentsForAdvanceMyRequest(needCorrection));

        }

        [HttpGet]
        // GET: api/workflow/GetPendingReqByVerticalID
        [Route("GetPendingReqByVerticalID")]
        public List<GetVendorData_Result> GetPendingReqByVerticalID(string VerticalID)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<GetVendorData_Result> lstGetVendorData_Result = paymentWorkflowModel.GetPendingReqByVerticalID(Convert.ToInt32(VerticalID));
            return lstGetVendorData_Result;
        }

        [HttpGet]
        // GET: api/workflow/GetVendorReqByVerticalID_Adv
        [Route("GetVendorReqByVerticalID_Adv")]
        public List<GetApproverVendorData_Result> GetVendorReqByVerticalID(string VerticalID, string UserId, string BillType, string TabName)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<GetApproverVendorData_Result> lstGetVendorData_Result = paymentWorkflowModel.GetApproverVendorData(Convert.ToInt32(VerticalID), Convert.ToInt32(UserId), BillType, TabName);
            return lstGetVendorData_Result;
        }
        [HttpGet]
        // GET: api/workflow/GetCompaniesReqByVendor_Adv
        [Route("GetCompaniesReqByVendor_Adv")]
        public List<GetApproverCompByVendor_Result> GetCompaniesReqByVendor(string VerticalID, string VendorCode, string UserId, string BillType, string TabName)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<GetApproverCompByVendor_Result> lstGetCompanyData_Result = paymentWorkflowModel.GetApproverCompByVendor(Convert.ToInt32(VerticalID), VendorCode, Convert.ToInt32(UserId), BillType, TabName);
            return lstGetCompanyData_Result;
        }

        [HttpGet]
        // GET: api/workflow/GetApproverEndItemsAdvance
        [Route("GetApproverEndItemsAdvance")]
        public List<GetApproverEndItems_Result> GetApproverEndItems(string VerticalID, string VendorCode, string CompanyCode, string UserId, string BillType, string TabName)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<GetApproverEndItems_Result> lstGetItems_Result = paymentWorkflowModel.GetApproverEndItems(Convert.ToInt32(VerticalID), VendorCode, CompanyCode, Convert.ToInt32(UserId), BillType, TabName);
            return lstGetItems_Result;
        }

        public int GetPaymentWorkflowStatusId(int StatusId, int ProfileId)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetPaymentWorkflowStatusId(StatusId, ProfileId);
        }

        public int GetPaymentWorkflowStatusId(string Status)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetPaymentWorkflowStatusId(Status);
        }

        [HttpGet]
        // GET: api/workflow/GetPendingReqByVerticalID
        [Route("getPendingCompanyDataForFnA")]
        public IHttpActionResult GetPendingCompanyDataForFnA(int VerticalID)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            var details = paymentWorkflowModel.GetPendingCompanyDataForFnA(VerticalID);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getPendingVendorDataForFnA
        [Route("getPendingVendorDataForFnA")]
        public IHttpActionResult GetPendingVendorDataForFnA(int VerticalID, string CompanyId)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            var details = paymentWorkflowModel.GetPendingVendorDataForFnA(VerticalID, CompanyId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getPendingVendorDataForFnA
        [Route("getPendingVendorItemsDataForFnA")]
        public IHttpActionResult GetPendingVendorItemsDataForFnA(int VerticalID, string CompanyId, string VendorId)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            var details = paymentWorkflowModel.GetPendingVendorItemsDataForFnA(VerticalID, CompanyId, VendorId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/GetPendingReqByVerticalID
        [Route("getPendingCompanyDataAgaBillForFnA")]
        public IHttpActionResult GetPendingCompanyDataAgaBillForFnA(int VerticalID)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            var details = paymentWorkflowModel.GetPendingCompanyDataAgainstBillForFnA(VerticalID);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getPendingVendorDataForFnA
        [Route("getPendingVendorDataAgaBillForFnA")]
        public IHttpActionResult GetPendingVendorDataAgaBillForFnA(int VerticalID, string CompanyId)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            var details = paymentWorkflowModel.GetPendingVendorDataAgainstBillForFnA(VerticalID, CompanyId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getPendingVendorDataForFnA
        [Route("getPendingVendorItemsDataAgaBillForFnA")]
        public IHttpActionResult GetPendingVendorItemsDataAgaBillForFnA(int VerticalID, string CompanyId, string VendorId)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            var details = paymentWorkflowModel.GetPendingVendorItemsDataAgainstBillForFnA(VerticalID, CompanyId, VendorId);
            return Ok(details);
        }



        public bool SavePaymentDetailTxn_Adv(List<PaymentDetailAdvanceTxn> Transactions)
        {
            bool isInserted = false;

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();

            isInserted = paymentWorkflowModel.SavePaymentDetailTxn_Adv(Transactions);

            return isInserted;
        }

        public bool SavePaymentAgainstDetailTxn_Adv(List<PaymentDetailAgainstBillTxn> Transactions)
        {
            bool isInserted = false;

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();

            isInserted = paymentWorkflowModel.SavePaymentAgainstDetailTxn_Adv(Transactions);

            return isInserted;
        }



        public string getNewApprovedGroupId()
        {
            // String nextFinIndex;
            //Int32 lastAdvIndex = 0;
            //Int32 lastAgtIndex = 0;
            //Int32 lastIndex = 0;
            //Int32 nextIndex = 1;
            String maxId; 
            try
            {
                using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                {
                    System.Data.Entity.Core.Objects.ObjectParameter maxLineGroupId = new System.Data.Entity.Core.Objects.ObjectParameter("maxLineGroupId", typeof(String));
                    suzlonBPPEntities.GetMaxApproveGroupLineItemNo(maxLineGroupId);
                    maxId = maxLineGroupId.Value.ToString();
                }
                //Int32 AdvCount = 0;
                //Int32 AgnCount = 0;
                //using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
                //{
                //    AdvCount = suzlonBPPEntities.PaymentDetailAdvances.ToList().Count();
                //    AgnCount = suzlonBPPEntities.PaymentDetailAgainstBills.ToList().Count();

                //    if (AdvCount > 0)
                //    {
                //        lastAdvIndex = suzlonBPPEntities.PaymentDetailAdvances.ToList().Max(e => Convert.ToInt32(e.ApprovedLineGroupID));
                //    }
                //    if (AgnCount > 0)
                //    {
                //        lastAgtIndex = suzlonBPPEntities.PaymentDetailAgainstBills.ToList().Max(e => Convert.ToInt32(e.ApprovedLineGroupID));
                //    }

                //    if (AdvCount == 0 && lastAgtIndex == 0)
                //    {
                //        lastIndex = 0;
                //    }
                //    else
                //    {
                //        lastIndex = (lastAdvIndex > lastAgtIndex) ? lastAdvIndex : lastAgtIndex;
                //    }

                //    nextIndex = Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(lastIndex)) ? 0 : lastIndex + 1);
                //    nextFinIndex = Convert.ToString(nextIndex).PadLeft(6 - Convert.ToString(nextIndex + 1).Length, '0');
            }            
            catch (Exception ex)
            {

                throw;
            }

            return maxId;

        }

        public bool UpdateAmountInPaymentDetailAdvance(List<PaymentDetailAdvance> Transactions)
        {
            bool isInserted = false;

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();

            isInserted = paymentWorkflowModel.UpdateAmountInPaymentDetailAdvance(Transactions);

            return isInserted;
        }

        public bool UpdateAmountInPaymentDetailAgainst(List<PaymentDetailAgainstBill> Transactions)
        {
            bool isInserted = false;

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();

            isInserted = paymentWorkflowModel.UpdateAmountInPaymentDetailAgainst(Transactions);

            return isInserted;
        }

        public List<ListItem> GetStatusList()
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetStatusList();
        }

        public string UpdatePaymentDetail_Adv(List<PaymentDetailAdvance> Transactions, List<PaymentAllocationDetail> Allocations, string Savemode, string HouseBank = "")
        {
            //bool isInserted = false;
            StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
            sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                            + "controller : UpdatePaymentDetail_Adv"));
            sw.Close();
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();

            return paymentWorkflowModel.UpdatePaymentDetailAdvance(Transactions, Allocations, Savemode, HouseBank);

            //return isInserted;
        }

        public string UpdatePaymentDetail_Agnt(List<PaymentDetailAgainstBill> Transactions, List<PaymentAllocationDetail> Allocations, string saveMode, string HouseBank = "")
        {
            //bool isInserted = false;

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();


            StreamWriter sw = System.IO.File.AppendText(ConfigurationManager.AppSettings["TraceLogFilePath"].ToString() + " / Log.txt");
            sw.WriteLine(Convert.ToString("Date" + DateTime.Now.ToString() + Environment.NewLine.Trim()
                                            + "controller : UpdatePaymentDetail_Agnt"));
            sw.Close();

            return paymentWorkflowModel.UpdatePaymentDetailAgainst(Transactions, Allocations,saveMode, HouseBank);

            //return isInserted;
        }

        //public decimal GetTodaysTransAmountByAggregator(string Compcode, string VendorCode, BillType billType)
        //{
        //    PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
        //    return paymentWorkflowModel.GetTodaysTransAmountByAggregator(Compcode, VendorCode, billType);
        //}





        public List<GetBudgetAllocationDetails_Result> GetBudgetAllocationDetails(String mode, String SAPPaumentIDs)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetBudgetAllocationDetails(mode, SAPPaumentIDs);
        }

        public decimal GetDailyPaymentLimit()
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetDailyPaymentLimit();
        }

        public List<GetNextPaymentWorkFlowId_Result> GetNextPaymentWorkFlowId(string CompCode, string VendorCode, BillType BillType, int ProfileId, bool IsOpenAdvnce, decimal AmountToBeApproved)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetNextPaymentWorkFlowId(CompCode, VendorCode, BillType, ProfileId, IsOpenAdvnce, AmountToBeApproved);
        }

        public string ValidPaymentAllocation(string mode, string PaymentIds)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.ValidPaymentAllocation(mode, PaymentIds);
        }

        public List<PaymentWorkflowStatu> GetPaymentWorkFlowStatus()
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetPaymentWorkFlowStatus();
        }

        [HttpGet]
        // GET: api/workflow/getPendingVendorDataForFnA
        [Route("getpaymentworkflowcomment")]
        public List<PaymentComments> GetPaymentWorkFlowComment(int payementId, string paymentbillType)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.getPaymentComment(payementId, paymentbillType);
        }

        [HttpGet]
        // GET: api/workflow/getpaymentworkflowinprocess
        [Route("getpaymentworkflowinprocess")]
        public List<GetPaymentWorkFlowInProcessDetail_Result> getPaymentWorkFlowInProcess(string paymentbillType)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.getPaymentWorkFlowInProcess(paymentbillType);
        }

        [HttpGet]
        // GET: api/workflow/isvendorbankupdationinprocess
        [Route("isvendorbankupdationinprocess")]
        public IHttpActionResult isVendorBankUpdationInProcess(string VendorCode, string CompCode)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool isInprocess = paymentWorkflowModel.isVendorBankUpdationInProcess(VendorCode, CompCode);
            return Ok(isInprocess);
        }

        [HttpGet]
        // GET: api/workflow/GetVendorReqByVerticalID_Adv
        [Route("getVerticalBalance")]
        public decimal getVerticalBalance(Int32 VerticalID)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            decimal lstGetVendorData_Result = paymentWorkflowModel.getVerticalBalance(Convert.ToInt32(VerticalID));
            return lstGetVendorData_Result;
        }

        [HttpGet]
        // GET: api/workflow/GetVendorReqByVerticalID_Adv
        [Route("getVerticalBalance")]
        public List<sp_getVerticalBalanceDetails_Result> getVerticalBalanceDetails(Int32 VerticalID)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<sp_getVerticalBalanceDetails_Result> getVerticalBalanceDetails_Result = paymentWorkflowModel.getVerticalBalanceDetails(Convert.ToInt32(VerticalID));
            return getVerticalBalanceDetails_Result;
        }


        [HttpGet]
        // GET: api/workflow/GetVendorReqByVerticalID_Adv
        [Route("getBalanceByTreasury")]
        public decimal getBalanceByTreasury(Int32 TreasuryId)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            decimal lstGetVendorData_Result = paymentWorkflowModel.getBalanceByTreasury(Convert.ToInt32(TreasuryId));
            return lstGetVendorData_Result;
        }

        [HttpGet]
        // GET: api/workflow/GetVendorReqByVerticalID_Adv
        [Route("getBalanceDetailsByTreasury")]
        public List<sp_getBalanceDetailsByTreasury_Result> getBalanceDetailsByTreasury(Int32 VerticalID)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            List<sp_getBalanceDetailsByTreasury_Result> getVerticalBalanceDetails_Result = paymentWorkflowModel.getBalanceDetailsByTreasury(Convert.ToInt32(VerticalID));
            return getVerticalBalanceDetails_Result;
        }
        [Route("UpdateSaveStatusAdvance")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateSaveStatusAdvance(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ArrayList values = JsonConvert.DeserializeObject<ArrayList>(paramDetail);
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool result = paymentWorkflowModel.UpdateSaveStatusAdvance(values);
            return Ok(result);
        }
        [Route("UpdateSaveStatusAgainst")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateSaveStatusAgainst(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ArrayList values = JsonConvert.DeserializeObject<ArrayList>(paramDetail);
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            bool result = paymentWorkflowModel.UpdateSaveStatusAgainst(values);
            return Ok(result);
        }

        [Route("getpaymentinitiatorpopupdata")]
        [HttpGet]
        //GET: api/naturerequest/exists
        public IHttpActionResult GetPaymentInitiatorPopupData(string VendorCode, string CompanyCode, string BillType)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.GetPaymentInitiatorPopupData(VendorCode, CompanyCode, BillType));
        }

        [Route("AgainstBillSveToSAP")]
        [HttpPost]      
        public IHttpActionResult AgainstBillSveToSAP(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ArrayList values = JsonConvert.DeserializeObject<ArrayList>(paramDetail);

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            SAPAgainstBillPayment result = paymentWorkflowModel.SAPAgainstBillSubmit(values);
            return Ok(result);
        }

        [Route("getpaymentinitiatorpopupdataadvance")]
        [HttpGet]
        //GET: api/naturerequest/exists
        public IHttpActionResult GetPaymentInitiatorPopupDataAdvance(string VendorCode, string CompanyCode, string BillType)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.GetPaymentInitiatorPopupDataAdvance(VendorCode, CompanyCode, BillType));
        }



        [Route("AdvanceBillSveToSAP")]
        [HttpPost]
        public IHttpActionResult AdvanceBillSveToSAP(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ArrayList values = JsonConvert.DeserializeObject<ArrayList>(paramDetail);

            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            SAPAdvancePayment result = paymentWorkflowModel.SAPAdvanceBillSubmit(values);
            return Ok(result);
        }

        [Route("getAdvanceReverseCaseDetail")]
        [HttpGet]
        //GET: api/naturerequest/exists
        public List<sp_GetReverseCaseData_Result> getAdvanceReverseCaseDetail(int sapAdvanceId,string billType)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
           return paymentWorkflowModel.getAdvanceReverseCaseDetail(sapAdvanceId, billType);
        }
        [Route("fillReverseReason")]
        [HttpGet]
        //GET: api/naturerequest/exists
        public List<ListItem> fillReverseReason()
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.fillReverseReason();
        }

        [Route("getReverseCaseDetail")]
        [HttpGet]
        //GET: api/naturerequest/exists
        public List<sp_GetReversePaymentDetail_Result> fillReversePaymentDetail(int VericalId, string billType)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.fillReversePaymentDetail(VericalId, billType);
        }

        [Route("UpdateReverseCaseDetail")]
        [HttpGet]
        //GET: api/naturerequest/exists
        public bool UpdateReverseCaseDetail(string DocumentClearingNo,int ReverseReasonId,string billType,DateTime ReverseDate)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.UpdateReverseCaseDetail(DocumentClearingNo, ReverseReasonId, billType, ReverseDate);
        }

        [Route("PushDataToSAPReversePaymentAdvance")]
        [HttpPost]
        //GET: api/naturerequest/exists
        public string PushDataToSAPReversePaymentAdvance(string ClearingDocumentNo, string FiscalYear, string CompCode, string ReverseReason, DateTime PostingDate)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.PushDataToSAPReversePaymentAdvance(ClearingDocumentNo, FiscalYear, CompCode, ReverseReason, PostingDate);
        }

        [Route("PushDataToSAPReversePaymentAgainst")]
        [HttpPost]
        //GET: api/naturerequest/exists
        public string PushDataToSAPReversePaymentAgainst(string ClearingDocumentNo, string FiscalYear, string CompCode, string ReverseReason, DateTime PostingDate)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.PushDataToSAPReversePaymentAgainst(ClearingDocumentNo, FiscalYear, CompCode, ReverseReason, PostingDate);
        }
        [Route("RefreshAdavanceDataFormSAP")]
        [HttpPost]
        //GET: api/naturerequest/exists
        public string RefreshAdavanceDataFormSAP(int sapAdvanceId)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.RefreshAdavanceDataFormSAP(sapAdvanceId);
        }
        [Route("GetSapAdvanceDetailById")]
        [HttpPost]
        //GET: api/naturerequest/exists
        public SAPAdvancePayment GetSapAdvanceDetailById(int sapAdvanceId)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetSapAdvanceDetailById(sapAdvanceId);
        }

        [Route("getReversedDetail")]
        [HttpGet]
        //GET: api/naturerequest/exists
        public List<Usp_GetReversedPaymentDone_Result> GetReversedDetail(int VericalId, string billType)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return paymentWorkflowModel.GetReversedDetail(VericalId, billType);
        }

        #endregion "Payment Workflow"
    }
}
