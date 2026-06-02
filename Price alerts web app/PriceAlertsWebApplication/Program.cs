using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PriceAlertsWebApplication;
using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Services;
using PriceAlertsWebApplication.workers;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.Configure<TwelveAPISettings>(
    builder.Configuration.GetSection("TwelveAPISettings"));
builder.Services.Configure<TelegramAPISettings>(
    builder.Configuration.GetSection("TelegramAPISettings"));


builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSingleton<ItwelveDataService, TwelveDataService>();
builder.Services.AddSingleton<ITelegramNotificationService, TelegramNotificationService>();
builder.Services.AddSingleton<IAlertService, AlertService>();

builder.Services.AddHostedService<GoldPriceWorker>();
builder.Services.AddHostedService<TriggeredAlertCleanupWorker>();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Price Alerts API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT like: Bearer <token>"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
        options.Events =
            new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token =
                        context.Request.Cookies["access_token"];

                    return Task.CompletedTask;
                }
            };
    });
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "Price Alerts API V1");

    options.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

// App started notification
app.Lifetime.ApplicationStarted.Register(() =>
{
    using var scope = app.Services.CreateScope();

    var telegramService =
        scope.ServiceProvider.GetRequiredService<ITelegramNotificationService>();

    _ = Task.Run(async () =>
    {
        await telegramService.SendTelegramNotification(
            $"""
             🟢 Gold Alert Bot Started

             Started At: {DateTime.Now:dd-MMM-yyyy HH:mm:ss}
             Environment: {app.Environment.EnvironmentName}
             """
        );
    });
});

app.Run();