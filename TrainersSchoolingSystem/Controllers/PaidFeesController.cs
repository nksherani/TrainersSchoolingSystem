﻿using System;
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
    public class PaidFeesController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: PaidFees
        public ActionResult Index()
        {
            var paidFees = db.PaidFees.Include(p => p.TrainerUser).Include(p => p.Student).Include(p => p.TrainerUser1);
            return View(paidFees.ToList());
        }

        // GET: PaidFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidFee paidFee = db.PaidFees.Find(id);
            if (paidFee == null)
            {
                return HttpNotFound();
            }
            return View(paidFee);
        }

        // GET: PaidFees/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName");
            return View();
        }

        // POST: PaidFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( PaidFee paidFee)
        {
            if (ModelState.IsValid)
            {
                paidFee.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                paidFee.CreatedDate = DateTime.Now;
                db.PaidFees.Add(paidFee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", paidFee.StudentId);
            return View(paidFee);
        }

        // GET: PaidFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidFee paidFee = db.PaidFees.Find(id);
            if (paidFee == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", paidFee.StudentId);
            return View(paidFee);
        }

        // POST: PaidFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( PaidFee paidFee)
        {
            if (ModelState.IsValid)
            {
                var paidfeedb = db.PaidFees.Find(paidFee.PaidFeeId);
                paidfeedb.PaidFeeId = paidFee.PaidFeeId;
                paidfeedb.PaymentDate = paidFee.PaymentDate;
                paidfeedb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                paidfeedb.UpdatedDate = DateTime.Now;
                db.PaidFees.AddOrUpdate(paidfeedb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", paidFee.StudentId);
            return View(paidFee);
        }

        // GET: PaidFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidFee paidFee = db.PaidFees.Find(id);
            if (paidFee == null)
            {
                return HttpNotFound();
            }
            return View(paidFee);
        }

        // POST: PaidFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaidFee paidFee = db.PaidFees.Find(id);
            db.PaidFees.Remove(paidFee);
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
