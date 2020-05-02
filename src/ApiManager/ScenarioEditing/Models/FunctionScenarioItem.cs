using System.Collections.Generic;

namespace ApiManager.ScenarioEditing.Models
{
	class FunctionScenarioItem : ScenarioLineItem
	{
		private string _command;
		public FunctionScenarioItem(string command) : base("function", command)
		{
			this.Command = command;
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
			}
		}

		public IEnumerable<string> Apis { get; }

		public override string GetCommand()
		{
			return this.Command;
		}
	}
}
