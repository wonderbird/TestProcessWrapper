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
    public class LightSensorReadingsStepDefinitions
    {
        private readonly List<int> _countReceivedSensorReadingsByClient = new();
        private readonly ITestOutputHelper _testOutputHelper;
        private int _countSentSensorValues;

        public LightSensorReadingsStepDefinitions(ITestOutputHelper testOutputHelper) =>
            _testOutputHelper = testOutputHelper;

        [When(@"the robot has sent at least (.*) sensor value")]
        public void WhenTheRobotHasSentAtLeastSensorValue(int expectedNumberOfSentSensorValues)
        {
            WaitUntilExpectedNumberOfSensorValuesWasSent(expectedNumberOfSentSensorValues);

            SharedStepDefinitions.ShutdownProcessesGracefully();

            ParseSensorDataFromRobotProcess();
            ParseSensorDataFromClientProcesses();
        }

        private async void WaitUntilExpectedNumberOfSensorValuesWasSent(int expectedNumberOfSentSensorValues)
        {
            do
            {
                ParseSensorDataFromRobotProcess();
                await Task.Delay(TimeSpan.FromMilliseconds(1));
            }
            while (_countSentSensorValues < expectedNumberOfSentSensorValues);
        }

        private void ParseSensorDataFromRobotProcess()
        {
            var output = SharedStepDefinitions.Robot.ReadOutput();
            var lines = output.Split('\n').ToList();
            _countSentSensorValues = lines.Count(l => l.Contains("Sensor data"));
        }

        private void ParseSensorDataFromClientProcesses()
        {
            foreach (var client in SharedStepDefinitions.Clients)
            {
                var output = client.ReadOutput();
                var lines = output.Split('\n').ToList();
                var countReceivedSensorReadings = lines.Count(l => l.Contains("Sensor data"));
                _countReceivedSensorReadingsByClient.Add(countReceivedSensorReadings);
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
    }
}