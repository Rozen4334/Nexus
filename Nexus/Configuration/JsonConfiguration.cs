using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nexus
{
    public abstract class JsonConfiguration : IConfiguration
    {
        /// <summary>
        ///     The path where this file should be saved.
        /// </summary>
        [JsonIgnore]
        public abstract string FilePath { get; }
    }
}
