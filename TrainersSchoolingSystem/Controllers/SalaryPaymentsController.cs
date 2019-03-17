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
    public class SalaryPaymentsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: SalaryPayments
        public ActionResult Index()
        {
            var salaryPayments = db.SalaryPayments.Include(s => s.TrainerUser).Include(s => s.Staff).Include(s => s.TrainerUser1);
            return View(salaryPayments.ToList());
        }

        // GET: SalaryPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalaryPayment salaryPayment = db.SalaryPayments.Find(id);
            if (salaryPayment == null)
            {
                return HttpNotFound();
            }
            return View(salaryPayment);
        }

        // GET: SalaryPayments/Create
        public ActionResult Create()
        {
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName");
            ViewBag.Month = new SelectList(Constants.months, "Key", "Value");
            return View();
        }

        // POST: SalaryPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( SalaryPayment salaryPayment)
        {
            if (ModelState.IsValid)
            {
                if (!salaryPayment.PaymentDate.HasValue)
                    salaryPayment.PaymentDate = DateTime.Now;
                salaryPayment.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                salaryPayment.CreatedDate = DateTime.Now;
                db.SalaryPayments.Add(salaryPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Month = new SelectList(Constants.months, "Key", "Value");
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName", salaryPayment.StaffId);
            return View(salaryPayment);
        }

        // GET: SalaryPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalaryPayment salaryPayment = db.SalaryPayments.Find(id);
            if (salaryPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName", salaryPayment.StaffId);
            ViewBag.Month = new SelectList(Constants.months, "Key", "Value", salaryPayment.Month);
            return View(salaryPayment);
        }

        // POST: SalaryPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( SalaryPayment salaryPayment)
        {
            if (ModelState.IsValid)
            {
                if (!salaryPayment.PaymentDate.HasValue)
                    salaryPayment.PaymentDate = DateTime.Now;
                var salaryPaymentdb = db.SalaryPayments.Find(salaryPayment.SalaryPaymentId);
                salaryPaymentdb.Amount = salaryPayment.Amount;
                salaryPaymentdb.PaymentDate = salaryPayment.PaymentDate;

                salaryPaymentdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                salaryPaymentdb.UpdatedDate = DateTime.Now;
                db.SalaryPayments.AddOrUpdate(salaryPaymentdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName", salaryPayment.StaffId);
            ViewBag.Month = new SelectList(Constants.months, "Key", "Value", salaryPayment.Month);
            return View(salaryPayment);
        }

        // GET: SalaryPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalaryPayment salaryPayment = db.SalaryPayments.Find(id);
            if (salaryPayment == null)
            {
                return HttpNotFound();
            }
            return View(salaryPayment);
        }

        // POST: SalaryPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalaryPayment salaryPayment = db.SalaryPayments.Find(id);
            db.SalaryPayments.Remove(salaryPayment);
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
