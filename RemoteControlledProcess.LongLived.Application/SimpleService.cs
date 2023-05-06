using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RemoteControlledProcess.LongLived.Application
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
                Logger.OperationCancelled();
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
            Logger.WaitingForCancellationRequest();
            stoppingToken.Register(() => Logger.StopRequestReceived());
            stoppingToken.ThrowIfCancellationRequested();
        }

        protected virtual async Task ExecuteSensorLoopBody(CancellationToken stoppingToken)
        {
            await Task.Delay(DelayAfterEachLoop, stoppingToken);
        }

        private void ShutdownService()
        {
            Logger.ShuttingDown();
            Logger.ShutDownComplete();
        }
    }
}
