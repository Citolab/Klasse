using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ThirtyMinutes.Persistence.Mongo
{
    public class MongoGameRepository : MongoRepository<Game>, IGameRepository
    {
        public MongoGameRepository(ILoggerFactory loggerFactory, MongoDatabaseOptions mongoDatabaseOptions) : base(
            loggerFactory, mongoDatabaseOptions)
        {
            // todo move this to a seeder
            if (Collection.EstimatedDocumentCount() == 0)
            {
                Collection.InsertOne(new Game(1, "IDENTITEIT", 5, 30));
                Collection.InsertOne(new Game(2, "PASSWORD", 4, 20));
            }
        }

        public Game GetById(int id)
        {
            return Collection.FindSync(o => o.Id == id).FirstOrDefault();
        }
    }
}