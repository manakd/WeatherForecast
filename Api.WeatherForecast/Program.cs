using Api.WeatherForecast.Logic;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var conString = builder.Configuration.GetConnectionString("WeatherForecastDatabase") ??
     throw new InvalidOperationException("Connection string 'WeatherForecastContext'" +
    " not found.");
builder.Services.AddDbContext<WeatherForecastDbContext>(options =>
    options.UseSqlServer(conString));
//IServiceCollection serviceCollection = builder.Services.AddDbContext<WeatherForecastDbContext>(options =>
//    options.UseSqlServer(conString));

builder.Services.AddScoped<IWeatherForecastLogic, WeatherForecastLogic>();

var app = builder.Build();


app.UseCors("AllowAll");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
