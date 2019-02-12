using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public partial class EnrolmentViewModel
    {
        public int EnrolmentId { get; set; }
        public Nullable<int> Student { get; set; }
        public Nullable<int> Class { get; set; }
        public string LastClass { get; set; }
        public string LastInstitude { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public virtual ClassViewModel Class_ { get; set; }

        public TrainerUserViewModel CreatedBy_ { get; set; }
        public TrainerUserViewModel UpdatedBy_ { get; set; }
    }
}