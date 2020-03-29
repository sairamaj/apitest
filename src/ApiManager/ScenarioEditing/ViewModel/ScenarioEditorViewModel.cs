using System.Collections.Generic;
using System.IO;
using ApiManager.Model;

namespace ApiManager.ScenarioEditing.ViewModels
{
	class ScenarioEditorViewModel
	{
		public ScenarioEditorViewModel(Scenario scenario)
		{
			this.Commands = File.ReadAllLines(scenario.FileName);
			this.Content = File.ReadAllText(scenario.FileName);
		}

		IEnumerable<string> Commands { get; set; }
		public string Content { get; set; }
	}
}
