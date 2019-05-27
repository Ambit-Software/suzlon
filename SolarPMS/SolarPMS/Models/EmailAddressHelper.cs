using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class EmailAddressHelper
    {
        public static string GetUserAndManagerEmail(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                string emailId = string.Empty;
                User user = solarPMSEntities.Users.FirstOrDefault(r => r.UserId == UserId);
                if (user != null)
                {
                    emailId = user.EmailId;
                }

                var manager = solarPMSEntities.Users
                                        .Join(solarPMSEntities.Users, u1 => u1.UserId, u2 => u2.ReportingManagerId, (u1, u2) => new { u1, u2 })
                                        .Where(r => r.u2.UserId == UserId)
                                        .FirstOrDefault();
                if (manager != null)
                    emailId += ";" + manager.u1.EmailId;
                return emailId;                     
            }
        }


        public static string GetUserEmailId(int UserId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                User user = solarPMSEntities.Users.FirstOrDefault(u => u.UserId == UserId);
                if (user != null)
                    return user.EmailId;
                return string.Empty;
            }
        }

        public static int GetTimesheetUserId(int TimesheetId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.Timesheets.FirstOrDefault(t => t.TimeSheetId == TimesheetId).CreatedBy;                
            }
        }

        public static string GetEmailIdForNotification(int ActivityId, int SubActivityId, int TimesheetId, 
            int NotificationId,int UserId, int AssignedTo, string Profiles)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                return solarPMSEntities.usp_GetUserEmailIdForNotification(UserId, Profiles, NotificationId, ActivityId,
                    SubActivityId, TimesheetId, AssignedTo).FirstOrDefault().ToString();
            }
        }
    }
}