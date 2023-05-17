namespace TestProcessWrapper
{
    internal interface IProcessFactory
    {
        IProcess Create(
            string appProjectName,
            BuildConfiguration buildConfiguration,
            bool isCoverletEnabled
        );
    }
}
