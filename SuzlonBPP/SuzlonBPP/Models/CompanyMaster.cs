//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuzlonBPP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CompanyMaster
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
