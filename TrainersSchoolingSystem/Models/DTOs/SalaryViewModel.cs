using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class SalaryViewModel
    {
        [Key]
        public int SalaryId { get; set; }
        public Nullable<decimal> BasicPay { get; set; }
        public Nullable<decimal> Bonus { get; set; }
        public Nullable<decimal> PF { get; set; }
        public Nullable<decimal> EOBI { get; set; }
        public Nullable<decimal> LoanDeduction { get; set; }
        public Nullable<decimal> GrossPay { get; set; }
        public Nullable<decimal> NetPay { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public virtual TrainerUserViewModel TrainerUser { get; set; }
        public virtual TrainerUserViewModel TrainerUser1 { get; set; }
    }
}