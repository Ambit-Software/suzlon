using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace SuzlonBPP.Models
{
    public class VerticalModel
    {
        #region "Public Methods"

        /// <summary>
        /// This method used to get all vertical details.
        /// </summary>
        /// <returns></returns>
        public List<VerticalMaster> GetVerticals()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.VerticalMasters.OrderBy(n => n.Name).ToList();
            }
        }

        /// <summary>
        /// This method used to add vertical details
        /// </summary>
        /// <param name="verticalMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VerticalMaster AddVertical(VerticalMaster verticalMaster, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                verticalMaster.CreatedBy = userId;
                verticalMaster.CreatedOn = DateTime.Now;
                verticalMaster.ModifiedBy = userId;
                verticalMaster.ModifiedOn = DateTime.Now;
                suzlonBPPEntities.VerticalMasters.Add(verticalMaster);
                suzlonBPPEntities.SaveChanges();
                return verticalMaster;
            }
        }

        /// <summary>
        /// This method used to update vertical details.
        /// </summary>
        /// <param name="verticalMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateVertical(VerticalMaster verticalMaster, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                VerticalMaster Vertical = suzlonBPPEntities.VerticalMasters.FirstOrDefault(l => l.VerticalId == verticalMaster.VerticalId);
                if (Vertical != null)
                {
                    Vertical.Name = verticalMaster.Name;
                    Vertical.Description = verticalMaster.Description;
                    Vertical.Status = verticalMaster.Status;
                    Vertical.ModifiedBy = userId;
                    Vertical.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(Vertical).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// This method used to check for duplicate vertical
        /// </summary>
        /// <param name="name"></param>
        /// <param name="verticalId"></param>
        /// <returns></returns>
        public bool VerticalExists(string name, int verticalId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.VerticalMasters.FirstOrDefault(l => l.Name.ToLower() == name.ToLower() && l.VerticalId != verticalId) != null;
            }
        }

        public List<VerticalMaster> GetVerticalsByUser(string UserId)
        {
            int iUserId = Convert.ToInt32(UserId);
            List<VerticalMaster> lstVerticalMaster = new List<VerticalMaster>(); 
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                User objUser = suzlonBPPEntities.Users.FirstOrDefault(c => c.UserId == iUserId);
                string[] strArr = objUser.Vertical.Split(',');
                if (strArr.Length > 0)
                {
                     lstVerticalMaster = (from c in suzlonBPPEntities.VerticalMasters
                                   where strArr.Contains(c.VerticalId.ToString())
                                          select c).ToList<VerticalMaster>();
                }
            }

            return lstVerticalMaster;
        }

        #endregion "Public Methods"
    }
}