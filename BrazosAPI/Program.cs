using BrazosAPI.DI_Config;
using BrazosApp.DataAccess.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using BrazosApp.Utility;
using BrazosApp.DataAccess.Initializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
      options.UseSqlServer(builder.Configuration.GetConnectionString("dbConString"));
});
//builder.Services.AddDbContext<PoolDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConString"));
//});

// Add services to the container.

builder.Services.AddAllServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTkey").Value))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("InspectorPolicy", policy => policy.RequireRole(SD.AdminInspector, SD.Inspector));
});

var BaseUrl = builder.Configuration.GetSection("BaseUrl").Value;

builder.Services.AddCors(options =>
{
      options.AddPolicy("AllowSpecificOrigin",
          builder => builder
              .WithOrigins(BaseUrl)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        //Description =
        //    "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
        //    "Enter your token here.\r\n\r\n",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
        new OpenApiSecurityScheme
        {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
        },
        new List<string>()
        }
    });
    options.SwaggerDoc("AdminAPI", new OpenApiInfo
    {
        Version = "v1",
        Title = "Admin",
        Description = "Description",
        Contact = new OpenApiContact
        {
            Name = "Brazos App Admin",
            Email = "stageocc@inspect2go44.com",
            Url = new Uri("https://stagebrazos.inspect2go44.com/")
        }
    });
});


var app = builder.Build();

SeedDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
      //app.UseSwagger();
      //app.UseSwaggerUI();
      //app.UseSwaggerUI(c =>
      //{
      //      c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminAPI");
      //});
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/AdminAPI/swagger.json", "Admin API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
//app.UsePathBase("https://localhost:44357");

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbinit = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbinit.Initialize();
    }
}
