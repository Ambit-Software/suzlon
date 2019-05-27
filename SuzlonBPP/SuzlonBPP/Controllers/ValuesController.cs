using Cryptography;
using Newtonsoft.Json;
using SuzlonBPP.Filters;
using SuzlonBPP.Models;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace SuzlonBPP.Controllers
{
    [Authorize]
    [RoutePrefix("api/values")]
    public class ValuesController : BaseApiController
    {
        [Route("populatedropdowns")]
        [HttpGet]
        public IHttpActionResult PopulateDropdowns(string name)
        {
            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.PopulateDropdowns(name));
        }

        [Route("populatedropdownsbyuser")]
        [HttpGet]
        public IHttpActionResult PopulateDropdownsbyuser(string name, int userid)
        {
            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.PopulateDropdownsByUser(name, userid));
        }

        [Route("errorlog")]
        [HttpPost]
        public IHttpActionResult ErrorLog(Exception ex)
        {
            CommonFunctions.WriteErrorLog(ex, UserId);
            return Ok();
        }

        [Route("populatebankworkflowdropdown")]
        [HttpGet]
        public IHttpActionResult PopulateBankWorkflowDropdown(string name,string subVerticalId)
        {
            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.PopulateBankWorkflowDropdowns(name, subVerticalId));
        }

        [Route("getFileUploads")]
        [HttpGet]
        public IHttpActionResult getFileUploads(int entityId,string entityName)
        {
            CommonFunctions commonFunctions = new CommonFunctions();
            return Ok(commonFunctions.getFileUploads(entityId, entityName));
        }


        // POST: api/workflow/AddFileUpload
        [Route("AddFileUpload")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        //TBM
        public IHttpActionResult AddFileUpload(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            FileUpload fileUpload = JsonConvert.DeserializeObject<Models.FileUpload>(paramDetail);
            CommonFunctions commonFunctions = new CommonFunctions();
            string Id = commonFunctions.AddFileUpload(fileUpload);
            return Ok(Id);
        }

        // POST: api/workflow/DeleteUploadedFile
        [Route("DeleteUploadedFile")]
        [HttpGet]
        [ResponseType(typeof(bool))]
        public IHttpActionResult DeleteUploadedFile(int Id,int userid)
        {          
            CommonFunctions commonFunctions = new CommonFunctions();
            bool isDeleted = commonFunctions.DeleteUploadedFile(Id, userid);
            return Ok(isDeleted);
        }


    }
}
