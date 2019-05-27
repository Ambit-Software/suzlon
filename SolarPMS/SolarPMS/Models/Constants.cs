using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{


    public class Constants
    {
        public static string RESTURL = "";
        public static string TOKEN_URL = "api/account/token";
        public static string ApplicationPath="";

        public const string CONST_NEW_MODE = "New";
        public const string CONST_EDIT_MODE = "Edit";

        public const string CONST_MSG_NO_RECORD = "No Records Found.";
        public const string CONST_ALREADY_EXISTS = " already exists.";

        /*--------------Location---------------*/
        public static string LOCATION_GET = "api/location/getall";
        public static string LOCATION_ADD = "api/location/Add";
        public static string LOCATION_EDIT = "api/location/update";
        public static string LOCATION_EXISTS = "api/Location/exists";
        /*--------------Location---------------*/

        /*--------------Village---------------*/
        public static string VILLAGE_ADD = "api/village/Add";
        public static string VILLAGE_EDIT = "api/village/update";
        public static string VILLAGE_EXISTS = "api/village/exists";
        public static string VILLAGE_GET = "api/village/getall";
        /*--------------Village---------------*/

        /*--------------Profile Master---------------*/
        public static string CHECK_PROFILE_EXIST = "api/profile/exists?name={0}&profileId={1}";
        public static string PROFILE_EXIST_MESSAGE = "Profile Name already exists.";
        public static string PROFILE_GET_MENULIST = "api/profile/getmenulist";
        public static string PROFILE_GET_ALL = "api/profile/getall";
        public static string PROFILE_ADD = "api/profile/add";
        public static string PROFILE_UPDATE = "api/profile/update";
        /*--------------Profile Master---------------*/
        /*--------------User Master---------------*/
        public static string USER_GETALL = "api/account/alluser";

        /*--------------User Master---------------*/
        /*--------------User Detail---------------*/

        public static string USERDETAIL_GETUSERDTL = "api/account/userdetail?UserId=";
        public static string USERDETAIL_GETDROPDOWNVALUE = "api/Values/populatedropdowns?name=";
        public static string CHECK_USERDTL_EXIST = "api/account/checkexistance";
        public static string USERDETAIL_ADD = "api/account/add";
        public static string USERDETAIL_UPDATE = "api/account/update";
        public static string USERDETAIL_MSGFILENAME = "FileName already exist.";
        public static string USERDETAIL_MSGFILENAMEEMPTY = "Please select file.";
        public static string USERDETAIL_MSGEMPPIDEXIST = "Employee id already exist";
        public static string USERDETAIL_MSGEMAILID = "Email id already exist";
        public static string USERDETAIL_MSGUSERNAME = "User id already exist";
        public static string USER_IMAGEVALIDATION_MSG = "Please upload only .JPEG,.JPG,.PNG,.GIF file only";
        public static string USERDETAIL_DISABLEUSER = "api/account/isuserdisable?userId={0}";
        public static string USERDETAIL_MSGUSERDISABLE = "User data is not allocated to any other user. Please allocate first and then disable the user’.";
        /*--------------User Detail---------------*/
        /*--------------Survay---------------*/
        public static string SURVAY_MASTER_ADD = "api/survey/Add";
        public static string SURVAY_MASTER_EXISTS = "api/survey/exists?suveryNo={0}&villageId={1}&suveryId={2}&site={3}&projid={4}";

        public static string SURVAY_MASTER_EDIT = "api/survey/update";
        public static string SURVAY_MASTER_GET = "api/survey/getall";
        public static string SURVAYNO_EXIST_MSG = "Survey No already exists.";
        /*--------------Village---------------*/
        /*------------------------Forgot Password-----------------------*/
        public static string FORGOT_PASSWORD = "api/account/forgotpassword";
        public static string VALIDATE_CREDETIALS = "api/account/ValidateCredentials";
        /*------------------------Forgot Password-----------------------*/

        /*------------------------Table Master----------------------*/
        public static string TABLE_MASTER_GET = "api/table/getall";
        public static string TABLE_MASTER_ADD = "api/table/add";
        public static string TABLE_MASTER_EXISTS = "api/table/exists";
        public static string TABLE_MASTER_EDIT = "api/table/update";
        public static string TABLE_GET_PROJECTBYSITE = "api/values/getprojectbysite?name={0}";
        public static string TABLE_ALREADY_EXISIT = "Table already exists.";
        /*------------------------Table Master----------------------*/
        /*------------------------IssueCategory Master----------------------*/
        public static string ISSUECAT_MASTER_GET = "api/issuecategory/getall";
        public static string ISSUECAT_MASTER_ADD = "api/issuecategory/add";
        public static string ISSUECAT_MASTER_EXISTS = "api/issuecategory/exists?name={0}&id={1}";
        public static string ISSUECAT_MASTER_EDIT = "api/issuecategory/update";
        public static string ISSUECAT_ALREADY_EXISIT = "Category already exists.";
        /*------------------------IssueCategory Master----------------------*/

        /*------------------------Escalation Matrix----------------------*/
        public static string ESCALATION_MATRIX_GET = "api/escalationmatrix/getall";
        public static string ESCALATION_MATRIX_ADD = "api/escalationmatrix/add";
        public static string ESCALATION_MATRIX_UPDATE = "api/escalationmatrix/update";
        /*------------------------Escalation Matrix----------------------*/
        /*------------------------Table Activity----------------------*/
        public static string TABLEACTIVITY_GETAREA = "api/values/getarea?siteId={0}&projId={1}";
        public static string TABLEACTIVITY_GETNETWORK = "api/values/getnetwork?projId={0}&area={1}";
        public static string TABLEACTIVITY_GETACTIVITY = "api/values/getactivity?projId={0}&area={1}&network={2}";
        public static string TABLEACTIVITY_GETSUBACTIVITY = "api/values/getsubactivity?projId={0}&area={1}&network={2}&activity={3}";
        public static string TABLEACTIVITY_ADD = "api/tableactivity/add";
        public static string TABLEACTIVITY_EXIST = "api/tableactivity/exists";
        public static string TABLEACTIVITY_EXISTBLOCK = "Block already exist.";
        public static string TABLEACTIVITY_EXISTTABLE = "Table already exist.";
        public static string TABLEACTIVITY_EXISTSCB = "SCB already exist.";
        public static string TABLEACTIVITY_EXISTINVERTOR = "Invertor already exist";

        public static string SAPGETAREA_BYSITEPROJ = "api/values/getareabysiteproj";

        /*------------------------Table Activity---------------------*/
        /*------------------------Issue Management----------------------*/

        public static string ISSUE_MGT_GET_ASSSIGN_TO_ME = "api/issuemgmt/getassignedtome?searchText={0}";
        public static string ISSUE_MGT_GET_ASSSIGN_BY_ME = "api/issuemgmt/getraisedbyme?searchText={0}";
        public static string ISSUE_MGT_GET_ALL_ISSUES = "api/issuemgmt/getallissues?searchText={0}";

        public static string ISSUE_MGT_ADD_ISSUE = "api/issuemgmt/add";
        public static string ISSUE_MGT_UPDATE_ISSUE = "api/issuemgmt/update";
        public static string ISSUE_MGT_GET_ISSUEDETAIL = "api/issuemgmt/getissuedetail?issueId={0}";
        public static string ISSUE_MGT_GET_ASSIGNHISTORY = "api/issuemgmt/getissueassignhistory?issueId={0}";



        public static string ISSUE_MGT_All_ISSUE_ACCESSADMIN = "ADMIN";
        public static string ISSUE_MGT_All_ISSUE_ACCESS_FUNCTIONAL_MANAGER = "SITE FUNCTIONAL MANAGER";
        public static string ISSUE_MGT_All_ISSUE_ACCESS_STATE_HEAD = "STATE PROJECT HEAD";
        public static string ISSUE_MGT_All_ISSUE_CO_ORDINATOR = "CO-ORDINATOR";
        public static string ISSUE_MGT_ENTER_COMMENT = "Please enter comment.";

        /*------------------------Issue Management----------------------*/

        /*------------------------Task Allocation Detail----------------------*/
        public static string GET_TASK_ALLOCATION_DETAIL = "api/Task/getalocatedtask?projectId={0}&sapSite={1}";
        public static string GET_NOT_ALLOCATED_TASK = "api/Task/getnotalocatedtask?projectId={0}&sapSite={1}";
        /*------------------------Task Allocation Detail----------------------*/

        /*-----------------------Session name-----------------------*/
        public const string CONST_SESSION_TIMESHEET_PARAMETER = "timeSheetParam";
        public const string CONST_SESSION_UPLOAD_DOCUMENT_PARAMETER = "uploadDocumentParam";
        public const string CONST_SESSION_TIMESHEET_ID = "TimesheetId";
        public const string CONST_SESSION_ACTIVITY_ID = "ActivityId";
        public const string CONST_SESSION_SUBACTIVITY_ID = "SubActivityId";
        public const string CONST_SESSION_ACTIVITY_DESCRIPTION = "ActivityDescription";
        public const string CONST_SESSION_SUBACTIVITY_DESCRIPTION = "SAPSubActivityDescription";
        public const string CONST_SESSION_TIMESHEET_USERID = "TimesheetUserId";
        public const string CONST_SESSION_IS_PARTIAL_APPROVE = "PartialApprove";
        public const string CONST_SESSION_DEDOCUMENT_ACEESS = "DEDocumentAccess";
        public const string CONST_SESSION_DEDOCUMENT_ISREVIEW = "IsDocumentReview";
        /*-----------------------End -Session name-----------------------*/

        /*-----------------------Start Timesheet constants---------------------------------*/
        public const string CONST_SESSION_MANAGER_INVERTOR_LIST = "SessionManagerInvertorList";
        public const string CONST_SESSION_MANAGER_SURVEY_LIST = "SessionManagerSurveyList";
        public const string CONST_SESSION_MANAGER_BLOCK_LIST= "SessionManagerBlockList";

        public const string CONST_TIMESHEET_INVEROER_RANGE_DETAILS_MANAGER = "TimeSheetInverterDetailsManager";
        public const string CONST_TIMESHEET_INVEROER_MANUAL_DETAILS_MANAGER = "TimeSheetInverterManualDetailsManager";
        public const string CONST_TIMESHEET_USER_BLOCK_DETAILS = "UserTimeSheetBlockDetails";
        public const string CONST_TIMESHEET_USER_SURVEY_DETAILS = "UserTimeSheetSurveyDetails";
        /*-----------------------End Timesheet constants---------------------------------*/

        public const string CHANGE_PASSWORD = "api/account/changepassword";

        public const string CONST_SELECT_PROJECT_TEXT = "--- Select Project ---";
        public const string CONST_SELECT_SITE_TEXT = "--- Select Site ---";
        public const string CONST_SELECT_USER = "--- Select User ---";
        public const string CONST_SELECT_TEXT = "--- Select ---";
        public const string CONST_ALL_TEXT = "All";

        public const string CONST_SITE_FUNCTIONAL_USER = "Site Functional User";
        public const string CONST_SITE_FUNCTIONAL_MANAGER = "Site Functional Manager";
        public const string CONST_DESIGN_ENGINEER = "Design Engineer";
        public const string CONST_DESIGN_MANAGER = "Design Manager";
        public const string CONST_STATE_PROJECT_HEAD = "State Project Head";
        public const string CONST_SITE_QUALITY_MANAGER = "Site Quality Manager";
        public const string CONST_SITE_ADMIN = "Admin";
        public const string CONST_SITE_COORDINATOR = "Co-ordinator";
        public const string CONST_TIMESHEET_ACTIVITY_TYPE_RANGE = "Range";
        public const string CONST_TIMESHEET_ACTIVITY_TYPE_MANUAL = "Manual";
        public const string CONST_TABLE_ACTIVITY_TABLE = "Table";
        public const string CONST_TABLE_ACTIVITY_SCB = "SCB";
        public const string CONST_TABLE_ACTIVITY_INVERTER = "Inverter";
        public const string CONST_TABLE_ACTIVITY_BLOCK = "Block";
        public const string CONST_TABLE_ACTIVITY_NUMBER = "Number";
        public const string CONST_TABLE_ACTIVITY_LOCATION = "Location";

        /*------------------------To Do List----------------------*/
        public static string GET_AREA_BY_SITE_PROJECT = "api/values/getarea?siteId={0}&projectId={1}";
        public static string GET_NETWORK_BY_AREAID = "api/network/getall?areaId={0}&searchText={1}";
        /*------------------------End To Do List----------------------*/

        /*----------------------Start -SAP------------------------*/

        public static string SAP_ERROR = "Error occurred while connection with SAP";
        public static string SUCCESS = "Success";

        /*---------------------End-SAP---------------------------*/

        /*--------------------Reports----------------------------*/
        public const string SESSION_CONST_REPORT_NAME = "ReportName";
        /*--------------------Reports----------------------------*/

        /*-----------------------------------------*/
        public const string DESIGN_ENGINEER_DOC_PATH_TEMP = "Upload\\DEDocumentTemp\\";
        public const string DESIGN_ENGINEER_DOC_PATH = "Upload\\DEDocument\\";
        public const string DESIGN_ENGINEER_DELETED_DOC_PATH = "Upload\\DEDocument\\Deleted";
        /*----------------------------------------*/

        #region "Grid Constants"

        public const int CONST_GRID_PAGE_SIZE = 10;
        public const string CONST_GRID_RECORD_DETAILS = "records matching your search criteria";
        public const string CONST_GRID_RECORD_PER_PAGE = "Records per page:";

        public const string CONST_ERROR_MAX_150_CHARACTERS = "Maximum 150 characters allowed.";
        public const string CONST_ERROR_MAX_135_CHARACTERS = "Maximum 135 characters allowed.";
        public const string CONST_ERROR_MAX_200_CHARACTERS = "Maximum 50 characters allowed.";

        #endregion

        #region "Messages"

        public const string CONST_RECORD_ALREADY_EXIST = "Record already exists";
        public const string CONST_MESSAGE_SENT = "e-Mail is send to your registered email id.";
        public const string CONST_MESSAGE_SENDING_FAIL = "Error in sending email. Please try again.";
        public const string OLD_PASSWORD_NOT_MATCH = "Please enter correct Current password";
        public const string OLD_PASSWORD_SHOULD_NOT_SAME = "Current password and new password should not be same.";
        public const string PASSWORD_UPATED = "Password changed successfully.";
        public const string PASSWORD_UPATE_FAILED = "Failed to Update Password.";
        public const string ERROR_OCCURED_WHILE_UPDATING = "Error occured while updating.";
        public const string UPLOAD_RECORD_MESSAGE = " Record Uploaded Sucessfully: ";
        public const string NOTUPLOAD_RECORD_MESSAGE = " Records Not Uploaded: ";
        public const string TOTAL_RECORD_MESSAGE = "Total Records: ";
        public const string EXCELSHEETERRORMSG = "Name Of the worksheet must be Sheet1";
        public const string FILEExTENSION = "Please Upload only .xls,.xlsx files";
        public const string ERROR_OCCURED_WHILE_SAVING = "Error occured while saving.";
        public const string RECORD_SAVE_SUCESSFULLY = "Record save successfully.";
        public const string REST_CALL_FAILURE = "Rest call Failed.";
        public const string BLANK_EXCELSHEET_MSG = "Uploaded sheet is blank";
        public const string FAILED_TO_SEND_NOTIFICATION = "Failed to send notification to user. Please contact administrator.";
        public const string INVALID_FILE_TYPE = "Invalid file type.";
        public const string INVALID_TEMPLATE_FILE = "Invalid template file.";
        public const string BLOCK_SAVED = "Block saved successfully.";
        public const string TABLE_SAVED = "Table saved successfully.";
        public const string SCB_SAVED = "SCB saved successfully.";
        public const string INVERTOR_SAVED = "Inverter saved successfully.";
        #endregion

        #region "enum"

        public enum TASKMaster
        {
            Area = 1,
            Network = 2,
            Activity = 3,
            SubActivity = 4
        }

        public enum WorkflowStatus
        {
            QAApprovalPending = 1,
            QARejected = 2,
            QAApproved = 3,
            PMApprovalPending = 4,
            PMRejected = 5,
            PMApproved = 6,
            AutoApproved = 7,
            PartialApprovedByQM = 8,
            PartialApprovedByPM = 9
            
        }

        public enum TimesheetStatus
        {
            Ongoing = 1,
            Draft = 2,
            Closed = 3
        }

        public enum NotificationType
        {

            ActivityIssueAssigned = 1,
            ActivityIssueResolution = 2,
            ActivityIssueClosed = 3,
            IssueAssigned = 4,
            IssueResolution = 5,
            IssueClosed = 6,
            QualityRejection = 7,
            QualityResolution = 8,
            QualityRejectClosed = 9,
            SiteManagerRejection = 10,
            NewUserCreation = 11,
            UserDeactivation = 12,
            ActivityAllocation = 13,
            ActivityRemoved = 14,
            IssueNotResolved = 15,
            IssueNotClosed = 16,
            QualityResolutionEscalation = 17,
            NoUserLogin = 18,
            DEDocumentUpload = 19
        }

        public enum ReportName
        {
            AllCommon = 1,
            LandAnalysis = 2,
            IssuesManagement = 3,
            DailyReport = 4,
            RejectionReport = 5,
            ECblock = 6,
            ECInverter = 7,
            ECSCB = 8,
            ECTable = 9,
            PE = 10,
            DE = 11,
            ApprovalTimesheets = 12,
            Material = 13
        }

        public enum DEDocumentUploadAccess
        {
            None = 0,
            View = 1,
            Full = 2
        }

        public enum ManagerTimesheetStatus
        {
            Rejected = 0,
            Approved = 1
        }

        public enum ManagerType
        {
            QM = 1,
            PM = 2
        }

        public enum Shift
        {
            A = 1,
            B = 2,
            C = 3,
            General = 4
        }
        #endregion
    }
}
