using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Nexus
{
    public abstract class BsonModel : IModel
    {
        [BsonId]
        public ObjectId ObjectId { get; set; }

        [BsonIgnore]
        public ModelState State { get; set; }

        [BsonIgnore]
        public ModelType ModelType { get; } = ModelType.Bson;

        public static async ValueTask<bool> SaveAsync<T, TField>(T? entity, Expression<Func<T, TField>> expression, TField value, CancellationToken cancellationToken = default)
            where T : BsonModel, new()
            => entity is not null && await BsonModelHelper<T>.SaveAsync(entity, Builders<T>.Update.Set(expression, value), cancellationToken);

        public static async ValueTask<bool> DeleteAsync<T>(T model, CancellationToken cancellationToken = default)
            where T : BsonModel, new()
            => await BsonModelHelper<T>.DeleteAsync(model, cancellationToken);

        public static async ValueTask<T> CreateAsync<T>(Action<T> action, CancellationToken cancellationToken = default)
            where T : BsonModel, new()
            => await BsonModelHelper<T>.CreateAsync(action, cancellationToken);

        public static async ValueTask<T?> GetAsync<T>(Expression<Func<T, bool>> func, bool createOnFailedFetch = false, CancellationToken cancellationToken = default)
            where T : BsonModel, new()
            => await BsonModelHelper<T>.GetAsync(func, createOnFailedFetch, cancellationToken);
    }
}
