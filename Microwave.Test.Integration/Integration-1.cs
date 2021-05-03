using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    public class IntegrationTest1
    {
        private Light light;
        private IOutput output;
        [SetUp]
        public void Setup()
        {
            output = new Output();
            light = new Light(output);

        }

        [Test]
        public void OutputStringOnLightOn_Equals_LightIsOn()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            light.TurnOn();

            //Assert
            string expectedOutput = "Light is turned on\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        [Test]
        public void OutputStringOnLightOff_Equals_LightIsOff()
        {
            // Arrange
            light.TurnOn();
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            light.TurnOff();

            //Assert
            string expectedOutput = "Light is turned off\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
    }
}