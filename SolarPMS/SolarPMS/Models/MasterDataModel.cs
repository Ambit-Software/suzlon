using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class MasterDataModel
    {

        public static List<WorkflowStatu> GetWorkflowStatus()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.WorkflowStatus.ToList();
            }
        }
        public static List<StatusMaster> GetTimesheetStatus()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.StatusMasters.ToList();
            }
        }

        public static List<usp_GetTableMasterDataForOfflineSync_Result> GetTableMasterData(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.usp_GetTableMasterDataForOfflineSync(UserId).ToList();
            }
        }

        public static List<usp_GetTableActivityDataForOfflineSync_Result> GetTableAtctivityMasterData(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.usp_GetTableActivityDataForOfflineSync(UserId).ToList();
            }
        }

        public static List<VillageMaster> GetVillageMasterData()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.VillageMasters.ToList();
            }
        }

        public static List<usp_GetSurveyDataForOfflineSync_Result> GetSurveyMasterData(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.usp_GetSurveyDataForOfflineSync(UserId).ToList();
            }
        }

        public static List<WorkflowStatu> GetWorkflowStatuData()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.WorkflowStatus.ToList();
            }
        }

        public static List<usp_GetUpdatedTablesForOfflineSync_Result> GetUpdatedMasterTables(DateTime LatsSyncDate)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.usp_GetUpdatedTablesForOfflineSync(LatsSyncDate).ToList();
            }
        }

        public static List<usp_GetContractorForOfflineSync_Result> GetContractors(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.usp_GetContractorForOfflineSync(UserId).ToList();
            }
        }

        public static List<usp_SyncMasterDataForManPowerOffline_Result> GetManPowerMaster(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.usp_SyncMasterDataForManPowerOffline(UserId).ToList();
            }
        }
    }
}