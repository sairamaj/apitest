using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.ScenarioEditing;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;
using ApiManager.Extensions;

namespace ApiManager.ViewModels
{
	class ScenarioContainerViewModel : CommandTreeViewModel
	{
		private IEnumerable<string> _apis;
		private readonly ApiInfo _apiInfo;
		private readonly IDataRepository _repository;
		private Action<ScenarioAction, Scenario> _onEvent;

		public ScenarioContainerViewModel(
			TreeViewItemViewModel parent,
			Scenario scenario,
			Action<ScenarioAction, Scenario> onEvent,
			ApiInfo apiInfo,
			IDataRepository repository)
			: base(parent, scenario.Name, scenario.Name)
		{
			this.Scenario = scenario;
			this._apiInfo = apiInfo;
			this._repository = repository;
			this.FileName = scenario.FileName;
			this.Name = scenario.Name;
			this._onEvent = onEvent;

			this.EditCommandFileCommand = new DelegateCommand(async () =>
		   {
			   try
			   {
				   //var helpCommands = await repository.GetHelpCommands().ConfigureAwait(true);
				   //var apiCommands = await repository.GetCommands(apiInfo).ConfigureAwait(true);

				   //var view = new ScenarioEditorView { DataContext = new ScenarioEditorViewModel(scenario, helpCommands, apiCommands) };
				   //view.ShowDialog();
				   Process.Start(scenario.FileName);
			   }
			   catch (Exception e)
			   {
				   MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			   }
		   });

			Action deleteAction = () => this.DeleteScenario();
			this.DeleteCommand = new DelegateCommand(deleteAction.WithErrorMessageBox);
		}

		protected override void LoadChildren()
		{
			this.Load();
		}

		public string Name { get; }
		public string FileName { get; }
		public ICommand EditCommandFileCommand { get; }
		public IEnumerable<string> Apis
		{
			get
			{
				if (_apis == null)
				{
					if (File.Exists(this.FileName))
					{
						this._apis = File.ReadAllLines(this.FileName);
					}
				}

				return this._apis;
			}
		}

		public Scenario Scenario { get; }
		public ICommand DeleteCommand { get; }

		private void Load()
		{
			this.Children.Clear();
			foreach (var child in this.Scenario.Children.Where(s => !s.IsContainer))
			{
				this.Children.Add(new ScenarioViewModel(this, child, (e, s) => this.DoScenarioAction(e, s), this._apiInfo, this._repository));
			}

			// Add containers.
			foreach (var child in this.Scenario.Children.Where(s => s.IsContainer))
			{
				this.Children.Add(new ScenarioContainerViewModel(
					this.Parent,
					child,
					(e, s) => this.DoScenarioAction(e, s),
					this._apiInfo, 
					this._repository));
			}
		}

		internal void AddScenario(Scenario scenario)
		{
			this.Children.Add(new ScenarioViewModel(this, scenario, (e, s) => this.DoScenarioAction(e, s), this._apiInfo, this._repository));
		}

		internal void AddScenarioContainer(Scenario scenarioContainer)
		{
			this.ExpandAll();   // make sure that it expands otherwise child nodes are not added properly.	
			this.Children.Add(new ScenarioContainerViewModel(
				this.Parent,
				scenarioContainer,
				(e, s) => this.DoScenarioAction(e, s),
				this._apiInfo,
				this._repository));
		}

		private void DoScenarioAction(ScenarioAction e, Scenario scenario)
		{
			switch (e)
			{
				case ScenarioAction.Copy:
					this.AddScenario(scenario);
					break;
				case ScenarioAction.Delete:
					if (scenario.IsContainer)
					{
						var childToRemove = this.Children.OfType<ScenarioContainerViewModel>().FirstOrDefault(s => s.Scenario.Name == scenario.Name);
						this.Children.Remove(childToRemove);
					}
					else
					{
						var childToRemove = this.Children.OfType<ScenarioViewModel>().FirstOrDefault(s => s.Scenario.Name == scenario.Name);
						this.Children.Remove(childToRemove);
					}
					break;
			}
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
	}
}
