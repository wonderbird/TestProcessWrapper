using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Xunit;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    public static class ClientProcess
    {
        public static bool HasExited => _process == null || _process.HasExited;
        
        public static bool IsRunning => _process != null && !_process.HasExited;

        private static Process _process;
        
        public static ITestOutputHelper TestOutputHelper;

        public static void Start()
        {
            var clientAppFullDir = GetClientAppFullDir();
            var clientProcessStartInfo = CreateProcessStartInfo(clientAppFullDir);

            _process = Process.Start(clientProcessStartInfo);
            Assert.NotNull(_process);

            WaitUntilConnectedToRabbitMq();
        }

        private static string GetClientAppFullDir()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var clientAppRelativeDir = Path.Combine(currentDirectory, "..", "..", "..", "..", "kata-rabbitmq.client.app", "bin", "Debug", "net5.0");
            var clientAppFullDir = Path.GetFullPath(clientAppRelativeDir);
            
            return clientAppFullDir;
        }

        private static ProcessStartInfo CreateProcessStartInfo(string clientAppFullDir)
        {
            var clientProcessStartInfo = new ProcessStartInfo("dotnet")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = $"\"kata-rabbitmq.client.app.dll\"",
                WorkingDirectory = clientAppFullDir
            };
            
            clientProcessStartInfo.AddEnvironmentVariable("RabbitMq__HostName", RabbitMq.Container.Hostname);
            clientProcessStartInfo.AddEnvironmentVariable("RabbitMq__Port", RabbitMq.Container.Port.ToString());
            clientProcessStartInfo.AddEnvironmentVariable("RabbitMq__UserName", RabbitMq.Container.Username);
            clientProcessStartInfo.AddEnvironmentVariable("RabbitMq__Password", RabbitMq.Container.Password);
            
            return clientProcessStartInfo;
        }

        private static void WaitUntilConnectedToRabbitMq()
        {
            const string expectedMessageAfterRabbitMqConnected = "Established connection to RabbitMQ";
            string startupMessage;
            do
            {
                startupMessage = _process.StandardOutput.ReadLine();
                TestOutputHelper.WriteLine(startupMessage);
            } while (startupMessage == null || !startupMessage.Contains(expectedMessageAfterRabbitMqConnected));
        }

        public static void SendTermSignal()
        {
            TestOutputHelper?.WriteLine("Sending TERM signal to client process ...");

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

            TestOutputHelper?.WriteLine("Waiting for client process to shutdown ...");
            _process.WaitForExit(2000);

            TestOutputHelper?.WriteLine("Client process has " + (_process.HasExited ? "" : "NOT ") + "completed.");
        }

        public static void Kill()
        {
            _process.Kill();
        }
    }
}