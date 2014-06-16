using Newtonsoft.Json.Linq;

namespace Tweetinvi.Core.Wrappers
{
    public interface IJObjectStaticWrapper
    {
        JObject GetJobjectFromJson(string json);
        T ToObject<T>(JToken jObject);
        string GetNodeRootName(JToken jToken);
    }
}