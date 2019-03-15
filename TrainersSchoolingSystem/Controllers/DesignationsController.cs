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
    public class DesignationsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: Designations
        public ActionResult Index()
        {
            var designations = db.Designations.Include(d => d.TrainerUser).Include(d => d.TrainerUser1);
            return View(designations.ToList());
        }

        // GET: Designations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation designation = db.Designations.Find(id);
            if (designation == null)
            {
                return HttpNotFound();
            }
            return View(designation);
        }

        // GET: Designations/Create
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Category"), "LookupText", "LookupText");
            return View();
        }

        // POST: Designations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Designation designation)
        {
            if (ModelState.IsValid)
            {
                designation.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                designation.CreatedDate = DateTime.Now;
                db.Designations.Add(designation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Category"), "LookupText", "LookupText");
            return View(designation);
        }

        // GET: Designations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation designation = db.Designations.Find(id);
            if (designation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Category").ToList(), "LookupText", "LookupText", designation.Category);
            return View(designation);
        }

        // POST: Designations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Designation designation)
        {
            if (ModelState.IsValid)
            {
                var designationdb = db.Designations.Find(designation.DesignationId);
                designationdb.DesignationName = designation.DesignationName;
                designationdb.Category = designation.Category;
                designationdb.LateComingScale = designation.LateComingScale;
                designationdb.PaidLeaves = designation.PaidLeaves;
                designationdb.ShortLeavesScale = designation.ShortLeavesScale;
                designationdb.LateComingScale = designation.LateComingScale;
                designationdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                designationdb.UpdatedDate = DateTime.Now;
                db.Designations.AddOrUpdate(designationdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Category"), "LookupText", "LookupText", designation.Category);
            return View(designation);
        }

        // GET: Designations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation designation = db.Designations.Find(id);
            if (designation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Category"), "LookupText", "LookupText", designation.Category);
            return View(designation);
        }

        // POST: Designations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Designation designation = db.Designations.Find(id);
            db.Designations.Remove(designation);
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
