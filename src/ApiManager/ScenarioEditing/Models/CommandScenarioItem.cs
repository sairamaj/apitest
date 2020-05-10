using System;
using System.Linq;
using ApiManager.Model;

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
			this._command = line;
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
				OnPropertyChanged(() => this.Command);
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

		public BangCommandType CommandType
		{
			get
			{
				switch (this.Name)
				{
					case "!assert":
						return BangCommandType.Assert;
					case "!extract":
						return BangCommandType.Extract;
					case "!print":
						return BangCommandType.Print;
					default:
						return BangCommandType.Unknown;
				}
			}
		}

		private void Parse(string line)
		{
			var parts = line.Split();
			this.Name = parts.First();
			this.Arg1 = parts.Length > 1 ? parts[1] : string.Empty;
			this.Arg2 = parts.Length > 2 ? string.Join(" ", parts.Skip(2).ToArray()) : string.Empty;

			OnPropertyChanged(() => this.Name);
			OnPropertyChanged(() => this.Arg1);
			OnPropertyChanged(() => this.Arg2);
		}
	}
}
