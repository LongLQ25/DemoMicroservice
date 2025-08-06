using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthService.Infrastructure.Options;
using AuthService.Infrastructure.Persistence.DBContext;

namespace AuthService.Infrastructure.Extensions
{
    public static class ConfigureDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseOptions = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
            _ = services.AddDbContext<AppDBContext>(options =>
            {
                _ = options.UseSqlServer(databaseOptions?.ConnectionStrings);
                _ = options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
