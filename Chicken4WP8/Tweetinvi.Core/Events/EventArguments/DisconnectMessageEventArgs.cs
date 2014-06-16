using System;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class DisconnectMessageEventArgs : EventArgs
    {
        public DisconnectMessageEventArgs(IDisconnectMessage disconnectMessage)
        {
            DisconnectMessage = disconnectMessage;
        }

        public IDisconnectMessage DisconnectMessage { get; private set; }
    }
}