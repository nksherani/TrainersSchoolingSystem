using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Models.DTOs;

namespace TrainersSchoolingSystem.Controllers
{
    public class ReportsController : Controller
    {
        TrainersEntities db = new TrainersEntities();
        // GET: Reports
        public ActionResult GetMonthlyFeeCollection()
        {
            var report = db.Database.SqlQuery<ReportModel>("MonthlyFeeCollection").ToList();

            return Json(SortByMonth(report), JsonRequestBehavior.AllowGet);
        }
        List<ReportModel> SortByMonth(List<ReportModel> report)
        {
            List<ReportModel> sorted = new List<ReportModel>();
            if (report.Where(x => x.X.StartsWith("Jan")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Jan")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Feb")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Feb")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Mar")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Mar")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Apr")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Apr")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("May")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("May")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Jun")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Jun")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Jul")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Jul")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Aug")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Aug")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Sep")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Sep")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Oct")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Oct")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Nov")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Nov")).FirstOrDefault());
            if (report.Where(x => x.X.StartsWith("Dec")).Count() > 0)
                sorted.Add(report.Where(x => x.X.StartsWith("Dec")).FirstOrDefault());
            return sorted;
        }
        // GET: Reports/Details/5
        [HttpGet]
        public ActionResult GetMonthlySalaryPayments()
        {
            var report = db.Database.SqlQuery<ReportModel>("MonthlySalaryPayments").ToList();
            return Json(SortByMonth(report), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetClasswiseCount()
        {
            var report = db.Database.SqlQuery<ClasswiseCountModel>("ClasswiseCount").ToList();
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reports/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reports/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
