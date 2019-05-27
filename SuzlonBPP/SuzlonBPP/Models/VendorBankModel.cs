using System.Collections.Generic;
using System.Linq;

namespace SuzlonBPP.Models
{
    public class VendorModel
    {
        #region "Public Methods"

        /// <summary>
        /// This method is used to get all vendor bank details.
        /// </summary>
        /// <returns></returns>
        public List<VendorBankMaster> GetVendorBankDetails()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.VendorBankMasters.OrderBy(n => n.VendorName).ToList();
            }
        }

        /// <summary>
        /// This method is used to get all vendor details.
        /// </summary>
        /// <returns></returns>
        public List<VendorMaster> GetVendorDetails()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.VendorMasters.OrderBy(n => n.VendorName).ToList();
            }
        }

        #endregion "Public Methods"
    }
}