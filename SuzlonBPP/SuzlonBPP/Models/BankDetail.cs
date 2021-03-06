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
    
    public partial class BankDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BankDetail()
        {
            this.BankComments = new HashSet<BankComment>();
        }
    
        public int BankDetailId { get; set; }
        public string VendorCode { get; set; }
        public string CompanyCode { get; set; }
        public string VendorName { get; set; }
        public string VendorPanNo { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string City { get; set; }
        public string SuzlonEmailID1 { get; set; }
        public string SuzlonEmailID2 { get; set; }
        public string VendorEmailID1 { get; set; }
        public string VendorEmailID2 { get; set; }
        public Nullable<int> WorkFlowStatusId { get; set; }
        public Nullable<bool> OriginalDocumentsSent { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public Nullable<bool> OriginalDocumentsReceived { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public Nullable<int> VerticalId { get; set; }
        public Nullable<int> SubVerticalId { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public Nullable<int> AssignedTo { get; set; }
        public bool IsNew { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankComment> BankComments { get; set; }
        public virtual BankWorkflowStatu BankWorkflowStatu { get; set; }
        public virtual SubVerticalMaster SubVerticalMaster { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual VerticalMaster VerticalMaster { get; set; }
    }
}
