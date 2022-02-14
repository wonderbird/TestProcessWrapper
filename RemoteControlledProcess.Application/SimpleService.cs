using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RemoteControlledProcess;

namespace katarabbitmq.client.app
{
    public class SimpleService : BackgroundService
    {
        public SimpleService(ILogger<SimpleService> logger) => Logger = logger;

        private ILogger<SimpleService> Logger { get; }

        private TimeSpan DelayAfterEachLoop { get; } = TimeSpan.FromMilliseconds(50.0);

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
                // TODO: Re-enable Inspection CA1848 "Use the LoggerMessage delegates"
#pragma warning disable CA1848
                Logger.LogInformation("Operation has been canceled");
#pragma warning restore CA1848
            }
            catch (Exception e)
            {
                e.Log(Logger);
            }
            finally
            {
                ShutdownService();
            }
        }

        private void RegisterCancellationRequest(CancellationToken stoppingToken)
        {
            // TODO: Re-enable Inspection CA1848 "Use the LoggerMessage delegates"
#pragma warning disable CA1848
            Logger.LogInformation("Waiting for cancellation request");
            stoppingToken.Register(() => Logger.LogInformation("STOP request received"));
#pragma warning restore CA1848
            stoppingToken.ThrowIfCancellationRequested();
        }

        protected virtual async Task ExecuteSensorLoopBody(CancellationToken stoppingToken)
        {
            await Task.Delay(DelayAfterEachLoop, stoppingToken);
        }

        private void ShutdownService()
        {
            // TODO: Re-enable Inspection CA1848 "Use the LoggerMessage delegates"
#pragma warning disable CA1848
            Logger.LogInformation("Shutting down ...");
            Logger.LogDebug("Shutdown complete");
#pragma warning restore CA1848
        }
    }
}