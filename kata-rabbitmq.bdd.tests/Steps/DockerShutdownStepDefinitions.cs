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

            const string expectedMessageAfterRabbitMqConnected = "Enter 'stop' to shutdown the robot.";
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
            _testOutputHelper.WriteLine("Stopping robot application ...");
            _robotProcess.StandardInput.WriteLine("stop");
            _robotProcess.WaitForExit(2000);
            _robotProcess.Kill();

            _testOutputHelper.WriteLine(_robotProcess.StandardOutput.ReadToEnd());
            _testOutputHelper.WriteLine(_robotProcess.StandardError.ReadToEnd());
            
            _testOutputHelper.WriteLine("OK");
        }

        [Then("the application shuts down.")]
        public void TheApplicationShutsDown()
        {
            Assert.True(_robotProcess.HasExited);
        }
    }
}