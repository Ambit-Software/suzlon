using System.Net.Http;
using System.Web.Http;
using SuzlonBPP.Models;
using Microsoft.Owin.Security.Cookies;
using System.Web;
using Microsoft.AspNet.Identity;
using SuzlonBPP.Filters;
using System.Web.Http.Description;
using Cryptography;
using Newtonsoft.Json;

namespace SuzlonBPP.Controllers
{
    [RoutePrefix("api/account")]
    public class UserController : BaseApiController
    {
        UserModel userModel = new UserModel();       

        [Authorize]
        [Route("userdetail")]
        public IHttpActionResult GetUserInfo(int userId)
        {
            return Ok(userModel.GetUserInfo(userId));
        }

        [Authorize]
        [Route("alluser")]
        public IHttpActionResult GetAllUserInfo()
        {
            return Ok(userModel.GetAllUserInfo());
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddUser(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            User user = JsonConvert.DeserializeObject<User>(paramDetail);
            return Ok(userModel.AddUser(user, UserId));
        }

        [Route("update")]
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateUserDetail(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            User user = JsonConvert.DeserializeObject<User>(paramDetail);
            bool isUpdated = userModel.UpdateUserDetail(user, UserId);
            return Ok(isUpdated);
        }

        [Route("checkexistance")]
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult CheckExistance(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            User user = JsonConvert.DeserializeObject<User>(paramDetail);
            return Ok(userModel.CheckExistance(user));
        }

        [Route("forgotpassword")]
        [HttpGet]
        //[ResponseType(typeof(string))]
        // POST: api/account/forgotpassword?emailId=value
        public IHttpActionResult ForgotPassword(string emailId)
        {
            emailId = Crypto.Instance.Decrypt(emailId);
            UserModel userModel = new UserModel();
            return Ok(userModel.ForgotPassword(emailId));
            //return userModel.ForgotPassword(emailId);
        }

        [Route("validatecredentials")]
        [HttpGet]
        [ResponseType(typeof(bool))]
        // POST: api/account/forgotpassword?emailId=value
        public bool ValidateCredentials(string emailId, string password)
        {
            emailId = Cryptography.Crypto.Instance.Decrypt(emailId);
            //password = Cryptography.Crypto.Instance.Decrypt(password);
            UserModel userModel = new UserModel();
            bool result = userModel.ValidateCredentials(emailId, password);
            return result;
        }

        [Route("changepassword")]
        [HttpPost]
        // POST: api/account/changepassword
        public IHttpActionResult ChangePassword(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ChangePasswordModel changePasswordModel = JsonConvert.DeserializeObject<ChangePasswordModel>(paramDetail);
            return Ok(userModel.ChangePassword(changePasswordModel));
        }

        [Route("isuserassignedtoworkflow")]
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(bool))]
        public IHttpActionResult IsUserAssignedToWorkflow(int userId)
        {
            return Ok(userModel.IsUserAssignedToWorkflow(userId));
        }

        [Route("sendemailforforgotpassword")]
        [HttpPost]
        //[ResponseType(typeof(string))]
        // POST: api/account/forgotpassword?emailId=value
        public IHttpActionResult SendEmailForForForgotPassword(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            User user = JsonConvert.DeserializeObject<User>(paramDetail);
            UserModel userModel = new UserModel();
            return Ok(userModel.SendEmail(user));
            //return userModel.ForgotPassword(emailId);
        }

        [Route("adauthentication")]
        [HttpGet]
        [ResponseType(typeof(string))]
        // POST: api/account/adauthentication?emailId=value&password=value
        public string ADAuthentication(string emailId, string password)
        {
            emailId = Cryptography.Crypto.Instance.Decrypt(emailId);
            password = Cryptography.Crypto.Instance.Decrypt(password);
            UserModel userModel = new UserModel();
            return userModel.ADAuthentication(emailId, password);
        }

        [Authorize]
        [Route("logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);
            return Ok();
        }
    }
}