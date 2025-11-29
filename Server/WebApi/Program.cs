using EfcRepositories;
using FileRepositories;
using RepositoryContracts;
using AppContext = EfcRepositories.AppContext;


var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // fine to keep
builder.Services.AddOpenApi();              // built-in OpenAPI (no Swashbuckle)

builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();
builder.Services.AddDbContext<AppContext>();

var app = builder.Build();

// Pipeline
//app.UseHttpsRedirection();
app.MapControllers();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    // Serves the OpenAPI JSON at /openapi/v1.json
    app.MapOpenApi();
}

app.Run();