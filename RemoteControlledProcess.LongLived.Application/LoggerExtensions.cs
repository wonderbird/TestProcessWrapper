using System;
using Microsoft.Extensions.Logging;

namespace RemoteControlledProcess.LongLived.Application;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, Exception> OperationCancelledAction =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1, nameof(OperationCancelled)),
            "Operation has been canceled"
        );

    public static void OperationCancelled(this ILogger logger) =>
        OperationCancelledAction(logger, null);

    private static readonly Action<ILogger, Exception> WaitingForCancellationRequestAction =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(2, nameof(WaitingForCancellationRequest)),
            "Waiting for cancellation request"
        );

    public static void WaitingForCancellationRequest(this ILogger logger) =>
        WaitingForCancellationRequestAction(logger, null);

    private static readonly Action<ILogger, Exception> StopRequestReceivedAction =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(3, nameof(StopRequestReceived)),
            "STOP request received"
        );

    public static void StopRequestReceived(this ILogger logger) =>
        StopRequestReceivedAction(logger, null);

    private static readonly Action<ILogger, Exception> ShuttingDownAction = LoggerMessage.Define(
        LogLevel.Information,
        new EventId(4, nameof(ShuttingDown)),
        "Shutting down ..."
    );

    public static void ShuttingDown(this ILogger logger) => ShuttingDownAction(logger, null);

    private static readonly Action<ILogger, Exception> ShutDownCompleteAction =
        LoggerMessage.Define(
            LogLevel.Debug,
            new EventId(5, nameof(ShutDownComplete)),
            "Shutdown complete"
        );

    public static void ShutDownComplete(this ILogger logger) =>
        ShutDownCompleteAction(logger, null);

    private static readonly Action<ILogger, int, Exception> ProcessIdAction =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(6, nameof(ProcessId)),
            "Process ID {ProcessId}"
        );

    public static void ProcessId(this ILogger logger, int processId) =>
        ProcessIdAction(logger, processId, null);

    private static readonly Action<ILogger, string, Exception> ConfiguredEnvironmentVariableAction =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(7, nameof(ConfiguredEnvironmentVariable)),
            "Configured environment variable: \"{Value}\""
        );

    public static void ConfiguredEnvironmentVariable(this ILogger logger, string value) =>
        ConfiguredEnvironmentVariableAction(logger, value, null);
}
