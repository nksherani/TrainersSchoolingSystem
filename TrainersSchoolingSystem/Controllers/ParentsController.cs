using Kendo.Mvc;
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
    public class ParentsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();
        public ParentsController()
        {
            if (!SiteMapManager.SiteMaps.ContainsKey("TMXMAP"))
            {
                SiteMapManager.SiteMaps.Register<XmlSiteMap>("TMXMAP", sitmap => sitmap.LoadFrom("~/Content/TMX.sitemap"));
            }
        }
        // GET: Parents
        public ActionResult Index()
        {
            var parents = db.Parents.Include(p => p.TrainerUser).Include(p => p.TrainerUser1);
            return View(parents.ToList());
        }

        // GET: Parents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParentId,Name,CNIC,Profession,Education,MonthlyIncome,Mobile,Landline,Address,OfficePhone,OfficeAddress,Email,Relation,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                parent.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                parent.CreatedDate = DateTime.Now;
                db.Parents.Add(parent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parent);
        }

        // GET: Parents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ParentId,Name,CNIC,Profession,Education,MonthlyIncome,Mobile,Landline,Address,OfficePhone,OfficeAddress,Email,Relation,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                var parentdb = db.Parents.Find(parent.ParentId);
                parentdb.Name = parent.Name;
                parentdb.CNIC = parent.CNIC;
                parentdb.Profession = parent.Profession;
                parentdb.OrganizationType = parent.OrganizationType;
                parentdb.Education = parent.Education;
                parentdb.MonthlyIncome = parent.MonthlyIncome;
                parentdb.Mobile = parent.Mobile;
                parentdb.Landline = parent.Landline;
                parentdb.Address = parent.Address;
                parentdb.OfficePhone = parent.OfficePhone;
                parentdb.OfficeAddress = parent.OfficeAddress;
                parentdb.Email = parent.Email;
                parentdb.Relation = parent.Relation;
                parentdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                parentdb.UpdatedDate = DateTime.Now;
                db.Parents.AddOrUpdate(parent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parent);
        }

        // GET: Parents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parent parent = db.Parents.Find(id);
            db.Parents.Remove(parent);
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
