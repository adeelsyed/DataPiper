using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataPiper
{
    public static class IEnumerableExtenstions
    {
        public static string ToCsv(this IEnumerable<FileInfo> list)
        {
            return string.Join(", ", list.Select(f => f.Name).ToArray());
        }
    }
}
