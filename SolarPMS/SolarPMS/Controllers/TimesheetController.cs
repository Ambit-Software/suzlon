using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SolarPMS.Controllers
{
    [Authorize]
    [RoutePrefix("api/timesheet")]
    public class TimesheetController : BaseApiController
    {
        TimesheetModel timesheetModel = new TimesheetModel();

        [HttpPost]
        [Route("addtimesheet")]
        public IHttpActionResult AddTimesheet(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheet = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.AddTimesheet(timesheet, UserId));
        }

        [HttpPost]
        [Route("syncofflinetimesheet")]
        public IHttpActionResult SyncOfflineTimesheet(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            List<Timesheet> lstTmesheet = JsonConvert.DeserializeObject<List<Timesheet>>(paramDetail);

            string savedTimesheetIds = string.Empty;
            foreach (Timesheet timesheet in lstTmesheet)
            {
                int offlineTimesheetId = timesheet.TimeSheetId;
                timesheet.TimeSheetId = 0;
                timesheet.ActualDate = timesheet.ActualDate.ToLocalTime();
                string validationResult = TimesheetModel.ValidateOfflineTimesheet(timesheet);
                if (String.IsNullOrEmpty(validationResult))
                    timesheet.TimeSheetId = timesheetModel.AddTimesheet(timesheet, UserId);

                timesheetModel.AddOfflineTimesheet(timesheet, timesheet.CreatedBy, timesheet.TimeSheetId != 0, validationResult);
                savedTimesheetIds += offlineTimesheetId.ToString() + ";";
            }

            return Ok(savedTimesheetIds);
        }

        [HttpPost]
        [Route("gettimesheetbytimesheetId")]
        public IHttpActionResult GetTimesheetByTimesheetId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheet = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.GetTimesheetByTimesheetId(timesheet));
        }


        [HttpPost]
        [Route("updatetimesheet")]
        public IHttpActionResult UpdateTimesheet(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheet = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.UpdateTimesheet(timesheet, UserId));
        }

        [HttpPost]
        [Route("gettimesheetsbyactivityId")]
        public IHttpActionResult GetTimesheetsByActivityId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheet = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.GetTimesheetsByActivityId(timesheet));
        }

        [HttpPost]
        [Route("gettimesheetsbysubactivityId")]
        public IHttpActionResult GetTimesheetsBySubActivityId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheet = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.GetTimesheetsBySubActivityId(timesheet));
        }

        [Route("getworkflow")]
        public IHttpActionResult GetWorkFlow()
        {
            return Ok(timesheetModel.GetWorkFlow());
        }

        [Route("getstatus")]
        public IHttpActionResult GetStatus()
        {
            return Ok(timesheetModel.GetStatus());
        }

        [HttpPost]
        [Route("gettotalactualquantitybyactivityId")]
        public IHttpActionResult GetTotalActualQuantityByActivityId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheet = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.GetTotalActualQuantityByActivityId(timesheet));
        }


        [HttpPost]
        [Route("gettotalactualquantitybysubactivityId")]
        public IHttpActionResult GetTotalActualQuantityBySubActivityId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheet = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.GetTotalActualQuantityBySubActivityId(timesheet));
        }

        [HttpPost]
        [Route("approverejecttimesheet")]
        public IHttpActionResult ApproveRejectTimesheet(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            List<Timesheet> lstTimesheet = JsonConvert.DeserializeObject<List<Timesheet>>(paramDetail);
            return Ok(timesheetModel.ApproveRejectTimesheet(lstTimesheet, UserId));
        }

        [HttpPost]
        [Route("gettimesheetapprovalcomment")]
        public IHttpActionResult GetTimesheetApprovalComment(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheetObj = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.GetTimesheetApprovalComment(timesheetObj.TimeSheetId));
        }

        [HttpPost]
        [Route("checktimesheetexistsfordate")]
        public IHttpActionResult CheckTimesheetExistsForDate(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheetObj = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(TimesheetModel.CheckTimesheetExistsForDate(timesheetObj, timesheetObj.ActualDate, UserId));
        }

        [HttpPost]
        [Route("partialapprovetimesheet")]
        public IHttpActionResult PartailApprove(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet timesheet = JsonConvert.DeserializeObject<Timesheet>(paramDetail);
            return Ok(timesheetModel.PartialApproveTimesheet(timesheet));
        }

        [HttpGet]
        [Route("ispartailapprovebyqm")]
        public IHttpActionResult IsPartailApproveByQM(int TimesheetId)
        {
            return Ok(TimesheetModel.IsPartailApproveByQM(TimesheetId));
        }

        [HttpGet]
        [Route("gettimesheetapprovedquantity")]
        public IHttpActionResult GetTimesheetApprovedQuantity(int TimesheetId)
        {
            return Ok(TimesheetModel.GetTimesheetApprovedQuantity(TimesheetId));
        }


        [HttpGet]
        [Route("getmanageractivity")]
        public IHttpActionResult GetManagerActivityDetails(int TimesheetId, string Flag)
        {
            return Ok(TimesheetModel.GetManagerActivityDetails(TimesheetId, Flag));
        }

        [HttpGet]
        [Route("managerblockdetails")]
        public IHttpActionResult GetManagerBlockDetails(int TimesheetId, string Flag)
        {
            return Ok(TimesheetModel.GetManagerBlockDetails(TimesheetId, Flag));
        }

        [HttpGet]
        [Route("getmanagersurveydetails")]
        public IHttpActionResult GetManagerSurveyDetails(int TimesheetId, string Flag)
        {
            return Ok(TimesheetModel.GetManagerSurveyDetails(TimesheetId, Flag));
        }

        [HttpGet]
        [Route("getmanagersurvey")]
        public IHttpActionResult GetManagerBlocks(int TimesheetId, string Flag)
        {
            return Ok(TimesheetModel.GetManagerBlockDetails(TimesheetId, Flag));
        }
    }
}