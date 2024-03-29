using System;
using System.Diagnostics;

namespace TestProcessWrapper;

internal sealed class TestProcess : ITestProcess
{
    private readonly Process _process = new();
    private bool _isDisposed;

    public bool HasExited => _process.HasExited;

    public ProcessStartInfo StartInfo
    {
        get => _process.StartInfo;
        set => _process.StartInfo = value;
    }

    public void Start() => _process.Start();

    public void BeginOutputReadLine() => _process.BeginOutputReadLine();

    public event DataReceivedEventHandler OutputDataReceived
    {
        add => _process.OutputDataReceived += value;
        remove => _process.OutputDataReceived -= value;
    }

    public void WaitForExit(int milliseconds) => _process.WaitForExit(milliseconds);

    public void Kill() => _process.Kill();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~TestProcess()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _process.Dispose();
        }

        _isDisposed = true;
    }
}
