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
    public class SubjectAssignmentsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: SubjectAssignments
        public ActionResult Index()
        {
            var subjectAssignments = db.SubjectAssignments.Include(s => s.Class1).Include(s => s.Staff).Include(s => s.Subject1).Include(s => s.TrainerUser).Include(s => s.TrainerUser1);
            return View(subjectAssignments.ToList());
        }

        // GET: SubjectAssignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectAssignment subjectAssignment = db.SubjectAssignments.Find(id);
            if (subjectAssignment == null)
            {
                return HttpNotFound();
            }
            return View(subjectAssignment);
        }

        // GET: SubjectAssignments/Create
        public ActionResult Create()
        {
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value");
            ViewBag.Teacher = new SelectList(db.Staffs, "StaffId", "FirstName");
            ViewBag.Subject = new SelectList(db.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        // POST: SubjectAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubjectAssignmentId,Description,Teacher,Subject,Class,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] SubjectAssignment subjectAssignment)
        {
            if (ModelState.IsValid)
            {
                subjectAssignment.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                subjectAssignment.CreatedDate = DateTime.Now;
                db.SubjectAssignments.Add(subjectAssignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value", subjectAssignment.Class);
            ViewBag.Teacher = new SelectList(db.Staffs, "StaffId", "FirstName", subjectAssignment.Teacher);
            ViewBag.Subject = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectAssignment.Subject);
            return View(subjectAssignment);
        }

        // GET: SubjectAssignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectAssignment subjectAssignment = db.SubjectAssignments.Find(id);
            if (subjectAssignment == null)
            {
                return HttpNotFound();
            }
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName+item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value", subjectAssignment.Class);
            ViewBag.Teacher = new SelectList(db.Staffs, "StaffId", "FirstName", subjectAssignment.Teacher);
            ViewBag.Subject = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectAssignment.Subject);
            return View(subjectAssignment);
        }

        // POST: SubjectAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubjectAssignmentId,Description,Teacher,Subject,Class,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] SubjectAssignment subjectAssignment)
        {
            if (ModelState.IsValid)
            {
                var subjectAssignmentdb = db.SubjectAssignments.Find(subjectAssignment.SubjectAssignmentId);
                subjectAssignmentdb.Class = subjectAssignment.Class;
                subjectAssignmentdb.Teacher = subjectAssignment.Teacher;
                subjectAssignmentdb.Subject = subjectAssignment.Subject;
                subjectAssignmentdb.Description = subjectAssignment.Description;
                subjectAssignmentdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                subjectAssignmentdb.UpdatedDate = DateTime.Now;
                db.SubjectAssignments.AddOrUpdate(subjectAssignmentdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value", subjectAssignment.Class);
            ViewBag.Teacher = new SelectList(db.Staffs, "StaffId", "FirstName", subjectAssignment.Teacher);
            ViewBag.Subject = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectAssignment.Subject);
            return View(subjectAssignment);
        }

        // GET: SubjectAssignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectAssignment subjectAssignment = db.SubjectAssignments.Find(id);
            if (subjectAssignment == null)
            {
                return HttpNotFound();
            }
            return View(subjectAssignment);
        }

        // POST: SubjectAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectAssignment subjectAssignment = db.SubjectAssignments.Find(id);
            db.SubjectAssignments.Remove(subjectAssignment);
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
