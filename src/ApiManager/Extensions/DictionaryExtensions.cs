using System.Collections.Generic;

namespace ApiManager.Extensions
{
	internal static class DictionaryExtensions
	{
		public static string Get(this IDictionary<string, string> dict, string key, string defaultValue)
		{
			if (dict.ContainsKey(key))
			{
				return dict[key];
			}

			return defaultValue;
		}
	}
}
