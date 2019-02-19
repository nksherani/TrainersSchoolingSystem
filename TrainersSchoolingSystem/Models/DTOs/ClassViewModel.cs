using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class ClassViewModel
    {
        [Key]
        public int ClassId { get; set; }
        [Required]
        [Display(Name = "Class")]
        public string ClassName { get; set; }
        [Required]
        public string Section { get; set; }
        [Required]
        [Display(Name = "Class Advisor")]
        public Nullable<int> ClassAdvisor { get; set; }
        public Nullable<int> Level { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public StaffViewModel ClassAdvisor_ { get; set; }

        public TrainerUserViewModel CreatedBy_ { get; set; }
        public TrainerUserViewModel UpdatedBy_ { get; set; }
    }
}