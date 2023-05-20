using System.Collections.Generic;

namespace TestProcessWrapper;

internal class CoverletWrappedProcessBuilder : TestProcessBuilder
{
    private readonly BuildConfiguration _buildConfiguration;

    public CoverletWrappedProcessBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration
    )
        : base(appProjectName) => _buildConfiguration = buildConfiguration;

    public override void CreateProcessStartInfo()
    {
        var arguments =
            $"\".\" --target \"dotnet\" "
            + $"--targetargs \"{TestProjectInfo.AppDllName}\" "
            + $"--output {TestProjectInfo.CoverageReportPath} "
            + $"--format cobertura";

        ProcessStartInfo = CreateProcessStartInfo("coverlet", arguments, _buildConfiguration);
    }

    public override void AddCommandLineArguments(Dictionary<string, string> arguments)
    {
        foreach (var (argument, value) in arguments)
        {
            ProcessStartInfo.Arguments += string.IsNullOrEmpty(value)
                ? $" {argument}"
                : $" {argument}={value}";
        }
    }
}
