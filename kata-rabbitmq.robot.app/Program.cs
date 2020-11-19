using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace kata_rabbitmq.robot.app
{
    public class Program
    {
        public static async Task Main()
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SensorDataSender>();
                });

            await builder.RunConsoleAsync();
        }
    }
}
