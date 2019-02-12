﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class SubjectAssignmentViewModel
    {
        public int SubjectAssignmentId { get; set; }
        public string Description { get; set; }
        public Nullable<int> Teacher { get; set; }
        public Nullable<int> Subject { get; set; }
        public Nullable<int> Class { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public StaffViewModel Teacher_ { get; set; }
        public SubjectViewModel Subject_ { get; set; }
        public ClassViewModel Class_ { get; set; }

        public TrainerUserViewModel CreatedBy_ { get; set; }
        public TrainerUserViewModel UpdatedBy_ { get; set; }
    }
}