namespace TestProcessWrapper;

internal class TestProcessBuilderFactory : ITestProcessBuilderFactory
{
    public TestProcessBuilder CreateBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration,
        bool isCoverletEnabled
    )
    {
        if (isCoverletEnabled)
        {
            return new CoverletWrappedProcessBuilder(appProjectName, buildConfiguration);
        }
        else
        {
            return new UnwrappedProcessBuilder(appProjectName, buildConfiguration);
        }
    }
}
