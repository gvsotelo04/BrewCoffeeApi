namespace BrewCoffeApi.Services
{
    public interface ICurrentDateProvider
    {
        DateTimeOffset Now { get; }
    }
}
