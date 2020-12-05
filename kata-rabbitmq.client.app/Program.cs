using kata_rabbitmq.infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace kata_rabbitmq.client.app
{
    class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services.AddHostedService<SensorDataConsumer>()
                        .AddHostedService<LogApplicationInfoService>()
                        .AddRabbitMqInfrastructure());
    }
}