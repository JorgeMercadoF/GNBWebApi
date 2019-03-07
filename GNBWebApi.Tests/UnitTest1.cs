using GNBWebApi.Controllers;
using GNBWebApi.Models;
using GNBWebApi.Repositorios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GNBWebApi.Tests
{
    [TestClass]
    public class TestRepositorios
    {
        //Preparacion
        DivisasRepository dbD = new DivisasRepository();
        TransactionsRepository dbT = new TransactionsRepository();
        List<Divisas> Divisas = new List<Divisas>();
        List<Transactions> Transactions = new List<Transactions>();

        [TestMethod]
        public void GetJsonFromURL()
        {
            //Ejecucion y Resultado
            Divisas.Clear();
            Transactions.Clear();
            Divisas = dbD.GetJsonUrlDivisas();
            Transactions = dbT.GetJsonUrlTransactions();
            if (Divisas == null || Divisas.Count() == 0)
            {
                Console.Write("No se ha podido leer la URL del JSON de Divisas");
            }
            if (Transactions == null || Transactions.Count() == 0)
            {
                Console.Write("No se ha podido leer la URL del JSON de Transacciones");
            }
        }

        [TestMethod]
        public void GetJsonFromFile()
        {
            Divisas.Clear();
            Transactions.Clear();
            Divisas = dbD.GetJsonTextDivisas();
            Transactions = dbT.GetJsonTextTransactions();
            if (Divisas == null || Divisas.Count() == 0)
            {
                Console.Write("No se ha podido leer el fichero del JSON de Divisas");
            }
            if (Transactions == null || Transactions.Count() == 0)
            {
                Console.Write("No se ha podido leer el fichero del JSON de Transacciones");
            }
        }
    }

    [TestClass]
    public class GNBWebApi
    {
        DivisasRepository dbD = new DivisasRepository();
        TransactionsRepository dbT = new TransactionsRepository();
        List<Divisas> Divisas = new List<Divisas>();
        List<Transactions> Transactions = new List<Transactions>();
        GNBWebApiController controller = new GNBWebApiController();

        [TestMethod]
        public void GetDivisasFiltrar()
        {
            List<Divisas> divisas = controller.ConversorDivisas("EUR", "AUD");
            decimal rate = divisas.Sum(a => a.Rate);
            Assert.AreEqual(rate, rate, "0.01", "asdasd");
        }

        [TestMethod]
        public void GetTransaccionesFiltrar()
        {
            
        }
    }
}
