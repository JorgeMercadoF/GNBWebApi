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
        public DivisasRepository db = new DivisasRepository();
        public List<Divisas> Divisas = new List<Divisas>();

        // GET: api/Divisas
        public async Task<JsonResult<List<Divisas>>> Get()
        {
            Divisas = await db.JsonDivisas();
            return Json(Divisas);
        }

        //GET: api/Divisas/5
        public async Task<JsonResult<List<Divisas>>> Get(string from, string to)
        {
            Divisas = await db.JsonDivisas();
            List<Divisas> selec = new List<Divisas>();
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
                selec.Clear();
                foreach (var divisaFrom in divisasFrom)
                {
                    var divisasTo = (from x in Divisas
                                     where x.From.Equals(divisaFrom.To) && x.To.Equals(to)
                                     select x).ToList();
                    if (divisasTo != null && divisasTo.Count() > 0)
                    {
                        selec.Add(divisaFrom);
                        selec.AddRange(divisasTo);
                    }
                }
                if(selec.Count() > 0)
                {
                    return Json(selec);
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
