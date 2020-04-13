using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ApiManager.Model;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core;

namespace ApiManager.ScenarioEditing.ViewModels
{
	class ScenarioEditorViewModel
	{
		public ScenarioEditorViewModel(Scenario scenario)
		{
			var lines = File.ReadAllLines(scenario.FileName);
			this.ScenarioLineItems = new SafeObservableCollection<ScenarioLineItemViewModel>();

			foreach (var line in lines)
			{
				if (line.StartsWith("#", System.StringComparison.OrdinalIgnoreCase))
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new CommentScenarioItem(line)));
				}
				else if (line.Trim().Length == 0)
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new LineBreakScenarioItem()));
				}
				else if (line.StartsWith("!", System.StringComparison.OrdinalIgnoreCase))
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new CommandScenarioItem(line)));
				}
				else
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new ApiScenarioItem(line.Split().First())));
				}
			}
		}

		public ObservableCollection<ScenarioLineItemViewModel> ScenarioLineItems { get; }
	}
}
