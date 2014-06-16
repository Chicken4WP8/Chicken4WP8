using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Credentials.QueryDTO;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Credentials.QueryJsonConverters;

namespace Tweetinvi.Credentials
{
    public class TwitterAccessor : ITwitterAccessor
    {
        private readonly ITwitterRequester _twitterRequester;
        private readonly IJObjectStaticWrapper _wrapper;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly IExceptionHandler _exceptionHandler;

        public TwitterAccessor(
            ITwitterRequester twitterRequester,
            IJObjectStaticWrapper wrapper,
            IJsonObjectConverter jsonObjectConverter,
            IExceptionHandler exceptionHandler)
        {
            _twitterRequester = twitterRequester;
            _wrapper = wrapper;
            _jsonObjectConverter = jsonObjectConverter;
            _exceptionHandler = exceptionHandler;
        }

       // Execute<Json>
        public string ExecuteJsonGETQuery(string query)
        {
            return ExecuteQuery(query, HttpMethod.GET);
        }

        public string ExecuteJsonPOSTQuery(string query)
        {
            return ExecuteQuery(query, HttpMethod.POST);
        }

        // Try Execute<Json>
        public bool TryExecuteJsonGETQuery(string query, out string json)
        {
            try
            {
                json = ExecuteJsonGETQuery(query);
                return json != null;
            }
            catch (WebException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                json = null;
                return false;
            }
        }

        public bool TryExecuteJsonPOSTQuery(string query, out string json)
        {
            try
            {
                json = ExecuteJsonPOSTQuery(query);
                return json != null;
            }
            catch (WebException)
            {
                json = null;
                return false;
            }
        }

        // Execute<JObject>
        public JObject ExecuteGETQuery(string query)
        {
            string jsonResponse = ExecuteQuery(query, HttpMethod.GET);
            return _wrapper.GetJobjectFromJson(jsonResponse);
        }

        public JObject ExecutePOSTQuery(string query)
        {
            string jsonResponse = ExecuteQuery(query, HttpMethod.POST);
            return _wrapper.GetJobjectFromJson(jsonResponse);
        }

        // Execute<T>
        public T ExecuteGETQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = ExecuteQuery(query, HttpMethod.GET);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        public T ExecutePOSTQuery<T>(string query, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = ExecuteQuery(query, HttpMethod.POST);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        public T ExecutePOSTMultipartQuery<T>(string query, IEnumerable<IMedia> medias, JsonConverter[] converters = null) where T : class
        {
            string jsonResponse = ExecuteMultipartQuery(query, HttpMethod.POST, medias);
            return _jsonObjectConverter.DeserializeObject<T>(jsonResponse, converters);
        }

        // Try Execute
        public bool TryExecuteGETQuery(string query, JsonConverter[] converters = null)
        {
            try
            {
                var jObject = ExecuteGETQuery(query);
                return jObject != null;
            }
            catch (WebException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                return false;
            }
        }

        public bool TryExecutePOSTQuery(string query, JsonConverter[] converters = null)
        {
            try
            {
                var jObject = ExecutePOSTQuery(query);
                return jObject != null;
            }
            catch (WebException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                return false;
            }
        }

        // Try Execute<T>
        public bool TryExecuteGETQuery<T>(string query, out T resultObject, JsonConverter[] converters = null)
            where T : class
        {
            try
            {
                resultObject = ExecuteGETQuery<T>(query, converters);
                return resultObject != null;
            }
            catch (WebException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                resultObject = null;
                return false;
            }
        }

        public bool TryExecutePOSTQuery<T>(string query, out T resultObject, JsonConverter[] converters = null)
            where T : class
        {
            try
            {
                resultObject = ExecutePOSTQuery<T>(query, converters);
                return resultObject != null;
            }
            catch (WebException)
            {
                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }

                resultObject = null;
                return false;
            }
        }

        // Cursor Query
        public IEnumerable<string> ExecuteJsonCursorGETQuery<T>(
                string baseQuery,
                int maxObjectToRetrieve = Int32.MaxValue,
                long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            int nbOfObjectsProcessed = 0;
            long previousCursor = -2;
            long nextCursor = cursor;

            // add & for query parameters
            baseQuery = FormatBaseQuery(baseQuery);

            var result = new List<string>();
            while (previousCursor != nextCursor && nbOfObjectsProcessed < maxObjectToRetrieve)
            {
                T cursorResult = ExecuteCursorQuery<T>(baseQuery, cursor, true);
                if (cursorResult == null)
                {
                    return result;
                }

                nbOfObjectsProcessed += cursorResult.GetNumberOfObjectRetrieved();
                previousCursor = cursorResult.PreviousCursor;
                nextCursor = cursorResult.NextCursor;

                result.Add(cursorResult.RawJson);
            }

            return result;
        }

        public IEnumerable<T> ExecuteCursorGETQuery<T>(
                string baseQuery,
                int maxObjectToRetrieve = Int32.MaxValue,
                long cursor = -1)
            where T : class, IBaseCursorQueryDTO
        {
            int nbOfObjectsProcessed = 0;
            long previousCursor = -2;
            long nextCursor = cursor;

            // add & for query parameters
            baseQuery = FormatBaseQuery(baseQuery);

            var result = new List<T>();
            while (previousCursor != nextCursor && nbOfObjectsProcessed < maxObjectToRetrieve)
            {
                T cursorResult = ExecuteCursorQuery<T>(baseQuery, nextCursor, false);

                if (cursorResult == null)
                {
                    return result;
                }

                nbOfObjectsProcessed += cursorResult.GetNumberOfObjectRetrieved();
                previousCursor = cursorResult.PreviousCursor;
                nextCursor = cursorResult.NextCursor;

                result.Add(cursorResult);
            }

            return result;
        }

        private string FormatBaseQuery(string baseQuery)
        {
            if (baseQuery.Contains("?") && baseQuery[baseQuery.Length - 1] != '?')
            {
                baseQuery += "&";
            }

            return baseQuery;
        }

        private T ExecuteCursorQuery<T>(string baseQuery, long cursor, bool storeJson) where T : class, IBaseCursorQueryDTO
        {
            var query = String.Format("{0}cursor={1}", baseQuery, cursor);

            string json;
            if (TryExecuteJsonGETQuery(query, out json))
            {
                var dtoResult = _jsonObjectConverter.DeserializeObject<T>(json, JsonQueryConverterRepository.Converters);

                if (storeJson)
                {
                    dtoResult.RawJson = json;
                }

                return dtoResult;
            }

            return null;
        }

        // Concrete Execute
        public string ExecuteQuery(string query, HttpMethod method)
        {
            if (query == null)
            {
                throw new ArgumentException("At least one of the arguments provided to the query was invalid.");
            }

            try
            {
                return _twitterRequester.ExecuteQuery(query, method);
            }
            catch (WebException)
            {
                if (_exceptionHandler.SwallowWebExceptions)
                {
                    return null;
                }

                throw;
            }
        }

        public string ExecuteMultipartQuery(string query, HttpMethod method, IEnumerable<IMedia> medias)
        {
            if (query == null)
            {
                throw new ArgumentException("At least one of the arguments provided to the query was invalid.");
            }

            try
            {
                return _twitterRequester.ExecuteMultipartQuery(query, method, medias);
            }
            catch (WebException)
            {
                if (_exceptionHandler.SwallowWebExceptions)
                {
                    return null;
                }

                throw;
            }
        }
    }
}