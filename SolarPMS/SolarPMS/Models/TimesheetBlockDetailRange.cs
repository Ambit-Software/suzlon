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
    
    public partial class TimesheetBlockDetailRange
    {
        public int TimesheetBlockDetailRangeId { get; set; }
        public int TimesheetBlockDetailId { get; set; }
        public Nullable<int> Block { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        public virtual TimesheetBlockDetail TimesheetBlockDetail { get; set; }
    }
}
