using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SolarPMS.Controllers
{
    public class AuthController : AuthorizeAttribute
    {
        //public override void OnAuthorization(HttpActionContext actionContext)
        //{
        //    var claimsIdentity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
        //    if (claimsIdentity == null)
        //    {
        //        this.HandleUnauthorizedRequest(actionContext);
        //    }

        //    // Check if the password has been changed. If it was, this token should be not accepted any more.
        //    // We generate a GUID stamp upon registration and every password change, and put it in every token issued.
        //    var passwordTokenClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "PasswordTokenClaim");

        //    if (passwordTokenClaim == null)
        //    {
        //        // There was no stamp in the token.
        //        this.HandleUnauthorizedRequest(actionContext);
        //    }
        //    else
        //    {
        //        MyContext ctx = (MyContext)System.Web.Mvc.DependencyResolver.Current.GetService(typeof());

        //        var userName = claimsIdentity.Claims.First(c => c.Type == ClaimTypes.Name).Value;

        //        if (ctx.Users.First(u => u.UserName == userName).LatestPasswordStamp.ToString() != passwordTokenClaim.Value)
        //        {
        //            // The stamp has been changed in the DB.
        //            this.HandleUnauthorizedRequest(actionContext);
        //        }
        //    }

        //    base.OnAuthorization(actionContext);
        //}
    }
}
