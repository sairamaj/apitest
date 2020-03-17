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
		public IEnumerable<EnvironmentInfo> GetEnvironments()
		{
			var envs = new List<EnvironmentInfo>();
			foreach (var envFolder in
				Directory.GetDirectories(
					Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\Environments")))
			{
				var environment = new EnvironmentInfo(Path.GetFileNameWithoutExtension(envFolder), envFolder);
				environment.CommandFiles = Directory.GetFiles(envFolder, "*.txt").ToList();
				environment.VariableFiles = Directory.GetFiles(envFolder, "*.var").ToList();
				envs.Add(environment);
			}

			return envs;
		}
	}
}
