using System;
using System.IO;
using System.Text.RegularExpressions;
using BlazorApp.Server.Controllers;
using BlazorApp.Server.Interfaces;
using BlazorApp.Server.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlazorApp.Tests.IntegrationTests {
    [TestClass]
    public class GameControllerTests {
        private readonly GameController _gameController;

        public GameControllerTests() {
            var numberServiceMock = new Mock<INumberService>();
            var loggerMock = new Mock<IResetableLogger>();

            numberServiceMock.Setup(service =>
                    service.Validate(It.Is<string>(s => int.Parse(s) == CorrectNumber)))
                .Returns(() => CorrectNumber);

            numberServiceMock.Setup(service =>
                    service.Validate(It.Is<string>(s => int.Parse(s) < CorrectNumber)))
                .Returns(() => int.Parse(GetLowNumber()));

            numberServiceMock.Setup(service =>
                    service.Validate(It.Is<string>(s => int.Parse(s) > CorrectNumber)))
                .Returns(() => int.Parse(GetHighNumber()));

            numberServiceMock.Setup(service =>
                    service.Validate(It.Is<string>(s => !new Regex(Resources.ValidNumberRegex).IsMatch(s))))
                .Returns(() => null);

            _gameController = new GameController(numberServiceMock.Object, loggerMock.Object);
        }


        private static int CorrectNumber => int.Parse(Resources.CorrectNumber);

        private static string GetLowNumber() {
            var random = new Random();
            var lowerBound = int.Parse(Resources.LowerBound);
            var lowNumber = random.Next(lowerBound, CorrectNumber);
            return lowNumber.ToString();
        }

        private static string GetHighNumber() {
            var random = new Random();
            var upperBound = int.Parse(Resources.UpperBound);
            var highNumber = random.Next(CorrectNumber + 1, upperBound);
            return highNumber.ToString();
        }

        private static string GetRandomString() {
            var randomString = Path.GetRandomFileName();
            randomString = randomString.Replace(".", "");
            return randomString;
        }

        [TestMethod]
        public void CheckNumber_CorrectNumber_CorrectMessage() {
            // Arrange
            var expectedStatusCode = new StatusCodeResult(200).StatusCode;
            var expectedMessage = Resources.CorrectGuessMessage;
            // Act
            var actual = _gameController.CheckNumber(CorrectNumber.ToString()).Result as OkObjectResult;
            //TODO: assert for null in here; maybe get the result from another line
            // Assert
            Assert.AreEqual(expectedStatusCode, actual?.StatusCode);
            Assert.AreEqual(expectedMessage, actual?.Value);
        }

        [TestMethod]
        public void CheckNumber_LowNumber_TooLowMessage() {
            // Arrange
            var expectedStatusCode = new StatusCodeResult(303).StatusCode;
            var expectedMessage = Resources.TooLowMessage;
            // Act
            var actual = _gameController.CheckNumber(GetLowNumber()).Result as ObjectResult;
            // Assert
            Assert.AreEqual(expectedStatusCode, actual?.StatusCode);
            Assert.AreEqual(expectedMessage, actual?.Value);
        }


        [TestMethod]
        public void CheckNumber_HighNumber_TooHighMessage() {
            // Arrange
            var expectedStatusCode = new StatusCodeResult(303).StatusCode;
            var expectedMessage = Resources.TooHighMessage;
            // Act
            var actual =
                _gameController.CheckNumber(GetHighNumber()).Result as ObjectResult;
            // Assert
            Assert.AreEqual(expectedStatusCode, actual?.StatusCode);
            Assert.AreEqual(expectedMessage, actual?.Value);
        }

        [TestMethod]
        public void CheckNumber_InvalidNumber_InvalidNumberMessage() {
            // Arrange
            var expectedStatusCode = new StatusCodeResult(400).StatusCode;
            var expectedMessage =
                string.Format(Resources.InvalidNumberMessage, Resources.LowerBound, Resources.UpperBound);
            // Act
            var actual =
                _gameController.CheckNumber(GetRandomString()).Result as BadRequestObjectResult;
            // Assert
            Assert.AreEqual(expectedStatusCode, actual?.StatusCode);
            Assert.AreEqual(expectedMessage, actual?.Value);
        }
    }
}