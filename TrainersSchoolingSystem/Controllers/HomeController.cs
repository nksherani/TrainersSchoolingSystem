using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainersSchoolingSystem.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class HomeController : Controller
    {
        public HomeController()
        {
            if (!SiteMapManager.SiteMaps.ContainsKey("TMXMAP"))
            {
                SiteMapManager.SiteMaps.Register<XmlSiteMap>("TMXMAP", sitmap => sitmap.LoadFrom("~/Content/TMX.sitemap"));
            }
        }
        public ActionResult Index()
        {
            
            return View();
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