using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Nexus
{
    public class BsonCollection<T>
        where T : BsonModel, new()
    {
        private readonly MongoCollectionBase<T> _collection;

        /// <summary>
        ///     Creates or finds a collection from passed <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException">Thrown when the manager cannot succesfully create a collection for provided document name.</exception>
        public BsonCollection(string name)
        {
            if (StorageProvider.IsDatabaseConfigured())
                _collection = StorageProvider.GetMongoCollection<T>(name);
            else
                throw new InvalidOperationException("Cannot fetch a mongo collection if the database is not live.");
        }

        /// <summary>
        ///     Inserts a document from the passed <paramref name="document"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async ValueTask InsertDocumentAsync(T document, CancellationToken cancellationToken = default)
            => await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);

        /// <summary>
        ///     Inserts a collection of documents from the passed <paramref name="documents"/>.
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        public async ValueTask InsertDocumentsAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default)
            => await _collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

        /// <summary>
        ///     Updates an existing entity to the new instance if found. Otherwise creates a new entity.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async ValueTask InsertOrUpdateDocumentAsync(T document, CancellationToken cancellationToken = default)
        {
            if (document.ObjectId == ObjectId.Empty)
                await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
            else
                await _collection.ReplaceOneAsync(x => x.ObjectId == document.ObjectId, document, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///     Updates a document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async ValueTask<bool> UpdateDocumentAsync(T document, CancellationToken cancellationToken = default)
        {
            var entity = await (await _collection.FindAsync(x => x.ObjectId == document.ObjectId, cancellationToken: cancellationToken))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (entity is not null)
            {
                await _collection.ReplaceOneAsync(x => x.ObjectId == document.ObjectId, document, cancellationToken: cancellationToken);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Modifies an existing document in atomic declaration.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async ValueTask<bool> ModifyDocumentAsync(T document, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
            => (await _collection.UpdateOneAsync(x => x.ObjectId == document.ObjectId, update, cancellationToken: cancellationToken)).IsAcknowledged;

        /// <summary>
        ///     Deletes a document from provided <paramref name="document"/>
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteDocumentAsync(T document, CancellationToken cancellationToken = default)
            => (await _collection.DeleteOneAsync(x => x.ObjectId == document.ObjectId, cancellationToken: cancellationToken)).IsAcknowledged;

        /// <summary>
        ///     Deletes a set of document matching the provided <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteManyDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            => (await _collection.DeleteManyAsync<T>(filter, cancellationToken: cancellationToken)).IsAcknowledged;

        /// <summary>
        ///     Finds the first occurence in a range of documents returned by the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async ValueTask<T> FindDocumentAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            => await (await _collection.FindAsync(filter, cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        /// <summary>
        ///     Returns all found documents matching <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<T> FindManyDocumentsAsync(Expression<Func<T, bool>> filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collection = await _collection.FindAsync(filter, cancellationToken: cancellationToken);

            foreach (var entity in collection.ToEnumerable(cancellationToken: cancellationToken))
            {
                yield return entity;
            }
        }

        /// <summary>
        ///     Gets the first document from a collection.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<T> GetFirstDocumentAsync(CancellationToken cancellationToken = default)
            => await (await _collection.FindAsync(new BsonDocument(), cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        /// <summary>
        ///     Gets all documents from a collection.
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<T> GetAllDocumentsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collection = await _collection.FindAsync(new BsonDocument(), cancellationToken: cancellationToken);

            foreach (var entity in collection.ToEnumerable(cancellationToken: cancellationToken))
            {
                yield return entity;
            }
        }
    }
}
