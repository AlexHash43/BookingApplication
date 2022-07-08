
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using NLog;
using Microsoft.AspNetCore.HttpOverrides;
using BookingApplication.Extensions;

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

builder.Services.AddControllers();
builder.Services.AddDbContext<AppointmentContext>(options =>
{
    if (!options.IsConfigured) options.UseSqlServer(builder.Configuration.GetConnectionString("Stomatology"));
});
builder.Services.AddIdentityCore<User>()
            //var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppointmentContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();

//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddCookie("Identity.Application")
//.AddJwtBearer(x =>
//{
//    x.RequireHttpsMetadata = false;
//    x.SaveToken = true;
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(Configuration.GetSection("Jwt:PrivateKey").Value)),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };

//    x.Events = new JwtBearerEvents
//    {
//        OnMessageReceived = context =>
//        {
//            context.Token = context.Request.Cookies["jwt"];
//            return Task.CompletedTask;
//        }
//    };
//});
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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
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
