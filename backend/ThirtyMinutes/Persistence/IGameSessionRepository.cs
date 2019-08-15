using System;

namespace ThirtyMinutes.Persistence
{
    public interface IGameSessionRepository
    {
        GameSession GetById(Guid id);
        void Add(GameSession gameSession);
        void Update(GameSession gameSession);
    }
}