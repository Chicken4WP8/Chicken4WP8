using Newtonsoft.Json;

namespace Chicken4WP8.Common
{
    public class Const
    {
        public const string OAUTH_MODE_BASE = "oauth_mode_base";

        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        };
    }
}
