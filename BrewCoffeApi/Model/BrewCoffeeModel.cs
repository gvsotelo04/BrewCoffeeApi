namespace BrewCoffeApi.Model
{
    public class BrewCoffeeModel
    {
        public string Message { get; set; } ="";
        public DateTimeOffset Prepared { get; set; } = DateTimeOffset.Now;
    }
}
