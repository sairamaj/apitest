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
		ICommandExecutor _executor;
		IDictionary<string, IEnumerable<string>> _apiCommands = new Dictionary<string, IEnumerable<string>>();
		public DataRepository(ICommandExecutor executor)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
		}
		public async Task<IEnumerable<string>> GetCommands(ApiInfo info)
		{
			if (this._apiCommands.TryGetValue(info.Name, out var commands))
			{
				return await Task.FromResult<IEnumerable<string>>(commands).ConfigureAwait(false);
			}
			
			await this._executor.GetApiCommands(info).ConfigureAwait(false);
			if (this._apiCommands.TryGetValue(info.Name, out var commands1))
			{
				return await Task.FromResult<IEnumerable<string>>(commands1).ConfigureAwait(false);
			}

			return new List<string>();
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

		public void AddManagementInfo(Info info)
		{
			if (info is ManagementCommandInfo)
			{
				var commandInfo = info as ManagementCommandInfo;
				_apiCommands[commandInfo.Session] = commandInfo.Commands.ToList();
			}
		}
	}
}
