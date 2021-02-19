using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using katarabbitmq.bdd.tests.Helpers;
using Xunit.Abstractions;

namespace RemoteControlledProcess
{
    public sealed partial class ProcessWrapper : IDisposable
    {
        private readonly string _appDir;

        private readonly string _appDllName;

        private readonly string _appProjectName;

        private readonly bool _isCoverletEnabled;

        private readonly string _projectDir;

        private int? _dotnetHostProcessId;

        private bool _isDisposed;

        private Process _process;
        private ProcessStreamBuffer _processStreamBuffer;

        private void WaitForProcessToExitForUpTo2Seconds(Process killProcess)
        {
            if (killProcess != null)
            {
                TestOutputHelper?.WriteLine("Waiting for system call to complete.");
                killProcess.WaitForExit(2000);
            }
        }

        private void KillProcessIfItIsStillRunning(Process killProcess)
        {
            if (!killProcess.HasExited)
            {
                TestOutputHelper?.WriteLine("System call has " + (killProcess.HasExited ? "" : "NOT ") + "completed.");
                killProcess.Kill();
            }
        }

        private void WaitForProcessExit()
        {
            TestOutputHelper?.WriteLine("Waiting for process to shutdown ...");
            _process.WaitForExit(2000);
            TestOutputHelper?.WriteLine($"Process {_appProjectName} has " + (_process.HasExited ? "" : "NOT ") +
                                        "completed.");
        }

        public void ForceTermination()
        {
            _process.Kill();
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                _process?.Dispose();
                _processStreamBuffer?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
