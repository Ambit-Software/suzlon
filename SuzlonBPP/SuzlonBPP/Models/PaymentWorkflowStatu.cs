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
    
    public partial class PaymentWorkflowStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentWorkflowStatu()
        {
            this.PaymentDetailAdvances = new HashSet<PaymentDetailAdvance>();
            this.PaymentDetailAdvanceTxns = new HashSet<PaymentDetailAdvanceTxn>();
            this.PaymentDetailAgainstBills = new HashSet<PaymentDetailAgainstBill>();
            this.PaymentDetailAgainstBillTxns = new HashSet<PaymentDetailAgainstBillTxn>();
            this.PaymentDetailAgainstBillTxns1 = new HashSet<PaymentDetailAgainstBillTxn>();
        }
    
        public int PaymentWorkflowStatusId { get; set; }
        public string Status { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> ProfileId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAdvance> PaymentDetailAdvances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAdvanceTxn> PaymentDetailAdvanceTxns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAgainstBill> PaymentDetailAgainstBills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAgainstBillTxn> PaymentDetailAgainstBillTxns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAgainstBillTxn> PaymentDetailAgainstBillTxns1 { get; set; }
        public virtual ProfileMaster ProfileMaster { get; set; }
        public virtual StatusMaster StatusMaster { get; set; }
    }
}
