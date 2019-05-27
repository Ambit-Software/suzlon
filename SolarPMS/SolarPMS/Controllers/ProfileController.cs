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
    [RoutePrefix("api/profile")]
    [Authorize]
    public class ProfileController : BaseApiController
    {
        private ProfileModel profileModel = new ProfileModel();

        [HttpGet]
        // GET: api/Profile
        [Route("getall")]
        public IHttpActionResult GetProfiles()
        {
           
                return Ok(profileModel.GetProfiles());

           
        }

        [HttpGet]
        // GET: api/Profile
        [Route("getmenulist")]
        public IHttpActionResult GetAllMenuList()
        {
          
                return Ok(profileModel.GetAllMenuList());

            
        }

        // POST: api/Profile
        [HttpPost]
        [Route("add")]
        public IHttpActionResult Addprofile(PostParam param)
        {
           
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                ProfileModel profile = JsonConvert.DeserializeObject<ProfileModel>(paramDetail);
                profileModel = profileModel.AddProfile(profile, UserId);
                return Ok(profileModel);
           
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateProfile(PostParam param)
        {
             var paramDetail = Crypto.Instance.Decrypt(param.Data);
                ProfileModel profile = JsonConvert.DeserializeObject<ProfileModel>(paramDetail);
                bool isUpdated = profileModel.UpdateProfile(profile, UserId);
                return Ok(isUpdated);
           
        }

        [Route("exists")]
        [HttpGet]
        // POST: api/profile/exists/1
        public IHttpActionResult ProfileExist(string name,int profileId)
        {
            
                bool isExists = profileModel.ProfileExist(name, profileId);
                return Ok(isExists);
           
        }
    }
}
