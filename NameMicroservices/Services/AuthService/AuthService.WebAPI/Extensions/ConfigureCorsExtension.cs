namespace AuthService.WebAPI.Extensions
{
    public static class ConfigureCorsExtension
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            _ = services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        _ = policy.WithOrigins()
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
            });

            return services;
        }

    }
}
