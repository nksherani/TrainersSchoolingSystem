using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class ConfigurationViewModel
    {
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string Campus { get; set; }

        [Required]
        public string Admin { get; set; }
        public string Head { get; set; }
        public string MobileNumber { get; set; }
        public string Landline { get; set; }
        public string Address { get; set; }

        public string Description { get; set; }
        public string Picture { get; set; }
        public string FirstMonth { get; set; }

        [Display(Name ="Logo")]
        public HttpPostedFileBase File { get; set; }

        public ConfigurationViewModel()
        {
            SchoolName = "School Name not defined";
            Campus = "Campus Name not defined";
            Admin = "";
            Head = "";
            MobileNumber = "Mobile Number not defined";
            Landline = "Landline Number not defined";
            Address = "School Address not defined";
            Description = "";
            Picture = "../Content/Images/Logo.png";
            FirstMonth = "4";
        }
    }
}