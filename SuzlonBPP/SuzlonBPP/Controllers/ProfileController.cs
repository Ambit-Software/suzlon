using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Filters;
using SuzlonBPP.Models;
using System.Web.Http;

namespace SuzlonBPP.Controllers
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

        //[Route("exists")]
        //[HttpGet]
        //// GET: api/profile/exists/1
        //public IHttpActionResult ProfileExist(string name, int profileId)
        //{
        //    bool isExists = profileModel.ProfileExist(name, profileId);
        //    return Ok(isExists);
        //}
        [Route("exists")]
        [HttpPost]
        // GET: api/profile/exists/1
        public IHttpActionResult ProfileExist(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ProfileModel profile = JsonConvert.DeserializeObject<ProfileModel>(paramDetail);
            bool isExists = profileModel.ProfileExist(profile);
            return Ok(isExists);
        }

        [HttpGet]
        // GET: api/Profile
        [Route("getmenulist")]
        public IHttpActionResult GetAllMenuList()
        {
            return Ok(profileModel.GetAllMenuList());

        }
    }
}
