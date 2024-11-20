using BrazosApp.DataAccess.Data;
using BrazosApp.DataAccess.Initializer;
using BrazosApp.DI_Config;
using BrazosApp.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Wkhtmltopdf.NetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConString"));
});
//builder.Services.AddDbContext<PoolDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConString"));
//});
//builder.Services.AddDbContext<DayCareDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConString"));
//});

// Add services to the container.
builder.Services.AddAllServices();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //option.DefaultChallengeScheme = option.AddScheme("InspectorLoginScheme")
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(option =>
{
    option.LoginPath = "/Login";
    option.LogoutPath = "/Home/Index";
    option.AccessDeniedPath = "/Accessed_Denied";
    option.ReturnUrlParameter = "returnUrl";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(45);
    option.SlidingExpiration = false;
    option.Cookie.HttpOnly = true;
    option.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    //option.Events = new CookieAuthenticationEvents
    //{
    //    OnRedirectToLogin = context =>
    //    {

    //        if (context.Request.Path.StartsWithSegments("/Inspector"))
    //        {
    //            context.Response.Redirect("/InspectorLogin?returnUrl=" + context.Request.Path);
    //        }
    //        else
    //        {
    //            context.Response.Redirect("/Login?returnUrl=" + context.Request.Path);
    //        }
    //        return Task.CompletedTask;
    //    }
    //};
})
.AddCookie("InspectorLoginScheme", option =>
{
    option.LoginPath = "/InspectorLogin";
    option.LogoutPath = "/Home/Index";
    option.AccessDeniedPath = "/Accessed_Denied";
    option.ReturnUrlParameter = "returnUrl";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(45);
    option.SlidingExpiration = false;
    option.Cookie.HttpOnly = true;
    option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
//.AddJwtBearer(option =>
//{
//    option.SaveToken = true;
//    option.RequireHttpsMetadata = false;
//    option.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ClockSkew = TimeSpan.Zero,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTkey").Value))
//    };
//});

builder.Services.AddAuthorization(options =>
{
      options.AddPolicy("CommonPolicy", policy => policy.RequireRole(SD.SuperAdmin, SD.AdminInspector, SD.Admin, SD.Inspector, SD.Clerk, SD.ViewOnly));
      options.AddPolicy("OfficePolicy", policy => policy.RequireRole(SD.SuperAdmin, SD.Admin, SD.Clerk, SD.ViewOnly));
      options.AddPolicy("UserManagePolicy", policy => policy.RequireRole(SD.SuperAdmin, SD.AdminInspector, SD.Admin));
      options.AddPolicy("PermitManagePolicy", policy => policy.RequireRole(SD.SuperAdmin, SD.AdminInspector, SD.Admin, SD.Clerk));
      options.AddPolicy("InspectorPolicy", policy => policy.RequireRole(SD.AdminInspector, SD.Inspector, SD.SuperAdmin));

      options.AddPolicy("PerformInspectionPolicy", policy => policy.RequireRole(SD.SuperAdmin, SD.AdminInspector, SD.Inspector));
      //options.AddPolicy("CommonPolicy", policy => policy.RequireRole(SD.SuperAdmin, SD.AdminInspector, SD.Admin, SD.Inspector, SD.Clerk, SD.ViewOnly));
});

builder.Services.AddWkhtmltopdf();

var APIUrl = builder.Configuration.GetSection("APIUrl").Value;

builder.Services.AddCors(options =>
{
      options.AddPolicy("AllowSpecificOrigin",
          builder => builder
              .WithOrigins(APIUrl)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowSpecificOrigin");

app.UseRouting();

SeedDatabase();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
      endpoints.MapControllerRoute(
          name: "Areas",
          pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

      endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}"); 
          //pattern: "{controller=Home}/{action=Verification}/{id?}"); 
}
);

app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbinit = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbinit.Initialize();
    }
}
