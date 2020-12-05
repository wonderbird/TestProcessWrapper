using kata_rabbitmq.infrastructure;
using Microsoft.Extensions.Hosting;

namespace kata_rabbitmq.robot.app
{
    public static class Program
    {
        public static void Main()
        {
            RabbitMqConnectedHostBuilder.Create<SensorDataSender>().Build().Run();
        }
    }
}
