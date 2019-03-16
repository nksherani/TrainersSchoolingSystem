using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Models.DTOs;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize]
    public class SMSController : Controller
    {
        TrainersEntities db = new TrainersEntities();
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
            //action = ip
            //class = msg
            List<int> StudentIDs = bulk.Ids.Select(x => Convert.ToInt32(x)).ToList();
            StudentIDs.Sort();
            List<Student> students = db.Students.Where(x => StudentIDs.Contains(x.StudentId)).ToList();
            //var mobileNumbers = db.Students.Join(db.Parents, a => a.Father, b => b.ParentId, (a, b) => b).ToList();

            var builder = new UriBuilder($"http://{bulk.Action}/v1/sms/send/");
            builder.Port = 8080;
            Dictionary<int, System.Threading.Tasks.Task<HttpResponseMessage>> responses = new Dictionary<int, System.Threading.Tasks.Task<HttpResponseMessage>>();
            foreach (Student item in students)
            {
                var msg = bulk.Class;
                msg = msg.Replace("_STUDENT_", item.FirstName);
                var query = HttpUtility.ParseQueryString(builder.Query);
                if (item.Parent != null)
                {
                    query["phone"] = item.Parent.Mobile;
                    msg = msg.Replace("_PARENT_", item.Parent.Name);
                }
                else if (item.Parent1 != null)
                {
                    query["phone"] = item.Parent1.Mobile;
                    msg = msg.Replace("_PARENT_", item.Parent1.Name);
                }
                else if (item.Parent2 != null)
                {
                    query["phone"] = item.Parent2.Mobile;
                    msg = msg.Replace("_PARENT_", item.Parent2.Name);
                }
                query["message"] = msg;

                builder.Query = query.ToString();
                string url = builder.ToString();
                HttpClient client = new HttpClient();
                var response = client.GetAsync(url);
                responses.Add(item.StudentId, response);
            }

            return Json(responses, JsonRequestBehavior.AllowGet);
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
