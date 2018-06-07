using BlazorApp.Server.Interfaces;
using BlazorApp.Server.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlazorApp.Tests.UnitTests {
    [TestClass]
    public class NumberServiceTests {
        private int? _expectedNumber;
        private string _numberAsString;
        private INumberService _service;

        [TestMethod]
        public void Validate_ValidNumber_Number() {
            // Arrange
            _service = new NumberService();
            _numberAsString = "55";
            _expectedNumber = 55;
            // Act
            var actualNumber = _service.Validate(_numberAsString);
            // Assert
            Assert.AreEqual(_expectedNumber, actualNumber);
        }

        [TestMethod]
        public void Validate_InvalidNumber_Null() {
            // Arrange
            _service = new NumberService();
            _numberAsString = "!@#$%";
            _expectedNumber = null;
            // Act
            var actualNumber = _service.Validate(_numberAsString);
            // Assert
            Assert.AreEqual(_expectedNumber, actualNumber);
        }

        [TestMethod]
        public void Validate_NullOrEmptyOrWhitespace_Null() {
            // Arrange
            _service = new NumberService();
            _numberAsString = null;
            _expectedNumber = null;
            // Act
            var actualNumber = _service.Validate(_numberAsString);
            // Assert
            Assert.AreEqual(_expectedNumber, actualNumber);
        }
    }
}