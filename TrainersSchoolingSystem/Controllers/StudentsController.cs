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
                if (student.Parent2 != null)
                    model.Mother_ = Mapper<ParentViewModel>.GetObject(student.Parent2);
                if (student.Parent1 != null)
                {
                    model.Guardian_ = Mapper<GuardianViewModel>.GetObject(student.Parent1);
                }
                var enrolmentdb = enrolments.Where(x => x.Student == student.StudentId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (enrolmentdb == null)
                    continue;
                if (!enrolmentdb.IsActive.Value)
                    continue;
                model.Enrolment = Mapper<EnrolmentViewModel>.GetObject(enrolmentdb);
                model.Enrolment.Class_ = Mapper<ClassViewModel>.GetObject(enrolmentdb.Class1);
                modellist.Add(model);
            }
            return Json(modellist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateFeeSlips2(Bulk bulk)
        {
            string fname = "FeeSlips" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".html";

            var stdIds = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();

            string css = "";
            var csslines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "FeeSlipHeader.html")).ToList();
            foreach (var item in csslines)
            {
                css += item;
            }
            int i = 0;
            string feeslipsBody = "";
            int month = Convert.ToInt32(bulk.Month) + 1;
            stdIds.Sort();
            foreach (var id in stdIds)
            {
                var html = GetFeeSlip(id, month);
                if (i % 5 == 0)
                    html += "<div style=\"width: 240px; height: 1px; background - color:white\"></div>";

                feeslipsBody += html;
                i++;
            }
            if (!feeslipsBody.Contains("Due Date"))
                feeslipsBody = "<h1>No unpaid fees was found for the selected month</h1>";
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
            //return Json("../Content/Temp/FeeSlips.html", JsonRequestBehavior.AllowGet);
            return Json("../Content/Temp/" + fname, JsonRequestBehavior.AllowGet);

        }
        private string GetFeeSlip(int studentid, int month)
        {
            
            string data = "";

            try
            {
                int date = DateTime.Today.Day;
                if(date>10 && month == DateTime.Today.Month)
                    return "";
                DateTime start, end, currentMonth;
                if (date > 10)
                {
                    currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 11);
                }
                else
                {
                    currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, 11);
                }
                start = new DateTime(DateTime.Today.Year, month-1, 11);
                end = new DateTime(DateTime.Today.Year, month, 10);

                bool ispaid = db.PaidFees.Where(x => x.ReceivedAmount.HasValue &&
                x.StudentId == studentid && x.Month.Value.Month == month &&
                    x.Month.Value.Year == DateTime.Today.Year).Count() > 0;
                if (ispaid)
                    return "";
                FeeSlipModel FeeSlipData = new FeeSlipModel();
                var enr = db.Enrolments.Where(x => x.Student == studentid && x.IsActive.Value).FirstOrDefault();
                var std = enr.Student1;
                FeeSlipData.FirstName = std.FirstName;
                FeeSlipData.LastName = std.LastName;
                FeeSlipData.GRNo = enr.GRNo;
                FeeSlipData.RollNo = enr.RollNo;
                FeeSlipData.Class = enr.Class1.ClassName + "-" + enr.Class1.Section;
                FeeSlipData.MonthlyFee = enr.MonthlyFee.Value;
                var paidfees_ = db.PaidFees.Select(x => x.ChallanNo).ToList();
                int challan = 0;
                if(paidfees_.Count==0)
                {
                    challan = 0;
                }
                else
                {
                    challan = db.PaidFees.Select(x => x.ChallanNo.HasValue ? x.ChallanNo.Value : 0).Max();
                }
                FeeSlipData.ChallanNo = Convert.ToInt32(challan + 1);
                FeeSlipData.FatherName = std.Parent.Name;
                if (month.ToString() == GlobalData.configuration.FirstMonth)
                {
                    var annualdb = db.PaidFees.Where(x => x.StudentId == studentid && x.Description == "AnnualFee" && x.Month.Value.Month == DateTime.Today.Month && x.Month.Value.Year == DateTime.Today.Year).FirstOrDefault();
                    if (annualdb == null)
                    {
                        PaidFee annualFee = new PaidFee();
                        annualFee.StudentId = std.StudentId;
                        annualFee.Description = "AnnualFee";
                        annualFee.Month = DateTime.Today.AddDays((-1) * (DateTime.Today.Day - 1));
                        if (enr.AnnualFee != 0)
                        {
                            annualFee.CalculatedAmount = enr.AnnualFee;
                        }
                        else
                        {
                            annualFee.CalculatedAmount = GlobalData.feeSetup.AnnualFee;
                        }
                        annualFee.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                        annualFee.CreatedDate = DateTime.Now;
                        annualFee.ChallanNo = FeeSlipData.ChallanNo;
                        db.PaidFees.Add(annualFee);
                        db.SaveChanges();
                    }
                }
                else { 
                    //naveed
                    var annualdb = db.PaidFees.Where(x => x.StudentId == studentid && x.Description == "AnnualFee" && x.Month.Value > start && x.Month.Value<end).FirstOrDefault();
                    if (annualdb != null)
                        FeeSlipData.AnnualFee = annualdb.CalculatedAmount.Value;
                    var admissiondb = db.PaidFees.Where(x => x.StudentId == studentid && x.Description == "AdmissionFee" && x.Month.Value > start && x.Month.Value < end).FirstOrDefault();
                    if (admissiondb != null)
                        FeeSlipData.AdmissionFee = admissiondb.CalculatedAmount.Value;
                }
                var unpaid = db.PaidFees.Where(x => !x.ReceivedAmount.HasValue && x.StudentId == studentid);
                
                //bool isInsideCurrentDateRangel
                //decimal unpaidamount = 0;
                var monthlyunpaid = unpaid.Where(x => x.Description == "MonthlyFee").ToList();
                var annualunpaid = unpaid.Where(x => x.Description == "AnnualFee").ToList();
                var admissionunpaid = unpaid.Where(x => x.Description == "AdmissionFee").FirstOrDefault();
                foreach (var item in monthlyunpaid)
                {
                    if (date < 10 && (item.Month.Value.Month == DateTime.Today.Month &&
                        item.Month.Value.Year == DateTime.Today.Year))
                    {
                        continue;
                    }
                    else
                    {
                        if (month != item.Month.Value.Month && item.Month.Value.Month!= end.Month && item.Month.Value<currentMonth)
                            FeeSlipData. UnpaidAmount += item.CalculatedAmount.Value;
                    }
                }
                if (date < 10 && admissionunpaid != null && (admissionunpaid.Month.Value.Month == DateTime.Today.Month &&
                        admissionunpaid.Month.Value.Year == DateTime.Today.Year))
                {
                    if (month == admissionunpaid.Month.Value.Month)
                        FeeSlipData.AdmissionFee = admissionunpaid.CalculatedAmount.Value;
                }
                else if (admissionunpaid != null)
                {
                    if (month != admissionunpaid.Month.Value.Month && admissionunpaid.Month.Value.Month != end.Month && admissionunpaid.Month.Value < currentMonth)
                        FeeSlipData.UnpaidAmount += admissionunpaid.CalculatedAmount.Value;
                }
                foreach (var item in annualunpaid)
                {
                    if (date < 10 && (item.Month.Value.Month == DateTime.Today.Month &&
                        item.Month.Value.Year == DateTime.Today.Year))
                    {
                        if (month == item.Month.Value.Month)
                            FeeSlipData.AnnualFee = item.CalculatedAmount.Value;
                    }
                    else
                    {
                        if (month != item.Month.Value.Month && item.Month.Value.Month != end.Month && item.Month.Value < currentMonth)
                            FeeSlipData.UnpaidAmount += item.CalculatedAmount.Value;
                    }
                }
                var arrearsdb = db.Arrears.Where(x => x.StudentId == studentid).ToList();
                if (arrearsdb.Count > 0)
                    FeeSlipData.ArrearAmount = arrearsdb.Select(x => x.Amount.HasValue ? x.Amount.Value : 0).Sum();
                else
                    FeeSlipData.ArrearAmount = 0;
                FeeSlipData.ArrearType = "Receivable";
                FeeSlipData.ArrearAmount += FeeSlipData.UnpaidAmount;
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "FeeSlip.html"));

                foreach (var item in lines)
                {
                    data += item;
                }

                var totalFee = FeeSlipData.ArrearAmount +
                    Convert.ToInt32(FeeSlipData.MonthlyFee) + FeeSlipData.AdmissionFee +
                    FeeSlipData.AnnualFee + GlobalData.feeSetup.LabCharges;
                data = data.Replace("__Picture__", "../" + GlobalData.configuration.Picture);
                data = data.Replace("__SchoolName__", GlobalData.configuration.SchoolName);
                data = data.Replace("__Campus__", GlobalData.configuration.Campus);
                var tempmonth = Constants.months[month - 1].Value + DateTime.Now.ToString("-yyyy");
                data = data.Replace("__Month__", tempmonth);
                data = data.Replace("__IssueDate__", DateTime.Now.ToString("dd-MM-yyyy"));
                data = data.Replace("__DueDate__", DateTime.Now.ToString($"10-{month}-yyyy"));
                //data = data.Replace("__Challan__", FeeSlipData.ChallanNo.ToString());
                data = data.Replace("__GRNo__", FeeSlipData.GRNo);
                data = data.Replace("__StudentName__", FeeSlipData.FirstName + " " + FeeSlipData.LastName);
                data = data.Replace("__Class__", FeeSlipData.Class);
                data = data.Replace("__FatherName__", FeeSlipData.FatherName);
                data = data.Replace("__Arrears__", FeeSlipData.ArrearAmount.ToString());
                data = data.Replace("__TuitionFee__", FeeSlipData.MonthlyFee.ToString());
                data = data.Replace("__AdmissionFee__", FeeSlipData.AdmissionFee.ToString());
                data = data.Replace("__AnnualFee__", FeeSlipData.AnnualFee.ToString());
                data = data.Replace("__OtherFee__", "0");
                data = data.Replace("__LateFee__", GlobalData.feeSetup.LatePaymentSurcharge.ToString());
                data = data.Replace("__LabCharges__", GlobalData.feeSetup.LabCharges.ToString());
                data = data.Replace("__Till__", totalFee.ToString());
                data = data.Replace("__After__", (totalFee + GlobalData.feeSetup.LatePaymentSurcharge).ToString());


                var feeDb = db.PaidFees.Where(x => !x.ReceivedAmount.HasValue &&
                x.StudentId == studentid && x.Month.Value.Month == month &&
                    x.Month.Value.Year == DateTime.Today.Year).ToList();

                if (FeeSlipData.MonthlyFee != 0)
                {
                    PaidFee paidFee;
                    var monthly = feeDb.Where(x => x.Description == "MonthlyFee").FirstOrDefault();
                    if (monthly != null)
                    {
                        paidFee = monthly;
                        data = data.Replace("__Challan__", paidFee.ChallanNo.ToString());

                    }
                    else
                    {
                        paidFee = new PaidFee();
                        paidFee.ChallanNo = FeeSlipData.ChallanNo;
                        data = data.Replace("__Challan__", FeeSlipData.ChallanNo.ToString());
                    }
                    paidFee.Month = new DateTime(DateTime.Today.Year, month, 1);
                    paidFee.StudentId = studentid;
                    paidFee.CalculatedAmount = FeeSlipData.MonthlyFee;
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
                    PaidFee paidFee;
                    var labfee = feeDb.Where(x => x.Description == "LabCharges").FirstOrDefault();
                    if (labfee != null)
                        paidFee = labfee;
                    else
                    {
                        paidFee = new PaidFee();
                        paidFee.ChallanNo = FeeSlipData.ChallanNo;
                    }
                    paidFee.StudentId = studentid;
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
                if (month.ToString()==GlobalData.configuration.FirstMonth)
                {
                    PaidFee paidFee;
                    var annual = feeDb.Where(x => x.Description == "AnnualFee").FirstOrDefault();
                    if (annual != null)
                        paidFee = annual;
                    else
                    {
                        paidFee = new PaidFee();
                        paidFee.ChallanNo = FeeSlipData.ChallanNo;
                        paidFee.Month = new DateTime(DateTime.Today.Year, month, 1);
                        //naveed
                        //if (data > 10)
                        //    paidFee.Month = paidFee.Month.Value.AddMonths(1);
                    }
                    paidFee.StudentId = studentid;
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

        private string GetFeeSlip_(int studentid, int month)
        {
            string data = "";

            try
            {
                SqlParameter StudentId = new SqlParameter("@StudentId", studentid);
                SqlParameter Month = new SqlParameter("@Month", month);
                var FeeSlipData = db.Database.SqlQuery<FeeSlipModel>("exec GenerateFeeSlips @StudentId, @Month", StudentId, Month).FirstOrDefault();

                Server.MapPath("~/Content/FeeSlips/");
                string[] lines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/" + "FeeSlip.html"));

                foreach (var item in lines)
                {
                    data += item;
                }
                if (FeeSlipData.ArrearType == "Receivable")
                {
                    FeeSlipData.ArrearAmount += FeeSlipData.UnpaidAmount;
                }

                else
                {
                    FeeSlipData.ArrearAmount -= FeeSlipData.UnpaidAmount;
                    if (FeeSlipData.ArrearAmount < 0)
                    {
                        FeeSlipData.ArrearType = "Receivable";
                    }
                    FeeSlipData.ArrearAmount *= -1;
                }

                var totalFee = FeeSlipData.ArrearAmount +
                    Convert.ToInt32(FeeSlipData.MonthlyFee) + FeeSlipData.AdmissionFee +
                    FeeSlipData.AnnualFee + GlobalData.feeSetup.LabCharges;
                data = data.Replace("__Picture__", "../" + GlobalData.configuration.Picture);
                data = data.Replace("__SchoolName__", GlobalData.configuration.SchoolName);
                data = data.Replace("__Campus__", GlobalData.configuration.Campus);
                var tempmonth = Constants.months[month - 1].Value + DateTime.Now.ToString("-yyyy");
                data = data.Replace("__Month__", tempmonth);
                data = data.Replace("__IssueDate__", DateTime.Now.ToString("dd-MM-yyyy"));
                data = data.Replace("__DueDate__", DateTime.Now.ToString($"10-{month}-yyyy"));
                //data = data.Replace("__Challan__", FeeSlipData.ChallanNo.ToString());
                data = data.Replace("__GRNo__", FeeSlipData.GRNo);
                data = data.Replace("__StudentName__", FeeSlipData.FirstName + " " + FeeSlipData.LastName);
                data = data.Replace("__Class__", FeeSlipData.Class);
                data = data.Replace("__FatherName__", FeeSlipData.FatherName);
                data = data.Replace("__Arrears__", FeeSlipData.ArrearAmount.ToString());
                data = data.Replace("__TuitionFee__", FeeSlipData.MonthlyFee.ToString());
                data = data.Replace("__AdmissionFee__", FeeSlipData.AdmissionFee.ToString());
                data = data.Replace("__AnnualFee__", FeeSlipData.AnnualFee.ToString());
                data = data.Replace("__OtherFee__", "0");
                data = data.Replace("__LateFee__", GlobalData.feeSetup.LatePaymentSurcharge.ToString());
                data = data.Replace("__LabCharges__", GlobalData.feeSetup.LabCharges.ToString());
                data = data.Replace("__Till__", totalFee.ToString());
                data = data.Replace("__After__", (totalFee + GlobalData.feeSetup.LatePaymentSurcharge).ToString());

                bool ispaid = db.PaidFees.Where(x => x.ReceivedAmount.HasValue &&
                x.StudentId == studentid && x.Month.Value.Month == month &&
                    x.CreatedDate.Value.Year == DateTime.Today.Year).Count() > 0;
                if (ispaid)
                    return "";
                var feeDb = db.PaidFees.Where(x => !x.ReceivedAmount.HasValue &&
                x.StudentId == studentid && x.Month.Value.Month == month &&
                    x.CreatedDate.Value.Year == DateTime.Today.Year).ToList();

                if (FeeSlipData.MonthlyFee != 0)
                {
                    PaidFee paidFee;
                    var count = feeDb.Where(x => x.Description == "MonthlyFee" && x.Month.Value.Month == month &&
                    x.CreatedDate.Value.Year == DateTime.Today.Year).ToList();
                    if (count.Count > 0)
                    {
                        paidFee = feeDb.Where(x => x.Description == "MonthlyFee" && x.Month.Value.Month == month &&
                    x.CreatedDate.Value.Year == DateTime.Today.Year).FirstOrDefault();
                        data = data.Replace("__Challan__", paidFee.ChallanNo.ToString());

                    }
                    else
                    {
                        paidFee = new PaidFee();
                        paidFee.ChallanNo = FeeSlipData.ChallanNo;
                        data = data.Replace("__Challan__", FeeSlipData.ChallanNo.ToString());
                    }
                    paidFee.Month = new DateTime(DateTime.Today.Year, month, 1);
                    paidFee.StudentId = studentid;
                    paidFee.CalculatedAmount = Convert.ToInt32(FeeSlipData.MonthlyFee);
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
                    PaidFee paidFee;
                    var count = feeDb.Where(x => x.Description == "LabCharges" && x.Month.Value.Year == month &&
                    x.CreatedDate.Value.Year == DateTime.Today.Year).Count();
                    if (count > 0)
                        paidFee = feeDb.Where(x => x.Description == "LabCharges" && x.Month.Value.Month == month &&
                        x.CreatedDate.Value.Year == DateTime.Today.Year).FirstOrDefault();
                    else
                    {
                        paidFee = new PaidFee();
                        paidFee.ChallanNo = FeeSlipData.ChallanNo;
                    }
                    paidFee.StudentId = studentid;
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
                    PaidFee paidFee;
                    if (feeDb.Where(x => x.Description == "AnnualFee" && x.CreatedDate.Value.Year == DateTime.Today.Year).Count() > 0)
                        paidFee = feeDb.Where(x => x.Description == "AnnualFee" && x.CreatedDate.Value.Year == DateTime.Today.Year).FirstOrDefault();
                    else
                    {
                        paidFee = new PaidFee();
                        paidFee.ChallanNo = FeeSlipData.ChallanNo;
                        paidFee.Month = new DateTime(DateTime.Today.Year, month, 1);
                    }
                    paidFee.StudentId = studentid;
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


        [HttpPost]
        public ActionResult Bulk(Bulk bulk)
        {
            string path = "";
            bool flag = false;
            try
            {
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

            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        bool PromoteToUpperClass(Bulk bulk)
        {
            try
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
                        newEnr.MonthlyFee = enrolment.MonthlyFee;
                        newEnr.AdmissionFee = enrolment.AdmissionFee;
                        newEnr.AnnualFee = enrolment.AnnualFee;
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
            return false;
        }
        bool ChangeClass(Bulk bulk)
        {
            try
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
                        newEnr.MonthlyFee = enrolment.MonthlyFee;
                        newEnr.AdmissionFee = enrolment.AdmissionFee;
                        newEnr.AnnualFee = enrolment.AnnualFee;
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
            return false;
        }
        bool ChangeSection(Bulk bulk)
        {
            try
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
                        newEnr.MonthlyFee = enrolment.MonthlyFee;
                        newEnr.AdmissionFee = enrolment.AdmissionFee;
                        newEnr.AnnualFee = enrolment.AnnualFee;
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

            return false;
        }
        bool IncreaseFee(Bulk bulk)
        {
            try
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
                        newEnr.MonthlyFee = enrolment.MonthlyFee + Convert.ToDecimal(bulk.Fee);
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
                        enrolment.MonthlyFee = enrolment.MonthlyFee + Convert.ToDecimal(bulk.Fee);
                        enrolment.UpdatedDate = DateTime.Now;
                        enrolment.UpdatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                    }
                    db.Enrolments.AddOrUpdate(enrolment);
                }

                db.Enrolments.AddRange(newList);
                db.SaveChanges();
                return true;
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

            return false;
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
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.ClassAdmitted = new SelectList(classes, "Key", "Value");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");
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
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.ClassAdmitted = new SelectList(classes, "Key", "Value");
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", Common.FirstCharToUpper(student.Gender));
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
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText");
            ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name");
            ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name");
            ViewBag.Mother = new SelectList(db.Parents, "ParentId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAdmission(StudentViewModel student)
        {
            try
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
                    var temp = db.Parents.Where(x => x.CNIC == father.CNIC).ToList();
                    if (temp.Count == 0)
                    {
                        db.Parents.Add(father);
                        db.SaveChanges();
                    }
                    else
                    {
                        father = temp.FirstOrDefault();
                    }
                    std.Father = father.ParentId;

                    temp = db.Parents.Where(x => x.CNIC == mother.CNIC).ToList();
                    if (temp.Count == 0)
                    {
                        db.Parents.Add(mother);
                        db.SaveChanges();
                    }
                    else
                    {
                        mother = temp.FirstOrDefault();
                    }
                    std.Mother = mother.ParentId;

                    if (student.Guardian_.Name != null)
                    {
                        student.Guardian_.CreatedBy = student.CreatedBy;
                        student.Guardian_.CreatedDate = student.CreatedDate;
                        var guardian = Mapper<Parent>.GetObject(student.Guardian_);

                        temp = db.Parents.Where(x => x.CNIC == guardian.CNIC).ToList();
                        if (temp.Count == 0)
                        {
                            db.Parents.Add(guardian);
                            db.SaveChanges();
                        }
                        else
                        {
                            guardian = temp.FirstOrDefault();
                        }

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
                    admissionFee.CalculatedAmount = enrolment.AdmissionFee;
                    admissionFee.Month = DateTime.Today.AddDays(-DateTime.Today.Day+1);
                    admissionFee.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    admissionFee.CreatedDate = DateTime.Now;
                    db.PaidFees.Add(admissionFee);

                    PaidFee annualfee = new PaidFee();
                    annualfee.StudentId = std.StudentId;
                    annualfee.Description = "AnnualFee";
                    annualfee.CalculatedAmount = enrolment.AnnualFee;
                    annualfee.Month = DateTime.Today.AddDays(-DateTime.Today.Day+1);
                    annualfee.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    annualfee.CreatedDate = DateTime.Now;
                    db.PaidFees.Add(annualfee);

                    PaidFee monthlyfee = new PaidFee();
                    monthlyfee.StudentId = std.StudentId;
                    monthlyfee.ChallanNo = db.PaidFees.Select(x => x.ChallanNo.HasValue?x.ChallanNo:0).Max() + 1;
                    monthlyfee.Description = "MonthlyFee";
                    monthlyfee.CalculatedAmount = enrolment.MonthlyFee;
                    monthlyfee.Month = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                    monthlyfee.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                    monthlyfee.CreatedDate = DateTime.Now;
                    db.PaidFees.Add(monthlyfee);

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
                ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", Common.FirstCharToUpper(student.Gender));
                ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name", student.Father);
                ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name", student.Guardian);
                ViewBag.Mother = new SelectList(db.Parents, "ParentId", "Name", student.Mother);

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
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.ClassAdmitted = new SelectList(classes, "Key", "Value", student.ClassAdmitted);
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", Common.FirstCharToUpper(student.Gender));
            ViewBag.Father = new SelectList(db.Parents, "ParentId", "Name", student.Father);
            ViewBag.Guardian = new SelectList(db.Parents, "ParentId", "Name", student.Guardian);
            ViewBag.Mother = new SelectList(db.Parents.Where(x => x.Relation.ToLower() == "mother"), "ParentId", "Name", student.Mother);
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
                studentdb.ClassAdmitted = student.ClassAdmitted;
                studentdb.EndDate = student.EndDate;
                studentdb.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                studentdb.UpdatedDate = DateTime.Now;
                db.Students.AddOrUpdate(studentdb);
                db.SaveChanges();
                return RedirectToAction("Students");
            }
            List<KeyValuePair<int, string>> classes = new List<KeyValuePair<int, string>>();
            foreach (var item in db.Classes)
            {
                var pair = new KeyValuePair<int, string>(item.ClassId, item.ClassName + item.Section);
                classes.Add(pair);
            }
            ViewBag.ClassAdmitted = new SelectList(classes, "Key", "Value", student.ClassAdmitted);
            ViewBag.Gender = new SelectList(db.Lookups.Where(x => x.LookupType.LookupTypeName == "Gender"), "LookupText", "LookupText", Common.FirstCharToUpper(student.Gender));
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


        ///unused code
        bool SavePdf(string filename, string path, string html)
        {
            try
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
            return false;
        }
        bool SavePdf2(string filename, string path, string html)
        {
            try
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
            return false;
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

    }
}
