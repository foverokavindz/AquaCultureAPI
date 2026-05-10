using AquaCulture.API.Middleware;
using AquaCulture.Application.Interfaces;
using AquaCulture.Application.Interfaces.Repositories;
using AquaCulture.Application.Interfaces.services;
using AquaCulture.Application.Interfaces.Services;
using AquaCulture.Application.Services;
using AquaCulture.Infrastructure.Data;
using AquaCulture.Infrastructure.Repositories;
using AquaCulture.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IFishFarmRepository, FishFarmRepository>();
builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();

// Register services
builder.Services.AddScoped<IFishFarmService, FishFarmService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();

builder.Services.AddScoped<IImageUploader, CloudinaryImageUploader>();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed test data
    await DbSeeder.SeedAsync(app.Services);
}

app.UseCors("AllowAll");                        
app.UseHttpsRedirection();                      
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();

app.Run();