namespace ThirtyMinutes.Persistence.Mongo
{
    public class MongoDatabaseOptions
    {
        public string ConnectionString { get; }
        public string DatabaseName { get; }

        public MongoDatabaseOptions(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
        }
    }
}