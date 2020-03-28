using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Model;
using System.IO;
using Newtonsoft.Json;
using ApiEnvironment = ApiManager.Model.Environment;

namespace ApiManager.Repository
{
	class DataRepository : IDataRepository
	{
		IApiExecutor _executor;
		public DataRepository(IApiExecutor executor)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
		}
		public IEnumerable<string> GetCommands(ApiInfo info)
		{
			this._executor.GetCommands(info.Configuration);
			return null;
		}

		public IEnumerable<ApiInfo> GetApiConfigurations()
		{
			var apis = new List<ApiInfo>();
			foreach (var envFolder in
				Directory.GetDirectories(
					Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\Apis")))
			{
				var api = new ApiInfo(Path.GetFileNameWithoutExtension(envFolder), envFolder);
				var scenariosPath = Path.Combine(envFolder, "scenarios");
				if (Directory.Exists(scenariosPath))
				{
					api.Scenarios = Directory.GetFiles(scenariosPath, "*.txt")
					  .Select(f => new Scenario(Path.GetFileNameWithoutExtension(f), f)).ToList();
				}

				var environmentsPath = Path.Combine(envFolder, "environments");
				if (Directory.Exists(environmentsPath))
				{
					api.Environments = Directory.GetFiles(environmentsPath, "*.var")
					  .Select(f => new ApiEnvironment(Path.GetFileNameWithoutExtension(f), f)).ToList();
				}

				api.Configuration = Path.Combine(envFolder, "config.json");
				apis.Add(api);
			}

			return apis;
		}
	}
}
