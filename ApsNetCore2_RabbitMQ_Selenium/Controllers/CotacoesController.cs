using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper.Contrib.Extensions;
using ApsNetCore2_RabbitMQ_Selenium.Entities;
using RabbitMQ.Client;

namespace ApsNetCore2_RabbitMQ_Selenium.Controllers
{
    public class CotacoesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<Cotacao> GetCotacoes(
      [FromServices]IConfiguration configuration)
        {
            using (SqlConnection conexao = new SqlConnection(
                configuration.GetConnectionString("TestesRabbitMQ")))
            {
                return conexao.GetAll<Cotacao>();
            }
        }

        [HttpGet("carregar")]
        public object CarregarCotacoes(
            [FromServices]RabbitMQConfigurations rabbitMQConfigurations)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMQConfigurations.HostName,
                Port = rabbitMQConfigurations.Port,
                UserName = rabbitMQConfigurations.UserName,
                Password = rabbitMQConfigurations.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "CarregarCotacoes",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Solicitação de Carga - " +
                    $"API Cotacoes - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "CarregarCotacoes",
                                     basicProperties: null,
                                     body: body);
            }

            return new
            {
                Resultado = "Mensagem encaminhada com sucesso"
            };
        }
    }
}