namespace RemoteControlledProcess
{
    internal class ProcessFactory : IProcessFactory
    {
        public IProcess CreateProcess() => new DotnetProcess();
    }
}