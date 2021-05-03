using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    public class IntegrationTest2
    {
        private Display display;
        private IOutput output;
        [SetUp]
        public void Setup()
        {
            output = new Output();
            display = new Display(output);

        }

        [Test]
        public void OutputStringOnDisplayShowTime_Equals_GivenTime()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            display.ShowTime(10, 10);

            //Assert
            string expectedOutput = "Display shows: 10:10\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        [Test]
        public void OutputStringOnDisplayShowPower_Equals_GivenPower()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            display.ShowPower(50);

            //Assert
            string expectedOutput = "Display shows: 50 W\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }

        [Test]
        public void OutputStringOnDisplayClear_Equals_Cleared()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            display.Clear();

            //Assert
            string expectedOutput = "Display cleared\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
    }
}