namespace SuzlonBPP.Models
{
    public class Constants
    {
        public static string RESTURL = "";
        public static string TOKEN_URL = "api/account/token";
        public const string CONST_NEW_MODE = "New";
        public const string CONST_EDIT_MODE = "Edit";
        public const string CONST_MSG_NO_RECORD = "No Records Found.";
        public const string CONST_ALREADY_EXISTS = " already exists.";
        public const string SEPERATOR = "SOLAR_BPP";

        /*--------------Start Profile Master---------------*/
        //public static string CHECK_PROFILE_EXIST = "api/profile/exists?name={0}&profileId={1}";
        public static string CHECK_PROFILE_EXIST = "api/profile/exists";
        public static string PROFILE_EXIST_MESSAGE = "Profile Name already exists.";
        public static string PROFILE_GET_ALL = "api/profile/getall";
        public static string PROFILE_ADD = "api/profile/add";
        public static string PROFILE_UPDATE = "api/profile/update";
        public static string PROFILE_GET_MENULIST = "api/profile/getmenulist";

        /*--------------End Profile Master---------------*/

        /*--------------Start Company Master---------------*/
        public static string Company_GET_ALL = "api/company/getall";
        public static string Company_GET_USERWISE = "api/company/GetCompanyUserWise?CompanyCodes={0}";
        public static string Company_GET_VENDORS = "api/company/GetVendorCompanyWise?CompanyCode={0}";
        public static string Company_SEARCHVENDORS = "api/company/SearchVendorCompanyWise?CompanyCode={0}&searchText={1}";
        /*--------------End Company Master---------------*/

        /*--------------Start Vendor Bank Master---------------*/
        public static string Vendor_BANK_GET_ALL = "api/vendor/getallbankdetail";
        /*--------------End Vendor Bank Master---------------*/

        /*--------------Start Vendor Master---------------*/
        public static string Vendor_GET_ALL = "api/vendor/getallvendordetail";
        /*--------------End Vendor Master---------------*/


        /*--------------Start Vertical Master---------------*/
        public static string VERTICAL_GET = "api/vertical/getall";
        public static string VERTICAL_ADD = "api/vertical/Add";
        public static string VERTICAL_EDIT = "api/vertical/update";
        public static string VERTICAL_EXISTS = "api/vertical/exists?name={0}&verticalId={1}";
        public static string GETVERTICALBYUSER = "api/vertical/getverticalsbyuser?UserId={0}";
        /*--------------End Vertical Master---------------*/

        /*--------------Start Sub Vertical Master---------------*/
        public static string SUBVERTICAL_GET = "api/subvertical/getall";
        public static string SUBVERTICAL_ADD = "api/subvertical/Add";
        public static string SUBVERTICAL_EDIT = "api/subvertical/update";
        public static string SUBVERTICAL_EXISTS = "api/subvertical/exists?name={0}&subverticalId={1}&verticalId={2}";
        public static string SUBVERTICAL_BYVERTICAL = "api/subvertical/getbasedonvertical?verticalIds={0}";
        public static string SUBVERTICAL_BYUSER = "api/subvertical/GetSubVerticalsByUser";
        public static string SUBVERTICAL_DUPLIACATE_MSG = "Duplicate values for  combination of same vertical and same sub-Vertical is not allowed";

        /*--------------End Sub Vertical Master---------------*/

        /*--------------Start IFSC Code Master---------------*/
        public static string IFSCCODE_GET = "api/ifsccode/getall";
        public static string IFSCCODE_ADD = "api/ifsccode/Add";
        public static string IFSCCODE_EDIT = "api/ifsccode/update";
        public static string IFSCCODE_EXISTS = "api/ifsccode/exists?iFSCCode={0}&iFSCCodeId={1}";
        /*--------------End Vertical Master---------------*/

        /*--------------Start Nature of Request Master---------------*/
        public static string NATUREREQUEST_GET = "api/naturerequest/getall";
        public static string NATUREREQUEST_ADD = "api/naturerequest/Add";
        public static string NATUREREQUEST_EDIT = "api/naturerequest/update";
        public static string NATUREREQUEST_EXISTS = "api/naturerequest/exists?name={0}&requestId={1}";
        /*--------------End Nature of Request Master---------------*/

        /*--------------Start User Master---------------*/
        public static string USER_GETALL = "api/account/alluser";
        /*--------------User Master---------------*/

        /*--------------End Start User Detail---------------*/
        public static string USERDETAIL_GETUSERDTL = "api/account/userdetail?UserId=";
        public static string USERDETAIL_GETDROPDOWNVALUE = "api/Values/populatedropdowns?name=";
        public static string USERDETAIL_GETDROPDOWNVALUEBYUSER = "api/Values/populatedropdownsbyuser?name=";
        public static string CHECK_USERDTL_EXIST = "api/account/checkexistance";
        public static string USERDETAIL_ADD = "api/account/add";
        public static string USERDETAIL_UPDATE = "api/account/update";
        public static string CHECK_USER_DEPENDENCY = "api/account/isuserassignedtoworkflow?userId={0}";
        public static string USERDETAIL_MSGFILENAME = "FileName already exist.";
        public static string USERDETAIL_MSGFILENAMEEMPTY = "Please select file.";
        public static string USERDETAIL_MSGEMPPIDEXIST = "Employee id already exist";
        public static string USERDETAIL_MSGEMAILID = "Email id already exist";
        public static string USERDETAIL_MSGUSERNAME = "User id already exist";
        public static string USER_ASSIGNED_TO_WORKFLOW_MSG = "User is assigned to workflow, you can not disabled.";
        public static string USER_IMAGEVALIDATION_MSG = "Please upload only .JPEG,.JPG,.PNG,.GIF,.PDF file only";
        public static string USERPROFILE_IMAGEVALIDATION_MSG = "Please upload only .JPEG,.JPG,.PNG,.GIF file only";
        /*--------------End User Detail---------------*/

        /*--------------Start Application Setting---------------*/
        public static string GET_APPLICATION_SETTINGS = "api/setting/getapplsettings";
        public static string SAVE_APPLICATION_SETTINGS = "api/setting/saveapplsettings";
        /*--------------End Application Setting---------------*/

        /*--------------Start Bank Workflow---------------*/
        public static string GET_INITIATOR_ACCESS = "api/workflow/initiatoraccess";
        public static string GET_BANK_WORKFLOW = "api/workflow/getbankworkflow?subVerticalIds={0}";
        public static string SAVE_BANK_WORKFLOW = "api/workflow/savebankworkflow";
        public static string GET_VENDOR_CODE_WORKFLOW = "api/workflow/getvendorcodeworkflow?userId={0}";
        public static string GET_COMPANY_CODE_BY_VENDOR_CODE = "api/workflow/getcompanycodebyvendorcode?vendorCode={0}";
        public static string GET_VENDORDETAILS_FOR_WORKFLOW = "api/workflow/getvendordetailsforworkflow?vendorCode={0}&companyCode={1}";
        public static string GET_BANK_MY_RECORDS = "api/workflow/getbankmyrecords?vendorCode={0}&companyCode={1}";
        public static string GET_BANK_PENDING_RECORDS = "api/workflow/getbankpendingrecords?vendorCode={0}&companyCode={1}";
        public static string UPDATE_BANK_PENDING_APPROVAL_DETAILS = "api/workflow/updatebankpendingapprovaldetails";
        public static string GET_BANK_ASSIGNED_TO_VALUES = "api/workflow/getbankassignedtovalues?profileId={0}";
        public static string GET_BANK_COMMENTS = "api/workflow/getbankdetailcomment?bankDetailId={0}";
        public static string BANKWORKFLOW_GETDROPDOWNVALUE = "api/Values/populatebankworkflowdropdown?name={0}&subVerticalId={1}";
        public static string BANKWORKFLOW_AUDITTRAIL = "api/workflow/getbankworkflowaudittrail?WorkFlowId={0}&SubVerticalId={1}";
        public static string GET_BANK_PENDING_RECORDSFOR_DOC = "api/workflow/getpendingforcbfordocrec?vendorCode={0}&companyCode={1}";
        public static string UPDATE_BANK_DOC_RECEIVED = "api/workflow/updatebankdetailhistory";
        public static string GET_INITIATORDOCSENDPENDING = "api/workflow/getpendingforinitiatordocsend?vendorCode={0}&companyCode={1}";
        public static string UPDATE_BANK_DOC_SEND = "api/workflow/updatebankdetailhistorydocsend";

        /*--------------End Bank Workflow---------------*/

        /*--------------Start Treasury Workflow---------------*/
        public static string GET_TREASURY_WORKFLOW = "api/workflow/gettreasuryworkflow?subVerticalIds={0}";
        public static string SAVE_TREASURY_WORKFLOW = "api/workflow/savetreasuryworkflow";
        /*--------------End Treasury Workflow---------------*/

        /*--------------Start Payment Workflow---------------*/
        public static string GET_PAYMENT_WORKFLOW = "api/workflow/getpaymentworkflow?subVerticalIds={0}";
        public static string SAVE_PAYMENT_WORKFLOW = "api/workflow/savepaymentworkflow";
        public static string GET_ADVANCE_PAYMENT_DETAILS = "api/workflow/getAdvancePaymentDetails?companycode={0}&vendorcode={1}&Type={2}";
        public static string GET_AGAINSTBILL_PAYMENT_DETAILS = "api/workflow/getAgainstBillPaymentDetails?companycode={0}&vendorcode={1}&Type={2}";
        public static string GET_ADVANCE_PAY = "api/workflow/GetAdvancePay?Id={0}";
        public static string GET_AGAINSTBILL_PAY = "api/workflow/GetAgainstBillPay?Id={0}";
        public static string UPDATE_AGAINSTBILL_PAY = "api/workflow/UpdateAgainstBillPayment";
        public static string UPDATE_ADVANCE_PAY = "api/workflow/UpdateAdvanceBillPayment";
        public static string INSERT_AGAINSTBILL_PAY = "api/workflow/InsertAgainstBillPayment";
        public static string INSERT_ADVANCE_PAY = "api/workflow/InsertAdvancePayment";
        public static string GET_PENDINGREQ_BYVERTICALID = "api/workflow/GetPendingReqByVerticalID_adv?VerticalID={0}";

        public static string GET_PENDINGREQFORFnA_BYVERTICALID = "api/workflow/getPendingCompanyDataForFnA?VerticalID={0}";
        public static string GET_PENDINGREQVENDORFORFnA = "api/workflow/getPendingVendorDataForFnA?VerticalID={0}&CompanyID={1}";
        public static string GET_PENDINGREQVENDORITMSFORFnA = "api/workflow/getPendingVendorItemsDataForFnA?VerticalID={0}&CompanyID={1}&VendorID={2}";
        public static string GET_PENDINGREQAGABILLQFORFnA = "api/workflow/getPendingCompanyDataAgaBillForFnA?VerticalID={0}";
        public static string GET_PENDINGREQVENDORAGABILLFORFnA = "api/workflow/getPendingVendorDataAgaBillForFnA?VerticalID={0}&CompanyID={1}";
        public static string GET_PENDINGREQVENDORITMSAGABILLFORFnA = "api/workflow/getPendingVendorItemsDataAgaBillForFnA?VerticalID={0}&CompanyID={1}&VendorID={2}";
        public static string GET_VERTICALBUDGETFORFnA = "api/workflow/getVerticalBudgetForFnA?VerticalID={0}";

        public static string GET_VENDORREQ_BYVERTICALID_ADV = "api/workflow/GetVendorReqByVerticalID_Adv?VerticalID={0}";
        public static string GET_COMPREQ_BYVENDOR_ADV = "api/workflow/GetCompaniesReqByVendor_Adv?VerticalID={0}&VendorCode={1}";
        public static string GET_APPROVED_ENDITEMS_ADV = "api/workflow/GetApproverEndItemsAdvance?VerticalID={0}&VendorCode={1}&CompanyCode={2}";
        public static string GET_VENDORREQ_BYVERTICALID_AGA = "api/workflow/GetVendorReqByVerticalID_Aga?VerticalID={0}";
        public static string GET_COMPREQ_BYVENDOR_AGA = "api/workflow/GetCompaniesReqByVendor_Aga?VerticalID={0}&VendorCode={1}";
        public static string SAVE_PAYMENTDETAIL_TXN_ADV = "api/workflow/SavePaymentDetailTxn_Adv";
        public static string UPDATE_PAYMENTDETAIL_ADV = "api/workflow/UpdatePaymentDetail_Adv";

        public static string GET_AGAINSTBILL_INPROCESS = "api/workflow/GetInitiatorAgainstBillInProcessData?VendorCode={0}&CompanyCode={1}";
        public static string GET_ADVANCE_INPROCESS = "api/workflow/GetInitiatorAdvanceInProcessData?VendorCode={0}&CompanyCode={1}";
        public static string AGAINST_BILL_TRANSACTIONS = "api/workflow/AgainstBillTransaction?values={0}";
        public static string ADVANCE_BILL_TRANSACTIONS = "api/workflow/AdvancePaymentTransaction?values={0}";
        public static string GET_AGAINSTBILL_CORRECTION = "api/workflow/GetInitiatorAgainstBillCorrectionData?VendorCode={0}&CompanyCode={1}";
        public static string GET_ADVANCE_CORRECTION = "api/workflow/GetInitiatorAdvanceCorrectionData?VendorCode={0}&CompanyCode={1}";
        public static string AGAINST_BILL_CORRECTIONS = "api/workflow/AgainstBillCorrection?values={0}";
        public static string ADVANCE_PAY_CORRECTIONS = "api/workflow/AdvancePaymentCorrection?values={0}";
        public static string AGAINST_BILL_COMMENTS = "api/workflow/GetCommentsForMyRequest?needCorrection={0}";
        public static string ADVANCE_PAY_COMMENTS = "api/workflow/GetCommentsForAdvanceMyRequest?needCorrection={0}";
        public static string PAYMENTS_COMMENTS = "api/workflow/getpaymentworkflowcomment?payementId={0}&paymentbillType={1}";

        public static string GETPAYMENTS_WORKFLOW_INPROCESSDETAIL = "api/workflow/getpaymentworkflowinprocess?paymentbillType={0}";
        public static string ISVENDORBANKDETAILUPDATE_INPROCESS = "api/workflow/isvendorbankupdationinprocess?vendorCode={0}&CompCode={1}";
        public static string ISVENDORBANKDETAILUPDATE_INPROCESSMSG = "For Vendor and Company bank details updation is in Progress you cannot submit the record for payment.";
        public static string DAILYPAIDLIMITMSG_CB = "Per lot Vendor payment Limit should not be more than rs. 2 crore";
        public static string HOUSE_KEY_VALIDATED = "House Key is validated successfully.";
        public static string HOUSE_KEY_APPLIED = "House Key is applied.";
        public static string ENTER_HOUSE_BANK = "Please Enter House Bank Code.";
        public static string ADVANCE_BILL_SAVEUPDATEMODE = "api/workflow/UpdateSaveStatusAdvance?values={0}";
        public static string AGAINST_BILL_SAVEUPDATEMODE = "api/workflow/UpdateSaveStatusAgainst?values={0}";
        public static string GET_PAYMENTINITIATORPOPUP_DATA = "api/workflow/getpaymentinitiatorpopupdata?VendorCode={0}&CompanyCode={1}&BillType={2}";
        public static string AGAINST_BILL_SAVETOSAP = "api/workflow/AgainstBillSveToSAP?values={0}";
        public static string GET_PAYMENTINITIATORPOPUP_DATAADVANCE = "api/workflow/getpaymentinitiatorpopupdataadvance?VendorCode={0}&CompanyCode={1}&BillType={2}";
        public static string ADVANCE_BILL_SAVETOSAP = "api/workflow/AdvanceBillSveToSAP?values={0}";
        /*--------------End Payment Workflow---------------*/

        /*------------------------Start Forgot Password-----------------------*/
        public static string FORGOT_PASSWORD = "api/account/forgotpassword";
        public static string VALIDATE_CREDETIALS = "api/account/ValidateCredentials";
        /*------------------------End Forgot Password-----------------------*/
        /*------------------------Start Add Vendor Bank Detail-----------------------*/
        //public static string VENDOR_BANK_DTL_BYIFSC = "api/workflow/getbankdetailsbyifsc?BankName={0}&BrnachName={1}";
        // public static string VENDOR_BANK_DTL_BY_BRANCH_BANK = "api/workflow/getbankdetailsbybranchbank?ifscCode={0}";
        public static string VENDOR_BANK_IFSC_ACNO_EXIST= "Details for IFSC code & account no already entered.";
        public static string VENDOR_BANK_DTL_BY_BRANCH_BANK = "api/workflow/getbankdetailsbybranchbank?BankName={0}&BrnachName={1}";
        public static string VENDOR_BANK_DTL_BYIFSC = "api/workflow/getbankdetailsbyifsc?ifscCode={0}";
        public static string VENDOR_BANK_SUZLON_MAILEXIST = "api/workflow/existsuzlonmailidforvalidator?suzlonmailid1={0}&suzlonmailid2={1}";
        public static string VENDOR_BANK_SUZLON_MAIL1EXISTMSG = "Suzlon Email ID 1 does not match with the database. ";
        public static string VENDOR_BANK_SUZLON_MAIL2EXISTMSG = "Suzlon Email ID 2 does not match with the database.";
        public static string VENDOR_BANK_ATTACHMENT_MISSING = "Please Add Attachment Cancelled Cheque or Account Certificate";
        public static string VENDOR_BANK_DETAIL_ADD = "api/workflow/addbankdetails";
        public static string VENDOR_BANK_DETAIL_EXIST = "api/workflow/existvendorbankdetail?vendorCode={0}&&compCode={1}";       
        public static string VENDOR_BANK_DETAIL_EXISTMSG = "Bank details are already submitted and pending for approval";
        public const string VENDOR_BANK_GETVERTICAl_BYSUBVERTICAL = "api/workflow/getverticalbysubvertical?subVertical={0}";
        public const string VENDOR_BANK_GETDETAIL = "api/workflow/getvendorbankdetail?bankDetailId={0}";
        // TBM
        public const string VENDOR_BANK_ATTACHMENT_PATH_TEMP = "UploadsTEMP\\";
        public const string VENDOR_BANK_ATTACHMENT_PATH = "Uploads//";


        public const string VENDOR_BANK_DOCRCEIVEDATE_NULL = "Please Select the document receive date";
        public const string VENDOR_BANK_DOCSENDDATE_NULL = "Please Select the document sent date";
        public const string VENDOR_BANK_GETHISTORY = "api/workflow/getbankdetailhistory?bankDetailId={0}";
        public static string VENDOR_BANK_DETAIL_UPDATE = "api/workflow/updatebankdetails";
        public static string VENDOR_BANK_GETAUDIT_TRAIL = "api/workflow/getbankdetailaudittrail?BankDetailId={0}";
        public static string VENDOR_BANK_ASSIGNTO_MSG = "Please Select the Assign to User.";
        public static string VENDOR_BANK_SUBVERTICAL_MSG = "Please Select the SubVertical.";
        public static string VENDOR_BANK_COMMENT_MSG = "Please Enter Comment.";
        public static string VENDOR_BANK_OCRREADER = "api/workflow/getbankdetailbyocrreader?FilePath={0}";
        public const string VENDOR_BANK_DATE_VALIDATION = "Document received date should not be less than dispatch date";
        public static string VENDOR_BANK_BYIFSC_ACCNO = "api/workflow/getvendordetailbyifscacc?vendorCode={0}&companyCode={1}&accountNo={2}";
        public static string VENDOR_BANK_WORKFLOWLOG = "api/workflow/getbankdetailworkflowlog?BankDetailId={0}";
        public static string VENDOR_BANK_WORKFLOWAPPROVELOG = "api/workflow/getbankapprovalworkflowlog?BankDetailId={0}";
        public static string VENDOR_BANK_CHKIFSC_ACNO_EXIST_MAIL = "api/workflow/ifscacnoexistswithemail";

        public static string VENDOR_BANK_CHKIFSC_ACNO_EXIST = "api/workflow/ifscacnoexists?BankDetailid={0}&IFSCCode={1}&AccNo={2}&VendorCode={3}&CompanyCode={4}";
        public const string VENDOR_BANK_GETHISTORY_CBDOC = "api/workflow/getbankdetailhistorycbdoc?bankDetailHistoryId={0}";

        public static string EXISTVENDOR_BANK_DETAIL = "api/workflow/getbankdetailsfrombankmaster?vendorCode={0}&&compCode={1}";
        public static string GETVENDOR_BANK_DETAILID = "api/workflow/getbankdetailid?vendorCode={0}&&compCode={1}";

        /*------------------------Start Add Vendor Bank Detail-----------------------*/

        /*------------------------ Start Bank Validator -----------------------*/
        public static string GET_BANK_VALIDATOR_DATA = "api/workflow/getbankvalidatordata";
        public static string UPDATE_BANK_VALIDATOR_DETAILS = "api/workflow/updatebankvalidatordata";
        public static string CHECK_BANK_VENDOR_COMPANY_CODE_BLOCK = "api/workflow/checkbankvendorcompcodeexistinsap?VendorCode={0}&CompanyCode={1}";
        /*------------------------ End Bank Validator -----------------------*/

        public const string CHANGE_PASSWORD = "api/account/changepassword";

        /*------------------------ Start Treasure Validator -----------------------*/
        public static string TREASURE_DATE_MSG = "Please Select Valid Range in Utilisation Period.";
        public static string TREASURE_ADDNATURE_MSG = "Please Select Request Type.";
        public static string TREASURE_AMOUNT_MSG = "Approved Amount Cannot Be Greater Than Requested Amount.";
        public static string TREASURE_ADN_DATE_MSG = "Please Select Valid Rrange in Utilisation Period.";
        public static string TREASURE_NATURE_REQUEST_MSG = "Please Insert the Nature of Request.";
        public static string TREASURE_APPAMNT_MSG = "Please Insert Approval Amount for All Records.";
        public static string TREASURE_COMMENT_MSG = "Please Insert Comment.";
        public const int CONST_INNERGRID_PAGE_SIZE = 3;
        public static string TREASURE_CONTROLLER_DETAIL_ADD = "api/workflow/addvendorcontrloerdetails";
        public const string TREASURE_CONTROLLER_GETDETAIL = "api/workflow/getvendorcontrollerdetail?controllerDetailId={0}";
        public const string TREASURE_CONTROLLER_DETAIL_EDIT = "api/workflow/editvendorcontrloerdetails";
        public const string ADDANDUM_DETAIL_ADD = "api/workflow/addaddandumddetails";
        public const string ADDANDUM_DETAIL_EDIT = "api/workflow/editaddandumddetails";
        public const string ADDBUDGETUTILISATION_DETAIL = "api/workflow/addbudegetutilisation";
        public const string UPDATEBUDGETUTILISATION_DETAIL = "api/workflow/updatebudegetutilisation";
        public const string DELETEBUDGETUTILISATION_DETAIL = "api/workflow/deletebudegetutilisation?UtilisationId={0}";
        public const string GETBUDGETUTILISATION_DETAIL = "api/workflow/getbudgetutilisation?treasuryDetailId={0}";
        public const string BUDGETUTILISATION_DETAILEXIST = "api/workflow/checkutilisationexist";
        public const string BUDGETUTILISATION_DETAILEXISTMSG = "Budget Utilisation already exist";
        public const string GETADDENDUM_COMMENT = "api/workflow/getaddendumcomment?addendumId={0}";
        /*------------------------ End Treasure Validator -----------------------*/

        /*------------------------ Start Treasure ListDetail -----------------------*/
        public const string GET_TREASURY_MYREQUEST = "api/workflow/getmyrequesttreasury";
        public const string GET_TREASURY_MYRAPPROVEREQUEST = "api/workflow/getmyapproverequesttreasury";
        public const string GET_TREASURY_MYADDENDUMREQUEST = "api/workflow/getmyaddendumtreasuryrequest";
        public const string GET_TREASURY_PENDINGREQUEST = "api/workflow/gettreasurypendingrequest";
        public const string GET_TREASURY_REQUESTCOMMENT = "api/workflow/gettreasuryrequestcomment?TreasuryRequestId={0}";
        public const string GET_TREASURY_REQUESTATTACHMENT = "api/workflow/gettreasuryrequestattachment?TreasuryRequestId={0}";
        public const string GET_TREASURY_ADDENDUMPENDINGREQUEST = "api/workflow/gettreasurypendingaddendumrequest";
        public const string GET_TREASURY_REQUESTAPPROVEBYME = "api/workflow/gettreasuryapprovebymerequest";
        public const string GET_TREASURY_REQUESTTOCB = "api/workflow/gettreasuryrequesttocb";
        /*------------------------ End Treasure ListDetail -----------------------*/

        /*------------------------ Start FileUpload -----------------------*/

        public const string GET_FILEUPLOADS = "api/values/getFileUploads?entityId={0}&entityName={1}";
        public const string ADD_FILEUPLOAD = "api/values/AddFileUpload";
        public const string DELETE_FILEUPLOAD = "api/values/DeleteUploadedFile?Id={0}&userid={1}";

        /*------------------------ End FileUpload -----------------------*/

        #region "Grid Constants"

        public const int CONST_GRID_PAGE_SIZE = 10;
        public const string CONST_GRID_RECORD_DETAILS = "records matching your search criteria";
        public const string CONST_GRID_RECORD_PER_PAGE = "Records per page:";

        public const string CONST_ERROR_MAX_150_CHARACTERS = "Maximum 150 characters allowed.";
        public const string CONST_ERROR_MAX_135_CHARACTERS = "Maximum 135 characters allowed.";

        #endregion

        #region "Messages"

        public const string CONST_RECORD_ALREADY_EXIST = "Record already exists";
        public const string CONST_MESSAGE_SENT = "e-Mail is send to your registered email id.";
        public const string CONST_MESSAGE_SENDING_FAIL = "Error in sending email. Please try again.";
        public const string OLD_PASSWORD_NOT_MATCH = "Old password didn't match.";
        public const string OLD_PASSWORD_SHOULD_NOT_SAME = "Old password and new password should not be same.";
        public const string PASSWORD_UPATED = "Password Updated Successfully.";
        public const string PASSWORD_UPATE_FAILED = "Failed to Update Password.";

        public const string ERROR_OCCURED_WHILE_UPDATING = "Error occured while updating.";
        public const string UPLOAD_RECORD_MESSAGE = " Record Uploaded Sucessfully.";
        public const string NOTUPLOAD_RECORD_MESSAGE = " Records Not Uploaded.";
        public static string RECORD_EXIST_MESSAGE = "Record already exists.";
        public static string REST_CALL_FAILURE = "Rest call failed.";
        public static string ERROR_OCC_WHILE_SAVING = "Error occured while saving details.";
        public static string ERROR_OCC_WHILE_UPDATING = "Error occured while updating details.";
        public static string ERROR_OCC_WHILE_GETTING_DETAILS = "Error occured while getting details.";
        public static string RAD_MESSAGE_TITLE = "Message";
        public static string DETAIL_SAVE_FAILURE = "Details not get saved.";
        public static string DETAIL_SAVE_SUCCESS = "Details saved successfully.";
        public static string SUCCESS = "Success.";
        public static string ERROR = "Failed";
        public static string SAP_CONNECTION_FAILURE = "Error occurred while connection to SAP";
        public static string SAP_RESULT = "Error occurred while getting result from SAP";
        public static string SAP_ERROR = "Error occurred while inserting data to SAP.";
        public static string ERROR_WHILE_INSERTING = "Error occurred while inserting data.";
        public static string ERROR_WHILE_UPDATING = "Error occurred while updating data.";
        public static string SAP_SUCCESS = "S";
        public static string SAP_WARNING = "W";
        public static string SAVE_MODE = "Save";
        public static string SUBMIT_MODE = "Submit";
        public static string SELECT_RECORD = "Select atleast one record.";
        public static string ENTERAMOUNT = "Please enter Approved Amount for selected records";
        public static string DAILYPAYMTLIMITCROSSED = "Daily Payment Limit of Rs. {0} is crossed, Do you want to continue ?";

        public static string DAILYAPPROVEDAMOUNTCROSSED = "Daily Approved amount limit is crossed";
        public static string SELECTVERTICAL = "Please select vertical";
        public static string SELECTREMARK = "Please enter remark";
        public static string SELECTAMOUNTREMARK = "Please enter valid amount for selected record.";
        public static string APPROVEDAMOUNTVALIDATION = "Please enter Approved Amount less than Amount Proposed";
        #endregion

        #region "Session Variable"
        public const string DROPDOWN_VALUES_FOR_USERCONTROL = "DPValuesForUC";
        #endregion "Session Variable"
    }

    #region "Enum"

    public enum WorkFlow
    {
        BankWorkflow = 1,
        Treasury,
        Payment
    }

    public enum Status
    {
        Approved = 1,
        Rejected = 2,
        NeedCorrection = 3,
        InProgress = 4
    }

    public enum WorkFlowStatusEnum
    {
        ApprovedByValidator = 1,
        RejectedByValidator,
        NeedCorrectionByValidator,
        ApprovedByVerticalController,
        RejectedByVerticalController,
        NeedCorrectionByVerticalController,
        ApprovedByGroupController,
        RejectedByGroupController,
        NeedCorrectionByGroupController,
        ApprovedByTreasury,
        RejectedByTreasury,
        NeedCorrectionByTreasury,
        ApprovedByManagementAssurance,
        RejectedByManagementAssurance,
        NeedCorrectionByManagementAssurance,
        ApprovedByFASSC,
        RejectedByFASSC,
        NeedCorrectionByFASSC,
        ApprovedByCB,
        RejectedByCB,
        NeedCorrectionByCB,
        ApprovedByPaymentProposer,
        RejectedByPaymentProposer,
        NeedCorrectionByPaymentProposer,
        NeedCorrectionByVendor

    }

    public enum UserProfileEnum
    {
        Admin = 1,
        VerticalController = 2,
        GroupController = 3,
        Treasury = 4,
        ManagementAssurance = 5,
        FASSC = 6,
        CB = 7,
        PaymentProposer = 8,
        Vendor = 9,
        Validator = 10,
        Controller=11,
        Auditor=12,
        Aggregator=1017,
        ExceptionalApprover=13
    }

    public enum PageIndex
    {
        BankDetailListVendor = 1,
        BankDetailListEmployee = 2,
        BankValidator
    }

    public enum TreasuryWorkFlowStatusEnum
    {
        DraftByVerticalController = 1,
        PendingForApproval =2,
        ApprovedByTreasury =3,
        RejectedByTreasury = 4,
        NeedCorrectionByTreasury = 5
    }

    public enum AddendomWorkFlowStatusEnum
    {
        DraftByVerticalController = 1,
        PendingForApproval = 2,
        ApprovedByTreasury = 3,
        RejectedByTreasury = 4,
        NeedCorrectionByTreasury = 5
    }

    public enum BillType
    {
        Against = 1,
        Advance = 2,
    }

    public enum TabType
    {
        PendingForApproval = 1,
        Approved = 2,
        NeedCorrection = 3
    }

    public enum NotificationTypeEnum
    {

        TreasuryRequestSubmit=1,
        TreasuryRequestApproved=2,
        TreasuryRequestReject = 3,
        TreasuryRequestNeedCorrection = 4,
        AddendumRequestSubmit = 5,
        AddendumRequestApproved = 6,
        AddendumRequestReject = 7,
        AddendumRequestNeedCorrection = 8,
        PaymentRequestSubmit=9,
        PaymentRequestApprove =10,
        PaymentRequestReject = 11,
        PaymentRequestNeedCorrection = 12,
    }

    #endregion
}
