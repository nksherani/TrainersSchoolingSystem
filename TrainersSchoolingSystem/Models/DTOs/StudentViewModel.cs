using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class StudentViewModel
    {
        [Key]
        public int StudentId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Family Code")]
        public string FamilyCode { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]
        public Nullable<DateTime> DateOfBirth { get; set; }
        public Nullable<int> Age { get; set; }
        [Required]
        [Display(Name = "Birth Place")]
        public string PlaceOfBirth { get; set; }
        [Required]
        public string Religion { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        [Display(Name = "Mother Tongue")]
        public string MotherTongue { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Required]
        [Display(Name = "Form B No.")]
        public string BFormNo { get; set; }
        
        [Required]
        [Display(Name = "Admission Based on")]
        public string AdmissionBasis { get; set; }
        public Nullable<int> Father { get; set; }
        public Nullable<int> Mother { get; set; }
        public Nullable<int> Guardian { get; set; }
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
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "Admission Date")]
        public Nullable<DateTime> JoiningDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public EnrolmentViewModel Enrolment { get; set; }
        [Display(Name = "Father")]
        public ParentViewModel Father_ { get; set; }
        [Display(Name = "Mother")]
        public ParentViewModel Mother_ { get; set; }
        [Display(Name = "Guardian")]
        public GuardianViewModel Guardian_ { get; set; }
        public TrainerUserViewModel CreatedBy_ { get; set; }
        public TrainerUserViewModel UpdatedBy_ { get; set; }

    }
}