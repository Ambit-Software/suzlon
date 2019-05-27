using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SolarPMS.Models
{
    public class TaskModel
    {

        public List<ListItem> GetAllSite()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                //solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                var result = solarPMSEntities.SAPMasters.Where(s => s.IsDeleted == null || s.IsDeleted == false).Select(s => s.SAPSite).Distinct().ToList();
                List<ListItem> lstSite = null;
                if (result != null)
                {
                    lstSite = new List<ListItem>();
                    foreach (string site in result)
                        lstSite.Add(new ListItem() { Id = site.Trim(), Name = site.Trim() });
                }
                return lstSite;
            }
        }

        /// <summary>
        /// Get all projects according to site.
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public List<ListItem> GetAllProjects(string SiteId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<ListItem> result = solarPMSEntities.SAPMasters.Where(s => s.SAPSite == SiteId && (s.IsDeleted == null || s.IsDeleted == false))
                                                        .Select(s => new ListItem { Id = s.SAPProjectId.Trim(), Name = s.ProjectDescription.Trim() })
                                                        .Distinct().OrderBy(l => l.Name).ToList();
                return result;
            }
        }

        /// <summary>
        /// Get user list for taska allocation.
        /// </summary>
        /// <returns></returns>
        public List<ListItem> GetAllUsers()
        {
            UserModel userModel = new UserModel();
            List<ListItem> lstUser = new List<ListItem>();
            List<UserModel> lstAllUsers = userModel.GetAllUserInfo();

            foreach (UserModel user in lstAllUsers.Where(l => l.UserDetail.Status == true))
            {
                lstUser.Add(new ListItem()
                {
                    Id = user.UserDetail.UserId.ToString(),
                    Name = user.UserDetail.Name + " (" + user.ProfileName + ")"
                });
            }

            return lstUser.OrderBy(l => l.Name).ToList();
        }


        /// <summary>
        /// Get users profile details according to user id.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ProfileMaster GetUserProfileDetails(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.ProfileMasters.Where(p => p.ProfileId == (solarPMSEntities.Users.Where(u => u.UserId == UserId).
                                                                                  Select(u => u.ProfileId).FirstOrDefault())).FirstOrDefault();
            }
        }

        public List<UserForCopy> GetUsersForCopyTaskAllocation(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetUserForCopy(UserId).ToList();
            }
        }
        /// <summary>
        /// Get master data for task allocation.
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="SiteId"></param>
        /// <param name="UserId"></param>
        /// <param name="Flag"></param>
        /// <param name="AreaId"></param>
        /// <param name="NetworkId"></param>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        public List<TaskAllocationMasterData> GetTaskAllocationMasterData(string ProjectId, string SiteId, int UserId, int Flag, string AreaId = "", string NetworkId = "", string ActivityId = "")
        {
            List<TaskAllocationMasterData> lstArea = new List<TaskAllocationMasterData>();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetTaskAllocationData(ProjectId, SiteId, AreaId, NetworkId, ActivityId, Flag, UserId).ToList();
            }
        }

        public void SaveTaskDetails(List<TaskAllocationData> TaskData, int userId, string site, string projectId, int loggedInUserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                ExecuteTableValueProcedure<TaskAllocationData>(solarPMSEntities, TaskData, userId, site, projectId, loggedInUserId);
            }
        }

        public void CopyTaskDetails(int userId, string usersToCopy, int loggedInUserId, string sapSite, string projectId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var result = solarPMSEntities.usp_CopyTaskAllocation(userId, usersToCopy, loggedInUserId, sapSite, projectId);
            }
        }
        /// <summary>
        /// Execute store procedure to save task allocation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <param name="site"></param>
        /// <param name="projectId"></param>
        public void ExecuteTableValueProcedure<T>(DbContext context, List<TaskAllocationData> data, int userId, string site, string projectId, int loggedInUserId)
        {
            CommonFunctions commonFunction = new CommonFunctions();
            //// convert source data to DataTable 
            DataTable table = ToDataTable(data);
            SqlConnection con = commonFunction.GetConnection();
            //// create parameter 
            SqlParameter parameter = new SqlParameter("@TaskDataTable", table);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = "TaskDataTable";

            //// execute sp sql 
            string sql = String.Format("EXEC {0} {1};", "usp_SaveTaskAllocationDetails", "@TaskDataTable");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.Parameters.Add(parameter);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@SiteId", site);
            cmd.Parameters.AddWithValue("@ProjectId", projectId);
            cmd.Parameters.AddWithValue("@CreatedById", loggedInUserId);
            cmd.CommandText = "usp_SaveTaskAllocationDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            con.Close();
        }


        /// <summary>
        /// Convert list to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public dynamic GetAllocatedTaskActivity(string projectId, string siteId)
        {
            List<TaskAllocatedDetail> taskAllocatedDetail = new List<TaskAllocatedDetail>();

            CommonFunctions commonFunction = new CommonFunctions();
            SqlConnection dbConnection = commonFunction.GetConnection();

            using (SqlCommand dbCommand = new SqlCommand())
            {

                dbCommand.Connection = dbConnection;
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "[dbo].[usp_TaskDetailsAllocated]";
                dbCommand.Parameters.Add(new SqlParameter("@SAPProjectId", projectId));
                dbCommand.Parameters.Add(new SqlParameter("@SAPSite", siteId));

                dbConnection.Open();
                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    taskAllocatedDetail.Add(
                        new TaskAllocatedDetail()
                        {
                            SAPSite = Convert.ToString(reader["SAPSite"]),
                            ProjectDescription = Convert.ToString(reader["ProjectDescription"]),
                            WBSArea = Convert.ToString(reader["WBSArea"]),
                            SAPNetWork = Convert.ToString(reader["SAPNetWork"]),
                            SAPActivity = Convert.ToString(reader["SAPActivity"]),
                            SAPSubActivity = Convert.ToString(reader["SAPSubActivity"]),
                            CountSiteFunctionalUser = Convert.ToString(reader["CountSiteFunctionalUser"]),
                            SiteFunctionalUser = Convert.ToString(reader["SiteFunctionalUser"]),
                            CountSiteFunctionalManager = Convert.ToString(reader["CountSiteFunctionalManager"]),
                            SiteFunctionalManager = Convert.ToString(reader["SiteFunctionalManager"]),
                            CountSiteQualityManager = Convert.ToString(reader["CountSiteQualityManager"]),
                            SiteQualityManager = Convert.ToString(reader["SiteQualityManager"]),
                            CountStateProjectHead = Convert.ToString(reader["CountStateProjectHead"]),
                            StateProjectHead = Convert.ToString(reader["StateProjectHead"]),
                            CountDesignEngineer = Convert.ToString(reader["CountDesignEngineer"]),
                            DesignEngineer = Convert.ToString(reader["DesignEngineer"]),
                            CountDesignManager = Convert.ToString(reader["CountDesignManager"]),
                            DesignManager = Convert.ToString(reader["DesignManager"])
                        });
                }

                reader.NextResult();

                while (reader.Read())
                {
                    taskAllocatedDetail.Add(
                        new TaskAllocatedDetail()
                        {
                            SAPSite = Convert.ToString(reader["SAPSite"]),
                            ProjectDescription = Convert.ToString(reader["ProjectDescription"]),
                            WBSArea = Convert.ToString(reader["WBSArea"]),
                            SAPNetWork = Convert.ToString(reader["SAPNetWork"]),
                            SAPActivity = Convert.ToString(reader["SAPActivity"]),
                            SAPSubActivity = Convert.ToString(reader["SAPSubActivity"]),
                            CountSiteFunctionalUser = Convert.ToString(reader["CountSiteFunctionalUser"]),
                            SiteFunctionalUser = Convert.ToString(reader["SiteFunctionalUser"]),
                            CountSiteFunctionalManager = Convert.ToString(reader["CountSiteFunctionalManager"]),
                            SiteFunctionalManager = Convert.ToString(reader["SiteFunctionalManager"]),
                            CountSiteQualityManager = Convert.ToString(reader["CountSiteQualityManager"]),
                            SiteQualityManager = Convert.ToString(reader["SiteQualityManager"]),
                            CountStateProjectHead = Convert.ToString(reader["CountStateProjectHead"]),
                            StateProjectHead = Convert.ToString(reader["StateProjectHead"]),
                            CountDesignEngineer = Convert.ToString(reader["CountDesignEngineer"]),
                            DesignEngineer = Convert.ToString(reader["StateProjectHead"]),
                            CountDesignManager = Convert.ToString(reader["CountDesignManager"]),
                            DesignManager = Convert.ToString(reader["DesignManager"])
                        });
                }

                dbConnection.Close();
            }
            return taskAllocatedDetail;


        }
        public List<SAPActivitiesNotAllocated> GetNotAlocatedTask(string projectId, string siteId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;

                List<SAPActivitiesNotAllocated> lstNotAllocated = solarPMSEntities.usp_SAPActivitiesNotAllocated(projectId, siteId).ToList();
                return lstNotAllocated;
            }
        }

        public List<ToDoList> GetToDoList(int userId, string network, string area, string project, string site, bool isFromMobile)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetToDoList(userId, isFromMobile, network, area, project, site, 1).ToList();
            }
        }

        public List<ToDoListPendingForApproval> GetToDoPendingList(int userId, string network, string area, string project, string site, bool isFromMobile)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_ToDoListPendingForApproval(userId, isFromMobile, network, area, project, site).ToList();
            }
        }

        public List<ToDoListApproved> GetToDoApprovedList(int userId, string network, string area, string project, string site, bool isFromMobile)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_ToDoListApproved(userId, isFromMobile, network, area, project, site).ToList();
            }
        }

        public List<ToDoListRejected> GetToDoRejectedList(int userId, string network, string area, string project, string site, bool isFromMobile)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_ToDoListRejected(userId, isFromMobile, network, area, project, site).ToList();
            }
        }

        public List<usp_ToDoListMyRecords_Result> GetMyRecords(int userId, bool isFromMobile, string network, string area, string project, string site)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_ToDoListMyRecords(userId, isFromMobile, network, area, project, site).ToList();
            }
        }

        public List<ToDoListPendingForApproval> GetPendingForApproval(int userId, bool isFromMobile, string network, string area, string project, string site)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_ToDoListPendingForApproval(userId, isFromMobile, network, area, project, site).ToList();
            }
        }


        public DropdownValues GetAllocatedSite(int userId)
        {
            DropdownValues ddValues = new DropdownValues();
            ddValues.site = new List<ListItem>();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.usp_GetUserAssignedTaskDataNonDE(userId,"site",string.Empty,string.Empty,string.Empty).ToList().ForEach(l =>
                      {
                          ddValues.site.Add(new ListItem() { Id = l.Id.Trim(), Name = l.Value.Trim() });
                      });
            }
            return ddValues;
        }

        public DropdownValues GetAllocatedProject(int userId, string site)
        {
            DropdownValues ddValues = new DropdownValues();
            ddValues.project = new List<ListItem>();
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                //var projectList = (from taskAllocation in solarPMSEntities.TaskAllocations
                //                   join sapMaster in solarPMSEntities.SAPMasters
                //                   on taskAllocation.SAPProjectId equals sapMaster.SAPProjectId 
                //                   where taskAllocation.UserId == userId && taskAllocation.SAPSite == site
                //                   && sapMaster.SAPSite == site
                //                   && sapMaster.MobileFunction.ToLower() != "f"
                //                   orderby sapMaster.ProjectDescription ascending
                //                   select new
                //                   {
                //                       taskAllocation.SAPProjectId,
                //                       sapMaster.ProjectDescription
                //                   }).Distinct().ToList();

                solarPMSEntities.usp_GetUserAssignedTaskDataNonDE(userId, "project",site, string.Empty, string.Empty).ToList().ForEach(l =>
                {
                    ddValues.project.Add(new ListItem() { Id = l.Id.Trim(), Name = l.Value.Trim() });
                });               

                return ddValues;
            }
        }

        public List<ListItem> GetAllocatedNetwork(int userId, string siteId, string projectId, int areaId)
        {
            List<ListItem> lstNetwok = new List<ListItem>();

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;

                //var netWorkList = (from network in solarPMSEntities.NetWorks
                //                   join area in solarPMSEntities.Areas
                //                   on network.AreaId equals area.AreaId
                //                   join wArea in solarPMSEntities.WBSAreas
                //                   on area.WBSAreaId equals wArea.WBSAreaId
                //                   join sapMaster in solarPMSEntities.SAPMasters
                //                   on network.SAPNetWork equals sapMaster.SAPNetwork
                //                   join taskAllocations in solarPMSEntities.TaskAllocations
                //                   on area.TaskAllocationId equals taskAllocations.TaskAllocationId
                //                   where
                //                   wArea.WBSAreaId == areaId
                //                   && sapMaster.SAPSite == siteId
                //                   && sapMaster.SAPProjectId == projectId
                //                   && sapMaster.WBSArea == wArea.WBSArea1
                //                   && taskAllocations.UserId == userId
                //                   && sapMaster.MobileFunction.ToLower() != "f"
                //                   orderby sapMaster.SAPNetwork ascending
                //                   select
                //                   new
                //                   {
                //                       sapMaster.SAPNetwork,
                //                       sapMaster.NetworkDescription
                //                   }).Distinct().ToList();

                solarPMSEntities.usp_GetUserAssignedTaskDataNonDE(userId, "network", siteId, projectId, areaId.ToString()).ToList().ForEach(l =>
                {
                    lstNetwok.Add(new ListItem() { Id = l.Id.Trim(), Name = l.Value.Trim() });
                });

                //if (netWorkList != null)
                //{
                //    foreach (var network in netWorkList)
                //    {
                //        lstNetwok.Add(new ListItem() { Id = network.SAPNetwork.ToString().Trim(), Name = network.NetworkDescription.Trim() });
                //    }
                //}
            }
            return lstNetwok;
        }

        public List<DesignEnggActivityDetails> GetDesignEngggActivityRecords(int userId, string Area, string SapSite, string Project, string Network)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetDesignEnggActivityDetails(userId, Area, SapSite, Project, Network).ToList();
            }
        }

        public static int GetHighestVersionOfDocument(string SapSite, string SapProject,int Area, string SapNetwork, string SapActivity, string SapSubActivity)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<int> result = new List<int>();
                if (!string.IsNullOrEmpty(SapSubActivity))
                {
                    result = solarPMSEntities.DesignEngineerDocuments
                                   .Where(d => d.SAPSubActivity==SapSubActivity
                                            && d.SAPActivity == SapActivity
                                            && d.SAPNetwork == SapNetwork
                                            && d.WBSAreaId == Area
                                            && d.SAPSite == SapSite
                                            && d.SAPProject == SapProject
                                            //&& d.CreatedBy == UserId
                                            )
                                            .Select(d => d.Version).ToList();

                }
                else
                {
                    result = solarPMSEntities.DesignEngineerDocuments
                                   .Where(d => d.SAPActivity == SapActivity
                                            && d.SAPNetwork == SapNetwork
                                            && d.WBSAreaId == Area
                                            && d.SAPSite == SapSite
                                            && d.SAPProject == SapProject
                                            //&& d.CreatedBy == UserId
                                            )
                                            .Select(d => d.Version).ToList();
                }

                if (result.Count > 0)
                {
                    return result.Max()+1;
                }

                return 1;
            }
        }

        public static void SaveDesignEngineerDocument(DesignEngineerDocument designEngineerDocument, int userId)
        {

            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.DesignEngineerDocuments.Add(designEngineerDocument);
                solarPMSEntities.SaveChanges();
                //SaveFileDetails(designEngineerDocument.DEDocumentFiles.ToList(), designEngineerDocument.Id, userId);
            }
        }

        public static void SaveFileDetails(List<DEDocumentFile> DEAttachment, int DocumentDetailsId, int userId)
        {
            if (DEAttachment != null && DEAttachment.Count > 0)
            {
                using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
                {
                    DEAttachment.ForEach(attachment =>
                    {
                        attachment.DocumentDetailsId = DocumentDetailsId;
                        attachment.CreatedBy = userId;
                        attachment.CreatedOn = DateTime.Now;
                        solarPMSEntities.DEDocumentFiles.Add(attachment);
                    });
                    solarPMSEntities.SaveChanges();
                }
            }
        }

        public static void SaveReviewDetails(DocumentReviewDetail documentReviewDetail, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.DocumentReviewDetails.Add(documentReviewDetail);
                solarPMSEntities.SaveChanges();
            }
        }

        public static List<DesignEngineerDocuments> GetDesignEngineerDocuments(int ActivityId, int SubactivityId, int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.usp_GetDesignEngineerDocuments(ActivityId, SubactivityId,UserId ).ToList();  
            }
        }

        public static void DeleteDEDocument(int DEFileId, int DocumentDetailsId, int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                var fileToBeDeleted = solarPMSEntities.DEDocumentFiles.FirstOrDefault(f => f.Id == DEFileId && f.DocumentDetailsId == DocumentDetailsId);
                fileToBeDeleted.IsDeleted = true;

                var childFiles = solarPMSEntities.DEDocumentFiles.Where(f => f.DocumentDetailsId == DocumentDetailsId && !f.IsDeleted).ToList();

                if (childFiles == null || childFiles.Count <= 1)
                {
                    var documentMasterEntry = solarPMSEntities.DesignEngineerDocuments.FirstOrDefault(f => f.Id == DocumentDetailsId);
                    documentMasterEntry.IsDeleted = true;
                    documentMasterEntry.ModifiedBy = UserId;
                    documentMasterEntry.ModifiedOn = DateTime.Now;
                }
                solarPMSEntities.SaveChanges();
            }
        }

        /// <summary>
        /// Get task allocation details for today after task assignment.
        /// </summary>
        /// <param name="UserId"></param>

        public class TaskAllocatedDetail
        {
            public string SAPSite { get; set; }
            public string ProjectDescription { get; set; }
            public string WBSArea { get; set; }
            public string SAPNetWork { get; set; }
            public string SAPActivity { get; set; }
            public string SAPSubActivity { get; set; }
            public string CountSiteFunctionalUser { get; set; }
            public string SiteFunctionalUser { get; set; }
            public string CountSiteFunctionalManager { get; set; }
            public string SiteFunctionalManager { get; set; }
            public string CountSiteQualityManager { get; set; }
            public string SiteQualityManager { get; set; }
            public string CountStateProjectHead { get; set; }
            public string StateProjectHead { get; set; }
            public string CountDesignEngineer { get; set; }
            public string DesignEngineer { get; set; }
            public string CountDesignManager { get; set; }
            public string DesignManager { get; set; }
        }

        public class TaskNotAllocated
        {
            public string SAPSite { get; set; }
            public string ProjectDescription { get; set; }
            public string WBSArea { get; set; }
            public string SAPNetWork { get; set; }
            public string SAPActivity { get; set; }
            public string SAPSubActivity { get; set; }
            public System.DateTime CreatedDate { get; set; }

        }

        public class ActivityDeleted
        {
            public string SAPSite { get; set; }
            public string ProjectDescription { get; set; }
            public string WBSArea { get; set; }
            public string SAPNetWork { get; set; }
            public string SAPActivity { get; set; }
            public string SAPSubActivity { get; set; }
            public System.DateTime EstStartDate { get; set; }
            public System.DateTime EstEndtDate { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }
            public string SiteFunctionalUser { get; set; }
            public string SiteFunctionalManager { get; set; }
            public string SiteQualityManager { get; set; }
            public string StateProjectHead { get; set; }
            public string DesignEngineer { get; set; }
            public string DesignManager { get; set; }
        }

        public partial class ToDoTask
        {
            public int userId { get; set; }
            public string network { get; set; }
            public string area { get; set; }
            public string project { get; set; }
            public string site { get; set; }
            public bool isFromMobile { get; set; }
            public int areaId { get; set; }

        }

    }

}