using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Chicken4WP8.Common;
using CoreTweet;

namespace Chicken4WP8.Controllers.Implemention.Base
{
    public class Utils
    {
        #region parse entities
        public static IEnumerable<IEntity> ParseUserMentions(string text, IList<IUserMentionEntity> mentions)
        {
            foreach (var mention in mentions.GroupBy(m => m.Id).Select(g => g.First()))
            {
                var matches = Regex.Matches(text, string.Format(Const.USERNAMEPATTERN, Regex.Escape(mention.DisplayText)), RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    var entity = new UserMentionEntity
                    {
                        Id = mention.Id,
                        Name = mention.Name,
                        ScreenName = mention.ScreenName,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new UserMentionEntityModel(entity);
                    yield return model;
                }
            }
        }

        public static IEnumerable<IEntity> ParseHashTags(string text, IList<ISymbolEntity> hashtags)
        {
            foreach (var hashtag in hashtags.GroupBy(h => h.Text).Select(g => g.First()))
            {
                var matches = Regex.Matches(text, string.Format(Const.HASHTAGPATTERN, Regex.Escape(hashtag.DisplayText)));
                foreach (Match match in matches)
                {
                    var entity = new SymbolEntity
                    {
                        Text = hashtag.Text,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new HashTagEntityModel(entity);
                    yield return model;
                }
            }
        }

        public static IEnumerable<IEntity> ParseSymbols(string text, IList<ISymbolEntity> symbols)
        {
            foreach (var symbol in symbols.GroupBy(s => s.Text).Select(g => g.First()))
            {
                var matches = Regex.Matches(text, string.Format(Const.HASHTAGPATTERN, Regex.Escape(symbol.DisplayText)));
                foreach (Match match in matches)
                {
                    var entity = new SymbolEntity
                    {
                        Text = symbol.Text,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new SymbolEntityModel(entity);
                    yield return model;
                }
            }
        }

        public static IEnumerable<IEntity> ParseUrls(string text, IList<IUrlEntity> urls)
        {
            foreach (var url in urls.GroupBy(u => u.Url.AbsoluteUri).Select(g => g.First()))
            {
                var matches = Regex.Matches(text, string.Format(Const.URLPATTERN, Regex.Escape(url.Url.AbsoluteUri)));
                foreach (Match match in matches)
                {
                    var entity = new UrlEntity
                    {
                        DisplayUrl = url.DisplayUrl,
                        ExpandedUrl = url.ExpandedUrl,
                        Url = url.Url,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new UrlEntityModel(entity);
                    yield return model;
                }
            }
        }

        public static IEnumerable<IEntity> ParseMedias(string text, IList<IMediaEntity> medias)
        {
            foreach (var media in medias)
            {
                var matches = Regex.Matches(text, string.Format(Const.URLPATTERN, Regex.Escape(media.Url.AbsoluteUri)));
                foreach (Match match in matches)
                {
                    var entity = new MediaEntity
                    {
                        Id = media.Id,
                        MediaUrl = media.MediaUrl,
                        MediaUrlHttps = media.MediaUrlHttps,
                        SourceStatusId = media.SourceStatusId,
                        Type = media.Type,
                        DisplayUrl = media.DisplayUrl,
                        ExpandedUrl = media.ExpandedUrl,
                        Url = media.Url,
                        Indices = new int[] { match.Index, 0 }
                    };
                    var model = new MediaEntityModel(entity);
                    yield return model;
                }
            }
        }
        #endregion

        #region parse entities in profile description
        public static IEnumerable<IEntity> ParseUserMentions(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;
            var matches = Const.UserNameRegex.Matches(text);
            foreach (Match match in matches)
            {
                var entity = new UserMentionEntity
                {
                    ScreenName = match.Groups["name"].Value,
                    Indices = new int[] { match.Groups["name"].Index - 1, 0 }//remove @
                };
                yield return new UserMentionEntityModel(entity);
            }
        }

        public static IEnumerable<IEntity> ParseHashTags(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;
            var matches = Const.HashTagRegex.Matches(text);
            foreach (Match match in matches)
            {
                var entity = new SymbolEntity
                {
                    Text = match.Groups["hashtag"].Value,
                    Indices = new int[] { match.Index, 0 }
                };
                yield return new HashTagEntityModel(entity);
            }
        }
        #endregion
    }
}