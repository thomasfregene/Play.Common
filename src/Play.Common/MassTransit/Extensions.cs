using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MassTransit;
using MassTransit.Definition;
using Play.Common.Settings;
using System.Reflection;

namespace Play.Common.MassTransit
{
    public static class MassTransit
    {
        public static IServiceCollection AddMassTransitWithRabitMq(this IServiceCollection services)
        {
             services.AddMassTransit(configurator=>
            {
                configurator.AddConsumers(Assembly.GetEntryAssembly());
                configurator.UsingRabbitMq((context, configurator)=>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}