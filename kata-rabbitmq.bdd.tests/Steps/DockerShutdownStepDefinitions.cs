using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class DockerShutdownStepDefinitions
    {
        private ITestOutputHelper _testOutputHelper;
        private Process _robotProcess;
        private bool _hasExitedGracefully;

        public DockerShutdownStepDefinitions(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Given("the server is running")]
        public void GivenTheServerIsRunning()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var robotAppRelativeDir = Path.Combine(currentDirectory, "..", "..", "..", "..");
            var robotAppFullDir = Path.GetFullPath(robotAppRelativeDir);

            var robotProcessStartInfo = new ProcessStartInfo("dotnet");
            robotProcessStartInfo.UseShellExecute = false;
            robotProcessStartInfo.RedirectStandardInput = true;
            robotProcessStartInfo.RedirectStandardOutput = true;
            robotProcessStartInfo.RedirectStandardError = true;
            robotProcessStartInfo.Arguments = $"run --project \"kata-rabbitmq.robot.app\"";
            robotProcessStartInfo.WorkingDirectory = robotAppFullDir;

            robotProcessStartInfo.AddEnvironmentVariable("RABBITMQ_HOSTNAME", RabbitMq.Container.Hostname);
            robotProcessStartInfo.AddEnvironmentVariable("RABBITMQ_PORT", RabbitMq.Container.Port.ToString());
            robotProcessStartInfo.AddEnvironmentVariable("RABBITMQ_USERNAME", RabbitMq.Container.Username);
            robotProcessStartInfo.AddEnvironmentVariable("RABBITMQ_PASSWORD", RabbitMq.Container.Password);
            
            _robotProcess = Process.Start(robotProcessStartInfo);
            Assert.NotNull(_robotProcess);

            const string expectedMessageAfterRabbitMqConnected = "Established connection to RabbitMQ";
            string startupMessage;
            do
            {
                startupMessage = _robotProcess.StandardOutput.ReadLine();
                _testOutputHelper.WriteLine(startupMessage);
            }
            while (!startupMessage.Contains(expectedMessageAfterRabbitMqConnected));
        }

        [When("a TERM signal is sent")]
        public void WhenATermSignalIsSent()
        {
            _testOutputHelper.WriteLine("Sending TERM signal to robot process ...");

            var killCommand = "kill";
            var killArguments = $"-s TERM {_robotProcess.Id}";
            _testOutputHelper.WriteLine($"Invoking system call: {killCommand} {killArguments}");
            var killProcess = Process.Start(killCommand, killArguments);
            
            if (killProcess != null)
            {
                _testOutputHelper.WriteLine("Waiting for system call to complete.");
                killProcess.WaitForExit(2000);
                _testOutputHelper.WriteLine("System call has " + (killProcess.HasExited ? "" : "NOT ") + "completed.");
                killProcess.Kill();
            }

            _testOutputHelper.WriteLine("Waiting for robot process to shutdown ...");
            _robotProcess.WaitForExit(2000);
            _hasExitedGracefully = _robotProcess.HasExited;

            _testOutputHelper.WriteLine("Robot process has " + (killProcess.HasExited ? "" : "NOT ") + "completed.");
            _robotProcess.Kill();
        }

        [Then("the application shuts down.")]
        public void TheApplicationShutsDown()
        {
            Assert.True(_hasExitedGracefully);
        }
    }
}