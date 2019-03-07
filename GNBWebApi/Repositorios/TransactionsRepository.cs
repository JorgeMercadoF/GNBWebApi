using GNBWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GNBWebApi.Repositorios
{
    public class TransactionsRepository
    {
        private readonly string url = @"http://quiet-stone-2094.herokuapp.com/transactions.json";
        private readonly string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\transactions.json");
        public List<Transactions> Transactions = new List<Transactions>();

        public List<Transactions> GetJsonUrlTransactions()
        {
            using (WebClient httpClient = new WebClient())
            {
                var jsonData = httpClient.DownloadString(url);
                using (StreamWriter archivo = new StreamWriter(ruta, false))
                {
                    archivo.Write(jsonData);
                }
                Transactions = JsonConvert.DeserializeObject<List<Transactions>>(jsonData);
                return Transactions;
            }
        }

        public List<Transactions> GetJsonTextTransactions()
        {
            Transactions = JsonConvert.DeserializeObject<List<Transactions>>(System.IO.File.ReadAllText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\Transactions.json")));
            return Transactions;
        }
    }
}