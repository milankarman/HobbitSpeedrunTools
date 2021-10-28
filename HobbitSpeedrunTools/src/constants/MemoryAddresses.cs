using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HobbitSpeedrunTools
{
    public static class MemoryAddresses
    {
        public const string local = "base+0035BA3C";
        public const string devMode = "7600e8";
        public const string loadingTriggers = "00777B18";
        public const string otherTriggers = "00777B04";
        public const string stamina = local + ",A04";
    }
}
