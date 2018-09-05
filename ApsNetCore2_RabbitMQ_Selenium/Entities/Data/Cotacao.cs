using Dapper.Contrib.Extensions;
using System;

namespace ApsNetCore2_RabbitMQ_Selenium.Entities
{
    [Table("dbo.Cotacoes")]
    public class Cotacao
    {
        [ExplicitKey]
        public string NomeMoeda { get; set; }
        public DateTime DtUltimaCarga { get; set; }
        public double ValorCompra { get; set; }
        public double ValorVenda { get; set; }
        public string Variacao { get; set; }
    }
}
