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
    
    public partial class usp_GetBankDetialsReportData_Result
    {
        public string CompanyCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string VendorEmailID1 { get; set; }
        public string VendorEmailID2 { get; set; }
        public string SuzlonEmailID1 { get; set; }
        public string SuzlonEmailID2 { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNumber { get; set; }
        public int RequestId { get; set; }
        public System.DateTime RequestedDate { get; set; }
        public Nullable<System.DateTime> RequestAccepted { get; set; }
        public string Status { get; set; }
    }
}
