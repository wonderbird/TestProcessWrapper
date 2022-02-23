using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RemoteControlledProcess.Application
{
    public static class SimpleHostBuilder
    {
        public static IHostBuilder Create<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        THostedService>()
            where THostedService : class, IHostedService
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services.AddHostedService<THostedService>()
                        .AddHostedService<LogApplicationInfoService>());
        }
    }
}