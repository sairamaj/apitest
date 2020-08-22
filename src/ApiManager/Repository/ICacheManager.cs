namespace ApiManager.Repository
{
	interface ICacheManager
	{
		void Add(string key, string val);
		string Get(string key);
	}
}
