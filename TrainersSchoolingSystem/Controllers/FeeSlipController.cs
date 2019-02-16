using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrainersSchoolingSystem.Models;

namespace TrainersSchoolingSystem.Controllers
{
    public class FeeSlipController : ApiController
    {
        // GET: api/FeeSlip
        TrainersEntities db = new TrainersEntities();
        public IEnumerable<string> Get()
        {
            var students = db.Students.ToList();
            var enrolments = db.Enrolments.ToList();
            
            return new string[] { "value1", "value2" };
        }

        // GET: api/FeeSlip/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/FeeSlip
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/FeeSlip/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FeeSlip/5
        public void Delete(int id)
        {
        }
    }
}
