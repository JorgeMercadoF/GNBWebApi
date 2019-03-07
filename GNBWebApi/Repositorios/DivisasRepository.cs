using GNBWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GNBWebApi.Repositorios
{
    public class DivisasRepository
    {
        private readonly string url = @"http://quiet-stone-2094.herokuapp.com/rates.json";
        private readonly string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\divisas.json");
        public List<Divisas> Divisas = new List<Divisas>();

        public List<Divisas> GetJsonUrlDivisas()
        {
            using (WebClient httpClient = new WebClient())
            {
                var jsonData = httpClient.DownloadString(url);
                using (StreamWriter archivo = new StreamWriter(ruta, false))
                {
                    archivo.Write(jsonData);
                }
                Divisas = JsonConvert.DeserializeObject<List<Divisas>>(jsonData);
                return Divisas;
            }
        }

        public List<Divisas> GetJsonTextDivisas()
        {
            Divisas = JsonConvert.DeserializeObject<List<Divisas>>(System.IO.File.ReadAllText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\divisas.json")));
            return Divisas;
        }
    }
}