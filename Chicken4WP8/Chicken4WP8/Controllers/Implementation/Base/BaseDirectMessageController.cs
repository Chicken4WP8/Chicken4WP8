using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chicken4WP8.Controllers.Interface;

namespace Chicken4WP8.Controllers.Implementation.Base
{
    public class BaseDirectMessageController : BaseControllerBase, IDirectMessageController
    {
        public async Task<IEnumerable<IDirectMessageModel>> SentAsync(IDictionary<string, object> parameters)
        {
            var messages = await tokens.DirectMessages.SentAsync(parameters);
            var list = new List<DirectMessageModel>();
            if (messages != null)
                foreach (var message in messages)
                    list.Add(new DirectMessageModel(message));
            return list;
        }

        public async Task<IEnumerable<IDirectMessageModel>> ReceivedAsync(IDictionary<string, object> parameters)
        {
            var messages = await tokens.DirectMessages.ReceivedAsync(parameters);
            var list = new List<DirectMessageModel>();
            if (messages != null)
                foreach (var message in messages)
                    list.Add(new DirectMessageModel(message));
            return list;
        }

        public Task<IDirectMessageModel> NewAsync(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IDirectMessageModel> DestroyAsync(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
