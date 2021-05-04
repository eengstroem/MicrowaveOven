using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Controllers;
using NUnit.Framework;
using System;
using System.IO;
using NSubstitute;
using System.Threading;

namespace Microwave.Test.Integration
{
    public class IntegrationTest7
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
            powerbutton = new Button();
            timebutton = new Button();
            startcancelbutton = new Button();

            // Timer, PowerTube and door are only substituted to construct other classes.
            // Each module have been tested in another integration step.
            timer = Substitute.For<ITimer>();
            powertube = Substitute.For<IPowerTube>();
            door = Substitute.For<IDoor>();

            cookController = new CookController(timer, display, powertube);

            UI = new UserInterface(powerbutton, timebutton, startcancelbutton, door, display, light, cookController);

        }

        [Test]
        public void PowerPressedEvent_StateREADY_ShowPower()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act

            powerbutton.Press();


            //Assert
            string expectedOutput = "Display shows: 50 W\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        [Test]
        public void PowerPressedEvent_StateSetPower_RaisePower()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Press();
            Console.SetOut(output);

            //Act
            powerbutton.Press();


            //Assert
            string expectedOutput = "Display shows: 100 W\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        [Test]
        public void TimePressedEvent_StateSetPower_ShowTime()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Press();
            Console.SetOut(output);

            //Act
            timebutton.Press();


            //Assert
            string expectedOutput = "Display shows: 01:00\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }

        [Test]
        public void TimePressedEvent_StateSetTime_RaiseTime()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Press();
            timebutton.Press();
            Console.SetOut(output);

            //Act
            timebutton.Press();


            //Assert
            string expectedOutput = "Display shows: 02:00\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }

        [Test]
        public void StartCancelPressedEvent_StateSetPower_Clear()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Press();
            Console.SetOut(output);

            //Act
            startcancelbutton.Press();

            //Assert
            string expectedOutput = "Display cleared\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }

        [Test]
        public void StartCancelPressedEvent_StateSetTime_Clear()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Press();            
            timebutton.Press();
            Console.SetOut(output);

            //Act
            startcancelbutton.Press();

            //Assert
            string expectedOutput = "Light is turned on\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
            powertube.Received().TurnOn(50);
            timer.Received().Start(60000);
        }

        [Test]
        public void StartCancelPressedEvent_StateCooking_Stops()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Press();
            timebutton.Press();
            startcancelbutton.Press();
            Console.SetOut(output);

            //Act
            startcancelbutton.Press();

            //Assert
            string expectedOutput = "Light is turned off\r\nDisplay cleared\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
            powertube.Received().TurnOff();
            timer.Received().Stop();
        }
    }
}