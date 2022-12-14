namespace Nexus
{
    public static class JsonModelExtensions
    {
        public static async ValueTask<bool> SaveAsync<T>(this T? entity, CancellationToken cancellationToken = default)
            where T : JsonModel, new()
            => entity is not null && await JsonModelHelper<T>.SaveAsync(entity, cancellationToken);

        public static async ValueTask<bool> DeleteAsync<T>(this T? entity, CancellationToken cancellationToken = default)
            where T : JsonModel, new()
            => entity is not null && await JsonModelHelper<T>.DeleteAsync(entity, cancellationToken);
    }
}
