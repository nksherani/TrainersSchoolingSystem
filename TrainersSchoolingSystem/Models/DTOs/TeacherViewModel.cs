using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class TeacherViewModel
    {
        [Key]
        public int StaffId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public Nullable<int> Designation { get; set; }
        
        [Required]
        public string Gender { get; set; }
        [Required]
        public Nullable<DateTime> DateOfBirth { get; set; }
        public Nullable<int> Age { get; set; }
        [Required]
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        [Required]
        [RegularExpression("^\\+[0-9]{12}$", ErrorMessage = "Enter Mobile No. in proper format like +923331234567")]
        [MaxLength(13, ErrorMessage = "Maximum characters limit exceeded")]
        public string Mobile { get; set; }
        [RegularExpression("(^\\+[0-9]{11}$)|(^\\+[0-9]{12}$)", ErrorMessage = "Enter Phone No. in proper format like +92211234567 or +922112345678")]
        [MaxLength(13, ErrorMessage = "Maximum characters limit exceeded")]
        [Display(Name = "Land Line")]
        public string LandLine { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        [Required]
        [Display(Name = "Appointed on")]
        public Nullable<DateTime> JoiningDate { get; set; }
        [Display(Name = "Leaving Date")]
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        [Required]
        [Display(Name = "Salary")]
        public SalaryViewModel Salary_ { get; set; }
    }
}