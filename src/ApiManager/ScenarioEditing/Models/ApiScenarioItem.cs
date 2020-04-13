namespace ApiManager.ScenarioEditing.Models
{
	class ApiScenarioItem : ScenarioLineItem
	{
		private string _command;
		public ApiScenarioItem(string command) : base("api")
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
				this.EditingModeOn = false;
				OnPropertyChanged(() => this.Command);
				OnPropertyChanged(() => this.EditingModeOn);

			}
		}
		public string[] Apis
		{
			get
			{
				return new string[] { "accesstoken.password", "developers", "apis" };
			}
		}

	}
}
