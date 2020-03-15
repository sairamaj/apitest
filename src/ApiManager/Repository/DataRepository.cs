using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Model;
using System.IO;
using Newtonsoft.Json;

namespace ApiManager.Repository
{
	class DataRepository : IDataRepository
	{
		public IDictionary<string, IEnumerable<EnvironmentInfo>> GetEnvironments()
		{
			var envs = new Dictionary<string, IEnumerable<EnvironmentInfo>>();
			foreach (var envFolder in
				Directory.GetDirectories(
					Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\Environments")))
			{
				var environments = new List<EnvironmentInfo>(); ;
				foreach (var env in Directory.GetFiles(envFolder, "*.json"))
				{
					environments.Add(JsonConvert.DeserializeObject<EnvironmentInfo>(File.ReadAllText(env)));
				}
				envs[Path.GetFileNameWithoutExtension(envFolder)] = environments;
			}

			return envs;
		}
	}
}
