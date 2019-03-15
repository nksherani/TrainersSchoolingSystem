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
    public class StaffsController : Controller
    {
        private TrainersEntities db = new TrainersEntities();

        // GET: Staffs
        public ActionResult Index()
        {
            return View();
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
            ViewBag.Designation = new SelectList(db.Designations.ToList(), "DesignationId", "DesignationName");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        public ActionResult Appoint(StaffViewModel staff_)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    staff_.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    staff_.CreatedDate = DateTime.Now;
                    if (staff_.DateOfBirth.HasValue)
                        staff_.Age = Convert.ToInt32((DateTime.Now - staff_.DateOfBirth.Value).TotalDays / 365);
                    Staff staff = Mapper<Staff>.GetObject(staff_);
                    db.Staffs.Add(staff);
                    db.SaveChanges();

                    Salary salary = Mapper<Salary>.GetObject(staff_.Salary_);
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
                ViewBag.Designation = new SelectList(db.Designations.ToList(), "DesignationId", "DesignationName");
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
            return View(staff_);
        }
        // GET: Teachers/Edit/5
        public ActionResult UpdateData(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            Salary salary = db.Salaries.Where(x => x.StaffId == staff.StaffId).FirstOrDefault();
            StaffViewModel staff_ = Mapper<StaffViewModel>.GetObject(staff);
            staff_.Salary_ = Mapper<SalaryViewModel>.GetObject(salary);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.Designation = new SelectList(db.Designations.ToList(), "DesignationId", "DesignationName", staff_.Designation);
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", staff_.Gender);
            return View(staff_);
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        public ActionResult UpdateData(StaffViewModel staff_)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var staffdb = db.Staffs.Find(staff_.StaffId);
                    staffdb.FirstName = staff_.FirstName;
                    staffdb.LastName = staff_.LastName;
                    staffdb.CNIC = staff_.CNIC;
                    staffdb.Designation = staff_.Designation;
                    staffdb.Qualification = staff_.Qualification;
                    staffdb.Gender = staff_.Gender;
                    staffdb.DateOfBirth = staff_.DateOfBirth;
                    if (staffdb.DateOfBirth.HasValue)
                        staffdb.Age = DateTime.Now.Year - staffdb.DateOfBirth.Value.Year;
                    staffdb.FatherName = staff_.FatherName;
                    staffdb.SpouseName = staff_.SpouseName;
                    staffdb.Mobile = staff_.Mobile;
                    staffdb.LandLine = staff_.LandLine;
                    staffdb.PostalCode = staff_.PostalCode;
                    staffdb.StreetAddress = staff_.StreetAddress;
                    staffdb.City = staff_.City;
                    staffdb.JoiningDate = staff_.JoiningDate;
                    staffdb.EndDate = staff_.EndDate;
                    staffdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    staffdb.UpdatedDate = DateTime.Now;
                    db.Staffs.AddOrUpdate(staffdb);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Designation = new SelectList(db.Designations.ToList(), "DesignationId", "DesignationName", staff_.Designation);
                ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", staff_.Gender);

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
        [HttpPost]
        public ActionResult MarkAttendance(List<StaffSP> models)
        {
            try
            {
                var Ids = models.Select(x => x.StaffId).ToList();
                var userId = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                var AttendanceSummary = db.StaffAttendances.Where(x => Ids.Contains(x.StaffId.Value));
                foreach (var item in models)
                {
                    if (item.Status > 0)
                    {
                        var existing = db.DailyAttendances
                            .Where(x => x.StaffId == item.StaffId && x.AttendanceDate == DateTime.Today)?
                            .FirstOrDefault();
                        string prevStatus = "";
                        if (existing == null)
                        {
                            existing = new DailyAttendance();
                            existing.CreatedBy = userId;
                            existing.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            prevStatus = existing.Status;
                            existing.UpdatedBy = userId;
                            existing.UpdatedDate = DateTime.Now;
                        }
                        existing.StaffId = item.StaffId;
                        existing.Status = Enum.GetName(typeof(AttendanceStatus), item.Status);
                        existing.AttendanceDate = DateTime.Today;


                        db.DailyAttendances.AddOrUpdate(existing);
                        var smry = AttendanceSummary.Where(x => x.StaffId == item.StaffId).FirstOrDefault();
                        if (smry == null)
                        {
                            smry = new StaffAttendance();
                            smry.StaffId = item.StaffId;
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
        public ActionResult Bulk(Bulk bulk)
        {
            if (bulk.Action == "1")
                return Terminate(bulk);
            else
                return GeneratePaySlips(bulk);
        }

        private ActionResult Terminate(Bulk bulk)
        {
            try
            {
                var staffids = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();
                var staffs = db.Staffs.Where(x => staffids.Contains(x.StaffId)).ToList();
                foreach (var item in staffs)
                {
                    item.EndDate = DateTime.Now;
                    db.Staffs.AddOrUpdate(item);
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

        public ActionResult GeneratePaySlips(Bulk bulk)
        {
            string fname = "payslips" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".html";
            try
            {
                var staffids = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();

                string css = "";
                var csslines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "FeeSlipHeader.html")).ToList();
                foreach (var item in csslines)
                {
                    css += item;
                }
                int i = 0;
                string feeslipsBody = "";
                int month = Convert.ToInt32(bulk.Month) + 1;
                staffids.Sort();
                foreach (var id in staffids)
                {
                    var html = GetPaySlip(id, month);
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
                string folderpath = Server.MapPath("~/Content/Temp");
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                var filePath = Server.MapPath("~/Content/Temp/" + fname);
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
            return Json("../Content/Temp/" + fname, JsonRequestBehavior.AllowGet);
        }
        private string GetPaySlip(int StaffId, int month)
        {
            string data = "";

            try
            {

                SqlParameter staffId = new SqlParameter("@StaffId", StaffId);
                SqlParameter Month = new SqlParameter("@Month", month);

                var PaySlipData = db.Database.SqlQuery<PaySlipModel>("exec GeneratePaySlips @StaffId, @Month", staffId, Month).FirstOrDefault();
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
                data = data.Replace("__StaffId__", PaySlipData.StaffId.ToString());
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

                var mPayment = db.SalaryPayments.Where(x => x.StaffId == StaffId &&
                                x.Month == month && x.CreatedDate.Value.Year == DateTime.Now.Year).FirstOrDefault();
                var userid = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                if (mPayment != null)
                {
                    mPayment.Amount = Convert.ToInt32(PaySlipData.NetPay);
                    mPayment.UpdatedDate = DateTime.Now;
                    mPayment.UpdatedBy = userid;
                    data = data.Replace("__Challan__", mPayment.ChallanNo.ToString());
                }
                else
                {
                    mPayment = new SalaryPayment();
                    mPayment.ChallanNo = PaySlipData.ChallanNo;
                    mPayment.StaffId = PaySlipData.StaffId;
                    mPayment.Amount = Convert.ToInt32(PaySlipData.NetPay);
                    mPayment.CreatedDate = DateTime.Now;
                    mPayment.CreatedBy = userid;
                    data = data.Replace("__Challan__", PaySlipData.ChallanNo.ToString());
                }
                mPayment.Month = month;
                db.SalaryPayments.AddOrUpdate(mPayment);
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

        public ActionResult GetCurrentStaffs()
        {
            try
            {
                var staff = db.Database.SqlQuery<StaffSP>("GetCurrentStaff").ToList();
                return Json(staff, JsonRequestBehavior.AllowGet);
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

        // GET: Staffs/Create
        public ActionResult Create()
        {
            ViewBag.Designation = new SelectList(db.Designations.ToList(), "DesignationId", "DesignationName");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");

            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                staff.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                staff.CreatedDate = DateTime.Now;
                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Designation = new SelectList(db.Designations.ToList(), "DesignationId", "DesignationName");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");
            //ViewBag.CreatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", staff.CreatedBy);
            //ViewBag.UpdatedBy = new SelectList(db.TrainerUsers, "TrainerUserId", "Username", staff.UpdatedBy);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Designation = new SelectList(db.Designations.ToList(), "DesignationId", "DesignationName", staff.Designation);
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", staff.Gender);

            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Staff staff)
        {
            if (ModelState.IsValid)
            {
                var staffdb = db.Staffs.Find(staff.StaffId);
                staffdb.FirstName = staff.FirstName;
                staffdb.LastName = staff.LastName;
                staffdb.CNIC = staff.CNIC;
                staffdb.Designation = staff.Designation;
                staffdb.Qualification = staff.Qualification;
                staffdb.Gender = staff.Gender;
                staffdb.DateOfBirth = staff.DateOfBirth;
                if (staffdb.DateOfBirth.HasValue)
                    staffdb.Age = DateTime.Now.Year - staffdb.DateOfBirth.Value.Year;
                staffdb.FatherName = staff.FatherName;
                staffdb.SpouseName = staff.SpouseName;
                staffdb.Mobile = staff.Mobile;
                staffdb.LandLine = staff.LandLine;
                staffdb.PostalCode = staff.PostalCode;
                staffdb.StreetAddress = staff.StreetAddress;
                staffdb.City = staff.City;
                staffdb.JoiningDate = staff.JoiningDate;
                staffdb.EndDate = staff.EndDate;
                staffdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                staffdb.UpdatedDate = DateTime.Now;
                db.Staffs.AddOrUpdate(staffdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Designation = new SelectList(db.Designations.ToList(), "DesignationId", "DesignationName", staff.Designation);
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", staff.Gender);

            return View(staff);
        }

        // GET: Staffs/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
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
