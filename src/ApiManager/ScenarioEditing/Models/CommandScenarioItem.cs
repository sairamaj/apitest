using System.Linq;

namespace ApiManager.ScenarioEditing.Models
{
	class CommandScenarioItem : ScenarioLineItem
	{

		public CommandScenarioItem(string line) : base("command")
		{
			var parts = line.Split();
			this.Command = parts.First();
			this.Arg1 = parts.Length > 1 ? parts[1] : string.Empty;
			this.Arg2 = parts.Length > 2 ? parts[2] : string.Empty;
		}

		public string Command { get; }
		public string Arg1 { get; set; }
		public string Arg2 { get; set; }
	}
}
