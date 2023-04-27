namespace RemoteControlledProcess
{
    internal interface IProcessFactory
    {
        IProcess Create(string appProjectName, bool isCoverletEnabled);
    }
}
