using MongoDB.Driver;
using Nexus;
using Nexus.Sample.Console;
using System.Text.Json;

var config = IConfiguration.Load<MyConfiguration>();

var storageConfig = new StorageConfiguration()
{
    DatabaseUrl = new MongoUrl(config.DatabaseUrl),
    DataPathName = config.DatabaseName,
    LocalPathName = config.LocalFilePath,
    SerializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
    }
};

var database = new StorageProvider(storageConfig);

if (StorageProvider.IsDatabaseConfigured())
{
    var model = await IModel.GetAsync(GetRequest.Json<MyModel>(x => x.Value == "value"));

    var otherModel = await IModel.GetAsync(GetRequest.Bson<MyDBModel>(x => x.State == ModelState.Ready));
}

Environment.Exit(0);