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
    
    public partial class ReportParameter
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string ParameterName { get; set; }
        public bool IsEnabled { get; set; }
    
        public virtual Report Report { get; set; }
    }
}
