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
    
    public partial class Usp_GetReversedPaymentDone_Result
    {
        public string CompanyCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string LotNo { get; set; }
        public string DocumentClearingNo { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public Nullable<decimal> ApprovedAmount { get; set; }
        public Nullable<int> FiscalYear { get; set; }
        public Nullable<System.DateTime> ReverseDate { get; set; }
        public string ReverseReason { get; set; }
    }
}
