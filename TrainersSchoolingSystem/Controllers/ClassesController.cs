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
using TrainersSchoolingSystem.Utils;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private TrainersEntities db = new TrainersEntities();
        
        // GET: Classes
        public ActionResult Index()
        {
            return View(db.Classes.ToList());
        }
        public ActionResult GetClasses()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var classes = db.Classes.ToList();
                foreach (var class_ in classes)
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = class_.ClassId.ToString();
                    item.Text = class_.ClassName + class_.Section;
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {

                Logger.Fatal(ex.Message);
                Logger.Fatal(ex.Source);
                Logger.Fatal(ex.TargetSite.Name);
                Logger.Fatal(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Logger.Fatal(ex.InnerException.Message);
                    Logger.Fatal(ex.InnerException.Source);
                    Logger.Fatal(ex.InnerException.TargetSite.Name);
                    Logger.Fatal(ex.InnerException.StackTrace);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        // GET: Classes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Classes/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.ClassAdvisor = new SelectList(db.Staffs, "StaffId", "FirstName");
                ViewBag.Section = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Section"), "LookupText", "LookupText");
                ViewBag.Level = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Level"), "LookupText", "LookupText");

            }
            catch (Exception ex)
            {

                Logger.Fatal(ex.Message);
                Logger.Fatal(ex.Source);
                Logger.Fatal(ex.TargetSite.Name);
                Logger.Fatal(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Logger.Fatal(ex.InnerException.Message);
                    Logger.Fatal(ex.InnerException.Source);
                    Logger.Fatal(ex.InnerException.TargetSite.Name);
                    Logger.Fatal(ex.InnerException.StackTrace);
                }
            }
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Class @class)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    @class.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    @class.CreatedDate = DateTime.Now;
                    db.Classes.Add(@class);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ClassAdvisor = new SelectList(db.Staffs, "StaffId", "FirstName");
                ViewBag.Section = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Section"), "LookupText", "LookupText");
                ViewBag.Level = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Level"), "LookupText", "LookupText");

            }
            catch (Exception ex)
            {

                Logger.Fatal(ex.Message);
                Logger.Fatal(ex.Source);
                Logger.Fatal(ex.TargetSite.Name);
                Logger.Fatal(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Logger.Fatal(ex.InnerException.Message);
                    Logger.Fatal(ex.InnerException.Source);
                    Logger.Fatal(ex.InnerException.TargetSite.Name);
                    Logger.Fatal(ex.InnerException.StackTrace);
                }
            }
            return View(@class);
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassAdvisor = new SelectList(db.Staffs, "StaffId", "FirstName", @class.ClassAdvisor);
            ViewBag.Section = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Section"), "LookupText", "LookupText", @class.Section);
            ViewBag.Level = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Level"), "LookupText", "LookupText", @class.Level);

            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Class @class)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbClass = db.Classes.Where(x => x.ClassId == @class.ClassId).FirstOrDefault();
                    dbClass.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    dbClass.UpdatedDate = DateTime.Now;
                    dbClass.ClassName = @class.ClassName;
                    dbClass.Section = @class.Section;
                    dbClass.ClassAdvisor = @class.ClassAdvisor;
                    dbClass.Level = @class.Level;
                    db.Classes.AddOrUpdate(dbClass);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ClassAdvisor = new SelectList(db.Staffs, "StaffId", "FirstName", @class.ClassAdvisor);
                ViewBag.Section = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Section"), "LookupText", "LookupText", @class.Section);
                ViewBag.Level = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Level"), "LookupText", "LookupText", @class.Level);

            }
            catch (Exception ex)
            {

                Logger.Fatal(ex.Message);
                Logger.Fatal(ex.Source);
                Logger.Fatal(ex.TargetSite.Name);
                Logger.Fatal(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Logger.Fatal(ex.InnerException.Message);
                    Logger.Fatal(ex.InnerException.Source);
                    Logger.Fatal(ex.InnerException.TargetSite.Name);
                    Logger.Fatal(ex.InnerException.StackTrace);
                }
            }
            return View(@class);
        }

        // GET: Classes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
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
