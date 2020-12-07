using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace katarabbitmq.infrastructure
{
    public class LogApplicationInfoService : BackgroundService
    {
        private readonly ILogger<LogApplicationInfoService> _logger;

        public LogApplicationInfoService(ILogger<LogApplicationInfoService> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var processId = System.Environment.ProcessId;
            _logger.LogInformation($"Process ID {processId}");

            return Task.CompletedTask;
        }
    }
}
