using System.Text.Json.Serialization;

namespace Nexus
{
    public abstract class JsonModel : IModel
    {
        [JsonIgnore]
        public ModelType ModelType { get; } = ModelType.Json;

        public static async ValueTask<bool> SaveAsync<T>(T model, CancellationToken cancellationToken = default)
            where T : JsonModel, new()
            => await JsonModelHelper<T>.SaveAsync(model, cancellationToken);

        public static async ValueTask<bool> DeleteAsync<T>(T model, CancellationToken cancellationToken = default)
            where T : JsonModel, new()
            => await JsonModelHelper<T>.DeleteAsync(model, cancellationToken);

        public static async ValueTask<T> CreateAsync<T>(Action<T> action, CancellationToken cancellationToken = default)
            where T : JsonModel, new()
            => await JsonModelHelper<T>.CreateAsync(action, cancellationToken);

        public static async ValueTask<T?> GetAsync<T>(Func<T, bool> func, bool createOnFailedFetch = false, CancellationToken cancellationToken = default)
            where T : JsonModel, new()
            => await JsonModelHelper<T>.GetAsync(func, createOnFailedFetch, cancellationToken);
    }
}
