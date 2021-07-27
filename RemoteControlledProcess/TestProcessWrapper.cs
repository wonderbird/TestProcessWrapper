using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit.Abstractions;

namespace RemoteControlledProcess
{
    public delegate bool ReadinessCheck(string processOutput);

    public class TestProcessWrapper : IDisposable
    {
        private readonly IProcessFactory _processFactory = new ProcessFactory();

        private readonly IProcessOutputRecorderFactory _processOutputRecorderFactory =
            new ProcessOutputRecorderFactory();

        private readonly List<ReadinessCheck> _readinessChecks = new();
        private readonly TestProjectInfo _testProjectInfo;
        private int? _dotnetHostProcessId;
        private bool _isDisposed;
        private IProcess _process;
        private IProcessOutputRecorder _processOutputRecorder;

        public TestProcessWrapper(string appProjectName, bool isCoverletEnabled)
        {
            _testProjectInfo = new TestProjectInfo(appProjectName);
            IsCoverletEnabled = isCoverletEnabled;
        }

        internal TestProcessWrapper(IProcessFactory processFactory, IProcessOutputRecorderFactory outputRecorderFactory)
            : this("fakeProjectName", false)
        {
            _processFactory = processFactory;
            _processOutputRecorderFactory = outputRecorderFactory;
        }

        public bool IsCoverletEnabled { get; }

        public bool HasExited => _process == null || _process.HasExited;

        public bool IsRunning => _process != null && !_process.HasExited;

        private static string BinFolder
        {
            get
            {
#if DEBUG
                var binFolder = Path.Combine("bin", "Debug", "net5.0");
#else
                var binFolder = Path.Combine("bin", "Release", "net5.0");
#endif
                return binFolder;
            }
        }

        public ITestOutputHelper TestOutputHelper { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            _process = _processFactory.CreateProcess();
            _process.StartInfo = CreateProcessStartInfo();

            TestOutputHelper?.WriteLine(
                $"Starting process: {_process.StartInfo.FileName} {_process.StartInfo.Arguments} ...");
            _process.Start();

            _processOutputRecorder = _processOutputRecorderFactory.Create();
            _processOutputRecorder.StartRecording(_process);

            TestOutputHelper?.WriteLine($"Process ID: {_process.Id} has exited: {_process.HasExited} ...");

            WaitForProcessIdAndReadinessChecks();
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

        private ProcessStartInfo CreateProcessStartInfoWithCoverletWrapper()
        {
            var arguments =
                $"\".\" --target \"dotnet\" --targetargs \"{_testProjectInfo.AppDllName}\" --output {_testProjectInfo.CoverageReportPath} --format cobertura";

            return CreateProcessStartInfo("coverlet", arguments);
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
                WorkingDirectory = Path.Combine(_testProjectInfo.ProjectDir, _testProjectInfo.AppProjectName,
                    BinFolder)
            };

            TestOutputHelper?.WriteLine($".NET Application: {processStartInfo.Arguments}");
            TestOutputHelper?.WriteLine($"Application path: {processStartInfo.WorkingDirectory}");
            return processStartInfo;
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
                var processOutput = ReadOutput();
                isReady = _readinessChecks.All(check => check(processOutput));
                Thread.Sleep(100);
            }
            while (!isReady);
        }

        public string ReadOutput() => _processOutputRecorder.Output;

        public void AddReadinessCheck(ReadinessCheck readinessCheck)
        {
            _readinessChecks.Add(readinessCheck);
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
                TestOutputHelper?.WriteLine("System call has " + (theProcess.HasExited ? "" : "NOT ") + "completed.");
                theProcess.Kill();
            }
        }

        private void WaitForProcessExit()
        {
            TestOutputHelper?.WriteLine("Waiting for process to shutdown ...");
            _process.WaitForExit(2000);
            TestOutputHelper?.WriteLine($"Process {_testProjectInfo.AppProjectName} has " +
                                        (_process.HasExited ? "" : "NOT ") +
                                        "completed.");
        }

        public void ForceTermination()
        {
            _process.Kill();
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

        ~TestProcessWrapper()
        {
            Dispose(false);
        }
    }
}