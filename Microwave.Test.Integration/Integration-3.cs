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

        [TestCase(1)]
        [TestCase(50)]
        [TestCase(100)]
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
        [TestCase(0)]
        [TestCase(101)]
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