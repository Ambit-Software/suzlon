using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using SuzlonBPP.Controllers;
using SuzlonBPP.Models;
using System.Web.Http;
using System.Web.Http.Description;

namespace SolarPMS.Controllers
{
    [Authorize]
    [RoutePrefix("api/naturerequest")]
    public class NatureRequestController : BaseApiController
    {
        private NatureRequestModel requestModel = new NatureRequestModel();

        [HttpGet]
        // GET: api/naturerequest
        [Route("getall")]
        public IHttpActionResult GetRequests()
        {
            return Ok(requestModel.GetRequests());
        }

        // POST: api/naturerequest
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddRequest(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            NatureRequestMaster requestMaster = JsonConvert.DeserializeObject<NatureRequestMaster>(paramDetail);
            requestMaster = requestModel.AddRequest(requestMaster, UserId);
            return Ok(requestMaster);
        }

        // POST: api/naturerequest
        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateRequest(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            NatureRequestMaster requestMaster = JsonConvert.DeserializeObject<NatureRequestMaster>(paramDetail);
            bool isUpdated = requestModel.UpdateRequest(requestMaster, UserId);
            return Ok(isUpdated);
        }

        [Route("exists")]
        [HttpGet]
        // GET: api/naturerequest/exists
        public IHttpActionResult RequestExists(string name, int RequestId)
        {
            bool isExists = requestModel.RequestExists(name, RequestId);
            return Ok(isExists);
        }
    }
}
