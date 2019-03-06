using GNBWebApi.Models;
using GNBWebApi.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace GNBWebApi.Controllers
{
    public class DivisasController : ApiController
    {
        private DivisasRepository db = new DivisasRepository();
        private List<Divisas> Divisas = new List<Divisas>();
        private List<Divisas> Conversion = new List<Divisas>();

        public DivisasController()
        {
            Divisas = db.GetJsonUrlDivisas();
            if (Divisas == null)
            {
                Divisas = db.GetJsonTextDivisas();
            }
        }

        // GET: api/Divisas
        public JsonResult<List<Divisas>> Get()
        {
            return Json(Divisas);
        }

        //GET: api/Divisas/5
        public JsonResult<List<Divisas>> Get(string from, string to)
        {
            var divisasFromTo = (from x in Divisas
                           where x.From.Equals(@from) && x.To.Equals(to)
                           select x).ToList();
            if (divisasFromTo != null && divisasFromTo.Count() > 0)
            {
                return Json(divisasFromTo);
            }
            else
            {
                var divisasFrom = (from x in Divisas
                                   where x.From.Equals(@from)
                                   select x).ToList();
                Conversion.Clear();
                foreach (var divisaFrom in divisasFrom)
                {
                    var divisasTo = (from x in Divisas
                                     where x.From.Equals(divisaFrom.To) && x.To.Equals(to)
                                     select x).ToList();
                    if (divisasTo != null && divisasTo.Count() > 0)
                    {
                        Conversion.Add(divisaFrom);
                        Conversion.AddRange(divisasTo);
                    }
                }
                if(Conversion.Count() > 0)
                {
                    return Json(Conversion);
                }
                else
                {
                    return null;
                }
            }
        }

        // GET api/HelloWorld2  
        //[Route("api/Divisas/Hello2")]
        //public string GetHello2(string frase){
        //    return frase;
        //}

    }
}
