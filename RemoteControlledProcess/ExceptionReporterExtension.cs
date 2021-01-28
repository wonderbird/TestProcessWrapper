using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;

namespace RemoteControlledProcess
{
    public static class ExceptionReporterExtension
    {
        public static void Write(this Exception exception, TextWriter writer)
        {
            var stackFrame = new StackTrace(true).GetFrame(1);
            var fileName = stackFrame?.GetFileName();
            var lineNumber = stackFrame?.GetFileLineNumber();

            writer.WriteLine($"Unhandled exception in {fileName}:{lineNumber}.");
            writer.WriteLine(exception.ToString());
        }

        public static void Log(this Exception exception, ILogger logger)
        {
            var stackFrame = new StackTrace(true).GetFrame(1);
            var fileName = stackFrame?.GetFileName();
            var lineNumber = stackFrame?.GetFileLineNumber();

            logger.LogCritical(exception, "Unhandled exception in {FileName}:{@LineNumber}", fileName, lineNumber);
        }
    }
}