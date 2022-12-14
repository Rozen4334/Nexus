using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus
{
    public static class CreateRequest
    {
        public static JsonCreateRequest<T> Json<T>()
            where T : JsonModel, new()
            => new(x => { });

        public static JsonCreateRequest<T> Json<T>(Action<T> request)
            where T : JsonModel, new()
            => new(request);

        public static BsonCreateRequest<T> Bson<T>()
            where T : BsonModel, new()
            => new(x => { });

        public static BsonCreateRequest<T> Bson<T>(Action<T> request)
            where T : BsonModel, new()
            => new(request);
    }
}
