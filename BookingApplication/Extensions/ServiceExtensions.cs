using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Entities;
using Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;
using Entities.Seeds;
using Microsoft.AspNetCore.Identity;
using Entities.Models;

namespace BookingApplication.Extensions
{
        public static class ServiceExtensions
        {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })//.AddCookie("Identity.Application")
                    .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(configuration.GetSection("JWTSettings:securityKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    }
                };
            });
        }
            public static void ConfigureCors(this IServiceCollection services)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                });
            }

            public static void ConfigureIISIntegration(this IServiceCollection services)
            {
                services.Configure<IISOptions>(options =>
                {

                });
            }

            public static void ConfigureLoggerService(this IServiceCollection services)
            {
                services.AddSingleton<ILoggerManager, LoggerManager>();
            }

            public static void ConfigureRepositoryWrapper(this IServiceCollection services)
            {
                services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            }
            
            public static void ConfigureHttpContextAccessor(this IServiceCollection services)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }
            public static void ConfigureAutoMapper(this IServiceCollection services)
            {
                services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            }
        //find another way to implement!!!
        public static async void ConfigureSeedData(this WebApplication host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var seedService = services.GetRequiredService<Seed>();

                //if (seedService.Database.IsSqlServer())
                //{
                //    seedService.Database.Migrate();
                //}

                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                await Seed.SeedData(userManager, roleManager);
            }
            catch (Exception ex)
            {
                //Log some error
                //_logger.LogError($"Seeding Data Failed: {ex.Message}");
                //return StatusCode(500, $"Internal Server Error:{ex.Message}"); ;
            }
        }
    }
}
