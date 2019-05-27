using SuzlonBPP.Models;
using System.Web.Http;

namespace SuzlonBPP.Controllers
{
    [System.Web.Http.RoutePrefix("api/error")]
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