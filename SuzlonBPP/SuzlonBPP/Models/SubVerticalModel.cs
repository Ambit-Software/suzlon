using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SuzlonBPP.Models
{
    public class SubVerticalModel
    {
        public int SubVerticalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int VerticalId { get; set; }
        public string VerticalName { get; set; }

        #region "Public Methods"

        /// <summary>
        /// This method used to get all sub vertical details.
        /// </summary>
        /// <returns></returns>
        public List<SubVerticalModel> GetSubVerticals()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var subVerticalDetails = (from subVertical in suzlonBPPEntities.SubVerticalMasters
                                          join vertical in suzlonBPPEntities.VerticalMasters
                                          on subVertical.VerticalId equals vertical.VerticalId
                                          select new SubVerticalModel()
                                          {
                                              SubVerticalId = subVertical.SubVerticalId,
                                              Name = subVertical.Name,
                                              Description = subVertical.Description,
                                              Status = subVertical.Status,
                                              VerticalId = subVertical.VerticalId,
                                              VerticalName = vertical.Name
                                          }).OrderBy(v=> v.Name).ToList();
                return subVerticalDetails;
            }
        }

        public List<GetSubVerticalByVertical_Result> GetSubVerticalBasedOnVertical(string verticalIds)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.GetSubVerticalByVertical(verticalIds).ToList();
            }
        }

        /// <summary>
        /// This method used to add sub vertical details
        /// </summary>
        /// <param name="subVerticalMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SubVerticalMaster AddSubVertical(SubVerticalMaster subVerticalMaster, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                subVerticalMaster.CreatedBy = userId;
                subVerticalMaster.CreatedOn = DateTime.Now;
                subVerticalMaster.ModifiedBy = userId;
                subVerticalMaster.ModifiedOn = DateTime.Now;
                suzlonBPPEntities.SubVerticalMasters.Add(subVerticalMaster);
                suzlonBPPEntities.SaveChanges();
                return subVerticalMaster;
            }
        }

        /// <summary>
        /// This method used to update sub vertical details.
        /// </summary>
        /// <param name="subVerticalMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateSubVertical(SubVerticalMaster subVerticalMaster, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                SubVerticalMaster subVertical = suzlonBPPEntities.SubVerticalMasters.FirstOrDefault(l => l.SubVerticalId == subVerticalMaster.SubVerticalId);
                if (subVertical != null)
                {
                    subVertical.VerticalId = subVerticalMaster.VerticalId;
                    subVertical.Name = subVerticalMaster.Name;
                    subVertical.Description = subVerticalMaster.Description;
                    subVertical.Status = subVerticalMaster.Status;
                    subVertical.ModifiedBy = userId;
                    subVertical.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(subVertical).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// This method used to check for duplicate sub vertical.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subVerticalId"></param>
        /// <returns></returns>
        public bool SubVerticalExists(string name, int subverticalId, int verticalId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.SubVerticalMasters.FirstOrDefault(l => l.Name.ToLower() == name.ToLower() && l.SubVerticalId != subverticalId && l.VerticalId == verticalId) != null;
            }
        }

        public List<SubVerticalModel> GetSubVerticalsByUser(UserModel UserInfo)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                var subVerticalDetails = (from subVertical in suzlonBPPEntities.SubVerticalMasters
                                          join vertical in suzlonBPPEntities.VerticalMasters
                                          on subVertical.VerticalId equals vertical.VerticalId
                                          where UserInfo.UserDetail.SubVertical.Contains(subVertical.SubVerticalId.ToString())
                                          select new SubVerticalModel()
                                          {
                                              SubVerticalId = subVertical.SubVerticalId,
                                              Name = subVertical.Name,                                             
                                              VerticalId = subVertical.VerticalId,
                                              VerticalName = vertical.Name
                                          }).OrderBy(v => v.Name).ToList();
                return subVerticalDetails;
            }
        }
        #endregion "Public Methods"
    }
}