using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tweetinvi.Core.Interfaces.Models.Entities
{
    public interface IWebsiteEntity
    {
        IEnumerable<IUrlEntity> Urls { get; set; }
    }
}