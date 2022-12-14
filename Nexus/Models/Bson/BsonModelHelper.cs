using MongoDB.Driver;
using System.Linq.Expressions;

namespace Nexus
{
    internal static class BsonModelHelper<T>
        where T : BsonModel, new()
    {
        public static readonly BsonCollection<T> Collection = new(typeof(T).Name + "s");

        public static async ValueTask<bool> SaveAsync(T model, UpdateDefinition<T> updateDefinition, CancellationToken cancellationToken = default)
        {
            if (model.State is ModelState.Stateless or ModelState.Deleted or ModelState.Deserializing)
                return false;

            return await Collection.ModifyDocumentAsync(model, updateDefinition, cancellationToken);
        }

        public static async ValueTask<bool> DeleteAsync(T model, CancellationToken cancellationToken = default)
        {
            if (model.State is ModelState.Stateless or ModelState.Deleted)
                return false;

            model.State = ModelState.Deleted;

            return await Collection.DeleteDocumentAsync(model, cancellationToken);
        }

        public static async ValueTask<T?> GetAsync(Expression<Func<T, bool>> func, bool createOnFailedFetch, CancellationToken cancellationToken = default)
        {
            var value = await Collection.FindDocumentAsync(func, cancellationToken);

            if (value is null)
            {
                if (createOnFailedFetch)
                    value = await CreateAsync((x) => { }, cancellationToken);

                else
                    return default;
            }

            value.State = ModelState.Ready;

            return value;
        }

        public static async ValueTask<T> CreateAsync(Action<T> action, CancellationToken cancellationToken = default)
        {
            var value = new T();

            action(value);

            await Collection.InsertOrUpdateDocumentAsync(value, cancellationToken);

            value.State = ModelState.Ready;

            return value;
        }
    }
}
