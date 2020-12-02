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

        private Process _process;
        
        private string _appFullDir;
        private string _appDllName;

        public ITestOutputHelper TestOutputHelper { get; set; }

        public RemoteControlledProcess(string appDllName, string appRelativeDir)
        {
            _appDllName = appDllName;
            _appFullDir = NormalizeDir(appRelativeDir);
        }

        private string NormalizeDir(string relativeDir)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var fullDir = Path.Combine(currentDirectory, relativeDir);
            var normalizedDir = Path.GetFullPath(fullDir);
            
            return normalizedDir;
        }

        public void Start()
        {
            var processStartInfo = CreateProcessStartInfo();

            TestOutputHelper?.WriteLine($"Starting .NET application {processStartInfo.Arguments} ...");

            _process = Process.Start(processStartInfo);
            Assert.NotNull(_process);
            
            TestOutputHelper?.WriteLine($"Process id: {_process.Id}, name: {_process.ProcessName}, has exited: {_process.HasExited} ...");

            WaitUntilConnectedToRabbitMq();
        }

        private ProcessStartInfo CreateProcessStartInfo()
        {
            var processStartInfo = new ProcessStartInfo("dotnet")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = _appDllName,
                WorkingDirectory = _appFullDir
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
            do
            {
                startupMessage = _process.StandardOutput.ReadLine();
                if (startupMessage != null)
                    TestOutputHelper.WriteLine(startupMessage);
            } while (startupMessage == null || !startupMessage.Contains(expectedMessageAfterRabbitMqConnected));
        }

        public void SendTermSignal()
        {
            TestOutputHelper?.WriteLine("Sending TERM signal to process ...");

            var killCommand = "kill";
            var killArguments = $"-s TERM {_process.Id}";
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