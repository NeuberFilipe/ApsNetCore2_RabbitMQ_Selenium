using ApsNetCore2_RabbitMQ_Selenium.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ApsNetCore2_RabbitMQ_Selenium.Infra
{
    public class CotacoesContext
    {
        private string _strConnection;

        public CotacoesContext(string stringConnection)
        {
            _strConnection = stringConnection;
        }

        public void CarregarDados(List<Cotacao> cotacoes)
        {
            using (SqlConnection conexao =
                new SqlConnection(_strConnection))
            {
                conexao.Execute("TRUNCATE TABLE dbo.Cotacoes");
                conexao.Insert(cotacoes);
            }
        }
    }
}
