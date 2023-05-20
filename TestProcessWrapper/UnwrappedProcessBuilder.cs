using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TestProcessWrapper;

internal class UnwrappedProcessBuilder : TestProcessBuilder
{
    private readonly BuildConfiguration _buildConfiguration;

    public UnwrappedProcessBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration
    )
        : base(appProjectName) =>
        _buildConfiguration = buildConfiguration;

    public override void CreateStartInfo()
    {
        var binFolder = Path.Combine("bin", _buildConfiguration.ToString(), "net7.0");

        var processStartInfo = new ProcessStartInfo("dotnet")
        {
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            Arguments = TestProjectInfo.AppDllName,
            WorkingDirectory = Path.Combine(
                TestProjectInfo.ProjectDir,
                TestProjectInfo.AppProjectName,
                binFolder
            )
        };

        ProcessStartInfo = processStartInfo;
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
