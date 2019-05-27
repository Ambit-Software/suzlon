using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SuzlonBPP.Models
{
    public class ApproverModel
    {
        #region "Public Methods"

        

        public DataSet GetAgainstBillApprover(int VerticalID)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                conn = new SqlConnection(suzlonBPPEntities.Database.Connection.ConnectionString);
                cmd = new SqlCommand("sp_GetAgainstBillApproverData",conn);
                cmd.Parameters.AddWithValue("@VerticalID", VerticalID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                return ds;
            }
        }


        public DataSet GetReqPendingForApprovalByVertical(int VerticalID)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;

            using (SuzlonBPPEntities suzlonBPPEntities = new SuzlonBPPEntities())
            {
                conn = new SqlConnection(suzlonBPPEntities.Database.Connection.ConnectionString);
                cmd = new SqlCommand("sp_GetVendorData", conn);
                cmd.Parameters.AddWithValue("@VerticalID", VerticalID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                return ds;
            }
        }
       


        
        #endregion "Public Methods"
    }
}