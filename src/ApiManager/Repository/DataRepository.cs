﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiManager.Model;
using ApiManager.Pipes;
using Newtonsoft.Json;
using ApiEnvironment = ApiManager.Model.Environment;

namespace ApiManager.Repository
{
	class DataRepository : IDataRepository
	{
		private ICommandExecutor _executor;
		private ISettings _settings;
		private IDictionary<string, ApiCommandInfo> _apiCommands = new Dictionary<string, ApiCommandInfo>();
		private IDictionary<string, IEnumerable<string>> _apiVariables = new Dictionary<string, IEnumerable<string>>();
		private BangCommandInfo _bangCommands;
		private FunctionCommandInfo _functionCommandInfo;
		private DynamicVariableInfo _dynamicVariableInfo;

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

			using (var communicator = new ApiTestConsoleCommunicator(new MessageListener()))
			{
				communicator.Add("management", "apicommands", data =>
				{
					var apiCommandsInfo = JsonConvert.DeserializeObject<ApiCommandInfo>(data);
					_apiCommands[apiCommandsInfo.Session] = apiCommandsInfo;
				});
				await this._executor.GetApiCommands(info).ConfigureAwait(false);
			}
				
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
				api.Scenarios = GetScenarios(api);

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

		public async Task<BangCommandInfo> GetBangCommands()
		{
			if (_bangCommands != null)
			{

				return this._bangCommands;
			}

			using (var communicator = new ApiTestConsoleCommunicator(new MessageListener()))
			{
				communicator.Add("management", "bangcommands", data =>
				{
					_bangCommands = JsonConvert.DeserializeObject<BangCommandInfo>(data);
				});
				await this._executor.GetBangCommands().ConfigureAwait(false);
			}

			if (_bangCommands != null)
			{
				return this._bangCommands;
			}

			return new BangCommandInfo();
		}

		public async Task<FunctionCommandInfo> GetFunctionCommandInfo()
		{
			if (_functionCommandInfo != null)
			{
				return this._functionCommandInfo;
			}

			using (var communicator = new ApiTestConsoleCommunicator(new MessageListener()))
			{
				communicator.Add("management", "functions", data =>
				{
					try
					{
						_functionCommandInfo = JsonConvert.DeserializeObject<FunctionCommandInfo>(data);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				});
				await this._executor.GetFunctionCommands().ConfigureAwait(false);
			}

			if (_functionCommandInfo != null)
			{
				return this._functionCommandInfo;
			}

			return new FunctionCommandInfo();
		}

		public async Task<DynamicVariableInfo> GetDynamicVariableInfo()
		{
			if (_dynamicVariableInfo != null)
			{
				return this._dynamicVariableInfo;
			}

			using (var communicator = new ApiTestConsoleCommunicator(new MessageListener()))
			{
				communicator.Add("management", "dynamicvariables", data =>
				{
					try
					{
						_dynamicVariableInfo = JsonConvert.DeserializeObject<DynamicVariableInfo>(data);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				});
				await this._executor.GetDynamicVariables().ConfigureAwait(false);
			}

			if (_dynamicVariableInfo != null)
			{
				return this._dynamicVariableInfo;
			}

			return new DynamicVariableInfo();
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
		}

		public IEnumerable<Scenario> GetScenarios(ApiInfo apiInfo)
		{
			var scenariosPath = Path.Combine(apiInfo.Path, "scenarios");
			if (Directory.Exists(scenariosPath))
			{
				return GetScenarios(scenariosPath);
			}

			return new List<Scenario>();
		}

		private IEnumerable<Scenario> GetScenarios(string path)
		{
			var scenarios = new List<Scenario>();
			foreach (var subDir in Directory.GetDirectories(path)
			  .Where(d => !Path.GetFileName(d).StartsWith("_", StringComparison.OrdinalIgnoreCase)))
			{
				var containerScenario = new Scenario(subDir, true);
				AddScenarios(containerScenario, subDir);
				scenarios.Add(containerScenario);
			}

			return scenarios;
		}

		private void AddScenarios(Scenario container, string path)
		{
			var scenarios = Directory.GetFiles(path, "*.txt")
				.Where(s => !Path.GetFileName(s).StartsWith("_", StringComparison.OrdinalIgnoreCase))
			  .Select(f => new Scenario(f)).ToList();
			scenarios.ToList().ForEach(s => container.AddScenario(s));

			foreach (var subDir in Directory.GetDirectories(path))
			{
				var subContainer = new Scenario(subDir, true);
				AddScenarios(subContainer, subDir);
				container.AddScenario(subContainer);
			}
		}
	}
}
