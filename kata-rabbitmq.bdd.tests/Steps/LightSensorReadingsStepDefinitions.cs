using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using katarabbitmq.bdd.tests.Helpers;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class LightSensorReadingsStepDefinitions
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private int _countReceivedSensorReadings;

        private List<RemoteControlledProcess> _clients = new();
        private RemoteControlledProcess _robot;

        public LightSensorReadingsStepDefinitions(ITestOutputHelper testOutputHelper) =>
            _testOutputHelper = testOutputHelper;

        [Given(@"the server and (.*) client are running")]
        [Given(@"the server and (.*) clients are running")]
        public void GivenTheServerAndClientAreRunning(int numberOfClients)
        {
            _robot = new("kata-rabbitmq.robot.app");
            _robot.TestOutputHelper = _testOutputHelper;
            _robot.Start();

            for (var clientIndex = 0; clientIndex < numberOfClients; clientIndex++)
            {
                var client = new RemoteControlledProcess("kata-rabbitmq.client.app");
                client.TestOutputHelper = _testOutputHelper;
                client.Start();

                _clients.Add(client);
            }

            Assert.True(_clients.All(c => c.IsRunning));
            Assert.True(_robot.IsRunning);
        }

        [When(@"the robot and client apps have been connected for (.*) seconds")]
        public async Task WhenTheRobotAndClientAppsHaveBeenConnectedForSeconds(double seconds)
        {
            await WaitUntilProcessesConnectedToRabbitMq();

            await WaitForSeconds(seconds);

            ParseSensorDataFromClientProcesses();
        }

        private async Task WaitUntilProcessesConnectedToRabbitMq()
        {
            bool IsConnectionEstablished() => _clients.All(p => p.IsConnectionEstablished) && _robot.IsConnectionEstablished;

            while (!IsConnectionEstablished())
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1.0));
            }
        }

        private async Task WaitForSeconds(double seconds)
        {
            var stopwatch = Stopwatch.StartNew();
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            stopwatch.Stop();

            _testOutputHelper?.WriteLine($"Waited for {stopwatch.ElapsedMilliseconds / 1000.0} seconds");
        }

        private void ParseSensorDataFromClientProcesses()
        {
            // TODO: Parse the output of all clients
            var output = _clients[0].ReadOutput();
            var lines = output.Split('\n').ToList();
            _countReceivedSensorReadings = lines.Count(l => l.Contains("Sensor data"));
        }

        [Then(@"each client app received at least (.*) sensor values")]
        public void ThenEachClientAppReceivedAtLeastSensorValues(int expectedSensorValuesCount)
        {
            _testOutputHelper.WriteLine($"Received {_countReceivedSensorReadings} values");
            Assert.True(_countReceivedSensorReadings >= expectedSensorValuesCount,
                $"Client app must receive at least {expectedSensorValuesCount} sensor value(s). It actually received {_countReceivedSensorReadings} values");
        }

        [AfterScenario("LightSensorReadings")]
        public void StopProcesses()
        {
            _robot.ShutdownGracefully();
            _robot.ForceTermination();

            foreach (var client in _clients)
            {
                client.ShutdownGracefully();
                client.ForceTermination();
            }
        }
    }
}