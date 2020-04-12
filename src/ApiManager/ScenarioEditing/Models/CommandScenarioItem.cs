namespace ApiManager.ScenarioEditing.Models
{
	class CommandScenarioItem : ScenarioLineItem
	{
		public CommandScenarioItem(string line) : base("command")
		{
			this.Command = line;
		}

		public string Command { get; }
	}
}
