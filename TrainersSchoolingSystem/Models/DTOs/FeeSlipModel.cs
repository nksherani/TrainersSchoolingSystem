using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class FeeSlipModel
    {
        [Key]
        public int ChallanNo { get; set; }
        public int StudentId { get; set; }
        [Required]
        [Display(Name = "G.R No.")]
        public string GRNo { get; set; }
        [Required]
        [Display(Name = "Roll No.")]
        public Nullable<int> RollNo { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string Class { get; set; }
        public decimal MonthlyFee { get; set; }
        public decimal AnnualFee { get; set; }
        public decimal ArrearAmount { get; set; }
        public string ArrearType { get; set; }
        public decimal UnpaidAmount { get; set; }
        public decimal AdmissionFee { get; set; }
        public decimal ReceivedAmount { get; set; }
    }
}