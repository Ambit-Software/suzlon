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
    
    public partial class usp_GetTableMasterDataForOfflineSync_Result
    {
        public int TableId { get; set; }
        public string Site { get; set; }
        public string ProjectId { get; set; }
        public string ProjectDescription { get; set; }
        public int Block { get; set; }
        public int Invertor { get; set; }
        public int SCB { get; set; }
        public int Table { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    }
}
