using GNBWebApi.Models;
using GNBWebApi.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace GNBWebApi.Controllers
{
    public class GNBWebApiController : ApiController
    {
        private DivisasRepository dbD = new DivisasRepository();
        private readonly List<Divisas> Divisas = new List<Divisas>();
        private TransactionsRepository dbT = new TransactionsRepository();
        private readonly List<Transactions> Transactions = new List<Transactions>();

        #region controladores
        public GNBWebApiController()
        {
            try
            {
                Divisas.Clear();
                Transactions.Clear();
                Divisas = dbD.GetJsonUrlDivisas();
                Transactions = dbT.GetJsonUrlTransactions();
                if (Divisas == null || Divisas.Count() == 0)
                {
                    Divisas = dbD.GetJsonTextDivisas();
                }
                if (Transactions == null || Transactions.Count() == 0)
                {
                    Transactions = dbT.GetJsonTextTransactions();
                }
            }
            catch
            {
                throw new Exception("Error al leer las URL o Ficheros Json para Divisas y Transacciones");
            }
        }

        // GET: api/Divisas
        [Route("api/Divisas")]
        public JsonResult<List<Divisas>> GetRates()
        {
            return Json(Divisas);
        }

        // GET api/Transactions  
        [Route("api/Transacciones")]
        public JsonResult<List<Transactions>> GetTransactions()
        {
            return Json(Transactions);
        }

        // GET api/GetTransactionsSKU  
        [Route("api/TransaccionesFiltrar")]
        public JsonResult<List<Transactions>> GetTransaccionesFiltrar(string sku)
        {
            if (sku.Equals(""))
            {
                throw new Exception("Se ha de informar el codigo de transaccion [SKU]");
            }
            else
            {
                List<Transactions> TransactionsSku = new List<Transactions>();
                var transactionsSku = (from x in Transactions
                                       where x.Sku.Equals(sku)
                                       select x).ToList();
                if (transactionsSku.Count() == 0 || transactionsSku == null)
                {
                    throw new Exception("El codigo de transaccion [SKU] indicado, no existe");
                }
                else
                {
                    TransactionsSku.Clear();
                    foreach (var transaction in transactionsSku)
                    {
                        if (transaction.Currency.Equals("EUR"))
                        {
                            TransactionsSku.Add(transaction);
                        }
                        else
                        {
                            TransactionsSku.Add(TransactionToCurrency(transaction, "EUR"));
                        }
                    }
                }
                decimal amount = TransactionsSku.Sum(a => a.Amount);
                return Json(TransactionsSku);
            }
            
        }

        //GET: api/Divisas/5
        [Route("api/DivisasFiltrar")]
        public JsonResult<List<Divisas>> GetDivisasFiltrar(string from, string to)
        {
                return Json(ConversorDivisas(from, to));
        }
        #endregion








        #region Metodos
        [NonAction]
        public List<Divisas> ConversorDivisas(string from, string to)
        {
            if (from.Equals("") || to.Equals(""))
            {
                throw new Exception("Se han de informar correctamente las Divisas FROM y TO");
            }
            else
            {
                List<Divisas> Conversion = new List<Divisas>();
                var divisasFromTo = (from x in Divisas
                                     where x.From.Equals(@from) && x.To.Equals(to)
                                     select x).ToList();
                if (divisasFromTo != null && divisasFromTo.Count() > 0)
                {
                    return divisasFromTo;
                }
                else
                {
                    var divisasFrom = (from x in Divisas
                                       where x.From.Equals(@from)
                                       select x).ToList();
                    if (divisasFrom == null || divisasFrom.Count() == 0)
                    {
                        throw new Exception("El codigo de la Divisa indicada " + from + " no existe en la Lista de Divisas");
                    }
                    else
                    {
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
                        if (Conversion.Count() == 0 || Conversion == null)
                        {
                            throw new Exception("No ha sido posible encontrar el cambio de Rate para las siguientes Divisas " + from + " -> " + to);
                        }
                        else
                        {
                            return Conversion;
                        }
                    }
                }
            }
        }

        [NonAction]
        public decimal RateToCurrency(string currency1, string currency2)
        {
            if (currency1.Equals("") || currency2.Equals(""))
            {
                throw new Exception("Se han de informar correctamente las Divisas de las cuales se quiere hayar el rate");
            }
            else
            {
                decimal rate = 1;
                List<Divisas> divisas = ConversorDivisas(currency1, currency2);
                foreach (var div in divisas)
                {
                    rate = rate * div.Rate;
                }
                return rate;
            }
        }

        [NonAction]
        public Transactions TransactionToCurrency(Transactions transaction, string currency)
        {
            if (transaction.Currency.Equals(""))
            {
                throw new Exception("La transaccion " + transaction.Sku + " no tiene informada su Currency");
            }
            else
            {
                if (transaction.Amount.ToString().Equals("") || transaction.Amount.ToString() == null)
                {
                    throw new Exception("La transaccion " + transaction.Sku + " no tiene informado su Amount");
                }
                else
                {
                    decimal rate = RateToCurrency(transaction.Currency, currency);
                    transaction.Amount = Math.Round(transaction.Amount * rate, MidpointRounding.ToEven);
                    transaction.Currency = currency;
                    return transaction;
                }
            }
        }
        #endregion

    }
}
