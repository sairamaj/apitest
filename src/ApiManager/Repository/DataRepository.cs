using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiManager.Model;
using ApiEnvironment = ApiManager.Model.Environment;

namespace ApiManager.Repository
{
	class DataRepository : IDataRepository
	{
		ICommandExecutor _executor;
		IDictionary<string, ApiCommandInfo> _apiCommands = new Dictionary<string, ApiCommandInfo>();
		IDictionary<string, IEnumerable<string>> _apiVariables = new Dictionary<string, IEnumerable<string>>();
		public DataRepository(ICommandExecutor executor)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
		}
		public async Task<ApiCommandInfo> GetCommands(ApiInfo info)
		{
			if (this._apiCommands.TryGetValue(info.Name, out var commands))
			{
				return await Task.FromResult<ApiCommandInfo>(commands).ConfigureAwait(false);
			}
			
			await this._executor.GetApiCommands(info).ConfigureAwait(false);
			
			// Hack. We are waiting a little bit the pipe line response to be parsed and added to this dictionary.
			// Better way is to coordinate between this and pipe line responser.
			await Task.Delay(100).ConfigureAwait(false);
			if (this._apiCommands.TryGetValue(info.Name, out var commands1))
			{
				return await Task.FromResult<ApiCommandInfo>(commands1).ConfigureAwait(false);
			}

			return new ApiCommandInfo();
		}


		public async Task<IEnumerable<string>> GetVariables(ApiInfo info)
		{
			if (this._apiVariables.TryGetValue(info.Name, out var variables))
			{
				return await Task.FromResult<IEnumerable<string>>(variables).ConfigureAwait(false);
			}

			await this._executor.GetApiVariables(info).ConfigureAwait(false);
			if (this._apiVariables.TryGetValue(info.Name, out var variables1))
			{
				return await Task.FromResult<IEnumerable<string>>(variables1).ConfigureAwait(false);
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
			if (info is ApiCommandInfo)
			{
				var commandInfo = info as ApiCommandInfo;
				_apiCommands[commandInfo.Session] = commandInfo;
			}
			if (info is ManagementVariableInfo)
			{
				var variableInfo = info as ManagementVariableInfo;
				this._apiVariables[variableInfo.Session] = variableInfo.Variables.ToList();
			}
		}
	}
}
