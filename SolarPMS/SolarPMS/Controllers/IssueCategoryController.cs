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
using System.Web.Http.Description;

namespace SolarPMS.Controllers
{
    [ExceptionHandlingFilter]
    [Authorize]
    [RoutePrefix("api/issuecategory")]
    public class IssueCategoryController : BaseApiController
    {
        private IssueCategoryModel issueCategoryModel = new IssueCategoryModel();

        [HttpGet]
        // GET: api/issuecategory/getall
        [Route("getall")]
        public IHttpActionResult GetIssueCategorys()
        {
           
                return Ok(issueCategoryModel.GetIssueCategories());
           
        }

        // POST: api/issuecategory/add
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddIssueCategory(PostParam param)
        {
               var paramDetail = Crypto.Instance.Decrypt(param.Data);
                IssueCategory issueCategory = JsonConvert.DeserializeObject<IssueCategory>(paramDetail);
                issueCategory = issueCategoryModel.AddIssueCategory(issueCategory, UserId);
                return Ok(issueCategory);
           
        }

        // POST: api/issuecategory/update
        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateIssueCategory(PostParam param)
        {
           
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                IssueCategory issueCategory = JsonConvert.DeserializeObject<IssueCategory>(paramDetail);
                bool isUpdated = issueCategoryModel.UpdateIssueCategory(issueCategory, UserId);
                return Ok(isUpdated);
            
        }

        [Route("exists")]
        [HttpGet]
        // POST: api/issuecategory/exists?name=value&id=value
        public IHttpActionResult IssueCategoryExists(string name, int id)
        {
           
                bool isExists = issueCategoryModel.IssueCategoryExists(name, id);
                return Ok(isExists);
           
        }
    }
}
