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
    [Authorize]
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
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value");
            ViewBag.Student = new SelectList(db.Students, "StudentId", "FirstName");
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
                enrolment.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                enrolment.CreatedDate = DateTime.Now;
                db.Enrolments.Add(enrolment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value");
            ViewBag.Student = new SelectList(db.Students, "StudentId", "FirstName", enrolment.Student);
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
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value");
            ViewBag.Student = new SelectList(db.Students, "StudentId", "FirstName", enrolment.Student);
            return View(enrolment);
        }

        // POST: Enrolments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Enrolment enrolment)
        {
            if (ModelState.IsValid)
            {
                var enrolmentdb = db.Enrolments.Find(enrolment.EnrolmentId);
                enrolmentdb.RollNo = enrolment.RollNo;
                enrolmentdb.GRNo = enrolment.GRNo;
                enrolmentdb.Class = enrolment.Class;
                enrolmentdb.LastClass = enrolment.LastClass;
                enrolmentdb.LastInstitude = enrolment.LastInstitude;
                enrolmentdb.PaymentMode = enrolment.PaymentMode;
                enrolmentdb.MonthlyFee = enrolment.MonthlyFee;
                enrolmentdb.AdmissionFee = enrolment.AdmissionFee;
                enrolmentdb.AnnualFee = enrolment.AnnualFee;
                enrolmentdb.Student = enrolment.Student;
                enrolmentdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                enrolmentdb.UpdatedDate = DateTime.Now;
                db.Enrolments.AddOrUpdate(enrolmentdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value");
            ViewBag.Student = new SelectList(db.Students, "StudentId", "FirstName", enrolment.Student);
            return View(enrolment);
        }

        // GET: Enrolments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            Enrolment enrolment = student.Enrolments.FirstOrDefault();
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
            Student student = db.Students.Find(id);
            Enrolment enrolment = student.Enrolments.FirstOrDefault();
            enrolment.IsActive = false;
            db.Enrolments.AddOrUpdate(enrolment);
            db.SaveChanges();
            return Redirect("../../Students/Students");
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
