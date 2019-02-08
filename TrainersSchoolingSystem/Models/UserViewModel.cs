using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models
{
    public class UserViewModel
    {
        [Required]
        public int Username { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}