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
    
    public partial class ActivityAssignmentHistory
    {
        public int AllocationId { get; set; }
        public System.DateTime Date { get; set; }
        public int UserId { get; set; }
        public bool IsAssigned { get; set; }
        public string SAPSite { get; set; }
        public string SAPProject { get; set; }
        public int WBSAreaId { get; set; }
        public string SAPNetwork { get; set; }
        public string SAPActivity { get; set; }
        public string SAPSubactivity { get; set; }
        public int ActivityId { get; set; }
        public Nullable<int> SubActivityId { get; set; }
        public bool IsNotificationSent { get; set; }
    }
}
