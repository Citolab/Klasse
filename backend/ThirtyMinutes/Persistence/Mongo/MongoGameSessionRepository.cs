using System;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ThirtyMinutes.Persistence.Mongo
{
    public class MongoGameSessionRepository : MongoRepository<GameSession>, IGameSessionRepository
    {
        public MongoGameSessionRepository(ILoggerFactory loggerFactory, MongoDatabaseOptions mongoDatabaseOptions) :
            base(loggerFactory, mongoDatabaseOptions)
        {
        }

        public GameSession GetById(Guid id)
        {
            return Collection.FindSync(o => o.Id == id).FirstOrDefault();
        }

        public void Add(GameSession gameSession)
        {
            Collection.InsertOne(gameSession);
        }

        public void Update(GameSession gameSession)
        {
            Collection.ReplaceOne(o => o.Id == gameSession.Id, gameSession);            
        }
    }
}