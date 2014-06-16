﻿using Tweetinvi.Core.Interfaces.oAuth;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ITemporaryCredentials : IConsumerCredentials
    {
        string AuthorizationKey { get; set; }
        string AuthorizationSecret { get; set; }

        string VerifierCode { get; set; }
    }
}