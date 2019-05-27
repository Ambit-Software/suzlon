using SuzlonBPP.Filters;
using SuzlonBPP.Models;
using System.Web.Http;

namespace SuzlonBPP.Controllers
{
    [RoutePrefix("api/vendor")]
    [Authorize]
    public class VendorBankController : BaseApiController
    {
        private VendorModel vendorBankModel = new VendorModel();

        [HttpGet]
        // GET: api/vendor/getall
        [Route("getallbankdetail")]
        public IHttpActionResult GetVendorBankDetails()
        {
            return Ok(vendorBankModel.GetVendorBankDetails());
        }

        [HttpGet]
        // GET: api/vendor/getall
        [Route("getallvendordetail")]
        public IHttpActionResult GetVendorDetails()
        {
            return Ok(vendorBankModel.GetVendorDetails());
        }
    }
}
