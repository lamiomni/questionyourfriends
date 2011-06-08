using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace QuestionYourFriends.Caching
{
    /// <summary>
    /// Request caching
    /// </summary>
    public static class RequestCache
    {
        private static readonly ICacheManager _cache;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static RequestCache()
        {
            try
            {
                _cache = CacheFactory.GetCacheManager();
                Debug.WriteLine("Cache initialized.");
                _logger.Info("Cache initialized.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _logger.Error("Cache initialisation failed:", e);
            }
        }

        /// <summary>
        /// Get a dynamic data from cache
        /// </summary>
        /// <param name="key">Cache entry</param>
        /// <returns>Data from cache</returns>
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

        /// <summary>
        /// Adds data to the cache
        /// </summary>
        /// <param name="key">Data key entry</param>
        /// <param name="o">Data</param>
        /// <returns>True if the process is ok</returns>
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

        /// <summary>
        /// Clears the cache
        /// </summary>
        /// <returns>True if the process is ok</returns>
        public static bool Flush()
        {
            try
            {
                _cache.Flush();
                _logger.InfoFormat("Cache flush success.");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _logger.Error("Cache flush failed:", e);
                return false;
            }
        }
    }
}