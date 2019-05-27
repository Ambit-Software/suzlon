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
    [RoutePrefix("api/subactivity")]
    public class SubActivityController : BaseApiController
    {

        private SubActivityModel suActivityModel = new SubActivityModel();

        [Route("getsubactivitybyactivityid")]
        public IHttpActionResult GetNetworkByAreaId(int activityId)
        {
            return Ok(suActivityModel.GetSubActivityByActivityID(UserId, activityId));
        }


    }
}
