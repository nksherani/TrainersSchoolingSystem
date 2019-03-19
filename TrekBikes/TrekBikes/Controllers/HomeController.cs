using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrekBikes.Models;

namespace TrekBikes.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var url = "https://trekhiringassignments.blob.core.windows.net/interview/bikes.json";
            Dictionary<string, int> dict = new Dictionary<string, int>(); 

            string data = GET(url);
            data = data.Replace("\r", "").Replace("\n", "");
            var list = JsonConvert.DeserializeObject<List<SurveyObject>>(data);
            foreach (var item in list)
            {
                var key = item.bikes.Aggregate<string, string>("", (a, b) => a +", "+ b);
                if (dict.ContainsKey(key))
                {
                    dict[key] += 1;
                }
                else
                {
                    dict.Add(key, 1);
                }
            }
            var sorted = dict.OrderByDescending(x => x.Value).ToList();
            var Top20 = sorted.Take(20).ToList();
            return View(Top20);
        }
        // Returns JSON string
        string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
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