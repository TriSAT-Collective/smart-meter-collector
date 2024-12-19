using System;
using Xunit;
using trisatenergy_SMCollector.SmartMeterSimulation.EnergySources;

namespace SmartMeterTests.EnergySourceTests
{
    public class WindEnergyTests
    {
        private readonly WindEnergy _windEnergy;
        private readonly Random _random;

        public WindEnergyTests()
        {
            _windEnergy = new WindEnergy();
            _random = new Random();
        }

        [Fact]
        public void SimulateProduction_AnyHour_ReturnsPositiveValue()
        {
            // Act
            double production = _windEnergy.SimulateProduction(15, _random);

            // Assert
            Assert.True(production >= 0, "Wind production should always be positive.");
        }

        [Fact]
        public void SimulateProduction_AnyHour_ReturnsWithinExpectedRange()
        {
            // Act
            double production = _windEnergy.SimulateProduction(10, _random);

            // Assert
            Assert.InRange(production, 0, 3.0);
        }
    }
}
