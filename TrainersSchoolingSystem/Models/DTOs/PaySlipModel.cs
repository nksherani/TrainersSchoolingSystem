using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class PaySlipModel
    {
        public string Month { get; set; }
        public DateTime IssueDate { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public DateTime JoiningDate { get; set; }
        public int BasicPay { get; set; }
        public int Bonus { get; set; }
        public int PF { get; set; }
        public int EOBI { get; set; }
        public int LoanDeduction { get; set; }
        public int GrossPay { get; set; }
        public int NetPay { get; set; }
    }
}