using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SolarPMS.Models;
namespace SolarPMS.Controllers
{
    [Authorize]
    [RoutePrefix("api/material")]
    public class MaterialController : BaseApiController
    {
        [HttpGet]
        [Route("getmaterialreport")]
        public IHttpActionResult GetMaterialReportData(int UserId, string Site)
        {
            return Ok(MaterialMasterModel.GetMaterialReportData(UserId, Site));
        }

    }
}