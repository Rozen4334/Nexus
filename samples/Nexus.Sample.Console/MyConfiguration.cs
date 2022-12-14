using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Sample.Console
{
    internal class MyConfiguration : JsonConfiguration
    {
        public override string FilePath 
            => "config.json";

        public string DatabaseUrl { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public string LocalFilePath { get; set; } = string.Empty;
    }
}
