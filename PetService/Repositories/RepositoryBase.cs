using PetClubLib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;

namespace PetService.Repositories
{
    public abstract class RepositoryBase<T> where T : class, IModelMetadata
    {
        #region CRUD
        public abstract long? Insert(T obj);

        public abstract void Update(T obj);

        public abstract T Get(long id);

        public abstract void Delete(long id);
        #endregion // CRUD

        #region Cache
        protected virtual void UpdateCache(T entity, string key)
        {
            if (entity != null)
            {
                entity.Timestamp = DateTime.Now;
                _cache.Set(key, entity, new CacheItemPolicy { AbsoluteExpiration = CacheItemExpiration, UpdateCallback = RemoveCallback }, typeof(T).ToString());
            }
        }

        protected virtual void RemoveFromCache(T entity, string key)
        {
            if (_cache.Contains(key, typeof(T).ToString()))
            {
                _cache.Remove(key, typeof(T).ToString());
            }
        }

        protected virtual T GetCache(string key)
        {
            T item = default(T);

            if (_cache.Contains(key, typeof(T).ToString()))
            {
                item = (T)_cache.Get(key, typeof(T).ToString());
            }

            return item;
        }

        protected virtual IList<T> GetCache()
        {
            //Get List of Cache Items for this Region
            IEnumerable<T> cacheList = _cache.Where(s => s.Key.Contains(typeof(T).ToString()) && !s.Key.Contains("OnUpdateSentinelregion")).Select(s => s.Value).Cast<T>();
            return cacheList.Cast<T>().ToList();
        }

        protected ObjectCache RegionCache { get { return _cache; } }

        protected virtual DateTimeOffset CacheItemExpiration { get { return DateTime.Now.AddDays(1); } }
        #endregion // Cache

        public RepositoryBase(IEntityRepository entities, IDbConnection connection = null)
        {
            this._entities = entities;
            this._connection = connection;
        }

        protected IEntityRepository _entities;
        protected IDbConnection _connection;
        private static ObjectCache _cache = new CustomCache();

        public CacheEntryUpdateCallback RemoveCallback = delegate { };
    }

    public class CustomCache : MemoryCache
    {
        public CustomCache() : base("defaultCustomCache") { }

        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            Set(item.Key, item.Value, policy, item.RegionName);
        }

        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            Set(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration }, regionName);
        }

        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            base.Set(CreateKeyWithRegion(key, regionName), value, policy);
        }

        public override CacheItem GetCacheItem(string key, string regionName = null)
        {
            CacheItem temporary = base.GetCacheItem(CreateKeyWithRegion(key, regionName));
            return new CacheItem(key, temporary.Value, regionName);
        }

        public override object Get(string key, string regionName = null)
        {
            return base.Get(CreateKeyWithRegion(key, regionName));
        }

        public override bool Contains(string key, string regionName = null)
        {
            return base.Contains(CreateKeyWithRegion(key, regionName));
        }

        public override object Remove(string key, string regionName = null)
        {
            return base.Remove(CreateKeyWithRegion(key, regionName));
        }

        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                return (base.DefaultCacheCapabilities | System.Runtime.Caching.DefaultCacheCapabilities.CacheRegions);
            }
        }

        private string CreateKeyWithRegion(string key, string region)
        {
            return "region:" + (region == null ? "null_region" : region) + ";key=" + key;
        }
    }
}