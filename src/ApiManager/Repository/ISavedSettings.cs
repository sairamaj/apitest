namespace ApiManager.Repository
{
	interface ISavedSettings
	{
		void Add<T>(string key, T val);
		T Get<T>(string key);
	}
}
