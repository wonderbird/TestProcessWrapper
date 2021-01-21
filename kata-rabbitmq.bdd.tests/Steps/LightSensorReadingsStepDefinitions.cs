using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using katarabbitmq.bdd.tests.Helpers;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class LightSensorReadingsStepDefinitions : IDisposable
    {
        private readonly List<RemoteControlledProcess> _clients = new();
        private readonly List<int> _countReceivedSensorReadingsByClient = new();
        private readonly ITestOutputHelper _testOutputHelper;
        private int _countSentSensorValues;
        private bool _isDisposed;
        private RemoteControlledProcess _robot;

        public LightSensorReadingsStepDefinitions(ITestOutputHelper testOutputHelper) =>
            _testOutputHelper = testOutputHelper;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Given(@"the robot and (.*) client are running")]
        [Given(@"the robot and (.*) clients are running")]
        public void GivenTheRobotAndClientsAreRunning(int numberOfClients)
        {
            for (var clientIndex = 0; clientIndex < numberOfClients; clientIndex++)
            {
                var client = new RemoteControlledProcess("kata-rabbitmq.client.app");
                client.TestOutputHelper = _testOutputHelper;
                client.Start();

                _clients.Add(client);
            }

            Assert.True(_clients.All(c => c.IsRunning));

            _robot = new RemoteControlledProcess("kata-rabbitmq.robot.app");
            _robot.TestOutputHelper = _testOutputHelper;
            _robot.Start();

            Assert.True(_robot.IsRunning);
        }

        [When(@"the robot has sent at least (.*) sensor value")]
        public async void WhenTheRobotHasSentAtLeastSensorValue(int expectedNumberOfSentSensorValues)
        {
            await WaitUntilProcessesConnectedToRabbitMq();

            WaitUntilExpectedNumberOfSensorValuesWasSent(expectedNumberOfSentSensorValues);

            ShutdownProcessesGracefully();

            ParseSensorDataFromRobotProcess();
            ParseSensorDataFromClientProcesses();
        }

        private async Task WaitUntilProcessesConnectedToRabbitMq()
        {
            bool IsConnectionEstablished() =>
                _clients.All(p => p.IsConnectionEstablished) && _robot.IsConnectionEstablished;

            while (!IsConnectionEstablished())
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1.0));
            }
        }

        private void WaitUntilExpectedNumberOfSensorValuesWasSent(int expectedNumberOfSentSensorValues)
        {
            do
            {
                ParseSensorDataFromRobotProcess();
            }
            while (_countSentSensorValues < expectedNumberOfSentSensorValues);
        }

        private void ParseSensorDataFromRobotProcess()
        {
            var output = _robot.ReadOutput();
            var lines = output.Split('\n').ToList();
            _countSentSensorValues = lines.Count(l => l.Contains("Sensor data"));
        }

        private void ParseSensorDataFromClientProcesses()
        {
            foreach (var client in _clients)
            {
                var output = client.ReadOutput();
                var lines = output.Split('\n').ToList();
                var countReceivedSensorReadings = lines.Count(l => l.Contains("Sensor data"));
                _countReceivedSensorReadingsByClient.Add(countReceivedSensorReadings);
            }
        }

        private void ShutdownProcessesGracefully()
        {
            _robot.ShutdownGracefully();

            foreach (var client in _clients)
            {
                client.ShutdownGracefully();
            }
        }

        [Then]
        public void ThenEachClientReceivedAllSentSensorValues()
        {
            _testOutputHelper.WriteLine($"Robot has sent {_countSentSensorValues} values.");
            _testOutputHelper.WriteLine($"The client(s) received {string.Join(",", _countReceivedSensorReadingsByClient)} values");

            Assert.True(_countReceivedSensorReadingsByClient.All(c => c == _countSentSensorValues),
                $"Each client app must receive exactly {_countSentSensorValues} sensor value(s).");
        }

        [AfterScenario("LightSensorReadings")]
        public void ForceProcessTermination()
        {
            _robot.ForceTermination();

            foreach (var client in _clients)
            {
                client.ForceTermination();
            }
        }

        ~LightSensorReadingsStepDefinitions()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                _robot?.Dispose();
            }

            _isDisposed = true;
        }
    }
}