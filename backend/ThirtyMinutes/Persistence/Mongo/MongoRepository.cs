using System;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ThirtyMinutes.Persistence.Mongo
{
    public abstract class MongoRepository<T>
    {
        protected readonly IMongoCollection<T> Collection;

        protected MongoRepository(ILoggerFactory loggerFactory, MongoDatabaseOptions mongoDatabaseOptions)
        {
            ILogger logger = loggerFactory.CreateLogger<MongoGameRepository>();

            try
            {
                MongoClient client;
                try
                {
                    var mongoClientSettings = new MongoClientSettings
                    {
                        Server = MongoServerAddress.Parse(mongoDatabaseOptions.ConnectionString)
                    };
                    client = new MongoClient(mongoClientSettings);
                }
                catch
                {
                    client = new MongoClient(mongoDatabaseOptions.ConnectionString);
                }

                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (string.IsNullOrWhiteSpace(environment))
                {
                    environment = "Development";
                }

                var mongoDatabase = client.GetDatabase($"{mongoDatabaseOptions.DatabaseName}-{environment}");
                Collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
            }
            catch (Exception exception)
            {
                logger.LogCritical(
                    $"Error while connecting to {mongoDatabaseOptions.ConnectionString}.", exception);
                throw;
            }
        }
    }
}