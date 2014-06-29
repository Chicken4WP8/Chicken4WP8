using System.Text.RegularExpressions;

namespace Chicken4WP8.Common
{
    public static class TwitterHelper
    {
        #region const
        private static Regex SourceRegex = new Regex(@".*>(?<url>[\s\S]+?)</a>");
        private static Regex SourceUrlRegex = new Regex(@"<a href=\""(?<link>[^\s>]+)\""");
        private static Regex UserNameRegex = new Regex(@"([^A-Za-z0-9_]|^)@(?<name>(_*[A-Za-z0-9]{1,15}_*)+)(?![A-Za-z0-9_@])");
        private static Regex HashTagRegex = new Regex(@"#(?<hashtag>\w+)(?!(\w+))");
        private const string USERNAMEPATTERN = @"(?<name>{0})(?![A-Za-z0-9_@])";
        private const string HASHTAGPATTERN = @"(?<hashtag>{0})(?!(\w+))";
        private const string URLPATTERN = @"(?<text>{0})(?![A-Za-z0-9-_/])";
        #endregion

        public static string ParseToSource(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return Const.DEFAULTSOURCE;
            }
            string result = SourceRegex.Match(source).Groups["url"].Value;
            if (string.IsNullOrEmpty(result))
            {
                return source;
            }
            return result;
        }

        public static string ParseToSourceUrl(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return Const.DEFAULTSOURCEURL;
            }
            string result = SourceUrlRegex.Match(source).Groups["link"].Value;
            if (string.IsNullOrEmpty(result))
            {
                return Const.DEFAULTSOURCEURL;
            }
            return result;
        }
    }
}
