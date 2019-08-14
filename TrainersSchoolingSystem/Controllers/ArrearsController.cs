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
    public class ArrearsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: Arrears
        public ActionResult Index()
        {
            var arrears = db.Arrears.Include(a => a.TrainerUser).Include(a => a.Staff).Include(a => a.Student).Include(a => a.TrainerUser1);
            return View(arrears.ToList());
        }

        // GET: Arrears/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arrear arrear = db.Arrears.Find(id);
            if (arrear == null)
            {
                return HttpNotFound();
            }
            return View(arrear);
        }
        class StudentModel
        {
            public int StudentId { get; set; }
            public string Name { get; set; }
            public StudentModel(int id, string name)
            {
                StudentId = id;
                Name = name;
            }
            public StudentModel()
            {

            }
        }
        // GET: Arrears/Create
        public ActionResult Create()
        {
            var stds = db.Students.ToList();
            List<StudentModel> stdlist = new List<StudentModel>();
            foreach (var item in stds)
            {
                StudentModel std = new StudentModel(item.StudentId, item.Enrolments.FirstOrDefault().GRNo + " - " + item.FirstName + " "+item.LastName);
                stdlist.Add(std);
            }
            ViewBag.StudentId = new SelectList(stdlist, "StudentId", "Name");
            return View();
        }

        // POST: Arrears/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Arrear arrear)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Arrears.Where(x => x.StudentId == arrear.StudentId).FirstOrDefault();
                if(existing==null)
                {
                    arrear.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    arrear.CreatedDate = DateTime.Now;
                    arrear.ArrearType = "Receivable";
                    db.Arrears.Add(arrear);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    existing.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    existing.UpdatedDate = DateTime.Now;
                    existing.ArrearType = "Receivable";
                    existing.Amount += arrear.Amount;
                    db.Arrears.AddOrUpdate(existing);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", arrear.StudentId);
            return View(arrear);
        }

        // GET: Arrears/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arrear arrear = db.Arrears.Find(id);
            if (arrear == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", arrear.StudentId);
            return View(arrear);
        }

        // POST: Arrears/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Arrear arrear)
        {
            if (ModelState.IsValid)
            {
                var arrdb = db.Arrears.Find(arrear.ArrearId);
                arrdb.Amount = arrear.Amount;
                arrdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                arrdb.UpdatedDate = DateTime.Now;
                db.Arrears.AddOrUpdate(arrdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", arrear.StudentId);
            return View(arrear);
        }

        // GET: Arrears/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arrear arrear = db.Arrears.Find(id);
            if (arrear == null)
            {
                return HttpNotFound();
            }
            return View(arrear);
        }

        // POST: Arrears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Arrear arrear = db.Arrears.Find(id);
            db.Arrears.Remove(arrear);
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
