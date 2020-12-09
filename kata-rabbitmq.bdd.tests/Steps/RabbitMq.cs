using RabbitMQ.Client;

namespace katarabbitmq.bdd.tests.Steps
{
    public static class RabbitMq
    {
        public static string Hostname { get; set; }
        public static int Port { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static IConnection Connection { get; set; }
        public static IModel Channel { get; set; }
    }
}
