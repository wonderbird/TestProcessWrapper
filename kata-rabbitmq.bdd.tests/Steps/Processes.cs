using System.IO;

namespace kata_rabbitmq.bdd.tests.Steps
{
    public static class Processes
    {
        public static RemoteControlledProcess Robot { get; } = new("kata-rabbitmq.robot.app.dll", Path.Combine("..", "..", "..", "..", "kata-rabbitmq.robot.app", "bin", "Debug", "net5.0"));
        public static RemoteControlledProcess Client { get; } = new("kata-rabbitmq.client.app.dll", Path.Combine("..", "..", "..", "..", "kata-rabbitmq.client.app", "bin", "Debug", "net5.0"));
    }
}