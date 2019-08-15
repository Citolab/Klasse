using System;
using ThirtyMinutes.Persistence;

namespace ThirtyMinutes.Model
{
    /// <summary>
    /// Session data that will be exchanged with the client via cookie.
    /// </summary>
    public class SessionCookieData
    {
        public Guid Id;
        public int GameId;
        public int TotalPenaltySeconds;
        public DateTime StartTime;
        public DateTime LastResponseCheck;
        public GameState GameState;

        public SessionCookieData()
        {
            GameState = GameState.Started;
            Id = Guid.NewGuid();
        }
    }
}