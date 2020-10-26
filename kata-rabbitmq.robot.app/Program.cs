using System;
using System.Threading;
using RabbitMQ.Client;

namespace kata_rabbitmq.robot.app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME");
            connectionFactory.Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"));
            connectionFactory.UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
            connectionFactory.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
            connectionFactory.VirtualHost = "/";
            connectionFactory.ClientProvidedName = "app:robot";

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("robot", ExchangeType.Direct, durable: false, autoDelete: true, arguments: null);
            channel.QueueDeclare("sensors", durable: false, exclusive: false, autoDelete: true, arguments: null);

            Thread.Sleep(2000);

            channel.Close();
            connection.Close();
        }
    }
}
