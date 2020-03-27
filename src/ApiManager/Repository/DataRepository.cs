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
		IApiExecutor _executor;
		public DataRepository(IApiExecutor executor)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
		}
		public IEnumerable<string> GetCommands(EnvironmentInfo info)
		{
			this._executor.GetCommands(info.Configuration);
			return null;
		}

		public IEnumerable<EnvironmentInfo> GetEnvironments()
		{
			var envs = new List<EnvironmentInfo>();
			foreach (var envFolder in
				Directory.GetDirectories(
					Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\Environments")))
			{
				var environment = new EnvironmentInfo(Path.GetFileNameWithoutExtension(envFolder), envFolder);
				environment.Scenarios = Directory.GetFiles(envFolder, "*.txt")
					.Select(f => new Scenario(Path.GetFileNameWithoutExtension(f), f)).ToList();
				environment.VariableFiles = Directory.GetFiles(envFolder, "*.var").ToList();
				environment.Configuration = Path.Combine(envFolder, "config.json");
				envs.Add(environment);
			}

			return envs;
		}


	}
}
