namespace ApiManager.Repository
{
	interface ISettings
	{
		string ConsoleExecutableName { get; }
		string WorkingDirectory { get; }
		string ConfigurationPath { get; }
		string ResourcesPath { get; set; }
		bool IsPythonExecutable { get; }
	}
}
