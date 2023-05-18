namespace TestProcessWrapper
{
    internal interface IProcessFactory
    {
        ITestProcess Create(
            string appProjectName,
            BuildConfiguration buildConfiguration,
            bool isCoverletEnabled
        );
    }
}
