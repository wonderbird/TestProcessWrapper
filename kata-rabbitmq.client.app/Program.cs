using katarabbitmq.infrastructure;
using Microsoft.Extensions.Hosting;

namespace katarabbitmq.client.app
{
    public static class Program
    {
        public static void Main()
        {
            // TODO: Check for unhandled Exceptions
            RabbitMqConnectedHostBuilder.Create<SensorDataConsumer>().Build().Run();
        }
    }
}