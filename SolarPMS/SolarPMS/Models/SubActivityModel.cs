using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class SubActivityModel
    {
        public SubActivityList GetSubActivityByActivityID(int userId, int activityId)
        {
            SubActivityList networkList = new SubActivityList();

            List<SubActivities> myRecordList = new List<SubActivities>();
            List<SubActivities> pendingForApprovalRecordsList = new List<SubActivities>();
            List<SubActivities> approvedRecordsList = new List<SubActivities>();
            List<SubActivities> rejectedRecordsList = new List<SubActivities>();

            CommonFunctions commonFunction = new CommonFunctions();
            SqlConnection dbConnection = commonFunction.GetConnection();
            //using (SqlConnection dbConnection = new SqlConnection(commonFunction.GetConnection()))
            using (SqlCommand dbCommand = new SqlCommand())
            {

                dbCommand.Connection = dbConnection;
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "[dbo].[GetSubActivity]";
                dbCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                dbCommand.Parameters.Add(new SqlParameter("@ActivityId", activityId));

                dbConnection.Open();

                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    myRecordList.Add(
                        new SubActivities()
                        {
                            SubActivityId = (int)reader["SubActivityId"],
                            ActivityId = (int)reader["ActivityId"],
                            SAPSubActivity = (string)reader["SAPSubActivity"],
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
                        new SubActivities()
                        {
                            SubActivityId = (int)reader["SubActivityId"],
                            ActivityId = (int)reader["ActivityId"],
                            SAPSubActivity = (string)reader["SAPSubActivity"],
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
                        new SubActivities()
                        {
                            SubActivityId = (int)reader["SubActivityId"],
                            ActivityId = (int)reader["ActivityId"],
                            SAPSubActivity = (string)reader["SAPSubActivity"],
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
                        new SubActivities()
                        {
                            SubActivityId = (int)reader["SubActivityId"],
                            ActivityId = (int)reader["ActivityId"],
                            SAPSubActivity = (string)reader["SAPSubActivity"],
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

    }

    public class SubActivityList
    {
        public List<SubActivities> myRecordList;
        public List<SubActivities> pendingForApprovalRecordsList;
        public List<SubActivities> approvedRecordsList;
        public List<SubActivities> rejectedRecordsList;

    }

    public partial class SubActivities
    {
        public int? SubActivityId { get; set; }
        public int? ActivityId { get; set; }
        public string SAPSubActivity { get; set; }
        public decimal? ActivityActualQty { get; set; }
        public string ActivityActualQtyUoM { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime? ActivityPlanStartDate { get; set; }
        public DateTime? ActivityActualFinishDate { get; set; }
        public DateTime? ActivityActualStartDate { get; set; }
    }
}