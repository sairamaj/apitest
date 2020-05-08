using System;
using System.Collections.Generic;

namespace ApiManager.ScenarioEditing.Models
{
	class ApiScenarioItem : ScenarioLineItem
	{
		public ApiScenarioItem(string command) : base("api", command)
		{
			this.Command = command;
			this.Parse(command);
		}

		public string Command { get; set; }
		public string Method { get; set; }
		public string PayloadFileName { get; set; }

		public IEnumerable<string> Apis { get; }

		public override string GetCommand()
		{
			return this.Command;
		}

		public override void ToggleComment()
		{
		}

		private void Parse(string command)
		{
			var parts = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			this.Command = parts[0];
			this.Method = parts.Length > 1 ? parts[1]: null;
			this.PayloadFileName = parts.Length > 2 ? parts[2] : null;
		}
	}
}
