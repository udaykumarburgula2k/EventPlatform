using EventPlatform.Api.Common;
using EventPlatform.Api.Data;
using EventPlatform.Api.Infrastructure;
using EventPlatform.Api.Modules.Auth;
using EventPlatform.Api.Modules.Events;
using EventPlatform.Api.Modules.Events.Authorization;
using EventPlatform.Api.Modules.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<AuthModuleOptions>(
    builder.Configuration.GetSection(AuthModuleOptions.SectionName));

builder.Services.Configure<EventModuleOptions>(
    builder.Configuration.GetSection(EventModuleOptions.SectionName));

builder.Services.Configure<RegistrationModuleOptions>(
    builder.Configuration.GetSection(RegistrationModuleOptions.SectionName));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.AdminOnly, policy =>
        policy.RequireRole(Roles.Administrator));

    options.AddPolicy(Policies.ManageEvents, policy =>
        policy.RequireRole(Roles.Administrator, Roles.Organizer));

    options.AddPolicy(Policies.RegisterForEvent, policy =>
        policy.RequireRole(Roles.Administrator, Roles.Attendee));
});

builder.Services.AddScoped<IAuthorizationHandler, CanEditEventHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEditEvent", policy =>
        policy.Requirements.Add(new CanEditEventRequirement()));
});

builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddDefaultTokenProviders();

var jwtOptions = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key)),
            NameClaimType = "sub",
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestPath
        | HttpLoggingFields.RequestMethod
        | HttpLoggingFields.ResponseStatusCode
        | HttpLoggingFields.Duration;
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPaymentService, FakePaymentService>();

var app = builder.Build();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

app.Run();