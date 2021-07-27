namespace RemoteControlledProcess
{
    public class ProcessOutputRecorderFactory : IProcessOutputRecorderFactory
    {
        public IProcessOutputRecorder Create() => new ProcessOutputRecorder();
    }
}