using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System.Web.Http;

namespace SuzlonBPP.Controllers
{
    [Authorize]
    [RoutePrefix("api/workflow")]
    public class WorkflowController : BaseApiController
    {
        #region Bank Workflow"

        [Route("initiatoraccess")]
        [HttpGet]
        // GET: api/workflow/initiatoraccess
        public IHttpActionResult InitiatorAccess()
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            bool isExists = bankWorkflowModel.InitiatorAccess(UserId);
            return Ok(isExists);
        }
        [Route("ifscacnoexists")]
        [HttpGet]
        // GET: api/workflow/initiatoraccess
        public IHttpActionResult IfscAcnoExist(int BankDetailid, string IFSCCode, string AccNo,string VendorCode,string CompanyCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            bool isExists = bankWorkflowModel.IfscAcnoExists(BankDetailid, IFSCCode, AccNo, VendorCode, CompanyCode);
            return Ok(isExists);
        }

        [Route("ifscacnoexistswithemail")]
        [HttpPost]
        // GET: api/workflow/initiatoraccess
        public IHttpActionResult IfscAcnoExistsWithEmail(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankDetailModel bankModel = JsonConvert.DeserializeObject<BankDetailModel>(paramDetail);

            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            
            bool isExists = bankWorkflowModel.IfscAcnoExistsMail(bankModel);
            return Ok(isExists);
        }

        [Route("getbankdetailid")]
        [HttpGet]
        // GET: api/workflow/initiatoraccess
        public IHttpActionResult GetBankDetailId(string vendorCode, string compCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            string bankDetailId = bankWorkflowModel.GetBankDetailId(vendorCode, compCode);
            return Ok(bankDetailId);
        }

        [HttpGet]
        // GET: api/workflow/getbankworkflow
        [Route("getbankworkflow")]
        public IHttpActionResult GetBankWorkflow(string subVerticalIds)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankWorkflow(subVerticalIds);
            return Ok(details);
        }

        [Route("savebankworkflow")]
        [HttpPost]
        public IHttpActionResult SaveBankWorkflow(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankWorkflow bankWorkFlow = JsonConvert.DeserializeObject<BankWorkflow>(paramDetail);
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.SaveBankWorkflow(bankWorkFlow, UserId));
        }

        [HttpGet]
        // GET: api/workflow/getvendorcodeworkflow
        [Route("getvendorcodeworkflow")]
        public IHttpActionResult GetVendorCodeForWorkflow(int userId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetVendorCodeForWorkflow(userId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/GetVendorDetailBasedOnSearch
        [Route("getvendordetailbasedonsearch")]
        public IHttpActionResult GetVendorDetailBasedOnSearch(string searchText, int userId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetVendorDetailBasedOnSearch(searchText, userId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getcompanycodebyvendorcode
        [Route("getcompanycodebyvendorcode")]
        public IHttpActionResult GetCompanyCodeByVendorCode(string vendorCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetCompanyCodeByVendorCode(vendorCode);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getvendordetailsforworkflow
        [Route("getvendordetailsforworkflow")]
        public IHttpActionResult GetVendorDetailsForWorkflow(string vendorCode, string companyCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetVendorDetailsForWorkflow(vendorCode, companyCode);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getbankmyrecords
        [Route("getbankmyrecords")]
        public IHttpActionResult GetBankMyRecords(string vendorCode, string companyCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankMyRecords(vendorCode, companyCode, UserId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getbankpendingrecords
        [Route("getbankpendingrecords")]
        public IHttpActionResult GetBankPendingRecords(string vendorCode, string companyCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankPendingRecords(vendorCode, companyCode, UserId);
            return Ok(details);
        }
        [HttpGet]
        // GET: api/workflow/getpendingforcbfordocrec
        [Route("getpendingforcbfordocrec")]
        public IHttpActionResult GetpendingForCBForDocRec(string vendorCode, string companyCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetpendingForCBForDocRec(vendorCode, companyCode, UserId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getpendingforinitiatordocsend
        [Route("getpendingforinitiatordocsend")]
        public IHttpActionResult GetpendingForInitiatorDocSend(string vendorCode, string companyCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetpendingForInitiatorDocSend(vendorCode, companyCode, UserId);
            return Ok(details);
        }



        [HttpGet]
        // GET: api/workflow/getbankdetailsbyifsc
        [Route("getbankdetailsbyifsc")]
        public IHttpActionResult GetBankBranchDtl(string ifscCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankBranchDtl(ifscCode);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailsbybank&branch
        [Route("getbankdetailsbybranchbank")]
        public IHttpActionResult GetBankDetailsByBranchBank(string BankName, string BrnachName)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankDetailsByBranchBank(BankName, BrnachName);
            return Ok(details);
        }


        [HttpGet]
        // GET: api/workflow/getbankdetailsbyifsc
        [Route("existsuzlonmailidforvalidator")]
        public IHttpActionResult ExistSuzlonMailidOfValidator(string suzlonmailid1, string suzlonmailid2)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.SuzlonMailExists(suzlonmailid1, suzlonmailid2));
            //return Ok(exist);
        }

        [Route("addbankdetails")]
        [HttpPost]
        public IHttpActionResult AddBankDetail(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankDetailModel bankDetailModel = JsonConvert.DeserializeObject<BankDetailModel>(paramDetail);
            BankWorkflowModel paymentWorkflowModel = new BankWorkflowModel();
            return Ok(paymentWorkflowModel.AddBankDetail(bankDetailModel, UserId));
        }

        [Route("updatebankdetails")]
        [HttpPost]
        public IHttpActionResult UpdateBankDetail(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankDetailModel bankDetailModel = JsonConvert.DeserializeObject<BankDetailModel>(paramDetail);
            BankWorkflowModel paymentWorkflowModel = new BankWorkflowModel();
            return Ok(paymentWorkflowModel.UpdateBankDetail(bankDetailModel, UserId));
        }

        [Route("updatebankpendingapprovaldetails")]
        [HttpPost]
        public IHttpActionResult UpdateBankPendingApprovalDetails(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankPendingApprovalDetail bankPendingApprovalDetail = JsonConvert.DeserializeObject<BankPendingApprovalDetail>(paramDetail);
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.UpdateBankPendingApprovalDetails(bankPendingApprovalDetail, UserId));
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailsbyifsc
        [Route("getbankassignedtovalues")]
        public IHttpActionResult GetBankAssignedToValues(int profileId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.GetBankAssignedToValues(profileId));
        }

        //[HttpGet]
        ////GET: api/workflow/getverticalbysubvertical
        //[Route("getverticalbysubvertical")]
        //public IHttpActionResult GetVerticalBySubVertical(int subVertical)
        //{
        //    BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
        //    return Ok(bankWorkflowModel.GetVerticalBySubVertical(subVertical));
        //}

        //[HttpGet]
        ////GET: api/workflow/getvendorbankdetail
        //[Route("getvendorbankdetail")]
        //public IHttpActionResult GetVendorBankDetail(int bankDetailId)
        //{
        //    BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
        //    return Ok(bankWorkflowModel.GetVendorBankDetail(bankDetailId));
        //}


        [HttpGet]
        // GET: api/workflow/getbankdetailcomment
        [Route("getbankdetailcomment")]
        public IHttpActionResult GetBankDetailComment(int bankDetailId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.GetBankDetailComment(bankDetailId, UserId));
        }

        [HttpGet]
        // GET: api/workflow/getbankmyrecords
        [Route("getbankvalidatordata")]
        public IHttpActionResult GetBankValidatorData()
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankValidatorData(UserId);
            return Ok(details);
        }

        [Route("updatebankvalidatordata")]
        [HttpPost]
        public IHttpActionResult UpdateBankValidatorData(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankValidatorUpdate bankValidatorUpdate = JsonConvert.DeserializeObject<BankValidatorUpdate>(paramDetail);
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.UpdateBankValidatorData(bankValidatorUpdate, UserId));
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailaudittrail
        [Route("getbankdetailaudittrail")]
        public IHttpActionResult GetBankDetailAuditTrail(int BankDetailId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankDetailAuditTrail(BankDetailId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailaudittrail
        [Route("getbankworkflowaudittrail")]
        public IHttpActionResult GetBankWorkflowAuditTrail(int WorkFlowId, string SubVerticalId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankWorkflowAuditTrail(WorkFlowId, SubVerticalId);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailbyocrreader
        [Route("getbankdetailbyocrreader")]
        public async System.Threading.Tasks.Task<IHttpActionResult> GetBankDetailByOCRReader(string FilePath)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(await bankWorkflowModel.GetBankDetailByOCRReader(FilePath));
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailbyocrreader
        [Route("getbankdetailbyocrreaderformobile")]
        public async System.Threading.Tasks.Task<IHttpActionResult> GetBankDetailByOCRReaderForMobile(string FileName)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(await bankWorkflowModel.GetBankDetailByOCRReaderForMobile(FileName));
        }

        [HttpGet]
        // GET: api/workflow/getvendorbankaccountdetail
        [Route("getvendordetailbyifscacc")]
        public IHttpActionResult GetVendorDetailByIfscAcc(string vendorCode, string companyCode, string accountNo)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetVendorDetailByIfscAcc(vendorCode, companyCode, accountNo);
            return Ok(details);
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailworkflowlog
        [Route("getbankdetailworkflowlog")]
        public IHttpActionResult GetBankDetailWorkFlowLog(int BankDetailId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankDetailWorkFlowLogs(BankDetailId);
            return Ok(details);
        }


        [HttpGet]
        // GET: api/workflow/getbankapprovalworkflowlog
        [Route("getbankapprovalworkflowlog")]
        public IHttpActionResult GetBankApprovalWorkFlowLog(int BankDetailId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            var details = bankWorkflowModel.GetBankApprovalWorkFlowLogs(BankDetailId);
            return Ok(details);
        }

        [HttpPost]
        //GET: api/workflow/getvendorbankdetail
        [Route("checkbankvendorcompcodeexistinsap")]
        public IHttpActionResult CheckBankVendorCompCodeExistInSAP(string VendorCode, string CompanyCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.CheckBankVendorCompCodeExistInSAP(VendorCode, CompanyCode));
        }

        [HttpPost]
        //GET: api/workflow/updatebankdetailhistory
        [Route("updatebankdetailhistory")]
        public IHttpActionResult UpdateBankDetailHistory(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankDetailHistoryModel bankDetailHistoryModel = JsonConvert.DeserializeObject<BankDetailHistoryModel>(paramDetail);
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.UpdateBankDetailHistory(bankDetailHistoryModel));
        }

        [HttpPost]
        //GET: api/workflow/updatebankdetailhistorydocsend
        [Route("updatebankdetailhistorydocsend")]
        public IHttpActionResult UpdateBankDetailHistoryDocSend(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankDetailHistoryModel bankDetailHistoryModel = JsonConvert.DeserializeObject<BankDetailHistoryModel>(paramDetail);
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.UpdateBankDetailHistoryDocSend(bankDetailHistoryModel));
        }


        #endregion "Bank Workflow"

        #region "Treasury Workflow"

        [HttpGet]
        // GET: api/workflow/gettreasuryworkflow
        [Route("gettreasuryworkflow")]
        public IHttpActionResult GetTreasuryWorkflow(string subVerticalIds)
        {
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetTreasuryWorkflow(subVerticalIds);
            return Ok(details);
        }

        [Route("savetreasuryworkflow")]
        [HttpPost]
        public IHttpActionResult SaveTreasuryWorkflow(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TreasuryWorkflow treasuryWorkFlow = JsonConvert.DeserializeObject<TreasuryWorkflow>(paramDetail);
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            return Ok(treasuryWorkflowModel.SaveTreasuryWorkflow(treasuryWorkFlow, UserId));
        }

        [Route("getmyrequesttreasury")]
        [HttpGet]
        public IHttpActionResult GetMyRequestTreasury()
        {
            //TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            //return Ok(treasuryWorkflowModel.SaveTreasuryWorkflow(treasuryWorkFlow, UserId));
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetMyRequestTreasury(UserId);
            return Ok(details);
        }

        [Route("getmyapproverequesttreasury")]
        [HttpGet]
        public IHttpActionResult GetMyApproveRequestTreasury()
        {
            //TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            //return Ok(treasuryWorkflowModel.SaveTreasuryWorkflow(treasuryWorkFlow, UserId));
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetMyApproveRequestTreasury(UserId);
            return Ok(details);
        }
        [Route("getmyaddendumtreasuryrequest")]
        [HttpGet]
        public IHttpActionResult GetMyAddendumTreasuryRequest()
        {         
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetMyAddendumTreasuryRequest(UserId);
            return Ok(details);
        }

        [Route("gettreasurypendingrequest")]
        [HttpGet]
        public IHttpActionResult GetTreasuryPendingRequest()
        {
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetTreasuryPendingRequest(UserId);
            return Ok(details);
        }

        [Route("gettreasuryrequestcomment")]
        [HttpGet]
        public IHttpActionResult GetTreasuryRequestComment(int TreasuryRequestId)
        {
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetTreasuryRequestComment(TreasuryRequestId);
            return Ok(details);
        }

        [Route("gettreasuryrequestattachment")]
        [HttpGet]
        public IHttpActionResult GetTreasuryRequestAttachment(int TreasuryRequestId)
        {
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetTreasuryRequestAttachment(TreasuryRequestId);
            return Ok(details);
        }

        [Route("gettreasurypendingaddendumrequest")]
        [HttpGet]
        public IHttpActionResult GetTreasuryPendingAddendumRequest()
        {
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetTreasuryPendingAddendumRequest(UserId);
            return Ok(details);
        }
        [Route("gettreasuryapprovebymerequest")]
        [HttpGet]
        public IHttpActionResult GetTreasuryApproveByMeRequest()
        {
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetTreasuryApproveByMeRequest(UserId);
            return Ok(details);
        }
        [Route("gettreasuryrequesttocb")]
        [HttpGet]
        public IHttpActionResult GetTreasuryRequestToCB()
        {
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetTreasuryRequestToCB(UserId);
            return Ok(details);
        }

        [Route("addbudegetutilisation")]
        [HttpPost]
        public IHttpActionResult AddBudegetUtilisation(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TreasuryDetailModel treasuryModel = new TreasuryDetailModel();
            TreasuryBudgetUtilisation budUtil = JsonConvert.DeserializeObject<TreasuryBudgetUtilisation>(paramDetail);
            budUtil=treasuryModel.addUtilisation(budUtil, UserId);
            return Ok(budUtil);
             
        }
        [Route("updatebudegetutilisation")]
        [HttpPost]
        public IHttpActionResult UpdateBudegetUtilisation(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TreasuryDetailModel treasuryModel = new TreasuryDetailModel();
            TreasuryBudgetUtilisation budUtil = JsonConvert.DeserializeObject<TreasuryBudgetUtilisation>(paramDetail);
            bool isUpdate  = treasuryModel.updateUtilisation(budUtil, UserId);
            return Ok(isUpdate);
        }


        [Route("deletebudegetutilisation")]
        [HttpGet]
        public IHttpActionResult DeleteBudegetUtilisation(int UtilisationId)
        {            
            TreasuryDetailModel treasuryModel = new TreasuryDetailModel();            
            bool isDelete = treasuryModel.deleteutilisation(UtilisationId);
            return Ok(isDelete);
        }
        [Route("getbudgetutilisation")]
        [HttpGet]
        public IHttpActionResult GetBudegetUtilisation(int treasuryDetailId)
        {
            TreasuryDetailModel treasuryModel = new TreasuryDetailModel();
            var utilisationDetail = treasuryModel.getTresuryUtilizationDtl(treasuryDetailId);
            return Ok(utilisationDetail);
        }

        [Route("checkutilisationexist")]
        [HttpPost]        
        public IHttpActionResult CheckUtilisationExist(PostParam param)
        {
           var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TreasuryDetailModel treasuryModel = new TreasuryDetailModel();
            TreasuryBudgetUtilisation budUtil = JsonConvert.DeserializeObject<TreasuryBudgetUtilisation>(paramDetail);
            bool isExists = treasuryModel.checkutilisationExist(budUtil);
            return Ok(isExists);

        }

        #endregion "Treasury Workflow"

        #region "Payment Workflow"

        [HttpGet]
        // GET: api/workflow/getpaymentworkflow
        [Route("getpaymentworkflow")]
        public IHttpActionResult GetPaymentWorkflow(string subVerticalIds)
        {
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            var details = paymentWorkflowModel.GetPaymentWorkflow(subVerticalIds);
            return Ok(details);
        }

        [Route("savepaymentworkflow")]
        [HttpPost]
        public IHttpActionResult SavePaymentWorkflow(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            BankWorkflow paymentWorkFlow = JsonConvert.DeserializeObject<BankWorkflow>(paramDetail);
            PaymentWorkflowModel paymentWorkflowModel = new PaymentWorkflowModel();
            return Ok(paymentWorkflowModel.SavePaymentWorkflow(paymentWorkFlow, UserId));
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailsbyifsc
        [Route("existvendorbankdetail")]
        public IHttpActionResult Existvendorbankdetail(string vendorCode, string compCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.Existvendorbankdetail(vendorCode, compCode));
            //return Ok(exist);
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailsfrombankmaster
        [Route("getbankdetailsfrombankmaster")]
        public IHttpActionResult GetVendorDetailsFromBankMaster(string vendorCode, string compCode)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.GetVendorDetailsFromBankMasters(vendorCode, compCode));
            //return Ok(exist);
        }



        [HttpGet]
        //GET: api/workflow/getverticalbysubvertical
        [Route("getverticalbysubvertical")]
        public IHttpActionResult GetVerticalBySubVertical(int subVertical)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
             return Ok(bankWorkflowModel.GetVerticalBySubVertical(subVertical));            
        }

        [HttpGet]
        // GET: api/workflow/getbankdetailhistory
        [Route("getbankdetailhistory")]
        public IHttpActionResult GetBankDetailHistory(int bankDetailId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.GetBankDetailHistory(bankDetailId));
        }

        [HttpGet]
        // GET: api/workflow/GetBankDetailHistoryCBDoc
        [Route("getbankdetailhistorycbdoc")]
        public IHttpActionResult GetBankDetailHistoryCBDoc(int bankDetailHistoryId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.GetBankDetailHistoryCBDoc(bankDetailHistoryId));
        }



        [HttpGet]
        //GET: api/workflow/getvendorbankdetail
        [Route("getvendorbankdetail")]
        public IHttpActionResult GetVendorBankDetail(int bankDetailId)
        {
            BankWorkflowModel bankWorkflowModel = new BankWorkflowModel();
            return Ok(bankWorkflowModel.GetVendorBankDetail(bankDetailId, UserId));
        }

        [Route("addvendorcontrloerdetails")]
        [HttpPost]
        public IHttpActionResult AddVendorControllerDetail(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TreasuryDetailModel treasuryDetailModel = JsonConvert.DeserializeObject<TreasuryDetailModel>(paramDetail);
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            return Ok(treasuryWorkflowModel.AddTreasuryDetail(treasuryDetailModel, UserId));
        }
        [Route("editvendorcontrloerdetails")]
        [HttpPost]
        public IHttpActionResult EditVendorControllerDetail(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TreasuryDetailModel treasuryDetailModel = JsonConvert.DeserializeObject<TreasuryDetailModel>(paramDetail);
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            return Ok(treasuryWorkflowModel.EditTreasuryrDetail(treasuryDetailModel, UserId));
        }

        [Route("getvendorcontrollerdetail")]
        [HttpGet]
        public IHttpActionResult GetVendorControllerDetail(int controllerDetailId)
        {
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            var details = treasuryWorkflowModel.GetTreasuryDetail(controllerDetailId);
            return Ok(details);
        }

        [Route("addaddandumddetails")]
        [HttpPost]
        public IHttpActionResult AddaAdandumdDetails(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            AddandumModel addandumDetailModel = JsonConvert.DeserializeObject<AddandumModel>(paramDetail);
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            return Ok(treasuryWorkflowModel.AddAdandumdDetails(addandumDetailModel, UserId));
        }

        [Route("editaddandumddetails")]
        [HttpPost]
        public IHttpActionResult EditAddandumdDetails(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            AddandumModel addandumDetailModel = JsonConvert.DeserializeObject<AddandumModel>(paramDetail);
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
            return Ok(treasuryWorkflowModel.EditAddandumdDetails(addandumDetailModel, UserId));
        }

        [Route("getaddendumcomment")]
        [HttpGet]
        public IHttpActionResult GetAddendumComment(int addendumId)
        {         
            TreasuryWorkflowModel treasuryWorkflowModel = new TreasuryWorkflowModel();
             return Ok(treasuryWorkflowModel.GetAddendumComment(addendumId));
        }

        #endregion "Payment Workflow"   

    }
}
