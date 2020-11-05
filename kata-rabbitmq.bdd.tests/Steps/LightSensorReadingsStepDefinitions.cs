using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class LightSensorReadingsStepDefinitions
    {
        private bool _isSensorQueuePresent;
        private ITestOutputHelper _testOutputHelper;
        private Process _robotProcess;

        public LightSensorReadingsStepDefinitions(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [AfterScenario("LightSensorReadings")]
        public void AfterScenario()
        {
            _testOutputHelper.WriteLine("Stopping robot application ...");
            _robotProcess.StandardInput.WriteLine("");
            _robotProcess.WaitForExit(2000);
            _robotProcess.Kill();

            _testOutputHelper.WriteLine(_robotProcess.StandardOutput.ReadToEnd());
            _testOutputHelper.WriteLine(_robotProcess.StandardError.ReadToEnd());
            
            _testOutputHelper.WriteLine("OK");
        }

        [Given("the robot app is started")]
        public void GivenTheRobotAppIsStarted()
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

            const string expectedMessageAfterRabbitMqConnected = "Press ENTER to shutdown the robot.";
            string startupMessage;
            do
            {
                startupMessage = _robotProcess.StandardOutput.ReadLine();
                _testOutputHelper.WriteLine(startupMessage);
            }
            while (!startupMessage.Contains(expectedMessageAfterRabbitMqConnected));
        }
        
        [When("the sensor queue is checked")]
        public void WhenTheSensorQueueIsChecked()
        {
            try
            {
                _testOutputHelper.WriteLine("Testing whether robot:sensors exists ...");
                RabbitMq.Channel.ExchangeDeclarePassive("robot");
                RabbitMq.Channel.QueueDeclarePassive("sensors");

                _testOutputHelper.WriteLine("robot:sensors exists");
                _isSensorQueuePresent = true;
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine($"robot:sensors does not exist. Exception: {e.Message}");
                _isSensorQueuePresent = false;
            }
        }

        [Then("the sensor queue exists")]
        public void ThenTheSensorsQueueExists()
        {
            Assert.True(_isSensorQueuePresent);
        }
    }
}
