using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Chicken4WP8.Common
{
    public static class Const
    {
        public const string OAUTH_MODE_BASE = "oauth_mode_base";

        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Objects
        };

        #region defalut value
        public const string DEFAULTSOURCE = "web";
        public const string DEFAULTSOURCEURL = "https://github.com/";
        #endregion

        #region rest api
        public const string DEFAULT_VALUE_TRUE = "true";
        public const string DEFAULT_VALUE_FALSE = "false";
        public const string FOLLOWED_BY = "followed_by";
        #endregion

        #region rest api parameters
        public const string ID = "id";
        public const string USER_ID = "user_id";
        public const string USER_SCREEN_NAME = "screen_name";
        public const string COUNT = "count";
        public const string SINCE_ID = "since_id";
        public const string MAX_ID = "max_id";
        public const string INCLUDE_ENTITIES = "include_entities";
        public const string DIRECT_MESSAGE_SKIP_STATUS = "skip_status";
        public const string CURSOR = "cursor";
        public const string STATUS = "status";
        public const string IN_REPLY_TO_STATUS_ID = "in_reply_to_status_id";
        public const string TEXT = "text";
        public const string SKIP_STATUS = "skip_status";
        //update my profile
        public const string USER_NAME = "name";
        public const string URL = "url";
        public const string LOCATION = "location";
        public const string DESCRIPTION = "description";
        //search page
        public const string SEARCH_QUERY = "q";
        public const string PAGE = "page";
        #endregion

        #region const
        public static Regex SourceRegex = new Regex(@".*>(?<url>[\s\S]+?)</a>");
        public static Regex SourceUrlRegex = new Regex(@"<a href=\""(?<link>[^\s>]+)\""");
        public static Regex UserNameRegex = new Regex(@"([^A-Za-z0-9_]|^)@(?<name>(_*[A-Za-z0-9]{1,15}_*)+)(?![A-Za-z0-9_@])");
        public static Regex HashTagRegex = new Regex(@"#(?<hashtag>\w+)(?!(\w+))");
        public const string USERNAMEPATTERN = @"(?<name>{0})(?![A-Za-z0-9_@])";
        public const string HASHTAGPATTERN = @"(?<hashtag>{0})(?!(\w+))";
        public const string SYMBOLPATTERN = @"(?<symbol>{0})(?!(\w+))";
        public const string URLPATTERN = @"(?<text>{0})(?![A-Za-z0-9-_/])";
        #endregion

        #region parse tweet string
        public static string ParseToSource(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return DEFAULTSOURCE;
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
                return DEFAULTSOURCEURL;
            }
            string result = SourceUrlRegex.Match(source).Groups["link"].Value;
            if (string.IsNullOrEmpty(result))
            {
                return DEFAULTSOURCEURL;
            }
            return result;
        }
        #endregion

        public static IDictionary<string, object> GetDictionary()
        {
            return new Dictionary<string, object>();
        }
    }

    public static class Extension
    {
        public static VisualStateGroup GetVisualStateGroup(this FrameworkElement root, string groupName)
        {
            IEnumerable<FrameworkElement> selfOrAncestors = root.GetVisualAncestors().PrependWith(root);

            foreach (FrameworkElement element in selfOrAncestors)
            {
                IList groups = VisualStateManager.GetVisualStateGroups(element);
                foreach (VisualStateGroup group in groups)
                {
                    if (group != null && group.Name == groupName)
                        return group;
                }
            }
            return null;
        }

        public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
        {
            FrameworkElement parent = node.GetVisualParent();
            while (parent != null)
            {
                yield return parent;
                parent = parent.GetVisualParent();
            }
        }

        public static FrameworkElement GetVisualParent(this FrameworkElement node)
        {
            return VisualTreeHelper.GetParent(node) as FrameworkElement;
        }

        public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> list, T head)
        {
            yield return head;
            foreach (T item in list)
                yield return item;
        }

        public static IEnumerable<T> GetVisualDescendents<T>(this FrameworkElement root, bool allAtSameLevel) where T : FrameworkElement
        {
            bool found = false;
            foreach (FrameworkElement e in root.GetVisualDescendents())
            {
                if (e is T)
                {
                    found = true;
                    yield return e as T;
                }
                else
                {
                    if (found == true && allAtSameLevel == true)
                        yield break;
                }
            }
        }

        public static IEnumerable<FrameworkElement> GetVisualDescendents(this FrameworkElement root)
        {
            Queue<IEnumerable<FrameworkElement>> toDo = new Queue<IEnumerable<FrameworkElement>>();

            toDo.Enqueue(root.GetVisualChildren());
            while (toDo.Count > 0)
            {
                IEnumerable<FrameworkElement> children = toDo.Dequeue();
                foreach (FrameworkElement child in children)
                {
                    yield return child;
                    toDo.Enqueue(child.GetVisualChildren());
                }
            }
        }

        public static IEnumerable<FrameworkElement> GetVisualChildren(this FrameworkElement root)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                yield return VisualTreeHelper.GetChild(root, i) as FrameworkElement;
        }
    }
}
