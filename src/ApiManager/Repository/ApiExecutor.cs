using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Repository
{
	class ApiExecutor : IApiExecutor
	{
		private Settings _settings;
		private bool _isSettingsValidated;
		public ApiExecutor(Settings settings)
		{
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		public Task<string> StartAsync(TestData testData)
		{
			ValidateSettings(this._settings);
			Validate(testData);
			var tcs = new TaskCompletionSource<string>();

			var startInfo = new ProcessStartInfo(this._settings.ConsoleExecutableName, CreateArguments(testData, this._settings.IsPythonExecutable));
			startInfo.WorkingDirectory = this._settings.WorkingDirectory;
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

		private void Validate(TestData testData)
		{
		}

		private string CreateArguments(TestData testData, bool isPython, bool isBatch = true)
		{
			var command = string.Empty;
			if (isPython)
			{
				command += "main.py ";
			}
			
			command += $"--config {testData.ConfigName}";

			if (isBatch)
			{
				command += $" --batch {testData.CommandsTextFileName}";
			}

			if (!string.IsNullOrWhiteSpace(testData.VariablesFileName))
			{
				command += $" --varfile {testData.VariablesFileName}";
			}

			if (!string.IsNullOrWhiteSpace(testData.SessionName))
			{
				command += $" --session {testData.SessionName}";
			}

			return command;
		}

		private void ValidateSettings(Settings settings)
		{
			if (this._isSettingsValidated)
			{
				return;
			}

			if (!Directory.Exists(settings.WorkingDirectory))
			{
				throw new DirectoryNotFoundException($"{settings.WorkingDirectory} not found. Make sure that path exists.");
			}

			if (!File.Exists(this._settings.ConsoleExecutableName))
			{
				throw new FileNotFoundException($"{this._settings.ConsoleExecutableName} does not exist");
			}

			if (this._settings.IsPythonExecutable)
			{
				// validate python version
				this.ValidatePythonVersion();
			}

			this._isSettingsValidated = true;
		}

		public Task<string> GetCommands(string configFile)
		{
			ValidateSettings(this._settings);
			var tcs = new TaskCompletionSource<string>();

			var command = $"main.py --config {configFile} --batch c:\\temp\\command.txt";
			var startInfo = new ProcessStartInfo(this._settings.ConsoleExecutableName, command);
			startInfo.WorkingDirectory = this._settings.WorkingDirectory;
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

		private void ValidatePythonVersion()
		{
			var startInfo = new ProcessStartInfo(this._settings.ConsoleExecutableName, "--version");
			startInfo.RedirectStandardOutput = true;
			startInfo.UseShellExecute = false;
			var process = new Process()
			{
				StartInfo = startInfo,
				EnableRaisingEvents = true
			};

			var p = Process.Start(startInfo);
			p.WaitForExit();
			var versionMessage = p.StandardOutput.ReadToEnd();
			var parts = versionMessage.Split(' ');
			if (parts.Length < 2)
			{
				throw new ApplicationException($"Python --version did not return properly");
			}

			if (!parts[1].Trim().StartsWith("3.7"))
			{
				throw new ApplicationException($"Found: Pyton Version: {parts[1]} But 3.7.x  is required.");
			}
		}

		public async Task<string> OpenCommandPromptAsync(TestData testData)
		{
			return await this.ExecuteAsync(testData, false);
		}

		private Task<string> ExecuteAsync(TestData testData, bool isBatch)
		{
			ValidateSettings(this._settings);
			Validate(testData);
			var tcs = new TaskCompletionSource<string>();

			var startInfo = new ProcessStartInfo(this._settings.ConsoleExecutableName, CreateArguments(testData, this._settings.IsPythonExecutable, isBatch));
			startInfo.WorkingDirectory = this._settings.WorkingDirectory;
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

	}
}