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
        public string Fee { get; set; }
        public int AnnualFee { get; set; }
        public int ArrearAmount { get; set; }
        public string ArrearType { get; set; }
        public int UnpaidAmount { get; set; }
        public int AdmissionFee { get; set; }
        public int ReceivedAmount { get; set; }
    }
}