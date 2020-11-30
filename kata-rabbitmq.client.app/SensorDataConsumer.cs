using System;
using System.Threading;
using System.Threading.Tasks;
using kata_rabbitmq.infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace kata_rabbitmq.client.app
{
    public class SensorDataConsumer : BackgroundService
    {
        private readonly IRabbitMqConnection _rabbit;
        private readonly ILogger<SensorDataConsumer> _logger;

        public SensorDataConsumer(IRabbitMqConnection rabbit, ILogger<SensorDataConsumer> logger)
        {
            _rabbit = rabbit;
            _logger = logger;
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
            if (!_rabbit.IsConnected)
            {
                _rabbit.TryConnect();
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }

        private void ShutdownService()
        {
            _logger.LogInformation("Shutting down ...");

            _rabbit.Disconnect();

            _logger.LogDebug("Shutdown complete.");
        }
    }
}