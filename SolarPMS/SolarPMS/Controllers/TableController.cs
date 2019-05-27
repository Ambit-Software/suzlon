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
    [RoutePrefix("api/table")]
    public class TableController : BaseApiController
    {
        TableModel tableModel = new TableModel();

        [Route("getall")]
        public IHttpActionResult GetAllTableInfo()
        {
           
                return Ok(tableModel.GetAllTableInfo());
           
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddTable(PostParam param)
        {
           
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                TableMaster tableMaster = JsonConvert.DeserializeObject<TableMaster>(paramDetail);
                return Ok(tableModel.AddTable(tableMaster, UserId));
            
           
        }

        [Route("update")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateTableDetail(PostParam param)
        {
          
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                TableMaster tableMaster = JsonConvert.DeserializeObject<TableMaster>(paramDetail);
                bool isUpdated = tableModel.UpdateTableDetail(tableMaster, UserId);
                return Ok(isUpdated);
           
        }

        [Route("exists")]
        [HttpPost]
        // POST: api/table/exists/1
        public IHttpActionResult TableExists(PostParam param)
        {
           
                var paramDetail = Crypto.Instance.Decrypt(param.Data);
                TableMaster tableMaster = JsonConvert.DeserializeObject<TableMaster>(paramDetail);
                bool isExists = tableModel.TableExists(tableMaster);
                return Ok(isExists);
            
        }

    }
}