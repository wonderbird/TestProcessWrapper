using System;

namespace RemoteControlledProcess
{
    public interface IProcessOutputRecorder : IDisposable
    {
        string Output { get; }

        void StartRecording(IProcess process);
    }
}