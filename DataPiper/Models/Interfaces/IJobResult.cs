using System;

namespace DataPiper
{
    public interface IJobResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }
}
