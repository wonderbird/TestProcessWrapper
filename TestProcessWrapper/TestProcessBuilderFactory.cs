namespace TestProcessWrapper;

internal class TestProcessBuilderFactory : ITestProcessBuilderFactory
{
    public TestProcessBuilder CreateBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration,
        bool isCoverletEnabled
    ) => new UnwrappedProcessBuilder(appProjectName, buildConfiguration, isCoverletEnabled);
}
