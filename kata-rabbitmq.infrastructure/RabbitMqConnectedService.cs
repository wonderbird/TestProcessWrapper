using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace katarabbitmq.infrastructure
{
    public abstract class RabbitMqConnectedService : BackgroundService
    {
        protected readonly IRabbitMqConnection _rabbit;

        protected RabbitMqConnectedService(IRabbitMqConnection rabbit, ILogger<RabbitMqConnectedService> logger)
        {
            _rabbit = rabbit;
            Logger = logger;
        }

        protected ILogger<RabbitMqConnectedService> Logger { get; }

        protected TimeSpan DelayAfterEachLoop { get; init; } = TimeSpan.FromSeconds(2.0);

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
                Logger.LogCritical(e.ToString());
            }
            finally
            {
                ShutdownService();
            }
        }

        private void RegisterCancellationRequest(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Waiting for cancellation request");
            stoppingToken.Register(() => Logger.LogInformation("STOP request received"));
            stoppingToken.ThrowIfCancellationRequested();
        }

        protected virtual async Task ExecuteSensorLoopBody(CancellationToken stoppingToken)
        {
            if (!_rabbit.IsConnected)
            {
                _rabbit.TryConnect();
            }

            await Task.Delay(DelayAfterEachLoop, stoppingToken);
        }

        private void ShutdownService()
        {
            Logger.LogInformation("Shutting down ...");

            _rabbit.Disconnect();

            Logger.LogDebug("Shutdown complete.");
        }
    }
}
