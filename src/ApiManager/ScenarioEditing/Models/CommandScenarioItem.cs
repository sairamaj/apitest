using System;
using System.Linq;

namespace ApiManager.ScenarioEditing.Models
{
	class CommandScenarioItem : ScenarioLineItem
	{
		public CommandScenarioItem(
			string line) 
			: base("command", line)
		{
			var parts = line.Split();
			this.Command = parts.First();
			this.Arg1 = parts.Length > 1 ? parts[1] : string.Empty;
			this.Arg2 = parts.Length > 2 ? parts[2] : string.Empty;
		}

		public string Command { get; private set; }
		public string Arg1 { get; set; }
		public string Arg2 { get; set; }

		public override string GetCommand()
		{
			var cmd = this.Command;
			if (!string.IsNullOrWhiteSpace(this.Arg1))
			{
				cmd += " " + this.Arg1;
			}

			if (!string.IsNullOrWhiteSpace(this.Arg2))
			{
				cmd += " " + this.Arg2;
			}

			return cmd;
		}
	}
}
