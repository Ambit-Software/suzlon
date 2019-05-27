using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class ManPowerModel
    {
        public static dynamic GetManPowerData()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.ManPowerMasters.Join(solarPMSEntities.SAPMasters, m => m.Project, s => s.SAPProjectId, (m, s) => new { m, s })
                    .Where(r => r.m.IsDeleted == false && (r.s.IsDeleted == null || r.s.IsDeleted == false))
                    .Select(r => new { r.m.Project, r.m.Site, r.m.Name, r.m.Id, r.s.ProjectDescription }).Distinct().ToList();
            }
        }

        public static bool SaveContractor(ManPowerMaster manPowerMaster)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                if (manPowerMaster.Id == 0)
                {
                    solarPMSEntities.ManPowerMasters.Add(manPowerMaster);
                }
                else
                {
                    ManPowerMaster updateManPwer = solarPMSEntities.ManPowerMasters.FirstOrDefault(m => m.Id == manPowerMaster.Id);
                    updateManPwer.Site = manPowerMaster.Site;
                    updateManPwer.Project = manPowerMaster.Project;
                    updateManPwer.Name = manPowerMaster.Name;
                    updateManPwer.ModifiedBy = manPowerMaster.CreatedBy;
                    updateManPwer.ModifiedOn = DateTime.Now;

                }

                solarPMSEntities.SaveChanges();
                if (manPowerMaster.Id != 0)
                    return true;
                else
                    return false;
            }
        }

        public static bool IsContractorAlreadyExists(ManPowerMaster manPowerMaster)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var result = solarPMSEntities.ManPowerMasters.FirstOrDefault(m => m.Name.ToLower() == manPowerMaster.Name.ToLower()
                                                                              && m.Site == manPowerMaster.Site
                                                                              && m.Project == manPowerMaster.Project
                                                                              && m.Id != manPowerMaster.Id);
                if (result != null)
                    return true;
                else
                    return false;
            }
        }

        public static int SaveManPowerDetails(ManPowerDetail manPowerDetail)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                if (manPowerDetail.Id == 0)
                {
                    solarPMSEntities.ManPowerDetails.Add(manPowerDetail);
                }
                else
                {
                    ManPowerDetail updateManPwer = solarPMSEntities.ManPowerDetails.FirstOrDefault(m => m.Id == manPowerDetail.Id);
                    updateManPwer.Site = manPowerDetail.Site;
                    updateManPwer.Project = manPowerDetail.Project;
                    updateManPwer.AreaId = manPowerDetail.AreaId;
                    updateManPwer.Network = manPowerDetail.Network;
                    updateManPwer.UnskilledLabourCount = manPowerDetail.UnskilledLabourCount;
                    updateManPwer.ElectricalLabourCount = manPowerDetail.ElectricalLabourCount;
                    updateManPwer.MechanicalLabourCount = manPowerDetail.MechanicalLabourCount;
                    updateManPwer.CivilLabourCount = manPowerDetail.CivilLabourCount;
                    updateManPwer.Shift = manPowerDetail.Shift;
                    updateManPwer.Date = manPowerDetail.Date;
                    updateManPwer.ModifiedBy = manPowerDetail.CreatedBy;
                    updateManPwer.ModifiedOn = DateTime.Now;
                    updateManPwer.Comments = manPowerDetail.Comments;
                    updateManPwer.BlockNumbers = manPowerDetail.BlockNumbers;
                }

                solarPMSEntities.SaveChanges();
                return manPowerDetail.Id;
            }
        }

        public static void SaveOfflineManPowerDetails(ManPowerDetail manPowerDetail, string Reason, bool IsValid)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                OfflineManPowerDetail updateManPwer = new OfflineManPowerDetail()
                {
                    Site = manPowerDetail.Site,
                    Project = manPowerDetail.Project,
                    AreaId = manPowerDetail.AreaId,
                    Network = manPowerDetail.Network,
                    UnskilledLabourCount = manPowerDetail.UnskilledLabourCount,
                    ElectricalLabourCount = manPowerDetail.ElectricalLabourCount,
                    MechanicalLabourCount = manPowerDetail.MechanicalLabourCount,
                    CivilLabourCount = manPowerDetail.CivilLabourCount,
                    Shift = manPowerDetail.Shift,
                    Date = manPowerDetail.Date,
                    Comments = manPowerDetail.Comments,
                    BlockNumbers = manPowerDetail.BlockNumbers,
                    IsValid = IsValid,
                    Reason = Reason,
                    ContractorId = manPowerDetail.ContractorId,
                    CreatedBy = manPowerDetail.CreatedBy,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                };

                solarPMSEntities.OfflineManPowerDetails.Add(updateManPwer);
                solarPMSEntities.SaveChanges();
            }
        }

        public static List<ListItem> GetContractorList(string Site, string Project)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<ListItem> lstContractor = new List<ListItem>();
                var result = solarPMSEntities.ManPowerMasters.Where(m => m.Site == Site && m.Project == Project).ToList();

                if (result != null && result.Count > 0)
                {
                    result.ForEach(r => lstContractor.Add(new ListItem() { Id = r.Id.ToString(), Name = r.Name }));
                }

                return lstContractor;
            }
        }

        public static List<ManPowerMasterDetails> GetContractorDetailsList(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetManPowerMasterDetails(UserId).ToList();
            }
        }

        public static List<ContractorLaborDetails> GetContractorDetails(string Site, string Project, int Area, string Network, DateTime Date, int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetContractorLaborDetails(UserId, Site, Project, Area, Network, Date)
                    .ToList();
                    
            }
        }

        public static bool IsManPowerDetailsExists(ManPowerDetail manPowerDetail)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var result = solarPMSEntities.ManPowerDetails.FirstOrDefault(r => r.AreaId == manPowerDetail.AreaId
                                                                         && r.ContractorId == manPowerDetail.ContractorId
                                                                         && r.Site == manPowerDetail.Site
                                                                         && r.Project == manPowerDetail.Project
                                                                         && r.Network == manPowerDetail.Network
                                                                         && r.Id != manPowerDetail.Id
                                                                         && r.Date == manPowerDetail.Date
                                                                         && r.ContractorId == manPowerDetail.ContractorId
                                                                         && r.Shift == manPowerDetail.Shift
                                                                         && !r.IsDeleted);
                if (result != null)
                    return true;
                return false;
            }
        }

        public static int Delete(int Id, int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var result = solarPMSEntities.ManPowerDetails.FirstOrDefault(r => r.Id == Id);
                result.IsDeleted = true;
                result.ModifiedBy = UserId;
                result.ModifiedOn = DateTime.Now;
                solarPMSEntities.SaveChanges();
                return result.Id;

            }
        }
    }

    [Serializable]
    public partial class ManPowerDetail { }

    [Serializable]
    public partial class ManPowerMaster { }
}