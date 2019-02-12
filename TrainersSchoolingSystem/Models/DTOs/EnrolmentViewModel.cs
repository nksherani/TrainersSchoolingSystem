using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public partial class EnrolmentViewModel
    {
        [Key]
        public int EnrolmentId { get; set; }
        [Required]
        [Display(Name = "G.R No.")]
        public string GRNo { get; set; }
        [Required]
        [Display(Name = "Roll No.")]
        public Nullable<int> RollNo { get; set; }
        [Required]
        public Nullable<int> Class { get; set; }
        [Display(Name = "Last Class")]
        public string LastClass { get; set; }
        [Display(Name = "Last Institude")]
        public string LastInstitude { get; set; }
        [Required]
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }
        [Required]
        [RegularExpression("^\\d+$",ErrorMessage ="Fee must be a number")]
        public string Fee { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        [Display(Name = "Class")]
        public virtual ClassViewModel Class_ { get; set; }

        public TrainerUserViewModel CreatedBy_ { get; set; }
        public TrainerUserViewModel UpdatedBy_ { get; set; }
    }
}