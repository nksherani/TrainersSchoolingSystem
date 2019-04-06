using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class SalaryPaymentViewModel
    {
        public int SalaryPaymentId { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
    }
}