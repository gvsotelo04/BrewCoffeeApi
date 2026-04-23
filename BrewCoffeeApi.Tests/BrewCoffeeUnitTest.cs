using BrewCoffeApi.Controllers;
using BrewCoffeApi.Model;
using BrewCoffeApi.Services;
using BrewCoffeeApi.Model;
using BrewCoffeeApi.Services;
using BrewCoffeeApi.Test.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Moq;

namespace BrewCoffeeApi.Tests
{
    public class BrewCoffeeUnitTest
    {
        [Fact]
        public async Task Test418()
        {
            var mockTime = new Mock<ICurrentDateProvider>();
            
            mockTime.Setup(x => x.Now).Returns(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero));
            var handler = new FakeHttpMessageHandler();
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.openweathermap.org")
            };

            var settings = new OpenWeatherSettings
            {
                BaseUrl = "https://api.openweathermap.org/data/2.5/weather",
                ApiKey = "c9017f5ce4ff4b462019c09db8b89960"
            };

            var options = Options.Create(settings);
            var weatherService = new WeatherService(httpClient, options);
            var _ctrl = new BrewCoffeeController(mockTime.Object, weatherService);

            //var _ctrl = new BrewCoffeeController(mockTime.Object);
            var _result = await _ctrl.Get();
            var statusResult = Assert.IsType<ObjectResult>(_result.Result);
            Assert.Equal(418,statusResult.StatusCode);
            //Assert.IsType<ActionResult<BrewCoffeeModel>>(_result);
        }

        [Fact]
        public async Task TestStatusCode503Every5thRequest()
        {
            var mockTime = new Mock<ICurrentDateProvider>();

            //mockTime.Setup(x => x.Now).Returns(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero));
            var handler = new FakeHttpMessageHandler();
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.openweathermap.org")
            };

            var settings = new OpenWeatherSettings
            {
                BaseUrl = "https://api.openweathermap.org/data/2.5/weather",
                ApiKey = "c9017f5ce4ff4b462019c09db8b89960"
            };

            var options = Options.Create(settings);
            var weatherService = new WeatherService(httpClient, options);
           
            var controller = new BrewCoffeeController(mockTime.Object, weatherService);

            ActionResult<BrewCoffeeModel>? result = null;

            for (int i = 1; i <= 5; i++)
            {
                result = await controller.Get();
            }
            if (result == null)
            {
                Assert.Fail("Null value");
            } else
            {
                var obj = Assert.IsType<ObjectResult>(result.Result);
                Assert.Equal(503, obj.StatusCode);
            }
            
        }

        [Fact]
        public async Task TestStatusCode200()
        {
            var mockTime = new Mock<ICurrentDateProvider>();

            var handler = new FakeHttpMessageHandler();
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.openweathermap.org")
            };

            var settings = new OpenWeatherSettings
            {
                BaseUrl = "https://api.openweathermap.org/data/2.5/weather",
                ApiKey = "c9017f5ce4ff4b462019c09db8b89960"
            };

            var options = Options.Create(settings);
            var weatherService = new WeatherService(httpClient, options);

            var controller = new BrewCoffeeController(mockTime.Object, weatherService);
           

                var result = await controller.Get();
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                //var model = Assert.IsType<BrewCoffeeModel>(okResult.Value);

                Assert.Equal(200, okResult.StatusCode);
            

        }

        [Fact]
        public async Task TestStatusTemperatureGreaterThan30()
        {
            var mockTime = new Mock<ICurrentDateProvider>();

            //mockTime.Setup(x => x.Now).Returns(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero));

            var handler = new FakeHttpMessageHandler();
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.openweathermap.org")
            };

            var settings = new OpenWeatherSettings
            {
                BaseUrl = "https://api.openweathermap.org/data/2.5/weather",
                ApiKey = "c9017f5ce4ff4b462019c09db8b89960"
            };

            var options = Options.Create(settings);
            var weatherService = new WeatherService(httpClient, options);
            var controller = new BrewCoffeeController(mockTime.Object, weatherService);


            var result = await controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<BrewCoffeeModel>(okResult.Value);

            Assert.Equal("Your refreshing iced coffee is ready", model.Message);


        }
    }
}
