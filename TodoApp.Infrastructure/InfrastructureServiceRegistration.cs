using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Contracts.Logging;
using TodoApp.Infrastructure.Logging;

namespace TodoApp.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        return services;
    }
}