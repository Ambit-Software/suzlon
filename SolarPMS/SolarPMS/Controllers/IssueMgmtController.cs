using Cryptography;
using Newtonsoft.Json;
using SolarPMS.Filters;
using SolarPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SolarPMS.Controllers
{
    [ExceptionHandlingFilter]
    [Authorize]
    [RoutePrefix("api/issuemgmt")]
    public class IssueMgmtController : BaseApiController
    {
        IssueMgmtModel issueMgmtModel = new IssueMgmtModel();

        [Route("getissuedetail")]
        public IHttpActionResult GetIssueDetail(int issueId)
        {
            return Ok(issueMgmtModel.GetIssueDetail(issueId));
        }

        [Route("getassignedtome")]
        public IHttpActionResult GetIssueAssignedToMe(string searchText)
        {
            return Ok(issueMgmtModel.GetIssueAssignedToMe(UserId, searchText));
        }

        [Route("getraisedbyme")]
        public IHttpActionResult GetIssueRaisedByMe(string searchText)
        {
            return Ok(issueMgmtModel.GetIssueRaisedByMe(UserId, searchText));
        }
        [Route("getallissues")]
        public IHttpActionResult GetAllIssues(string searchText)
        {
            return Ok(issueMgmtModel.GetAllIssues(searchText));
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddIssue(PostParam param)
        {
            try
            {
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                IssueManagement issueManagement = JsonConvert.DeserializeObject<IssueManagement>(paramDetail);
                return Ok(issueMgmtModel.AddIssue(issueManagement, UserId));
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateIssue(PostParam param)
        {
            try
            {
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                IssueManagement issueManagement = JsonConvert.DeserializeObject<IssueManagement>(paramDetail);
                return Ok(issueMgmtModel.UpdateIssue(issueManagement, UserId));
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

        }

        [HttpPost]
        [Route("getissuebyactivityid")]
        public IHttpActionResult GetIssueByActivityId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            IssueManagement issueManagement = JsonConvert.DeserializeObject<IssueManagement>(paramDetail);
            return Ok(issueMgmtModel.GetIssueByActivityId(issueManagement.ActivityId));
        }

        [HttpPost]
        [Route("getissuebysubactivityid")]
        public IHttpActionResult GetIssueBySubActivityId(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            IssueManagement issueManagement = JsonConvert.DeserializeObject<IssueManagement>(paramDetail);
            return Ok(issueMgmtModel.GetIssueBySubActivityId(issueManagement.SubActivityId));
        }

        [HttpGet]
        [Route("getissueassignhistory")]
        public IHttpActionResult GetIssueAssignHistory(int issueId)
        {
            return Ok(issueMgmtModel.GetIssueAssignHistory(issueId));
        }
    }
}
