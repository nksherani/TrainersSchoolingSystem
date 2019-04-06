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
using TrainersSchoolingSystem.Models.DTOs;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize]
    public class FeesController : Controller
    {
        private TrainersEntities db = new TrainersEntities();
        public ActionResult UnpaidFees()
        {
            return View();
        }
        public ActionResult GetUnpaidFees()
        {
            var UnpaidList = db.Database.SqlQuery<FeeSlipModel>("GetUnpaidStudents").ToList();
            return Json(UnpaidList, JsonRequestBehavior.AllowGet);
        }
        //public void PayFee([System.Web.Http.FromUri]string models)
        [HttpPost]
        public ActionResult PayFee(List<FeeSlipModel> models)
        {
            var studentIds = models.Select(x => x.StudentId).ToList();
            var arrears = db.Arrears.Where(x => studentIds.Contains(x.StudentId.Value));
            var paidfees = db.PaidFees.Where(x => studentIds.Contains(x.StudentId.Value) && !x.ReceivedAmount.HasValue);
            var userId = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).
                            FirstOrDefault().TrainerUserId;
            foreach (var fee in models)
            {
                decimal ReceivedAmount = fee.ReceivedAmount;
                if (fee.ReceivedAmount == fee.UnpaidAmount)
                {
                    var temp1 = paidfees.Where(x => x.StudentId == fee.StudentId).ToList();
                    foreach (var item in temp1)
                    {
                        item.ReceivedAmount = item.CalculatedAmount;
                        item.PaymentDate = DateTime.Now;
                        db.PaidFees.AddOrUpdate(item);
                    }
                    var temp2 = arrears.Where(x => x.StudentId == fee.StudentId).ToList();
                    if (temp2.Count > 0)
                    {
                        temp2.FirstOrDefault().Amount = 0;
                        temp2.FirstOrDefault().ArrearType = "Receivable";
                        temp2.FirstOrDefault().UpdatedDate = DateTime.Now;
                        temp2.FirstOrDefault().UpdatedBy = userId;
                        db.Arrears.AddOrUpdate(temp2.FirstOrDefault());
                    }

                }
                else if (fee.ReceivedAmount < fee.UnpaidAmount)
                {
                    var temp1 = paidfees.Where(x => x.StudentId == fee.StudentId).ToList();
                    foreach (var item in temp1)
                    {
                        if (item.CalculatedAmount < fee.ReceivedAmount)
                        {
                            item.ReceivedAmount = item.CalculatedAmount;
                            fee.ReceivedAmount -= item.ReceivedAmount.Value;
                        }
                        else
                        {
                            item.ReceivedAmount = fee.ReceivedAmount;
                            fee.ReceivedAmount = 0;
                        }
                        item.PaymentDate = DateTime.Now;
                        db.PaidFees.AddOrUpdate(item);

                    }
                    var temp2 = arrears.Where(x => x.StudentId == fee.StudentId).ToList();
                    if (temp2.Count > 0)
                    {
                        temp2.FirstOrDefault().Amount = fee.UnpaidAmount-fee.ReceivedAmount;
                        temp2.FirstOrDefault().UpdatedDate = DateTime.Now;
                        temp2.FirstOrDefault().UpdatedBy = userId;
                        temp2.FirstOrDefault().ArrearType = "Receivable";
                        db.Arrears.AddOrUpdate(temp2.FirstOrDefault());
                    }
                    else
                    {
                        Arrear arrear = new Arrear();
                        arrear.StudentId = fee.StudentId;
                        arrear.Amount = fee.UnpaidAmount - ReceivedAmount;
                        arrear.CreatedDate = DateTime.Now;
                        arrear.CreatedBy = userId;
                        arrear.ArrearType = "Receivable";
                        db.Arrears.AddOrUpdate(arrear);
                    }
                }
            }
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // GET: Fees
        public ActionResult Index()
        {
            var fees = db.Fees.Include(f => f.TrainerUser).Include(f => f.Class1).Include(f => f.TrainerUser1);
            return View(fees.ToList());
        }

        // GET: Fees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fee fee = db.Fees.Find(id);
            if (fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }

        // GET: Fees/Create
        public ActionResult Create()
        {
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value");
            return View();
        }

        // POST: Fees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeeId,Student,FeeType,Amount,Description,Year,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Fee fee)
        {
            if (ModelState.IsValid)
            {
                fee.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                fee.CreatedDate = DateTime.Now;
                db.Fees.Add(fee);
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
            return View(fee);
        }

        // GET: Fees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fee fee = db.Fees.Find(id);
            if (fee == null)
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
            return View(fee);
        }

        // POST: Fees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fee fee)
        {
            if (ModelState.IsValid)
            {
                var feedb = db.Fees.Find(fee.FeeId);
                feedb.FeeType = fee.FeeType;
                feedb.Amount = fee.Amount;
                feedb.Description = fee.Description;
                feedb.Year = fee.Year;
                feedb.Class = fee.Class;
                feedb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                feedb.UpdatedDate = DateTime.Now;
                db.Fees.AddOrUpdate(fee);
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
            return View(fee);
        }

        // GET: Fees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fee fee = db.Fees.Find(id);
            if (fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }

        // POST: Fees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fee fee = db.Fees.Find(id);
            db.Fees.Remove(fee);
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
