using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace katarabbitmq.client.app
{
    public class LogApplicationInfoService : BackgroundService
    {
        private readonly ILogger<LogApplicationInfoService> _logger;

        public LogApplicationInfoService(ILogger<LogApplicationInfoService> logger) => _logger = logger;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            LogEnvironmentVariable("CONFIGURED_ENVIRONMENT_VARIABLE_1");
            LogEnvironmentVariable("CONFIGURED_ENVIRONMENT_VARIABLE_2");

            var processId = Environment.ProcessId;
            // TODO: Re-enable Inspection CA1848 "Use the LoggerMessage delegates"
            // TODO: Re-enable Inspection CA2254 "Template should be a static expression"
#pragma warning disable CA1848
#pragma warning disable CA2254
            _logger.LogInformation($"Process ID {processId}");
#pragma warning restore CA2254
#pragma warning restore CA1848

            return Task.CompletedTask;
        }

        private void LogEnvironmentVariable(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            // TODO: Re-enable Inspection CA1848 "Use the LoggerMessage delegates"
            // TODO: Re-enable Inspection CA2254 "Template should be a static expression"
#pragma warning disable CA1848
#pragma warning disable CA2254
            _logger.LogInformation($"Configured environment variable 1: \"{value}\"");
#pragma warning restore CA2254
#pragma warning restore CA1848
        }
    }
}