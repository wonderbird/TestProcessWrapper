using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xunit.Abstractions;

namespace RemoteControlledProcess
{
    public delegate bool ReadinessCheck(string processOutput);

    public class TestProcessWrapper : IDisposable
    {
        private readonly string _appProjectName;
        private readonly IProcessFactory _processFactory = new DotnetProcessFactory();

        private readonly IProcessOutputRecorderFactory _processOutputRecorderFactory =
            new ProcessOutputRecorderFactory();

        private readonly List<ReadinessCheck> _readinessChecks = new();
        private int? _dotnetHostProcessId;
        private bool _isDisposed;
        private IProcess _process;
        private IProcessOutputRecorder _processOutputRecorder;

        public TestProcessWrapper(string appProjectName, bool isCoverletEnabled)
        {
            _appProjectName = appProjectName;
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

        public ITestOutputHelper TestOutputHelper { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            _process = _processFactory.Create(_appProjectName, IsCoverletEnabled);

            TestOutputHelper?.WriteLine(
                $"Starting process: {_process.StartInfo.FileName} {_process.StartInfo.Arguments} in directory {_process.StartInfo.WorkingDirectory} ...");

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
            TestOutputHelper?.WriteLine($"Process {_appProjectName} has " +
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