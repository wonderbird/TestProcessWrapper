// error CA1852: Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
#pragma warning disable CA1852
Console.WriteLine($"Process ID {Environment.ProcessId}");
#pragma warning restore CA1852
