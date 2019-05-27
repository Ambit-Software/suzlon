using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SuzlonBPP.Controllers
{
    [Authorize]
    [RoutePrefix("api/subvertical")]
    public class SubVerticalController : BaseApiController
    {
        private SubVerticalModel subVerticalModel = new SubVerticalModel();

        [HttpGet]
        // GET: api/subvertical
        [Route("getall")]
        public IHttpActionResult GetSubVerticals()
        {
            return Ok(subVerticalModel.GetSubVerticals());
        }

        [HttpGet]
        // GET: api/subvertical/getbasedonvertical?verticalId=value
        [Route("getbasedonvertical")]
        public IHttpActionResult GetSubVerticalBasedOnVertical(string verticalIds)
        {
            return Ok(subVerticalModel.GetSubVerticalBasedOnVertical(verticalIds));
        }

        // POST: api/subvertical
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddSubVertical(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Models.SubVerticalMaster subVerticalMaster = JsonConvert.DeserializeObject<Models.SubVerticalMaster>(paramDetail);
            subVerticalMaster = subVerticalModel.AddSubVertical(subVerticalMaster, UserId);
            return Ok(subVerticalMaster);
        }

        // POST: api/subvertical
        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateSubVertical(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Models.SubVerticalMaster subVerticalMaster = JsonConvert.DeserializeObject<Models.SubVerticalMaster>(paramDetail);
            bool isUpdated = subVerticalModel.UpdateSubVertical(subVerticalMaster, UserId);
            return Ok(isUpdated);
        }

        [Route("exists")]
        [HttpGet]
        // GET: api/subvertical/exists
        public IHttpActionResult SubVerticalExists(string name, int subverticalId, int verticalId)
        {
            bool isExists = subVerticalModel.SubVerticalExists(name, subverticalId, verticalId);
            return Ok(isExists);
        }

        [HttpPost]
        // GET: api/subvertical
        [Route("GetSubVerticalsByUser")]
        public IHttpActionResult GetSubVerticalsByUser(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            UserModel UserProfile = JsonConvert.DeserializeObject<UserModel>(paramDetail);
            return Ok(subVerticalModel.GetSubVerticalsByUser(UserProfile));
        }
    }
}
