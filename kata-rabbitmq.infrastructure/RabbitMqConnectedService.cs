using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace katarabbitmq.infrastructure
{
    public abstract class RabbitMqConnectedService : BackgroundService
    {
        protected RabbitMqConnectedService(IRabbitMqConnection rabbit, ILogger<RabbitMqConnectedService> logger)
        {
            Rabbit = rabbit;
            Logger = logger;
        }

        protected IRabbitMqConnection Rabbit { get; }

        protected ILogger<RabbitMqConnectedService> Logger { get; }

        protected TimeSpan DelayAfterEachLoop { get; init; } = TimeSpan.FromMilliseconds(50.0);

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
            if (!Rabbit.IsConnected)
            {
                Rabbit.TryConnect();
            }

            await Task.Delay(DelayAfterEachLoop, stoppingToken);
        }

        private void ShutdownService()
        {
            Logger.LogInformation("Shutting down ...");

            Rabbit.Disconnect();

            Logger.LogDebug("Shutdown complete.");
        }
    }
}