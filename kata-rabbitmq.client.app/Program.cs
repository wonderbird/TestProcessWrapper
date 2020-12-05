using kata_rabbitmq.infrastructure;
using Microsoft.Extensions.Hosting;

namespace kata_rabbitmq.client.app
{
    public static class Program
    {
        public static void Main()
        {
            RabbitMqConnectedHostBuilder.Create<SensorDataConsumer>().Build().Run();
        }
    }
}