using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class DesignationViewModel
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public Nullable<int> PaidLeaves { get; set; }
        public Nullable<int> ShortLeavesScale { get; set; }
        public Nullable<int> LateComingScale { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public virtual TrainerUser TrainerUser { get; set; }
        public virtual TrainerUser TrainerUser1 { get; set; }
    }
}