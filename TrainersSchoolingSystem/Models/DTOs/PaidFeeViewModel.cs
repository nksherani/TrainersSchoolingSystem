using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class PaidFeeViewModel
    {
        public int PaidFeeId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> CalculatedAmount { get; set; }
        public Nullable<decimal> ReceivedAmount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}