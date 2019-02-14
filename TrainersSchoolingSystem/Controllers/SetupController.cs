using Kendo.Mvc;
using System;
using System.Collections.Generic;
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
        public SetupController()
        {
            if (!SiteMapManager.SiteMaps.ContainsKey("TMXMAP"))
            {
                SiteMapManager.SiteMaps.Register<XmlSiteMap>("TMXMAP", sitmap => sitmap.LoadFrom("~/Content/TMX.sitemap"));
            }
        }
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
        public string InitLookups()
        {
            LookupType lookupType1 = new LookupType();
            lookupType1.LookupTypeName = "Designation";
            lookupType1.CreatedBy = db.TrainerUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault().TrainerUserId;
            lookupType1.CreatedDate = DateTime.Now;
            db.LookupTypes.Add(lookupType1);

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
            return "success";
        }
        public string ResetDb()
        {
            db.Database.ExecuteSqlCommand("exec ResetDb");
            return "success";
        }
        // GET: Setup/Edit/5
        public ActionResult Edit(int id)
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

        // POST: Setup/Edit/5
        [HttpPost]
        public ActionResult Edit(ConfigurationViewModel collection)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
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

                    ViewBag.FirstMonth = new SelectList(Months, "Key", "Value");
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
