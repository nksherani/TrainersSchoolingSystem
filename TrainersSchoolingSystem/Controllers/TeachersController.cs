using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;

namespace TrainersSchoolingSystem.Controllers
{
    public class TeachersController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: Teachers
        public ActionResult Index()
        {
            var teachers = db.Teachers.Include(t => t.TrainerUser).Include(t => t.TrainerUser1);
            return View(teachers.ToList());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherId,FirstName,LastName,Gender,DateOfBirth,Age,FatherName,MotherName,GuardianName,Mobile,LandLine,PostalCode,StreetAddress,City,JoiningDate,EndDate,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                teacher.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                teacher.CreatedDate = DateTime.Now;
                if (teacher.DateOfBirth.HasValue)
                    teacher.Age = (DateTime.Now.Year - teacher.DateOfBirth.Value.Year);

                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", teacher.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", teacher.UpdatedBy);
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", teacher.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", teacher.UpdatedBy);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherId,FirstName,LastName,Gender,DateOfBirth,Age,FatherName,MotherName,GuardianName,Mobile,LandLine,PostalCode,StreetAddress,City,JoiningDate,EndDate,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                var dbTeacher = db.Teachers.Where(x => x.TeacherId == teacher.TeacherId).FirstOrDefault();
                dbTeacher.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                dbTeacher.UpdatedDate = DateTime.Now;
                dbTeacher.FirstName = teacher.FirstName;
                dbTeacher.LastName = teacher.LastName;
                dbTeacher.Gender = teacher.Gender;
                dbTeacher.DateOfBirth = teacher.DateOfBirth;
                if (teacher.DateOfBirth.HasValue)
                    dbTeacher.Age = (DateTime.Now.Year - teacher.DateOfBirth.Value.Year);
                dbTeacher.FatherName = teacher.FatherName;
                dbTeacher.MotherName = teacher.MotherName;
                dbTeacher.GuardianName = teacher.GuardianName;
                dbTeacher.Mobile = teacher.Mobile;
                dbTeacher.LandLine = teacher.LandLine;
                dbTeacher.PostalCode = teacher.PostalCode;
                dbTeacher.StreetAddress = teacher.StreetAddress;
                dbTeacher.City = teacher.City;
                dbTeacher.JoiningDate = teacher.JoiningDate;
                dbTeacher.EndDate = teacher.EndDate;
                dbTeacher.UpdatedDate = DateTime.Now;
                dbTeacher.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                db.Teachers.AddOrUpdate(dbTeacher);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", teacher.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", teacher.UpdatedBy);
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
