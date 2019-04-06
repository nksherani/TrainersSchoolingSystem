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
        public decimal AdmissionFee { get; set; }
        [Display(Name = "Annual Fee")]
        public decimal AnnualFee { get; set; }
        [Display(Name = "Lab Fee")]
        public decimal LabCharges { get; set; }
        [Display(Name = "Late Payment Surcharge")]
        public decimal LatePaymentSurcharge { get; set; }
    }
}