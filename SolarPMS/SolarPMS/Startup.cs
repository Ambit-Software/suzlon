using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using System.Security.Principal;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using SolarPMS.Models;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using Microsoft.Owin.Security.Infrastructure;
using System.Collections.Concurrent;
using System.Configuration;
using Cryptography;

[assembly: OwinStartup(typeof(SolarPMS.Startup))]

namespace SolarPMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            //other configurations

            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/account/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExipredTime"])),
                Provider = new AuthorizationServerProvider()
            };


            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }

    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            try
            {
                UserModel userModel = new UserModel();
                //retrieve your user from database.
                var user = userModel.Authenticate(Crypto.Instance.Decrypt(context.UserName), context.Password);
               // var user = userModel.Authenticate(context.UserName, context.Password);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserId.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.UserName));
                identity.AddClaim(new Claim("userId", user.UserId.ToString()));
                //roles example
                var rolesTechnicalNamesUser = new List<string>();
                var principal = new GenericPrincipal(identity, rolesTechnicalNamesUser.ToArray());
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "userid", user.UserId.ToString()
                    },
                    {
                        "name", user.Name.ToString()
                    },
                    {
                        "profileid", user.ProfileMaster.ProfileId.ToString()
                    },
                    {
                        "profilename", user.ProfileMaster.ProfileName.ToString()
                    },
                    {
                        "DocumentAccess", user.ProfileMaster.DEDocumentUploadAccess.ToString()
                    }
                });
                Thread.CurrentPrincipal = principal;
                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect");
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}