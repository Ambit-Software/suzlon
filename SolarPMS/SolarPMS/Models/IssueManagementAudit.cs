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
    
    public partial class IssueManagementAudit
    {
        public int IssueMgmtAuditId { get; set; }
        public int IssueId { get; set; }
        public int AssignedTo { get; set; }
        public System.DateTime AssignedDate { get; set; }
    }
}