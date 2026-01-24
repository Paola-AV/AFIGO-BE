
using AfigoBackend.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Infraestructura (DbContext + servicios)
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger UI (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();     // UI en /swagger   
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
