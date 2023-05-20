using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TestProcessWrapper;

internal class CoverletWrappedProcessBuilder : TestProcessBuilder
{
    private readonly BuildConfiguration _buildConfiguration;

    public CoverletWrappedProcessBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration
    )
        : base(appProjectName) =>
        _buildConfiguration = buildConfiguration;

    public override void CreateStartInfo()
    {
        ProcessStartInfo = CreateProcessStartInfoWithCoverletWrapper();
    }

    private ProcessStartInfo CreateStartInfo(string processName, string processArguments)
    {
        var binFolder = Path.Combine("bin", _buildConfiguration.ToString(), "net7.0");

        var processStartInfo = new ProcessStartInfo(processName)
        {
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            Arguments = processArguments,
            WorkingDirectory = Path.Combine(
                TestProjectInfo.ProjectDir,
                TestProjectInfo.AppProjectName,
                binFolder
            )
        };

        return processStartInfo;
    }

    private ProcessStartInfo CreateProcessStartInfoWithCoverletWrapper()
    {
        var arguments =
            $"\".\" --target \"dotnet\" --targetargs \"{TestProjectInfo.AppDllName}\" --output {TestProjectInfo.CoverageReportPath} --format cobertura";

        return CreateStartInfo("coverlet", arguments);
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
