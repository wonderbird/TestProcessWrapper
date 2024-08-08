using System;
using System.Globalization;

namespace TestProcessWrapper;

/// <summary>
/// Read process ID from stdout.
/// </summary>
/// <remarks>
/// We cannot use the ID of the dotnet process, because that would refer to the coverlet instance.
/// We want to kill the dotnet process and let coverlet finish gracefully. Otherwise we would not
/// receive the correct coverage data.
/// </remarks>
internal class ProcessIdReader
{
    public int ProcessId { get; private set; }

    public bool Read(string processOutput)
    {
        if (!processOutput.Contains("Process ID"))
        {
            return false;
        }

        var processIdStartIndex = processOutput.IndexOf("Process ID", StringComparison.Ordinal);
        var newLineAfterProcessIdIndex = processOutput.IndexOf('\n', processIdStartIndex);
        var processIdNumberOfDigits = newLineAfterProcessIdIndex - processIdStartIndex - 10;
        var processIdString = processOutput.Substring(
            processIdStartIndex + 10,
            processIdNumberOfDigits
        );
        ProcessId = int.Parse(processIdString, NumberStyles.Integer, CultureInfo.InvariantCulture);

        return true;
    }
}
