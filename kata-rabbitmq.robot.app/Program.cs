using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace kata_rabbitmq.robot.app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("kata_rabbitmq.robot.app.Program", LogLevel.Debug)
                    .AddConsole();
            });
            var logger = loggerFactory.CreateLogger<Program>();
            
            logger.LogDebug("Connecting to RabbitMQ ...");

            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME");
            var portString = Environment.GetEnvironmentVariable("RABBITMQ_PORT");
            if (portString != null) connectionFactory.Port = int.Parse(portString);
            connectionFactory.UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
            connectionFactory.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
            connectionFactory.VirtualHost = "/";
            connectionFactory.ClientProvidedName = "app:robot";

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("robot", ExchangeType.Direct, durable: false, autoDelete: true, arguments: null);
            channel.QueueDeclare("sensors", durable: false, exclusive: false, autoDelete: true, arguments: null);

            logger.Log(LogLevel.Information, "Press ENTER to shutdown the robot.");
            Console.ReadLine();
            logger.Log(LogLevel.Information, "Shutting down ...");
            
            channel.Close();
            connection.Close();
        }
    }
}
