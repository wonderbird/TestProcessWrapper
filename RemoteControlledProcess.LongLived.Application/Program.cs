using System;
using Microsoft.Extensions.Hosting;

// TODO: Rename RemoteControlledProcess.LongLived.Application to RemoteControlledProcess.LongLived.Application
namespace RemoteControlledProcess.LongLived.Application
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
