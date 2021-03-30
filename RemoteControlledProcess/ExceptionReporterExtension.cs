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
            var exceptionOrigin = GetExceptionOriginFromStackTrace();
            writer.WriteLine($"Unhandled exception in {exceptionOrigin}");
            writer.WriteLine(exception.ToString());
        }

        public static void Log(this Exception exception, ILogger logger)
        {
            var exceptionOrigin = GetExceptionOriginFromStackTrace();
            logger.LogCritical(exception, "Unhandled exception in {FileName}:{@LineNumber}", exceptionOrigin.FileName,
                exceptionOrigin.LineNumber);
        }

        private static ExceptionOrigin GetExceptionOriginFromStackTrace()
        {
            var stackFrame = new StackTrace(true).GetFrame(2);
            var exceptionOrigin = new ExceptionOrigin
            {
                FileName = stackFrame?.GetFileName(),
                LineNumber = stackFrame?.GetFileLineNumber()
            };
            return exceptionOrigin;
        }

        private class ExceptionOrigin
        {
            public string FileName { get; init; }
            public int? LineNumber { get; init; }

            public override string ToString() => $"{FileName}:{LineNumber}";
        }
    }
}