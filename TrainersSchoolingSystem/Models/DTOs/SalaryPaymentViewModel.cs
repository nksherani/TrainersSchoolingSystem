using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class SalaryPaymentViewModel
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
    }
}