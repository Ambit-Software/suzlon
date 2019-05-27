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
    [RoutePrefix("api/setting")]
    [Authorize]
    public class SettingController : BaseApiController
    {
        SettingModel settingModel = new SettingModel();
                
        [Route("getapplsettings")]
        public IHttpActionResult GetApplSettings()
        {
            return Ok(settingModel.GetApplSettings());
        }
            
        [Route("saveapplsettings")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult SaveApplSettings(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            Models.ApplicationConfiguration setting = JsonConvert.DeserializeObject<Models.ApplicationConfiguration>(paramDetail);
            bool isUpdated = settingModel.SaveApplSettings(setting, UserId);
            return Ok(isUpdated);
        }
    }
}
