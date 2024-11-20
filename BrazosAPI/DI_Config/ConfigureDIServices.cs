using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.DataAccess.Repository;
using BrazosApp.Utility.Helpers;
using BrazosApp.Utility.Services;
using BrazosApp.DataAccess.Initializer;

namespace BrazosAPI.DI_Config
{
    public static class ConfigureDIServices
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPasswordGenerator, PasswordGenerator>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IEncrypt, Encrypt>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
