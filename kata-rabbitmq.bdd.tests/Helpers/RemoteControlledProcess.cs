using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Helpers
{
    public sealed class RemoteControlledProcess : IDisposable
    {
        private readonly string _appDir;

        private readonly string _appDllName;

        private readonly string _appProjectName;

        private readonly string _projectDir;

        private int? _dotnetHostProcessId;

        private bool _isDisposed;

        private Process _process;
        private ProcessStreamBuffer _processStreamBuffer;

        public RemoteControlledProcess(string appProjectName)
        {
            var projectRelativeDir = Path.Combine("..", "..", "..", "..");
            _projectDir = Path.GetFullPath(projectRelativeDir);

            _appProjectName = appProjectName;

            _appDllName = _appProjectName + ".dll";

            _appDir = Path.Combine(_projectDir, _appProjectName, BinFolder);
        }

        public bool IsConnectionEstablished { get; private set; }

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

        ~RemoteControlledProcess()
        {
            Dispose(false);
        }

        public void Start()
        {
            _process = new Process { StartInfo = CreateProcessStartInfo() };

            TestOutputHelper?.WriteLine(
                $"Starting process: {_process.StartInfo.FileName} {_process.StartInfo.Arguments} ...");
            _process.Start();

            _processStreamBuffer = new ProcessStreamBuffer();
            _processStreamBuffer.BeginCapturing(_process.BeginOutputReadLine,
                handler => _process.OutputDataReceived += handler, handler => _process.OutputDataReceived -= handler);

            TestOutputHelper?.WriteLine($"Process ID: {_process.Id} has exited: {_process.HasExited} ...");

            WaitAndProcessRequiredStartupMessages();
        }

        private ProcessStartInfo CreateProcessStartInfo()
        {
            var coverageReportFileName = $"{_appProjectName}.{Guid.NewGuid().ToString()}.xml";
            var coverageReportPath = Path.Combine(_projectDir, "kata-rabbitmq.bdd.tests", "TestResults",
                coverageReportFileName);

            var processStartInfo = new ProcessStartInfo("coverlet")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments =
                    $"\".\" --target \"dotnet\" --targetargs \"{_appDllName}\" --output {coverageReportPath} --format cobertura",
                WorkingDirectory = _appDir
            };

            processStartInfo.AddEnvironmentVariable("RabbitMq__HostName", RabbitMq.Hostname);
            processStartInfo.AddEnvironmentVariable("RabbitMq__Port",
                RabbitMq.Port.ToString(CultureInfo.InvariantCulture));
            processStartInfo.AddEnvironmentVariable("RabbitMq__UserName", RabbitMq.Username);
            processStartInfo.AddEnvironmentVariable("RabbitMq__Password", RabbitMq.Password);

            TestOutputHelper?.WriteLine($".NET Application: {processStartInfo.Arguments}");
            TestOutputHelper?.WriteLine($"Application path: {processStartInfo.WorkingDirectory}");

            return processStartInfo;
        }

        private void WaitAndProcessRequiredStartupMessages()
        {
            do
            {
                var startupMessage = ReadOutput();
                ParseStartupMessage(startupMessage);

                Thread.Sleep(100);
            }
            while (!IsConnectionEstablished || !_dotnetHostProcessId.HasValue);
        }

        public string ReadOutput() => _processStreamBuffer.StreamContent;

        private void ParseStartupMessage(string startupMessage)
        {
            const string expectedMessageAfterRabbitMqConnected = "Established connection to RabbitMQ";

            if (!IsConnectionEstablished)
            {
                IsConnectionEstablished = startupMessage.Contains(expectedMessageAfterRabbitMqConnected);
            }

            if (!_dotnetHostProcessId.HasValue && startupMessage.Contains("Process ID"))
            {
                var processIdStartIndex = startupMessage.IndexOf("Process ID", StringComparison.Ordinal);
                var newLineAfterProcessIdIndex =
                    startupMessage.IndexOf("\n", processIdStartIndex, StringComparison.Ordinal);
                var processIdNumberOfDigits = newLineAfterProcessIdIndex - processIdStartIndex - 10;
                var processIdString = startupMessage.Substring(processIdStartIndex + 10, processIdNumberOfDigits);
                _dotnetHostProcessId = int.Parse(processIdString, NumberStyles.Integer, CultureInfo.InvariantCulture);
                TestOutputHelper?.WriteLine($"Process ID: {_dotnetHostProcessId.Value}");
            }
        }

        public void ShutdownGracefully()
        {
            SendTermSignalToProcess();
            WaitForProcessExit();
        }

        private void SendTermSignalToProcess()
        {
            var killProcess = InvokeKillSystemCall();
            WaitForKillSystemCallToFinish(killProcess);
            TerminateKillSystemCall(killProcess);
        }

        private Process InvokeKillSystemCall()
        {
            string killCommand;
            string killArguments;
            string signalName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                killCommand = "taskkill";
                killArguments = $"/f /pid {_dotnetHostProcessId.Value}";

                // Under Windows, SIGINT doesn't work. Thus we use the KILL signal.
                //
                // To try this out you can place a breakpoint here and check on the
                // command line yourself.
                //
                // This can be tolerated for our case here, because the application
                // is intended to run in a linux docker container and because the
                // build pipeline uses linux containers for testing.
                signalName = "KILL";
            }
            else
            {
                killCommand = "kill";
                killArguments = $"-s TERM {_dotnetHostProcessId.Value}";
                signalName = "TERM";
            }

            TestOutputHelper?.WriteLine($"Sending {signalName} signal to process ...");
            TestOutputHelper?.WriteLine($"Invoking system call: {killCommand} {killArguments}");
            var killProcess = Process.Start(killCommand, killArguments);
            return killProcess;
        }

        private void WaitForKillSystemCallToFinish(Process killProcess)
        {
            if (killProcess != null)
            {
                TestOutputHelper?.WriteLine("Waiting for system call to complete.");
                killProcess.WaitForExit(2000);
            }
        }

        private void TerminateKillSystemCall(Process killProcess)
        {
            if (!killProcess.HasExited)
            {
                TestOutputHelper?.WriteLine("System call has " + (killProcess.HasExited ? "" : "NOT ") + "completed.");
                killProcess.Kill();
            }
        }

        private void WaitForProcessExit()
        {
            TestOutputHelper?.WriteLine("Waiting for process to shutdown ...");
            _process.WaitForExit(2000);
            TestOutputHelper?.WriteLine($"Process {_appProjectName} has " + (_process.HasExited ? "" : "NOT ") + "completed.");
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
                _processStreamBuffer?.Dispose();
            }

            _isDisposed = true;
        }
    }
}