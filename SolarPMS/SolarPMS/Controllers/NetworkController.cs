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
    [RoutePrefix("api/network")]
    public class NetworkController : BaseApiController
    {
        private NetworkModel networkModel = new NetworkModel();

        [HttpGet]
        // GET: api/issuecategory/getall
        [Route("getall")]
        public IHttpActionResult GetNetwork(int areaId, string searchText)
        {
            return Ok(networkModel.GetNetwork(UserId, areaId, searchText));

        }


        [Route("getnetworkbyareaid")]
        public IHttpActionResult GetNetworkByAreaId(int areaId)
        {
            return Ok(networkModel.GetNetworkByAreaId(UserId, areaId));

        }


        [Route("getnetworksforarea")]
        [HttpGet]
        public IHttpActionResult GetNetworksForArea(string siteId, string projectId, int areaId)
        {
            return Ok(networkModel.GetNetworks(UserId, siteId, projectId, areaId));

        }


    }
}