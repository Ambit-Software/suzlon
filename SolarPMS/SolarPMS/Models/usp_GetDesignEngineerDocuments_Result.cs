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
    
    public partial class usp_GetDesignEngineerDocuments_Result
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Comments { get; set; }
        public int Version { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityId { get; set; }
        public string SAPSite { get; set; }
        public string SAPProject { get; set; }
        public int WBSAreaId { get; set; }
        public string SAPNetwork { get; set; }
        public string SAPActivity { get; set; }
        public string SAPSubActivity { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
