using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;

namespace TrainersSchoolingSystem.Controllers
{
    public class EnrolmentsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: Enrolments
        public ActionResult Index()
        {
            var enrolments = db.Enrolments.Include(e => e.Class1).Include(e => e.TrainerUser).Include(e => e.Student1).Include(e => e.TrainerUser1);
            return View(enrolments.ToList());
        }

        // GET: Enrolments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrolment enrolment = db.Enrolments.Find(id);
            if (enrolment == null)
            {
                return HttpNotFound();
            }
            return View(enrolment);
        }

        // GET: Enrolments/Create
        public ActionResult Create()
        {
            ViewBag.Class = new SelectList(db.Classes, "ClassId", "ClassName");
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            ViewBag.Student = new SelectList(db.Students, "StudentId", "FirstName");
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            return View();
        }

        // POST: Enrolments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrolmentId,Student,Class,LastClass,LastInstitude,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Enrolment enrolment)
        {
            if (ModelState.IsValid)
            {
                db.Enrolments.Add(enrolment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Class = new SelectList(db.Classes, "ClassId", "ClassName", enrolment.Class);
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", enrolment.CreatedBy);
            ViewBag.Student = new SelectList(db.Students, "StudentId", "FirstName", enrolment.Student);
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", enrolment.UpdatedBy);
            return View(enrolment);
        }

        // GET: Enrolments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrolment enrolment = db.Enrolments.Find(id);
            if (enrolment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Class = new SelectList(db.Classes, "ClassId", "ClassName", enrolment.Class);
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", enrolment.CreatedBy);
            ViewBag.Student = new SelectList(db.Students, "StudentId", "FirstName", enrolment.Student);
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", enrolment.UpdatedBy);
            return View(enrolment);
        }

        // POST: Enrolments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrolmentId,Student,Class,LastClass,LastInstitude,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Enrolment enrolment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrolment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Class = new SelectList(db.Classes, "ClassId", "ClassName", enrolment.Class);
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", enrolment.CreatedBy);
            ViewBag.Student = new SelectList(db.Students, "StudentId", "FirstName", enrolment.Student);
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", enrolment.UpdatedBy);
            return View(enrolment);
        }

        // GET: Enrolments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrolment enrolment = db.Enrolments.Find(id);
            if (enrolment == null)
            {
                return HttpNotFound();
            }
            return View(enrolment);
        }

        // POST: Enrolments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrolment enrolment = db.Enrolments.Find(id);
            db.Enrolments.Remove(enrolment);
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
