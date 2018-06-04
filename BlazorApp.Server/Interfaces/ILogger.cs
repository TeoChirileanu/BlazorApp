namespace BlazorApp.Server.Interfaces {
    public interface ILogger {
        void Log(string message);

        string GetLog();
        void Reset();
    }
}