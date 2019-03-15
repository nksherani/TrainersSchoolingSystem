using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;

namespace TrainersSchoolingSystem.Controllers
{
    public class MigrationController : Controller
    {
        TrainersEntities db = new TrainersEntities();
        // GET: Migration
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProcessData(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                string directory = Server.MapPath("~/App_Data/uploads");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(directory, fileName);
                file.SaveAs(path);
                ProcessDesignationsData(path);
            }
            ViewBag.Message = "Data Uploaded Successfully";
            return RedirectToAction("Index");
        }
        private bool ProcessDesignationsData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
            var designationdb = db.Designations.ToList();
            var userid = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            for (int i = workSheet.Dimension.Start.Row;
                     i <= workSheet.Dimension.End.Row;
                     i++)
            {
                string cellValue = workSheet.Cells[i, 1].Text;
                int temp = 0;
                if (int.TryParse(cellValue, out temp))
                {
                    //list.Add(workSheet.Cells[i, 1, i, workSheet.Dimension.End.Column]);
                    Designation des = new Designation();
                    des.DesignationName = workSheet.Cells[i, 2].Text;
                    if (designationdb.Where(x => x.DesignationName == des.DesignationName).Count() > 0)
                        des = designationdb.Where(x => x.DesignationName == des.DesignationName).FirstOrDefault();
                    des.Category = workSheet.Cells[i, 3].Text;
                    des.PaidLeaves = Convert.ToInt32(workSheet.Cells[i, 4].Text == "" ? "0" : workSheet.Cells[i, 4].Text);
                    des.ShortLeavesScale = Convert.ToInt32(workSheet.Cells[i, 5].Text==""? "0": workSheet.Cells[i, 5].Text);
                    des.LateComingScale = Convert.ToInt32(workSheet.Cells[i, 6].Text == "" ? "0" : workSheet.Cells[i, 6].Text);
                    des.CreatedDate = DateTime.Now;
                    des.CreatedBy = userid;
                    db.Designations.AddOrUpdate(des);

                }

            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessTeachersData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));

            ExcelWorksheet workSheet = package.Workbook.Worksheets[2];
            List<ExcelRange> list = new List<ExcelRange>();
            for (int i = workSheet.Dimension.Start.Row;
                     i <= workSheet.Dimension.End.Row;
                     i++)
            {
                string cellValue = workSheet.Cells[i, 1].Text;
                int temp = 0;
                if (int.TryParse(cellValue, out temp))
                {
                    list.Add(workSheet.Cells[i, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column]);
                }
            }
            return true;
        }
        private bool ProcessClassesData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));

            ExcelWorksheet workSheet = package.Workbook.Worksheets[3];
            List<ExcelRange> list = new List<ExcelRange>();
            for (int i = workSheet.Dimension.Start.Row;
                     i <= workSheet.Dimension.End.Row;
                     i++)
            {
                string cellValue = workSheet.Cells[i, 1].Text;
                int temp = 0;
                if (int.TryParse(cellValue, out temp))
                {
                    list.Add(workSheet.Cells[i, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column]);
                }
            }
            return true;
        }
        private bool ProcessSubjectsData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));

            ExcelWorksheet workSheet = package.Workbook.Worksheets[4];
            List<ExcelRange> list = new List<ExcelRange>();
            for (int i = workSheet.Dimension.Start.Row;
                     i <= workSheet.Dimension.End.Row;
                     i++)
            {
                string cellValue = workSheet.Cells[i, 1].Text;
                int temp = 0;
                if (int.TryParse(cellValue, out temp))
                {
                    list.Add(workSheet.Cells[i, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column]);
                }
            }
            return true;
        }

        private bool ProcessStudentsData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));

            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

            for (int i = workSheet.Dimension.Start.Row;
                     i <= workSheet.Dimension.End.Row;
                     i++)
            {
                object cellValue = workSheet.Cells[i, 0].Value;
            }
            return true;
        }
        // GET: Migration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Migration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Migration/Create
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

        // GET: Migration/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Migration/Edit/5
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

        // GET: Migration/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Migration/Delete/5
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
