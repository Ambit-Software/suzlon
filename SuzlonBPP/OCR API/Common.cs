using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace OCRAPI
{
    class Common
    {
        static SqlConnection m_sqlConn = new SqlConnection();

        public static bool ConnectDB()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SuzlonBPP"].ConnectionString.ToString();
                m_sqlConn = new SqlConnection(connectionString);
                m_sqlConn.Open();
                return true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static DataTable GetDataTable(string Query)
        {
            DataTable dt = new DataTable();

            try
            {
                ConnectDB();
                SqlDataAdapter da = new SqlDataAdapter(Query, m_sqlConn);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
