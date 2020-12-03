using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace kata_rabbitmq.infrastructure
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
            var processId = Process.GetCurrentProcess().Id;
            _logger.LogInformation($"Process ID {processId}");
            
            return Task.CompletedTask;
        }
    }
}