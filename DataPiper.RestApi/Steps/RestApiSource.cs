using Polly;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataPiper
{
    internal class RestApiSource : Source
    {
        //constructors
        public RestApiSource(IServiceProvider svcProvider) : base(svcProvider) { }

        //properties
        private new RestApiSourceOptions Options { get => (RestApiSourceOptions)base.Options; set => base.Options = value; }

        //methods
        protected override IEnumerable<FileInfo> Extract()
        {
            var extractedFiles = new List<FileInfo>();

            //build request object (could easily enhance RestApiSource to allow headers/body/timeout/etc.)
            var client = new RestClient(Options.Uri);
            var request = new RestRequest("", Enum.Parse<Method>(Options.Method.ToTitleCase()));
            
            //setup retry policy
            Policy<RestResponse> retryPolicy = GetRetryPolicy();

            //execute
            LogService.LogDebug($"Executing request");
            var response = retryPolicy.Execute(() => client.Execute(request));

            //check response
            string responseString = GetResponseString(response);
            LogService.LogDebug(responseString.Substring(0, Math.Min(1000, responseString.Length)));
            LogService.LogTrace(responseString);
            if (!response.IsSuccessful)
                throw new Exception(responseString);

            //save as file
            var downloadedFilePath = Path.Combine(WorkingDirectory, Guid.NewGuid() + ".json");
            LogService.LogDebug($"Saving response to {downloadedFilePath}");
            File.WriteAllText(downloadedFilePath, response.Content);
            var file = new FileInfo(downloadedFilePath);

            extractedFiles.Add(file);
            return extractedFiles;
        }

        private Policy<RestResponse> GetRetryPolicy()
        {
            if (Options.RetryWaitSeconds.Count() == 0)
                return Policy.NoOp<RestResponse>();

            var waitSeconds = Options.RetryWaitSeconds.Select(i => TimeSpan.FromSeconds(i));

            return Policy
                .Handle<Exception>()
                .OrResult<RestResponse>(r => !r.IsSuccessful)
                .WaitAndRetry(waitSeconds, onRetry: (response, timespan, retryCount, context) =>
                {
                    if (response.Exception != null)
                        LogService.LogError(response.Exception.ToString());
                    else
                        LogService.LogError(GetResponseString(response.Result));
                });
        }
        private static string GetResponseString(RestResponse response)
        {
            if (response == null)
                return "Response: null";
            return $"Response: {response.StatusCode} | {response.StatusDescription} | {response.Content} | {response.ErrorMessage}";
        }
    }
}
