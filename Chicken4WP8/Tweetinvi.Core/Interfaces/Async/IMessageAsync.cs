﻿using System.Threading.Tasks;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface IMessageAsync
    {
        Task<bool> PublishAsync();
        Task<bool> PublishToAsync(IUser recipient);
        Task<bool> DestroyAsync();
    }
}
