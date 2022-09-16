
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using NLog;
using Microsoft.AspNetCore.HttpOverrides;
using BookingApplication.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

//// allows both to access and to set up the config
//ConfigurationManager configuration = builder.Configuration; 
//IWebHostEnvironment environment = builder.Environment;

// Add services to the container..
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryWrapper();
builder.Services.ConfigureHttpContextAccessor();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureAuthentication(builder.Configuration);



builder.Services.AddDbContext<AppointmentContext>(options =>
{
    if (!options.IsConfigured) options.UseSqlServer(builder.Configuration.GetConnectionString("Stomatology"), opts => opts.MigrationsAssembly("BookingApplication"));
});
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
            //var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            //.AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppointmentContext>()
            //.AddSignInManager<SignInManager<User>>();
            .AddDefaultTokenProviders();
builder.Services.AddControllers();

// In production, the React files will be served from this directory
//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "ClientApp/build";
//});
//services.AddSingleton<ILoggerManager, LoggerManager>
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//builder.Services.AddSingleton<IJwtHandlerAuth>(new JwtHandlerAuth(Configuration.GetSection("Jwt:PrivateKey").Value));
//builder.Services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
//builder.Services.AddScoped<IUserService, UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");
//app.UseStaticFiles();
//app.UseSpaStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

//app.UseSpa(spa =>
//{
//    spa.Options.SourcePath = "ClientApp";

//    if (env.IsDevelopment())
//    {
//        spa.UseReactDevelopmentServer(npmScript: "start");
//    }
//});
app.MapControllers();
app.Run();
