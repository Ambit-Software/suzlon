using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Security.Principal;
namespace SolarPMS.Filters
{
    public class ExceptionHandlingFilter: ExceptionFilterAttribute
    {
        //public override void OnException(HttpActionExecutedContext context)
        //{
        //    //throw new HttpResponseException(
        //    //context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, context.Exception.ToString()));
        //    //throw new HttpResponseException(context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, context.ActionContext.ModelState));
        //    context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //    {
        //        Content = new StringContent(context.Exception.ToString()),
        //        ReasonPhrase = "Exception"
        //    };
        //}

        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(context.Exception.ToString()),
                ReasonPhrase = "Exception"
            };
            int userId = GetUserId();
            CommonFunctions.WriteErrorLog(context.Exception, userId);
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