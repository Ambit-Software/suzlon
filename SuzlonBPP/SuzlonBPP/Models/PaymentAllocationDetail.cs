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
    
    public partial class PaymentAllocationDetail
    {
        public int PaymentAllocationDetailsId { get; set; }
        public Nullable<int> SAPPaymentID { get; set; }
        public string SAPPaymentTYPE { get; set; }
        public Nullable<int> TreasuryDetailId { get; set; }
        public Nullable<decimal> AllocatedAmount { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual TreasuryDetail TreasuryDetail { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}