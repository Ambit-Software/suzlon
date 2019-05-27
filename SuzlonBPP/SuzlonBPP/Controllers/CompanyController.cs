using SuzlonBPP.Filters;
using SuzlonBPP.Models;
using System.Web.Http;

namespace SuzlonBPP.Controllers
{
    [RoutePrefix("api/company")]
    [Authorize]
    public class CompanyController : BaseApiController
    {
        private CompanyModel companyModel = new CompanyModel();

        [HttpGet]
        // GET: api/company/getall
        [Route("getall")]
        public IHttpActionResult GetCompanyDetails()
        {
            return Ok(companyModel.GetCompanyDetails());
        }

        [HttpGet]
        // GET: api/company/getall
        [Route("GetCompanyUserWise")]
        public IHttpActionResult GetCompanyUserWise(string CompanyCodes)
        {
            return Ok(companyModel.GetCompanyUserWise(CompanyCodes));
        }

        [HttpGet]
        // GET: api/company/getall
        [Route("GetVendorCompanyWise")]
        public IHttpActionResult GetVendorCompanyWise(string CompanyCode)
        {
            return Ok(companyModel.GetVendorCompanyWise(CompanyCode));
        }

        [HttpGet]
        // GET: api/company/getall
        [Route("SearchVendorCompanyWise")]
        public IHttpActionResult SearchVendorCompanyWise(string CompanyCode,string searchText)
        {
            return Ok(companyModel.SearchVendorCompanyWise(CompanyCode, searchText));
        }

    }
}
