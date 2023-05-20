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
        var applicationArguments = "";
        foreach (var (argument, value) in arguments)
        {
            applicationArguments += string.IsNullOrEmpty(value)
                ? $" {argument}"
                : $" {argument}={value}";
        }

        ProcessStartInfo.Arguments =
            $"\".\" --target \"dotnet\" "
            + $"--targetargs \"{TestProjectInfo.AppDllName}{applicationArguments}\" "
            + $"--output {TestProjectInfo.CoverageReportPath} "
            + $"--format cobertura";
    }
}
