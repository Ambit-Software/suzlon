using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class NatureRequestModel
    {
        #region "Public Methods"

        /// <summary>
        /// This method used to get all nature request details.
        /// </summary>
        /// <returns></returns>
        public List<NatureRequestMaster> GetRequests()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.NatureRequestMasters.OrderBy(n=> n.Name).ToList();
            }
        }

        /// <summary>
        /// This method used to add nature request details
        /// </summary>
        /// <param name="requestMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public NatureRequestMaster AddRequest(NatureRequestMaster requestMaster, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                requestMaster.CreatedBy = userId;
                requestMaster.CreatedOn = DateTime.Now;
                requestMaster.ModifiedBy = userId;
                requestMaster.ModifiedOn = DateTime.Now;
                suzlonBPPEntities.NatureRequestMasters.Add(requestMaster);
                suzlonBPPEntities.SaveChanges();
                return requestMaster;
            }
        }

        /// <summary>
        /// This method used to update nature request details.
        /// </summary>
        /// <param name="requestMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateRequest(NatureRequestMaster requestMaster, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                NatureRequestMaster request = suzlonBPPEntities.NatureRequestMasters.FirstOrDefault(l => l.RequestId == requestMaster.RequestId);
                if (request != null)
                {
                    request.RequestId = requestMaster.RequestId;
                    request.Name = requestMaster.Name;
                    request.Description = requestMaster.Description;
                    request.Type = requestMaster.Type;
                    request.Status = requestMaster.Status;
                    request.ModifiedBy = userId;
                    request.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(request).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// This method used to check for duplicate nature request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public bool RequestExists(string name, int requestId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.NatureRequestMasters.FirstOrDefault(l => l.Name.ToLower() == name.ToLower() && l.RequestId != requestId) != null;
            }
        }
        #endregion "Public Methods"
    }
}