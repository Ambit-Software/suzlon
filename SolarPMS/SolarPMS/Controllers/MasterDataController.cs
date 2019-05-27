using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SolarPMS.Controllers
{
    [Authorize]
    [RoutePrefix("api/masterdata")]
    public class MasterDataController : BaseApiController
    {
        [Route("getworkflowstaus")]
        public IHttpActionResult GetWorkFlowStatus()
        {
            return Ok(MasterDataModel.GetWorkflowStatus());
        }
        [Route("gettimesheetstatus")]
        public IHttpActionResult GetTimesheetStatus()
        {
            return Ok(MasterDataModel.GetTimesheetStatus());
        }
        [Route("gettablemaster")]
        public IHttpActionResult GetTableMaster(int UserId)
        {
            return Ok(MasterDataModel.GetTableMasterData(UserId));
        }

        [Route("gettableactivity")]
        [HttpGet]
        public IHttpActionResult GetTableActivityMaster(int UserId)
        {
            return Ok(MasterDataModel.GetTableAtctivityMasterData(UserId));
        }

        [Route("getsurveymaster")]
        [HttpGet]
        public IHttpActionResult GetSurveyActivityMaster(int UserId)
        {
            return Ok(MasterDataModel.GetSurveyMasterData(UserId));
        }

        [Route("getvillagemaster")]
        [HttpGet]
        public IHttpActionResult GetVillageMaster()
        {
            return Ok(MasterDataModel.GetVillageMasterData());
        }

        [Route("getupdatedmastertables")]
        [HttpGet]
        public IHttpActionResult GetUpdatedMasterTables(string LatsSyncDate)
        {
            return Ok(MasterDataModel.GetUpdatedMasterTables(Convert.ToDateTime(LatsSyncDate)));
        }

        [Route("getcontractors")]
        [HttpGet]
        public IHttpActionResult GetContractors(int UserId)
        {
            return Ok(MasterDataModel.GetContractors(UserId));
        }

        [Route("getmanpowermaster")]
        [HttpGet]
        public IHttpActionResult GetManPowerMaster(int UserId)
        {
            return Ok(MasterDataModel.GetManPowerMaster(UserId));
        }
    }
}