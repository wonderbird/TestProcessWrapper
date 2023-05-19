namespace TestProcessWrapper;

internal interface ITestProcessBuilderFactory
{
    UnwrappedProcessBuilder CreateBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration,
        bool isCoverletEnabled
    );
}
