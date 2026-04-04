
using AfigoBackend.Infraestructure;
using AfigoBackend.Infraestructure.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); 

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var serverVersion = ServerVersion.AutoDetect(connectionString);

    options.UseMySql(connectionString, serverVersion, mySqlOptions =>
    {
        mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    });

    options.EnableSensitiveDataLogging(false);
    options.EnableDetailedErrors(true);
});

builder.Services.AddDbContext<ExternalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ExternalSqlConnection")));


// DI propia (services, hasher, etc.)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSyncScheduler();


// Cookie Auth
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "afigo_auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        options.Cookie.SameSite = SameSiteMode.None;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:5173",
            "http://localhost:5160",
            "https://localhost:7122",
            "http://18.217.167.146",
            "https://www.vizodatasolution.com",
            "https://vizodatasolution.com"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                       Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

// ====== Middleware CSRF ======
var allowedOrigins = new[]
{
     "http://localhost:3000",
    "http://localhost:5173",
    "http://localhost:5160",    
    "https://localhost:7122",
    "https://www.vizodatasolution.com",
    "https://vizodatasolution.com"
};

app.Use(async (ctx, next) =>
{
    var path = ctx.Request.Path.Value ?? string.Empty;


    if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith("swagger.json", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))

    {
        await next();
        return;
    }

    if (HttpMethods.IsGet(ctx.Request.Method) ||
        HttpMethods.IsHead(ctx.Request.Method) ||
        HttpMethods.IsOptions(ctx.Request.Method))
    {
        await next();
        return;
    }

    if (HttpMethods.IsPost(ctx.Request.Method) ||
        HttpMethods.IsPut(ctx.Request.Method) ||
        HttpMethods.IsPatch(ctx.Request.Method) ||
        HttpMethods.IsDelete(ctx.Request.Method))
    {
        var origin = ctx.Request.Headers.Origin.ToString();
        var referer = ctx.Request.Headers.Referer.ToString();
        var appOrigin = $"{ctx.Request.Scheme}://{ctx.Request.Host}";
        bool originOk = !string.IsNullOrEmpty(origin) && allowedOrigins.Contains(origin);
        bool refererOk = !string.IsNullOrEmpty(referer) && allowedOrigins.Any(a =>
            referer.StartsWith(a, StringComparison.OrdinalIgnoreCase));
        bool sameOrigin = (!string.IsNullOrEmpty(origin) && origin.StartsWith(appOrigin, StringComparison.OrdinalIgnoreCase))
                      || (!string.IsNullOrEmpty(referer) && referer.StartsWith(appOrigin, StringComparison.OrdinalIgnoreCase));


        if (!originOk && !refererOk && !sameOrigin)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            await ctx.Response.WriteAsync("CSRF blocked: invalid origin");
            return;
        }

        if (!sameOrigin&&!ctx.Request.Headers.ContainsKey("X-Requested-With"))
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            await ctx.Response.WriteAsync("CSRF blocked: missing X-Requested-With");
            return;
        }
    }

    await next();
});
// ====== Fin Middleware CSRF ======

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
