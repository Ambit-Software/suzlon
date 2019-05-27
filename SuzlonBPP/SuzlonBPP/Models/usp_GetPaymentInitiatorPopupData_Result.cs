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
    
    public partial class usp_GetPaymentInitiatorPopupData_Result
    {
        public string CompanyCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string DPRNumber { get; set; }
        public string Reference { get; set; }
        public string FiscalYear { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Currency { get; set; }
        public string DueDays { get; set; }
        public Nullable<System.DateTime> BaseLineDate { get; set; }
        public string BusinessArea { get; set; }
        public string ProfitCentre { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string DCFLag { get; set; }
    }
}
