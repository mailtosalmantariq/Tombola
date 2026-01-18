using Microsoft.EntityFrameworkCore;
using MT.Beans.API.HostedService;
using MT.Beans.API.Service.BeanDayService;
using MT.Beans.API.Service.BeansServices;
using MT.Middleware;
using MT.Tombola.Api.Data.Data;
using MT.Tombola.Api.Data.Models;
using MT.Tombola.Api.Data.Repos.BeanDay;
using MT.Tombola.Api.Data.Repos.Beans;
using MT.Tombola.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers + OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Database
builder.Services.AddDbContext<BeansDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BeansDbConnection"),
        b => b.MigrationsAssembly("MT.Beans.API")
    ));

// Repositories
builder.Services.AddScoped<IBeanRepository<Bean>, BeanRepository<Bean>>();
builder.Services.AddScoped<IBeanOfTheDayRepository, BeanOfTheDayRepository>();

// Services
builder.Services.AddScoped<IBeanService, BeanService>();
builder.Services.AddScoped<IBeanOfTheDayService, BeanOfTheDayService>();

// Hosted service for safe seeding
builder.Services.AddHostedService<DatabaseSeederHostedService>();

builder.Services.AddSwaggerGen();

// CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7264") // Blazor WASM URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tombola API V1");
    });

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.UseMiddleware<Logging>();

app.MapControllers();

app.Run();
