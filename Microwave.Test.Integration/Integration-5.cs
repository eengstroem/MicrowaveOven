using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Controllers;
using NUnit.Framework;
using System;
using System.IO;
using NSubstitute;

namespace Microwave.Test.Integration
{
    public class IntegrationTest5
    {
        private IUserInterface UI;
        private ICookController cookController;
        private IDisplay display;
        private ILight light;
        private IOutput output;
        private IDoor door;
        private IButton powerbutton;
        private IButton timebutton;
        private IButton startcancelbutton;
        private ITimer timer;
        private IPowerTube powertube;
        [SetUp]
        public void Setup()
        {
            output = new Output();
            display = new Display(output);
            light = new Light(output);

            // Timer and PowerTube are only substituted to construct the CookController.
            // Each module have been tested in another integration step.
            timer = Substitute.For<ITimer>();
            powertube = Substitute.For<IPowerTube>();

            // These four substitutions will be used to raise events. The real modules will be tested in the last two integration steps. 
            door = Substitute.For<IDoor>();
            powerbutton = Substitute.For<IButton>();
            timebutton = Substitute.For<IButton>();
            startcancelbutton = Substitute.For<IButton>();

            cookController = new CookController(timer, display, powertube);

            UI = new UserInterface(powerbutton, timebutton, startcancelbutton, door, display, light, cookController);

        }

        [Test]
        public void OnPowerPressed_StateREADY_ShowPower()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            
            powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            

            //Assert
            string expectedOutput = "Display shows: 50 W\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        [Test]
        public void OnPowerPressed_StateSetPower_RaisePower()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Console.SetOut(output);

            //Act
            powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            

            //Assert
            string expectedOutput = "Display shows: 100 W\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        [Test]
        public void OnTimePressed_StateSetPower_ShowTime()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Console.SetOut(output);

            //Act
            timebutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            

            //Assert
            string expectedOutput = "Display shows: 01:00\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }

        [Test]
        public void OnTimePressed_StateSetTime_RaiseTime()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timebutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Console.SetOut(output);

            //Act
            timebutton.Pressed += Raise.EventWith(this, EventArgs.Empty);


            //Assert
            string expectedOutput = "Display shows: 02:00\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
    }
}