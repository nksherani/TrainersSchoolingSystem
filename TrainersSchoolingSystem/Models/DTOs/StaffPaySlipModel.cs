using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class StaffPaySlipModel
    {
        public int ChallanNo { get; set; }
        public string Month { get; set; }
        public DateTime? IssueDate { get; set; }
        public int StaffId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public DateTime? JoiningDate { get; set; }
        public decimal BasicPay { get; set; }
        public decimal Bonus { get; set; }
        public decimal PF { get; set; }
        public decimal EOBI { get; set; }
        public decimal LoanDeduction { get; set; }
        public decimal GrossPay { get; set; }
        public int UnpaidLeaves { get; set; }
        public decimal AmountDeducted { get; set; }
        public decimal NetPay { get; set; }
    }
}