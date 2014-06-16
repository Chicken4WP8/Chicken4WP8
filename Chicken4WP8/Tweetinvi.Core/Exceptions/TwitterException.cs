using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Exceptions;

namespace Tweetinvi.Logic.Exceptions
{
    public class TwitterException : WebException, ITwitterException
    {
        public WebException WebException { get; private set; }
        public string URL { get; set; }
        public int StatusCode { get; private set; }
        public string TwitterDescription { get; private set; }
        public DateTime CreationDate { get; private set; }
        public IEnumerable<ITwitterExceptionInfo> TwitterExceptionInfos { get; private set; }

        public TwitterException(
            IWebExceptionInfoExtractor webExceptionInfoExtractor,
            WebException webException,
            string url)
        {
            CreationDate = DateTime.Now;
            WebException = webException;
            URL = url;
            StatusCode = webExceptionInfoExtractor.GetWebExceptionStatusNumber(webException);
            TwitterExceptionInfos = webExceptionInfoExtractor.GetTwitterExceptionInfo(webException);
            TwitterDescription = webExceptionInfoExtractor.GetStatusCodeDescription(StatusCode);
        }

        public override string ToString()
        {
            string date = String.Format("--- Date : {0}\r\n", CreationDate.ToLocalTime());
            string url = URL == null ? String.Empty : String.Format("URL : {0}\r\n", URL);
            string code = String.Format("Code : {0}\r\n", StatusCode);
            string description = String.Format("Error documentation description : {0}\r\n", TwitterDescription);

            string exceptionInfos = String.Empty;
            foreach (var twitterExceptionInfo in TwitterExceptionInfos)
            {
                exceptionInfos += String.Format("{0} ({1})\r\n", twitterExceptionInfo.Message, twitterExceptionInfo.Code);
            }

            return String.Format("{0}{1}{2}{3}{4}", date, url, code, description, exceptionInfos);
        }
    }
}