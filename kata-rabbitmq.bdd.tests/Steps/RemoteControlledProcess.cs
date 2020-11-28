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
        
        private string _appRelativeDir;

        public ITestOutputHelper TestOutputHelper { get; set; }

        public RemoteControlledProcess(string appRelativeDir)
        {
            _appRelativeDir = appRelativeDir;
        }
        public void Start()
        {
            var robotAppFullDir = NormalizeAppFullDir();
            var robotProcessStartInfo = CreateProcessStartInfo(robotAppFullDir);

            _process = Process.Start(robotProcessStartInfo);
            Assert.NotNull(_process);

            WaitUntilConnectedToRabbitMq();
        }

        private string NormalizeAppFullDir()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var appFullDir = Path.Combine(currentDirectory, _appRelativeDir);
            var appNormalizedDir = Path.GetFullPath(appFullDir);
            
            return appNormalizedDir;
        }

        private ProcessStartInfo CreateProcessStartInfo(string robotAppFullDir)
        {
            var robotProcessStartInfo = new ProcessStartInfo("dotnet")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = $"\"kata-rabbitmq.robot.app.dll\"",
                WorkingDirectory = robotAppFullDir
            };
            
            robotProcessStartInfo.AddEnvironmentVariable("RabbitMq__HostName", RabbitMq.Container.Hostname);
            robotProcessStartInfo.AddEnvironmentVariable("RabbitMq__Port", RabbitMq.Container.Port.ToString());
            robotProcessStartInfo.AddEnvironmentVariable("RabbitMq__UserName", RabbitMq.Container.Username);
            robotProcessStartInfo.AddEnvironmentVariable("RabbitMq__Password", RabbitMq.Container.Password);
            
            return robotProcessStartInfo;
        }

        private void WaitUntilConnectedToRabbitMq()
        {
            const string expectedMessageAfterRabbitMqConnected = "Established connection to RabbitMQ";
            string startupMessage;
            do
            {
                startupMessage = _process.StandardOutput.ReadLine();
                TestOutputHelper.WriteLine(startupMessage);
            } while (startupMessage == null || !startupMessage.Contains(expectedMessageAfterRabbitMqConnected));
        }

        public void SendTermSignal()
        {
            TestOutputHelper?.WriteLine("Sending TERM signal to robot process ...");

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

            TestOutputHelper?.WriteLine("Waiting for robot process to shutdown ...");
            _process.WaitForExit(2000);

            TestOutputHelper?.WriteLine("Robot process has " + (_process.HasExited ? "" : "NOT ") + "completed.");
        }

        public void Kill()
        {
            _process.Kill();
        }
    }
}