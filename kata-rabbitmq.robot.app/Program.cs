using katarabbitmq.infrastructure;
using Microsoft.Extensions.Hosting;

namespace katarabbitmq.robot.app
{
    public static class Program
    {
        public static void Main()
        {
            RabbitMqConnectedHostBuilder.Create<SensorDataSender>().Build().Run();
        }
    }
}
