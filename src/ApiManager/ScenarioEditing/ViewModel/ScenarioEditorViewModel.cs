using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.NewLineItem.ViewModels;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.ViewModels
{
	class ScenarioEditorViewModel
	{
		public ScenarioEditorViewModel(
			Scenario scenario, 
			BangCommandInfo bangCommandInfo,
			ApiCommandInfo apiCommandInfo,
			FunctionCommandInfo functionCommandInfo,
			DynamicVariableInfo dynamicVariableInfo)
		{
			var lines = File.ReadAllLines(scenario.FileName);
			this.ScenarioLineItems = new SafeObservableCollection<ScenarioLineItemViewModel>();

			// todo: move to parser.
			foreach (var line in lines)
			{
				if (line.StartsWith("#", System.StringComparison.OrdinalIgnoreCase))
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new CommentScenarioItem(line), OnEditAction));
				}
				else if (line.Trim().Length == 0)
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new LineBreakScenarioItem(), OnEditAction));
				}
				else if (line.StartsWith("!", System.StringComparison.OrdinalIgnoreCase))
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new CommandScenarioItem(line), OnEditAction));
				}
				else if (line.StartsWith("__", System.StringComparison.OrdinalIgnoreCase))
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(new FunctionScenarioItem(line), OnEditAction));
				}
				else
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(
						new ApiScenarioItem(line.Split().First()), OnEditAction));
				}
			}

			this.SaveCommandFileCommand = new DelegateCommand(() =>
		   {
			   this.Save();
		   });

			this.ScenarioEditTitle = $"{scenario.Name} ({scenario.FileName})";
			this.RootCommands = new List<CommandTreeViewModel>()
			{
				new ApiInfoContainerViewModel(apiCommandInfo),
				new BangContainerCommandInfoViewModel(bangCommandInfo),
				new FunctionContainerCommandInfoViewModel(functionCommandInfo),
				new DynamicVariableContainerInfoViewModel(dynamicVariableInfo),
			};

		}

		public IEnumerable<CommandTreeViewModel> RootCommands { get; }
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

		private void OnEditAction(ScenarioEditingAction action, ScenarioLineItemViewModel item)
		{
			switch (action)
			{
				case ScenarioEditingAction.Delete:
					this.ScenarioLineItems.Remove(item);
					break;
				case ScenarioEditingAction.MoveUp:
					var index = this.ScenarioLineItems.IndexOf(item);
					if (index <= 0)
					{
						return;	// already top
					}
					this.ScenarioLineItems.Remove(item);
					this.ScenarioLineItems.Insert(index - 1, item);
					break;
				case ScenarioEditingAction.MoveDown:
					var index1 = this.ScenarioLineItems.IndexOf(item);
					if (index1 >= this.ScenarioLineItems.Count()-1)
					{
						return; // already bootom
					}
					this.ScenarioLineItems.Remove(item);
					this.ScenarioLineItems.Insert(index1 + 1, item);
					break;
			}
		}
	}
}
