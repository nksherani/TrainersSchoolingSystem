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
    public class LookupTypesController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: LookupTypes
        public ActionResult Index()
        {
            var lookupTypes = db.LookupTypes.Include(l => l.TrainerUser).Include(l => l.TrainerUser1);
            return View(lookupTypes.ToList());
        }

        // GET: LookupTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LookupType lookupType = db.LookupTypes.Find(id);
            if (lookupType == null)
            {
                return HttpNotFound();
            }
            return View(lookupType);
        }

        // GET: LookupTypes/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            return View();
        }

        // POST: LookupTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LookupTypeId,LookupTypeName,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] LookupType lookupType)
        {
            if (ModelState.IsValid)
            {
                lookupType.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                lookupType.CreatedDate = DateTime.Now;
                db.LookupTypes.Add(lookupType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookupType.CreatedBy);
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookupType.UpdatedBy);
            return View(lookupType);
        }

        // GET: LookupTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LookupType lookupType = db.LookupTypes.Find(id);
            if (lookupType == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookupType.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookupType.UpdatedBy);
            return View(lookupType);
        }

        // POST: LookupTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LookupTypeId,LookupTypeName,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] LookupType lookupType)
        {
            if (ModelState.IsValid)
            {
                var lookup = db.LookupTypes.Find(lookupType.LookupTypeId);
                lookup.LookupTypeName = lookupType.LookupTypeName;
                db.LookupTypes.AddOrUpdate(lookup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookupType.CreatedBy);
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", lookupType.UpdatedBy);
            return View(lookupType);
        }

        // GET: LookupTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LookupType lookupType = db.LookupTypes.Find(id);
            if (lookupType == null)
            {
                return HttpNotFound();
            }
            return View(lookupType);
        }

        // POST: LookupTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LookupType lookupType = db.LookupTypes.Find(id);
            db.LookupTypes.Remove(lookupType);
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
