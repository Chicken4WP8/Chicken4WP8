﻿using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models.Entities
{
    public interface IUserEntities
    {
        IWebsiteEntity Website { get; set; }
        IDescriptionEntity Description { get; set; }
    }
}