using System;
using System.Linq;

namespace ApiManager.ScenarioEditing.Models
{
	class CommandScenarioItem : ScenarioLineItem
	{
		string _command;
		public CommandScenarioItem(
			string line) 
			: base("command", line)
		{
			this.Parse(line);
		}

		public string Command
		{
			get
			{
				return this._command;
			}
			set
			{
				this._command = value;
				Parse(value);
			}
		}

		public string Name { get; set; }
		public string Arg1 { get; set; }
		public string Arg2 { get; set; }

		public override string GetCommand()
		{
			var cmd = this.IsCommented ? "# " : string.Empty;
			cmd += this.Command;
			return cmd;
		}

		public bool IsAssertCommand
		{
			get
			{
				return this.Name == "!assert";
			}
		}

		private void Parse(string line)
		{
			var parts = line.Split();
			this.Name = parts.First();
			this.Arg1 = parts.Length > 1 ? parts[1] : string.Empty;
			this.Arg2 = parts.Length > 2 ? parts[2] : string.Empty;
		}
	}
}
