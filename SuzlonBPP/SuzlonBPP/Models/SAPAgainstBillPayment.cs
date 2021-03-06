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
    
    public partial class SAPAgainstBillPayment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SAPAgainstBillPayment()
        {
            this.PaymentDetailAgainstBills = new HashSet<PaymentDetailAgainstBill>();
            this.PaymentDetailAgainstBillTxns = new HashSet<PaymentDetailAgainstBillTxn>();
        }
    
        public int SAPAgainstBillPaymentId { get; set; }
        public string CompanyCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public Nullable<int> NatureofRequestId { get; set; }
        public string DocumentNumber { get; set; }
        public string Reference { get; set; }
        public Nullable<int> FiscalYear { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountProposed { get; set; }
        public string Currency { get; set; }
        public Nullable<int> DueDays { get; set; }
        public Nullable<System.DateTime> BaseLineDate { get; set; }
        public string BusinessArea { get; set; }
        public string ProfitCentre { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public string HouseBank { get; set; }
        public string PaymentMethod { get; set; }
        public Nullable<int> CheckLotNumber { get; set; }
        public string HeaderText { get; set; }
        public string PaymentDocumentNumber { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsWIP { get; set; }
        public string DCFLag { get; set; }
        public Nullable<int> ReverseReasonId { get; set; }
        public Nullable<System.DateTime> ReverseDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAgainstBill> PaymentDetailAgainstBills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAgainstBillTxn> PaymentDetailAgainstBillTxns { get; set; }
    }
}
