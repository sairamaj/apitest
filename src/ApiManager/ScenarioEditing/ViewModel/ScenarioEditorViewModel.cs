using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
			this.BangCommandInfo = bangCommandInfo;
			this.ApiCommandInfo = apiCommandInfo;
			FunctionCommandInfo = functionCommandInfo;
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
		public BangCommandInfo BangCommandInfo { get; }
		public ApiCommandInfo ApiCommandInfo { get; }
		public FunctionCommandInfo FunctionCommandInfo { get; }

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
					this._isDirty = true;
					break;
				case ScenarioEditingAction.Edit:
					if (EditCommand(item))
					{
						this._isDirty = true;
					}
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

				if (bangCommandScenarioItem.CommandType == BangCommandType.Print)
				{
					var win = new EditPrintCommandWindow();
					var vm = new EditPrintCommandViewModel(win, bangCommandScenarioItem);
					win.DataContext = vm;
					win.ShowDialog();
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
				var scenarioItem = new ScenarioParser().Parse(line);
				if (scenarioItem is ApiScenarioItem apiScenarioItem)
				{
					this.ScenarioLineItems.Add(new ScenarioApiCommandLineItemViewModel(apiScenarioItem, OnEditAction));
					var commandNames = this.ApiCommandInfo.ApiCommands.SelectMany(api => api.Routes.Select(r =>
					{
						return r.IsDefault ? $"{api.Name}" : $"{api.Name}.{r.Name}";
					}));

					if (!scenarioItem.IsCommented)
					{
						apiScenarioItem.IsError = !commandNames.Contains(apiScenarioItem.ApiName);
					}
				}
				else
				{
					this.ScenarioLineItems.Add(new ScenarioLineItemViewModel(scenarioItem, OnEditAction));
				}
			}
		}
	}
}

