namespace ApiManager.Model
{
	class EnvironmentInfo
	{
		public EnvironmentInfo(string name, string configName)
		{
			this.Name = name;
			this.ConfigName = configName;
		}

		public string Name { get; }
		public string ConfigName { get; }
	}
}
