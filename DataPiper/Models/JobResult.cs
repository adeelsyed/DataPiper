using System;

namespace DataPiper
{
    public class JobResult : IJobResult
    {
        //proeprties
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }

        //constructors
        public JobResult(bool success)
        {
            IsSuccessful = success;
        }
        public JobResult(Exception ex)
        {
            IsSuccessful = false;
            ErrorMessage = ex.Message;
            ErrorException = ex;
        }
    }
}
