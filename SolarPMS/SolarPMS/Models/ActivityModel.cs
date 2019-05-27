using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class ActivityModel
    {

        public ActivityList GetActivityByNetworkID(int userId, int networkId)
        {
            ActivityList networkList = new ActivityList();

            List<Activities> myRecordList = new List<Activities>();
            List<Activities> pendingForApprovalRecordsList = new List<Activities>();
            List<Activities> approvedRecordsList = new List<Activities>();
            List<Activities> rejectedRecordsList = new List<Activities>();

            CommonFunctions commonFunction = new CommonFunctions();
            SqlConnection dbConnection = commonFunction.GetConnection();
            //using (SqlConnection dbConnection = new SqlConnection(commonFunction.GetConnection()))
            using (SqlCommand dbCommand = new SqlCommand())
            {

                dbCommand.Connection = dbConnection;
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "[dbo].[GetActivity]";
                dbCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                dbCommand.Parameters.Add(new SqlParameter("@NetworkId", networkId));

                dbConnection.Open();

                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    myRecordList.Add(
                        new Activities()
                        {
                            ActivityId = (int)reader["ActivityId"],
                            NetWorkId = (int)reader["NetWorkId"],
                            SAPActivity = (string)reader["SAPActivity"],
                            ActivityActualQty = (decimal)reader["ActivityActualQty"],
                            ActivityActualQtyUoM = (string)reader["ActivityActualQtyUoM"],
                            ActivityDescription = (string)reader["ActivityDescription"],
                            ActivityPlanStartDate = (DateTime)reader["ActivityPlanStartDate"],
                            ActivityActualFinishDate = (DateTime)reader["ActivityActualFinishDate"],
                            ActivityActualStartDate = (DateTime)reader["ActivityActualStartDate"]
                        });
                }

                reader.NextResult();

                while (reader.Read())
                {
                    pendingForApprovalRecordsList.Add(
                        new Activities()
                        {
                            ActivityId = (int)reader["ActivityId"],
                            NetWorkId = (int)reader["NetWorkId"],
                            SAPActivity = (string)reader["SAPActivity"],
                            ActivityActualQty = (decimal)reader["ActivityActualQty"],
                            ActivityActualQtyUoM = (string)reader["ActivityActualQtyUoM"],
                            ActivityDescription = (string)reader["ActivityDescription"],
                            ActivityPlanStartDate = (DateTime)reader["ActivityPlanStartDate"],
                            ActivityActualFinishDate = (DateTime)reader["ActivityActualFinishDate"],
                            ActivityActualStartDate = (DateTime)reader["ActivityActualStartDate"]
                        });
                }

                while (reader.Read())
                {
                    approvedRecordsList.Add(
                        new Activities()
                        {
                            ActivityId = (int)reader["ActivityId"],
                            NetWorkId = (int)reader["NetWorkId"],
                            SAPActivity = (string)reader["SAPActivity"],
                            ActivityActualQty = (decimal)reader["ActivityActualQty"],
                            ActivityActualQtyUoM = (string)reader["ActivityActualQtyUoM"],
                            ActivityDescription = (string)reader["ActivityDescription"],
                            ActivityPlanStartDate = (DateTime)reader["ActivityPlanStartDate"],
                            ActivityActualFinishDate = (DateTime)reader["ActivityActualFinishDate"],
                            ActivityActualStartDate = (DateTime)reader["ActivityActualStartDate"]
                        });
                }

                while (reader.Read())
                {
                    rejectedRecordsList.Add(
                        new Activities()
                        {
                            ActivityId = (int)reader["ActivityId"],
                            NetWorkId = (int)reader["NetWorkId"],
                            SAPActivity = (string)reader["SAPActivity"],
                            ActivityActualQty = (decimal)reader["ActivityActualQty"],
                            ActivityActualQtyUoM = (string)reader["ActivityActualQtyUoM"],
                            ActivityDescription = (string)reader["ActivityDescription"],
                            ActivityPlanStartDate = (DateTime)reader["ActivityPlanStartDate"],
                            ActivityActualFinishDate = (DateTime)reader["ActivityActualFinishDate"],
                            ActivityActualStartDate = (DateTime)reader["ActivityActualStartDate"]
                        });
                }

                networkList.myRecordList = myRecordList;
                networkList.pendingForApprovalRecordsList = pendingForApprovalRecordsList;
                networkList.approvedRecordsList = approvedRecordsList;
                networkList.rejectedRecordsList = rejectedRecordsList;

                dbConnection.Close();
            }
            return networkList;
        }

        public dynamic GetSAPMasterByActitiyId(Activity activity)
        {

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var SAPMasterDetail = (from activities in solarPMSEntities.Activities
                                       join sapMaster in solarPMSEntities.SAPMasters
                                       on activities.SAPActivity equals sapMaster.SAPActivity
                                       join wBSAreas in solarPMSEntities.WBSAreas
                                       on activities.WBSAreaId equals wBSAreas.WBSAreaId
                                       where activities.ActivityId == activity.ActivityId
                                       && activities.SAPNetwork == sapMaster.SAPNetwork
                                       && activities.SAPSite == sapMaster.SAPSite
                                       && activities.SAPProject == sapMaster.SAPProjectId
                                       && wBSAreas.WBSArea1 == sapMaster.WBSArea
                                       select new
                                       {
                                           sapMaster.SAPId,
                                           sapMaster.SAPNetwork,
                                           sapMaster.SAPActivity,
                                           sapMaster.ActivityDescription,
                                           sapMaster.ActivityPlanStartDate,
                                           sapMaster.ActivityPlanFinishDate,
                                           sapMaster.ActivityActualStartDate,
                                           sapMaster.ActivityPlanQtyUoM,
                                           sapMaster.ActivityQty,
                                           sapMaster.MobileFunction,
                                           sapMaster.NetworkDescription,
                                           sapMaster.SAPSite,
                                           sapMaster.SAPProjectId,
                                           sapMaster.ProjectDescription,
                                           sapMaster.BlockTable,
                                           sapMaster.WBSArea,
                                           activities.ActivityId,
                                           activities.NetWorkId,
                                           wBSAreas.WBSAreaId
                                       }).FirstOrDefault();
                return SAPMasterDetail;
            }
        }

        public dynamic GetSAPMasterBySubActitiyId(SubActivity subactivityObj)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var SAPMasterDetail = (from subActivities in solarPMSEntities.SubActivities
                                       join sapMaster in solarPMSEntities.SAPMasters
                                       on subActivities.SAPSubActivity equals sapMaster.SAPSubActivity
                                       join activities in solarPMSEntities.Activities
                                       on subActivities.ActivityId equals activities.ActivityId
                                       join wBSAreas in solarPMSEntities.WBSAreas
                                       on activities.WBSAreaId equals wBSAreas.WBSAreaId
                                       where subActivities.SubActivityId == subactivityObj.SubActivityId
                                       && activities.SAPNetwork == sapMaster.SAPNetwork
                                       && activities.SAPSite == sapMaster.SAPSite
                                       && activities.SAPProject == sapMaster.SAPProjectId
                                       && activities.SAPActivity == sapMaster.SAPActivity
                                       && sapMaster.WBSArea == wBSAreas.WBSArea1
                                       //&& activities.SAPActivity == sapMaster.ActivityElementof
                                       select new
                                       {
                                           sapMaster.SAPId,
                                           sapMaster.SAPNetwork,
                                           sapMaster.SAPActivity,
                                           sapMaster.ActivityDescription,
                                           sapMaster.ActivityPlanStartDate,
                                           sapMaster.ActivityPlanFinishDate,
                                           sapMaster.ActivityActualStartDate,
                                           sapMaster.ActivityPlanQtyUoM,
                                           sapMaster.ActivityQty,
                                           sapMaster.MobileFunction,
                                           sapMaster.NetworkDescription,
                                           sapMaster.SAPSite,
                                           sapMaster.SAPProjectId,
                                           sapMaster.WBSArea,
                                           sapMaster.ProjectDescription,
                                           sapMaster.BlockTable,
                                           sapMaster.SAPSubActivity,
                                           sapMaster.SAPSubActivityDescription,
                                           subActivities.ActivityId,
                                           wBSAreas.WBSAreaId
                                       }).FirstOrDefault();
                return SAPMasterDetail;
            }
        }

        public static List<ActivityToSendNotification> GetActivityAssignmentDetailsForNotification(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetActivityToSendNotification(UserId).ToList();
            }
        }

        public static List<MobileDashbaord> GetMobileDashboardData(TaskModel.ToDoTask Activity, bool IsFromMobile=true)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetDataForMobileDashbaord(Activity.userId, IsFromMobile, Activity.network,Convert.ToString(Activity.areaId), Activity.project, Activity.site).ToList();
            }
        }
    }

    public class ActivityList
    {
        public List<Activities> myRecordList;
        public List<Activities> pendingForApprovalRecordsList;
        public List<Activities> approvedRecordsList;
        public List<Activities> rejectedRecordsList;

    }

    public partial class Activities
    {
        public int? ActivityId { get; set; }
        public int? NetWorkId { get; set; }
        public string SAPActivity { get; set; }
        public decimal? ActivityActualQty { get; set; }
        public string ActivityActualQtyUoM { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime? ActivityPlanStartDate { get; set; }
        public DateTime? ActivityActualFinishDate { get; set; }
        public DateTime? ActivityActualStartDate { get; set; }
    }

    public partial class TableActivityRange
    {
        public int? FromRange { get; set; }
        public int? ToRange { get; set; }
        public string Flag { get; set; }
        public string Site { get; set; }
        public string ProjectId { get; set; }
        public string NetworkId { get; set; }
        public string ActivityId { get; set; }
        public string SubActivityId { get; set; }
        public int? WBSAreaId { get; set; }

    }

    public partial class ManualEntryRange
    {
        public int[] RangeArray { get; set; }
        public string Flag { get; set; }
        public string Site { get; set; }
        public string ProjectId { get; set; }
        public string NetworkId { get; set; }
        public string ActivityId { get; set; }
        public string SubActivityId { get; set; }
        public int? WBSAreaId { get; set; }

    }

    public partial class TableBlock
    {
        public int? BlockNo { get; set; }
        public string Flag { get; set; }
        public string Site { get; set; }
        public string ProjectId { get; set; }
        public string NetworkId { get; set; }
        public string ActivityId { get; set; }
        public string SubActivityId { get; set; }
        public string WBSArea { get; set; }

    }
}