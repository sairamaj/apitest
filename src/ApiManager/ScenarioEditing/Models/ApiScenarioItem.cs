namespace ApiManager.ScenarioEditing.Models
{
	class ApiScenarioItem : ScenarioLineItem
	{
		public ApiScenarioItem(string command) : base("api")
		{
			this.Command = command;
		}

		public string Command { get; }
	}
}
