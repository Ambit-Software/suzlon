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
    
    public partial class usp_ToDoListMyRecords_Result1
    {
        public int ActivityId { get; set; }
        public int SubActivityId { get; set; }
        public string SAPSite { get; set; }
        public string ProjectDescription { get; set; }
        public string NetworkDescription { get; set; }
        public string WBSArea { get; set; }
        public string ActivityDescription { get; set; }
        public string SAPSubActivityDescription { get; set; }
        public decimal ActivityQty { get; set; }
        public string ActivityPlanQtyUoM { get; set; }
        public System.DateTime ActivityPlanStartDate { get; set; }
        public System.DateTime ActivityPlanFinishDate { get; set; }
        public int TimeSheetId { get; set; }
        public int StatusId { get; set; }
        public string MobileFunction { get; set; }
        public string BlockTable { get; set; }
        public string SAPProjectId { get; set; }
        public Nullable<int> WBSAreaId { get; set; }
        public string SAPNetwork { get; set; }
        public string SAPActivity { get; set; }
        public string SAPSubActivity { get; set; }
        public Nullable<int> EstDuration { get; set; }
        public Nullable<System.DateTime> Actualstart { get; set; }
        public Nullable<System.DateTime> ActualEnd { get; set; }
        public Nullable<decimal> ActualQty { get; set; }
        public int ActualDuration { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> CRR { get; set; }
        public Nullable<decimal> RRR { get; set; }
        public int IsTimesheetFound { get; set; }
    }
}
