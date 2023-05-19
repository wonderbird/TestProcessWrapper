using System.Collections.Generic;
using System.Diagnostics;

namespace TestProcessWrapper;

internal abstract class TestProcessBuilder
{
    public string AppProjectName => TestProjectInfo.AppProjectName;

    protected readonly TestProjectInfo TestProjectInfo;
    protected ProcessStartInfo ProcessStartInfo;

    protected TestProcessBuilder() => TestProjectInfo = new TestProjectInfo("");

    protected TestProcessBuilder(string appProjectName) =>
        TestProjectInfo = new TestProjectInfo(appProjectName);

    public abstract void CreateStartInfo();

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
}
