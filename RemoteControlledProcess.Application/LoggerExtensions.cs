using System;
using Microsoft.Extensions.Logging;

namespace RemoteControlledProcess.Application;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, Exception> OperationCancelledAction;
    private static readonly Action<ILogger, Exception> WaitingForCancellationRequestAction;
    private static readonly Action<ILogger, Exception> StopRequestReceivedAction;
    private static readonly Action<ILogger, Exception> ShuttingDownAction;
    private static readonly Action<ILogger, Exception> ShutDownCompleteAction;
    private static readonly Action<ILogger, int, Exception> ProcessIdAction;
    private static readonly Action<ILogger, string, Exception> ConfiguredEnvironmentVariableAction;

    static LoggerExtensions()
    {
        OperationCancelledAction = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1, nameof(OperationCancelled)),
            "Operation has been canceled"
        );
        WaitingForCancellationRequestAction = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(2, nameof(WaitingForCancellationRequest)),
            "Waiting for cancellation request"
        );
        StopRequestReceivedAction = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(3, nameof(StopRequestReceived)),
            "STOP request received"
        );
        ShuttingDownAction = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(4, nameof(ShuttingDown)),
            "Shutting down ..."
        );
        ShutDownCompleteAction = LoggerMessage.Define(
            LogLevel.Debug,
            new EventId(5, nameof(ShutDownComplete)),
            "Shutdown complete"
        );
        ProcessIdAction = LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(6, nameof(ProcessId)),
            "Process ID {ProcessId}"
        );
        ConfiguredEnvironmentVariableAction = LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(7, nameof(ConfiguredEnvironmentVariable)),
            "Configured environment variable: \"{Value}\""
        );
    }

    public static void OperationCancelled(this ILogger logger) =>
        OperationCancelledAction(logger, null);

    public static void WaitingForCancellationRequest(this ILogger logger) =>
        WaitingForCancellationRequestAction(logger, null);

    public static void StopRequestReceived(this ILogger logger) =>
        StopRequestReceivedAction(logger, null);

    public static void ShuttingDown(this ILogger logger) => ShuttingDownAction(logger, null);

    public static void ShutDownComplete(this ILogger logger) =>
        ShutDownCompleteAction(logger, null);

    public static void ProcessId(this ILogger logger, int processId) =>
        ProcessIdAction(logger, processId, null);

    public static void ConfiguredEnvironmentVariable(this ILogger logger, string value) =>
        ConfiguredEnvironmentVariableAction(logger, value, null);
}
