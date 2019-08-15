using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ThirtyMinutes.Persistence.InMemory
{
    public class TestGameSessionRepository : IGameSessionRepository
    {
        private readonly List<GameSession> _gameSessions;

        public TestGameSessionRepository()
        {
            _gameSessions = new List<GameSession>();
        }

        public GameSession GetById(Guid id)
        {
            var gameSession = _gameSessions.FirstOrDefault(g => g.Id == id);
            return gameSession != null ? DeepClone(gameSession) : null;
        }

        public void Add(GameSession gameSession)
        {
            if (_gameSessions.Any(g => g.Id == gameSession.Id))
            {
                throw new Exception("Duplicate game session id.");
            }

            _gameSessions.Add(gameSession);
        }

        public void Update(GameSession gameSession)
        {
            var oldGameSession = _gameSessions.FirstOrDefault(g => g.Id == gameSession.Id);
            if (oldGameSession == null)
            {
                throw new Exception("Not found.");
            }

            _gameSessions.Remove(oldGameSession);
            _gameSessions.Add(gameSession);
        }

        private GameSession DeepClone(GameSession gameSession)
        {
            var json = JsonConvert.SerializeObject(gameSession);
            return JsonConvert.DeserializeObject<GameSession>(json);
        }
    }
}