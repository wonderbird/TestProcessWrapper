using System;
using Microsoft.Extensions.Hosting;

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
                var stackFrame = new System.Diagnostics.StackTrace(true).GetFrame(0);
                var fileName = stackFrame?.GetFileName();
                var lineNumber = stackFrame?.GetFileLineNumber();

                Console.Error.WriteLine($"Unhandled exception in {fileName}:{lineNumber}.");
                Console.Error.WriteLine(e.ToString());
            }
        }
    }
}