using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Models.DTOs
{
    public class FeeSetup
    {
        [Display(Name = "Admission Fee")]
        public int AdmissionFee { get; set; }
        [Display(Name = "Annual Fee")]
        public int AnnualFee { get; set; }
        [Display(Name = "Lab Fee")]
        public int LabCharges { get; set; }
        [Display(Name = "Late Payment Surcharge")]
        public int LatePaymentSurcharge { get; set; }
    }
}