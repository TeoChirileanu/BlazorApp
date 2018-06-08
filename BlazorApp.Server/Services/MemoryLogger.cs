using System;
using System.Text;
using BlazorApp.Server.Interfaces;
using BlazorApp.Server.Properties;

namespace BlazorApp.Server.Services {
    public class MemoryLogger : IResetableLogger {
        private StringBuilder Messages { get; set; } = new StringBuilder();

        public void Log(string message) {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(Resources.InvalidStringMessage);
            var time = DateTime.Now.ToString(Resources.DateTimeFormat);
            Messages.AppendLine($"{time} {message}");
        }

        public string GetLog() {
            var messages = Messages.ToString();
            Reset();
            return messages;
        } 

        public void Reset() {
            Messages = new StringBuilder();
        }
    }
}