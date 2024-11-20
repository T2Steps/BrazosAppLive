using BrazosApp.DataAccess.Initializer;
using BrazosApp.Utility.Helpers;
using BrazosApp.DataAccess.Repository;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Utility.Services;
using BrazosApp.Services;

namespace BrazosApp.DI_Config
{

    public static class ConfigureDIServices
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPasswordGenerator, PasswordGenerator>();
            services.AddScoped<IEncrypt, Encrypt>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IJetPayService, JetPayService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped(typeof(IPoolRepository<>), typeof(PoolRepository<>));
            return services;
        }
    }
}
