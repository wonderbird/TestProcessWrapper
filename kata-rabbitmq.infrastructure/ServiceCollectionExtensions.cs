using Microsoft.Extensions.DependencyInjection;

namespace kata_rabbitmq.infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IRabbitMqConnection, RabbitMqConnection>();

            return services;
        }
    }
}