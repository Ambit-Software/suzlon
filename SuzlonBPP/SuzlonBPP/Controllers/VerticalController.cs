using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Filters;
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
    [RoutePrefix("api/vertical")]
    public class VerticalController : BaseApiController
    {
        private VerticalModel verticalModel = new VerticalModel();

        [HttpGet]
        // GET: api/vertical
        [Route("getall")]
        public IHttpActionResult GetVerticals()
        {
            return Ok(verticalModel.GetVerticals());
        }

        // POST: api/vertical
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddVertical(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Models.VerticalMaster verticalMaster = JsonConvert.DeserializeObject<Models.VerticalMaster>(paramDetail);
            verticalMaster = verticalModel.AddVertical(verticalMaster, UserId);
            return Ok(verticalMaster);
        }

        // POST: api/vertical
        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateVertical(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Models.VerticalMaster verticalMaster = JsonConvert.DeserializeObject<Models.VerticalMaster>(paramDetail);
            bool isUpdated = verticalModel.UpdateVertical(verticalMaster, UserId);
            return Ok(isUpdated);
        }

        [Route("exists")]
        [HttpGet]
        // GET: api/vertical/exists
        public IHttpActionResult VerticalExists(string name, int verticalId)
        {
            bool isExists = verticalModel.VerticalExists(name, verticalId);
            return Ok(isExists);
        }

        [Route("getverticalsbyuser")]
        [HttpGet]
        // GET: api/vertical/GetVerticalsByUser
        public IHttpActionResult GetVerticalsByUser(string UserId)
        {
            List<Models.VerticalMaster> lstVerticalMaster =  verticalModel.GetVerticalsByUser(UserId);
            return Ok(lstVerticalMaster);
        }
    }
}
