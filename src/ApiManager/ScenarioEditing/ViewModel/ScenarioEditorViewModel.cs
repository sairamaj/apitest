﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.ScenarioEditing.CommandEditing.ViewModel;
using ApiManager.ScenarioEditing.CommandEditing.Views;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.NewLineItem.ViewModels;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.ViewModels
{
	class ScenarioEditorViewModel : CoreViewModel
	{
		private bool _isDirty;
		public ScenarioEditorViewModel(
			Window win,
			Scenario scenario,
			BangCommandInfo bangCommandInfo,
			ApiCommandInfo apiCommandInfo,
			FunctionCommandInfo functionCommandInfo,
			DynamicVariableInfo dynamicVariableInfo)
		{
			this.Scenario = scenario;
			this.ScenarioLineItems = new SafeObservableCollection<ScenarioLineItemViewModel>();
			this.Refresh();
			this.SaveCommandFileCommand = new DelegateCommand(() =>
		   {
			   this.Save(this.Scenario.FileName);
		   });

			this.EditCommandFileCommand = new DelegateCommand(() =>
		   {
			   Process.Start("notepad.exe", this.Scenario.FileName);
		   });

			this.RefreshCommandCommand = new DelegateCommand(() =>
		   {
			   this.Refresh();
		   });

			win.Closing += OnClosing;
			this.ScenarioEditTitle = $"{scenario.Name} ({scenario.FileName})";
			this.RootCommands = new List<CommandTreeViewModel>()
			{
				new ApiInfoContainerViewModel(apiCommandInfo),
				new BangContainerCommandInfoViewModel(bangCommandInfo),
				new FunctionContainerCommandInfoViewModel(functionCommandInfo),
				new DynamicVariableContainerInfoViewModel(dynamicVariableInfo),
			};

			this.ScenarioLineItems.CollectionChanged += (s, e) =>
			{
				OnScenarioDrop(s, e);
			};
		}

		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this._isDirty)
			{
				var ret = MessageBox.Show(
					"Do you want save before quiting", 
					"Save", 
					MessageBoxButton.YesNoCancel, 
					MessageBoxImage.Question);
				if (ret == MessageBoxResult.Yes)
				{
					this.Save(this.Scenario.FileName);
				}
				else if (ret == MessageBoxResult.Cancel)
				{
					e.Cancel = true;
				}
			}
		}

		public IEnumerable<CommandTreeViewModel> RootCommands { get; }
		public string ScenarioEditTitle { get; }
		public ObservableCollection<ScenarioLineItemViewModel> ScenarioLineItems { get; }
		public ICommand SaveCommandFileCommand { get; }
		public ICommand EditCommandFileCommand { get; }
		public ICommand RefreshCommandCommand { get; }
		public Scenario Scenario { get; }

		private void Save(string fileName)
		{
			var builder = new StringBuilder();
			foreach (var item in ScenarioLineItems)
			{
				builder.AppendLine(item.LineItem.GetCommand());
			}

			File.WriteAllText(fileName, builder.ToString());
			MessageBox.Show($"{fileName} has been saved.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
			this._isDirty = false;
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
						return; // already top
					}
					this.ScenarioLineItems.Remove(item);
					this.ScenarioLineItems.Insert(index - 1, item);
					break;
				case ScenarioEditingAction.MoveDown:
					var index1 = this.ScenarioLineItems.IndexOf(item);
					if (index1 >= this.ScenarioLineItems.Count - 1)
					{
						return; // already bootom
					}
					this.ScenarioLineItems.Remove(item);
					this.ScenarioLineItems.Insert(index1 + 1, item);
					break;
			}
		}

		private bool EditCommand(ScenarioLineItemViewModel lineItemViewModel)
		{
			if (lineItemViewModel.LineItem is ApiScenarioItem apiScenarioItem)
			{
				var win = new EditApiCommandWindow();
				var viewModel = new EditApiCommandViewModel(win, apiScenarioItem, ServiceLocator.Locator.Resolve<IResourceManager>());
				win.DataContext = viewModel;
				return win.ShowDialog().Value;
			}

			if (lineItemViewModel.LineItem is CommandScenarioItem bangCommandScenarioItem)
			{
				if (bangCommandScenarioItem.CommandType == BangCommandType.Assert)
				{
					var win = new EditAssertCommandWindow();
					var vm = new EditAssertCommandViewModel(win, bangCommandScenarioItem);
					win.DataContext = vm;
					return win.ShowDialog().Value;
				}
				if (bangCommandScenarioItem.CommandType == BangCommandType.Extract)
				{
					var win = new EditExtractCommandWindow();
					var vm = new EditExtractCommandViewModel(win, bangCommandScenarioItem);
					win.DataContext = vm;
					return win.ShowDialog().Value;
				}
			}

			return true;
		}

		private void OnScenarioDrop(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				if (e.NewItems.Count == 0)
				{
					return;
				}
				if (e.NewItems[0] is ScenarioLineItemViewModel scenarioLineItemViewModel)
				{
					// Edit only if it is coming from dragging from the command palette.
					if (scenarioLineItemViewModel.IsDraggedAsNewItem)
					{
						if (!EditCommand(scenarioLineItemViewModel))
						{
							// delete in background thread as we are already in collection notifications.
							Task.Run(() =>
							{
								Thread.Sleep(10);
								this.ScenarioLineItems.Remove(scenarioLineItemViewModel);
							});
						}
						else
						{
							scenarioLineItemViewModel.IsDraggedAsNewItem = false;
							scenarioLineItemViewModel.AttachEditAction(this.OnEditAction);
							this._isDirty = true;
						}
					}
				}
			}
		}

		private new void Refresh()
		{
			var lines = File.ReadAllLines(this.Scenario.FileName);
			this.ScenarioLineItems.Clear();

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
					this.ScenarioLineItems.Add(new ScenarioApiCommandLineItemViewModel(
						new ApiScenarioItem(line), OnEditAction));
				}
			}
		}
	}
}

