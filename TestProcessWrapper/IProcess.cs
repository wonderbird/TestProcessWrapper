using System;
using System.Diagnostics;

namespace TestProcessWrapper
{
    internal interface IProcess : IDisposable
    {
        bool HasExited { get; }

        ProcessStartInfo StartInfo { get; }

        void AddEnvironmentVariable(string name, string value);

        void Start();

        void BeginOutputReadLine();

        event DataReceivedEventHandler OutputDataReceived;

        void WaitForExit(int milliseconds);

        void Kill();
    }
}
