using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiManager.Model;

namespace ApiManager.Scenario.ViewModels
{
	class ScenarioEditorViewModel
	{
		public ScenarioEditorViewModel(
			Scenario scenario, 
			IEnumerable<HelpCommand> helpCommands,
			ApiCommandInfo apiCommandInfo)
		{
			this.Commands = File.ReadAllLines(scenario.FileName);
			this.Content = File.ReadAllText(scenario.FileName);
			this.Content += "\r\n";
			this.Content += string.Join(",", helpCommands.Select(h => h.Name).ToArray());
			this.Content += "\r\n";
			this.Content += string.Join(",", apiCommandInfo.ApiCommands.Select(a => a.Key).ToArray());
		}

		IEnumerable<string> Commands { get; set; }
		public string Content { get; set; }
	}
}
