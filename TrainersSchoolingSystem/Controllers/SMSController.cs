using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models.DTOs;

namespace TrainersSchoolingSystem.Controllers
{
    public class SMSController : Controller
    {
        // GET: SMS
        public ActionResult Index()
        {
            return View();
        }
        // GET: SMS/Details/5
        public ActionResult Send()
        {
            return View();
        }
        // GET: SMS/Details/5
        [HttpPost]
        public ActionResult Send(SMS msg_)
        {
            var builder = new UriBuilder(msg_.url);
            builder.Port = 8080;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["phone"] = msg_.number;
            query["message"] = msg_.Message;
            builder.Query = query.ToString();
            string url = builder.ToString();


            HttpClient client = new HttpClient();
            client.GetAsync(url);
            return View();
        }
        public ActionResult SendBulk(Bulk bulk)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

            // GET: SMS/Create
            public ActionResult Create()
        {
            return View();
        }

        // POST: SMS/Create
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

        // GET: SMS/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SMS/Edit/5
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

        // GET: SMS/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SMS/Delete/5
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
