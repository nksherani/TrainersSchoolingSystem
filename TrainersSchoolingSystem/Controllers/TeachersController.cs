﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Models.DTOs;
using TrainersSchoolingSystem.Utils;
using System.Net;

namespace TrainersSchoolingSystem.Controllers
{
    public class TeachersController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: Teachers
        public ActionResult Index()
        {
            var designationIds = db.Designations.Where(x => x.Category == "Teaching").Select(x => x.DesignationId).ToList();
            var staffs = db.Staffs.Where(x => designationIds.Contains(x.Designation.Value)).Include(s => s.TrainerUser).Include(s => s.TrainerUser1);
            return View(staffs.ToList());
        }
        public ActionResult GetTeachers()
        {
            var teachers = db.Staffs.Join(db.Designations.Where(x => x.Category == "Teaching"), a => a.Designation, b => b.DesignationId, (a, b) => a);
            var enrolments = db.Enrolments.Where(x => x.IsActive.Value).ToList();
            List<TeacherViewModel> modellist = new List<TeacherViewModel>();
            foreach (var teacher in teachers)
            {
                TeacherViewModel model = new TeacherViewModel();
                model = Mapper<TeacherViewModel>.GetObject(teacher);
                model.Designation_ = Mapper<DesignationViewModel>.GetObject(teacher.Designation1);
                if (teacher.Salaries.Count > 0)
                    model.Salary_ = Mapper<SalaryViewModel>.GetObject(teacher.Salaries.FirstOrDefault());
                if (teacher.StaffAttendances.Count > 0)
                    model.Attendance = Mapper<StaffAttendanceViewModel>.GetObject(teacher.StaffAttendances.FirstOrDefault());
                if (teacher.SalaryPayments.Count > 0)
                {
                    var payments = teacher.SalaryPayments.ToList();
                    foreach (var payment in payments)
                    {
                        model.salaryPayment.Add(Mapper<SalaryPaymentViewModel>.GetObject(payment));
                    }
                }
                if (teacher.SubjectAssignments.Count > 0)
                {
                    var subjects = teacher.SubjectAssignments.ToList();
                    foreach (var subject in subjects)
                    {
                        SubjectAssignmentViewModel assignment = Mapper<SubjectAssignmentViewModel>.GetObject(subject);
                        assignment.Class_ = Mapper<ClassViewModel>.GetObject(subject.Class1);
                        assignment.Subject_ = Mapper<SubjectViewModel>.GetObject(subject.Subject1);
                        model.subjects.Add(assignment);
                    }
                }

                modellist.Add(model);
            }
            return Json(modellist, JsonRequestBehavior.AllowGet);
        }
        // GET: Staffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Teachers/Create
        public ActionResult Appoint()
        {
            ViewBag.Designation = new SelectList(db.Designations.Where(x => x.Category == "Teaching").ToList(), "DesignationId", "DesignationName");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        public ActionResult Appoint(TeacherViewModel teacher)
        {
            if (ModelState.IsValid)
            {
                teacher.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                teacher.CreatedDate = DateTime.Now;

                Staff staff = Mapper<Staff>.GetObject(teacher);
                db.Staffs.Add(staff);
                db.SaveChanges();

                Salary salary = Mapper<Salary>.GetObject(teacher.Salary_);
                salary = SalariesController.SalaryCalculation(salary);
                salary.StaffId = staff.StaffId;
                salary.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                salary.CreatedDate = DateTime.Now;
                db.Salaries.Add(salary);

                StaffAttendance attendance = new StaffAttendance();
                attendance.StaffId = staff.StaffId;
                attendance.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                attendance.CreatedDate = DateTime.Now;
                db.StaffAttendances.Add(attendance);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.Designation = new SelectList(db.Designations.Where(x => x.Category == "Teaching").ToList(), "DesignationId", "DesignationName");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult UpdateData(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            TeacherViewModel teacher = Mapper<TeacherViewModel>.GetObject(staff);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.Designation = new SelectList(db.Designations.Where(x => x.Category == "Teaching").ToList(), "DesignationId", "DesignationName", teacher.Designation);
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", teacher.Gender);
            return View();
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        public ActionResult UpdateData(TeacherViewModel teacher)
        {
            if (ModelState.IsValid)
            {
                var staffdb = db.Staffs.Find(teacher.StaffId);
                staffdb.FirstName = teacher.FirstName;
                staffdb.LastName = teacher.LastName;
                staffdb.Designation = teacher.Designation;
                staffdb.Gender = teacher.Gender;
                staffdb.DateOfBirth = teacher.DateOfBirth;
                if (staffdb.DateOfBirth.HasValue)
                    staffdb.Age = DateTime.Now.Year - staffdb.DateOfBirth.Value.Year;
                staffdb.FatherName = teacher.FatherName;
                staffdb.SpouseName = teacher.SpouseName;
                staffdb.Mobile = teacher.Mobile;
                staffdb.LandLine = teacher.LandLine;
                staffdb.PostalCode = teacher.PostalCode;
                staffdb.StreetAddress = teacher.StreetAddress;
                staffdb.City = teacher.City;
                staffdb.JoiningDate = teacher.JoiningDate;
                staffdb.EndDate = teacher.EndDate;
                staffdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                staffdb.UpdatedDate = DateTime.Now;
                db.Staffs.AddOrUpdate(staffdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Designation = new SelectList(db.Designations.Where(x => x.Category == "Teaching").ToList(), "DesignationId", "DesignationName", teacher.Designation);
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", teacher.Gender);
            return View();

        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Teachers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
