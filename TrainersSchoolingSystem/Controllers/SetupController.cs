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
            return View();
        }

        // POST: Setup/Create
        [HttpPost]
        public ActionResult Create(ConfigurationViewModel configuration)
        {
            try
            {
                if (ModelState.IsValid && db.Configurations.Count()==0)
                {
                    var properties = configuration.GetType().GetProperties();
                    int i = 1;
                    foreach (var property in properties)
                    {
                        Configuration config = new Configuration();
                        config.ConfigurationId = i++;
                        config.Key = property.Name;
                        config.Value = (string)property.GetValue(configuration);
                        config.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                        config.CreatedDate = DateTime.Now;
                        db.Configurations.Add(config);
                    }
                    
                    db.SaveChanges();
                    InitLookups();
                    return RedirectToAction("Index", "Home");
                }
                else
                    return View();
            }
            catch
            {
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
        // GET: Setup/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Setup/Edit/5
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
