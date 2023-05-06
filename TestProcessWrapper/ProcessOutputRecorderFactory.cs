namespace TestProcessWrapper
{
    internal class ProcessOutputRecorderFactory : IProcessOutputRecorderFactory
    {
        public IProcessOutputRecorder Create() => new ProcessOutputRecorder();
    }
}
