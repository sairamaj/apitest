namespace ApiManager.Model
{
	class EnvironmentInfo
	{
		public EnvironmentInfo(string name, string configuration, string variableFileName, string commandFileName)
		{
			this.Name = name;
			this.Configuration = configuration;
			this.VariableFileName = variableFileName;
			this.CommandFileName = commandFileName;
		}

		public string Name { get; }
		public string Configuration { get; }
		public string VariableFileName { get; }
		public string CommandFileName { get; }
	}
}
