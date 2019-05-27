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
    [Authorize]
    [ExceptionHandlingFilter]
    [RoutePrefix("api/survey")]
    public class SurveyController : BaseApiController
    {
        SurveyModel surveyModel = new SurveyModel();

        [Route("getall")]
        public IHttpActionResult GetAllSurveyInfo()
        {

            return Ok(surveyModel.GetAllSurveyInfo());

        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddSurvey(PostParam param)
        {

            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            SurveyMaster surveyMaster = JsonConvert.DeserializeObject<SurveyMaster>(paramDetail);
            return Ok(surveyModel.AddSurvey(surveyMaster, UserId));

        }

        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateSurveyDetail(PostParam param)
        {

            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            SurveyMaster surveyMaster = JsonConvert.DeserializeObject<SurveyMaster>(paramDetail);
            bool isUpdated = surveyModel.UpdateSurveyDetail(surveyMaster, UserId);
            return Ok(isUpdated);

        }

        [Route("exists")]
        [HttpGet]
        // POST: api/survey/exists/1
        public IHttpActionResult SurveyExists(string suveryNo, int villageId, int suveryId, string site, string projid)
        {

            bool isExists = surveyModel.SurveyExists(suveryNo, villageId, suveryId, site, projid);
            return Ok(isExists);

        }

        [Route("getvillage")]
        [HttpPost]
        // POST: api/survey/exists/1
        public IHttpActionResult GetVillage(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            SurveyMaster surveyMaster = JsonConvert.DeserializeObject<SurveyMaster>(paramDetail);
            return Ok(surveyModel.GetVillage(surveyMaster));

        }

        [Route("getcompletedquantity")]
        [HttpGet]
        // POST: api/survey/exists/1
        public IHttpActionResult GetCompletedQuantity(string SubActivity, string Activity, string Network, int AreaId, string Project, string Site, int VillageId, string SurveyNo, int TimesheetId)
        {
            return Ok(SurveyModel.GetCompletedQuantity(SubActivity, Activity, Network, AreaId, Project, Site, VillageId, SurveyNo, TimesheetId));
        }

        [Route("getallsurveynumberforvillage")]
        [HttpGet]
        // POST: api/survey/exists/1
        public IHttpActionResult GetAllSurveyNumberForVillage(int VillageId, string SAPSite, string SAPProjectId)
        {
            return Ok(TimesheetModel.GetAllSurveyNumberForVillage(VillageId, SAPSite, SAPProjectId));
        }
    }
}
