
using AfigoBackend.Infraestructure;
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

// Cookie Auth
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "afigo_auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        // ⬇️ PRODUCCIÓN si FRONTEND y API están en dominios distintos (subdominios o dominios distintos):
        // options.Cookie.SameSite = SameSiteMode.None; // requiere HTTPS

        // ⬇️ DESARROLLO o MISMO ORIGEN:
        options.Cookie.SameSite = SameSiteMode.Lax;

        // Inactividad: 20 minutos + sliding
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;

        // API: evita redirecciones y devuelve 401/403
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

// CORS (agrega tus dominios reales de prod)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:5173",
            "https://localhost:3000",
            "https://localhost:5173",
            "https://app.tudominio.com",
            "https://tudominio.com"
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // /swagger
}

// CORS primero (para preflights y headers)
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

// ====== Middleware CSRF ======
// Define aquí los orígenes permitidos (mismos que en CORS)
var allowedOrigins = new[]
{
    "http://localhost:3000",
    "http://localhost:5173",
    "https://localhost:3000",
    "https://localhost:5173",
    "https://app.tudominio.com",
    "https://tudominio.com"
};

app.Use(async (ctx, next) =>
{
    var path = ctx.Request.Path.Value ?? string.Empty;

    // Omitir swagger y endpoints de diagnóstico/health si usas alguno
    if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    // Permitir sin validación CSRF métodos "seguros" y preflight
    if (HttpMethods.IsGet(ctx.Request.Method) ||
        HttpMethods.IsHead(ctx.Request.Method) ||
        HttpMethods.IsOptions(ctx.Request.Method))
    {
        await next();
        return;
    }

    // Validar solo métodos que modifican estado
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

        // Exigir header AJAX para evitar submits ciegos
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
