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
    [RoutePrefix("api/timesheetapproval")]
    public class TimesheetApprovalController : BaseApiController
    {
        TimesheetApprovalModel timesheetApprovalModel = new TimesheetApprovalModel();

        [HttpPost]
        [Route("addtimesheetapproval")]
        public IHttpActionResult AddQAApproval(PostParam param)
        {

            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Timesheet_Approval timesheetApproval = JsonConvert.DeserializeObject<Timesheet_Approval>(paramDetail);
            return Ok(timesheetApprovalModel.Add(timesheetApproval, UserId));


        }
    }
}
