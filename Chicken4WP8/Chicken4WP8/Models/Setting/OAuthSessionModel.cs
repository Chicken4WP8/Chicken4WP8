using System;

namespace Chicken4WP8.Models.Setting
{
    public class OAuthSessionModel
    {
        private Uri authorizeUri;

        public OAuthSessionModel(Uri authorizeUri)
        {
            this.authorizeUri = authorizeUri;
        }

        public Uri AuthorizeUri
        {
            get { return authorizeUri; }
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string RequestToken { get; set; }
        public string RequestTokenSecret { get; set; }
    }
}
