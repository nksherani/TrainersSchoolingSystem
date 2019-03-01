using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
//using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Models.DTOs;
using TrainersSchoolingSystem.Utils;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SetupController : Controller
    {
        TrainersEntities db = new TrainersEntities();
        
        // GET: Setup
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error(string error)
        {
            ViewBag.Error = error;
            return View();
        }

        // GET: Setup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Setup/Create
        public ActionResult Create()
        {
            ViewBag.FirstMonth = new SelectList(Constants.months, "Key", "Value");

            return View();
        }

        // POST: Setup/Create
        [HttpPost]
        public ActionResult Create(ConfigurationViewModel configuration)
        {
            try
            {
                if (ModelState.IsValid && db.Configurations.Count() == 0)
                {
                    var file = configuration.File;
                    if (file != null)
                    {
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Content/Images"), pic);
                        configuration.Picture = "../Content/Images/" + pic;
                        // file is uploaded
                        file.SaveAs(path);
                    }

                    var properties = configuration.GetType().GetProperties();
                    int i = 1;
                    foreach (var property in properties)
                    {
                        if (property.Name == "File")
                            continue;
                        Configuration config = new Configuration();
                        //config.ConfigurationId = i++;
                        config.Key = property.Name;
                        config.Value = (string)property.GetValue(configuration);
                        config.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                        config.CreatedDate = DateTime.Now;
                        db.Configurations.Add(config);
                    }

                    db.SaveChanges();
                    InitLookups();
                    GlobalData.RefreshConfiguration();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.FirstMonth = new SelectList(Constants.months, "Key", "Value");
                    return View();
                }

            }
            catch
            {
                ViewBag.FirstMonth = new SelectList(Constants.months, "Key", "Value");
                return View();
            }
        }
        public string InitLookups()
        {
            //LookupType lookupType1 = new LookupType();
            //lookupType1.LookupTypeName = "Designation";
            //lookupType1.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            //lookupType1.CreatedDate = DateTime.Now;
            //db.LookupTypes.Add(lookupType1);

            LookupType lookupType2 = new LookupType();
            lookupType2.LookupTypeName = "Gender";
            lookupType2.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookupType2.CreatedDate = DateTime.Now;
            db.LookupTypes.Add(lookupType2);

            LookupType lookupType3 = new LookupType();
            lookupType3.LookupTypeName = "Section";
            lookupType3.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookupType3.CreatedDate = DateTime.Now;
            db.LookupTypes.Add(lookupType3);

            LookupType lookupType4 = new LookupType();
            lookupType4.LookupTypeName = "Category";
            lookupType4.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookupType4.CreatedDate = DateTime.Now;
            db.LookupTypes.Add(lookupType4);

            LookupType lookupType5 = new LookupType();
            lookupType5.LookupTypeName = "Level";
            lookupType5.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookupType5.CreatedDate = DateTime.Now;
            db.LookupTypes.Add(lookupType5);

            db.SaveChanges();

            Lookup lookup = new Lookup();
            lookup.LookupTypeId = lookupType2.LookupTypeId;
            lookup.LookupText = "Male";
            lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookup.CreatedDate = DateTime.Now;
            db.Lookups.Add(lookup);
            lookup = new Lookup();
            lookup.LookupTypeId = lookupType2.LookupTypeId;
            lookup.LookupText = "Female";
            lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookup.CreatedDate = DateTime.Now;
            db.Lookups.Add(lookup);

            lookup = new Lookup();
            lookup.LookupTypeId = lookupType3.LookupTypeId;
            lookup.LookupText = "A";
            lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookup.CreatedDate = DateTime.Now;
            db.Lookups.Add(lookup);
            lookup = new Lookup();
            lookup.LookupTypeId = lookupType3.LookupTypeId;
            lookup.LookupText = "B";
            lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookup.CreatedDate = DateTime.Now;
            db.Lookups.Add(lookup);

            //lookup = new Lookup();
            //lookup.LookupTypeId = lookupType1.LookupTypeId;
            //lookup.LookupText = "Teacher";
            //lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            //lookup.CreatedDate = DateTime.Now;
            //db.Lookups.Add(lookup);

            lookup = new Lookup();
            lookup.LookupTypeId = lookupType4.LookupTypeId;
            lookup.LookupText = "Teaching";
            lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookup.CreatedDate = DateTime.Now;
            db.Lookups.Add(lookup);

            lookup = new Lookup();
            lookup.LookupTypeId = lookupType4.LookupTypeId;
            lookup.LookupText = "Non-Teaching";
            lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookup.CreatedDate = DateTime.Now;
            db.Lookups.Add(lookup);

            for (int i = 0; i < 13; i++)
            {
                lookup = new Lookup();
                lookup.LookupTypeId = lookupType5.LookupTypeId;
                lookup.LookupText = (i+1).ToString();
                lookup.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
                lookup.CreatedDate = DateTime.Now;
                db.Lookups.Add(lookup);
            }
            Designation designation = new Designation();
            designation.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            designation.CreatedDate = DateTime.Now;
            designation.DesignationName = "Teacher";
            designation.LateComingScale = 3;
            designation.PaidLeaves = 13;
            designation.ShortLeavesScale = 2;
            db.Designations.Add(designation);

            db.SaveChanges();

            Staff staff = new Staff();
            staff.Age = 20;
            staff.Category = "Teaching";
            staff.City = "Karachi";
            staff.DateOfBirth = DateTime.Now.AddYears(-20);
            staff.Designation = designation.DesignationId;
            staff.FatherName = "ffdgdg";
            staff.FirstName = "fname";
            staff.LastName = "lname";
            staff.Mobile = "+923314567896";
            staff.PostalCode = "745";
            staff.StreetAddress = "ffdgdg";

            db.Staffs.Add(staff);
            db.SaveChanges();
            return "success";
        }
        [AllowAnonymous]
        public string ResetDb()
        {
            db.Database.ExecuteSqlCommand("exec ResetDb");
            if (User.Identity.IsAuthenticated)
                new AccountController().LogOff();
            return "success";
        }
        // GET: Setup/Edit/5
        public ActionResult Edit()
        {
            ConfigurationViewModel configuration = new ConfigurationViewModel();
            var configurationdb = db.Configurations.ToList();
            var properties = configuration.GetType().GetProperties();
            int i = 1;
            foreach (var property in properties)
            {
                if (property.Name == "File")
                    continue;
                property.SetValue(configuration, configurationdb.Where(x => x.Key == property.Name).FirstOrDefault().Value);
            }
            List<KeyValuePair<int, string>> Months = new List<KeyValuePair<int, string>>();
            Months.Add(new KeyValuePair<int, string>(1, "January"));
            Months.Add(new KeyValuePair<int, string>(2, "February"));
            Months.Add(new KeyValuePair<int, string>(3, "March"));
            Months.Add(new KeyValuePair<int, string>(4, "April"));
            Months.Add(new KeyValuePair<int, string>(5, "May"));
            Months.Add(new KeyValuePair<int, string>(6, "June"));
            Months.Add(new KeyValuePair<int, string>(7, "July"));
            Months.Add(new KeyValuePair<int, string>(8, "August"));
            Months.Add(new KeyValuePair<int, string>(9, "September"));
            Months.Add(new KeyValuePair<int, string>(10, "October"));
            Months.Add(new KeyValuePair<int, string>(11, "November"));
            Months.Add(new KeyValuePair<int, string>(12, "December"));

            ViewBag.FirstMonth = new SelectList(Months, "Key", "Value", Months[Convert.ToInt32(configuration.FirstMonth)]);
            return View(configuration);
        }
        [HttpPost]
        public string Temp(HttpPostedFileBase file)
        {
            string path = "";
            string pic = "";
            var path_ = Server.MapPath("~/Content/Temp");
            if(Directory.Exists(path_))
            {
                var files = System.IO.Directory.EnumerateFiles(path_);
                foreach (var item in files)
                {
                    if (System.IO.File.Exists(item))
                    {
                        System.IO.File.Delete(item);
                    }
                }
            }
            

            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                path = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/Temp"), pic);
                // file is uploaded
                file.SaveAs(path);
            }
            return $"../Content/Temp/{pic}";
        }
        // POST: Setup/Edit/5
        [HttpPost]
        public ActionResult Edit(ConfigurationViewModel configuration)
        {
            try
            {
                var configdb = db.Configurations.ToList();
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var file = configuration.File;
                    if (file != null)
                    {
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Content/Images"), pic);
                        configuration.Picture = "../Content/Images/" + pic;
                        // file is uploaded
                        file.SaveAs(path);
                    }

                    var properties = configuration.GetType().GetProperties();
                    int i = 1;
                    foreach (var property in properties)
                    {
                        if (property.Name == "File")
                            continue;
                        Configuration config = configdb.Where(x => x.Key == property.Name).FirstOrDefault();
                        //config.ConfigurationId = i++;
                        config.Key = property.Name;
                        config.Value = (string)property.GetValue(configuration);
                        config.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                        config.UpdatedDate = DateTime.Now;
                        db.Configurations.AddOrUpdate(config);
                    }

                    db.SaveChanges();
                    GlobalData.RefreshConfiguration();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    List<KeyValuePair<int, string>> Months = new List<KeyValuePair<int, string>>();
                    Months.Add(new KeyValuePair<int, string>(1, "January"));
                    Months.Add(new KeyValuePair<int, string>(2, "February"));
                    Months.Add(new KeyValuePair<int, string>(3, "March"));
                    Months.Add(new KeyValuePair<int, string>(4, "April"));
                    Months.Add(new KeyValuePair<int, string>(5, "May"));
                    Months.Add(new KeyValuePair<int, string>(6, "June"));
                    Months.Add(new KeyValuePair<int, string>(7, "July"));
                    Months.Add(new KeyValuePair<int, string>(8, "August"));
                    Months.Add(new KeyValuePair<int, string>(9, "September"));
                    Months.Add(new KeyValuePair<int, string>(10, "October"));
                    Months.Add(new KeyValuePair<int, string>(11, "November"));
                    Months.Add(new KeyValuePair<int, string>(12, "December"));

                    ViewBag.FirstMonth = new SelectList(Months, "Key", "Value", Convert.ToInt32(configuration.FirstMonth));
                    return View();
                }
            }
            catch
            {
                List<KeyValuePair<int, string>> Months = new List<KeyValuePair<int, string>>();
                Months.Add(new KeyValuePair<int, string>(1, "January"));
                Months.Add(new KeyValuePair<int, string>(2, "February"));
                Months.Add(new KeyValuePair<int, string>(3, "March"));
                Months.Add(new KeyValuePair<int, string>(4, "April"));
                Months.Add(new KeyValuePair<int, string>(5, "May"));
                Months.Add(new KeyValuePair<int, string>(6, "June"));
                Months.Add(new KeyValuePair<int, string>(7, "July"));
                Months.Add(new KeyValuePair<int, string>(8, "August"));
                Months.Add(new KeyValuePair<int, string>(9, "September"));
                Months.Add(new KeyValuePair<int, string>(10, "October"));
                Months.Add(new KeyValuePair<int, string>(11, "November"));
                Months.Add(new KeyValuePair<int, string>(12, "December"));

                ViewBag.FirstMonth = new SelectList(Months, "Key", "Value");
                return View();
            }
        }

        // GET: Setup/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Setup/Delete/5
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
