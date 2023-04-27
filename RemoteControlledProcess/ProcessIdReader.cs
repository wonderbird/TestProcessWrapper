using System;
using System.Globalization;

namespace RemoteControlledProcess
{
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
            var newLineAfterProcessIdIndex = processOutput.IndexOf(
                "\n",
                processIdStartIndex,
                StringComparison.Ordinal
            );
            var processIdNumberOfDigits = newLineAfterProcessIdIndex - processIdStartIndex - 10;
            var processIdString = processOutput.Substring(
                processIdStartIndex + 10,
                processIdNumberOfDigits
            );
            ProcessId = int.Parse(
                processIdString,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture
            );

            return true;
        }
    }
}
