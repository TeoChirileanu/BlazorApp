using System;
using BlazorApp.Server.Interfaces;
using BlazorApp.Server.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlazorApp.Tests.UnitTests {
    [TestClass]
    public class MemoryLoggerTests {
        private IResetableLogger _logger;
        private string _message;
        private string _messages;

        [TestMethod]
        public void Log_Message_ItShouldBeInMessages() {
            // Arrange
            _logger = new MemoryLogger();
            _message = "foo";
            // Act
            _logger.Log(_message);
            _messages = _logger.GetLog();
            // Assert
            Assert.IsTrue(_messages.Contains(_message));
        }

        [TestMethod]
        public void Log_NullOrEmptyOrWhitespace_Exception() {
            // Arrange
            _logger = new MemoryLogger();
            _message = null;
            // Act && Assert
            Assert.ThrowsException<ArgumentException>(() => _logger.Log(_message));
        }

        [TestMethod]
        public void Reset_Call_NoMessages() {
            // Arrange
            _logger = new MemoryLogger();
            _message = "foo";
            // Act
            _logger.Log(_message);
            _logger.Reset();
            _messages = _logger.GetLog();
            // Assert
            Assert.IsTrue(string.IsNullOrWhiteSpace(_messages));
        }
    }
}