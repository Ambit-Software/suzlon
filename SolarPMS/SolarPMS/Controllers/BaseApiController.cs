using SolarPMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace SolarPMS.Controllers
{
    [ExceptionHandlingFilter]
    public class BaseApiController : ApiController
    {        
        private int _userId = 0;
        protected int UserId
        {
            get
            {
                if (_userId == 0)
                {
                    _userId = Convert.ToInt32(((string[])(Request.Headers.GetValues("UserId")))[0]);
                }

                return _userId = GetUserId();                
            }
        }

        private int GetUserId()
        {
            int userId = 0;
            IIdentity userDetail = HttpContext.Current.User.Identity;
            if (userDetail != null)
            {
                (((System.Security.Claims.ClaimsIdentity)userDetail).Claims).ToList().ForEach(claim =>
                {
                    if (claim.Type == "userId")
                    {
                        userId = Convert.ToInt32(claim.Value);
                    }
                });
            }
            return userId;
        }

        //[ExceptionHandlingFilter]
        //public class BaseApiController : ApiController
        //{
        //}

    }
}
