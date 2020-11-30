using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace kata_rabbitmq.infrastructure
{
    public class RabbitMqConnectedService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //throw new NotImplementedException("TODO: move the shared logic to RabbitMqConnectedService");
            return Task.CompletedTask;
        }
    }
}