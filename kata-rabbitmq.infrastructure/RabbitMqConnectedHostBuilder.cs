using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace katarabbitmq.infrastructure
{
    public static class RabbitMqConnectedHostBuilder
    {
        public static IHostBuilder Create<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
            THostedService>()
            where THostedService : class, IHostedService
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services.AddHostedService<THostedService>()
                        .AddHostedService<LogApplicationInfoService>()
                        .AddRabbitMqInfrastructure());
        }
    }
}
