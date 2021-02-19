using System;
using System.Diagnostics;
using Xunit.Abstractions;

namespace RemoteControlledProcess
{
    public sealed class KillProcessFactory
    {
        public KillProcessFactory(int? dotnetHostProcessId, ITestOutputHelper testOutputHelper)
        {
            _dotnetHostProcessId = dotnetHostProcessId;
            TestOutputHelper = testOutputHelper;
        }
        private int? _dotnetHostProcessId;
        public ITestOutputHelper TestOutputHelper { get; set; }
        public Func<Process> CreateUnixStragey()
        {
            return () =>
            {
                var killCommand = "kill";
                // ReSharper disable once PossibleInvalidOperationException
                var killArguments = $"-s TERM {_dotnetHostProcessId.Value}";
                var signalName = "TERM";
                TestOutputHelper?.WriteLine($"Sending {signalName} signal to process ...");
                TestOutputHelper?.WriteLine($"Invoking system call: {killCommand} {killArguments}");
                var killProcess = Process.Start(killCommand, killArguments);
                return killProcess;
            };
        }


        public Func<Process> CreateWindowsStrategy()
        {
            return () =>
            {
                var killCommand = "taskkill";
                // ReSharper disable once PossibleInvalidOperationException
                var killArguments = $"/f /pid {_dotnetHostProcessId.Value}";

                // Under Windows, SIGINT doesn't work. Thus we use the KILL signal.
                //
                // To try this out you can place a breakpoint here and check on the
                // command line yourself.
                //
                // This can be tolerated for our case here, because the application
                // is intended to run in a linux docker container and because the
                // build pipeline uses linux containers for testing.
                var signalName = "KILL";
                TestOutputHelper?.WriteLine($"Sending {signalName} signal to process ...");
                TestOutputHelper?.WriteLine($"Invoking system call: {killCommand} {killArguments}");
                var killProcess = Process.Start(killCommand, killArguments);
                return killProcess;
            };
        }
    }
}