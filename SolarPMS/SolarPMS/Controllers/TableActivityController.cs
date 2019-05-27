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
    [Authorize]
    [ExceptionHandlingFilter]
    [RoutePrefix("api/tableactivity")]
    public class TableActivityController : BaseApiController
    {
        TableActivityModel tableActivityModel = new TableActivityModel();

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddSurvey(PostParam param)
        {

            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableActivity tableActivity = JsonConvert.DeserializeObject<TableActivity>(paramDetail);
            return Ok(tableActivityModel.AddTableActivity(tableActivity, UserId));

        }

        [Route("exists")]
        [HttpPost]
        public IHttpActionResult TableExists(PostParam param)
        {

            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableActivity tableActivity = JsonConvert.DeserializeObject<TableActivity>(paramDetail);
            bool isExists = tableActivityModel.TableActivityExists(tableActivity);
            return Ok(isExists);

        }

        [Route("getblock")]
        [HttpPost]
        public IHttpActionResult GetBlock(PostParam param)
        {

            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableActivity tableActivity = JsonConvert.DeserializeObject<TableActivity>(paramDetail);
            return Ok(tableActivityModel.GetBlock(tableActivity));

        }

        [Route("gettableactivitybyrange")]
        [HttpPost]
        public IHttpActionResult GetTableActivityByRange(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableActivityRange tableActivityRange = JsonConvert.DeserializeObject<TableActivityRange>(paramDetail);
            return Ok(tableActivityModel.GetTableActivityByRange(tableActivityRange));

        }

        [Route("gettableactivitybymanualentry")]
        [HttpPost]
        public IHttpActionResult GetTableActivityByManualEntry(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            ManualEntryRange manualEntryRange = JsonConvert.DeserializeObject<ManualEntryRange>(paramDetail);
            return Ok(tableActivityModel.GetTableActivityByManualEntry(manualEntryRange));

        }

        [Route("gettableactivitybyblock")]
        [HttpPost]
        public IHttpActionResult GetTableActivityByBlock(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableBlock tableBlock = JsonConvert.DeserializeObject<TableBlock>(paramDetail);
            return Ok(tableActivityModel.GetTableActivityByBlock(tableBlock));

        }

        [Route("getblocksforactivity")]
        [HttpPost]
        public IHttpActionResult GetBlocks(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableBlock tableBlock = JsonConvert.DeserializeObject<TableBlock>(paramDetail);
            return Ok(tableActivityModel.GetBlocksForActivity(tableBlock));

        }

        [Route("getblocksforsubactivity")]
        [HttpPost]
        public IHttpActionResult GetBlocksForSubActivity(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableBlock tableBlock = JsonConvert.DeserializeObject<TableBlock>(paramDetail);
            return Ok(tableActivityModel.GetBlocksForSubActivity(tableBlock));
        }

        [Route("getallblock")]
        [HttpPost]
        public IHttpActionResult GetAllBlock(PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableActivity tableBlock = JsonConvert.DeserializeObject<TableActivity>(paramDetail);
            return Ok(TableActivityModel.GetAllBlock(tableBlock));
        }

        [Route("getcompletedquantity")]
        [HttpGet]
        // POST: api/survey/exists/1
        public IHttpActionResult GetCompletedQuantity(string SubActivity, string Activity, string Network, int AreaId, string Project, string Site, int BlockNo, int TimesheetId)
        {
            return Ok(TableActivityModel.GetCompletedQuantity(SubActivity, Activity, Network, AreaId, Project, Site, BlockNo, TimesheetId));
        }

        [Route("checkexistingnumbers")]
        [HttpGet]
        // POST: api/survey/exists/1
        public IHttpActionResult CheckExistingNumbers(string SubActivity, string Activity, string Network, int AreaId, string Project, string Site, int TimesheetId, string Numbers, string Flag)
        {
            int[] numberArray = !string.IsNullOrEmpty(Numbers) ? Array.ConvertAll(Numbers.Trim().Split(',').ToArray(), int.Parse) : new int[1];
            SubActivity = string.IsNullOrEmpty(SubActivity) ? string.Empty : SubActivity;
            return Ok(TableActivityModel.CheckExistingNumbers(SubActivity, Activity, Network, AreaId, Project, Site, TimesheetId, numberArray, Flag));
        }

        [Route("findmissingtableno")]
        [HttpPost]
        // POST: api/tableactivity/FindMissingTableNo/1
        public IHttpActionResult FindMissingTableNo(int FromRange, int ToRange, PostParam param)
        {
            var paramDetail = Crypto.Instance.Decrypt(param.Data);
            TableActivityRange tableActivityRange = JsonConvert.DeserializeObject<TableActivityRange>(paramDetail);
            return Ok(TableActivityModel.FindMissingTableNo(FromRange, ToRange, tableActivityRange));
        }
    }
}
