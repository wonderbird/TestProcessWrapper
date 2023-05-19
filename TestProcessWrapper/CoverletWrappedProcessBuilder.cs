using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TestProcessWrapper;

internal class CoverletWrappedProcessBuilder : TestProcessBuilder
{
    public BuildConfiguration BuildConfiguration { get; set; }

    public bool IsCoverletEnabled { get; set; }

    private string BinFolder => Path.Combine("bin", BuildConfiguration.ToString(), "net7.0");

    public CoverletWrappedProcessBuilder() { }

    public CoverletWrappedProcessBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration,
        bool isCoverletEnabled
    )
        : base(appProjectName)
    {
        BuildConfiguration = buildConfiguration;
        IsCoverletEnabled = isCoverletEnabled;
    }

    public override void CreateStartInfo()
    {
        if (!IsCoverletEnabled)
        {
            ProcessStartInfo = CreateStartInfo("dotnet", TestProjectInfo.AppDllName);
        }
        else
        {
            ProcessStartInfo = CreateProcessStartInfoWithCoverletWrapper();
        }
    }

    private ProcessStartInfo CreateStartInfo(string processName, string processArguments)
    {
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
                BinFolder
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
