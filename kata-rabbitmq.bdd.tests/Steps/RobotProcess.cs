using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Xunit;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    public static class RobotProcess
    {
        public static RemoteControlledProcess Process { get; } = new();
    }
}