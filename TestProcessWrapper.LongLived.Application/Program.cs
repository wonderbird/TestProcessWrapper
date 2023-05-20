using System;
using Microsoft.Extensions.Hosting;

namespace TestProcessWrapper.LongLived.Application;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            SimpleHostBuilder.Create<SimpleService>(args).Build().Run();
        }
        catch (Exception e)
        {
            e.Write(Console.Error);
        }
    }
}
