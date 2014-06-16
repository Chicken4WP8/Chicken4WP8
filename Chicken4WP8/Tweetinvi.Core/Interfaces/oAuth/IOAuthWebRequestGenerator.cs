﻿using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.oAuth
{
    /// <summary>
    /// Generator of HttpWebRequest using OAuth specification
    /// </summary>
    public interface IOAuthWebRequestGenerator
    {
        IOAuthQueryParameter GenerateParameter(string key, string value, bool requiredForSignature, bool requiredForHeader, bool isPartOfOAuthSecretKey);
        
        IEnumerable<IOAuthQueryParameter> GenerateConsumerParameters(IConsumerCredentials consumerCredentials);
        IEnumerable<IOAuthQueryParameter> GenerateApplicationParameters(ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> additionalParameters = null);
        IEnumerable<IOAuthQueryParameter> GenerateParameters(IOAuthCredentials credentials, IEnumerable<IOAuthQueryParameter> additionalParameters = null);

        /// <summary>
        /// Generates an HttpWebRequest by giving minimal information
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="httpMethod">HTTP Method for the request</param>
        /// <param name="parameters">Parameters used to generate the query</param>
        /// <returns>The appropriate WebRequest</returns>
        HttpWebRequest GenerateWebRequest(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters);

        string GenerateAuthorizationHeader(Uri uri, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters);
        byte[] GenerateMultipartContent(string url, HttpMethod httpMethod, IMultipartRequestConfiguration multipartRequestConfiguration, IEnumerable<IMultipartElement> multipartElements);
    }
}