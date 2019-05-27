using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class TableModel
    {
        /// <summary>
        /// This method used to get all table master details
        /// </summary>
        /// <returns></returns>
        public List<TableMaster> GetAllTableInfo()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.TableMasters.ToList();
            }
        }

        /// <summary>
        /// This method used to add table master details
        /// </summary>
        /// <param name="tableMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TableMaster AddTable(TableMaster tableMaster, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                tableMaster.Status = true;
                tableMaster.CreatedBy = userId;
                tableMaster.CreatedOn = DateTime.Now;
                tableMaster.ModifiedBy = userId;
                tableMaster.ModifiedOn = DateTime.Now;
                solarPMSEntities.TableMasters.Add(tableMaster);
                solarPMSEntities.SaveChanges();
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return tableMaster;
            }
        }

        /// <summary>
        /// This method used to update table master details.
        /// </summary>
        /// <param name="tableMaster"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateTableDetail(TableMaster tableMaster, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                TableMaster Table = solarPMSEntities.TableMasters.AsNoTracking().FirstOrDefault(l => l.TableId == tableMaster.TableId);
                if (Table != null)
                {
                    tableMaster.CreatedBy = Table.CreatedBy;
                    tableMaster.CreatedOn = Table.CreatedOn;
                    tableMaster.Status = Table.Status;
                    tableMaster.ModifiedBy = userId;
                    tableMaster.ModifiedOn = DateTime.Now;
                    solarPMSEntities.Entry(tableMaster).State = EntityState.Modified;
                    solarPMSEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// This method used to check for duplicate table master entry.
        /// </summary>
        /// <param name="tableMaster"></param>
        /// <returns></returns>
        public bool TableExists(TableMaster tableMaster)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.TableMasters.AsNoTracking().FirstOrDefault(t =>
                t.Site.ToLower() == tableMaster.Site.ToLower()
                && t.ProjectId == tableMaster.ProjectId
                && t.Block == tableMaster.Block
                && t.Invertor == tableMaster.Invertor
                && t.SCB == tableMaster.SCB
                && t.TableId != tableMaster.TableId) != null;
            }
        }
       
    }
}