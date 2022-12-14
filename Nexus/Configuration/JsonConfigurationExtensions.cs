using System.Text.Json;

namespace Nexus
{
    public static class JsonConfigurationExtensions
    {
        public static bool Save<T>(this T value, JsonSerializerOptions? options = null)
            where T : JsonConfiguration, new()
            => IConfiguration.Save(value, options);

        public static async Task<bool> SaveAsync<T>(this T value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
            where T : JsonConfiguration, new()
            => await IConfiguration.SaveAsync(value, options, cancellationToken);
    }
}
