using System.Collections.Generic;

namespace DataPiper
{
    public class RestApiSourceOptions : Options
    {
        public string Method { get; set; }
        public string Uri { get; set; }
        public IEnumerable<int> RetryWaitSeconds { get; set; }
    }
}