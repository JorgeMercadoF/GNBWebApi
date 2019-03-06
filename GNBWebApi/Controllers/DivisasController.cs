using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GNBWebApi.Controllers
{
    public class DivisasController : ApiController
    {
        // GET: api/Divisas
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Divisas/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Divisas
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Divisas/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Divisas/5
        public void Delete(int id)
        {
        }
    }
}
