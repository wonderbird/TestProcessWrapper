namespace RemoteControlledProcess
{
    internal class ProcessOutputRecorderFactory : IProcessOutputRecorderFactory
    {
        public IProcessOutputRecorder Create() => new ProcessOutputRecorder();
    }
}
