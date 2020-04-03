using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Repository
{
	class CommandExecutor : ICommandExecutor
	{
		private ISettings _settings;
		private bool _isSettingsValidated;
		public CommandExecutor(ISettings settings)
		{
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		public Task<string> StartAsync(TestData testData)
		{
			ValidateSettings(this._settings);
			Validate(testData);
			var tcs = new TaskCompletionSource<string>();

			CheckForConsoleRunningAlready();
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

		private void ValidateSettings(ISettings settings)
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

		public async Task<string> GetApiCommands(ApiInfo info)
		{
			ValidateSettings(this._settings);
			var args = new CommandFormatter(this._settings).GetCommandArguments(new CommandInfo
			{
				ConfigFileName = info.Configuration,
				SessionName = info.Name,
				Commands = new string[] { "!management apicommands" }
			});

			var ret = await StartProcess(this._settings.ConsoleExecutableName, args).ConfigureAwait(false);

			return ret.ToString(CultureInfo.InvariantCulture);
		}

		public async Task<string> GetApiVariables(ApiInfo info)
		{
			ValidateSettings(this._settings);
			var args = new CommandFormatter(this._settings).GetCommandArguments(new CommandInfo
			{
				ConfigFileName = info.Configuration,
				SessionName = info.Name,
				Commands = new string[] { "!management variables" }
			});

			var ret = await StartProcess(this._settings.ConsoleExecutableName, args).ConfigureAwait(false);

			return ret.ToString(CultureInfo.InvariantCulture);
		}

		public async Task<string> GetHelpCommands()
		{
			ValidateSettings(this._settings);
			var args = new CommandFormatter(this._settings).GetCommandArguments(new CommandInfo
			{
				ConfigFileName = FileHelper.WriteToTempFile("[]",".json"),
				SessionName = "apimanger",
				Commands = new string[] { "!management commands" }
			});

			var ret = await StartProcess(this._settings.ConsoleExecutableName, args).ConfigureAwait(false);

			return ret.ToString(CultureInfo.InvariantCulture);
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
			return await this.ExecuteAsync(testData, false, true);
		}

		private Task<string> ExecuteAsync(TestData testData, bool isBatch, bool isConsole = false)
		{
			ValidateSettings(this._settings);
			Validate(testData);
			var tcs = new TaskCompletionSource<string>();

			if (!isConsole)
			{
				CheckForConsoleRunningAlready();
			}

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

		public Task<string> ConvertJsonToHtml(string jsonFile, string htmlFile)
		{
			string tempConfigFile = FileHelper.WriteToTempFile("[]", ".json");
			string tempBatchFile = string.Empty;
			var command = "main.py ";
			command += $"--config {tempConfigFile}";
			tempBatchFile = FileHelper.WriteToTempFile($"!convert_json_html {jsonFile} {htmlFile}", ".bat");
			command += $" --batch {tempBatchFile}";
			var tcs = new TaskCompletionSource<string>();

			CheckForConsoleRunningAlready();
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
				FileHelper.DeleteIfExists(tempConfigFile);
				FileHelper.DeleteIfExists(tempBatchFile);
			};

			return tcs.Task;
		}

		private Task<int> StartProcess(string cmd, string args)
		{
			CheckForConsoleRunningAlready();
			var tcs = new TaskCompletionSource<int>();

			var startInfo = new ProcessStartInfo(this._settings.ConsoleExecutableName, args);
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
				var output = process.ExitCode;
				tcs.SetResult(output);
			};

			return tcs.Task;
		}

		private bool CheckForConsoleRunningAlready()
		{
			var consoleProgramName = Path.GetFileNameWithoutExtension(this._settings.ConsoleExecutableName);
			var foundProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == consoleProgramName);
			if (foundProcess != null)
			{
				throw new Exception(
					$"Please close {foundProcess.ProcessName} as currently both console and GUI cannot be run at the same time.");
				return true;
			}
			return false;
		}
	}
}