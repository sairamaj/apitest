using System.Collections.Generic;

namespace ApiManager.ScenarioEditing.Models
{
	class ApiScenarioItem : ScenarioLineItem
	{
		private string _command;
		public ApiScenarioItem(string command, IEnumerable<string> apis) : base("api")
		{
			this.Command = command;
			Apis = apis;
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
				this.EditingModeOn = false;
				OnPropertyChanged(() => this.Command);
				OnPropertyChanged(() => this.EditingModeOn);

			}
		}

		public IEnumerable<string> Apis { get; }

	}
}
