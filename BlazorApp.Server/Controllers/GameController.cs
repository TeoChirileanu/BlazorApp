using BlazorApp.Server.Interfaces;
using BlazorApp.Server.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Server.Controllers {
    [Route("api/[controller]")]
    public class GameController : Controller {
        public GameController(INumberService numberService, IResetableLogger logger) {
            NumberService = numberService;
            Logger = logger;
        }

        private INumberService NumberService { get; }
        private IResetableLogger Logger { get; }

        private static int CorrectNumber => int.Parse(Resources.CorrectNumber);

        [HttpGet("{number}/check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status303SeeOther)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> CheckNumber([FromRoute] string number) {
            var guessedNumber = NumberService.Validate(number);
            if (guessedNumber is null) {
                var errorMessage = string.Format(Resources.InvalidNumberMessage, Resources.LowerBound,
                    Resources.UpperBound);
                return BadRequest(errorMessage);
            }
            Logger.Log($"Checking number {guessedNumber}...");
            if (guessedNumber < CorrectNumber) {
                Logger.Log("Unfortunately, this number is too low.");
                return StatusCode(303, Resources.TooLowMessage);
            }

            if (guessedNumber > CorrectNumber) {
                Logger.Log("Unfortunately, this number is too high.");
                return StatusCode(303, Resources.TooHighMessage);
            }

            Logger.Log("Awesome, this is the correct number!");
            return Ok(Resources.CorrectGuessMessage);
        }

        [HttpGet("log")]
        public ActionResult<string> Logs() {
            var log = Logger.GetLog();
            if (string.IsNullOrWhiteSpace(log)) {
                return BadRequest("Error while getting the logs!");
            }

            Logger.Reset();
            return Ok(log);
        }
    }
}