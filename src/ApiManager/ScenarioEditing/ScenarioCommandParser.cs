using System.Collections.Generic;
using System.IO;
using ApiManager.ScenarioEditing.Models;

namespace ApiManager.ScenarioEditing
{
	class ScenarioCommandParser
	{
		public ScenarioCommandParser(string fileName)
		{
			var apiScenarios = new List<ApiScenarioItem>();
			foreach (var line in File.ReadAllLines(fileName))
			{
				if (new ScenarioParser().Parse(line) is ApiScenarioItem apiScenarioItem)
				{
					apiScenarios.Add(apiScenarioItem);
				}
			}

			this.ApiScenarios = apiScenarios;
		}

		public IEnumerable<ApiScenarioItem> ApiScenarios { get; }
	}
}
