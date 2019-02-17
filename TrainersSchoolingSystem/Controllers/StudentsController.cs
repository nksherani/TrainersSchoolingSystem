using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
using TrainersSchoolingSystem.Utils;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    [Authorize]
    public class StudentsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        public StudentsController()
        {
            if (!SiteMapManager.SiteMaps.ContainsKey("TMXMAP"))
            {
                SiteMapManager.SiteMaps.Register<XmlSiteMap>("TMXMAP", sitmap => sitmap.LoadFrom("~/Content/TMX.sitemap"));
            }
        }
        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Parent).Include(s => s.Parent1).Include(s => s.Parent2).Include(s => s.TrainerUser).Include(s => s.TrainerUser1);
            return View(students.ToList());
        }
        public ActionResult Students()
        {
            return View();
        }
        public ActionResult GetStudents()
        {
            var students = db.Students.Include(s => s.Parent).Include(s => s.Parent1).Include(s => s.Parent2).Include(s => s.TrainerUser).Include(s => s.TrainerUser1);
            var enrolments = db.Enrolments.ToList();
            List<StudentViewModel> modellist = new List<StudentViewModel>();
            foreach (var student in students)
            {
                StudentViewModel model = new StudentViewModel();
                model = Mapper<StudentViewModel>.GetObject(student);
                model.Father_ = Mapper<ParentViewModel>.GetObject(student.Parent);
                model.Mother_ = Mapper<ParentViewModel>.GetObject(student.Parent2);
                if(student.Parent1!=null)
                {
                    model.Guardian_ = Mapper<GuardianViewModel>.GetObject(student.Parent1);
                }
                var enrolmentdb = enrolments.Where(x => x.Student == student.StudentId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                model.Enrolment = Mapper<EnrolmentViewModel>.GetObject(enrolmentdb);
                model.Enrolment.Class_ = Mapper<ClassViewModel>.GetObject(enrolmentdb.Class1);
                modellist.Add(model);
            }
            //request.Filters.Add(new FilterDescriptor() { Member = "Enrolment.Class_.ClassName", MemberType = typeof(string), Operator = FilterOperator.IsEqualTo/*, Value = "Chai"*/ });
            return Json(modellist,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name");
            ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name");
            ViewBag.Mother = new SelectList(db.Parents, "ParentId", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                student.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                student.CreatedDate = DateTime.Now;
                //var std = Mapper.Map<StudentViewModel, Student>(student);
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name", student.Father);
            ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name", student.Guardian);
            ViewBag.Mother = new SelectList(db.Parents, "ParentId", "Name", student.Mother);
            return View(student);
        }
        // GET: Students/Create
        //[Authorize(Roles ="Admin,Moderator")]
        public ActionResult NewAdmission()
        {
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.Class = new SelectList(classes, "Key", "Value");
            ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name");
            ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name");
            ViewBag.Mother = new SelectList(db.Parents, "ParentId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAdmission(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                student.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                student.CreatedDate = DateTime.Now;
                var std = Mapper<Student>.GetObject(student);
                if (std.DateOfBirth.HasValue)
                    std.Age = DateTime.Now.Year - std.DateOfBirth.Value.Year;
                db.Students.Add(std);
                db.SaveChanges();
                var father = Mapper<Parent>.GetObject(student.Father_);
                var mother = Mapper<Parent>.GetObject(student.Mother_);
                father.Relation = "Father";
                mother.Relation = "Mother";
                db.Parents.Add(father);
                db.SaveChanges();
                std.Father = father.ParentId;
                db.Parents.Add(mother);
                db.SaveChanges();
                std.Mother = mother.ParentId;

                if (student.Guardian_.Name != null)
                {
                    var guardian = Mapper<Parent>.GetObject(student.Guardian_);
                    db.Parents.Add(guardian);
                    db.SaveChanges();
                    std.Guardian = guardian.ParentId;
                }
                var enrolment = Mapper<Enrolment>.GetObject(student.Enrolment);
                enrolment.Student = std.StudentId;
                db.Enrolments.Add(enrolment);
                db.Students.AddOrUpdate(std);
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
            ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name", student.Father);
            ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name", student.Guardian);
            ViewBag.Mother = new SelectList(db.Parents, "ParentId", "Name", student.Mother);
            return View(student);
        }
        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name", student.Father);
            ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name", student.Guardian);
            ViewBag.Mother = new SelectList(db.Parents, "ParentId", "Name", student.Mother);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                var studentdb = db.Students.Find(student.StudentId);
                studentdb.FirstName = student.FirstName;
                studentdb.LastName = student.LastName;
                studentdb.DateOfBirth = student.DateOfBirth;
                studentdb.Age = DateTime.Now.Year - studentdb.DateOfBirth.Value.Year;
                studentdb.PlaceOfBirth = student.PlaceOfBirth;
                studentdb.Religion = student.Religion;
                studentdb.Nationality = student.Nationality;
                studentdb.MotherTongue = student.MotherTongue;
                studentdb.BloodGroup = student.BloodGroup;
                studentdb.BFormNo = student.BFormNo;
                studentdb.AdmissionBasis = student.AdmissionBasis;
                studentdb.Father = student.Father;
                studentdb.Mother = student.Mother;
                studentdb.Guardian = student.Guardian;
                studentdb.Mobile = student.Mobile;
                studentdb.LandLine = student.LandLine;
                studentdb.PostalCode = student.PostalCode;
                studentdb.StreetAddress = student.StreetAddress;
                studentdb.City = student.City;
                studentdb.JoiningDate = student.JoiningDate;
                studentdb.EndDate = student.EndDate;
                studentdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                studentdb.UpdatedDate = DateTime.Now;
                db.Students.AddOrUpdate(studentdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name", student.Father);
            ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name", student.Guardian);
            ViewBag.Mother = new SelectList(db.Parents, "ParentId", "Name", student.Mother);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
