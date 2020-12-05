using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace kata_rabbitmq.infrastructure
{
    public static class RabbitMqConnectedHostBuilder
    {
        public static IHostBuilder Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THostedService>()
            where THostedService : class, IHostedService =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services.AddHostedService<THostedService>()
                        .AddHostedService<LogApplicationInfoService>()
                        .AddRabbitMqInfrastructure());
    }
}