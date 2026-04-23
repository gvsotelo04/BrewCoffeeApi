using BrewCoffeApi.Model;
using BrewCoffeApi.Services;
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
        public BrewCoffeeController(ICurrentDateProvider currentDate)
        {
            _currentDate = currentDate;
        }

        [HttpGet("/brew-coffee")]
        public ActionResult<BrewCoffeeModel> Get()
        {

            var dateNow = _currentDate.Now;
            if (dateNow.Month == 4 && dateNow.Day == 1) {
                brewingCounter = 0; //reset counter 
                return StatusCode(StatusCodes.Status418ImATeapot,null); 
            } else
            {
                int currentCount = Interlocked.Increment(ref brewingCounter); 
                
                if (currentCount % 5 == 0)
                {                    
                    return StatusCode(StatusCodes.Status503ServiceUnavailable,null);
                }
                else
                {
                    return Ok(new BrewCoffeeModel
                    {
                        Message = "Your piping hot coffee is ready"                        
                    });
                }
            }
            
        }

        
    }
}

