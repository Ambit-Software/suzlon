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
    
    public partial class SAPAdvancePayment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SAPAdvancePayment()
        {
            this.PaymentDetailAdvances = new HashSet<PaymentDetailAdvance>();
            this.PaymentDetailAdvanceTxns = new HashSet<PaymentDetailAdvanceTxn>();
        }
    
        public int SAPAdvancePaymentId { get; set; }
        public string CompanyCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public Nullable<int> Natureofrequest { get; set; }
        public string DPRNumber { get; set; }
        public string Reference { get; set; }
        public Nullable<int> FiscalYear { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountProposed { get; set; }
        public string Currency { get; set; }
        public Nullable<System.DateTime> ExpectedClearingDate { get; set; }
        public string BusinessArea { get; set; }
        public string ProfitCentre { get; set; }
        public string PurchasingDocument { get; set; }
        public Nullable<int> POLineItemNo { get; set; }
        public string POItemText { get; set; }
        public string SpecialGL { get; set; }
        public string WithholdingTaxCode { get; set; }
        public Nullable<decimal> UnsettledOpenAdvance { get; set; }
        public string JustificationforAdvPayment { get; set; }
        public Nullable<System.DateTime> Documentdate { get; set; }
        public string HouseBank { get; set; }
        public string PaymentMethod { get; set; }
        public Nullable<int> ChequeLotNumber { get; set; }
        public string HeaderText { get; set; }
        public string PaymentDocumentNumber { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsWIP { get; set; }
        public string DCFLag { get; set; }
        public Nullable<decimal> TaxRate { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }
        public Nullable<int> ReverseReasonId { get; set; }
        public Nullable<System.DateTime> ReverseDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAdvance> PaymentDetailAdvances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDetailAdvanceTxn> PaymentDetailAdvanceTxns { get; set; }
    }
}
