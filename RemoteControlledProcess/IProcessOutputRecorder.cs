using System;

namespace RemoteControlledProcess
{
    internal interface IProcessOutputRecorder : IDisposable
    {
        string Output { get; }

        void StartRecording(IProcess process);
    }
}