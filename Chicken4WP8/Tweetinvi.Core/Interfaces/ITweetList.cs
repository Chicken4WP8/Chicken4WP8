using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Interfaces
{
    public interface ITweetList : ITweetListAsync
    {
        ITweetListDTO TweetListDTO { get; set; }

        long Id { get; }
        string IdStr { get; }

        string Slug { get; }
        string Name { get; }
        string FullName { get; }

        IUser Creator { get; }
        DateTime CreatedAt { get; }
        string Uri { get; }
        string Description { get; }
        bool Following { get; }
        PrivacyMode PrivacyMode { get; }

        int MemberCount { get; }
        int SubscriberCount { get; }

        bool Update(IListUpdateParameters parameters);
        bool Destroy();
        IEnumerable<ITweet> GetTweets();
        IEnumerable<IUser> GetMembers(int maxNumberOfUsersToRetrieve = 100);
    }
}