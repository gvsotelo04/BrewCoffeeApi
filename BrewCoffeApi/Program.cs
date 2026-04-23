using BrewCoffeApi.Services;
using BrewCoffeeApi.Model;
using BrewCoffeeApi.Services;
using BrewCoffeeApi.Tests.Model;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddHttpClient<WeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org");
});

builder.Services.Configure<OpenWeatherSettings>(
    builder.Configuration.GetSection("OpenWeather"));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
