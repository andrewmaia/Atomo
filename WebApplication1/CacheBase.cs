using System;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace WebApplication1_Teste
{
    public class CacheBase
    {
        // ManagerCache of EnterpriseLibrary to contain the cache.
        static ICacheManager cacheManager;

        public CacheBase()
        {

            if (cacheManager == null)
            {
                DictionaryConfigurationSource internalConfigurationSource = new DictionaryConfigurationSource();
                CacheManagerSettings settings = new CacheManagerSettings();
                internalConfigurationSource.Add(CacheManagerSettings.SectionName, settings);
                CacheStorageData storageConfig = new CacheStorageData("Null Storage", typeof(NullBackingStore));
                settings.BackingStores.Add(storageConfig);
                CacheManagerData cacheManagerConfig = new CacheManagerData("CustomCache", 60, 1000, 10, storageConfig.Name);
                settings.CacheManagers.Add(cacheManagerConfig);
                settings.DefaultCacheManager = cacheManagerConfig.Name;
                CacheManagerFactory cacheFactory = new CacheManagerFactory(internalConfigurationSource);
                cacheManager = cacheFactory.CreateDefault();

            }
        }

        // Returns the number of items in the cache.
        public int Count
        {
            get
            {
                return cacheManager.Count;
            }
        }

        //Cache Generic
        public GenericType Add<GenericType>(object Key, GenericType c, params  ICacheItemExpiration[] expirations)
        {
            if (Key != null)
            {
                cacheManager.Add(getKey(Key), c, CacheItemPriority.Normal, null, expirations);
            }

            return c;
        }

        public GenericType Add<GenericType>(string Key, GenericType c, params  ICacheItemExpiration[] expirations)
        {
            if (Key != null)
            {
                cacheManager.Add(Key, c, CacheItemPriority.Normal, null, expirations);
            }

            return c;
        }

        //Cache SlidingTime
        public GenericType Add<GenericType>(object Key, GenericType c, TimeSpan refreshTime)
        {
            if (Key != null)
            {

                SlidingTime expireTime = new SlidingTime(refreshTime);
                cacheManager.Add(getKey(Key), c, CacheItemPriority.Normal, null, expireTime);
            }

            return c;
        }

        public void Remove(object Key)
        {
            cacheManager.Remove(getKey(Key));
        }

        public void Flush()
        {
            cacheManager.Flush();
        }

        // Accesses a data object from the cache.
        public object this[object Key]
        {
            get
            {
                if (Key == null)
                    return null;

                return cacheManager.GetData(getKey(Key));
            }
        }

        public object this[string Key]
        {
            get
            {
                return cacheManager.GetData(Key);
            }
        }

        private string getKey(object input)
        {
            string key = "";

            foreach (PropertyInfo property in input.GetType().GetProperties())
                key += String.Format("{0}_{1}={2}", input.GetType().Name, property.Name, property.GetValue(input, null));

            return key;

        }
    }
}