using System.IO;
using ApiManager.Repository;

namespace ApiManager.Model
{
	class Settings : ISettings
	{
		public string ConsoleExecutableName { get; set; }
		public string WorkingDirectory { get; set; }
		public string ConfigurationPath { get; set; }

		public bool IsPythonExecutable
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.ConsoleExecutableName))
				{
					return false;
				}

				return this.ConsoleExecutableName.ToLower().Contains("python");
			}
		}

		public void MakeAbsolultePaths()
		{
			this.ConsoleExecutableName = Path.GetFullPath(this.ConsoleExecutableName);
			this.WorkingDirectory = Path.GetFullPath(this.WorkingDirectory);
			this.ConfigurationPath = Path.GetFullPath(this.ConfigurationPath);
		}
	}
}