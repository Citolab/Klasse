using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThirtyMinutes.Helpers;
using ThirtyMinutes.Model;
using ThirtyMinutes.Persistence;
using ErrorResponse = ThirtyMinutes.Model.ErrorResponse;

namespace ThirtyMinutes.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameSessionRepository _gameSessionRepository;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public GameController(IGameRepository gameRepository, IDataProtectionProvider dataProtectionProvider,
            IGameSessionRepository gameSessionRepository)
        {
            _gameRepository = gameRepository;
            _dataProtectionProvider = dataProtectionProvider;
            _gameSessionRepository = gameSessionRepository;
        }

        [HttpPost("start/{gameId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StartGameResponse), StatusCodes.Status200OK)]
        public IActionResult StartGame(int gameId)
        {
            var now = DateTime.Now;
            var game = _gameRepository.GetById(gameId);
            if (game == null)
            {
                return NotFound(new ErrorResponse(39, "Unknown game id."));
            }

            var sessionCookieData =
                Request.Cookies.Get<SessionCookieData>(Strings.SessionDataCookieName, _dataProtectionProvider);
            if (sessionCookieData == null || sessionCookieData.GameId != gameId)
            {
                sessionCookieData = CreateSession(gameId, now);
                Response.Cookies.Set(Strings.SessionDataCookieName, sessionCookieData, _dataProtectionProvider);
            }

            var remainingSeconds =
                game.GetRemainingSeconds(sessionCookieData.StartTime, now, sessionCookieData.TotalPenaltySeconds);
            return Ok(new StartGameResponse
            {
                StartTime = sessionCookieData.StartTime,
                SolutionLength = game.SolutionLength,
                MaxTimeInMinutes = game.MaxTimeInMinutes,
                PenaltySeconds = game.PenaltySeconds,
                RemainingSeconds = remainingSeconds,
                State = sessionCookieData.GameState
            });
        }

        [HttpGet("gamerunning/{gameId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StartGameResponse), StatusCodes.Status200OK)]
        public IActionResult GameRunning(int gameId)
        {
            var sessionCookieData =
                Request.Cookies.Get<SessionCookieData>(Strings.SessionDataCookieName, _dataProtectionProvider);

            if (sessionCookieData != null && (sessionCookieData.GameState == GameState.Started || sessionCookieData.GameState == GameState.Continued) )
            {
                return Ok(true);
            } else {
                return Ok(false);

            }
        }

        private SessionCookieData CreateSession(int gameId, DateTime now)
        {
            var sessionCookieData = new SessionCookieData
            {
                GameId = gameId,
                StartTime = now,
                TotalPenaltySeconds = 0,
                LastResponseCheck = DateTime.MinValue
            };
            var gameSession = new GameSession(sessionCookieData.Id)
            {
                StartTime = sessionCookieData.StartTime,
                TotalPenaltySeconds = sessionCookieData.TotalPenaltySeconds,
                GameId = gameId
            };

            var ipAddressDetails =
                IpAddressHelper.GetIpAddressDetails(HttpContext.Connection.RemoteIpAddress.ToString());
            gameSession.IpAddressDetails = ipAddressDetails;
            _gameSessionRepository.Add(gameSession);
            return sessionCookieData;
        }

        [HttpPost("continue")]
        public IActionResult Continue()
        {
            var sessionCookieData =
                Request.Cookies.Get<SessionCookieData>(Strings.SessionDataCookieName, _dataProtectionProvider);
            if (sessionCookieData != null)
            {
                var gameSession = _gameSessionRepository.GetById(sessionCookieData.Id);
                if (gameSession.State == GameState.Passed || gameSession.State == GameState.Stopped)
                {
                    return BadRequest(new ErrorResponse(3, "Already passed or stopped this game session."));
                }
                gameSession.State = GameState.Continued;
                _gameSessionRepository.Update(gameSession);
                sessionCookieData.GameState = GameState.Continued;
                Response.Cookies.Set(Strings.SessionDataCookieName, sessionCookieData, _dataProtectionProvider);
            }

            return Ok();
        }

        [HttpPost("stop")]
        public IActionResult Stop()
        {
            var sessionCookieData =
                Request.Cookies.Get<SessionCookieData>(Strings.SessionDataCookieName, _dataProtectionProvider);
            if (sessionCookieData != null)
            {
                var gameSession = _gameSessionRepository.GetById(sessionCookieData.Id);
                if (gameSession.State != GameState.Passed)
                {
                    gameSession.State = GameState.Stopped;
                }
                gameSession.FinishTime = DateTime.Now;
                _gameSessionRepository.Update(gameSession);
            }

            Response.Cookies.Delete(Strings.SessionDataCookieName);
            return Ok();
        }

        [HttpPost("checkanswer")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CheckAnswerResponse), StatusCodes.Status200OK)]
        public IActionResult CheckAnswer([FromBody] string answer)
        {
            var now = DateTime.Now;
            var sessionCookieData =
                Request.Cookies.Get<SessionCookieData>(Strings.SessionDataCookieName, _dataProtectionProvider);
            if (sessionCookieData == null)
            {
                return BadRequest(new CheckAnswerResponse(13, "No session cookie found."));
            }

            var game = _gameRepository.GetById(sessionCookieData.GameId);
            var lastResponseCheck = sessionCookieData.LastResponseCheck;
            sessionCookieData.LastResponseCheck = now;

            // Check rate limit, wait 5 seconds before rechecking
            if (now - lastResponseCheck < TimeSpan.FromSeconds(5))
            {
                Response.Cookies.Set(Strings.SessionDataCookieName, sessionCookieData, _dataProtectionProvider);
                return BadRequest(new ErrorResponse(144, "Exceeding check rate limit."));
            }

            if (game == null)
            {
                Response.Cookies.Set(Strings.SessionDataCookieName, sessionCookieData, _dataProtectionProvider);
                return BadRequest(new ErrorResponse(39, "Unknown game id."));
            }

            var gameSession = _gameSessionRepository.GetById(sessionCookieData.Id);

            if (gameSession.State == GameState.Passed || gameSession.State == GameState.Stopped)
            {
                return BadRequest(new ErrorResponse(3, "Already passed or stopped this game session."));
            }

            gameSession.Responses.Add(now, answer);
            var response = new CheckAnswerResponse(2, $"Incorrect, penalty time ({game.PenaltySeconds}s) applied.");
            if (game.CheckResponse(answer))
            {
                response.Code = 42;
                response.Message = "Correct";
                gameSession.FinishTime = now;
                gameSession.State = GameState.Passed;
                sessionCookieData.GameState = GameState.Passed;
            }
            else
            {
                sessionCookieData.TotalPenaltySeconds += game.PenaltySeconds;
                gameSession.TotalPenaltySeconds = sessionCookieData.TotalPenaltySeconds;
            }

            _gameSessionRepository.Update(gameSession);

            response.TotalPenaltySecondsApplied = sessionCookieData.TotalPenaltySeconds;
            response.RemainingSeconds =
                game.GetRemainingSeconds(sessionCookieData.StartTime, now, sessionCookieData.TotalPenaltySeconds);
            Response.Cookies.Set(Strings.SessionDataCookieName, sessionCookieData, _dataProtectionProvider);
            return Ok(response);
        }
    }
}