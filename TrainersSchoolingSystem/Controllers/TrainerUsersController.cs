using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Models;

namespace TrainersSchoolingSystem.Controllers
{
    public class TrainerUsersController : Controller
    {
        private TrainersEntities db = new TrainersEntities();
        private ApplicationUserManager _userManager;
        public TrainerUsersController()
        {

        }
        public TrainerUsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: TrainerUsers
        public ActionResult Index()
        {
            return View(db.TrainerUsers.ToList());
        }

        // GET: TrainerUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainerUser trainerUser = db.TrainerUsers.Find(id);
            if (trainerUser == null)
            {
                return HttpNotFound();
            }
            return View(trainerUser);
        }

        // GET: TrainerUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainerUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TrainerUserId,Username,FirstName,LastName,Email,Mobile,Landline,Address,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] TrainerUser trainerUser)
        {
            if (ModelState.IsValid)
            {
                trainerUser.CreatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                trainerUser.CreatedDate = DateTime.Now;
                db.TrainerUsers.Add(trainerUser);
                db.SaveChanges();
                var user = new ApplicationUser { UserName = trainerUser.Username, Email = trainerUser.Email };
                var result = await UserManager.CreateAsync(user, trainerUser.Username);
                return RedirectToAction("Index");
            }

            return View(trainerUser);
        }

        // GET: TrainerUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainerUser trainerUser = db.TrainerUsers.Find(id);
            if (trainerUser == null)
            {
                return HttpNotFound();
            }
            return View(trainerUser);
        }

        // POST: TrainerUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainerUserId,Username,FirstName,LastName,Email,Mobile,Landline,Address,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] TrainerUser trainerUser)
        {
            if (ModelState.IsValid)
            {
                var dbuser = db.TrainerUsers.Where(x => x.Username == trainerUser.Username).FirstOrDefault();
                dbuser.UpdatedBy = db.TrainerUsers.Where(x => x.Username.ToString() == User.Identity.Name.ToString()).FirstOrDefault().TrainerUserId;
                dbuser.UpdatedDate = DateTime.Now;
                dbuser.FirstName = trainerUser.FirstName;
                dbuser.LastName = trainerUser.LastName;
                dbuser.Mobile = trainerUser.Mobile;
                dbuser.Landline = trainerUser.Landline;
                dbuser.Address = trainerUser.Address;
                db.TrainerUsers.AddOrUpdate(dbuser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainerUser);
        }

        // GET: TrainerUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainerUser trainerUser = db.TrainerUsers.Find(id);
            if (trainerUser == null)
            {
                return HttpNotFound();
            }
            return View(trainerUser);
        }

        // POST: TrainerUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainerUser trainerUser = db.TrainerUsers.Find(id);
            db.TrainerUsers.Remove(trainerUser);
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
