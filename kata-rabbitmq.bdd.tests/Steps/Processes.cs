using System.IO;

namespace kata_rabbitmq.bdd.tests.Steps
{
    public static class Processes
    {
        public static RemoteControlledProcess Robot { get; } = new(Path.Combine("..", "..", "..", "..", "kata-rabbitmq.robot.app", "bin", "Debug", "net5.0"));
        public static RemoteControlledProcess Client { get; } = new(Path.Combine("..", "..", "..", "..", "kata-rabbitmq.client.app", "bin", "Debug", "net5.0"));
    }
}