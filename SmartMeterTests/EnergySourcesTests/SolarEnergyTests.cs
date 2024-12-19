using System;
using Xunit;
using trisatenergy_SMCollector.SmartMeterSimulation.EnergySources;

namespace SmartMeterTests.EnergySourceTests
{
    public class SolarEnergyTests
    {
        private readonly SolarEnergy _solarEnergy;
        private readonly Random _random;

        public SolarEnergyTests()
        {
            _solarEnergy = new SolarEnergy();
            _random = new Random();
        }

        [Fact]
        public void SimulateProduction_DaytimeHours_ReturnsPositiveValue()
        {
            // Act
            double production = _solarEnergy.SimulateProduction(12, _random);

            // Assert
            Assert.True(production > 0, "Solar production during daytime should be positive.");
        }

        [Fact]
        public void SimulateProduction_NightHours_ReturnsZeroOrNearZero()
        {
            // Act
            double production = _solarEnergy.SimulateProduction(2, _random);

            // Assert
            Assert.True(production >= 0 && production <= 1, "Solar production at night should be zero or near zero.");
        }
    }
}
