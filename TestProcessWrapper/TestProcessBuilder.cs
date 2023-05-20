using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TestProcessWrapper;

internal abstract class TestProcessBuilder
{
    public string AppProjectName => TestProjectInfo.AppProjectName;

    protected readonly TestProjectInfo TestProjectInfo;
    protected ProcessStartInfo ProcessStartInfo;

    protected TestProcessBuilder() =>
        TestProjectInfo = new TestProjectInfo("fakeApplicationProject");

    protected TestProcessBuilder(string appProjectName) =>
        TestProjectInfo = new TestProjectInfo(appProjectName);

    public abstract void CreateProcessStartInfo();

    public abstract void AddCommandLineArguments(Dictionary<string, string> arguments);

    public void AddEnvironmentVariables(Dictionary<string, string> environmentVariables)
    {
        foreach (var item in environmentVariables)
        {
            ProcessStartInfo.Environment.Add(item);
        }
    }

    public virtual ITestProcess Build()
    {
        var process = new TestProcess();
        process.StartInfo = ProcessStartInfo;
        return process;
    }

    /// <summary>
    /// Encapsulate common aspects to create a ProcessStartInfo structure.
    /// </summary>
    /// <param name="processName">name of process to start</param>
    /// <param name="processArguments">arguments for the process</param>
    /// <param name="buildConfiguration">.NET build configuration used to determine the working directory</param>
    /// <returns></returns>
    protected ProcessStartInfo CreateProcessStartInfo(
        string processName,
        string processArguments,
        BuildConfiguration buildConfiguration
    )
    {
        var binFolder = Path.Combine("bin", buildConfiguration.ToString(), "net7.0");

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
}
