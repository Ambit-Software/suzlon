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
    [RoutePrefix("api/activity")]
    public class ActivityController : BaseApiController
    {
        private ActivityModel activityModel = new ActivityModel();

        [Route("getactivitybynetworkid")]
        public IHttpActionResult GetNetworkByAreaId(int networkid)
        {
            return Ok(activityModel.GetActivityByNetworkID(UserId, networkid));
        }

        [Route("getsapmasterbyactitiyid")]
        [HttpPost]
        public IHttpActionResult GetSAPMasterByActitiyId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Activity tableActivity = JsonConvert.DeserializeObject<Activity>(paramDetail);
            return Ok(activityModel.GetSAPMasterByActitiyId(tableActivity));
        }

        [Route("getsapmasterbysubactitiyid")]
        [HttpPost]
        public IHttpActionResult GetSAPMasterBySubActitiyId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            SubActivity subActivity = JsonConvert.DeserializeObject<SubActivity>(paramDetail);
            return Ok(activityModel.GetSAPMasterBySubActitiyId(subActivity));
        }

        [Route("getdashbaorddata")]
        [HttpPost]
        public IHttpActionResult GetMobileDashboardData(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TaskModel.ToDoTask activity = JsonConvert.DeserializeObject<TaskModel.ToDoTask>(paramDetail);
            return Ok(ActivityModel.GetMobileDashboardData(activity));
        }
    }
}
