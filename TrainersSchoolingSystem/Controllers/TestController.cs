using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Models.DTOs;

namespace TrainersSchoolingSystem.Controllers
{
    [AllowAnonymous]
    public class TestController : Controller
    {
        TrainersEntities db = new TrainersEntities();
        public TestController()
        {
            if (!SiteMapManager.SiteMaps.ContainsKey("TMXMAP"))
            {
                SiteMapManager.SiteMaps.Register<XmlSiteMap>("TMXMAP", sitmap => sitmap.LoadFrom("~/Content/TMX.sitemap"));
            }
        }
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FeeSlip()
        {

            return View();
        }
        public string AddRoles(string rolename)
        {
            AspNetRole role = new AspNetRole();
            role.Id = Guid.NewGuid().ToString();
            role.Name = rolename;
            var user = db.AspNetUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            role.AspNetUsers.Add(user);
            db.AspNetRoles.Add(role);
            db.SaveChanges();
            return $"Roles {rolename} has been added successfully";
        }
        public string Mapper()
        {
            StudentViewModel svm = new StudentViewModel();
            Student std = StudentViewModel.ToEntity(svm);
            StudentViewModel svm2 = StudentViewModel.ToModel(std);
            return "success";
        }
        
        
        // GET: Test/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Test/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Test/Create
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

        // GET: Test/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Test/Edit/5
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

        // GET: Test/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Test/Delete/5
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
