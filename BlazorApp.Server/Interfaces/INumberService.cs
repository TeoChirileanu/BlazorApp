namespace BlazorApp.Server.Interfaces {
    public interface INumberService {
        int? Validate(string numberToCheck);
    }
}