namespace TestProcessWrapper;

internal interface ITestProcessBuilderFactory
{
    TestProcessBuilder CreateBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration,
        bool isCoverletEnabled
    );
}
