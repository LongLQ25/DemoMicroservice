using Microsoft.Extensions.DependencyInjection;
using AuthService.Application.Repositories;
using AuthService.Infrastructure.Persistence.Repositories;
using AuthService.Application.Services;
using AuthService.Infrastructure.Services;

namespace AuthService.Infrastructure.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            _ = services.AddScoped<IUserRepository, UserRepository>();

            _ = services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
