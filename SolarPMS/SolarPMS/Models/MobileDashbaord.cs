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
    
    public partial class MobileDashbaord
    {
        public string ActivityDescription { get; set; }
        public string ActivityPlanQtyUoM { get; set; }
        public Nullable<int> Total { get; set; }
        public Nullable<int> Plan { get; set; }
        public Nullable<int> Actual { get; set; }
        public Nullable<decimal> CR { get; set; }
        public Nullable<decimal> RR { get; set; }
        public Nullable<int> AEDelay { get; set; }
    }
}
