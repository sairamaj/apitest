namespace ApiManager.Model
{
	class ApiExecuteInfo : Info
	{
		public ApiExecuteInfo(string session, string enviornment, Scenario scenario)
		{
			this.Type = "ApiExecute";
			this.Session = session;
			this.Scenario = scenario;
		}

		public Scenario Scenario { get; set; }
	}
}
