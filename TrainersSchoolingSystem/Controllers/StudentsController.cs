using HiQPdf;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Models.DTOs;
using TrainersSchoolingSystem.Utils;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();


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
            var enrolments = db.Enrolments.Where(x => x.IsActive.Value).ToList();
            List<StudentViewModel> modellist = new List<StudentViewModel>();
            foreach (var student in students)
            {
                StudentViewModel model = new StudentViewModel();
                model = Mapper<StudentViewModel>.GetObject(student);
                model.Father_ = Mapper<ParentViewModel>.GetObject(student.Parent);
                model.Mother_ = Mapper<ParentViewModel>.GetObject(student.Parent2);
                if (student.Parent1 != null)
                {
                    model.Guardian_ = Mapper<GuardianViewModel>.GetObject(student.Parent1);
                }
                var enrolmentdb = enrolments.Where(x => x.Student == student.StudentId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                model.Enrolment = Mapper<EnrolmentViewModel>.GetObject(enrolmentdb);
                model.Enrolment.Class_ = Mapper<ClassViewModel>.GetObject(enrolmentdb.Class1);
                modellist.Add(model);
            }
            return Json(modellist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateFeeSlips2(BulkStudents bulk)
        {
            var stdIds = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();
            var enrolments = db.Enrolments.Where(x => x.IsActive.Value && stdIds.Contains(x.Student.Value)).ToList();
            var students = db.Students.Where(x => stdIds.Contains(x.StudentId)).Include(s => s.Parent).Include(s => s.Parent1).Include(s => s.Parent2).Include(s => s.TrainerUser).Include(s => s.TrainerUser1);
            List<StudentViewModel> modellist = new List<StudentViewModel>();
            foreach (var student in students)
            {
                StudentViewModel model = new StudentViewModel();
                model = Mapper<StudentViewModel>.GetObject(student);
                model.Father_ = Mapper<ParentViewModel>.GetObject(student.Parent);
                model.Mother_ = Mapper<ParentViewModel>.GetObject(student.Parent2);
                if (student.Parent1 != null)
                {
                    model.Guardian_ = Mapper<GuardianViewModel>.GetObject(student.Parent1);
                }
                var enrolmentdb = enrolments.Where(x => x.Student == student.StudentId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                model.Enrolment = Mapper<EnrolmentViewModel>.GetObject(enrolmentdb);
                model.Enrolment.Class_ = Mapper<ClassViewModel>.GetObject(enrolmentdb.Class1);
                modellist.Add(model);
            }
            //modellist.AddRange(modellist);
            //modellist.AddRange(modellist);
            //string feeslips = "";
            string css = "";
            Server.MapPath("~/Content/FeeSlips/");
            var csslines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "FeeSlipHeader.html")).ToList();
            foreach (var item in csslines)
            {
                css += item;
            }
            int i = 0;
            string feeslipsBody = "";
            foreach (var studentView in modellist)
            {
                var html = GetFeeSlip(studentView);
                if (i % 5 == 0)
                    html += "<div style=\"width: 240px; height: 1px; background - color:white\"></div>";

                feeslipsBody += html;
                i++;
            }
            StringBuilder strBody = new StringBuilder();
            strBody.Append("<html>" +
            "<head><title>Fee Slips</title>");

            strBody.Append(css + "</head>");
            strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
                                    feeslipsBody +
                                    "</body></html>");

            var filePath = Server.MapPath("~/Content/Temp/FeeSlips.html");
            System.IO.File.WriteAllText(filePath, strBody.ToString());
            return Json("../Content/Temp/FeeSlips.html", JsonRequestBehavior.AllowGet);
        }

        //public string GenerateFeeSlips(BulkStudents bulk)
        //{
        //    var stdIds = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();
        //    var enrolments = db.Enrolments.Where(x => x.IsActive.Value && stdIds.Contains(x.Student.Value)).ToList();
        //    var students = db.Students.Where(x => stdIds.Contains(x.StudentId)).Include(s => s.Parent).Include(s => s.Parent1).Include(s => s.Parent2).Include(s => s.TrainerUser).Include(s => s.TrainerUser1);
        //    List<StudentViewModel> modellist = new List<StudentViewModel>();
        //    foreach (var student in students)
        //    {
        //        StudentViewModel model = new StudentViewModel();
        //        model = Mapper<StudentViewModel>.GetObject(student);
        //        model.Father_ = Mapper<ParentViewModel>.GetObject(student.Parent);
        //        model.Mother_ = Mapper<ParentViewModel>.GetObject(student.Parent2);
        //        if (student.Parent1 != null)
        //        {
        //            model.Guardian_ = Mapper<GuardianViewModel>.GetObject(student.Parent1);
        //        }
        //        var enrolmentdb = enrolments.Where(x => x.Student == student.StudentId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        //        model.Enrolment = Mapper<EnrolmentViewModel>.GetObject(enrolmentdb);
        //        model.Enrolment.Class_ = Mapper<ClassViewModel>.GetObject(enrolmentdb.Class1);
        //        modellist.Add(model);
        //    }
        //    modellist.AddRange(modellist);
        //    modellist.AddRange(modellist);
        //    string feeslips = "";
        //    Server.MapPath("~/Content/FeeSlips/");
        //    var csslines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "FeeSlipHeader.html")).ToList();
        //    foreach (var item in csslines)
        //    {
        //        feeslips += item;
        //    }
        //    int i = 0;
        //    string feeslipsBody = "";
        //    foreach (var studentView in modellist)
        //    {
        //        var html = GetFeeSlip(studentView);
        //        if (i % 5 == 0)
        //            html += "<div style=\"width: 240px; height: 1px; background - color:white\"></div>";

        //        feeslipsBody += html;
        //        i++;
        //    }
        //    feeslips += feeslipsBody;
        //    var path = Server.MapPath("~/Content/FeeSlips/");
        //    var filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + "_by_" + User.Identity.Name + ".pdf";
        //    if (SavePdf2(filename, path, feeslips))
        //        return "../Content/FeeSlips/" + filename;
        //    return "error";

        //}
        bool SavePdf(string filename, string path, string html)
        {
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();


            //htmlToPdfConverter.Document.PageSize = new PdfPageSize(400, 400);
            //htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.Document.Margins = new PdfMargins(0);
            byte[] pdfBuffer = null;
            pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(html, "http://localhost:50496");
            System.IO.File.WriteAllBytes(path + filename, pdfBuffer);
            return true;
        }
        bool SavePdf2(string filename, string path, string html)
        {
            var word = new Microsoft.Office.Interop.Word.Application();
            word.Visible = false;

            //html = html.Replace("Content")
            var filePath = Server.MapPath("~/Content/Temp/Html2PdfTest.html");
            System.IO.File.WriteAllText(filePath, html);
            var savePathPdf = path + filename;
            var wordDoc = word.Documents.Open(FileName: filePath, ReadOnly: false);
            wordDoc.PageSetup.PaperSize = WdPaperSize.wdPaperA4;
            wordDoc.SaveAs2(FileName: savePathPdf, FileFormat: WdSaveFormat.wdFormatPDF);
            wordDoc.Close();
            return true;
        }
        private string GetFeeSlip(StudentViewModel studentView)
        {
            var lastdate = db.PaidFees.Where(x => x.StudentId == studentView.StudentId 
            && x.Description=="MonthlyFee" && x.ReceivedAmount.HasValue)?.OrderByDescending(x => x.CreatedDate)?.FirstOrDefault()?.CreatedDate;
            if (lastdate.HasValue && (DateTime.Now - lastdate).Value.TotalDays < 28)
                return "";
            SqlParameter StudentId = new SqlParameter("@StudentId", studentView.StudentId);
            var FeeSlipData = db.Database.SqlQuery<FeeSlipModel>("exec GenerateFeeSlips @StudentId", StudentId).FirstOrDefault();

            string data = "";
            Server.MapPath("~/Content/FeeSlips/");
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "FeeSlip.html"));

            foreach (var item in lines)
            {
                data += item;
            }
            var totalFee = FeeSlipData.ArrearAmount +
                Convert.ToInt32(FeeSlipData.Fee) + FeeSlipData.AdmissionFee +
                FeeSlipData.AnnualFee + GlobalData.feeSetup.LabCharges;
            data = data.Replace("__Picture__", "../" + GlobalData.configuration.Picture);
            data = data.Replace("__SchoolName__", GlobalData.configuration.SchoolName);
            data = data.Replace("__Campus__", GlobalData.configuration.Campus);
            data = data.Replace("__Month__", DateTime.Now.ToString("MM-yyyy"));
            data = data.Replace("__IssueDate__", DateTime.Now.ToString("dd-MM-yyyy"));
            data = data.Replace("__DueDate__", DateTime.Now.ToString("10-MM-yyyy"));
            data = data.Replace("__Challan__", DateTime.Now.ToString() + studentView.StudentId);
            data = data.Replace("__GRNo__", studentView.Enrolment.GRNo);
            data = data.Replace("__StudentName__", studentView.FirstName + " " + studentView.LastName);
            data = data.Replace("__Class__", FeeSlipData.Class);
            data = data.Replace("__FatherName__", studentView.Father_.Name);
            data = data.Replace("__Arrears__", FeeSlipData.ArrearAmount.ToString());
            data = data.Replace("__TuitionFee__", FeeSlipData.Fee);
            data = data.Replace("__AdmissionFee__", FeeSlipData.AdmissionFee.ToString());
            data = data.Replace("__AnnualFee__", FeeSlipData.AnnualFee.ToString());
            data = data.Replace("__OtherFee__", "0");
            data = data.Replace("__LateFee__", GlobalData.feeSetup.LatePaymentSurcharge.ToString());
            data = data.Replace("__LabCharges__", GlobalData.feeSetup.LabCharges.ToString());
            data = data.Replace("__Till__", totalFee.ToString());
            data = data.Replace("__After__", (totalFee + GlobalData.feeSetup.LatePaymentSurcharge).ToString());

            var feeDb = db.PaidFees.Where(x => !x.ReceivedAmount.HasValue && x.StudentId == studentView.StudentId).ToList();
            List<PaidFee> temp = new List<PaidFee>();
            foreach (var item in feeDb)
            {
                if ((DateTime.Now - item.CreatedDate).Value.Days < 28)
                    temp.Add(item);
            }
            feeDb = temp;
            if (FeeSlipData.Fee != null && FeeSlipData.Fee != "")
            {
                PaidFee paidFee = new PaidFee();
                var count = feeDb.Where(x => x.Description == "MonthlyFee").ToList();
                if (count.Count > 0)
                    paidFee = feeDb.Where(x => x.Description == "MonthlyFee").FirstOrDefault();
                paidFee.StudentId = studentView.StudentId;
                paidFee.CalculatedAmount = Convert.ToInt32(FeeSlipData.Fee);
                paidFee.Description = "MonthlyFee";
                if (paidFee.CreatedDate.HasValue)
                    paidFee.UpdatedDate = DateTime.Now;
                else
                    paidFee.CreatedDate = DateTime.Now;
                if (paidFee.CreatedBy.HasValue)
                    paidFee.UpdatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                else
                    paidFee.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                db.PaidFees.AddOrUpdate(paidFee);
            }
            if (GlobalData.feeSetup.LabCharges > 0)
            {
                PaidFee paidFee = new PaidFee();
                var count = feeDb.Where(x => x.Description == "LabCharges").Count();
                if ( count> 0)
                    paidFee = feeDb.Where(x => x.Description == "LabCharges").FirstOrDefault();
                paidFee.StudentId = studentView.StudentId;
                paidFee.CalculatedAmount = GlobalData.feeSetup.LabCharges;
                paidFee.Description = "LabCharges";
                if (paidFee.CreatedDate.HasValue)
                    paidFee.UpdatedDate = DateTime.Now;
                else
                    paidFee.CreatedDate = DateTime.Now;
                if (paidFee.CreatedBy.HasValue)
                    paidFee.UpdatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                else
                    paidFee.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                db.PaidFees.AddOrUpdate(paidFee);
            }
            if (FeeSlipData.AnnualFee > 0)
            {
                PaidFee paidFee = new PaidFee();
                if (feeDb.Where(x => x.Description == "AnnualFee").Count() > 0)
                    paidFee = feeDb.Where(x => x.Description == "AnnualFee").FirstOrDefault();
                paidFee.StudentId = studentView.StudentId;
                paidFee.CalculatedAmount = FeeSlipData.AnnualFee;
                paidFee.Description = "AnnualFee";
                if (paidFee.CreatedDate.HasValue)
                    paidFee.UpdatedDate = DateTime.Now;
                else
                    paidFee.CreatedDate = DateTime.Now;
                if (paidFee.CreatedBy.HasValue)
                    paidFee.UpdatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                else
                    paidFee.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                db.PaidFees.AddOrUpdate(paidFee);
            }
            db.SaveChanges();
            return data;
        }
        [HttpPost]
        public ActionResult Bulk(BulkStudents bulk)
        {
            string path = "";
            bool flag = false;
            if (bulk.Action == "1")
                flag = PromoteToUpperClass(bulk);
            else if (bulk.Action == "2")
                flag = ChangeClass(bulk);
            else if (bulk.Action == "3")
                flag = ChangeSection(bulk);
            else if (bulk.Action == "4")
                flag = IncreaseFee(bulk);
            //else if (bulk.Action == "5")
            //    path = GenerateFeeSlips(bulk);
            if (path != "")
                return Json(path, JsonRequestBehavior.AllowGet);

            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        bool PromoteToUpperClass(BulkStudents bulk)
        {
            var IDs = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();
            var enrolments = db.Enrolments.Where(x => x.IsActive.Value).Where(x => IDs.Contains(x.Student.Value));
            var classes = db.Classes.ToList();
            List<Enrolment> newList = new List<Enrolment>();
            foreach (var enrolment in enrolments)
            {
                var oldClass = enrolment.Class1;
                var newClass = classes.Where(x => x.Level == oldClass.Level + 1)
                    .Where(x => x.Section == oldClass.Section).FirstOrDefault();
                if ((DateTime.Now - enrolment.CreatedDate).Value.TotalDays > 180)
                {
                    enrolment.IsActive = false;
                    Enrolment newEnr = new Enrolment();
                    newEnr.Fee = enrolment.Fee;
                    newEnr.IsActive = true;
                    newEnr.Student = enrolment.Student;
                    newEnr.GRNo = enrolment.GRNo;
                    newEnr.RollNo = enrolment.RollNo;
                    newEnr.Class = newClass.ClassId;
                    newEnr.PaymentMode = enrolment.PaymentMode;
                    newEnr.LastClass = enrolment.LastClass;
                    newEnr.LastInstitude = enrolment.LastInstitude;
                    newEnr.CreatedDate = DateTime.Now;
                    newEnr.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                    newList.Add(newEnr);
                }
                else
                {
                    enrolment.Class = newClass.ClassId;
                    enrolment.UpdatedDate = DateTime.Now;
                    enrolment.UpdatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                }
                db.Enrolments.AddOrUpdate(enrolment);
            }

            db.Enrolments.AddRange(newList);
            db.SaveChanges();
            return true;
        }
        bool ChangeClass(BulkStudents bulk)
        {
            var IDs = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();
            var enrolments = db.Enrolments.Where(x => x.IsActive.Value).Where(x => IDs.Contains(x.Student.Value));
            var classes = db.Classes.ToList();
            List<Enrolment> newList = new List<Enrolment>();
            foreach (var enrolment in enrolments)
            {
                var newClass = classes.Where(x => x.ClassId == Convert.ToInt32(bulk.Class)).FirstOrDefault();
                if ((DateTime.Now - enrolment.CreatedDate).Value.TotalDays > 180)
                {
                    enrolment.IsActive = false;
                    Enrolment newEnr = new Enrolment();
                    newEnr.Fee = enrolment.Fee;
                    newEnr.IsActive = true;
                    newEnr.Student = enrolment.Student;
                    newEnr.GRNo = enrolment.GRNo;
                    newEnr.RollNo = enrolment.RollNo;
                    newEnr.Class = newClass.ClassId;
                    newEnr.PaymentMode = enrolment.PaymentMode;
                    newEnr.LastClass = enrolment.LastClass;
                    newEnr.LastInstitude = enrolment.LastInstitude;
                    newEnr.CreatedDate = DateTime.Now;
                    newEnr.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                    newList.Add(newEnr);
                }
                else
                {
                    enrolment.Class = newClass.ClassId;
                    enrolment.UpdatedDate = DateTime.Now;
                    enrolment.UpdatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                }
                db.Enrolments.AddOrUpdate(enrolment);
            }

            db.Enrolments.AddRange(newList);
            db.SaveChanges();
            return true;
        }
        bool ChangeSection(BulkStudents bulk)
        {
            var IDs = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();
            var enrolments = db.Enrolments.Where(x => x.IsActive.Value).Where(x => IDs.Contains(x.Student.Value));
            var classes = db.Classes.ToList();
            var sectionId = Convert.ToInt32(bulk.Section);
            var newsection = db.Lookups.Where(x => x.LookupId == sectionId).FirstOrDefault().LookupText;
            List<Enrolment> newList = new List<Enrolment>();
            foreach (var enrolment in enrolments)
            {
                var oldClass = enrolment.Class1;
                var newClass = classes.Where(x => x.ClassName == oldClass.ClassName && x.Section == newsection).FirstOrDefault();
                if ((DateTime.Now - enrolment.CreatedDate).Value.TotalDays > 180)
                {
                    enrolment.IsActive = false;
                    Enrolment newEnr = new Enrolment();
                    newEnr.Fee = enrolment.Fee;
                    newEnr.IsActive = true;
                    newEnr.Student = enrolment.Student;
                    newEnr.GRNo = enrolment.GRNo;
                    newEnr.RollNo = enrolment.RollNo;
                    newEnr.Class = newClass.ClassId;
                    newEnr.PaymentMode = enrolment.PaymentMode;
                    newEnr.LastClass = enrolment.LastClass;
                    newEnr.LastInstitude = enrolment.LastInstitude;
                    newEnr.CreatedDate = DateTime.Now;
                    newEnr.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                    newList.Add(newEnr);
                }
                else
                {
                    enrolment.Class = newClass.ClassId;
                    enrolment.UpdatedDate = DateTime.Now;
                    enrolment.UpdatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                }
                db.Enrolments.AddOrUpdate(enrolment);
            }

            db.Enrolments.AddRange(newList);
            db.SaveChanges();
            return true;
        }
        bool IncreaseFee(BulkStudents bulk)
        {
            var IDs = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();
            var enrolments = db.Enrolments.Where(x => x.IsActive.Value)
                .Where(x => IDs.Contains(x.Student.Value)).ToList();
            List<Enrolment> newList = new List<Enrolment>();
            foreach (var enrolment in enrolments)
            {
                if ((DateTime.Now - enrolment.CreatedDate).Value.TotalDays > 180)
                {
                    enrolment.IsActive = false;
                    Enrolment newEnr = new Enrolment();
                    newEnr.Fee = (Convert.ToSingle(enrolment.Fee) + Convert.ToSingle(bulk.Fee)).ToString();
                    newEnr.IsActive = true;
                    newEnr.Student = enrolment.Student;
                    newEnr.GRNo = enrolment.GRNo;
                    newEnr.RollNo = enrolment.RollNo;
                    newEnr.Class = enrolment.Class;
                    newEnr.PaymentMode = enrolment.PaymentMode;
                    newEnr.LastClass = enrolment.LastClass;
                    newEnr.LastInstitude = enrolment.LastInstitude;
                    newEnr.CreatedDate = DateTime.Now;
                    newEnr.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                    newList.Add(newEnr);
                }
                else
                {
                    enrolment.Fee = (Convert.ToSingle(enrolment.Fee) + Convert.ToSingle(bulk.Fee)).ToString();
                    enrolment.UpdatedDate = DateTime.Now;
                    enrolment.UpdatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                }
                db.Enrolments.AddOrUpdate(enrolment);
            }

            db.Enrolments.AddRange(newList);
            db.SaveChanges();
            return true;
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
                student.Father_.CreatedBy = student.CreatedBy;
                student.Father_.CreatedDate = student.CreatedDate;
                student.Mother_.CreatedBy = student.CreatedBy;
                student.Mother_.CreatedDate = student.CreatedDate;
                student.Enrolment.CreatedBy = student.CreatedBy;
                student.Enrolment.CreatedDate = student.CreatedDate;
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
                    student.Guardian_.CreatedBy = student.CreatedBy;
                    student.Guardian_.CreatedDate = student.CreatedDate;
                    var guardian = Mapper<Parent>.GetObject(student.Guardian_);
                    db.Parents.Add(guardian);
                    db.SaveChanges();
                    std.Guardian = guardian.ParentId;
                }
                var enrolment = Mapper<Enrolment>.GetObject(student.Enrolment);
                enrolment.Student = std.StudentId;
                enrolment.IsActive = true;
                db.Enrolments.Add(enrolment);
                db.Students.AddOrUpdate(std);

                PaidFee admissionFee = new PaidFee();
                admissionFee.StudentId = std.StudentId;
                admissionFee.Description = "AdmissionFee";
                admissionFee.CalculatedAmount = GlobalData.feeSetup.AdmissionFee;
                admissionFee.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                admissionFee.CreatedDate = DateTime.Now;


                db.SaveChanges();

                return RedirectToAction("Students");
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
