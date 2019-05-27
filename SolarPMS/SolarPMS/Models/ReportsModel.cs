using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class ReportsModel
    {

        public static List<Report> GetReportList()
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.Reports.Where(r => r.IsDeleted == false).ToList();
            }
        }

        public static List<ReportParameter> GetReportParameterList(int ReportId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                return solarPMSEntities.ReportParameters.Where(r => r.ReportId == ReportId && r.IsEnabled).ToList();
            }
        }
    }
}