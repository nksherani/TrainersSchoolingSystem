//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainersSchoolingSystem.Models
{
    using System;
    
    public partial class GenerateFeeSlips_Result
    {
        public int StudentId { get; set; }
        public string GRNo { get; set; }
        public Nullable<int> RollNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Class { get; set; }
        public string Fee { get; set; }
        public Nullable<int> AnnualFee { get; set; }
        public int ArrearAmount { get; set; }
        public string ArrearType { get; set; }
    }
}
