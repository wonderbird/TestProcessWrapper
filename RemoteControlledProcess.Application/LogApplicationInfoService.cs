using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RemoteControlledProcess.Application
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
            _logger.ProcessId(processId);

            return Task.CompletedTask;
        }

        private void LogEnvironmentVariable(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            _logger.ConfiguredEnvironmentVariable(value);
        }
    }
}