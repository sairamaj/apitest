﻿using System;
using System.Linq;
using ApiManager.Model;

namespace ApiManager.Repository
{
	class CommandFormatter
	{
		private Settings _settings;

		public CommandFormatter(Settings settings)
		{
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		public string GetCommandArguments(CommandInfo cmdInfo)
		{
			var commandProgram = this._settings.ConsoleExecutableName;

			var command = string.Empty;
			if (this._settings.IsPythonExecutable)
			{
				command += "main.py ";
			}

			command += $"--config {cmdInfo.ConfigFileName}";

			if (!string.IsNullOrEmpty(cmdInfo.SessionName))
			{
				command += $" --session {cmdInfo.SessionName}";
			}

			if (!string.IsNullOrEmpty(cmdInfo.BatchFileName))
			{
				// Execute batch file name.
				command += $" --batch {cmdInfo.BatchFileName}";
			}
			else if (cmdInfo.Commands.Any())
			{
				// Generate batch file with commands.
				var commandsData = string.Join("\r\n", cmdInfo.Commands);
				if (cmdInfo.IsDebug)
				{
					commandsData += "\r\n!waitforuserinput";
				}

				var tempBatchFileName = FileHelper.WriteToTempFile(commandsData, ".bat");
				command += $" --batch {tempBatchFileName}";
			}

			return command;
		}
	}
}
