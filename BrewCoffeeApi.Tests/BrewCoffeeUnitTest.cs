using BrewCoffeApi.Controllers;
using BrewCoffeApi.Model;
using BrewCoffeApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Moq;

namespace BrewCoffeeApi.Tests
{
    public class BrewCoffeeUnitTest
    {
        [Fact]
        public void Test418()
        {
            var mockTime = new Mock<ICurrentDateProvider>();
            
            mockTime.Setup(x => x.Now).Returns(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero));

            var _ctrl = new BrewCoffeeController(mockTime.Object);
            var _result = _ctrl.Get();
            var statusResult = Assert.IsType<ObjectResult>(_result.Result);
            Assert.Equal(418,statusResult.StatusCode);
            //Assert.IsType<ActionResult<BrewCoffeeModel>>(_result);
        }

        [Fact]
        public void TestStatusCode503Every5thRequest()
        {
            var mockTime = new Mock<ICurrentDateProvider>();

            //mockTime.Setup(x => x.Now).Returns(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero));

            var controller = new BrewCoffeeController(mockTime.Object);

            ActionResult<BrewCoffeeModel>? result = null;

            for (int i = 1; i <= 5; i++)
            {
                result = controller.Get();
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
        public void TestStatusCode200()
        {
            var mockTime = new Mock<ICurrentDateProvider>();

            //mockTime.Setup(x => x.Now).Returns(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero));

            var controller = new BrewCoffeeController(mockTime.Object);
           

                var result = controller.Get();
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var model = Assert.IsType<BrewCoffeeModel>(okResult.Value);

                Assert.Equal("Your piping hot coffee is ready", model.Message);
            

        }
    }
}
