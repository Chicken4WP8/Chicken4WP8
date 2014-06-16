using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core;

namespace Tweetinvi.Controllers.Shared
{
    public interface IQueryParameterGenerator
    {
        string GenerateCountParameter(int count);
        string GenerateTrimUserParameter(bool trimUser);
        string GenerateSinceIdParameter(long sinceId);
        string GenerateMaxIdParameter(long maxId);
        string GenerateIncludeEntitiesParameter(bool includeEntities);
    }

    public class QueryParameterGenerator : IQueryParameterGenerator
    {
        public string GenerateCountParameter(int count)
        {
            if (count == -1)
            {
                return String.Empty;
            }

            return String.Format(Resources.QueryParameter_Count, count);
        }

        public string GenerateTrimUserParameter(bool trimUser)
        {
            return String.Format(Resources.QueryParameter_TrimUser, trimUser);
        }

        public string GenerateSinceIdParameter(long sinceId)
        {
            if (sinceId == TweetinviConfig.DEFAULT_ID)
            {
                return String.Empty;
            }

            return String.Format(Resources.QueryParameter_SinceId, sinceId);
        }

        public string GenerateMaxIdParameter(long maxId)
        {
            if (maxId == TweetinviConfig.DEFAULT_ID)
            {
                return String.Empty;
            }

            return String.Format(Resources.QueryParameter_MaxId, maxId);
        }

        public string GenerateIncludeEntitiesParameter(bool includeEntities)
        {
            return String.Format(Resources.QueryParameter_IncludeEntities, includeEntities);
        }
    }
}