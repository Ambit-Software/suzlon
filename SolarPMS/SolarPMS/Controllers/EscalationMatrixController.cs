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
    [RoutePrefix("api/escalationmatrix")]
    [Authorize]
    public class EscalationMatrixController : BaseApiController
    {
        private EscalationMatrixModel escalationmatrixmodel = new EscalationMatrixModel();

        [HttpGet]
        // GET: api/escalationmatrix
        [Route("getall")]
        public IHttpActionResult GetEscalationMatrix()
        {
            
                return Ok(escalationmatrixmodel.GetEscaltionMatrix());
           
        }



        // POST: api/Profile
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddEscalationMatrix(PostParam param)
        {
            
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                EscalationMatrix escaltionmatrix = JsonConvert.DeserializeObject<EscalationMatrix>(paramDetail);
                escaltionmatrix = escalationmatrixmodel.AddEscaltion(escaltionmatrix, UserId);
                return Ok(escaltionmatrix);           
        }
        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateEscalationMatrix(PostParam param)
        {
          
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                EscalationMatrix escalationmatrix = JsonConvert.DeserializeObject<EscalationMatrix>(paramDetail);
                bool isUpdated = escalationmatrixmodel.UpdateEscalation(escalationmatrix, UserId);
                return Ok(isUpdated);
           
        }



    }
}
