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
    
    public partial class VillageMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VillageMaster()
        {
            this.SurveyMasters = new HashSet<SurveyMaster>();
        }
    
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyMaster> SurveyMasters { get; set; }
    }
}