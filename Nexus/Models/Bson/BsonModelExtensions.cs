using MongoDB.Driver;
using System.Linq.Expressions;

namespace Nexus
{
    public static class BsonModelExtensions
    {
        public static async ValueTask<bool> SaveAsync<T, TField>(this T? entity, Expression<Func<T, TField>> expression, TField value, CancellationToken cancellationToken = default)
            where T : BsonModel, new()
            => entity is not null && await BsonModelHelper<T>.SaveAsync(entity, Builders<T>.Update.Set(expression, value), cancellationToken);

        public static async ValueTask<bool> DeleteAsync<T>(T model, CancellationToken cancellationToken = default)
            where T : BsonModel, new()
            => await BsonModelHelper<T>.DeleteAsync(model, cancellationToken);
    }
}
