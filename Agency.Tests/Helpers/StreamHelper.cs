using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Agency.Tests.Helpers
{
    public static class StreamHelper
    {
        public static Stream ToStream(this string str, Encoding enc = null)
        {
            enc = enc ?? Encoding.Default;
            return new MemoryStream(enc.GetBytes(str));
        }

        public static Stream ToJsonStream(this object obj)
        {
            return JsonConvert.SerializeObject(obj).ToStream();
        }
    }
}