using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.WebLogic
{
    public class TwitterRequestGenerator : ITwitterRequestGenerator
    {
        public const int BUFFER_SIZE = 4096;

        private readonly IOAuthWebRequestGenerator _webRequestGenerator;
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly IWebHelper _webHelper;

        public TwitterRequestGenerator(
            IOAuthWebRequestGenerator webRequestGenerator,
            ICredentialsAccessor credentialsAccessor,
            IWebHelper webHelper)
        {
            _webRequestGenerator = webRequestGenerator;
            _credentialsAccessor = credentialsAccessor;
            _webHelper = webHelper;
        }

        public HttpWebRequest GetQueryWebRequest(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> headers = null)
        {
            if (headers == null)
            {
                headers = _webRequestGenerator.GenerateParameters(_credentialsAccessor.CurrentThreadCredentials);
            }

            return _webRequestGenerator.GenerateWebRequest(url, httpMethod, headers);
        }

        public HttpWebRequest GetQueryWebRequestWithTemporaryCredentials(
            string url,
            HttpMethod httpMethod,
            ITemporaryCredentials temporaryCredentials,
            IEnumerable<IOAuthQueryParameter> parameters = null)
        {
            var headers = _webRequestGenerator.GenerateApplicationParameters(temporaryCredentials, parameters);
            return GetQueryWebRequest(url, httpMethod, headers);
        }

        private IMultipartElement GenerateMultipartElement(IMedia media, IMultipartRequestConfiguration configuration)
        {
            var additionalParameters = new Dictionary<string, string>();
            // additionalParameters.Add("filename", media.FileName);

            var element = new MultipartElement
            {
                Boundary = configuration.Boundary,
                ContentId = "media[]",
                ContentDispositionType = "form-data",
                ContentType = "application/octet-stream",
                AdditionalParameters = additionalParameters,
                Data = configuration.EncodingAlgorithm.GetString(media.Data, 0, media.Data.Length),
            };

            return element;
        }

        public class MultipartWebRequest : IMultipartWebRequest
        {
            public MultipartWebRequest(HttpWebRequest httpWebRequest, byte[] content)
            {
                WebRequest = httpWebRequest;
                Content = content;
            }

            public HttpWebRequest WebRequest { get; private set; }
            public byte[] Content { get; private set; }

            public string GetResult()
            {
                var manualResetEvent = new ManualResetEvent(false);
                string result = null;

                WebRequest.BeginGetRequestStream(asyncResult =>
                {
                    HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                    using (var reqStream = request.EndGetRequestStream(asyncResult))
                    {
                        int offset = 0;

                        while (offset < Content.Length)
                        {
                            int bytesToWrite = Math.Min(BUFFER_SIZE, Content.Length - offset);
                            reqStream.Write(Content, offset, bytesToWrite);
                            offset += bytesToWrite;
                        }

                        reqStream.Flush();
                    }

                    request.BeginGetResponse(a =>
                    {
                        try
                        {
                            var response = request.EndGetResponse(a);
                            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                                manualResetEvent.Set();
                            }
                        }
                        catch (Exception ex)
                        {
# if DEBUG
                            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                            if (TweetinviConfig.SHOW_DEBUG)
                            // ReSharper disable once CSharpWarnings::CS0162
                            {
                                Debug.WriteLine(ex);
                            }
# endif
                            manualResetEvent.Set();
                        }
                    }, null);
                }, WebRequest);

                manualResetEvent.WaitOne();
                return result;
            }

            public Task<string> GetResultAsync()
            {
                throw new NotImplementedException();
            }
        }

        public IMultipartWebRequest ExecuteMediaQueryWebRequest(string url, HttpMethod httpMethod, IEnumerable<IMedia> medias)
        {
            var baseURL = _webHelper.GetBaseURL(url);
            var requestConfiguration = new MultipartRequestConfiguration();
            var multipartElements = medias.Select(media => GenerateMultipartElement(media, requestConfiguration));

            var requestContent = _webRequestGenerator.GenerateMultipartContent(url, httpMethod, requestConfiguration, multipartElements);
            var request = GetQueryWebRequest(baseURL, httpMethod);
            request.ContentType = "multipart/form-data;boundary=" + requestConfiguration.Boundary;

            return new MultipartWebRequest(request, requestContent);
        }
    }
}