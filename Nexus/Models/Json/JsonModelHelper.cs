namespace Nexus
{
    internal static class JsonModelHelper<T>
        where T : JsonModel, new()
    {
        public static readonly JsonCollection<T> Collection = new(typeof(T).Name + "s");

        public static async ValueTask<bool> SaveAsync(T model, CancellationToken cancellationToken = default)
            => await Collection.InsertOrUpdateDocumentAsync(model, cancellationToken);

        public static async ValueTask<bool> DeleteAsync(T model, CancellationToken cancellationToken = default)
            => await Collection.DeleteDocumentAsync(model, cancellationToken);

        public static async ValueTask<T?> GetAsync(Func<T, bool> func, bool createOnFailedFetch, CancellationToken cancellationToken = default)
        {
            var document = Collection.FindDocument(func);

            if (createOnFailedFetch && document is null)
                document = await CreateAsync(x => { }, cancellationToken);

            return document;
        }

        public static async ValueTask<T> CreateAsync(Action<T> action, CancellationToken cancellationToken = default)
        {
            var document = new T();

            action(document);

            await Collection.InsertOrUpdateDocumentAsync(document, cancellationToken);

            return document;
        }
    }
}
