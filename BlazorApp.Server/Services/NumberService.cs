using System.Text.RegularExpressions;
using BlazorApp.Server.Interfaces;
using BlazorApp.Server.Properties;

namespace BlazorApp.Server.Services {
    public class NumberService : INumberService {
        private static Regex Regex => new Regex(Resources.ValidNumberRegex);

        public int? Validate(string numberToCheck) {
            if (string.IsNullOrWhiteSpace(numberToCheck)) return null;
            if (!Regex.IsMatch(numberToCheck)) return null;
            return int.Parse(numberToCheck);
        }
    }
}