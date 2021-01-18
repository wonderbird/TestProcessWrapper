using System;
using katarabbitmq.infrastructure;
using Microsoft.Extensions.Hosting;

namespace katarabbitmq.client.app
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                RabbitMqConnectedHostBuilder.Create<SensorDataConsumer>().Build().Run();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unhandled Exception");
                Console.Error.WriteLine(e);
            }
        }
    }
}