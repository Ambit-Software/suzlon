using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class LocationModel
    {
        #region "Public Methods"
        /// <summary>
        ///// This method used to get all location details.
        /// </summary>
        /// <returns></returns>
        public List<LocationMaster> GetLocations()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.LocationMasters.ToList();
            }
        }
        /// <summary>
        /// This method used to add location details
        /// </summary>
        /// <param name="locationMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public LocationMaster AddLocation(LocationMaster locationMaster, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                locationMaster.Status = true;
                locationMaster.CreatedBy = userId;
                locationMaster.CreatedOn = DateTime.Now;
                locationMaster.ModifiedBy = userId;
                locationMaster.ModifiedOn = DateTime.Now;
                solarPMSEntities.LocationMasters.Add(locationMaster);
                solarPMSEntities.SaveChanges();
                return locationMaster;
            }
        }

        /// <summary>
        /// This method used to update location details.
        /// </summary>
        /// <param name="locationMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateLocation(LocationMaster locationMaster, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                LocationMaster location = solarPMSEntities.LocationMasters.FirstOrDefault(l => l.LocationId == locationMaster.LocationId);
                if (location != null)
                {
                    location.LocationName = locationMaster.LocationName;
                    location.Description = locationMaster.Description;
                    location.Status = locationMaster.Status;
                    location.ModifiedBy = userId;
                    location.ModifiedOn = DateTime.Now;
                    solarPMSEntities.Entry(location).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        ///// This method used to check for duplicate location.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool LocationExists(string name, int locationId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.LocationMasters.FirstOrDefault(l => l.LocationName.ToLower() == name.ToLower() && l.LocationId != locationId) != null;
            }
        }
        #endregion "Public Methods"
    }
}