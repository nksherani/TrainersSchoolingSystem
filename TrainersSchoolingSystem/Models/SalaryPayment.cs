//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainersSchoolingSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SalaryPayment
    {
        public int SalaryPaymentId { get; set; }
        public Nullable<int> ChallanNo { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    
        public virtual TrainerUser TrainerUser { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual TrainerUser TrainerUser1 { get; set; }
    }
}
