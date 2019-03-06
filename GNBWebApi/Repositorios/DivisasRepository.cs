using GNBWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNBWebApi.Repositorios
{
    public class DivisasRepository
    {
        public async Task<List<Divisas>> JsonDivisas()
        {
            using (var Clientes = new System.Net.Http.HttpClient())
            {
                List<Divisas> Divisas = new List<Divisas>();
                string url = @"http://quiet-stone-2094.herokuapp.com/rates.json";
                var uri = new Uri(url);

                var response = await Clientes.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Divisas = JsonConvert.DeserializeObject<List<Divisas>>(content);

                    return Divisas;
                }
                return null;
            }

        }
    }
}