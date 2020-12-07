using Microsoft.Extensions.DependencyInjection;

namespace katarabbitmq.infrastructure
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
