namespace ApiManager.Model
{
	class BangCommandInfo
	{
		public BangCommandInfo(string name, string help)
		{
			Name = name;
			Help = help;
		}

		public string Name { get; }
		public string Help { get; }
	}
}
