//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolarPMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TimesheetApproval
    {
        public int TimesheetApprovalId { get; set; }
        public int TimesheetId { get; set; }
        public int WorkflowStatusId { get; set; }
        public string Comment { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual User User { get; set; }
        public virtual WorkflowStatu WorkflowStatu { get; set; }
        public virtual Timesheet Timesheet { get; set; }
    }
}
