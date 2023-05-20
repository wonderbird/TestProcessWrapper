using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestProcessWrapper.LongLived.Application;

public sealed class SimpleService : BackgroundService
{
    private ILogger<SimpleService> Logger { get; }

    private TimeSpan DelayAfterEachLoop { get; } = TimeSpan.FromMilliseconds(50.0);

    public SimpleService(ILogger<SimpleService> logger, IConfiguration configuration)
    {
        Logger = logger;

        const string testArgumentName = "test-argument";
        var testArgumentValue = configuration.GetValue<string>(testArgumentName);
        Logger.CommandLineArgument($"--{testArgumentName}", testArgumentValue);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            RegisterCancellationRequest(stoppingToken);

            while (true)
            {
                await PerformSampleWorkerTask(stoppingToken);
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

    private async Task PerformSampleWorkerTask(CancellationToken stoppingToken)
    {
        await Task.Delay(DelayAfterEachLoop, stoppingToken);
    }

    private void ShutdownService()
    {
        Logger.ShuttingDown();
        Logger.ShutDownComplete();
    }
}
