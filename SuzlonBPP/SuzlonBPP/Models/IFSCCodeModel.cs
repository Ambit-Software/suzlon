using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SolarPMS.Models
{
    public class IFSCCodeModel
    {
        #region "Public Methods"

        /// <summary>
        /// This method used to get all IFSC details.
        /// </summary>
        /// <returns></returns>
        public List<IFSCCodeMaster> GetIFSCCodes()
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                suzlonBPPEntities.Configuration.ProxyCreationEnabled = false;
                return suzlonBPPEntities.IFSCCodeMasters.OrderBy(n => n.BankName).ToList();
            }
        }

        /// <summary>
        /// This method used to add IFSC details
        /// </summary>
        /// <param name="iFSCCodeMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IFSCCodeMaster AddIFSCCode(IFSCCodeMaster iFSCCodeMaster, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                iFSCCodeMaster.CreatedBy = userId;
                iFSCCodeMaster.CreatedOn = DateTime.Now;
                iFSCCodeMaster.ModifiedBy = userId;
                iFSCCodeMaster.ModifiedOn = DateTime.Now;
                suzlonBPPEntities.IFSCCodeMasters.Add(iFSCCodeMaster);
                suzlonBPPEntities.SaveChanges();
                return iFSCCodeMaster;
            }
        }

        /// <summary>
        /// This method used to update IFSC details.
        /// </summary>
        /// <param name="iFSCCodeMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateIFSCCode(IFSCCodeMaster iFSCCodeMaster, int userId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                IFSCCodeMaster iFSCDetail = suzlonBPPEntities.IFSCCodeMasters.FirstOrDefault(l => l.IFSCCodeId == iFSCCodeMaster.IFSCCodeId);
                if (iFSCDetail != null)
                {
                    iFSCDetail.IFSCCode = iFSCCodeMaster.IFSCCode;
                    iFSCDetail.BankName = iFSCCodeMaster.BankName;
                    iFSCDetail.BranchName = iFSCCodeMaster.BranchName;
                    iFSCDetail.Status = iFSCCodeMaster.Status;
                    iFSCDetail.ModifiedBy = userId;
                    iFSCDetail.ModifiedOn = DateTime.Now;
                    suzlonBPPEntities.Entry(iFSCDetail).State = EntityState.Modified;
                    suzlonBPPEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// This method used to check for duplicate IFSC details.
        /// </summary>
        /// <param name="iFSCCodeMaster"></param>
        /// <returns></returns>
        public bool IFSCCodeExists(string iFSCCode, int iFSCCodeId)
        {
            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                return suzlonBPPEntities.IFSCCodeMasters.FirstOrDefault(l => l.IFSCCode.ToLower() == iFSCCode.ToLower()
                                                                            && l.IFSCCodeId != iFSCCodeId) != null;
            }
        }
        #endregion "Public Methods"
    }
}