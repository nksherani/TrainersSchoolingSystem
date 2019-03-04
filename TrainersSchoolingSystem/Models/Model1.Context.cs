﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TrainersEntities : DbContext
    {
        public TrainersEntities()
            : base("name=TrainersEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Arrear> Arrears { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<DailyAttendance> DailyAttendances { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Enrolment> Enrolments { get; set; }
        public virtual DbSet<Fee> Fees { get; set; }
        public virtual DbSet<Lookup> Lookups { get; set; }
        public virtual DbSet<LookupType> LookupTypes { get; set; }
        public virtual DbSet<PaidFee> PaidFees { get; set; }
        public virtual DbSet<Parent> Parents { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<SalaryPayment> SalaryPayments { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<StaffAttendance> StaffAttendances { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectAssignment> SubjectAssignments { get; set; }
        public virtual DbSet<TrainerUser> TrainerUsers { get; set; }
    
        public virtual int ResetDb()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ResetDb");
        }
    
        public virtual ObjectResult<GenerateFeeSlips_Result> GenerateFeeSlips(Nullable<int> studentId)
        {
            var studentIdParameter = studentId.HasValue ?
                new ObjectParameter("StudentId", studentId) :
                new ObjectParameter("StudentId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GenerateFeeSlips_Result>("GenerateFeeSlips", studentIdParameter);
        }
    
        public virtual ObjectResult<GetUnpaidStudents_Result> GetUnpaidStudents()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetUnpaidStudents_Result>("GetUnpaidStudents");
        }
    
        public virtual ObjectResult<GeneratePaySlips_Result> GeneratePaySlips(Nullable<int> teacherId)
        {
            var teacherIdParameter = teacherId.HasValue ?
                new ObjectParameter("TeacherId", teacherId) :
                new ObjectParameter("TeacherId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GeneratePaySlips_Result>("GeneratePaySlips", teacherIdParameter);
        }
    
        public virtual ObjectResult<GetCurrentTeachers_Result> GetCurrentTeachers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCurrentTeachers_Result>("GetCurrentTeachers");
        }
    }
}
