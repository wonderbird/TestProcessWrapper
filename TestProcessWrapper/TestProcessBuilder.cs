using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TestProcessWrapper;

internal class TestProcessBuilder
{
    public string AppProjectName { get; }

    public BuildConfiguration BuildConfiguration { get; set; }

    public bool IsCoverletEnabled { get; set; }

    private readonly Dictionary<string, string> _environmentVariables = new();

    private TestProjectInfo _testProjectInfo;

    private string BinFolder => Path.Combine("bin", BuildConfiguration.ToString(), "net7.0");

    public TestProcessBuilder() { }

    public TestProcessBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration,
        bool isCoverletEnabled
    )
    {
        AppProjectName = appProjectName;
        BuildConfiguration = buildConfiguration;
        IsCoverletEnabled = isCoverletEnabled;
    }

    public void AddEnvironmentVariable(string name, string value) =>
        _environmentVariables[name] = value;

    public virtual ITestProcess Build()
    {
        _testProjectInfo = new TestProjectInfo(AppProjectName);

        var process = new TestProcess();

        process.StartInfo = CreateProcessStartInfo();

        return process;
    }

    private ProcessStartInfo CreateProcessStartInfo()
    {
        ProcessStartInfo processStartInfo;

        if (!IsCoverletEnabled)
        {
            processStartInfo = CreateProcessStartInfo("dotnet", _testProjectInfo.AppDllName);
        }
        else
        {
            processStartInfo = CreateProcessStartInfoWithCoverletWrapper();
        }

        return processStartInfo;
    }

    private ProcessStartInfo CreateProcessStartInfo(string processName, string processArguments)
    {
        var processStartInfo = new ProcessStartInfo(processName)
        {
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            Arguments = processArguments,
            WorkingDirectory = Path.Combine(
                _testProjectInfo.ProjectDir,
                _testProjectInfo.AppProjectName,
                BinFolder
            )
        };

        foreach (var item in _environmentVariables)
        {
            processStartInfo.Environment.Add(item);
        }

        return processStartInfo;
    }

    private ProcessStartInfo CreateProcessStartInfoWithCoverletWrapper()
    {
        var arguments =
            $"\".\" --target \"dotnet\" --targetargs \"{_testProjectInfo.AppDllName}\" --output {_testProjectInfo.CoverageReportPath} --format cobertura";

        return CreateProcessStartInfo("coverlet", arguments);
    }
}
