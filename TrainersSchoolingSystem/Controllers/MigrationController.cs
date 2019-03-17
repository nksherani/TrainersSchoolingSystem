using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Utils;

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
                ProcessStaffData(path);
                ProcessClassesData(path);
                ProcessSubjectsData(path);
                ProcessStudentsData(path);
                ProcessDefaultersData(path);
                ProcessClassTeachersData(path);
                ProcessFeesData(path);
                ProcessSalariesData(path);
            }
            ViewBag.Message = "Data Uploaded Successfully";
            return Redirect("../Home/Index");
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
                    {
                        des = designationdb.Where(x => x.DesignationName == des.DesignationName).FirstOrDefault();
                        des.UpdatedBy = userid;
                        des.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        des.CreatedDate = DateTime.Now;
                        des.CreatedBy = userid;
                    }
                    des.Category = workSheet.Cells[i, 3].Text;
                    des.PaidLeaves = Convert.ToInt32(workSheet.Cells[i, 4].Text == "" ? "0" : workSheet.Cells[i, 4].Text);
                    des.ShortLeavesScale = Convert.ToInt32(workSheet.Cells[i, 5].Text == "" ? "0" : workSheet.Cells[i, 5].Text);
                    des.LateComingScale = Convert.ToInt32(workSheet.Cells[i, 6].Text == "" ? "0" : workSheet.Cells[i, 6].Text);
                    db.Designations.AddOrUpdate(des);

                }

            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessStaffData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[2];
            var staffdb = db.Staffs.ToList();
            var designations = db.Designations.ToList();
            var salaries = db.Salaries.ToList();
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
                    Staff staff = new Staff();
                    List<string> name = workSheet.Cells[i, 5].Text.Split(' ').ToList();
                    staff.FirstName = name.FirstOrDefault();
                    staff.LastName = name.Skip(1).Aggregate<string, string>("", (a, b) => a + " " + b).Trim();
                    if (staffdb.Where(x => x.FirstName == staff.FirstName && x.LastName == staff.LastName).Count() > 0)
                    {
                        staff = staffdb.Where(x => x.FirstName == staff.FirstName && x.LastName == staff.LastName).FirstOrDefault();
                        staff.UpdatedBy = userid;
                        staff.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        staff.CreatedDate = DateTime.Now;
                        staff.CreatedBy = userid;
                    }
                    var ndes = workSheet.Cells[i, 10].Text.ToLower().Replace("i", "").Replace("rr", "").Replace("r", "").Replace("ss", "").Replace("y", "");
                    var des = designations.Where(x => x.DesignationName.ToLower().Replace("i", "").Replace("rr", "").Replace("r", "").Replace("ss", "").Replace("y", "").Contains(ndes.ToLower()) ||
                    ndes.ToLower().Contains(x.DesignationName.ToLower().Replace("i", "").Replace("rr", "").Replace("r", "").Replace("ss", "").Replace("y", ""))
                    ).FirstOrDefault();
                    if (des != null)
                        staff.Designation = des.DesignationId;
                    else
                    {

                    }
                    staff.Qualification = workSheet.Cells[i, 11].Text;
                    DateTime date;
                    if (DateTime.TryParse(workSheet.Cells[i, 12].Text, out date))
                        staff.JoiningDate = date;
                    staff.CNIC = workSheet.Cells[i, 17].Text;
                    db.Staffs.AddOrUpdate(staff);
                    db.SaveChanges();
                    Salary salary = new Salary();
                    if (salaries.Where(x => x.StaffId == staff.StaffId).Count() > 0)
                    {
                        salary = salaries.Where(x => x.StaffId == staff.StaffId).FirstOrDefault();
                        salary.UpdatedBy = userid;
                        salary.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        salary.StaffId = staff.StaffId;
                        salary.CreatedBy = userid;
                        salary.CreatedDate = DateTime.Now;
                    }
                    if (workSheet.Cells[i, 14].Text != "")
                    {
                        salary.BasicPay = Convert.ToDecimal(workSheet.Cells[i, 14].Text);
                        salary.GrossPay = Convert.ToDecimal(workSheet.Cells[i, 14].Text);
                        salary.NetPay = Convert.ToDecimal(workSheet.Cells[i, 14].Text);
                    }
                    db.Salaries.AddOrUpdate(salary);
                }

            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessClassesData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[3];
            var classesdb = db.Classes.ToList();
            char[] sections = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K' };
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
                    int level = 0;
                    if (workSheet.Cells[i, 3].Text != "")
                        level = Convert.ToInt32(workSheet.Cells[i, 3].Text);
                    int count = 0;
                    if (workSheet.Cells[i, 4].Text != "")
                        count = Convert.ToInt32(workSheet.Cells[i, 4].Text);
                    for (int j = 0; j < count; j++)
                    {
                        Class _class = new Class();
                        _class.ClassName = workSheet.Cells[i, 2].Text;

                        if (classesdb.Where(x => x.ClassName.ToLower() == _class.ClassName.ToLower() &&
                        x.Section.ToUpper() == sections[j].ToString()).Count() > 0)
                        {
                            _class = classesdb.
                                Where(x => x.ClassName.ToLower() == _class.ClassName.ToLower() &&
                                x.Section.ToUpper() == sections[j].ToString()).FirstOrDefault();
                            _class.UpdatedBy = userid;
                            _class.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            _class.CreatedDate = DateTime.Now;
                            _class.CreatedBy = userid;
                        }
                        _class.Section = sections[j].ToString();
                        _class.Level = level;
                        db.Classes.AddOrUpdate(_class);

                    }

                }

            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessSubjectsData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[4];
            var subjectdb = db.Subjects.ToList();
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
                    Subject sub = new Subject();
                    sub.SubjectName = workSheet.Cells[i, 2].Text;
                    if (subjectdb.Where(x => x.SubjectName == sub.SubjectName).Count() > 0)
                    {
                        sub = subjectdb.Where(x => x.SubjectName == sub.SubjectName).FirstOrDefault();
                        sub.UpdatedBy = userid;
                        sub.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        sub.CreatedDate = DateTime.Now;
                        sub.CreatedBy = userid;
                    }
                    db.Subjects.AddOrUpdate(sub);

                }

            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessStudentsData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[5];
            var studentdb = db.Students.ToList();
            var classes = db.Classes.ToList();
            var enrolments = db.Enrolments.ToList();
            var parents = db.Parents.ToList();
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
                    Student student = new Student();
                    Enrolment enrolment = new Enrolment();
                    Parent parent = new Parent();
                    parent.CNIC = workSheet.Cells[i, 13].Text;
                    enrolment.GRNo = workSheet.Cells[i, 2].Text;
                    List<string> name = workSheet.Cells[i, 3].Text.Split(' ').ToList();
                    student.FirstName = name.FirstOrDefault();
                    student.LastName = name.Skip(1).Aggregate<string, string>("", (a, b) => a + " " + b).Trim();
                    if (studentdb.Where(x => x.FirstName == student.FirstName &&
                    x.LastName == student.LastName && x.Parent.CNIC == parent.CNIC).Count() > 0)
                    {
                        student = studentdb.Where(x => x.FirstName == student.FirstName &&
                    x.LastName == student.LastName && x.Parent.CNIC == parent.CNIC).FirstOrDefault();
                        student.UpdatedBy = userid;
                        student.UpdatedDate = DateTime.Now;

                        enrolment = enrolments.Where(x => x.GRNo == enrolment.GRNo).FirstOrDefault();
                        enrolment.UpdatedBy = userid;
                        enrolment.UpdatedDate = DateTime.Now;

                        parent = parents.Where(x => x.ParentId == student.Father).FirstOrDefault();
                        parent.UpdatedBy = userid;
                        parent.UpdatedDate = DateTime.Now;

                    }
                    else
                    {
                        student.CreatedDate = DateTime.Now;
                        student.CreatedBy = userid;
                        enrolment.CreatedDate = DateTime.Now;
                        enrolment.CreatedBy = userid;
                        if (parents.Where(x => x.CNIC.Replace("-", "").Replace(" ", "") == parent.CNIC.Replace("-", "").Replace(" ", "")).Count() > 0)
                        {
                            parent = parents.Where(x => x.CNIC.Replace("-", "").Replace(" ", "") == parent.CNIC.Replace("-", "").Replace(" ", "")).FirstOrDefault();
                            parent.UpdatedBy = userid;
                            parent.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            parent.CreatedDate = DateTime.Now;
                            parent.CreatedBy = userid;
                        }

                    }
                    parent.Name = workSheet.Cells[i, 4].Text;
                    parent.Relation = "Father";
                    if (workSheet.Cells[i, 5].Text != "")
                        student.FamilyCode = Convert.ToInt32(workSheet.Cells[i, 5].Text);
                    student.Gender = Common.FirstCharToUpper(workSheet.Cells[i, 6].Text);
                    DateTime dt;
                    if (DateTime.TryParse(workSheet.Cells[i, 7].Text, out dt))
                    {
                        student.DateOfBirth = dt;
                        student.Age = (int)(DateTime.Now - dt).TotalDays / 365;
                    }
                    if (DateTime.TryParse(workSheet.Cells[i, 8].Text, out dt))
                        student.JoiningDate = dt;
                    student.Mobile = "+92" + workSheet.Cells[i, 9].Text;
                    parent.Profession = workSheet.Cells[i, 10].Text;
                    parent.OfficeAddress = workSheet.Cells[i, 11].Text;
                    parent.Education = workSheet.Cells[i, 12].Text;
                    parent.OfficePhone = workSheet.Cells[i, 14].Text;
                    parent.Email = workSheet.Cells[i, 16].Text;
                    parent.MonthlyIncome = workSheet.Cells[i, 17].Text;
                    parent.Mobile = "+92" + workSheet.Cells[i, 18].Text;
                    if (workSheet.Cells[i, 19].Text != "")
                        enrolment.RollNo = Convert.ToInt32(workSheet.Cells[i, 19].Text);
                    var classadmitted = classes.Where(x => x.ClassName.ToUpper()
                    .Contains(workSheet.Cells[i, 21].Text.Replace(".", "").ToUpper())).FirstOrDefault();
                    if (classadmitted != null)
                        student.ClassAdmitted = classadmitted.ClassId;
                    var section = workSheet.Cells[i, 23].Text.ToUpper();
                    var currentclass = classes.Where(x => x.ClassName.ToUpper()
                    .Contains(workSheet.Cells[i, 22].Text.Replace(".", "").ToUpper()) &&
                    x.Section.ToUpper() == section).FirstOrDefault();
                    if (currentclass != null)
                        enrolment.Class = currentclass.ClassId;
                    if (workSheet.Cells[i, 25].Text != "")
                        enrolment.Fee = Convert.ToDecimal(workSheet.Cells[i, 25].Text);
                    if (workSheet.Cells[i, 26].Text.ToLower() == "present".ToLower())
                    {
                        enrolment.IsActive = true;
                    }
                    else
                    {
                        student.EndDate = DateTime.Today;
                    }
                    db.Parents.AddOrUpdate(parent);
                    db.SaveChanges();
                    student.Father = parent.ParentId;
                    db.Students.AddOrUpdate(student);
                    db.SaveChanges();
                    enrolment.Student = student.StudentId;
                    db.Enrolments.AddOrUpdate(enrolment);

                }

            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessDefaultersData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[6];
            var arrearsdb = db.Arrears.ToList();
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
                    Arrear arr = new Arrear();
                    string grno = workSheet.Cells[i, 2].Text;
                    int studentId = db.Enrolments.Where(x => x.GRNo == grno && x.IsActive.Value).FirstOrDefault().Student.Value;
                    arr.StudentId = studentId;
                    if (arrearsdb.Where(x => x.StudentId == arr.StudentId).Count() > 0)
                    {
                        arr = arrearsdb.Where(x => x.StudentId == arr.StudentId).FirstOrDefault();
                        arr.UpdatedBy = userid;
                        arr.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        arr.CreatedDate = DateTime.Now;
                        arr.CreatedBy = userid;
                    }
                    arr.Amount = Convert.ToDecimal(workSheet.Cells[i, 3].Text);
                    arr.ArrearType = "Receivable";
                    db.Arrears.AddOrUpdate(arr);
                }
            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessClassTeachersData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[7];
            var classesdb = db.Classes.ToList();
            var staffdb = db.Staffs.ToList();
            var userid = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            for (int i = workSheet.Dimension.Start.Row;
                     i <= workSheet.Dimension.End.Row;
                     i++)
            {
                string cellValue = workSheet.Cells[i, 1].Text;
                int temp = 0;

                if (int.TryParse(cellValue, out temp))
                {
                    string className = workSheet.Cells[i, 2].Text;
                    string section = workSheet.Cells[i, 3].Text;
                    string teacherName = workSheet.Cells[i, 4].Text;
                    var _class = classesdb.Where(x => x.ClassName.ToLower() == className.ToLower() &&
                    x.Section.ToLower() == section.ToLower()).FirstOrDefault();
                    var teacher = staffdb.Where(x => x.FirstName.ToLower() + " " + x.LastName.ToLower() == teacherName.ToLower()).FirstOrDefault();

                    if (_class != null && teacher != null)
                    {
                        _class.ClassAdvisor = teacher.StaffId;
                        _class.UpdatedBy = userid;
                        _class.UpdatedDate = DateTime.Now;
                        db.Classes.AddOrUpdate(_class);
                    }
                }
            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessFeesData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[8];
            var feedb = db.PaidFees.ToList();
            var enrolmentsdb = db.Enrolments.ToList();
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
                    PaidFee fee = new PaidFee();
                    string grno = workSheet.Cells[i, 2].Text;
                    var stdid = enrolmentsdb.Where(x => x.GRNo == grno && x.IsActive.Value).FirstOrDefault().Student.Value;
                    fee.StudentId = stdid;
                    
                    fee.Month = Constants.months2[workSheet.Cells[i, 4].Text];
                    var year = Convert.ToInt32( workSheet.Cells[i, 5].Text);
                    var date = Convert.ToInt32(workSheet.Cells[i, 6].Text);
                    fee.PaymentDate = new DateTime(year, fee.Month.Value, date);
                    var listtemp = feedb.Where(x => x.StudentId == fee.StudentId && x.Month == fee.Month &&
                        x.PaymentDate.Value.Year == fee.PaymentDate.Value.Year);
                    if (listtemp.Count() > 0)
                    {
                        fee = listtemp.FirstOrDefault();
                        fee.UpdatedBy = userid;
                        fee.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        fee.CreatedDate = DateTime.Now;
                        fee.CreatedBy = userid;
                    }
                    fee.ChallanNo = db.PaidFees.Select(x => x.ChallanNo).Max();
                    if (!fee.ChallanNo.HasValue)
                        fee.ChallanNo = 1;
                    else
                        fee.ChallanNo = fee.ChallanNo + 1;
                    fee.CalculatedAmount = Convert.ToDecimal(workSheet.Cells[i, 3].Text);
                    fee.ReceivedAmount = fee.CalculatedAmount;
                    db.PaidFees.AddOrUpdate(fee);
                    db.SaveChanges();
                }

            }
            db.SaveChanges();
            return true;
        }
        private bool ProcessSalariesData(string path)
        {
            var package = new ExcelPackage(new FileInfo(path));
            //List<ExcelRange> list = new List<ExcelRange>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[9];
            var salarydb = db.SalaryPayments.ToList();
            var staffdb = db.Staffs.ToList();
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
                    SalaryPayment salary = new SalaryPayment();
                    string cnic = workSheet.Cells[i, 3].Text;
                    var staffid = staffdb.Where(x => x.CNIC == cnic).FirstOrDefault().StaffId;
                    salary.StaffId = staffid;

                    salary.Month = Constants.months2[workSheet.Cells[i, 5].Text];
                    var year = Convert.ToInt32(workSheet.Cells[i, 6].Text);
                    salary.PaymentDate = new DateTime(year, salary.Month.Value, 1);
                    var listtemp = salarydb.Where(x => x.StaffId == salary.StaffId && x.Month == salary.Month &&
                        x.PaymentDate.Value.Year == salary.PaymentDate.Value.Year);
                    if (listtemp.Count() > 0)
                    {
                        salary = listtemp.FirstOrDefault();
                        salary.UpdatedBy = userid;
                        salary.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        salary.CreatedDate = DateTime.Now;
                        salary.CreatedBy = userid;
                    }
                    salary.Amount = Convert.ToDecimal(workSheet.Cells[i, 4].Text);
                    db.SalaryPayments.AddOrUpdate(salary);
                }

            }
            db.SaveChanges();
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
