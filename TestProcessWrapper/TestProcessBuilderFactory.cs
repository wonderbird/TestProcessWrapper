namespace TestProcessWrapper;

internal class TestProcessBuilderFactory : ITestProcessBuilderFactory
{
    public UnwrappedProcessBuilder CreateBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration,
        bool isCoverletEnabled
    ) => new(appProjectName, buildConfiguration, isCoverletEnabled);
}
