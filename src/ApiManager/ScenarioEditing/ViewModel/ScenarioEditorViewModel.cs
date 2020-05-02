using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;

namespace ApiManager.ScenarioEditing.ViewModels
{
	class ScenarioEditorViewModel
	{
		public ScenarioEditorViewModel(Scenario scenario, IEnumerable<string> apis)
		{
			var lines = File.ReadAllLines(scenario.FileName);
			this.ScenarioLineItems = new SafeObservableCollection<ScenarioLineItemViewModel>();

			// todo: move to parser.
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
				else if (line.StartsWith("__", System.StringComparison.OrdinalIgnoreCase))
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new FunctionScenarioItem(line)));
				}
				else
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new ApiScenarioItem(line.Split().First(), apis)));
				}
			}

			this.SaveCommandFileCommand = new DelegateCommand(() =>
		   {
			   this.Save();
		   });

			this.ScenarioEditTitle = $"{scenario.Name} ({scenario.FileName})";
		}

		public string ScenarioEditTitle { get; }
		public ObservableCollection<ScenarioLineItemViewModel> ScenarioLineItems { get; }
		public ICommand SaveCommandFileCommand { get; }
		private void Save()
		{
			var builder = new StringBuilder();
			foreach (var item in ScenarioLineItems)
			{
				builder.AppendLine(item.LineItem.GetCommand());
			}

			var file = @"c:\temp\foo.txt";
			File.WriteAllText(file, builder.ToString());
			MessageBox.Show($"{file} has been saved.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
