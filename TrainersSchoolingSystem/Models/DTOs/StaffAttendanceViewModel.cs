using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class StaffAttendanceViewModel
    {
        [Key]
        public int StaffAttendanceId { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<int> WorkingDays { get; set; }
        public Nullable<int> Absents { get; set; }
        public Nullable<int> ShortLeaves { get; set; }
        public Nullable<int> LateComings { get; set; }
        public Nullable<int> Month { get; set; }
        public string Year { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}