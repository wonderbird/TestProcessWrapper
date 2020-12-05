namespace kata_rabbitmq.bdd.tests.Steps
{
    public static class Processes
    {
        public static RemoteControlledProcess Robot { get; } = new("kata-rabbitmq.robot.app");
        public static RemoteControlledProcess Client { get; } = new("kata-rabbitmq.client.app");
    }
}