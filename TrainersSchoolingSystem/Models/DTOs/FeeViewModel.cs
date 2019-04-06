using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class FeeViewModel
    {
        [Key]
        public int FeeId { get; set; }
        public Nullable<int> Class { get; set; }
        public string FeeType { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Description { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}