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
    public class SalariesController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: Salaries
        public ActionResult Index()
        {
            var salaries = db.Salaries.Include(s => s.TrainerUser).Include(s => s.Staff).Include(s => s.TrainerUser1);
            return View(salaries.ToList());
        }

        // GET: Salaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salary salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }
            return View(salary);
        }

        // GET: Salaries/Create
        public ActionResult Create()
        {
            var tempStaff = db.Salaries.Join(db.Staffs, a => a.StaffId, b => b.StaffId, (a, b) => b.StaffId);
            ViewBag.StaffId = new SelectList(db.Staffs.Where(x=>!tempStaff.Contains(x.StaffId)), "StaffId", "FirstName");
            return View();
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salary salary)
        {
            if (ModelState.IsValid)
            {
                salary = SalaryCalculation(salary);
                salary.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                salary.CreatedDate = DateTime.Now;
                db.Salaries.Add(salary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var tempStaff = db.Salaries.Join(db.Staffs, a => a.StaffId, b => b.StaffId, (a, b) => b.StaffId);
            ViewBag.StaffId = new SelectList(db.Staffs.Where(x => !tempStaff.Contains(x.StaffId)), "StaffId", "FirstName");
            return View(salary);
        }
        public static Salary SalaryCalculation(Salary salary)
        {
            salary.GrossPay = (salary.BasicPay.HasValue ? salary.BasicPay.Value : 0) +
                    (salary.Bonus.HasValue ? salary.Bonus.Value : 0);
            salary.NetPay = (salary.GrossPay.HasValue ? salary.GrossPay.Value : 0) -
                (salary.PF.HasValue ? salary.PF.Value : 0) -
                (salary.EOBI.HasValue ? salary.EOBI.Value : 0) -
                (salary.LoanDeduction.HasValue ? salary.LoanDeduction.Value : 0);

            return salary;
        }
        // GET: Salaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salary salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName", salary.Staff.StaffId);
            return View(salary);
        }

        // POST: Salaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Salary salary)
        {
            if (ModelState.IsValid)
            {
                salary = SalaryCalculation(salary);
                var Salarydb = db.Salaries.Find(salary.SalaryId);
                Salarydb.BasicPay = salary.BasicPay;
                Salarydb.Bonus = salary.Bonus;
                Salarydb.PF = salary.PF;
                Salarydb.EOBI = salary.EOBI;
                Salarydb.LoanDeduction = salary.LoanDeduction;
                Salarydb.GrossPay = salary.GrossPay;
                Salarydb.NetPay = salary.NetPay;
                Salarydb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                Salarydb.UpdatedDate = DateTime.Now;
                db.Salaries.AddOrUpdate(Salarydb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "FirstName",salary.Staff.StaffId);
            return View(salary);
        }

        // GET: Salaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salary salary = db.Salaries.Find(id);
            if (salary == null)
            {
                return HttpNotFound();
            }
            return View(salary);
        }

        // POST: Salaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Salary salary = db.Salaries.Find(id);
            db.Salaries.Remove(salary);
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
