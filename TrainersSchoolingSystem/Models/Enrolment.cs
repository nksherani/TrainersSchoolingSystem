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
    
    public partial class Enrolment
    {
        public int EnrolmentId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Student { get; set; }
        public string GRNo { get; set; }
        public Nullable<int> RollNo { get; set; }
        public Nullable<int> Class { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public string PaymentMode { get; set; }
        public string LastClass { get; set; }
        public string LastInstitude { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    
        public virtual Class Class1 { get; set; }
        public virtual TrainerUser TrainerUser { get; set; }
        public virtual Student Student1 { get; set; }
        public virtual TrainerUser TrainerUser1 { get; set; }
    }
}
