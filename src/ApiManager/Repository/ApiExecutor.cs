using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Repository
{
	class ApiExecutor : IApiExecutor
	{
		private Settings _settings;
		public ApiExecutor(Settings settings)
		{
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		public Task<string> StartAsync(EnvironmentInfo environment, string commandsFileName)
		{
			var tcs = new TaskCompletionSource<string>();

			var startInfo = new ProcessStartInfo(this._settings.PythonPath, CreateArguments(environment, commandsFileName));
			startInfo.WorkingDirectory = this._settings.ApiTestPath;
			var process = new Process()
			{
				StartInfo = startInfo,
				EnableRaisingEvents = true
			};

			process.Start();
			process.Exited += (s, e) =>
			{
				// var output = process.StandardOutput.ReadToEnd();
				var output = process.ExitCode.ToString();
				tcs.SetResult(output);
			};

			return tcs.Task;
		}

		private string CreateArguments(EnvironmentInfo environment, string commandsFileName)
		{
			return $"main.py --config {environment.ConfigName} --batch {commandsFileName}";
		}
	}
}
