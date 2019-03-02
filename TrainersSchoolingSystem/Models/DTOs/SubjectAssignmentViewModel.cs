using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class SubjectAssignmentViewModel
    {
        [Key]
        public int SubjectAssignmentId { get; set; }
        public string Description { get; set; }
        [Required]
        public Nullable<int> Teacher { get; set; }
        [Required]
        public Nullable<int> Subject { get; set; }
        [Required]
        public Nullable<int> Class { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public SubjectViewModel Subject_ { get; set; }
        public ClassViewModel Class_ { get; set; }

    }
}