namespace ApiManager.Repository
{
	interface ISettings 
	{
		string ConsoleExecutableName { get; }
		string WorkingDirectory { get; }
		string ConfigurationPath { get; }
		bool IsPythonExecutable { get; }
	}
}
