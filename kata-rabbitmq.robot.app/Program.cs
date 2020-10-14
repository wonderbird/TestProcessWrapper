using System;
using System.Threading;
using RabbitMQ.Client;

namespace kata_rabbitmq.robot.app
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connectionFactory.VirtualHost = "/";
            connectionFactory.HostName = "localhost";
            connectionFactory.Port = 5672;
            connectionFactory.ClientProvidedName = "app:robot";

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("robot", ExchangeType.Direct, durable: false, autoDelete: true, arguments: null);
            channel.QueueDeclare("sensors", durable: false, exclusive: false, autoDelete: true, arguments: null);

            Console.WriteLine("Connection established. Press any key to exit.");
            Console.ReadKey();

            channel.Close();
            connection.Close();
        }
    }
}
