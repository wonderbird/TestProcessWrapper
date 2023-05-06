namespace TestProcessWrapper
{
    internal interface IProcessFactory
    {
        IProcess Create(string appProjectName, bool isCoverletEnabled);
    }
}
