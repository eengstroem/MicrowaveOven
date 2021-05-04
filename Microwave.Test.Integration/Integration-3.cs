using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    public class IntegrationTest3
    {
        private IPowerTube powertube;
        private IOutput output;
        [SetUp]
        public void Setup()
        {
            output = new Output();
            powertube = new PowerTube(output);

        }

        [Test]
        public void TurnOff_ActivePowerTube_TurnsOff()
        {
            // Arrange
            powertube.TurnOn(50);
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            powertube.TurnOff();

            //Assert
            string expectedOutput = "PowerTube turned off\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        [Test]
        public void TurnOff_InActivePowerTube_DoesNothing()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            powertube.TurnOff();

            //Assert
            string expectedOutput = "";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }

        // We need to change the test from testing range 1-100, to 50-700 instead with the new powertube changes.

        [TestCase(50)]
        [TestCase(350)]
        [TestCase(700)]
        public void TurnOn1_100_InactivePowerTube_TurnsOn(int power)
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            powertube.TurnOn(power);

            //Assert
            string expectedOutput = $"PowerTube works with {power}\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }


        // We need to change the test from testing range between -5 and 150, to -5 and 1000 instead with the new powertube changes.

        [TestCase(-5)]
        [TestCase(0)]
        [TestCase(49)]
        [TestCase(750)]
        [TestCase(1000)]
        public void TurnOn0_101_InactivePowerTube_ThrowsError(int power)
        {
            // Arrange

            //Act

            //Assert
            Assert.That(() => powertube.TurnOn(power),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void TurnOn_activePowerTube_ThrowsError()
        {
            // Arrange
            powertube.TurnOn(100);
            //Act

            //Assert
            Assert.That(() => powertube.TurnOn(100),
                Throws.TypeOf<ApplicationException>());
        }
    }
}