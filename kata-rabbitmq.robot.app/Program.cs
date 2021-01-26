using System;
using katarabbitmq.infrastructure;
using Microsoft.Extensions.Hosting;

namespace katarabbitmq.robot.app
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
                Console.Error.WriteLine("Unhandled exception:");
                Console.Error.WriteLine(e);
            }
        }
    }
}