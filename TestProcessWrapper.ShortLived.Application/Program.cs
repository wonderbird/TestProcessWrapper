// error CA1852: Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
#pragma warning disable CA1852
Console.WriteLine($"Process ID {Environment.ProcessId}");
if (args.Contains("--test-argument"))
{
    Console.WriteLine($"Received the command line argument '--test-argument'");
}
Console.WriteLine($"Shut down");
#pragma warning restore CA1852
