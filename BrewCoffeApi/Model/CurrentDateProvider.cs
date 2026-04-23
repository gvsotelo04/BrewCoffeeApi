using BrewCoffeApi.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrewCoffeeApi.Tests.Model
{
    public class CurrentDateProvider : ICurrentDateProvider
    {
        public DateTimeOffset Now { get; set; }
    }
}
