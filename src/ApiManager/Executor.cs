using System;
using System.Threading.Tasks;
using ApiManager.Model;
using ApiManager.Repository;
using Environment = ApiManager.Model.Environment;

namespace ApiManager
{
	internal class Executor
	{
		private readonly ICommandExecutor _executor;

		public Executor(ICommandExecutor executor)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor) );
		}

		public async Task RunScenarioAsync(ApiInfo api, Environment environment, Scenario scenario)
		{
			await _executor.StartAsync(
				new TestData
				{
					ConfigName = api.Configuration,
					CommandsTextFileName = scenario.FileName,
					VariablesFileName = environment.FileName,
					SessionName = api.Name,
				}).ConfigureAwait(false);
		}
	}
}
