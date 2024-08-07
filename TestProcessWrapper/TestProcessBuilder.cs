using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TestProcessWrapper;

internal abstract class TestProcessBuilder
{
    protected readonly TestProjectInfo TestProjectInfo;
    protected ProcessStartInfo ProcessStartInfo;

    /// <summary>
    /// A default constructor is required when a class shall be mocked with Moq.
    /// </summary>
    protected TestProcessBuilder() => TestProjectInfo = new TestProjectInfo("fakeAppProjectName");

    protected TestProcessBuilder(string appProjectName) =>
        TestProjectInfo = new TestProjectInfo(appProjectName);

    /// <summary>
    /// Initialize the <see cref="ProcessStartInfo"/> property.
    /// </summary>
    public abstract void CreateProcessStartInfo();

    /// <summary>
    /// Add command line arguments and values to <see cref="ProcessStartInfo"/>.
    /// </summary>
    /// <param name="arguments">
    ///     Dictionary of argument name and corresponding value.
    ///     If an argument represents a boolean option, then value is an empty string.
    /// </param>
    public abstract void AddCommandLineArguments(Dictionary<string, string> arguments);

    /// <summary>
    /// Add environment variables to <see cref="ProcessStartInfo"/>.
    /// </summary>
    /// <param name="environmentVariables">
    ///     Dictionary of environment variable name and corresponding value.
    /// </param>
    public void AddEnvironmentVariables(Dictionary<string, string> environmentVariables)
    {
        foreach (var item in environmentVariables)
        {
            ProcessStartInfo.Environment.Add(item);
        }
    }

    /// <summary>
    /// Create the <see cref="ITestProcess"/> from <see cref="ProcessStartInfo"/>.
    /// </summary>
    /// <remarks>
    /// This method must be virtual so that Moq allows mocking it in unit tests.
    /// </remarks>
    /// <returns>Created <see cref="ITestProcess"/> instance</returns>
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
        #if NET8_0_OR_GREATER
            var binFolder = Path.Combine("bin", buildConfiguration.ToString(), "net8.0");
        #elif NET7_0_OR_GREATER
            var binFolder = Path.Combine("bin", buildConfiguration.ToString(), "net7.0");
        #else
            var binFolder = Path.Combine("bin", buildConfiguration.ToString(), "net6.0");
        #endif

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
