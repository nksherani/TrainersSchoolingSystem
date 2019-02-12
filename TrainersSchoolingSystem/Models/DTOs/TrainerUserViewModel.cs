using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class TrainerUserViewModel
    {
        [Key]
        public int TrainerUserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^\\+[0-9]{12}$", ErrorMessage = "Enter Mobile No. in proper format like +923331234567")]
        [MaxLength(13, ErrorMessage = "Maximum characters limit exceeded")]
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public string Address { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}