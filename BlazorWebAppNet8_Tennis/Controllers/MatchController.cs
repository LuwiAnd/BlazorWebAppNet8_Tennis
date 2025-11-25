using BlazorWebAppNet8_Tennis.DTO.Match;
using BlazorWebAppNet8_Tennis.DTO.Request;
using BlazorWebAppNet8_Tennis.DTO.Response;
using BlazorWebAppNet8_Tennis.DTO.Response;
using BlazorWebAppNet8_Tennis.Services.Backend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BlazorWebAppNet8_Tennis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;
        private readonly ScoreBoardService _scoreBoardService;
        public MatchController(MatchService matchService, ScoreBoardService scoreBoardService)
        {
            _matchService = matchService;
            _scoreBoardService = scoreBoardService;
        }


        [HttpPost("add-point/{player}")]
        public ActionResult<MatchStateDto> AddPoint(
            int player,
            [FromBody] MatchStateDto match
        )
        {
            if (player != 1 && player != 2)
                return BadRequest("Player must be 1 or 2.");

            var updated = _matchService.AddPoint(player, match);
            return Ok(updated);
        }


        //[HttpPost("score")]
        //public async Task<ActionResult<MatchResponseDto>> UpdateScore([FromBody] MatchRequestDto match)
        //{
        //    int? gameNo = match.PlayerOneGameScores.Count;
        //    if (gameNo == null || gameNo != match.PlayerTwoGameScores.Count)
        //        throw new ArgumentException("Game score lists must be of the same lenght.");

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    GameRequestDto game = new GameRequestDto
        //    {
        //        IsTiebreak = false,
        //        PlayerOneName = match.PlayerOneName,
        //        PlayerTwoName = match.PlayerTwoName,
        //        PlayerOnePoints = match.PlayerOnePoints,
        //        PlayerTwoPoints = match.PlayerTwoPoints
        //    };
        //    try
        //    {
        //        var result = await _matchService.CalculatePointsText(game);
        //        return Ok(result);
        //    }catch(ArgumentException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }catch(Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        //    }
        //}




        [HttpPost("update")]
        //public ActionResult<MatchResponseDto> UpdateMatch([FromBody] MatchRequestDto request)
        public ActionResult<MatchStateDto> UpdateMatch([FromBody] MatchStateDto state)
        {
            var updated = _matchService.UpdateMatch(state);
            return Ok(updated);
        }


        [HttpPost("scoreboard")]
        public ActionResult<ScoreBoardResponseDto> BuildScoreboard([FromBody] MatchStateDto state)
        {
            var dto = _scoreBoardService.BuildScoreBoard(state);
            return Ok(dto);
        }


    }
}
