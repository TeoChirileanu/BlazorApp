namespace BlazorApp.Server.Interfaces {
    //TODO add tests
    public interface ILogger {
        void Log(string message);

        string GetLog();
    }
}