using System.Collections.Generic;

namespace katarabbitmq.bdd.tests.Helpers
{
    public static class Processes
    {
        public static RemoteControlledProcess Robot { get; } = new("kata-rabbitmq.robot.app");
        public static RemoteControlledProcess Client { get; } = new("kata-rabbitmq.client.app");
        public static IList<RemoteControlledProcess> Clients { get; } = new List<RemoteControlledProcess>();
    }
}