using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Xunit;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    public class RemoteControlledProcess
    {
        public bool HasExited => _process == null || _process.HasExited;
        
        public bool IsRunning => _process != null && !_process.HasExited;

        private static string BinFolder
        {
            get
            {
                string binFolder;
#if DEBUG
                binFolder = Path.Combine("bin", "Debug", "net5.0");
#else
                binFolder = Path.Combine("bin", "Release", "net5.0");
#endif
                return binFolder;
            }
        }

        private string _projectDir;
        
        private Process _process;

        private string _appProjectName;

        private string _appDir;

        private string _appDllName;

        private int _dotnetHostProcessId;

        public ITestOutputHelper TestOutputHelper { get; set; }

        public RemoteControlledProcess(string appProjectName)
        {
            var projectRelativeDir = Path.Combine("..", "..", "..", "..");
            _projectDir = Path.GetFullPath(projectRelativeDir);
            
            _appProjectName = appProjectName;

            _appDllName = _appProjectName + ".dll";

            _appDir = Path.Combine(_projectDir, _appProjectName, BinFolder);
        }

        public void Start()
        {
            var processStartInfo = CreateProcessStartInfo();

            TestOutputHelper?.WriteLine($"Starting .NET application: {processStartInfo.FileName} {processStartInfo.Arguments} ...");

            _process = Process.Start(processStartInfo);
            Assert.NotNull(_process);
            
            TestOutputHelper?.WriteLine($"Process ID: {_process.Id} has exited: {_process.HasExited} ...");

            WaitUntilConnectedToRabbitMq();
        }

        private ProcessStartInfo CreateProcessStartInfo()
        {
            var coverageReportFileName = $"{_appProjectName}.{Guid.NewGuid().ToString()}.xml";
            var coverageReportPath = Path.Combine(_projectDir, "kata-rabbitmq.bdd.tests", "TestResults", coverageReportFileName);
            
            var processStartInfo = new ProcessStartInfo("coverlet")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = $"\".\" --target \"dotnet\" --targetargs \"{_appDllName}\" --output {coverageReportPath} --format cobertura",
                WorkingDirectory = _appDir
            };
            
            processStartInfo.AddEnvironmentVariable("RabbitMq__HostName", RabbitMq.Container.Hostname);
            processStartInfo.AddEnvironmentVariable("RabbitMq__Port", RabbitMq.Container.Port.ToString());
            processStartInfo.AddEnvironmentVariable("RabbitMq__UserName", RabbitMq.Container.Username);
            processStartInfo.AddEnvironmentVariable("RabbitMq__Password", RabbitMq.Container.Password);
            
            TestOutputHelper?.WriteLine($".NET Application: {processStartInfo.Arguments}");
            TestOutputHelper?.WriteLine($"Application path: {processStartInfo.WorkingDirectory}");
            
            return processStartInfo;
        }

        private void WaitUntilConnectedToRabbitMq()
        {
            const string expectedMessageAfterRabbitMqConnected = "Established connection to RabbitMQ";
            string startupMessage;
            bool isConnectionEstablished = false;
            bool isDotnetHostProcessIdKnown = false;
            do
            {
                startupMessage = _process.StandardOutput.ReadLine();
                if (startupMessage != null)
                {
                    TestOutputHelper?.WriteLine(startupMessage);

                    if (!isConnectionEstablished)
                    {
                        isConnectionEstablished = startupMessage.Contains(expectedMessageAfterRabbitMqConnected);
                    }

                    if (!isDotnetHostProcessIdKnown && startupMessage.Contains("Process ID"))
                    {
                        var processIdStartIndex = startupMessage.IndexOf("Process ID", StringComparison.Ordinal);
                        var processIdString = startupMessage.Substring(processIdStartIndex + 10);
                        _dotnetHostProcessId = int.Parse(processIdString);
                        isDotnetHostProcessIdKnown = true;
                    
                        TestOutputHelper?.WriteLine($"Process ID: {_dotnetHostProcessId}");
                    }
                }
            }
            while (!isConnectionEstablished || !isDotnetHostProcessIdKnown);
        }

        public void SendTermSignal()
        {
            TestOutputHelper?.WriteLine("Sending TERM signal to process ...");

            var killCommand = "kill";
            var killArguments = $"-s TERM {_dotnetHostProcessId}";
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
    }
}