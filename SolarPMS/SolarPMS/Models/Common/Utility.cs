using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SolarPMS.Models.Common
{
    public class Utility
    {
        #region "Public Method"
        /// <summary>
        /// Write exception details to database.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="errorDetails"></param>
        public static void WriteErrorLog(string errorMessage, string errorDetails)
        {

        }
        /// <summary>
        /// Write transaction log to database
        /// </summary>
        public static void WriteTransactionLog()
        {

        }
        #endregion

        public static void ImportDetails()
        {

        }

        public static void ExportDetails(DataTable dt, HttpResponse Response)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            // Here we add five DataRows.
            table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);

            if (table.Rows.Count > 0)
            {
                string filename = "DownloadMobileNoExcel.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                WriteTsv(table, Response.Output);
                Response.End();
            }
        }

        public static void WriteTsv(DataTable data, TextWriter output)
        {
            string tab = "";
            foreach (DataColumn dc in data.Columns)
            {
                output.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            output.Write("\n");
            int i;
            foreach (DataRow dr in data.Rows)
            {
                tab = "";
                for (i = 0; i < data.Columns.Count; i++)
                {
                    output.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                output.Write("\n");
            }
        }
    }
}