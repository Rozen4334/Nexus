using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nexus
{
    public static class GetRequest
    {
        public static JsonGetRequest<T> Json<T>()
            where T : JsonModel, new()
            => new(x => true);

        public static JsonGetRequest<T> Json<T>(Func<T, bool> request)
            where T : JsonModel, new()
            => new(request);

        public static BsonGetRequest<T> Bson<T>()
            where T : BsonModel, new()
            => new(x => true);

        public static BsonGetRequest<T> Bson<T>(Expression<Func<T, bool>> request)
            where T : BsonModel, new()
            => new(request);
    }
}
