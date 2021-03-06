//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolarPMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SAPMASTER_BK_15July
    {
        public int SAPId { get; set; }
        public string State { get; set; }
        public string SAPSite { get; set; }
        public Nullable<decimal> TotalLand { get; set; }
        public string TotalLandUoM { get; set; }
        public string SAPProjectId { get; set; }
        public string ProjectDescription { get; set; }
        public string WBSelement { get; set; }
        public string WBSDescription { get; set; }
        public string SAPNetwork { get; set; }
        public string NetworkDescription { get; set; }
        public string SAPActivity { get; set; }
        public string ActivityDescription { get; set; }
        public string SAPSubActivity { get; set; }
        public string SAPSubActivityDescription { get; set; }
        public System.DateTime ActivityPlanStartDate { get; set; }
        public System.DateTime ActivityPlanFinishDate { get; set; }
        public Nullable<decimal> ActivityQty { get; set; }
        public string ActivityPlanQtyUoM { get; set; }
        public Nullable<System.DateTime> ActivityActualStartDate { get; set; }
        public Nullable<System.DateTime> ActivityActualFinishDate { get; set; }
        public Nullable<decimal> ActivityActualQty { get; set; }
        public string ActivityActualQtyUoM { get; set; }
        public string ActualCompletionText { get; set; }
        public Nullable<decimal> WorkCentre { get; set; }
        public string ActivityCompletionStatus { get; set; }
        public string Plant { get; set; }
        public string CompanyCode { get; set; }
        public string BusinessArea { get; set; }
        public string ProfitCentre { get; set; }
        public string MobileFunction { get; set; }
        public string WBSArea { get; set; }
        public string ActivityElementof { get; set; }
        public string BlockTable { get; set; }
        public string ActivityUniqueNo { get; set; }
        public Nullable<int> WBSAreaId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
