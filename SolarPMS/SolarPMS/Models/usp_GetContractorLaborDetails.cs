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
    
    public partial class usp_GetContractorLaborDetails
    {
        public int Id { get; set; }
        public string Site { get; set; }
        public string Project { get; set; }
        public int AreaId { get; set; }
        public string Network { get; set; }
        public int ContractorId { get; set; }
        public string Shift { get; set; }
        public int LabourCount { get; set; }
        public System.DateTime Date { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string ContractorName { get; set; }
        public string UserName { get; set; }
        public int CanDelete { get; set; }
        public int CanEdit { get; set; }
    }
}
