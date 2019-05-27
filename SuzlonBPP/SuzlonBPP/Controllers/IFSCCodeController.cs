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
    [RoutePrefix("api/ifsccode")]
    public class IFSCCodeController : BaseApiController
    {
        private IFSCCodeModel IFSCCodeModel = new IFSCCodeModel();

        [HttpGet]
        // GET: api/ifsccode
        [Route("getall")]
        public IHttpActionResult GetIFSCCodes()
        {
            return Ok(IFSCCodeModel.GetIFSCCodes());
        }

        // POST: api/ifsccode
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddIFSCCode(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            IFSCCodeMaster iFSCCodeMaster = JsonConvert.DeserializeObject<IFSCCodeMaster>(paramDetail);
            iFSCCodeMaster = IFSCCodeModel.AddIFSCCode(iFSCCodeMaster, UserId);
            return Ok(iFSCCodeMaster);
        }

        // POST: api/ifsccode
        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateIFSCCode(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            IFSCCodeMaster iFSCCodeMaster = JsonConvert.DeserializeObject<IFSCCodeMaster>(paramDetail);
            bool isUpdated = IFSCCodeModel.UpdateIFSCCode(iFSCCodeMaster, UserId);
            return Ok(isUpdated);
        }

        [Route("exists")]
        [HttpGet]
        // GET: api/ifsccode/exists
        public IHttpActionResult IFSCCodeExists(string iFSCCode, int iFSCCodeId)
        {
            bool isExists = IFSCCodeModel.IFSCCodeExists(iFSCCode, iFSCCodeId);
            return Ok(isExists);
        }
    }
}
