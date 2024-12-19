using System;
using Xunit;
using SmartMeterCollector.EnergySources;

namespace SmartMeterTests.EnergySourceTests
{
    public class OtherEnergyTests
    {
        private readonly OtherEnergy _otherEnergy;
        private readonly Random _random;

        public OtherEnergyTests()
        {
            _otherEnergy = new OtherEnergy();
            _random = new Random();
        }

        [Fact]
        public void SimulateProduction_AnyHour_ReturnsConstantBaseProduction()
        {
            // Act
            double production = _otherEnergy.SimulateProduction(15, _random);

            // Assert
            Assert.True(production >= 1.0 && production <= 1.2, "Other sources should have consistent production.");
        }
    }
}