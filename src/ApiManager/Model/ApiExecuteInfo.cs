using System.IO;

namespace ApiManager.Model
{
	class ApiExecuteInfo : Info
	{
		public ApiExecuteInfo(string session, Environment environment, Scenario scenario)
		{
			this.Type = "ApiExecute";
			this.Session = session;
			this.Environment = environment;
			this.Scenario = scenario;
			this.ScenarioContent = File.ReadAllText(scenario.FileName);
		}

		public Environment Environment { get; }
		public Scenario Scenario { get; }
		public string ScenarioContent { get; set; }
	}
}
