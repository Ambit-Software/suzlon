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
    
    public partial class TreasuryDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TreasuryDetail()
        {
            this.TreasuryComments = new HashSet<TreasuryComment>();
            this.TreasuryBudgetUtilisations = new HashSet<TreasuryBudgetUtilisation>();
            this.PaymentAllocationDetails = new HashSet<PaymentAllocationDetail>();
        }
    
        public int TreasuryDetailId { get; set; }
        public System.DateTime VendorCreatedOn { get; set; }
        public System.DateTime ProposedDate { get; set; }
        public string CompanyCode { get; set; }
        public Nullable<int> VerticalId { get; set; }
        public Nullable<int> SubVerticalId { get; set; }
        public string AllocationNumber { get; set; }
        public string RequestType { get; set; }
        public Nullable<decimal> RequestedAmount { get; set; }
        public Nullable<decimal> InitApprovedAmount { get; set; }
        public Nullable<decimal> AddendumTotal { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public Nullable<System.DateTime> UtilsationStartDate { get; set; }
        public Nullable<System.DateTime> UtilsationEndDate { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public string Status { get; set; }
        public int WorkflowStatusId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreasuryComment> TreasuryComments { get; set; }
        public virtual TreasuryWorkflowStatu TreasuryWorkflowStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TreasuryBudgetUtilisation> TreasuryBudgetUtilisations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentAllocationDetail> PaymentAllocationDetails { get; set; }
    }
}