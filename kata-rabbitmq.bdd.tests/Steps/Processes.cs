using System.IO;

namespace kata_rabbitmq.bdd.tests.Steps
{
    public static class Processes
    {
        public static RemoteControlledProcess Robot { get; } = new(Path.Combine("..", "..", "..", "..", "kata-rabbitmq.robot.app", "bin", "Debug", "net5.0"));
    }
}