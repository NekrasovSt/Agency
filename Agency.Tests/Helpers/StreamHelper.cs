using System.IO;
using System.Text;

namespace Agency.Tests.Helpers
{
    public static class StreamHelper
    {
        public static Stream ToStream(this string str, Encoding enc = null)
        {
            enc = enc ?? Encoding.Default;
            return new MemoryStream(enc.GetBytes(str));
        }
    }
}