using SuzlonBPP.Filters;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace SuzlonBPP.Controllers
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
                    _userId = GetUserId();
                }

                return _userId;
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
    }
}
