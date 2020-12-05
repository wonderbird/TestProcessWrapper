using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace kata_rabbitmq.infrastructure
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly ILogger<RabbitMqConnection> _logger;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public bool IsConnected => _connection != null;

        public RabbitMqConnection(ILogger<RabbitMqConnection> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        
        public void TryConnect()
        {
            if (IsConnected)
            {
                return;
            }

            try
            {
                _logger.LogDebug("Connecting to RabbitMQ ...");

                var connectionFactory = CreateConnectionFactory();
                _connection = connectionFactory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare("robot", ExchangeType.Direct, durable: false, autoDelete: true,
                    arguments: null);
                _channel.QueueDeclare("sensors", durable: false, exclusive: false, autoDelete: true,
                    arguments: null);

                _logger.LogInformation("Established connection to RabbitMQ");
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                _channel = null;
                _connection = null;
            }
        }

        private ConnectionFactory CreateConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory
            {
                VirtualHost = "/",
                ClientProvidedName = "app:robot",
                HostName = _configuration["RabbitMq:HostName"],
                Port = _configuration.GetValue<int>("RabbitMq:Port"),
                UserName = _configuration["RabbitMq:UserName"],
                Password = _configuration["RabbitMq:Password"]
            };
            
            _logger.LogDebug($"RabbitMQ HostName: {connectionFactory.HostName}");
            _logger.LogDebug($"RabbitMQ Port: {connectionFactory.Port}");
            _logger.LogDebug($"RabbitMQ UserName: {connectionFactory.UserName}");
            
            return connectionFactory;
        }

        public void Disconnect()
        {
            _channel?.Close();
            _connection?.Close();
            _channel = null;
            _connection = null;
        }
    }
}