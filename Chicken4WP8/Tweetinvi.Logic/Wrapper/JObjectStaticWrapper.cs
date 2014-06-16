using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.Wrapper
{
    // Wrapper classes "cannot" be tested
    public class JObjectStaticWrapper : IJObjectStaticWrapper
    {
        private readonly JsonSerializer _serializer;

        public JObjectStaticWrapper()
        {
            _serializer = new JsonSerializer();
            
            foreach (var converter in JsonPropertiesConverterRepository.Converters)
            {
                _serializer.Converters.Add(converter);
            }
        }

        public JObject GetJobjectFromJson(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return null;
            }

            return JObject.Parse(json);
        }

        public T ToObject<T>(JToken jToken)
        {
            return jToken.ToObject<T>(_serializer);
        }

        public string GetNodeRootName(JToken jToken)
        {
            var jProperty = jToken as JProperty;
            return jProperty != null ? jProperty.Name : null;
        }
    }
}