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
    [RoutePrefix("api/error")]
    public class ErrorLogController : BaseApiController
    {

        [Route("errorlog")]
        [HttpGet]
        public IHttpActionResult ErrorLog(string Message, string StackTrace, string PageName, string FunctionName)
        {
            CommonFunctions.WriteErrorLogMobile(Message, StackTrace, PageName, FunctionName, UserId);
            return Ok();
        }

    }
}
