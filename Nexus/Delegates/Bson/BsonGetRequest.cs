using System.Linq.Expressions;

namespace Nexus
{
    public readonly struct BsonGetRequest<T> 
        where T : BsonModel, new()
    {
        public Expression<Func<T, bool>> Request { get; }

        internal BsonGetRequest(Expression<Func<T, bool>> request)
        {
            Request = request;
        }

        public static implicit operator BsonGetRequest<T>(Expression<Func<T, bool>> request)
        {
            return new BsonGetRequest<T>(request);
        }

        public static implicit operator Expression<Func<T, bool>>(BsonGetRequest<T> request)
        {
            return request.Request;
        }
    }
}
