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
    
    public partial class TimesheetSurveyDetail
    {
        public int TimesheetSurveyDetailId { get; set; }
        public Nullable<int> TimesheetId { get; set; }
        public Nullable<int> VillageId { get; set; }
        public string SurveyNo { get; set; }
        public Nullable<int> ActualNoOfDivision { get; set; }
        public Nullable<decimal> ActualArea { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual Timesheet Timesheet { get; set; }
    }
}