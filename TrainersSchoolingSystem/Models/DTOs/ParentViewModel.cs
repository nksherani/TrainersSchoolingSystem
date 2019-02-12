using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    
    public class ParentViewModel
    {
        [Key]
        public int ParentId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CNIC { get; set; }
        [Required]
        public string Profession { get; set; }
        [Display(Name = "Organization Type")]
        public string OrganizationType { get; set; }
        [Required]
        public string Education { get; set; }
        [Required]
        [Display(Name = "Monthly Income")]
        public string MonthlyIncome { get; set; }
        [Required]
        public string Mobile { get; set; }
        public string Landline { get; set; }
        [Required]
        public string Address { get; set; }
        [Display(Name = "Office Phone")]
        public string OfficePhone { get; set; }
        [Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }
        public string Email { get; set; }
        [Required]
        public string Relation { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}