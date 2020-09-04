using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiManager.Repository
{
	class SavedSettings : ISavedSettings
	{
		string _path;
		public SavedSettings()
		{
			this._path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings");
			EnsurePath();
		}

		public void Add<T>(string key, T val)
		{
			var data = JsonConvert.SerializeObject(val, Formatting.Indented);
			File.WriteAllText(Path.Combine(this._path, $"{key}.json"), data);
		}

		public T Get<T>(string key)
		{
			var file = Path.Combine(this._path, $"{key}.json");
			if (!File.Exists(file))
			{
				return default(T);
			}

			try
			{
				return (T)JsonConvert.DeserializeObject(File.ReadAllText(file), typeof(T));
			}
			catch (Exception)
			{
				return default(T);
			}
		}

		private void EnsurePath()
		{
			if (Directory.Exists(this._path))
			{
				return;
			}

			Directory.CreateDirectory(this._path);
		}
	}
}
