using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Sample.Console
{
    public sealed class MyDBModel : BsonModel
    {
        public string Value { get; set; } = string.Empty;
    }
}
