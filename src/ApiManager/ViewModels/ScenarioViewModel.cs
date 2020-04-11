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

namespace ApiManager.ViewModels
{
	class ScenarioViewModel : CommandTreeViewModel
	{
		private IEnumerable<string> _apis;
		private IDataRepository _repository;
		private ApiInfo _apiInfo;
		private Action<ScenarioAction, Scenario> _onEvent;
		public ScenarioViewModel(
			CommandTreeViewModel parent,
			Scenario scenario,
			Action<ScenarioAction, Scenario> onEvent,
			ApiInfo apiInfo,
			IDataRepository repository)
			: base(parent, scenario.Name, scenario.Name)
		{
			this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this._onEvent = onEvent ?? throw new ArgumentNullException(nameof(onEvent));
			this._apiInfo = apiInfo;

			this.Scenario = scenario;
			this.FileName = scenario.FileName;
			this.Name = scenario.Name;
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

			this.CopyCommand = new DelegateCommand(() => this.CopyScenario());
			this.DeleteCommand = new DelegateCommand(() => this.DeleteScenario());
			this.IsExpanded = true;
		}

		public string Name { get; }
		public string FileName { get; }
		public ICommand EditCommandFileCommand { get; }
		public ICommand CopyCommand { get; }
		public ICommand DeleteCommand { get; }

		public Scenario Scenario { get; }
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

		protected override void LoadChildren()
		{
			foreach (var container in this.Scenario.Children.Where(s => s.IsContainer))
			{
				this.Children.Add(new ScenarioContainerViewModel(container, this._apiInfo, this._repository));
			}
		}

		private void CopyScenario()
		{
			var copiedScenario = ScenarioEditingHelper.CopyScenario(this.Scenario);
			if (copiedScenario == null)
			{
				return;
			}
			this._onEvent(ScenarioAction.Copy,copiedScenario);
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
