using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;

namespace RemoteControlledProcess
{
    public static class ExceptionReporterExtension
    {
        private static readonly Action<ILogger, string, int?, Exception> LogUnhandledExceptionAction;

        static ExceptionReporterExtension() =>
            LogUnhandledExceptionAction =
                LoggerMessage.Define<string, int?>(LogLevel.Critical, new EventId(1, nameof(Log)), "Unhandled exception in {FileName}:{@LineNumber}");

        public static void Write(this Exception exception, TextWriter writer)
        {
            var exceptionOrigin = GetExceptionOriginFromStackTrace();
            writer.WriteLine($"Unhandled exception in {exceptionOrigin}");
            writer.WriteLine(exception.ToString());
        }

        public static void Log(this Exception exception, ILogger logger)
        {
            var exceptionOrigin = GetExceptionOriginFromStackTrace();
            LogUnhandledExceptionAction(logger, exceptionOrigin.FileName, exceptionOrigin.LineNumber, exception);
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