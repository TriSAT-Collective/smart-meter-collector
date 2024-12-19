using System;
using Xunit;
using SmartMeterCollector;

namespace SmartMeterTests
{
    public class MessageCollectorTests
    {
        private readonly MessageCollector _messageCollector;

        public MessageCollectorTests()
        {
            _messageCollector = new MessageCollector(24);
        }

        [Fact]
        public void SimulateConsumption_ReturnsConsumptionWithinExpectedRange()
        {
            // Arrange
            var privateMethod = typeof(MessageCollector)
                .GetMethod("SimulateConsumption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            double consumption = (double)privateMethod.Invoke(_messageCollector, new object[] { 9 });

            // Assert
            Assert.InRange(consumption, 2.0, 2.5);
        }

        [Fact]
        public void SimulateTotalProduction_ReturnsPositiveValue()
        {
            // Arrange
            var privateMethod = typeof(MessageCollector)
                .GetMethod("SimulateTotalProduction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            double production = (double)privateMethod.Invoke(_messageCollector, new object[] { 10 });

            // Assert
            Assert.True(production > 0, "Total energy production should be positive.");
        }

        [Fact]
        public void Simulate_24Hours_SimulatesWithoutError()
        {
            // Act & Assert
            _messageCollector.SimulateAsync(24);
        }
    }
}