using katarabbitmq.infrastructure;
using Microsoft.Extensions.Hosting;

namespace katarabbitmq.client.app
{
    public static class Program
    {
        public static void Main()
        {
            RabbitMqConnectedHostBuilder.Create<SensorDataConsumer>().Build().Run();
        }
    }
}