using System;
using System.Text;
using BlazorApp.Server.Interfaces;

namespace BlazorApp.Server.Services {
    public class MemoryLogger : ILogger {
        private StringBuilder Messages { get; set; } = new StringBuilder();

        public void Log(string message) {
            var time = DateTime.Now.ToString("HH:mm:ss");
            Messages.AppendLine($"{time} {message}");
        }

        public string GetLog() => Messages.ToString();
        public void Reset() {
            Messages = new StringBuilder();
        }
    }
}