using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace katarabbitmq.infrastructure
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqConnection> _logger;

        public RabbitMqConnection(ILogger<RabbitMqConnection> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IConnection Connection { get; private set; }

        public string QueueName { get; private set; }

        public IModel Channel { get; private set; }

        public bool IsConnected => Connection != null;

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
                Connection = connectionFactory.CreateConnection();
                Channel = Connection.CreateModel();

                Channel.ExchangeDeclare("robot", ExchangeType.Fanout, false, true,
                    null);
                var queueDeclareOk = Channel.QueueDeclare("", false, true, true,
                    null);
                QueueName = queueDeclareOk.QueueName;
                Channel.QueueBind(QueueName, "robot", "", null);

                _logger.LogInformation($"Established connection to RabbitMQ, queue name {QueueName}.");
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                Channel = null;
                Connection = null;
            }
        }

        public void Disconnect()
        {
            Channel?.Close();
            Connection?.Close();
            Channel = null;
            Connection = null;
        }

        private ConnectionFactory CreateConnectionFactory()
        {
            var uniqueClientProvidedNameSuffix = "-" + Guid.NewGuid();

            var connectionFactory = new ConnectionFactory
            {
                VirtualHost = "/",
                ClientProvidedName = _configuration["RabbitMq:ClientProvidedName"] + uniqueClientProvidedNameSuffix,
                HostName = _configuration["RabbitMq:HostName"],
                Port = _configuration.GetValue<int>("RabbitMq:Port"),
                UserName = _configuration["RabbitMq:UserName"],
                Password = _configuration["RabbitMq:Password"]
            };

            _logger.LogDebug($"RabbitMQ HostName: {connectionFactory.HostName}");
            _logger.LogDebug($"RabbitMQ Port: {connectionFactory.Port}");
            _logger.LogDebug($"RabbitMQ UserName: {connectionFactory.UserName}");
            _logger.LogDebug($"RabbitMQ ClientProvidedName: {connectionFactory.ClientProvidedName}");

            return connectionFactory;
        }
    }
}