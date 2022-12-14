using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace Nexus
{
    public class StorageProvider
    {
        private static MongoClient? _client;

        private static MongoDatabaseBase? _database;

        public static StorageConfiguration Configuration { get; private set; } = null!;

        public StorageProvider(StorageConfiguration config)
        {
            config.SerializerOptions ??= new JsonSerializerOptions();

            if (config.DatabaseUrl is not null)
            {
                _client = new MongoClient(config.DatabaseUrl);
                ConnectDatabase(config);
            }
            Configuration = config;
        }

        private void ConnectDatabase(StorageConfiguration config)
        {
            if (string.IsNullOrEmpty(config.DataPathName))
                throw new ArgumentNullException(nameof(config.DataPathName));

            if (_client is not null)
            {
                _database = _client.GetDatabase(config.DataPathName) as MongoDatabaseBase;

                if (!IsDatabaseConfigured())
                    throw new InvalidOperationException("Database could not connect.");
            }
            else
                throw new InvalidOperationException("Client cannot resolve and was found null.");
        }

        public static MongoCollectionBase<T> GetMongoCollection<T>(string name)
            where T : BsonModel, new()
        {
            return _database?.GetCollection<T>(name) as MongoCollectionBase<T>
            ?? throw new ArgumentException("The configuration does not contain the required data to fetch mongo collections.", nameof(Configuration));
        }

        public static LocalCollectionBase<T> GetLocalCollection<T>(string name)
            where T : JsonModel, new()
        {
            var collection = new LocalCollectionBase<T>(Path.Combine(Configuration.LocalPathName, name), Configuration.SerializerOptions);

            if (collection is null)
                throw new ArgumentException("The configuration does not contain the required data to fetch local collections.", nameof(Configuration));

            collection.Read();

            return collection;
        }

        public static bool IsDatabaseConfigured()
        {
            try
            {
                _client?.ListDatabaseNames();
                return true;
            }
            catch (MongoException)
            {
                return false;
            }
        }

        public static string RunCommand(string command)
        {
            try
            {
                var result = _database?.RunCommand<BsonDocument>(BsonDocument.Parse(command));
                return result.ToJson();
            }
            catch (Exception ex) when (ex is FormatException or MongoCommandException)
            {
                throw;
            }
        }
    }
}
