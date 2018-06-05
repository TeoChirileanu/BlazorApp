namespace BlazorApp.Server.Interfaces {
    public interface IResetableLogger : ILogger {
        void Reset();
    }
}