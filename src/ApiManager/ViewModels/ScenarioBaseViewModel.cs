﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.ScenarioEditing;
using ApiManager.ScenarioEditing.ViewModel;
using ApiManager.ScenarioEditing.ViewModels;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ScenarioBaseViewModel : CommandTreeViewModel
	{
		protected Action<ScenarioAction, Scenario> _onEvent;
		private IEnumerable<ScenarioResourceViewModel> _scenarioResources;
		public ScenarioBaseViewModel(
			TreeViewItemViewModel parent,
			ApiInfo apiInfo,
			Scenario scenario,
			Action<ScenarioAction, Scenario> onEvent)
			: base(parent, scenario.Name, scenario.FileName)
		{
			this.ApiInfo = apiInfo;
			this.Scenario = scenario;
			this._onEvent = onEvent;
			this.TestStatus = ScenarioTestStatus.None;

			this.RelvealInExplorerCommand = new DelegateCommand(() =>
			{
				Process.Start(this.Scenario.ContainerPath);
			});


			this.DeleteCommand = new DelegateCommand(
				() => UiHelper.SafeAction(this.DeleteScenario, "Delete Scenario"));
			this.SmartEditorCommand = new DelegateCommand(
				async () => await this.ShowSmartEditor().ConfigureAwait(false));

		}

		public ScenarioTestStatus TestStatus { get; private set; }

		public ApiInfo ApiInfo { get; }
		public Scenario Scenario { get; }
		public ICommand RelvealInExplorerCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand SmartEditorCommand { get; }
		public IEnumerable<ScenarioResourceViewModel> Resources
		{
			get
			{
				if (this._scenarioResources == null)
				{
					this._scenarioResources = (new ScenarioCommandParser(this.Scenario.FileName)
						.ApiScenarios
						.Where(s =>
						{
							return new string[] { "post", "put", "patch" }.Contains(s.Method);
						})
						.Select(s => new ScenarioResourceViewModel(s))).ToList();
				}

				return this._scenarioResources;
			}
		}

		public int ResourcesCount => this.Resources.Count();

		public void UpdateStatus(ScenarioTestStatus status)
		{
			this.TestStatus = status;
			OnPropertyChanged(() => this.TestStatus);
		}

		private void DeleteScenario()
		{
			var deletedScenario = ScenarioEditingHelper.DeleteScenario(this.Scenario);
			if (!deletedScenario)
			{
				return;
			}
			this._onEvent(ScenarioAction.Delete, this.Scenario);
		}

		protected async Task ShowSmartEditor()
		{
			try
			{
				var repository = ServiceLocator.Locator.Resolve<IDataRepository>();
				var apiCommandInfo = await ServiceLocator.Locator.Resolve<IDataRepository>().GetCommands(this.ApiInfo).ConfigureAwait(true);
				var bangCommandInfo = await ServiceLocator.Locator.Resolve<IDataRepository>().GetBangCommands().ConfigureAwait(true);
				var functionCommandInfo = await ServiceLocator.Locator.Resolve<IDataRepository>().GetFunctionCommandInfo().ConfigureAwait(true);
				var dynamicVariablesInfo = await ServiceLocator.Locator.Resolve<IDataRepository>().GetDynamicVariableInfo().ConfigureAwait(true);
				EditorWindow editorWindow = new EditorWindow();
				var apis = apiCommandInfo.ApiCommands;

				editorWindow.DataContext = new ScenarioEditorViewModel(
					editorWindow,
					this.Scenario,
					bangCommandInfo,
					apiCommandInfo,
					functionCommandInfo,
					dynamicVariablesInfo);

				//editorWindow.DataContext = new ScenarioEditorViewModel(this.Scenario, items);
				editorWindow.ShowDialog();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}
	}
}
