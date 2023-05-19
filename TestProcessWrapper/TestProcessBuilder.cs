using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TestProcessWrapper;

internal class TestProcessBuilder
{
    public string AppProjectName => _testProjectInfo.AppProjectName;

    public BuildConfiguration BuildConfiguration { get; set; }

    public bool IsCoverletEnabled { get; set; }

    private readonly Dictionary<string, string> _arguments = new();

    private readonly Dictionary<string, string> _environmentVariables = new();

    private readonly TestProjectInfo _testProjectInfo;

    private ProcessStartInfo _processStartInfo;

    private string BinFolder => Path.Combine("bin", BuildConfiguration.ToString(), "net7.0");

    public TestProcessBuilder() => _testProjectInfo = new TestProjectInfo("");

    public TestProcessBuilder(
        string appProjectName,
        BuildConfiguration buildConfiguration,
        bool isCoverletEnabled
    )
    {
        _testProjectInfo = new TestProjectInfo(appProjectName);
        BuildConfiguration = buildConfiguration;
        IsCoverletEnabled = isCoverletEnabled;
    }

    public void AddCommandLineArgument(string argument, string value = "") =>
        _arguments[argument] = value;

    public void AddEnvironmentVariable(string name, string value) =>
        _environmentVariables[name] = value;

    public virtual ITestProcess Build()
    {
        AddCommandLineArguments(_arguments);
        AddEnvironmentVariables(_environmentVariables);

        var process = new TestProcess();
        process.StartInfo = _processStartInfo;

        return process;
    }

    public void CreateProcessStartInfo()
    {
        if (!IsCoverletEnabled)
        {
            _processStartInfo = CreateProcessStartInfo("dotnet", _testProjectInfo.AppDllName);
        }
        else
        {
            _processStartInfo = CreateProcessStartInfoWithCoverletWrapper();
        }
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

        return processStartInfo;
    }

    private ProcessStartInfo CreateProcessStartInfoWithCoverletWrapper()
    {
        var arguments =
            $"\".\" --target \"dotnet\" --targetargs \"{_testProjectInfo.AppDllName}\" --output {_testProjectInfo.CoverageReportPath} --format cobertura";

        return CreateProcessStartInfo("coverlet", arguments);
    }

    private void AddCommandLineArguments(Dictionary<string, string> arguments)
    {
        foreach (var (argument, value) in arguments)
        {
            _processStartInfo.Arguments += string.IsNullOrEmpty(value)
                ? $" {argument}"
                : $" {argument}={value}";
        }
    }

    private void AddEnvironmentVariables(Dictionary<string, string> environmentVariables)
    {
        foreach (var item in environmentVariables)
        {
            _processStartInfo.Environment.Add(item);
        }
    }
}
