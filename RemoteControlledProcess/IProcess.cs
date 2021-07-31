using System;
using System.Diagnostics;

namespace RemoteControlledProcess
{
    internal interface IProcess : IDisposable
    {
        int Id { get; }

        bool HasExited { get; }

        ProcessStartInfo StartInfo { get; set; }

        void AddEnvironmentVariable(string name, string value);

        void Start();

        void BeginOutputReadLine();

        event DataReceivedEventHandler OutputDataReceived;

        void WaitForExit(int milliseconds);

        void Kill();
    }
}