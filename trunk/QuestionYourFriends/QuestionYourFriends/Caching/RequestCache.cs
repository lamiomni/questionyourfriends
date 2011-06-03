using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace QuestionYourFriends.Caching
{
    public static class RequestCache
    {
        private static readonly ICacheManager _cache;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static RequestCache()
        {
            try
            {
                _cache = CacheFactory.GetCacheManager();
                _logger.Info("Cache initialized.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _logger.Error("Cache initialisation failed:", e);
            }
        }

        public static dynamic Get(string key)
        {
            try
            {
                dynamic res = _cache.GetData(key);
                if (res != null)
                {
                    _logger.InfoFormat("Cache get success: {0}", key);
                    return res;
                }
                throw new ApplicationException("GetData returns null.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _logger.Error("Cache get failed: " + key, e);
                return null;
            }
        }

        public static bool Add(string key, object o)
        {
            try
            {
                _cache.Add(key, o, CacheItemPriority.Normal, null, new AbsoluteTime(new TimeSpan(0, 5, 0)));
                _logger.InfoFormat("Cache add success: {0}", key);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _logger.Error("Cache add failed:", e);
                return false;
            }
        }
    }
}