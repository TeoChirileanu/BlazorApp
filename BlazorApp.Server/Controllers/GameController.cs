using BlazorApp.Server.Interfaces;
using BlazorApp.Server.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Server.Controllers {
    [Route("api/[controller]")]
    public class GameController : Controller {
        public GameController(INumberService numberService) {
            NumberService = numberService;
        }

        private INumberService NumberService { get; }

        private static int CorrectNumber => int.Parse(Resources.CorrectNumber);

        [HttpGet("[action]/{numberToCheck}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status303SeeOther)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> CheckNumber([FromRoute] string numberToCheck) {
            var guessedNumber = NumberService.Validate(numberToCheck);
            if (guessedNumber is null) {
                var errorMessage = string.Format(Resources.InvalidNumberMessage, Resources.LowerBound,
                    Resources.UpperBound);
                return BadRequest(errorMessage);
            }

            if (guessedNumber < CorrectNumber) {
                return StatusCode(303, Resources.TooLowMessage);
            }

            if (guessedNumber > CorrectNumber) {
                return StatusCode(303, Resources.TooHighMessage);
            }

            return Ok(Resources.CorrectGuessMessage);
        }
    }
}