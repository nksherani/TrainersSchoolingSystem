using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class BulkStudents
    {
        public string Action { get; set; }
        public string[] Ids { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Fee { get; set; }
    }
}