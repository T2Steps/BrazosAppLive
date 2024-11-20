
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.DataAccess.Repository;
using BrazosApp.RenewalCronApp;
using BrazosApp.Utility.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wkhtmltopdf.NetCore;
using BrazosApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using BrazosApp.Utility.Helpers;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {

        using IHost host = CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        

        //var services = new ServiceCollection();
        //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("blah-blah"));


        //services.AddDbContext<NewDBContext>(options =>
        //        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //var serviceProvider = services.BuildServiceProvider();
        //_siteService = serviceProvider.GetService<SiteService>();
        //_appDbContext = serviceProvider.GetService<ApplicationDbContext>();
        //GetData();

        //services.AddDbContext<ApplicationDbContext>(options =>
        //{
        //    options.UseSqlServer(services.Configuration.GetConnectionString("dbConString"));
        //});

        try
        {
                  //Task task = services.GetRequiredService<Renewal>().Run(args);
                  services.GetRequiredService<Renewal>().Run(args);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

        }

        //Console.WriteLine("Hello World");
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["BrazosAppCronJobs"].ConnectionString), ServiceLifetime.Scoped);
                //services.AddDbContext<ApplicationDbContext>(option=>option.UseSqlServer("Server=5HUYGP;Database=BrazosTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;"), ServiceLifetime.Scoped);
                //services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer("Server=DESKTOP-29P28OS;Database=BrazosAppDb;User Id=sa;Password=12345;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=False"), ServiceLifetime.Scoped);
                services.AddWkhtmltopdf();
                services.AddScoped<IEmailSenderService, EmailSenderService>();
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
                services.AddScoped<IGeneratePdf, GeneratePdf>();
                services.AddScoped<IEncrypt, Encrypt>();
                services.AddScoped<Renewal>();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure other middleware, if needed
        }
    }
}






