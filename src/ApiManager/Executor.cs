using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApiManager.Model;
using ApiManager.Repository;
using Environment = ApiManager.Model.Environment;

namespace ApiManager
{
	internal class Executor
	{
		private readonly ICommandExecutor _executor;
		private readonly ISettings _settings;

		public Executor(ICommandExecutor executor, ISettings settings)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor) );
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		public async Task RunScenarioAsync(ApiInfo api, Environment environment, Scenario scenario)
		{
			var variableFileName = CreateVariableFile(api, environment);
			await _executor.StartAsync(
				new TestData
				{
					ConfigName = api.Configuration,
					CommandsTextFileName = scenario.FileName,
					VariablesFileName = variableFileName,
					SessionName = $"{api.Name}|{scenario.Name}",
				}).ConfigureAwait(false);
		}

		private string CreateVariableFile(ApiInfo api, Environment environment)
		{
			var variables = new List<string>();
			
			// Get variables.var at root configuration.
			var systemLevelVariablesFileName = Path.Combine(this._settings.ConfigurationPath, "variables.var");
			if (File.Exists(systemLevelVariablesFileName))
			{
				variables.Add($"## Variables from:{systemLevelVariablesFileName}\r\n");
				variables.AddRange(File.ReadAllLines(systemLevelVariablesFileName));
			}

			var apiLevelVariablesFileName = Path.Combine(api.Path, "variables.var");
			if (File.Exists(apiLevelVariablesFileName))
			{
				variables.Add($"## Variables from:{apiLevelVariablesFileName}\r\n");
				variables.AddRange(File.ReadAllLines(apiLevelVariablesFileName));
			}

			variables.AddRange(File.ReadAllLines(environment.FileName));
			return FileHelper.WriteToTempFile(variables.ToArray(), ".var");
		}
	}
}