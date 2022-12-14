using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nexus.Sample.Console
{
    public sealed class MyModel : JsonModel
    {
        public string Value { get; set; } = string.Empty;
    }
}
