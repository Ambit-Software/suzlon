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
    
    public partial class GetNextPaymentWorkFlowId_Result
    {
        public Nullable<decimal> TodaysApprovedPayment { get; set; }
        public Nullable<bool> IsLimitCrossed { get; set; }
        public Nullable<int> NextPaymentWorkFlowId { get; set; }
    }
}