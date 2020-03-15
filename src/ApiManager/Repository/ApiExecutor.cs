using System;
using System.Diagnostics;
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

		public Task<string> StartAsync(TestData testData)
		{
			var tcs = new TaskCompletionSource<string>();

			var startInfo = new ProcessStartInfo(this._settings.PythonPath, CreateArguments(testData));
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

		private string CreateArguments(TestData testData)
		{
			var command = $"main.py --config {testData.ConfigName} --batch {testData.CommandsTextFileName}";
			if (!string.IsNullOrWhiteSpace(testData.VariablesFileName))
			{
				command += $" --varfile {testData.VariablesFileName}";
			}

			return command;
		}
	}
}
