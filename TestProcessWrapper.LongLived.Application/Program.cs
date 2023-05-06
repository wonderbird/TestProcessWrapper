using System;
using Microsoft.Extensions.Hosting;

namespace TestProcessWrapper.LongLived.Application
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
