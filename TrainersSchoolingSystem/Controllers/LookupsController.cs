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
    public class LookupsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();
        
        // GET: Lookups
        public ActionResult Index()
        {
            var lookups = db.Lookups.Include(l => l.TrainerUser).Include(l => l.LookupType).Include(l => l.TrainerUser1);
            return View(lookups.ToList());
        }

        // GET: Lookups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lookup lookup = db.Lookups.Find(id);
            if (lookup == null)
            {
                return HttpNotFound();
            }
            return View(lookup);
        }

        // GET: Lookups/Create
        public ActionResult Create()
        {
            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            ViewBag.LookupTypeId = new SelectList(db.LookupTypes, "LookupTypeId", "LookupTypeName");
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            return View();
        }

        // POST: Lookups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LookupId,LookupTypeId,LookupText,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Lookup lookup)
        {
            if (ModelState.IsValid)
            {
                lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                lookup.CreatedDate = DateTime.Now;
                db.Lookups.Add(lookup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookup.CreatedBy);
            ViewBag.LookupTypeId = new SelectList(db.LookupTypes, "LookupTypeId", "LookupTypeName", lookup.LookupTypeId);
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookup.UpdatedBy);
            return View(lookup);
        }

        // GET: Lookups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lookup lookup = db.Lookups.Find(id);
            if (lookup == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookup.CreatedBy);
            ViewBag.LookupTypeId = new SelectList(db.LookupTypes, "LookupTypeId", "LookupTypeName", lookup.LookupTypeId);
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookup.UpdatedBy);
            return View(lookup);
        }

        // POST: Lookups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LookupId,LookupTypeId,LookupText,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Lookup lookup)
        {
            if (ModelState.IsValid)
            {
                var lookupDb = db.Lookups.Find(lookup.LookupTypeId);
                lookupDb.LookupText = lookup.LookupText;
                lookupDb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                lookupDb.UpdatedDate = DateTime.Now;
                db.Lookups.AddOrUpdate(lookupDb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookup.CreatedBy);
            ViewBag.LookupTypeId = new SelectList(db.LookupTypes, "LookupTypeId", "LookupTypeName", lookup.LookupTypeId);
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookup.UpdatedBy);
            return View(lookup);
        }

        // GET: Lookups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lookup lookup = db.Lookups.Find(id);
            if (lookup == null)
            {
                return HttpNotFound();
            }
            return View(lookup);
        }

        // POST: Lookups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lookup lookup = db.Lookups.Find(id);
            db.Lookups.Remove(lookup);
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
