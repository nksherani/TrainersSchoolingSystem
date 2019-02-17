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
    [Authorize(Roles = "Admin")]
    public class StaffsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();
        
        // GET: Staffs
        public ActionResult Index()
        {
            var staffs = db.Staffs.Include(s => s.TrainerUser).Include(s => s.TrainerUser1);
            return View(staffs.ToList());
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

        // GET: Staffs/Create
        public ActionResult Create()
        {
            ViewBag.Designation = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Designation"), "LookupText", "LookupText");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");

            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffId,FirstName,LastName,Designation,Gender,DateOfBirth,Age,FatherName,SpouseName,Mobile,LandLine,PostalCode,StreetAddress,City,JoiningDate,EndDate,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                staff.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                staff.CreatedDate = DateTime.Now;
                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Designation = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Designation"), "LookupText", "LookupText");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");
            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", staff.CreatedBy);
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", staff.UpdatedBy);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Designation = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Designation"), "LookupText", "LookupText");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");

            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", staff.CreatedBy);
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", staff.UpdatedBy);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffId,FirstName,LastName,Designation,Gender,DateOfBirth,Age,FatherName,SpouseName,Mobile,LandLine,PostalCode,StreetAddress,City,JoiningDate,EndDate,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                var staffdb = db.Staffs.Find(staff.StaffId);
                staffdb.FirstName = staff.FirstName;
                staffdb.LastName = staff.LastName;
                staffdb.Designation = staff.Designation;
                staffdb.Gender = staff.Gender;
                staffdb.DateOfBirth = staff.DateOfBirth;
                if (staffdb.DateOfBirth.HasValue)
                    staffdb.Age = DateTime.Now.Year - staffdb.DateOfBirth.Value.Year;
                staffdb.FatherName = staff.FatherName;
                staffdb.SpouseName = staff.SpouseName;
                staffdb.Mobile = staff.Mobile;
                staffdb.LandLine = staff.LandLine;
                staffdb.PostalCode = staff.PostalCode;
                staffdb.StreetAddress = staff.StreetAddress;
                staffdb.City = staff.City;
                staffdb.JoiningDate = staff.JoiningDate;
                staffdb.EndDate = staff.EndDate;
                staffdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                staffdb.UpdatedDate = DateTime.Now;
                db.Staffs.AddOrUpdate(staffdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Designation = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Designation"), "LookupText", "LookupText");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");

            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", staff.CreatedBy);
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", staff.UpdatedBy);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
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
