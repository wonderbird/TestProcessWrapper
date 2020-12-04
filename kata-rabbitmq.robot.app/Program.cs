using kata_rabbitmq.infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace kata_rabbitmq.robot.app
{
    public class Program
    {
        public static void Main()
        {
            TODO: Finish (restore) reporting code coverage from the github build action
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services.AddHostedService<SensorDataSender>()
                        .AddHostedService<LogApplicationInfoService>()
                        .AddRabbitMqInfrastructure());
    }
}
