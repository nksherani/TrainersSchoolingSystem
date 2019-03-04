using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class TeacherSP
    {
        [Key]
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Designation { get; set; }
        public Nullable<DateTime> JoiningDate { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
    }
}