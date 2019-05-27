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
using System.Web.Http.Description;

namespace SolarPMS.Controllers
{
    [ExceptionHandlingFilter]
    [Authorize]
    [RoutePrefix("api/village")]
    public class VillageController : BaseApiController
    {
        private VillageModel villageModel = new VillageModel();

        [HttpGet]
        // GET: api/Village
        [Route("getall")]
        public IHttpActionResult GetVillages()
        {
          
                return Ok(villageModel.GetVillages());
            
        }

        // POST: api/Village
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddVillage(PostParam param)
        {
            
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                VillageMaster villageMaster = JsonConvert.DeserializeObject<VillageMaster>(paramDetail);
                villageMaster = villageModel.AddVillage(villageMaster, UserId);
                return Ok(villageMaster);
            
        }

        // POST: api/Village
        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateVillage(PostParam param)
        {
            
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                VillageMaster villageMaster = JsonConvert.DeserializeObject<VillageMaster>(paramDetail);
                bool isUpdated = villageModel.UpdateVillage(villageMaster, UserId);
                return Ok(isUpdated);
           
        }

        [Route("exists")]
        [HttpGet]
        // POST: api/Village/exists/1
        public IHttpActionResult VillageExists(string name, int id)
        {
           
                bool isExists = villageModel.VillageExists(name, id);
                return Ok(isExists);
           
        }
    }
}