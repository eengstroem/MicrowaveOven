using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using NUnit.Framework;
using System.IO;
using Microwave.Classes.Controllers;
using System.Threading;

namespace Microwave.Test.Integration
{
   public class IntegrationTest4
    {
        private ICookController cookController;
        private IPowerTube powertube;
        private ITimer timer;
        private IDisplay display;
        private IOutput output;

         [SetUp]
        public void Setup()
        {
            output = new Output();
            timer = new Classes.Boundary.Timer();
            powertube = new PowerTube(output);
            display = new Display(output);
            cookController = new CookController(timer, display, powertube);
        }

        [TestCase(1, 1000)]
        [TestCase(50, 1000)]
        [TestCase(100, 1000)]
        public void TestStartCooking_StartsCooking(int power, int time)
        {
            //Arrange 
            StringWriter output = new();
            Console.SetOut(output);

            //Act 
            cookController.StartCooking(power, time);

            //Assert
            string expectedOutput = $"PowerTube works with {power}\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
            Assert.That(timer.TimeRemaining, Is.EqualTo(time));
        }

        [TestCase(0, 1000)]
        [TestCase(101, 1000)]
        public void TestStartCooking_InvalidPower_ThrowsException(int power, int time)
        {
            //Arrange 


            //Act 


            //Assert
            Assert.That(() => cookController.StartCooking(power,time),
               Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(timer.TimeRemaining, Is.EqualTo(0));
        }

        [TestCase(100, 1000)]
        public void TestStartCooking_OnActiveCooking_ThrowsException(int power, int time)
        {
            //Arrange 
            cookController.StartCooking(power, time);

            //Act 

            //Assert
            Assert.That(() => cookController.StartCooking(power, time),
               Throws.TypeOf<ApplicationException>());
        }

        [Test]
        public void TurnOff_InActiveCooking_DoesNothing()
        {
            // Arrange
            StringWriter output = new();
            Console.SetOut(output);

            //Act
            cookController.Stop();
            
            //Assert
            string expectedOutput = "";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        [Test]
        public void TimerExpired_EventRaised_TurnOffCooking()
        {
            // Arrange
            StringWriter output = new();
           

            //Act
            cookController.StartCooking(50, 1000);
            Console.SetOut(output);
            Thread.Sleep(1100);

            //Assert
            string expectedOutput = "Display shows: 00:00\r\nPowerTube turned off\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));
        }
        
        [TestCase(2000,1100, "00:01")]
        [TestCase(4000, 1100, "00:03")]
        public void OnTimerTick_SecondPassed_EventRaised(int time, int sleep, string expected)
        {
            // Arrange
            cookController.Stop();
            StringWriter output = new();

            StringBuilder sb = output.GetStringBuilder();
            sb.Remove(0, sb.Length);

            //Act
            cookController.StartCooking(50, time);
            Console.SetOut(output);
            Thread.Sleep(sleep);

            //Assert
            string expectedOutput = $"Display shows: {expected}\r\n";
            Assert.That(output.ToString(), Is.EqualTo(expectedOutput));

        }

       


    }
}
