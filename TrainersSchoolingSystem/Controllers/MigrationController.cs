using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainersSchoolingSystem.Controllers
{
    public class MigrationController : Controller
    {
        // GET: Migration
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }
            ViewBag.Message = "Data Uploaded Successfully";
            return RedirectToAction("Index");
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
