using BrewCoffeApi.Model;
using BrewCoffeApi.Services;
using BrewCoffeeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;

namespace BrewCoffeApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BrewCoffeeController : ControllerBase
    {
        private static int brewingCounter = 0;
        private readonly ICurrentDateProvider _currentDate;
        private readonly WeatherService _weatherService;

        public BrewCoffeeController(ICurrentDateProvider currentDate, WeatherService weatherService)
        {
            _currentDate = currentDate;
            _weatherService = weatherService;


        }

        [HttpGet("/brew-coffee")]
        public async Task<ActionResult<BrewCoffeeModel>> Get()
        {
            

            var dateNow = _currentDate.Now;
            //check for April 1
            if (dateNow.Month == 4 && dateNow.Day == 1) {
                brewingCounter = 0; //reset counter to 0 so that after April 1, the count will start again to zero
                return StatusCode(StatusCodes.Status418ImATeapot,null); 
            } else
            {
                int currentCount = Interlocked.Increment(ref brewingCounter); //check for how many times the endpoint has been called.
                
                if (currentCount % 5 == 0) {                    
                    return StatusCode(StatusCodes.Status503ServiceUnavailable,null);
                } else {
                   

                    var temp = await _weatherService.GetTemperatureAsync("Manila");
                          
                    
                    if (temp > 30) {
                        return Ok(new BrewCoffeeModel { Message = "Your refreshing iced coffee is ready" });
                    } else {
                        return Ok(new BrewCoffeeModel { Message = "Your piping hot coffee is ready" });
                    }
                    
                }
            }
            
        }

        
    }
}

