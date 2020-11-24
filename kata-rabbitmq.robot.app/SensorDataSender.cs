using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace kata_rabbitmq.robot.app
{
    public class SensorDataSender : BackgroundService
    {
        private readonly ILogger<SensorDataSender> _logger;
        private IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public SensorDataSender(ILogger<SensorDataSender> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                RegisterCancellationRequest(stoppingToken);
                
                while (true)
                {
                    await ExecuteSensorLoopBody(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // This exception is desired, when shutdown is requested. No action is necessary.
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
            }
            finally
            {
                ShutdownService();
            }
        }

        private void RegisterCancellationRequest(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Waiting for cancellation request");
            stoppingToken.Register(() => _logger.LogInformation("STOP request received"));
            stoppingToken.ThrowIfCancellationRequested();
        }

        private async Task ExecuteSensorLoopBody(CancellationToken stoppingToken)
        {
            if (_channel == null)
            {
                ConnectToRabbitMq();
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }

        private void ConnectToRabbitMq()
        {
            try
            {
                _logger.LogDebug("Connecting to RabbitMQ ...");

                var connectionFactory = CreateRabbitMqConnectionFactory();
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

        private ConnectionFactory CreateRabbitMqConnectionFactory()
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

        private void ShutdownService()
        {
            _logger.LogInformation("Shutting down ...");

            DisconnectFromRabbitMq();

            _logger.LogDebug("Shutdown complete.");
        }

        private void DisconnectFromRabbitMq()
        {
            _channel?.Close();
            _connection?.Close();
            _channel = null;
            _connection = null;
        }
    }
}