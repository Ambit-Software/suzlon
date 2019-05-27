using System.Collections.Generic;
using System.Linq;

namespace SuzlonBPP.Models
{
    public class CompanyModel
    {
        #region "Public Methods"
        /// <summary>
        /// This method is used to get all comapnies details.
        /// </summary>
        /// <returns></returns>
        public List<CompanyMaster> GetCompanyDetails()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.CompanyMasters.OrderBy(n => n.Name).ToList();
            }
        }

        public DropdownValues GetCompanyUserWise(string CompanyCodes)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                DropdownValues ddValues = new DropdownValues();
                ddValues.Company = new List<ListItem>();
                 (from company in suzlonBPPEntities.VendorMasters
                 where CompanyCodes.Contains(company.CompanyCode)
                 select new ListItem()
                 {
                     Id = company.CompanyCode,
                     Name = company.CompanyCode+" - "+company.CompanyName
                 }).Distinct().OrderBy(v => v.Name).
                ToList().ForEach(item =>
                {
                    ddValues.Company.Add(item);
                });
           
                return ddValues;
            }
        }

        public DropdownValues GetVendorCompanyWise(string CompanyCode)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;             

                DropdownValues ddValues = new DropdownValues();
                ddValues.VendorCode = new List<ListItem>();
                (from Vendor in suzlonBPPEntities.VendorMasters
                 where Vendor.CompanyCode == CompanyCode
                 select new ListItem()
                 {
                     Id = Vendor.VendorCode,
                     Name = Vendor.VendorName
                 }).Distinct().OrderBy(v => v.Name)
                 .ToList().ForEach(item =>
                  {
                       ddValues.VendorCode.Add(item);
                  });

                return ddValues;
            }
        }

        public List<VendorSearch> SearchVendorCompanyWise(string CompanyCode, string searchText)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;

                if (string.IsNullOrEmpty(searchText))
                {
                    var VendorDetail = (from v in suzlonBPPEntities.VendorMasters
                                        where v.CompanyCode == CompanyCode
                                        select new VendorSearch
                                        {
                                            VendorCode = v.VendorCode,
                                            VendorName = v.VendorName,
                                            City = v.City,
                                            Region = v.Region
                                        }).ToList();
                    return VendorDetail;
                }
                else
                {
                    var VendorDetail = (from v in suzlonBPPEntities.VendorMasters
                                        where v.CompanyCode == CompanyCode && (v.VendorCode.Contains(searchText) || v.VendorName.Contains(searchText))
                                        select new VendorSearch
                                        {
                                            VendorCode = v.VendorCode,
                                            VendorName = v.VendorName,
                                            City = v.City,
                                            Region = v.Region
                                        }).ToList();
                    return VendorDetail;
                }
               
            }
        }
        #endregion "Public Methods"
    }
}