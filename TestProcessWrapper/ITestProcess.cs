using System;
using System.Diagnostics;

namespace TestProcessWrapper;

internal interface ITestProcess : IDisposable
{
    bool HasExited { get; }

    ProcessStartInfo StartInfo { get; }

    void Start();

    void BeginOutputReadLine();

    event DataReceivedEventHandler OutputDataReceived;

    void WaitForExit(int milliseconds);

    void Kill();
}
