using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Threading;
using ZstdSharp.Unsafe;

namespace Nexus
{
    public sealed class JsonCollection<T>
        where T : JsonModel, new()
    {
        private readonly LocalCollectionBase<T> _collection;

        public JsonCollection(string name)
        {
            _collection = StorageProvider.GetLocalCollection<T>(name);
        }

        /// <summary>
        ///     Inserts a document from the passed <paramref name="document"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async ValueTask InsertDocumentAsync(T document, CancellationToken cancellationToken = default)
        {
            _collection.Values.Add(document);

            await _collection.SaveAsync(cancellationToken);
        }

        /// <summary>
        ///     Inserts a collection of documents from the passed <paramref name="documents"/>.
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        public async ValueTask InsertDocumentsAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default)
        {
            _collection.Values.AddRange(documents);

            await _collection.SaveAsync(cancellationToken);
        }

        /// <summary>
        ///     Updates an existing entity to the new instance if found. Otherwise creates a new entity.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async ValueTask<bool> InsertOrUpdateDocumentAsync(T document, CancellationToken cancellationToken = default)
        {
            if (_collection.Values.Contains(document))
            {
                await UpdateDocumentAsync(document, cancellationToken);
                return false;
            }

            else
            {
                await InsertDocumentAsync(document, cancellationToken);
                return true;
            }
        }

        /// <summary>
        ///     Updates a document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async ValueTask<bool> UpdateDocumentAsync(T document, CancellationToken cancellationToken = default)
        {
            var index = _collection.Values.IndexOf(document);

            if (index is not -1)
            {
                _collection.Values[index] = document;

                await _collection.SaveAsync(cancellationToken);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Deletes a document from provided <paramref name="document"/>
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteDocumentAsync(T document, CancellationToken cancellationToken = default)
        {
            var value = _collection.Values.Remove(document);

            if (!value)
                return value;

            await _collection.SaveAsync(cancellationToken);

            return value;
        }

        /// <summary>
        ///     Deletes a set of document matching the provided <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteManyDocumentsAsync(Func<T, bool> filter, CancellationToken cancellationToken = default)
        {
            var value = _collection.Values.RemoveAll(x => filter(x));

            if (value is 0)
                return false;

            await _collection.SaveAsync(cancellationToken);

            return true;
        }

        /// <summary>
        ///     Finds the first occurence in a range of documents returned by the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T? FindDocument(Func<T, bool> filter, T? defaultValue = default)
            => _collection.Values.FirstOrDefault(filter!, defaultValue);

        /// <summary>
        ///     Returns all found documents matching <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> FindManyDocuments(Func<T, bool> filter)
            => _collection.Values.Where(filter);

        /// <summary>
        ///     Gets all documents from a collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAllDocuments()
            => _collection.Values;

        /// <summary>
        ///     Gets the first document from a collection.
        /// </summary>
        /// <returns></returns>
        public T? GetFirstDocument()
            => _collection.Values.FirstOrDefault(defaultValue: null);
    }
}
