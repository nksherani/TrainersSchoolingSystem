using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Models.DTOs;
using TrainersSchoolingSystem.Utils;
using System.Net;
using System.Text;
using System.Data.SqlClient;

namespace TrainersSchoolingSystem.Controllers
{
    public enum AttendanceStatus
    {
        Absent = 1,
        Present,
        ShortLeave,
        LateArrival
    }
    [Authorize]
    public class TeachersController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: Teachers
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCurrentTeachers()
        {
            try
            {
                var teachers = db.Database.SqlQuery<TeacherSP>("GetCurrentTeachers").ToList();
                return Json(teachers, JsonRequestBehavior.AllowGet);
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
            return Json("");
        }
        [HttpPost]
        public ActionResult MarkAttendance(List<TeacherSP> models)
        {
            try
            {
                var Ids = models.Select(x => x.TeacherId).ToList();
                var userId = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                var AttendanceSummary = db.StaffAttendances.Where(x => Ids.Contains(x.StaffId.Value));
                foreach (var item in models)
                {
                    if (item.Status > 0)
                    {
                        var existing = db.DailyAttendances
                            .Where(x => x.StaffId == item.TeacherId).OrderByDescending(x => x.CreatedDate)?
                            .FirstOrDefault();
                        string prevStatus = "";
                        if (existing == null || (DateTime.Now - existing.CreatedDate).Value.TotalHours > 24)
                            existing = new DailyAttendance();
                        else
                            prevStatus = existing.Status;
                        existing.StaffId = item.TeacherId;
                        existing.Status = Enum.GetName(typeof(AttendanceStatus), item.Status);
                        existing.AttendanceDate = DateTime.Now;
                        if (existing.CreatedBy.HasValue)
                        {
                            existing.UpdatedBy = userId;
                            existing.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            existing.CreatedBy = userId;
                            existing.CreatedDate = DateTime.Now;
                        }
                        db.DailyAttendances.AddOrUpdate(existing);
                        var smry = AttendanceSummary.Where(x => x.StaffId == item.TeacherId).FirstOrDefault();
                        if (smry == null)
                        {
                            smry = new StaffAttendance();
                            smry.StaffId = item.TeacherId;
                            smry.CreatedBy = userId;
                            smry.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            smry.UpdatedBy = userId;
                            smry.UpdatedDate = DateTime.Now;
                        }
                        if (prevStatus != existing.Status)
                        {
                            if (existing.Status == AttendanceStatus.Absent.ToString())
                            {
                                if (smry.Absents.HasValue)
                                    smry.Absents++;
                                else
                                    smry.Absents = 1;
                            }
                            else if (existing.Status == AttendanceStatus.Present.ToString())
                            {
                                if (smry.WorkingDays.HasValue)
                                    smry.WorkingDays++;
                                else
                                    smry.WorkingDays = 1;
                            }
                            else if (existing.Status == AttendanceStatus.LateArrival.ToString())
                            {
                                if (smry.LateComings.HasValue)
                                    smry.LateComings++;
                                else
                                    smry.LateComings = 1;
                            }
                            else if (existing.Status == AttendanceStatus.ShortLeave.ToString())
                            {
                                if (smry.ShortLeaves.HasValue)
                                    smry.ShortLeaves++;
                                else
                                    smry.ShortLeaves = 1;
                            }
                            if (prevStatus == AttendanceStatus.Absent.ToString())
                            {
                                smry.Absents--;
                            }
                            else if (prevStatus == AttendanceStatus.Present.ToString())
                            {
                                smry.WorkingDays--;
                            }
                            else if (prevStatus == AttendanceStatus.ShortLeave.ToString())
                            {
                                smry.ShortLeaves--;
                            }
                            else if (prevStatus == AttendanceStatus.LateArrival.ToString())
                            {
                                smry.LateComings--;
                            }
                        }

                        db.StaffAttendances.AddOrUpdate(smry);
                    }

                }
                db.SaveChanges();
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
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GeneratePaySlips(BulkStudents bulk)
        {
            try
            {
                var stdIds = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();

                string css = "";
                var csslines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "FeeSlipHeader.html")).ToList();
                foreach (var item in csslines)
                {
                    css += item;
                }
                int i = 0;
                string feeslipsBody = "";
                foreach (var id in stdIds)
                {
                    var html = GetPaySlip(id);
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

                var filePath = Server.MapPath("~/Content/Temp/PaySlips.html");
                System.IO.File.WriteAllText(filePath, strBody.ToString());
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
            return Json("../Content/Temp/PaySlips.html", JsonRequestBehavior.AllowGet);
        }
        private string GetPaySlip(int TeacherId)
        {
            string data = "";

            try
            {
                var lastdate = db.PaidFees.Where(x => x.StudentId == TeacherId
                    && x.Description == "MonthlyFee" && x.ReceivedAmount.HasValue)?.OrderByDescending(x => x.CreatedDate)?.FirstOrDefault()?.CreatedDate;
                if (lastdate.HasValue && (DateTime.Now - lastdate).Value.TotalDays < 28 &&
                    lastdate.Value.Month == DateTime.Now.Month)
                    return "";
                SqlParameter teacherId = new SqlParameter("@TeacherId", TeacherId);
                var PaySlipData = db.Database.SqlQuery<PaySlipModel>("exec GeneratePaySlips @TeacherId", teacherId).FirstOrDefault();
                List<string> lines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "PaySlip.html")).ToList();
                foreach (var item in lines)
                {
                    data += item;
                }

                data = data.Replace("__Picture__", "../" + GlobalData.configuration.Picture);
                data = data.Replace("__SchoolName__", GlobalData.configuration.SchoolName);
                data = data.Replace("__Campus__", GlobalData.configuration.Campus);
                data = data.Replace("__Month__", DateTime.Now.ToString("MM-yyyy"));
                data = data.Replace("__IssueDate__", DateTime.Now.ToString("dd-MM-yyyy"));
                data = data.Replace("__DueDate__", DateTime.Now.ToString("10-MM-yyyy"));
                //data = data.Replace("__Challan__", PaySlipData.ChallanNo.ToString());
                data = data.Replace("__TeacherId__", PaySlipData.TeacherId.ToString());
                data = data.Replace("__Name__", PaySlipData.Name);
                data = data.Replace("__Designation__", PaySlipData.Designation);
                data = data.Replace("__JoiningDate__", PaySlipData.JoiningDate.Value.ToString("dd-MM-yyyy"));
                data = data.Replace("__BasicPay__", PaySlipData.BasicPay.ToString());
                data = data.Replace("__Bonus__", PaySlipData.Bonus.ToString());
                data = data.Replace("__ProvidentFund__", PaySlipData.PF.ToString());
                data = data.Replace("__EOBI__", PaySlipData.EOBI.ToString());
                data = data.Replace("__EOBI__", PaySlipData.LoanDeduction.ToString());
                data = data.Replace("__GrossPay__", PaySlipData.GrossPay.ToString());
                data = data.Replace("__NetPay__", PaySlipData.NetPay.ToString());
                data = data.Replace("__LoanDeduction__", "0");
                data = data.Replace("__UnpaidLeaves__", PaySlipData.UnpaidLeaves.ToString());
                data = data.Replace("__AmountDeducted__", PaySlipData.AmountDeducted.ToString());

                var lastPayment = db.SalaryPayments.Where(x => x.StaffId == TeacherId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                var userid = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                if (lastPayment != null && lastPayment.CreatedDate.Value.ToString("MM-yyyy") == DateTime.Now.ToString("MM-yyyy"))
                {
                    lastPayment.Amount = Convert.ToInt32(PaySlipData.NetPay);
                    lastPayment.UpdatedDate = DateTime.Now;
                    lastPayment.UpdatedBy = userid;
                    data = data.Replace("__Challan__", lastPayment.ChallanNo.ToString());
                }
                else
                {
                    lastPayment = new SalaryPayment();
                    lastPayment.ChallanNo = PaySlipData.ChallanNo;
                    lastPayment.StaffId = PaySlipData.TeacherId;
                    lastPayment.Amount = Convert.ToInt32(PaySlipData.NetPay);
                    lastPayment.CreatedDate = DateTime.Now;
                    lastPayment.CreatedBy = userid;
                    data = data.Replace("__Challan__", PaySlipData.ChallanNo.ToString());
                }
                db.SalaryPayments.AddOrUpdate(lastPayment);
                db.SaveChanges();
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
            return data;
        }

        public ActionResult GetTeachersMasterData()
        {
            try
            {
                var teachers = db.Staffs.Join(db.Designations.Where(x => x.Category == "Teaching"), a => a.Designation, b => b.DesignationId, (a, b) => a);
                var enrolments = db.Enrolments.Where(x => x.IsActive.Value).ToList();
                List<TeacherViewModel> modellist = new List<TeacherViewModel>();
                foreach (var teacher in teachers)
                {
                    TeacherViewModel model = new TeacherViewModel();
                    model = Mapper<TeacherViewModel>.GetObject(teacher);
                    model.Designation_ = Mapper<DesignationViewModel>.GetObject(teacher.Designation1);
                    if (teacher.Salaries.Count > 0)
                        model.Salary_ = Mapper<SalaryViewModel>.GetObject(teacher.Salaries.FirstOrDefault());
                    if (teacher.StaffAttendances.Count > 0)
                        model.Attendance = Mapper<StaffAttendanceViewModel>.GetObject(teacher.StaffAttendances.FirstOrDefault());
                    if (teacher.SalaryPayments.Count > 0)
                    {
                        var payments = teacher.SalaryPayments.ToList();
                        foreach (var payment in payments)
                        {
                            model.salaryPayment.Add(Mapper<SalaryPaymentViewModel>.GetObject(payment));
                        }
                    }
                    if (teacher.SubjectAssignments.Count > 0)
                    {
                        var subjects = teacher.SubjectAssignments.ToList();
                        foreach (var subject in subjects)
                        {
                            SubjectAssignmentViewModel assignment = Mapper<SubjectAssignmentViewModel>.GetObject(subject);
                            assignment.Class_ = Mapper<ClassViewModel>.GetObject(subject.Class1);
                            assignment.Subject_ = Mapper<SubjectViewModel>.GetObject(subject.Subject1);
                            model.subjects.Add(assignment);
                        }
                    }

                    modellist.Add(model);
                }
                return Json(modellist, JsonRequestBehavior.AllowGet);
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
            return Json("", JsonRequestBehavior.AllowGet);

        }
        // GET: Staffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Teachers/Create
        public ActionResult Appoint()
        {
            ViewBag.Designation = new SelectList(db.Designations.Where(x => x.Category == "Teaching").ToList(), "DesignationId", "DesignationName");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        public ActionResult Appoint(TeacherViewModel teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacher.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    teacher.CreatedDate = DateTime.Now;

                    Staff staff = Mapper<Staff>.GetObject(teacher);
                    db.Staffs.Add(staff);
                    db.SaveChanges();

                    Salary salary = Mapper<Salary>.GetObject(teacher.Salary_);
                    salary = SalariesController.SalaryCalculation(salary);
                    salary.StaffId = staff.StaffId;
                    salary.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    salary.CreatedDate = DateTime.Now;
                    db.Salaries.Add(salary);

                    StaffAttendance attendance = new StaffAttendance();
                    attendance.StaffId = staff.StaffId;
                    attendance.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    attendance.CreatedDate = DateTime.Now;
                    db.StaffAttendances.Add(attendance);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                ViewBag.Designation = new SelectList(db.Designations.Where(x => x.Category == "Teaching").ToList(), "DesignationId", "DesignationName");
                ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");

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
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult UpdateData(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            TeacherViewModel teacher = Mapper<TeacherViewModel>.GetObject(staff);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.Designation = new SelectList(db.Designations.Where(x => x.Category == "Teaching").ToList(), "DesignationId", "DesignationName", teacher.Designation);
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", teacher.Gender);
            return View();
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        public ActionResult UpdateData(TeacherViewModel teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var staffdb = db.Staffs.Find(teacher.StaffId);
                    staffdb.FirstName = teacher.FirstName;
                    staffdb.LastName = teacher.LastName;
                    staffdb.Designation = teacher.Designation;
                    staffdb.Gender = teacher.Gender;
                    staffdb.DateOfBirth = teacher.DateOfBirth;
                    if (staffdb.DateOfBirth.HasValue)
                        staffdb.Age = DateTime.Now.Year - staffdb.DateOfBirth.Value.Year;
                    staffdb.FatherName = teacher.FatherName;
                    staffdb.SpouseName = teacher.SpouseName;
                    staffdb.Mobile = teacher.Mobile;
                    staffdb.LandLine = teacher.LandLine;
                    staffdb.PostalCode = teacher.PostalCode;
                    staffdb.StreetAddress = teacher.StreetAddress;
                    staffdb.City = teacher.City;
                    staffdb.JoiningDate = teacher.JoiningDate;
                    staffdb.EndDate = teacher.EndDate;
                    staffdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    staffdb.UpdatedDate = DateTime.Now;
                    db.Staffs.AddOrUpdate(staffdb);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Designation = new SelectList(db.Designations.Where(x => x.Category == "Teaching").ToList(), "DesignationId", "DesignationName", teacher.Designation);
                ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", teacher.Gender);

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

        // GET: Teachers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Teachers/Delete/5
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
