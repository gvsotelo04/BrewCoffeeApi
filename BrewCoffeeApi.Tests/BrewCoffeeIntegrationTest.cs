using BrewCoffeApi.Services;
using BrewCoffeeApi.Tests.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BrewCoffeeApi.Tests
{
    public class BrewCoffeeIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        public BrewCoffeeIntegrationTest(WebApplicationFactory<Program> factory)
        {
            var testTime = new TestCurrentDateProvider { Now = new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero)
            };

            factory = factory.WithWebHostBuilder(builder =>
             {
                 builder.ConfigureServices(services =>
                 {
                     
                     var _descriptor = services.SingleOrDefault(
                         d => d.ServiceType == typeof(ICurrentDateProvider));

                     if (_descriptor != null) services.Remove(_descriptor);


                     services.AddSingleton<ICurrentDateProvider>(testTime);
                 });
             });

            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task BrewCoffeeTestDifferentStatusCodes()
        {

            var response = await _httpClient.GetAsync("/brew-coffee");

            Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.ServiceUnavailable ||
            response.StatusCode == (HttpStatusCode)418
        );
        }

        [Fact]
        public async Task TestApril1()
        {
            var currentDate = new TestCurrentDateProvider { Now = new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero)  };

            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { builder.ConfigureServices(services =>
                    {                        
                        var _descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(ICurrentDateProvider));

                        if (_descriptor != null) services.Remove(_descriptor);

                        services.AddSingleton<ICurrentDateProvider>(currentDate);
                    });
                });

            var httpclient = factory.CreateClient();

            var response = await httpclient.GetAsync("/brew-coffee");

            Assert.Equal((HttpStatusCode)418, response.StatusCode);
        }

    }
}
