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
		private ICommandExecutor _executor;
		private ISettings _settings;
		private IDictionary<string, ApiCommandInfo> _apiCommands = new Dictionary<string, ApiCommandInfo>();
		private IDictionary<string, IEnumerable<string>> _apiVariables = new Dictionary<string, IEnumerable<string>>();
		private IEnumerable<HelpCommand> _helpCommands;

		public DataRepository(ICommandExecutor executor, ISettings settings)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
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
					Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._settings.ConfigurationPath)))
			{
				var api = new ApiInfo(Path.GetFileNameWithoutExtension(envFolder), envFolder);
				var scenariosPath = Path.Combine(envFolder, "scenarios");
				if (Directory.Exists(scenariosPath))
				{
					api.Scenarios = Directory.GetFiles(scenariosPath, "*.txt")
						.Where(s => !Path.GetFileName(s).StartsWith("_", StringComparison.OrdinalIgnoreCase))
					  .Select(f => new Scenario(Path.GetFileNameWithoutExtension(f), f)).ToList();
					var scenarioContainers = new List<Scenario>();
					foreach (var subDir in Directory.GetDirectories(scenariosPath)
						.Where(d=> !Path.GetFileName(d).StartsWith("_", StringComparison.OrdinalIgnoreCase)))
					{
						var subScenarios = Directory.GetFiles(subDir, "*.txt")
							.Where(s => !Path.GetFileName(s).StartsWith("_", StringComparison.OrdinalIgnoreCase))
						  .Select(f => new Scenario(Path.GetFileNameWithoutExtension(f), f)).ToList();
						var containerScenario = new Scenario(Path.GetFileNameWithoutExtension(subDir), subDir, true);
						subScenarios.ToList().ForEach(s => containerScenario.AddScenario(s));
						scenarioContainers.Add(containerScenario);
					}
					api.Scenarios = api.Scenarios.Union(scenarioContainers).ToList();
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

		public async Task<IEnumerable<HelpCommand>> GetHelpCommands()
		{
			if (_helpCommands != null)
			{
				return this._helpCommands;
			}

			await this._executor.GetHelpCommands().ConfigureAwait(false);

			// Hack. We are waiting a little bit the pipe line response to be parsed and added to this dictionary.
			// Better way is to coordinate between this and pipe line responser.
			await Task.Delay(100).ConfigureAwait(false);
			if (_helpCommands != null)
			{
				return this._helpCommands;
			}

			return new List<HelpCommand>();
		}

		public void AddManagementInfo(Info info)
		{
			if (info is ApiCommandInfo)
			{
				var commandInfo = info as ApiCommandInfo;
				_apiCommands[commandInfo.Session] = commandInfo;
			}
			else if (info is ManagementVariableInfo)
			{
				var variableInfo = info as ManagementVariableInfo;
				this._apiVariables[variableInfo.Session] = variableInfo.Variables.ToList();
			}
			else if (info is HelpCommandInfo)
			{
				this._helpCommands = (info as HelpCommandInfo).Commands.ToList();
			}
		}
	}
}
