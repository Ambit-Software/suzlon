using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarPMS.Models
{
    public class TimesheetApprovalModel
    {
        public TimesheetApproval Add(Timesheet_Approval timesheet_Approval, int userId)
        {
            using (SolarPMSEntities solarPMSEntities = new SolarPMSEntities())
            {
                List<TimesheetListIds> timesheetListIds = timesheet_Approval.TimesheetIds;

                TimesheetApproval timesheetApproval = new TimesheetApproval();

                if (timesheetListIds != null && timesheetListIds.Count > 0)
                {
                    foreach (var timesheetlistId in timesheetListIds)
                    {
                        timesheetApproval = new TimesheetApproval();
                        timesheetApproval.TimesheetId = timesheetlistId.TimesheetId;

                        //if (timesheet_Approval.QAStatus != null)
                        //{
                        //    timesheetApproval.QAComment = timesheet_Approval.QAComment;
                        //    timesheetApproval.QAStatus = timesheet_Approval.QAStatus;
                        //    timesheetApproval.QAApprover = userId;
                        //    timesheetApproval.QAStatusDate = DateTime.Now;
                        //}

                        //if (timesheet_Approval.PMStatus != null)
                        //{
                        //    timesheetApproval.PMStatusDate = DateTime.Now;
                        //    timesheetApproval.PMCommment = timesheet_Approval.PMCommment;
                        //    timesheetApproval.PMStatus = timesheet_Approval.PMStatus;
                        //    timesheetApproval.PMApprover = userId;
                        //}

                        solarPMSEntities.TimesheetApprovals.Add(timesheetApproval);
                        solarPMSEntities.SaveChanges();

                    }
                    solarPMSEntities.Configuration.ProxyCreationEnabled = false;
                }
                return timesheetApproval;
            }
        }
    }

    public partial class Timesheet_Approval
    {
        public Timesheet_Approval()
        {
            this.TimesheetIds = new List<TimesheetListIds>();
        }

        public int TimesheetApprovalId { get; set; }
        public int TimesheetId { get; set; }
        public Nullable<bool> QAStatus { get; set; }
        public Nullable<System.DateTime> QAStatusDate { get; set; }
        public Nullable<bool> PMStatus { get; set; }
        public Nullable<System.DateTime> PMStatusDate { get; set; }
        public Nullable<int> QAApprover { get; set; }
        public Nullable<int> PMApprover { get; set; }
        public string QAComment { get; set; }
        public string PMCommment { get; set; }

        public List<TimesheetListIds> TimesheetIds { get; set; }

    }
    public partial class TimesheetListIds
    {
        public int TimesheetId { get; set; }
    }
}