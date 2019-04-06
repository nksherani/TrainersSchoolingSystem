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
using TrainersSchoolingSystem.Utils;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize]
    public class StaffAttendancesController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: StaffAttendances
        public ActionResult Index()
        {
            
            var staffAttendances = db.StaffAttendances.Include(s => s.Staff);
            return View(staffAttendances.ToList());
        }

        // GET: StaffAttendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffAttendance staffAttendance = db.StaffAttendances.Find(id);
            if (staffAttendance == null)
            {
                return HttpNotFound();
            }
            return View(staffAttendance);
        }

        // GET: StaffAttendances/Create
        public ActionResult Create()
        {
            ViewBag.Month = new SelectList(Constants.months, "Key", "Value");
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName");
            return View();
        }

        // POST: StaffAttendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffAttendance staffAttendance)
        {
            if (ModelState.IsValid)
            {
                staffAttendance.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                staffAttendance.CreatedDate = DateTime.Now;
                db.StaffAttendances.Add(staffAttendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Month = new SelectList(Constants.months, "Key", "Value");

            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName", staffAttendance.StaffId);
            return View(staffAttendance);
        }

        // GET: StaffAttendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffAttendance staffAttendance = db.StaffAttendances.Find(id);
            if (staffAttendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.Month = new SelectList(Constants.months, "Key", "Value", staffAttendance.Month);

            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName", staffAttendance.StaffId);
            return View(staffAttendance);
        }

        // POST: StaffAttendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaffAttendance staffAttendance)
        {
            if (ModelState.IsValid)
            {
                var staffAttendancedb = db.StaffAttendances.Find(staffAttendance.StaffAttendanceId);
                staffAttendancedb.Absents = staffAttendance.Absents;
                staffAttendancedb.LateComings = staffAttendance.LateComings;
                staffAttendancedb.Month = staffAttendance.Month;
                staffAttendancedb.ShortLeaves = staffAttendance.ShortLeaves;
                staffAttendancedb.StaffId = staffAttendance.StaffId;
                staffAttendancedb.WorkingDays = staffAttendance.WorkingDays;
                staffAttendancedb.Year = staffAttendance.Year;
                staffAttendancedb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                staffAttendancedb.UpdatedDate = DateTime.Now;
                db.StaffAttendances.AddOrUpdate(staffAttendancedb); db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Month = new SelectList(Constants.months, "Key", "Value", staffAttendance.Month);
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName", staffAttendance.StaffId);
            return View(staffAttendance);
        }

        // GET: StaffAttendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffAttendance staffAttendance = db.StaffAttendances.Find(id);
            if (staffAttendance == null)
            {
                return HttpNotFound();
            }
            return View(staffAttendance);
        }

        // POST: StaffAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffAttendance staffAttendance = db.StaffAttendances.Find(id);
            db.StaffAttendances.Remove(staffAttendance);
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
