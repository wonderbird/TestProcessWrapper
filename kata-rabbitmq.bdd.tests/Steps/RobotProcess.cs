using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Xunit;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    public static class RobotProcess
    {
        public static bool HasExited => _process == null || _process.HasExited;
        public static bool IsRunning => _process != null && !_process.HasExited;

        private static Process _process;
        
        public static ITestOutputHelper TestOutputHelper;

        public static void Start()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var robotAppRelativeDir = Path.Combine(currentDirectory, "..", "..", "..", "..");
            var robotAppFullDir = Path.GetFullPath(robotAppRelativeDir);

            var robotProcessStartInfo = new ProcessStartInfo("dotnet")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = $"run --project \"kata-rabbitmq.robot.app\"",
                WorkingDirectory = robotAppFullDir
            };

            robotProcessStartInfo.AddEnvironmentVariable("RABBITMQ_HOSTNAME", RabbitMq.Container.Hostname);
            robotProcessStartInfo.AddEnvironmentVariable("RABBITMQ_PORT", RabbitMq.Container.Port.ToString());
            robotProcessStartInfo.AddEnvironmentVariable("RABBITMQ_USERNAME", RabbitMq.Container.Username);
            robotProcessStartInfo.AddEnvironmentVariable("RABBITMQ_PASSWORD", RabbitMq.Container.Password);
            
            _process = Process.Start(robotProcessStartInfo);
            Assert.NotNull(_process);

            const string expectedMessageAfterRabbitMqConnected = "Established connection to RabbitMQ";
            string startupMessage;
            do
            {
                startupMessage = _process.StandardOutput.ReadLine();
                TestOutputHelper.WriteLine(startupMessage);
            }
            while (startupMessage == null || !startupMessage.Contains(expectedMessageAfterRabbitMqConnected));
        }
        
        public static void SendTermSignal()
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

        public static void Kill()
        {
            _process.Kill();
        }
    }
}