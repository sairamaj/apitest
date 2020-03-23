namespace ApiManager.Model
{
	class Settings
	{
		public string ConsoleExecutableName { get; set; }
		public string WorkingDirectory { get; set; }

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
	}
}
