using System;

namespace TestProcessWrapper
{
    internal interface IProcessOutputRecorder : IDisposable
    {
        string Output { get; }

        void StartRecording(IProcess process);
    }
}
