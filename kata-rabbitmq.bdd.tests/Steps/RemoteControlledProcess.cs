using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    public class RemoteControlledProcess : IDisposable
    {
        private readonly string _appDir;

        private readonly string _appDllName;

        private readonly string _appProjectName;
        private readonly object _outputLock = new();

        private readonly string _projectDir;

        private int? _dotnetHostProcessId;

        private bool _isDisposed;
        private StringBuilder _output;
        private AutoResetEvent _outputWaitHandle;

        private Process _process;

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

        public StreamReader StandardOutput => _process.StandardOutput;

        public ITestOutputHelper TestOutputHelper { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            _process = new Process {StartInfo = CreateProcessStartInfo()};

            // Regarding reading the StandardOutput, see https://stackoverflow.com/questions/7160187/standardoutput-readtoend-hangs
            _output = new StringBuilder();
            _outputWaitHandle = new AutoResetEvent(false);
            _process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    _outputWaitHandle.Set();
                }
                else
                {
                    lock (_outputLock)
                    {
                        _output.AppendLine(e.Data);
                    }
                }
            };

            TestOutputHelper?.WriteLine(
                $"Starting process: {_process.StartInfo.FileName} {_process.StartInfo.Arguments} ...");
            _process.Start();
            _process.BeginOutputReadLine();
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
                TestOutputHelper?.WriteLine(startupMessage);
                ParseStartupMessage(startupMessage);

                Thread.Sleep(100);
            } while (!IsConnectionEstablished || !_dotnetHostProcessId.HasValue);
        }

        public string ReadOutput()
        {
            string outputString;
            lock (_outputLock)
            {
                outputString = _output.ToString();
            }

            return outputString;
        }

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

        public void SendTermSignal()
        {
            TestOutputHelper?.WriteLine("Sending TERM signal to process ...");

            const string killCommand = "kill";
            var killArguments = $"-s TERM {_dotnetHostProcessId.Value}";
            TestOutputHelper?.WriteLine($"Invoking system call: {killCommand} {killArguments}");
            var killProcess = Process.Start(killCommand, killArguments);

            if (killProcess != null)
            {
                TestOutputHelper?.WriteLine("Waiting for system call to complete.");
                killProcess.WaitForExit(2000);
                TestOutputHelper?.WriteLine("System call has " + (killProcess.HasExited ? "" : "NOT ") + "completed.");
                killProcess.Kill();
            }

            TestOutputHelper?.WriteLine("Waiting for process to shutdown ...");
            _process.WaitForExit(2000);

            TestOutputHelper?.WriteLine("Process has " + (_process.HasExited ? "" : "NOT ") + "completed.");
        }

        public void Kill()
        {
            _process.Kill();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                _process?.Dispose();
                _outputWaitHandle?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
