using MongoDB.Driver;
using System.Text.Json;

namespace Nexus
{
    /// <summary>
    ///     Represents the configuration for a mongo database.
    /// </summary>
    public class StorageConfiguration
    {
        /// <summary>
        ///     The database to use.
        /// </summary>
        public string DataPathName { get; set; } = string.Empty;

        /// <summary>
        ///     The url to connect to.
        /// </summary>
        public MongoUrl? DatabaseUrl { get; set; }

        /// <summary>
        ///     The root path for local storage.
        /// </summary>
        public string LocalPathName { get; set; } = string.Empty;

        /// <summary>
        ///     The serialization options for local storage.
        /// </summary>
        public JsonSerializerOptions? SerializerOptions { get; set; }
    }
}
