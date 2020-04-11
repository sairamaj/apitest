using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.ScenarioEditing;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ScenarioContainerViewModel : ScenarioBaseViewModel
	{
		private IEnumerable<string> _apis;
		private readonly ApiInfo _apiInfo;
		private readonly IDataRepository _repository;

		public ScenarioContainerViewModel(
			TreeViewItemViewModel parent,
			Scenario scenario,
			Action<ScenarioAction, Scenario> onEvent,
			ApiInfo apiInfo,
			IDataRepository repository)
			: base(parent, scenario, onEvent)
		{
			this._apiInfo = apiInfo;
			this._repository = repository;
			this.FileName = scenario.FileName;
			this.Name = scenario.Name;

			this.NewScenarioCommand = new DelegateCommand(this.CreateNewScenario);
			this.NewScenarioFolderCommand = new DelegateCommand(this.CreateNewScenarioFolder);
			
		}

		protected override void LoadChildren()
		{
			this.Load();
		}

		public string Name { get; }
		public string FileName { get; }
		public ICommand EditCommandFileCommand { get; }
		public ICommand NewScenarioCommand { get; }
		public ICommand NewScenarioFolderCommand { get; }

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

		private void CreateNewScenario()
		{
			var newScenario = ScenarioEditingHelper.CreateNewScenario(this.Scenario.ContainerPath);
			if (newScenario != null)
			{
				this.AddScenario(newScenario);
			}
		}

		private void CreateNewScenarioFolder()
		{
			var newScenario = ScenarioEditingHelper.CreateNewScenarioContainer(this.Scenario.ContainerPath);
			if (newScenario != null)
			{
				this.AddScenarioContainer(newScenario);
			}
		}
	}
}
