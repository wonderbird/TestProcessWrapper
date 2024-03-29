using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xunit.Abstractions;

namespace TestProcessWrapper;

public sealed class TestProcessWrapper : IDisposable
{
    #region Private members

    private readonly string _appProjectName;

    private readonly List<ReadinessCheck> _readinessChecks = new();

    private readonly Dictionary<string, string> _arguments = new();

    private readonly Dictionary<string, string> _environmentVariables = new();

    private readonly ITestProcessBuilderFactory _testProcessBuilderFactory;

    private ITestProcess _process;

    /// <summary>
    /// Id of the process under test.
    /// </summary>
    /// <see cref="ProcessIdReader"/>
    private int? _dotnetHostProcessId;

    private readonly IProcessOutputRecorderFactory _processOutputRecorderFactory =
        new ProcessOutputRecorderFactory();

    private IProcessOutputRecorder _processOutputRecorder;

    #endregion

    #region Public properties

    public string RecordedOutput => _processOutputRecorder.Output;

    public bool IsCoverletEnabled { get; set; }

    public BuildConfiguration BuildConfiguration { get; set; }

    public bool HasExited => _process == null || _process.HasExited;

    public bool IsRunning => _process is { HasExited: false };

    /// <summary>
    /// Log execution details to the SpecFlow TestOutputHelper.
    /// </summary>
    public ITestOutputHelper TestOutputHelper { get; set; }

    #endregion

    #region Create and configure TestProcessWrapper

    public TestProcessWrapper(
        string appProjectName,
        bool isCoverletEnabled,
        BuildConfiguration buildConfiguration
    )
    {
        _appProjectName = appProjectName;
        IsCoverletEnabled = isCoverletEnabled;
        BuildConfiguration = buildConfiguration;
        _testProcessBuilderFactory = new TestProcessBuilderFactory();
    }

    internal TestProcessWrapper(
        ITestProcessBuilderFactory testProcessBuilderFactory,
        IProcessOutputRecorderFactory outputRecorderFactory
    )
        : this("fakeAppProjectName", false, BuildConfiguration.Debug)
    {
        _testProcessBuilderFactory = testProcessBuilderFactory;
        _processOutputRecorderFactory = outputRecorderFactory;
    }

    public void AddEnvironmentVariable(string name, string value) =>
        _environmentVariables.Add(name, value);

    public void AddCommandLineArgument(string argument, string value = "") =>
        _arguments.Add(argument, value);

    public void AddReadinessCheck(ReadinessCheck readinessCheck) =>
        _readinessChecks.Add(readinessCheck);

    #endregion

    #region Start wrapped process

    public void Start()
    {
        var testProcessBuilder = _testProcessBuilderFactory.CreateBuilder(
            _appProjectName,
            BuildConfiguration,
            IsCoverletEnabled
        );
        testProcessBuilder.CreateProcessStartInfo();
        testProcessBuilder.AddCommandLineArguments(_arguments);
        testProcessBuilder.AddEnvironmentVariables(_environmentVariables);
        _process = testProcessBuilder.Build();

        TestOutputHelper?.WriteLine(
            $"Starting process: {_process.StartInfo.FileName} {_process.StartInfo.Arguments} in directory {_process.StartInfo.WorkingDirectory} ..."
        );
        _process.Start();

        _processOutputRecorder = _processOutputRecorderFactory.Create();
        _processOutputRecorder.StartRecording(_process);

        WaitForProcessIdAndReadinessChecks();
    }

    private void WaitForProcessIdAndReadinessChecks()
    {
        var processIdReader = new ProcessIdReader();
        AddReadinessCheck(processOutput => processIdReader.Read(processOutput));

        WaitForReadinessChecks();

        _dotnetHostProcessId = processIdReader.ProcessId;
        TestOutputHelper?.WriteLine($"Process ID: {_dotnetHostProcessId.Value}");
    }

    private void WaitForReadinessChecks()
    {
        bool isReady;
        do
        {
            isReady = _readinessChecks.All(check => check(RecordedOutput));
            Thread.Sleep(100);
        } while (!isReady);
    }

    #endregion

    #region Shutdown / Terminate wrapped process

    public void WaitForProcessExit()
    {
        TestOutputHelper?.WriteLine("Waiting for process to shutdown ...");
        _process.WaitForExit(10000);
        TestOutputHelper?.WriteLine(
            $"Process {_appProjectName} has " + (_process.HasExited ? "" : "NOT ") + "completed."
        );
    }

    public void ForceTermination()
    {
        _process.Kill();
    }

    public void ShutdownGracefully()
    {
        MurderTestProcess();
        WaitForProcessExit();
    }

    private void MurderTestProcess()
    {
        var murderFactory = new ProcessKillerFactory(TestOutputHelper);

        var murder = murderFactory.CreateProcessKillingMethod();
        var murderInProgress = murder(_dotnetHostProcessId);
        RemoveEvidenceForMurder(murderInProgress);
    }

    private void RemoveEvidenceForMurder(Process theMurder)
    {
        WaitSomeTimeForProcessToExit(theMurder);
        KillProcessIfItIsStillRunning(theMurder);
    }

    private void WaitSomeTimeForProcessToExit(Process theProcess)
    {
        if (theProcess != null)
        {
            TestOutputHelper?.WriteLine("Waiting for system call to complete.");
            theProcess.WaitForExit(2000);
        }
    }

    private void KillProcessIfItIsStillRunning(Process theProcess)
    {
        if (!theProcess.HasExited)
        {
            TestOutputHelper?.WriteLine(
                "System call has " + (theProcess.HasExited ? "" : "NOT ") + "completed."
            );
            theProcess.Kill();
        }
    }

    #endregion

    #region Implement IDispsable

    private bool _isDisposed;

    ~TestProcessWrapper()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _process?.Dispose();
            _processOutputRecorder?.Dispose();
        }

        _isDisposed = true;
    }

    #endregion
}

public delegate bool ReadinessCheck(string processOutput);
