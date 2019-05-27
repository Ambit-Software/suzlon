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
    [RoutePrefix("api/location")]
    public class LocationController : BaseApiController
    {
        private LocationModel locationModel = new LocationModel();

        [HttpGet]
        // GET: api/Location
        [Route("getall")]
        public IHttpActionResult GetLocations()
        {
            
                return Ok(locationModel.GetLocations());
            
        }

        // POST: api/Location
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddLocation(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
                LocationMaster locationMaster = JsonConvert.DeserializeObject<LocationMaster>(paramDetail);
                locationMaster = locationModel.AddLocation(locationMaster, UserId);
                return Ok(locationMaster);
           
        }

        // POST: api/Location
        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateLocation(PostParam param)
        {
            
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                LocationMaster locationMaster = JsonConvert.DeserializeObject<LocationMaster>(paramDetail);
                bool isUpdated = locationModel.UpdateLocation(locationMaster, UserId);
                return Ok(isUpdated);
        
        }

        [Route("exists")]
        [HttpGet]
        // POST: api/Location/exists/1
        public IHttpActionResult LocationExists(string name, int locationId)
        {
          
                bool isExists = locationModel.LocationExists(name, locationId);
                return Ok(isExists);
       
        }
    }
}
