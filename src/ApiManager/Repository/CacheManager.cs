using System;
using System.Runtime.Caching;

namespace ApiManager.Repository
{
	class CacheManager : ICacheManager
	{
		MemoryCache _cache = new MemoryCache("apimanager");
		public void Add(string key, string val)
		{
			_cache[key] = val;
		}

		public string Get(string key)
		{
			return _cache.Get(key) as string;
		}
	}
}
