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
    
    public partial class IssueAttachment
    {
        public int IssueAttachementId { get; set; }
        public int IssueId { get; set; }
        public string FilePath { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        public virtual IssueManagement IssueManagement { get; set; }
    }
}
