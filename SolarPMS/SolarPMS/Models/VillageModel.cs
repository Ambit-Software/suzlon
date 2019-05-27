using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class VillageModel
    {
        #region "Public Methods"
        /// <summary>
        ///// This method used to get all village details.
        /// </summary>
        /// <returns></returns>
        public List<VillageMaster> GetVillages()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.VillageMasters.ToList();
            }
        }
        /// <summary>
        /// This method used to add village details
        /// </summary>
        /// <param name="villageMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VillageMaster AddVillage(VillageMaster villageMaster, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                villageMaster.Status = villageMaster.Status;
                villageMaster.CreatedBy = userId;
                villageMaster.CreatedOn = DateTime.Now;
                villageMaster.ModifiedBy = userId;
                villageMaster.ModifiedOn = DateTime.Now;
                solarPMSEntities.VillageMasters.Add(villageMaster);
                solarPMSEntities.SaveChanges();
                return villageMaster;
            }
        }

        /// <summary>
        /// This method used to update village details.
        /// </summary>
        /// <param name="villageMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateVillage(VillageMaster villageMaster, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                VillageMaster Village = solarPMSEntities.VillageMasters.FirstOrDefault(l => l.VillageId == villageMaster.VillageId);
                if (Village != null)
                {
                    Village.VillageName = villageMaster.VillageName;
                    Village.Description = villageMaster.Description;
                    Village.Status = villageMaster.Status;
                    Village.ModifiedBy = userId;
                    Village.ModifiedOn = DateTime.Now;
                    solarPMSEntities.Entry(Village).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        ///// This method used to check for duplicate village.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool VillageExists(string name, int id)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.VillageMasters.FirstOrDefault(l => l.VillageName.ToLower() == name.ToLower() && l.VillageId != id) != null;
            }
        }
        #endregion "Public Methods"
    }
}