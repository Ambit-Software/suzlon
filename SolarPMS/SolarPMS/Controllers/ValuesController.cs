using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Filters;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SolarPMS.Controllers
{
    [ExceptionHandlingFilter]
    [Authorize]
    [RoutePrefix("api/values")]
    public class ValuesController : BaseApiController
    {
        [Route("populatedropdowns")]
        [HttpGet]
        public IHttpActionResult PopulateDropdowns(string name)
        {

            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.PopulateDropdowns(name));

        }

        [Route("getprojectbysite")]
        [HttpGet]
        public IHttpActionResult GetProjectsBySite(string name)
        {

            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.GetProjectsBySite(name));
        }

        [Route("getareabysiteproj")]
        [HttpPost]
        public IHttpActionResult GetAreaBySiteProj(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            SapMasterValues sapMasterValues = JsonConvert.DeserializeObject<SapMasterValues>(paramDetail);
            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.GetAreas(sapMasterValues.site, sapMasterValues.project));
        }

        [Route("getallocatedarea")]
        [HttpPost]
        public IHttpActionResult GetAllocatedArea(PostParam param)
        {

            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TaskModel.ToDoTask toDoTask = JsonConvert.DeserializeObject<TaskModel.ToDoTask>(paramDetail);
            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.GetAllocatedArea(UserId, toDoTask.site, toDoTask.project));

        }
        [Route("getnetwork")]
        [HttpGet]
        public IHttpActionResult getnetwork(string projId, string area)
        {

            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.GetNetworks(projId, area));

        }
        [Route("getactivity")]
        [HttpGet]
        public IHttpActionResult getactivity(string projId, string area, string network)
        {

            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.GetActivity(projId, area, network));

        }
        [Route("getsubactivity")]
        [HttpGet]
        public IHttpActionResult getsubactivity(string projId, string area, string network, string activity)
        {

            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.GetSubActivity(projId, area, network, activity));

        }
    }
}
