using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainersSchoolingSystem.Utils;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            RecurringJob.AddOrUpdate(() => scheduleBackup(), Cron.Monthly);
            return View();
        }
        public void scheduleBackup()
        {
            Logger.Debug("Starts Backup!");
            BackupService bkp = new BackupService();
            bkp.BackupDatabase("Trainers");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}