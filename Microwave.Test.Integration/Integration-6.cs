using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Controllers;
using NUnit.Framework;
using System.IO;
using NSubstitute;

namespace Microwave.Test.Integration
{
    class IntegrationTest6
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
            door = new Door();
            powerbutton = Substitute.For<IButton>();
            timebutton = Substitute.For<IButton>();
            startcancelbutton = Substitute.For<IButton>();

            cookController = new CookController(timer, display, powertube);

            UI = new UserInterface(powerbutton, timebutton, startcancelbutton, door, display, light, cookController);

        }

        [Test]
        public void DoorOpenedEvent_StateReady_LightOn()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            door.Open();

            //Assert
            string expectedOutput = "Light is turned on\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }



        [Test]
        public void DoorClosedEvent_StateDoorOpen_LightOff()
        {
            // Arrange
            StringWriter output = new();
            door.Open();
            Console.SetOut(output);

            //Act
            door.Close();

            //Assert
            string expectedOutput = "Light is turned off\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }

        public void DoorOpenedEvent_StateCooking_CookerStopDisplayClear()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timebutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            startcancelbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Console.SetOut(output);

            //Act
            door.Open();

            //Assert
            string expectedOutput = "Display cleared\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
            powertube.Received().TurnOff();
            timer.Received().Stop();
        }

        [Test]
        public void DoorOpenedEvent_StateSetTime_LightOnDisplayClear()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timebutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Console.SetOut(output);

            //Act
            door.Open();

            //Assert
            string expectedOutput = "Light is turned on\r\nDisplay cleared\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }


        [Test]
        public void DoorOpenedEvent_StateSetPower_LightOnDisplayClear()
        {
            // Arrange
            StringWriter output = new();
            powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Console.SetOut(output);

            //Act
            door.Open();

            //Assert
            string expectedOutput = "Light is turned on\r\nDisplay cleared\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }

    }
}
