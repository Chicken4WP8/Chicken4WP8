using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials.QueryDTO;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface ITwitterAccessorAsync
    {
        // Get Json
        Task<string> ExecuteJsonGETQueryAsync(string query);
        Task<string> ExecuteJsonPOSTQueryAsync(string query);

        // Try Execute<Json>
        Task<bool> TryExecuteJsonGETQueryAsync(string query, out string json);
        Task<bool> TryExecuteJsonPOSTQueryAsync(string query, out string json);

        // Get unknown type of objects
        Task<JObject> ExecuteGETQueryAsync(string query);
        Task<JObject> ExecutePOSTQueryAsync(string query);

        // Get specific type of object
        Task<T> ExecuteGETQueryAsync<T>(string query, JsonConverter[] converters = null) where T : class;
        Task<T> ExecutePOSTQueryAsync<T>(string query, JsonConverter[] converters = null) where T : class;
        Task<T> ExecutePOSTMultipartQueryAsync<T>(string query, IEnumerable<IMedia> medias, JsonConverter[] converters = null) where T : class;

        // Try Execute
        Task<bool> TryExecuteGETQueryAsync(string query, JsonConverter[] converters = null);
        Task<bool> TryExecutePOSTQueryAsync(string query, JsonConverter[] converters = null);

        // Try Get Result
        Task<bool> TryExecuteGETQueryAsync<T>(string query, out T resultObject, JsonConverter[] converters = null) where T : class;
        Task<bool> TryExecutePOSTQueryAsync<T>(string query, out T resultObject, JsonConverter[] converters = null) where T : class;

        // Cursor Query
        Task<IEnumerable<string>> ExecuteJsonCursorGETQueryAsync<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO;

        Task<IEnumerable<T>> ExecuteCursorGETQueryAsync<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO;

        // Get Json from Twitter
        Task<string> ExecuteQueryAsync(string query, HttpMethod method);
        Task<string> ExecuteMultipartQueryAsync(string query, HttpMethod method, IEnumerable<IMedia> medias);
    }

    public interface ITwitterAccessor : ITwitterAccessorAsync
    {
        // Get Json
        string ExecuteJsonGETQuery(string query);
        string ExecuteJsonPOSTQuery(string query);

        // Try Execute<Json>
        bool TryExecuteJsonGETQuery(string query, out string json);
        bool TryExecuteJsonPOSTQuery(string query, out string json);

        // Get unknown type of objects
        JObject ExecuteGETQuery(string query);
        JObject ExecutePOSTQuery(string query);

        // Get specific type of object
        T ExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        T ExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class;
        T ExecutePOSTMultipartQuery<T>(string query, IEnumerable<IMedia> medias, JsonConverter[] converters = null) where T : class;

        // Try Execute
        bool TryExecuteGETQuery(string query, JsonConverter[] converters = null);
        bool TryExecutePOSTQuery(string query, JsonConverter[] converters = null);

        // Try Get Result
        bool TryExecuteGETQuery<T>(string query, out T resultObject, JsonConverter[] converters = null) where T : class;
        bool TryExecutePOSTQuery<T>(string query, out T resultObject, JsonConverter[] converters = null) where T : class;

        // Cursor Query
        IEnumerable<string> ExecuteJsonCursorGETQuery<T>(
            string baseQuery,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO;

        IEnumerable<T> ExecuteCursorGETQuery<T>(
            string query,
            int maxObjectToRetrieve = Int32.MaxValue,
            long cursor = -1)
            where T : class, IBaseCursorQueryDTO;

        // Get Json from Twitter
        string ExecuteQuery(string query, HttpMethod method);
        string ExecuteMultipartQuery(string query, HttpMethod method, IEnumerable<IMedia> medias);
    }
}