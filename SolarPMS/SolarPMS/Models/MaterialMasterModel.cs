using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class MaterialMasterModel
    {

        public static dynamic GetMaterialData()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.Materials.Join(solarPMSEntities.SAPMasters, m => m.ProjectId, s => s.SAPProjectId, (m, s) => new { m, s })
                    .Where(r => r.m.IsDeleted == false && r.m.Site == r.s.SAPSite && r.s.IsDeleted == null)
                    .Select(r => new { r.m.ProjectId, r.m.Site, r.m.MaterialCode, r.m.Id, r.s.ProjectDescription, r.m.IsActive }).Distinct().ToList();
            }
        }

        public static void SaveMaterialData(Material Material)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                if (Material.Id == 0)
                {
                    solarPMSEntities.Materials.Add(Material);
                }
                else
                {
                    Material updateMaterial = solarPMSEntities.Materials.FirstOrDefault(m => m.Id == Material.Id);
                    updateMaterial.Site = Material.Site;
                    updateMaterial.ProjectId = Material.ProjectId;
                    updateMaterial.MaterialCode = Material.MaterialCode;
                    updateMaterial.IsActive = Material.IsActive;
                    updateMaterial.IsDeleted = Material.IsDeleted;
                    updateMaterial.ModifiedBy = Material.CreatedBy;
                    updateMaterial.ModifiedOn = Material.ModifiedOn;
                }
                solarPMSEntities.SaveChanges();
            }
        }

        public static bool IsMaterialCodeAlreadyExists(Material Material)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var result = solarPMSEntities.Materials.FirstOrDefault(m => m.MaterialCode.ToLower() == Material.MaterialCode.ToLower()
                                                                              && m.Id != Material.Id
                                                                              && m.ProjectId == Material.ProjectId
                                                                              && m.Site == Material.Site);
                if (result != null)
                    return true;
                else
                    return false;
            }
        }


        public static List<usp_GetMaterialReportData_Result> GetMaterialReportData(int UserId, string Site)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                List<usp_GetMaterialReportData_Result> lstReportData = solarPMSEntities.usp_GetMaterialReportData(UserId, Site).ToList();
                return lstReportData;
            }
        }
    }
}
