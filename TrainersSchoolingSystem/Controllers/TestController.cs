using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        
        // GET: Test
        public ActionResult Index()
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

        public ActionResult GetFeeSlipData()
        {
            List<FeeSlipModel> list = new List<FeeSlipModel>();
            for (int i = 0; i < 500; i++)
            {
                SqlParameter StudentId = new SqlParameter("@StudentId", 6);
                //SqlParameter endDate = new SqlParameter("@endDate", "Value");
                //db.Database.SqlQuery<yourObjectNameToCAST>("exec yourStoreProcedureName @startDate, @endDate", startDate, endDate).ToList();
                var data = db.Database.SqlQuery<FeeSlipModel>("exec GenerateFeeSlips @StudentId", StudentId).FirstOrDefault();
                list.Add(data);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
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
