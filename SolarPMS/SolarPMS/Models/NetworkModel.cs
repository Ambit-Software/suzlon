using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class NetworkModel
    {
        public dynamic GetNetwork(int userId, int areaId, string searchText)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;

                var netWorkList = (from taskAllocation in solarPMSEntities.TaskAllocations
                                   join area in solarPMSEntities.Areas
                                    on taskAllocation.TaskAllocationId equals area.TaskAllocationId
                                   join netWorks in solarPMSEntities.NetWorks
                                   on area.AreaId equals netWorks.AreaId
                                   join sapMaster in solarPMSEntities.SAPMasters
                                   on netWorks.SAPNetWork equals sapMaster.SAPNetwork
                                   where taskAllocation.UserId == userId && area.WBSAreaId == areaId
                                    && (string.IsNullOrEmpty(searchText) ? true : (
                                   sapMaster.NetworkDescription.Contains(searchText)))
                                   select new
                                   {
                                       netWorks.NetWorkId,
                                       netWorks.AreaId,
                                       netWorks.SAPNetWork,
                                       sapMaster.NetworkDescription
                                   }).ToList();

                return netWorkList;
            }

        }

        public static List<ListItem> GetNetworksForArea(int userId, int areaId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;

                var netWorkList = (from taskAllocation in solarPMSEntities.TaskAllocations
                                   join area in solarPMSEntities.Areas
                                    on taskAllocation.TaskAllocationId equals area.TaskAllocationId
                                   join netWorks in solarPMSEntities.NetWorks
                                   on area.AreaId equals netWorks.AreaId
                                   join sapMaster in solarPMSEntities.SAPMasters
                                   on netWorks.SAPNetWork equals sapMaster.SAPNetwork
                                   where taskAllocation.UserId == userId && area.WBSAreaId == areaId
                                   select new
                                   {
                                       netWorks.NetWorkId,
                                       sapMaster.NetworkDescription
                                   }).Distinct().ToList();
                List<ListItem> lstNetwok = new List<ListItem>();
                if (netWorkList != null)
                {
                    foreach (var network in netWorkList)
                    {
                        lstNetwok.Add(new ListItem() { Id = network.NetWorkId.ToString(), Name = network.NetworkDescription });
                    }
                }

                return lstNetwok;
            }

        }

        public List<ListItem> GetNetworks(int userId, string siteId, string projectId, int areaId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;

                var netWorkList = (from wBSArea in solarPMSEntities.WBSAreas
                                   join area in solarPMSEntities.Areas
                                   on wBSArea.WBSAreaId equals area.WBSAreaId
                                   join netWorks in solarPMSEntities.NetWorks
                                   on area.AreaId equals netWorks.AreaId
                                   join sapMaster in solarPMSEntities.SAPMasters
                                   on netWorks.SAPNetWork equals sapMaster.SAPNetwork
                                   where area.WBSAreaId == areaId
                                   && sapMaster.SAPSite == siteId
                                   && sapMaster.SAPProjectId == projectId
                                   select new
                                   {
                                       sapMaster.SAPNetwork,
                                       sapMaster.NetworkDescription
                                   }).Distinct().ToList();
                List<ListItem> lstNetwok = new List<ListItem>();
                if (netWorkList != null)
                {
                    foreach (var network in netWorkList)
                    {
                        lstNetwok.Add(new ListItem() { Id = network.SAPNetwork.ToString().Trim(), Name = network.NetworkDescription.Trim() });
                    }
                }

                return lstNetwok;
            }

        }

        public NetworkList GetNetworkByAreaId(int userId, int areaId)
        {
            NetworkList networkList = new NetworkList();

            List<GetToDoNetWorkList> myRecordList = new List<GetToDoNetWorkList>();
            List<GetToDoNetWorkList> pendingForApprovalRecordsList = new List<GetToDoNetWorkList>();
            List<GetToDoNetWorkList> approvedRecordsList = new List<GetToDoNetWorkList>();
            List<GetToDoNetWorkList> rejectedRecordsList = new List<GetToDoNetWorkList>();
            CommonFunctions commonFunction = new CommonFunctions();
            SqlConnection dbConnection = commonFunction.GetConnection();
            //using (SqlConnection dbConnection = new SqlConnection(commonFunction.GetConnection()))
            using (SqlCommand dbCommand = new SqlCommand())
            {

                dbCommand.Connection = dbConnection;
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "[dbo].[GetToDoNetWorkList]";
                dbCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                dbCommand.Parameters.Add(new SqlParameter("@AreaId", areaId));

                dbConnection.Open();

                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    myRecordList.Add(
                        new GetToDoNetWorkList()
                        {
                            NetWorkId = (int)reader["NetWorkId"],
                            SAPNetWork = (string)reader["SAPNetWork"],
                            AreaId = (int)reader["AreaId"],
                            NetworkDescription = (string)reader["NetworkDescription"]
                        });
                }

                reader.NextResult();

                while (reader.Read())
                {
                    pendingForApprovalRecordsList.Add(
                        new GetToDoNetWorkList()
                        {
                            NetWorkId = (int)reader["NetWorkId"],
                            SAPNetWork = (string)reader["SAPNetWork"],
                            AreaId = (int)reader["AreaId"],
                            NetworkDescription = (string)reader["NetworkDescription"]
                        });
                }

                while (reader.Read())
                {
                    approvedRecordsList.Add(
                        new GetToDoNetWorkList()
                        {
                            NetWorkId = (int)reader["NetWorkId"],
                            SAPNetWork = (string)reader["SAPNetWork"],
                            AreaId = (int)reader["AreaId"],
                            NetworkDescription = (string)reader["NetworkDescription"]
                        });
                }

                while (reader.Read())
                {
                    rejectedRecordsList.Add(
                        new GetToDoNetWorkList()
                        {
                            NetWorkId = (int)reader["NetWorkId"],
                            SAPNetWork = (string)reader["SAPNetWork"],
                            AreaId = (int)reader["AreaId"],
                            NetworkDescription = (string)reader["NetworkDescription"]
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

    public class NetworkList
    {
        public List<GetToDoNetWorkList> myRecordList;
        public List<GetToDoNetWorkList> pendingForApprovalRecordsList;
        public List<GetToDoNetWorkList> approvedRecordsList;
        public List<GetToDoNetWorkList> rejectedRecordsList;

    }

    public partial class GetToDoNetWorkList
    {
        public int NetWorkId { get; set; }
        public string SAPNetWork { get; set; }
        public int AreaId { get; set; }
        public string NetworkDescription { get; set; }
    }
}