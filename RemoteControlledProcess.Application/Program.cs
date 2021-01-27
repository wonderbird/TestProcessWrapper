using System;
using Microsoft.Extensions.Hosting;
using RemoteControlledProcess;

namespace katarabbitmq.client.app
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                SimpleHostBuilder.Create<SimpleService>().Build().Run();
            }
            catch (Exception e)
            {
                e.Write(Console.Error);
            }
        }
    }
}