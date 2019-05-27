using System;
using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Models;
using System.Web.Http;
using System.Collections.Generic;

namespace SolarPMS.Controllers
{
    [Authorize]
    [RoutePrefix("api/manpower")]
    public class ManPowerController : BaseApiController
    {
        [Route("getcontractordetailslist")]
        public IHttpActionResult GetContractorDetailsList(int Id)
        {
            return Ok(ManPowerModel.GetContractorDetailsList(Id));
        }

        [HttpPost]
        [Route("getcontractordetails")]
        public IHttpActionResult GetContractorDetails(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ManPowerDetail manPowerDetail = JsonConvert.DeserializeObject<ManPowerDetail>(paramDetail);
            return Ok(ManPowerModel.GetContractorDetails(manPowerDetail.Site, manPowerDetail.Project, manPowerDetail.AreaId, manPowerDetail.Network, manPowerDetail.Date, manPowerDetail.CreatedBy));
        }

        [Route("ismanpowerdetailsexists")]
        [HttpPost]
        public IHttpActionResult IsManPowerDetailsExists(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ManPowerDetail manPowerDetail = JsonConvert.DeserializeObject<ManPowerDetail>(paramDetail);
            return Ok(ManPowerModel.IsManPowerDetailsExists(manPowerDetail));
        }

        [Route("syncofflinemanpower")]
        [HttpPost]
        public IHttpActionResult SyncOfflineManpower(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            List<ManPowerDetail> manPowerDetail = JsonConvert.DeserializeObject<List<ManPowerDetail>>(paramDetail);

            string savedDetailsIds = string.Empty;
            foreach (ManPowerDetail detail in manPowerDetail)
            {
                int offlineId = detail.Id;
                detail.Id = 0;
                detail.Date = detail.Date.ToLocalTime();
                detail.CreatedOn = detail.CreatedOn.ToLocalTime();
                string reson = string.Empty;
                if (!ManPowerModel.IsManPowerDetailsExists(detail))
                {
                    if (ManPowerModel.SaveManPowerDetails(detail) != 0)
                        savedDetailsIds += offlineId.ToString() + ";";
                }
                else
                {
                    reson = "Duplicate man power details";
                    savedDetailsIds += offlineId.ToString() + ";";
                }

                ManPowerModel.SaveOfflineManPowerDetails(detail, reson, detail.Id != 0);
            }


            return Ok(savedDetailsIds);
        }

        [Route("savemanpowerdetails")]
        [HttpPost]
        public IHttpActionResult SaveManPowerDetails(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ManPowerDetail manPowerDetail = JsonConvert.DeserializeObject<ManPowerDetail>(paramDetail);
            return Ok(ManPowerModel.SaveManPowerDetails(manPowerDetail));
        }

        [Route("getcontractorlist")]
        public IHttpActionResult GetContractorList(string Site, string Project)
        {
            return Ok(ManPowerModel.GetContractorList(Site,Project));
        }

        [Route("delete")]
        [HttpGet]
        public IHttpActionResult Delete(int Id, int UserId)
        {
            return Ok(ManPowerModel.Delete(Id, UserId));
        }
    }
}